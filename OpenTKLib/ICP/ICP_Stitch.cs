
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Windows.Media;
using System.Diagnostics;
using OpenTK;
using OpenTKExtension;

namespace ICPLib
{
  
   
    public partial class IterativeClosestPointTransform 
    {
        
     
       
        
        private bool CheckExitOnIterations()
        {
            this.NumberOfIterations++;
            System.Diagnostics.Debug.WriteLine("Iteration: " + this.NumberOfIterations);
            if (this.NumberOfIterations >= ICPSettings.MaximumNumberOfIterations)
            {
                return true;
            }
            return false;
        }



        private void SetStartPoints(ref PointCloud points1, ref PointCloud points2, PointCloud pointsInput1, PointCloud pointsInput2)
        {

            List<int> randomIndices = RandomUtils.UniqueRandomIndices(3, pointsInput1.Count);


            points1 = RandomUtils.ExtractPoints(pointsInput1, randomIndices);
            points2 = RandomUtils.ExtractPoints(pointsInput2, randomIndices);

        }


        private static Matrix4 TryoutNewPoint(int iPoint, PointCloud pointsTarget, PointCloud pointsSource, PointCloud pointsTargetTrial, PointCloud pointsSourceTrial, LandmarkTransform myLandmarkTransform)
        {

            Vector3 p1 = pointsTarget.Vectors[iPoint];
            Vector3 p2 = pointsSource.Vectors[iPoint];

           
            pointsTargetTrial.AddVector(p1);
            pointsSourceTrial.AddVector(p2);



            MathUtilsVTK.FindTransformationMatrix(pointsSourceTrial, pointsTargetTrial, myLandmarkTransform);//, accumulate);
     
            Matrix4 myMatrix = myLandmarkTransform.Matrix;
          

            return myMatrix;
        }
        public static Matrix4 TryoutPoints(PointCloud pointsTarget, PointCloud pointsSource, ICPSolution res, LandmarkTransform myLandmarkTransform)
        {
            res.PointsTarget = RandomUtils.ExtractPoints(pointsTarget, res.RandomIndices);
            res.PointsSource = RandomUtils.ExtractPoints(pointsSource, res.RandomIndices);

            //transform:
            MathUtilsVTK.FindTransformationMatrix(res.PointsSource, res.PointsTarget, myLandmarkTransform);//, accumulate);

            res.Matrix = myLandmarkTransform.Matrix;

            return res.Matrix;

        }
        private static ICPSolution IterateStartPoints(PointCloud pointsSource, PointCloud pointsTarget, int myNumberPoints, LandmarkTransform myLandmarkTransform, int maxNumberOfIterations)
        {
            int maxIterationPoints = pointsSource.Count;
            int currentIteration = 0;
            try
            {
                if (myNumberPoints > pointsSource.Count)
                    myNumberPoints = pointsSource.Count;

                List<ICPSolution> solutionList = new List<ICPSolution>();

                for (currentIteration = 0; currentIteration < maxNumberOfIterations; currentIteration++)
                {

                    ICPSolution res = ICPSolution.SetRandomIndices(myNumberPoints, maxIterationPoints, solutionList);


                    res.Matrix = TryoutPoints(pointsTarget, pointsSource, res, myLandmarkTransform);//, accumulate);
                    res.PointsTransformed = MathUtilsVTK.TransformPoints(res.PointsSource, res.Matrix);

                    res.MeanDistance = PointCloud.MeanDistance(res.PointsTarget, res.PointsTransformed);
                    //res.MeanDistance = totaldist / Convert.ToSingle(res.PointsSource.Count);
                  
                    solutionList.Add(res);

                  
                }


                if (solutionList.Count > 0)
                {
                    solutionList.Sort(new ICPSolutionComparer());
                    RemoveSolutionIfMatrixContainsNaN(solutionList);
                    if(solutionList.Count == 0)
                        System.Windows.Forms.MessageBox.Show("No start solution could be found !");


                    Debug.WriteLine("Solutions found after: " + currentIteration.ToString() + " iterations, number of solution " + solutionList.Count.ToString());

                    if (solutionList.Count > 0)
                    {
                        ICPSolution result = solutionList[0];
                        //write solution to debug ouput
                        //System.Diagnostics.Debug.WriteLine("Solution of start sequence is: ");
                        DebugWriteUtils.WriteTestOutputVector3("Solution of start sequence", result.Matrix, result.PointsSource, result.PointsTransformed, result.PointsTarget);
                        return result;
                   
                    }

                }
                return null;
            }
            catch (Exception err)
            {
                System.Windows.Forms.MessageBox.Show("Error in IterateStartPoints of ICP at: " + currentIteration.ToString() + " : " + err.Message);
                return null;
            }


        }
        private static bool CheckIfMatrixIsOK(Matrix4 myMatrix)
        {
            //ContainsNaN
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (float.IsNaN( myMatrix[i, 0]  ))
                        return false;

                }
            }
            return true;

        }
        private static void RemoveSolutionIfMatrixContainsNaN(List<ICPSolution> solutionList)
        {
            int iTotal = 0;
            for (int i = solutionList.Count - 1; i >= 0; i--)
            {
                if (!CheckIfMatrixIsOK(solutionList[i].Matrix))
                {
                    iTotal++;
                    
                    solutionList.RemoveAt(i);
                }
            }
           // Debug.WriteLine("-->Removed a total of: " + iTotal.ToString() + " solutions - because invalid matrixes");
        }
        /// <summary>
        /// calculates a start solution set in total of "myNumberPoints" points
        /// </summary>
        /// <param name="pointsTargetSubset"></param>
        /// <param name="pointsSourceSubset"></param>
        /// <returns></returns>
        private static ICPSolution CalculateStartSolution(ref  PointCloud pointsSourceSubset, ref PointCloud pointsTargetSubset, int myNumberPoints,
            LandmarkTransform myLandmarkTranform, PointCloud pointsTarget, PointCloud pointsSource, int maxNumberOfIterations)
        {
            try
            {
                if (CheckSourceTarget(pointsTarget, pointsSource))
                    return null;
                pointsTargetSubset = PointCloud.CloneAll(pointsTarget);
                pointsSourceSubset = PointCloud.CloneAll(pointsSource);

                ICPSolution res = IterateStartPoints(pointsSourceSubset, pointsTargetSubset, myNumberPoints, myLandmarkTranform, maxNumberOfIterations);
                if (res == null)
                {
                    System.Windows.Forms.MessageBox.Show("Could not find starting points for ICP Iteration - bad matching");
                    return null;
                }
                PointCloud.RemoveEntriesByIndices(ref pointsSourceSubset, ref pointsTargetSubset, res.RandomIndices);

                return res;
            }
            catch (Exception err)
            {
                System.Windows.Forms.MessageBox.Show("Error in CalculateStartSolution of ICP: " + err.Message);
                return null;
            }
        }
  
        private float CheckNewPointDistance(int iPoint, Matrix4 myMatrix, PointCloud pointsTarget, PointCloud pointsSource)
        {
            Vector3 p1 = pointsTarget.Vectors[iPoint];
            Vector3 p2 = pointsSource.Vectors[iPoint];
            PointCloud tempPointReference = new PointCloud();
            PointCloud tempPointToBeMatched = new PointCloud();

            tempPointReference.AddVector(p1);
            tempPointToBeMatched.AddVector(p2);

            PointCloud tempPointRotate = MathUtilsVTK.TransformPoints(tempPointToBeMatched, myMatrix);
            float dist = PointCloud.MeanDistance(tempPointReference, tempPointRotate);
            return dist;

        }
        public PointCloud PerformICP_Stitching()
        {
            int iPoint = 0;
            try
            {
               
                PointCloud pointsTarget = null;
                PointCloud pointsSource = null;

                ICPSolution res = CalculateStartSolution(ref  pointsSource, ref pointsTarget, ICPSettings.NumberOfStartTrialPoints, this.LandmarkTransform, this.PTarget, this.PSource, ICPSettings.MaximumNumberOfIterations);
                if (res == null)
                    return null;

                Matrix4 myMatrix = res.Matrix;
                
               

                float oldMeanDistance = 0;
                //now try all points and check if outlier
                for (iPoint = (pointsTarget.Count - 1); iPoint >= 0; iPoint--)
                {
                    float distanceOfNewPoint = CheckNewPointDistance(iPoint, myMatrix, pointsTarget, pointsSource);

                    ////experimental

                    ////--compare this distance to:
                    //pointsTargetTrial.Add[pointsTargetTrial.Count, p1[0], p1[1], p1[2]);
                    //pointsSourceTrial.Add[pointsSourceTrial.Count, p2[0], p2[1], p2[2]);
                    //PointCloud tempPointRotateAll = TransformPoints(pointsSourceTrial, myMatrix, pointsSourceTrial.Count);


                    //dist = CalculateTotalDistance(pointsTargetTrial, tempPointRotateAll);
                    //DebugWriteUtils.WriteTestOutput(myMatrix, pointsSourceTrial, tempPointRotateAll, pointsTargetTrial, pointsTargetTrial.Count);
                    Debug.WriteLine("------>ICP Iteration Trial: " + iPoint.ToString() + " : Mean Distance: " + distanceOfNewPoint.ToString());
                    if (Math.Abs(distanceOfNewPoint - res.MeanDistance) < ICPSettings.ThresholdOutlier)
                    {
                        PointCloud pointsTargetTrial = PointCloud.CloneAll(res.PointsTarget);
                        PointCloud pointsSourceTrial = PointCloud.CloneAll(res.PointsSource);


                        myMatrix = TryoutNewPoint(iPoint, pointsTarget, pointsSource, pointsTargetTrial, pointsSourceTrial, this.LandmarkTransform);

                        PointCloud myPointsTransformed = MathUtilsVTK.TransformPoints(pointsSourceTrial, myMatrix);
                        this.MeanDistance = PointCloud.MeanDistance(pointsTargetTrial, myPointsTransformed);
                       // this.MeanDistance = totaldist / Convert.ToSingle(pointsTargetTrial.Count);


                        //DebugWriteUtils.WriteTestOutputVector3("Iteration " + iPoint.ToString(),  myMatrix, pointsSourceTrial, myPointsTransformed, pointsTargetTrial);

                        //could also remove this check...
                        if (Math.Abs(oldMeanDistance - this.MeanDistance) < ICPSettings.ThresholdOutlier)
                        {

                            res.PointsTarget = pointsTargetTrial;
                            res.PointsSource = pointsSourceTrial;
                            res.Matrix = myMatrix;
                            res.PointsTransformed = myPointsTransformed;
                            oldMeanDistance = this.MeanDistance;

                            //Debug.WriteLine("************* Point  OK : ");
                            DebugWriteUtils.WriteTestOutputVector3("************* Point  OK :" , myMatrix, res.PointsSource, myPointsTransformed, res.PointsTarget);

                        }
                        //remove point from point list
                        pointsTarget.RemoveAt(iPoint);
                        pointsSource.RemoveAt(iPoint);
                       

                    }


                }
                this.Matrix = res.Matrix;
                //System.Diagnostics.Debug.WriteLine("Solution of ICP is : ");
                DebugWriteUtils.WriteTestOutputVector3("Solution of ICP", Matrix, res.PointsSource, res.PointsTransformed, res.PointsTarget);
                pointsTransformed = res.PointsTransformed;

                return pointsTransformed;
            }
            catch (Exception err)
            {
                System.Windows.Forms.MessageBox.Show("Error in Update ICP at point: " + iPoint.ToString() + " : " + err.Message);
                return null;

            }
            //Matrix4 newMatrix = accumulate.GetMatrix();
            //this.Matrix = newMatrix;

        }

        private PointCloud ICPOnPoints_WithSubset(PointCloud mypointCloudTarget, PointCloud myPCLToBeMatched, PointCloud myPointsTargetSubset, PointCloud mypointsSourceSubset)
        {

            List<Vector3> myVectorsTransformed = null;
            PointCloud myPCLTransformed = null;

            try
            {
                Matrix4 m;


                PerformICP(mypointsSourceSubset, myPointsTargetSubset);
                myVectorsTransformed = PointsTransformed.ListVectors;
                m = Matrix;

                //DebugWriteUtils.WriteTestOutput(m, mypointsSourceSubset, myPointsTransformed, myPointsTargetSubset);
                //extend points:
                //myPointsTransformed = icpSharp.TransformPointsToPointsData(mypointsSourceSubset, m);
                //-----------------------------
                //DebugWriteUtils.WriteTestOutput(m, mypointsSourceSubset, myPointsTransformed, myPointsTargetSubset);

                //now with all other points as well...
                myVectorsTransformed = new List<Vector3>();

                myPCLTransformed = m.TransformPoints(myPCLToBeMatched);
              
                //write all results in debug output
                DebugWriteUtils.WriteTestOutputVector3("Soluation of Points With Subset", m, myPCLToBeMatched, myPCLTransformed, mypointCloudTarget);

            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine("Error in ICP : " + err.Message);
                return null;
            }
            //for output:
           

            return myPCLTransformed;

        }


        private PointCloud ICPOnPoints_WithSubset_PointsData(List<PointCloud> PointsDataList, List<System.Drawing.Point> pointsLeft, List<System.Drawing.Point> pointsRight)
        {

            PointCloud myPointsTarget = PointsDataList[0];
            PointCloud mypointsSource = PointsDataList[1];
            if (PointsDataList.Count > 1)
            {
                if (pointsLeft != null)
                {
                    PointCloud mySubsetLeft = PointCloud.FromPoints2d(pointsLeft, PointsDataList[0], pointsRight);
                    PointCloud mySubsetRight = PointCloud.FromPoints2d(pointsRight, PointsDataList[1], pointsLeft);

                    if (mySubsetLeft.Count == mySubsetRight.Count)
                    {

                        PointCloud myPointsTransformed = ICPOnPoints_WithSubset(myPointsTarget, mypointsSource, mySubsetLeft, mySubsetRight);
                        return myPointsTransformed;


                    }
                    else
                    {
                        MessageBox.Show("Error in identifying stitched points ");

                    }
                }
            }

            return null;
        }
        public PointCloud ICPOnPointss_WithSubset_Vector3(PointCloud myVector3Reference, PointCloud myVector3ToBeMatched, List<System.Drawing.Point> pointsLeft2D, List<System.Drawing.Point> pointsRight2D)
        {
            List<PointCloud> PointsDataList = new List<PointCloud>();
            PointCloud myPointsTarget = myVector3Reference;
            PointsDataList.Add(myPointsTarget);

            PointCloud mypointsSource = myVector3ToBeMatched;
            PointsDataList.Add(mypointsSource);


            PointCloud myPointsTransformed = ICPOnPoints_WithSubset_PointsData(PointsDataList, pointsLeft2D, pointsRight2D);
            if (myPointsTransformed != null)
            {

                //PointsTarget = myPointsTarget;
                //pointsSource = mypointsSource;
                //PointsTransformed = myPointsTransformed;

              
                return myPointsTransformed;
            }
            return null;

        }
        
    }
   
}