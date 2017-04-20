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
using System.Linq;

namespace ICPLib
{


    public partial class IterativeClosestPointTransform
    {
        
        #region public members

        public IKDTree KDTree;
        public SettingsICP ICPSettings = new SettingsICP();

        public int NumberOfIterations;
        public float MeanDistance;

        public PointCloud PSource;
        public PointCloud PTarget;
        public Matrix4 Matrix;

        Vector3 centroidSource = new Vector3(0, 0, 0);
        Vector3 centroidTarget = new Vector3(0, 0, 0);

        public PointCloud PMerged;
        public int PointsAdded;

        #endregion

        #region private members

        float startAngleForNormalsCheck = 45;

        private PointCloud pointsTransformed;
        private PointCloud pointsSource;
        private PointCloud pointsTarget;
        public PointCloud PointsResultKDTree ;

        //private List<Vector3> normalsSource;


        //private List<Vector3> normalsTarget;


        private LandmarkTransform LandmarkTransform;
        private static IterativeClosestPointTransform instance;

        #endregion

        public IterativeClosestPointTransform()//:base(PointerUtils.GetIntPtr(new float[3]), true, true)
        {
           
            KDTree = new KDTreeKennell();
            //KDTree = new KDTreeJeremyC();
            //KDTree = new KDTreeBruteForce();

            Reset();

            this.PSource = null;
            this.PTarget = null;

            this.LandmarkTransform = new LandmarkTransform();

            this.NumberOfIterations = 0;
            

        }


        public void Reset_RealData()
        {

            ICPSettings.MaximumNumberOfIterations = 100;
            ICPSettings.FixedTestPoints = false;
            ICPSettings.Normal_RemovePoints = false;
            ICPSettings.Normal_SortPoints = false;

            ICPSettings.ResetVector3ToOrigin = true;
            ICPSettings.ICPVersion = ICP_VersionUsed.Zinsser;
            
        }

        public void Settings_Reset_GeometricObject()
        {
            ICPSettings.MaximumNumberOfIterations = 5;
            ICPSettings.FixedTestPoints = true;
            ICPSettings.ResetVector3ToOrigin = false;

            ICPSettings.Normal_RemovePoints = false;

        }

        public void Reset()
        {
            instance = this;
            this.MeanDistance = float.MaxValue;

            this.ICPSettings.ICPVersion = ICP_VersionUsed.Umeyama;
            this.ICPSettings.SimulatedAnnealing = false;
            this.ICPSettings.Normal_RemovePoints = false;
            this.ICPSettings.Normal_SortPoints = false;

            this.ICPSettings.FixedTestPoints = false;
            this.ICPSettings.MaximumNumberOfIterations = 50;
            this.ICPSettings.ResetVector3ToOrigin = true;
            this.ICPSettings.DistanceOptimization = false;
            this.ICPSettings.MaximumMeanDistance = 1.0e-3f;

        }
        public PointCloud PerformICP(PointCloud mypointsSource, PointCloud myPointsTarget)
        {
            this.PTarget = myPointsTarget;
            this.PSource = mypointsSource;
            ResetToCenterOfMass();

            if (this.ICPSettings.Prealign_PCA)
            {
                PCA pca = new PCA();
                pca.MaxmimumIterations = 1;
                pca.AxesRotateEffect = false;
                this.PSource = pca.AlignPointClouds_SVD(this.PSource, this.PTarget);
            }
            if (ICPSettings.ICPVersion == ICP_VersionUsed.UsingStitchData)
                return PerformICP_Stitching();
            else if (ICPSettings.ICPVersion == ICP_VersionUsed.RandomPoints)
                return PerformICP();
            else
                return PerformICP();
        }
        public static IterativeClosestPointTransform Instance
        {
            get
            {
                if (instance == null)
                    instance = new IterativeClosestPointTransform();
                return instance;

            }
        }
        public PointCloud PointsTransformed
        {
            get
            {
                return pointsTransformed;
            }
            set
            {
                pointsTransformed = value;
            }
        }



        public void Inverse()
        {
            PointCloud tmp1 = this.PSource;
            this.PSource = this.PTarget;
            this.PTarget = tmp1;
            //this.Modified();
        }



