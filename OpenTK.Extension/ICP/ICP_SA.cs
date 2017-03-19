
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
        
    
        /// <summary>
        /// a simulated annealing like technique
        /// </summary>
        /// <param name="pointsTarget"></param>
        /// <param name="pointsSource"></param>
        /// <param name="myNumberPoints"></param>
        /// <param name="myLandmarkTransform"></param>
        /// <param name="maxSolutions"></param>
        /// <returns></returns>
        private static ICPSolution IterateSA(PointCloud pointsSource, PointCloud pointsTarget, int myNumberPoints, int maxSolutions)
        {
            int i = 0;
            //int currentIteration = 0;
            try
            {
                if (myNumberPoints > pointsTarget.Count)
                    myNumberPoints = pointsTarget.Count;

                List<ICPSolution> solutionList = new List<ICPSolution>();


                for (i = 0; i < maxSolutions; i++)
                {

                    ICPSolution myTrial = ICPSolution.SetRandomIndices(myNumberPoints, pointsSource.Count, solutionList);

                    //myTrial.PointsTargetTrial = RandomUtils.ExtractPoints(pointsTarget, myTrial.RandomIndices);
                    myTrial.PointsSource = RandomUtils.ExtractPoints(pointsSource, myTrial.RandomIndices);

                    //myTrial.Matrix = TryoutPointsSA(pointsTarget, pointsSource, myTrial, myLandmarkTransform);//, accumulate);
                    
                    myTrial.PointsTransformed = MathUtilsVTK.TransformPoints(myTrial.PointsSource, myTrial.Matrix);

                    myTrial.MeanDistance = PointCloud.MeanDistance(myTrial.PointsTarget, myTrial.PointsTransformed);
                   // myTrial.MeanDistance = totaldist / Convert.ToSingle(myTrial.PointsSource.Count);
                  
                    solutionList.Add(myTrial);

                }
                
                if (solutionList.Count > 0)
                {
                    solutionList.Sort(new ICPSolutionComparer());
                    RemoveSolutionIfMatrixContainsNaN(solutionList);
                    if(solutionList.Count == 0)
                        System.Windows.Forms.MessageBox.Show("No start solution could be found !");


                    Debug.WriteLine("Solutions found after: " + i.ToString() + " iterations, number of solution " + solutionList.Count.ToString());

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
                System.Windows.Forms.MessageBox.Show("Error in IterateStartPoints of ICP at: " + i.ToString() + " : " + err.Message);
                return null;
            }


        }

      public static Matrix4 TryoutPointsSA(PointCloud pointsTarget, PointCloud pointsSource, ICPSolution res, LandmarkTransform myLandmarkTransform)
      {
         

          //transform:
          MathUtilsVTK.FindTransformationMatrix(res.PointsSource, res.PointsTarget, myLandmarkTransform);//, accumulate);

          res.Matrix = myLandmarkTransform.Matrix;

          return res.Matrix;

      }
    }
   
}