        private static float TransformPoints(ref PointCloud myPointsTransformed, PointCloud pointsTarget, PointCloud pointsSource, Matrix4 myMatrix)
        {
            myPointsTransformed = MathUtilsVTK.TransformPoints(pointsSource, myMatrix);
            float meanDistance = PointCloud.MeanDistance(pointsTarget, myPointsTransformed);

            return meanDistance;

        }
        private Matrix4 Helper_FindTransformationMatrix(PointCloud resultKDTree)
        {
            Matrix4 myMatrix;

            if (ICPSettings.ICPVersion == ICP_VersionUsed.Horn)
            {
                MathUtilsVTK.FindTransformationMatrix(pointsSource, resultKDTree, this.LandmarkTransform);
                myMatrix = LandmarkTransform.Matrix;

            }
            else
            {
              
                if(ICPSettings.IgnoreFarPoints)
                    myMatrix = SVD.FindTransformationMatrix_MinimumDistance(pointsSource, resultKDTree, ICPSettings.ICPVersion, this.MeanDistance).ToMatrix4();
                else
                    myMatrix = SVD.FindTransformationMatrix(pointsSource, resultKDTree, ICPSettings.ICPVersion).ToMatrix4();
                   

            }
            return myMatrix;



        }
        private Matrix4 Helper_FindTransformationMatrixOld(PointCloud pointsSource, PointCloud pointsTarget)
        {
            Matrix4 myMatrix;

            if (ICPSettings.ICPVersion == ICP_VersionUsed.Horn)
            {
                MathUtilsVTK.FindTransformationMatrix(pointsSource, pointsTarget, this.LandmarkTransform);
                myMatrix = LandmarkTransform.Matrix;

            }
            else
            {

                myMatrix = SVD_Float.FindTransformationMatrix(pointsSource, pointsTarget, ICPSettings.ICPVersion);

            }
            return myMatrix;



        }
        private void SetNewSet()
        {

            pointsSource = this.pointsTransformed;
            pointsTarget = PointCloud.CloneAll(this.PTarget);


        }

        /// <summary>
        /// A single ICP Iteration
        /// </summary>
        /// <param name="pointsTarget"></param>
        /// <param name="pointsSource"></param>
        /// <param name="PT"></param>
        /// <param name="PS"></param>
        /// <param name="kdTree"></param>
        /// <returns></returns>
        private void Single_ICP_Iteration(float angleThreshold)
        {
            try
            {
                PointsResultKDTree = null;
                //for geometric objects:
                if(this.ICPSettings.FixedTestPoints)
                    PointsResultKDTree = this.pointsTarget.Clone();
                else
                    PointsResultKDTree = this.KDTree.FindClosestPointCloud_Parallel(pointsSource);

             

                Matrix4 myMatrix = Helper_FindTransformationMatrix(PointsResultKDTree);
             
                                
                if (myMatrix.CheckNAN())
                    return ;

                //overall matrix
                Matrix4.Mult(ref myMatrix, ref this.Matrix, out this.Matrix);

                //transform points:
                pointsTransformed = MathUtilsVTK.TransformPoints(this.PSource, Matrix);


                //for the "shuffle" effect (point order of source and target is different)
                if (this.ICPSettings.ShuffleEffect)
                {
                    CheckDuplicates_SetInteractionSet();
                }
                else
                {
                    SetNewSet();
                }
               
                this.MeanDistance = PointCloud.MeanDistance(pointsSource, PointsResultKDTree);
                if (MeanDistance < ICPSettings.MaximumMeanDistance) //< Math.Abs(MeanDistance - oldMeanDistance) < this.MaximumMeanDistance)
                    return ;

                //check: is this really needed ??
                //for the "shuffle" effect (point order of source and target is different)
                //if (this.ICPSettings.ShuffleEffect)
                //{
                //    SetNewSet();
                //}



            }
            catch (Exception err)
            {
                System.Windows.Forms.MessageBox.Show("Error in Single_ICP_Iteration: " + err.Message);
            }

        }
        private void CheckDuplicates_SetInteractionSet()
        {

            //in some cases, the pointsTarget contain ONLY the same point
            //check if at least 3 points are different
            List<int> indexToCheck = new List<int>() { 0, pointsTarget.Count - 1, (pointsTarget.Count - 1) / 2 };
            bool duplicates = false;
            for (int i = 0; i < indexToCheck.Count - 1; i++)
            {
                if (pointsTarget[indexToCheck[i]] == pointsTarget[indexToCheck[i + 1]])
                {
                    duplicates = true;
                    break;
                }

            }
            if (duplicates)
                SetNewSet();

        }

        //private void CalculateNormals(PointCloud pointsSource, PointCloud pointsTarget)
        //{

        //    PointCloud myModelTarget = new PointCloud ("Target");
        //    myModelTarget.PointCloud = pointsTarget;
        //    myModelTarget.CalculateNormals_PCA();
        //    //myModelTarget.CalculateNormals_Triangulation();


        //    PointCloud myModelSource = new PointCloud ("Source");
        //    myModelSource.PointCloud = pointsSource;
        //    //myModelSource.CalculateNormals_Triangulation();
        //    myModelSource.CalculateNormals_PCA();


        //    normalsTarget = myModelTarget.Normals;
        //    normalsSource = myModelSource.Normals;
        //}
        //private void CalculateNormals(PointCloud pointsSource, PointCloud pointsTarget, KDTreeJeremyC kdTreee)
        //{
        //    if (ICPSettings.Normal_RemovePoints || ICPSettings.Normal_SortPoints)
        //    {

        //        CalculateNormals(pointsSource, pointsTarget);
        //        kdTreee.NormalsSource = this.normalsSource;
        //        kdTreee.NormalsTarget = this.normalsTarget;
        //        if (ICPSettings.Normal_RemovePoints)
        //            kdTreee.Normals_RemovePoints = true;
        //        if (ICPSettings.Normal_SortPoints)
        //            kdTreee.Normals_SortPoints = true;


        //    }
        //}
 
     
        private void ResetToCenterOfMass()
        {

            //PointCloud PT = PointCloud.CloneAll(PTarget);
            //PointCloud PS = PointCloud.CloneAll(PSource);

            //PointCloud myPointsTransformed = null;
            ICPSettings.ResetVector3ToOrigin = false;
            if (ICPSettings.LogLevel > 0)
                GlobalVariables.ShowLastTimeSpan("Clone");

            centroidTarget = PointCloud.ResetCentroid(this.PTarget, true);
            centroidSource = PointCloud.ResetCentroid(this.PSource, true);



        }
        public PointCloud PerformICP()
        {
            //float convergenceThreshold = PTarget.BoundingBoxMaxFloat * ICPSettings.ConvergenceThreshold;
            //if (ICPSettings.ResetVector3ToOrigin)
            //{
            //}
            if(ICPSettings.LogLevel > 0)
                GlobalVariables.ShowLastTimeSpan("Before build");

            KDTree.Build(this.PTarget);

            if (ICPSettings.LogLevel > 0)
                GlobalVariables.ShowLastTimeSpan("Build Tree");

            try
            {
                if (!CheckSourceTarget(PTarget, this.PSource))
                    return null;

                pointsTarget = PointCloud.CloneAll(PTarget);
                pointsSource = PointCloud.CloneAll(PSource);

                this.Matrix = Matrix4.Identity;
                float oldMeanDistance = 0;

                if (ICPSettings.LogLevel > 0)
                    GlobalVariables.ShowLastTimeSpan("Start ICP");


                for (NumberOfIterations = 0; NumberOfIterations < ICPSettings.MaximumNumberOfIterations; NumberOfIterations++)
                {
                    //kdTreee.NormalsSource = this.normalsSource;
                    float angleThreshold = Convert.ToSingle(this.startAngleForNormalsCheck - 5) * (1.0f - this.NumberOfIterations * 1.0f / this.ICPSettings.MaximumNumberOfIterations) + 5;

                    Single_ICP_Iteration(angleThreshold);

                    //Debug.WriteLine("--------------Iteration: " + NumberOfIterations.ToString() + " : Mean Distance: " + MeanDistance.ToString("0.00000000000") );
                    if (ICPSettings.LogLevel > 0)
                        GlobalVariables.ShowLastTimeSpan("Iteration ");
                    if (MeanDistance < ICPSettings.ThresholdConvergence)
                    //if (Math.Abs(oldMeanDistance - MeanDistance) < convergenceThreshold)
                    {
                        Debug.WriteLine("Convergence reached - changes under: " + ICPSettings.ThresholdConvergence.ToString());
                        break;
                    }
                    oldMeanDistance = MeanDistance;


                }

               


                //shuffle effect - set points source to other order
                //PS = pointsSource;//reordered for shuffle effect
                //this.PSource = pointsSource;

                if (this.ICPSettings.ShuffleEffect)
                {
                    PTarget = pointsTarget;
                    //PS = pointsSource;//reordered for shuffle effect
                    //this.PSource = pointsSource;
                    PointsTransformed = this.pointsTransformed;
                }
                else
                {
                    PointsTransformed = MathUtilsVTK.TransformPoints(this.PSource, Matrix);
                }

                //ignore ICP result if the convergence results is bad
                if (MeanDistance > ICPSettings.ThresholdIgnoreICP)
                {
                    Debug.WriteLine("ICP RESULT IGNORED - TOO BAD " + this.MeanDistance.ToString("0.0000"));
                    return this.PTarget;

                }

                if (this.pointsTransformed != null)
                {
                    //DebugWriteUtils.WriteTestOutputVector3("Solution of ICP", Matrix, pointsSource, pointsTransformed, pointsTarget);
                }
                else
                {
                    //no convergence - write matrix
                    this.Matrix.Print("Cumulated Matrix ");
                }

                pointsTarget = PTarget;
                float thresh = this.MeanDistance;
                if (ICPSettings.SingleSourceTargetMatching)
                {
                    PointsTransformed = this.PointsResultKDTree;// this.pointsTransformed;
                    PointCloud.AddVector3(PointsTransformed, centroidSource);


                }
                else
                {
                    PointsTransformed = PointCloud.CalculateMergedPoints(this.pointsTransformed, this.pointsTarget, this.KDTree, ICPSettings.ThresholdMergedPoints, out PointsAdded, this.ICPSettings.ChangeColorsOfMergedPoints);
                    PointCloud.AddVector3(PointsTransformed, centroidSource);
                }
                Debug.WriteLine("--------****** Solution of ICP after : " + NumberOfIterations.ToString() + " iterations, Mean Distance: " + MeanDistance.ToString("0.00000000000") + " Matrix trace : " + this.Matrix.Trace.ToString("0.00") + " - points added : " + PointsAdded.ToString());
                
                //not the best solution but ...
                PointsTransformed.SetDefaultIndices();

                PointsTransformed.Name = "Result Cloud";
                PointsTransformed.FileNameLong = "ResultCloud.obj";


                return PointsTransformed;
            }
            catch (Exception err)
            {
                System.Windows.Forms.MessageBox.Show("Error in Update ICP at iteration: " + NumberOfIterations.ToString() + " : " + err.Message);
                return null;

            }

        }
        private static bool CheckSourceTarget(PointCloud myPointsTarget, PointCloud mypointsSource)
        {
            // Check source, target
            if (mypointsSource == null || mypointsSource.Count == 0)
            {
                MessageBox.Show("Source point set is empty");
                System.Diagnostics.Debug.WriteLine("Can't execute with null or empty input");
                return false;
            }

            if (myPointsTarget == null || myPointsTarget.Count == 0)
            {
                MessageBox.Show("Target point set is empty");
                System.Diagnostics.Debug.WriteLine("Can't execute with null or empty target");
                return false;
            }
            return true;
        }

        public bool TakenAlgorithm
        {
            get
            {
                if (KDTree != null)
                    return this.KDTree.TakenAlgorithm;
                return false;
            }
            set
            {
                if (KDTree != null)
                    this.KDTree.TakenAlgorithm = value;
            }
        }
        public PointCloud AlignCloudsFromDirectory_StartFromLast(string directory, int numberOfCloudPairs)
        {
            
            GlobalVariables.ResetTime();

            PointCloud pSource;
            PointCloud pTarget = null;


            string[] files = System.IO.Directory.GetFiles(directory, "*.obj");
            string pathResult = directory + "\\result";

            if (!System.IO.Directory.Exists(pathResult))
                System.IO.Directory.CreateDirectory(pathResult);

            int numberOfCloudsCurrent = 0;
            int numberOfCloudsResult = -1;

            int iteratorFile = -1;
            for (int i = 0; i < 1000; i++)
            {
                numberOfCloudsCurrent++;
                iteratorFile++;

                if (numberOfCloudsCurrent == numberOfCloudPairs)
                {
                    numberOfCloudsCurrent = 1;
                    numberOfCloudsResult++;
                    pTarget.ToObjFile(pathResult + "\\Result" + numberOfCloudsResult.ToString() + ".obj");

                   // iteratorFile--;
                }
                if(pTarget != null && iteratorFile == (files.Length - 2))
                {
                    numberOfCloudsResult++;
                    pTarget.ToObjFile(pathResult + "\\Result" + numberOfCloudsResult.ToString() + ".obj");
                    return pTarget;
                }
                //first iteration
                if (numberOfCloudsCurrent == 1)
                {
                    //pTarget = PointCloud.FromObjFile(files[iteratorFile]);
                    pTarget = PointCloud.FromObjFile(files[files.Length - iteratorFile -1]);
                }
                

                //pSource = PointCloud.FromObjFile(files[iteratorFile + 1]);
                pSource = PointCloud.FromObjFile(files[files.Length - iteratorFile -2]);




                //------------------
                //ICP

                Reset_RealData();



                pTarget = PerformICP(pSource, pTarget);
                System.Diagnostics.Debug.WriteLine("###### ICP for point cloud: " + pSource.Name + " - points added: " + PointsAdded.ToString());


                //   this.registrationMatrix = icp.Matrix;
                //   registrationMatrix.Save(GLSettings.Path + GLSettings.PathModels, "registrationMatrix.txt");


            }
            GlobalVariables.ShowLastTimeSpan("--> Time for ICP ");

            return pTarget;

        }
        public PointCloud AlignCloudsFromDirectory_StartLast(string directory, int numberOfCloudPairs)
        {

            GlobalVariables.ResetTime();

            PointCloud pSource;
            PointCloud pTarget = null;
            
            string[] files = IOUtils.FileNamesSorted(directory, "*.obj");
            string pathResult = directory + "\\result";

            if (!System.IO.Directory.Exists(pathResult))
                System.IO.Directory.CreateDirectory(pathResult);

            int numberOfCloudsCurrent = 0;
            int numberOfCloudsResult = -1;

            int iteratorFile = files.Length;
            for (int i = 0; i < 1000; i++)//maximum 1000 files, should be enough
            {
                numberOfCloudsCurrent++;
                iteratorFile--;

                if (numberOfCloudsCurrent == numberOfCloudPairs) //write result, start new loop
                {
                    numberOfCloudsCurrent = 1;
                    numberOfCloudsResult++;
                    pTarget.ToObjFile(pathResult + "\\Result" + numberOfCloudsResult.ToString() + ".obj");
                    
                }
                if (pTarget != null && iteratorFile < 1 )//write result - got to end - return
                {
                    numberOfCloudsResult++;
                    pTarget.ToObjFile(pathResult + "\\Result" + numberOfCloudsResult.ToString() + ".obj");
                    GlobalVariables.ShowLastTimeSpan("--> Time for ICP ");
                    return pTarget;
                }
                if (numberOfCloudsCurrent == 1)//new pairs 
                {
                    pTarget = PointCloud.FromObjFile(files[iteratorFile]);
                }


                pSource = PointCloud.FromObjFile(files[iteratorFile - 1]);

                //perform ICP:
                Reset_RealData();
                pTarget = PerformICP(pSource, pTarget);
                System.Diagnostics.Debug.WriteLine("###### ICP for point cloud: " + pSource.Name + " - points added: " + PointsAdded.ToString());
                

            }
            GlobalVariables.ShowLastTimeSpan("--> Time for ICP ");

            return pTarget;

        }
        public PointCloud AlignCloudsFromDirectory_StartFirst(string directory, int numberOfCloudPairs)
        {
           
            GlobalVariables.ResetTime();

            PointCloud pSource;
            PointCloud pTarget = null;

            string[] files = IOUtils.FileNamesSorted(directory, "*.obj");
            string pathResult = directory + "\\result";

            if (!System.IO.Directory.Exists(pathResult))
                System.IO.Directory.CreateDirectory(pathResult);

            int numberOfCloudsCurrent = 0;
            int numberOfCloudsResult = -1;

            int iteratorFile = -1;
            //for (int i = 0; i < 1000; i++)//maximum 1000 files, should be enough
            for (int i = 0; i < 1000; i++)//maximum 1000 files, should be enough
            {
                numberOfCloudsCurrent++;
                iteratorFile++;

                if (numberOfCloudsCurrent == numberOfCloudPairs) //write result, start new loop
                {
                    numberOfCloudsCurrent = 1;
                    numberOfCloudsResult++;
                    pTarget.ToObjFile(pathResult + "\\Result" + numberOfCloudsResult.ToString() + ".obj");

                }
                if (pTarget != null && iteratorFile >= (files.Length - 1))//write result - got to end - return
                {
                    numberOfCloudsResult++;
                    pTarget.ToObjFile(pathResult + "\\Result" + numberOfCloudsResult.ToString() + ".obj");
                    GlobalVariables.ShowLastTimeSpan("--> Time for ICP ");
                    return pTarget;
                }
                if (numberOfCloudsCurrent == 1)//new pairs 
                {
                    pTarget = PointCloud.FromObjFile(files[iteratorFile]);
                }


                pSource = PointCloud.FromObjFile(files[iteratorFile + 1]);

                //perform ICP:
                Reset_RealData();
                pTarget = PerformICP(pSource, pTarget);
                System.Diagnostics.Debug.WriteLine("###### ICP for point cloud: " + pSource.Name + " - points added: " + PointsAdded.ToString());


            }
            GlobalVariables.ShowLastTimeSpan("--> Time for ICP ");

            return pTarget;

        }
    

    }

}