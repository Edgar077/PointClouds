using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace OpenTKExtension
{

    public class PCA
    {
        public IKDTree KDTree;
       
        public Matrix3 U;
        

        public Matrix3 VT;
        public Matrix3 V;
        public Vector3 EV;

        public Matrix3 VT_NotNormalized;
        public Matrix3 U_NotNormalized;
        public Vector3 EV_NotNormalized;

     
        List<Vector3> listTransformed;
        public Vector3 Centroid;

       
        public float MeanDistance;
        public Matrix4 Matrix;

        PointCloud pcSourceCentered;
        PointCloud pcTargetCentered;
        PointCloud pcResultCentered;

      
        PointCloud pcResult = null;
        PointCloud pcResultBest = null;
        float bestResultMeanDistance = float.MaxValue;

        PointCloud pcTreeResult = null;
        public int MaxmimumIterations = 5;
        public float ThresholdConvergence = 1e-4f;

        public bool AxesRotateEffect = true;
       
        public PCA()
        {
            KDTree = new KDTreeKennell(); 
          
        }
        public static PointCloud RotateToOriginAxes(PointCloud mypointCloudSource)
        {
           

            PCA pca = new PCA();
            pca.PCA_OfPointCloud(mypointCloudSource);


            Matrix3 R = new Matrix3();
            PointCloud mypointCloudResult = PointCloud.CloneAll(mypointCloudSource);
            R = R.Rotation_ToOriginAxes(mypointCloudResult.PCAAxes);
            PointCloud.Rotate(mypointCloudResult, R);
            pca.PCA_OfPointCloud(mypointCloudResult);

            mypointCloudResult.Path = mypointCloudSource.Path;
            mypointCloudResult.Name = mypointCloudSource.Name;


            return mypointCloudResult;
        }
        private void CheckPCA(Matrix4d myMatrix4d, PointCloud sourceAxes, PointCloud targetAxes)
        {
            //-----------------------
            //for check - transform sourceAxes - should give targetAxis: i.w. resultList should contains only zeros
            PointCloud resultAxes = myMatrix4d.TransformPoints(sourceAxes);
            resultAxes.SubtractCloud(targetAxes);
            float fMax;
            if (!resultAxes.CheckCloud(this.ThresholdConvergence, out fMax))
            {
                System.Windows.Forms.MessageBox.Show("SW Error in SVD.FindTransformationMatrix, difference should be zero, is: " + fMax.ToString());
            }

        }
        /// <summary>
        /// calculates Matrix for alignment of sourceAxes and targetAxes; sets pointCloudResult
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="bestResultMeanDistance"></param>
        /// <param name="meanDistance"></param>
        /// <param name="myMatrixBestResult"></param>
        /// <param name="sourceAxes"></param>
        private void SVD_ForTwoPointCloudAlignment_Double(int i, int j, ref float meanDistance, ref Matrix4 myMatrixBestResult , PointCloud sourceAxes)
        {
            PointCloud targetAxes = InvertAxes(pcTargetCentered, pcTargetCentered.PCAAxes, j);

            //Matrix4 myMatrix = SVD_Float.FindTransformationMatrix(sourceAxes, targetAxes, ICP_VersionUsed.Scaling_Umeyama);

            Matrix4d myMatrix4d = SVD.FindTransformationMatrix_WithoutCentroids(sourceAxes, targetAxes, ICP_VersionUsed.Umeyama);

          
            //CheckPCA(myMatrix4d, sourceAxes, targetAxes);


            PointCloud myResultPC = myMatrix4d.TransformPoints(pcSourceCentered);


            //--------------
            pcTreeResult = KDTree.FindClosestPointCloud_Parallel(myResultPC);
            meanDistance = KDTree.MeanDistance;

          
            //PointCloud myPointCloudTargetTemp = kdtree.FindNearest_Rednaxela(ref myPointsResultTemp, pointCloudTargetCentered, -1);


            System.Diagnostics.Debug.WriteLine("   in iteration: MeanDistance between orientations: " + i.ToString() + " : " + j.ToString() + " : " + meanDistance.ToString("G") );

            if (meanDistance < bestResultMeanDistance)
            {
                myMatrixBestResult = myMatrix4d.ToMatrix4();
                bestResultMeanDistance = meanDistance;
                pcResultBest = myResultPC;
            }



            pcResult = myResultPC;
           

        }
        /// <summary>
        /// calculates Matrix for alignment of sourceAxes and targetAxes; sets pointCloudResult
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="bestResultMeanDistance"></param>
        /// <param name="meanDistance"></param>
        /// <param name="myMatrixBestResult"></param>
        /// <param name="sourceAxes"></param>
        private void SVD_ForTwoPointCloudAlignment_Float(int i, int j, ref float meanDistance, ref Matrix4 myMatrixBestResult, PointCloud sourceAxes)
        {
            PointCloud targetAxes = InvertAxes(pcTargetCentered, pcTargetCentered.PCAAxes, j);

            //Matrix4 myMatrix = SVD_Float.FindTransformationMatrix(sourceAxes, targetAxes, ICP_VersionUsed.Scaling_Umeyama);

            Matrix4 myMatrix = SVD_Float.FindTransformationMatrix_WithoutCentroids(sourceAxes, targetAxes, ICP_VersionUsed.Umeyama);


            //-----------------------
            //for check - should give TargetPCVectors
            List<Vector3> resultAxes = Matrix4Extension.TransformPoints(myMatrix, sourceAxes.ListVectors);
            resultAxes = resultAxes.Subtract(targetAxes.ListVectors);



            PointCloud myResultPC = myMatrix.TransformPoints(pcSourceCentered);




            //--------------
            pcTreeResult = KDTree.FindClosestPointCloud_Parallel(myResultPC);
            meanDistance = KDTree.MeanDistance;

            pcTreeResult = KDTree.BuildAndFindClosestPoints(myResultPC, this.pcTargetCentered, false);
            meanDistance = KDTree.MeanDistance;

            pcTreeResult = KDTree.FindClosestPointCloud_Parallel(myResultPC);
            meanDistance = KDTree.MeanDistance;



            pcTreeResult = KDTree.BuildAndFindClosestPoints(this.pcTargetCentered, this.pcTargetCentered, false);
            meanDistance = KDTree.MeanDistance;


            pcTreeResult = KDTree.BuildAndFindClosestPoints(this.pcSourceCentered, this.pcTargetCentered, false);
            meanDistance = KDTree.MeanDistance;


            pcTreeResult = KDTree.BuildAndFindClosestPoints(myResultPC, this.pcTargetCentered, false);

            meanDistance = KDTree.MeanDistance;




            //PointCloud myPointCloudTargetTemp = kdtree.FindNearest_Rednaxela(ref myPointsResultTemp, pointCloudTargetCentered, -1);

            float trace = myMatrix.Trace;


            //float trace = kdtree.MeanDistance;
            //float meanDistance = myMatrix.Trace;
            //Check:

            System.Diagnostics.Debug.WriteLine("   in iteration: MeanDistance between orientations: " + i.ToString() + " : " + j.ToString() + " : " + meanDistance.ToString("G"));

            if (meanDistance < bestResultMeanDistance)
            {
                myMatrixBestResult = myMatrix;
                bestResultMeanDistance = meanDistance;
                pcResultBest = myResultPC;
            }



            pcResult = myResultPC;


        }
        /// <summary>
        /// sets the result point cloud - resp. pointCloudResultBest 
        /// </summary>
        /// <param name="mypointCloudSource"></param>
        /// <returns></returns>
        private float SVD_Iteration(PointCloud mypointCloudSource)
        {
            
            float meanDistance = float.MaxValue; 
            

            pcSourceCentered = CalculatePCA_Internal(mypointCloudSource);

            Matrix4 myMatrixBestResult = Matrix4.Identity;
            // int i = -1;
            //SVD_ForTwoPointCloudAlignment(-1, -1, ref  bestResultMeanDistance, ref meanDistance, ref myMatrixBestResult, pointCloudSourceCentered.PCAAxes);
            SVD_ForTwoPointCloudAlignment_Double(-1, -1, ref meanDistance, ref myMatrixBestResult, pcSourceCentered.PCAAxes);


            //leads to a lot of iterations
            if (AxesRotateEffect)
            {
                //additionally try other solutions: Invert all axes 
                if (meanDistance > ThresholdConvergence)
                {

                    for (int i = -1; i < 3; i++)
                    {

                        PointCloud sourceAxes = InvertAxes(pcSourceCentered, pcSourceCentered.PCAAxes, i);

                        for (int j = -1; j < i; j++)
                        {
                            SVD_ForTwoPointCloudAlignment_Double(i, j, ref meanDistance, ref myMatrixBestResult, sourceAxes);
                            if (meanDistance < ThresholdConvergence)
                                break;
                        }

                        if (meanDistance < ThresholdConvergence)
                            break;

                    }
                }
                
            }
            Matrix4.Mult(ref myMatrixBestResult, ref this.Matrix, out this.Matrix);

            //pointCloudResultBest

            return bestResultMeanDistance;


        }

        public static PointCloud AlignPointClouds_Simple(PointCloud pointCloudSource, PointCloud pointCloudTarget)
        {

            PCA pca = new PCA();
            pca.MaxmimumIterations = 1;
            pca.AxesRotateEffect = false;

            PointCloud pcResult = pca.AlignPointClouds_SVD(pointCloudSource, pointCloudTarget);

            pcResult = PointCloud.CalculateMergedPoints_SimpleAdd(pcResult, pointCloudTarget , true);
            return pcResult;

        }
        public PointCloud AlignPointClouds_SVD(PointCloud pointCloudSource, PointCloud pointCloudTarget)
        {
            
            try
            {
                if (pointCloudSource == null || pointCloudTarget == null || pointCloudSource.Count == 0 || pointCloudTarget.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("PCA - please check point clouds ");
                    return null;

                }
                this.Matrix = Matrix4.Identity;
               // pointCloudSourceCentered = ShiftByCenterOfMass(pointCloudSource);
                pcSourceCentered = CalculatePCA_Internal(pointCloudSource);
                PrepareTargetTree(pointCloudTarget);

                PointCloud myPointCloudIteration = PointCloud.CloneAll(pointCloudSource);

                for (int i = 0; i < MaxmimumIterations; i++)
                {
                    float meanDistance = SVD_Iteration(myPointCloudIteration);
                    System.Diagnostics.Debug.WriteLine("-->>  Iteration " + i.ToString() + " : Mean Distance : " + meanDistance.ToString("G") + ": duration: " + GlobalVariables.TimeSpanString());


                    //myPointCloudIteration = pcResultBest;
                    if (meanDistance < ThresholdConvergence)
                        break;
                    
                }

                //final check:
                //this.Matrix = AdjustSourceTargetByTranslation(Matrix, pointCloudSource, pointCloudTarget);
                pcResult = Matrix.TransformPoints(pointCloudSource);

                pcTreeResult = KDTree.FindClosestPointCloud_Parallel(pcResult);
                MeanDistance = KDTree.MeanDistance;

            
                pcTreeResult = KDTree.FindClosestPointCloud_Parallel(pcResultBest);
                MeanDistance = KDTree.MeanDistance;

                //"Shuffle" effect - the target points are in other order after kdtree search:
                //The mean distance calculated again, as check (was calculated before in the kdTree routine)

               
                System.Diagnostics.Debug.WriteLine("-->>  TO CHECK: PCA (SVD) - Final Mean Distance : " + MeanDistance.ToString("G"));

                //MeanDistance = PointCloud.MeanDistance(pointCloudResult, pointCloudTarget);
                //System.Diagnostics.Debug.WriteLine("-->>  PCA (SVD) - Final Mean Distance : " + MeanDistance.ToString("G"));

                this.Matrix = AdjustSourceTargetByTranslation(Matrix, pointCloudSource, pointCloudTarget);
                pcResult = Matrix.TransformPoints(pointCloudSource);
                pcResultCentered = CalculatePCA_Internal(pcResult);

              
                
            }
            catch(Exception err)
            {
                System.Windows.Forms.MessageBox.Show("Error aligning point cloud" + err.Message);
            }
            return pcResult;
        }
        public PointCloud AlignPointClouds_SVD_WithShuflleEffect(bool axesRotateEffect, PointCloud pointCloudSource, PointCloud pointCloudTarget)
        {
            try
            {
                if (pointCloudSource == null || pointCloudTarget == null || pointCloudSource.Count == 0 || pointCloudTarget.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("PCA - please check point clouds ");
                    return null;

                }
                this.Matrix = Matrix4.Identity;
                pcSourceCentered = CalculatePCA_Internal(pointCloudSource);
                PrepareTargetTree(pointCloudTarget);


                PointCloud myPointCloudIteration = PointCloud.CloneAll(pointCloudSource);

                for (int i = 0; i < MaxmimumIterations; i++)
                {
                    float meanDistance = SVD_Iteration(myPointCloudIteration);
                    System.Diagnostics.Debug.WriteLine("-->>  Iteration " + i.ToString() + " : Mean Distance : " + meanDistance.ToString("G") + ": duration: " + GlobalVariables.TimeSpanString());

                    if (meanDistance < ThresholdConvergence)
                        break;
                    myPointCloudIteration = pcResultBest;
                }

                //final check:

                pcResultCentered = CalculatePCA_Internal(pcResult);


                //"Shuffle" effect - the target points are in other order after kdtree search:
                //The mean distance calculated again, as check (was calculated before in the kdTree routine)

                MeanDistance = PointCloud.MeanDistance(pcResultBest, pcTreeResult);
                System.Diagnostics.Debug.WriteLine("-->>  TO CHECK: PCA (SVD) - Final Mean Distance : " + MeanDistance.ToString("G"));

                //MeanDistance = PointCloud.MeanDistance(pointCloudResult, pointCloudTarget);
                //System.Diagnostics.Debug.WriteLine("-->>  PCA (SVD) - Final Mean Distance : " + MeanDistance.ToString("G"));

                this.Matrix = AdjustSourceTargetByTranslation(Matrix, pointCloudSource, pointCloudTarget);
                pcResult = Matrix.TransformPoints(pointCloudSource);
                pcResultCentered = CalculatePCA_Internal(pcResult);

                //MeanDistance = PointCloud.MeanDistance(pointCloudResult, pointCloudTarget);
                //System.Diagnostics.Debug.WriteLine("-->>  PCA (SVD) - Final Mean Distance : " + MeanDistance.ToString("G"));

                //for display later:




            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Error aligning point cloud");
            }
            return pcResult;
        }

        /// <summary>
        /// Calculates the PCA Axes of the PointCloud
        /// Returns centered points (by center of mass)
        /// </summary>
        /// <param name="pointCloud"></param>
        private PointCloud CalculatePCA_Internal(PointCloud pointCloud)
        {
            PointCloud mypointCloudCentered = new PointCloud();
            mypointCloudCentered = ShiftByCenterOfMass(pointCloud);
            SVD_OfPointCloud_Double(mypointCloudCentered, false);

            AssignPCAxes(pointCloud, mypointCloudCentered);

            return mypointCloudCentered;

        }
       
        /// <summary>
        /// projected result is in PointResult0, etc.
        /// </summary>
        /// <param name="pointsSource"></param>
        public void PCA_OfPointCloud(PointCloud pointsSource)
        {
            pcSourceCentered = CalculatePCA_Internal(pointsSource);

        }

        private void PrepareTargetTree(PointCloud pointCloudTarget)
        {

            //second object:
            //-----------
            pcTargetCentered = CalculatePCA_Internal(pointCloudTarget);
           
            //kdtree = new KDTreeJeremyC();
            KDTree = new KDTreeKennell();

            //kdtree.Build(pointCloudTarget);
            KDTree.Build(pcTargetCentered);

            pcResult = null;
            pcTreeResult = null;


        }
        
      
    
        public PointCloud AlignPointClouds_OneVector(PointCloud pointCloudSource, PointCloud pointCloudTarget, int vectorNumberSource, int vectorNumberTarget)
        {


            //-------------------
            pcSourceCentered = CalculatePCA_Internal(pointCloudSource);



            //second object:
            //-----------
            pcTargetCentered = CalculatePCA_Internal(pointCloudTarget);


            //Vector3 v = TargetPCVectors[vectorNumberTarget];
            //v.X = -v.X;
            //v.Y = -v.Y;
            //v.Z = -v.Z;
            //TargetPCVectors[vectorNumberTarget] = v;


            Matrix3 R = new Matrix3();
            //R = R.RotationOneVectorToAnother(TargetPCVectors[vectorNumber], SourcePCVectors[vectorNumber]);
            R = R.RotationOneVectorToAnother(pointCloudSource.PCAAxes[vectorNumberSource].Vector, pointCloudTarget.PCAAxes[vectorNumberTarget].Vector);


            //R.CheckRotationMatrix();

            //

            //test:
            //Vector3 testV = R.MultiplyVector(sourceV);


            PointCloud pointCloudResult = PointCloud.CloneAll(pointCloudSource);
            PointCloud.SubtractVectorRef(pointCloudResult, pointCloudSource.CentroidVector);
            PointCloud.Rotate(pointCloudResult, R);
            PointCloud.AddVectorToAll(pointCloudResult, pointCloudTarget.CentroidVector);

            pcResultCentered = CalculatePCA_Internal(pointCloudResult);


            MeanDistance = PointCloud.MeanDistance(pointCloudResult, pointCloudTarget);
            System.Diagnostics.Debug.WriteLine("-->>  PCA (V) - Mean Distance : " + MeanDistance.ToString("0.000000"));



            return pointCloudResult;


        }

        private List<Vector3> ShiftByCenterOfMassVectorList(PointCloud pointsSource)
        {
            Centroid = pointsSource.CentroidVectorRecalc;
            List<Vector3> listSource = pointsSource.ListVectors;
            listSource.SubtractVector(this.Centroid);
            return listSource;
        }
        private PointCloud ShiftByCenterOfMass(PointCloud pointsSource)
        {
            Centroid = pointsSource.CentroidVectorRecalc;
            PointCloud pcNew = PointCloud.SubtractVector(pointsSource, this.Centroid);
            return pcNew;
        }

   
        /// <summary>
        /// assume - vectors are mass - centered!
        /// </summary>
        /// <param name="pointCloud"></param>
        /// <param name="axesVectors"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private PointCloud InvertAxes(PointCloud pointCloud, PointCloud axesVectors, int i)
        {

            PointCloud resultC= PointCloud.CloneAll(axesVectors);
            
            if (i == -1)
                return resultC;

            
            resultC.Vectors[i] = resultC.Vectors[i].Negate();
            
            return resultC;

        }
        public PointCloud AlignToCenter(PointCloud pointCloudSource)
        {

            pcSourceCentered = CalculatePCA_Internal(pointCloudSource);
           
            
            Matrix3 R = new Matrix3();
            R = R.RotationChangeBasis(pointCloudSource.PCAAxes.ListVectors);


            
            PointCloud pointCloudResult = PointCloud.CloneAll(pointCloudSource);
            PointCloud.SubtractVectorRef(pointCloudResult, pointCloudSource.CentroidVector);
            PointCloud.Rotate(pointCloudResult, R);

            pcResultCentered = CalculatePCA_Internal(pointCloudResult);
            
           
            
           
            return pointCloudResult;


        }

        private void AssignPCVectorsOld(PointCloud pointsSource, PointCloud mypointCloudSourceCentered)
        {
            //pointsSource.PCAAxesNew = new PointCloud();
            //pointsSource.CentroidVector = Centroid;
            pointsSource.PCAAxes = new PointCloud();
            List<Vector3> vectorList = mypointCloudSourceCentered.ListVectors;
            for (int i = 0; i < 3; i++)
            {
                List<Vector3> vList = GetResultOfEigenvector(i, vectorList);
                vList.SubtractVector(Centroid);

                Vector3 v = vList.GetMax();
                if (EV[i] == 0)
                    v = new Vector3();
                pointsSource.PCAAxes.AddVector(new Vector3(v));
                

            }
            
            for (int i = 0; i < 3; i++)
            {

                pointsSource.PCAAxes.Vectors[i] += Centroid;
                              

            }
            mypointCloudSourceCentered.PCAAxes = pointsSource.PCAAxes;



        }
        /// <summary>
        /// assigns the PC Axes to both pointsSource and mypointCloudSourceCentered
        /// </summary>
        /// <param name="pointsSource"></param>
        /// <param name="mypointCloudSourceCentered"></param>
        private void AssignPCAxes(PointCloud pointsSource, PointCloud mypointCloudSourceCentered)
        {
            
            //pointsSource.CentroidVector = Centroid;
            
            pointsSource.PCAAxes = new PointCloud();
            List<Vector3> vectorList = mypointCloudSourceCentered.ListVectors;
            for (int i = 0; i < 3; i++)
            {
                Vector3 v = VT.ExtractColumn(i);
                v = v * Convert.ToSingle(Math.Sqrt(EV[i]));
                Vector3 ve = new Vector3(v);
                pointsSource.PCAAxes.AddVector(ve);

            }

            mypointCloudSourceCentered.PCAAxes = pointsSource.PCAAxes;


        }
        ////}
        ///// <summary>
        ///// PCA are center of mass - centered
        ///// </summary>
        ///// <param name="pointCloud"></param>
        ///// <param name="myCentroid"></param>
        //private void AssignPCVectors(PointCloud pointCloud, PointCloud mypointCloudSourceCentered)
        //{
        //    pointCloud.CentroidPCA = Centroid;
        //    pointCloud.PCAAxes = new PointCloud();
        //    for (int i = 0; i < 3; i++)
        //    {
        //        Vector3 v = VT.ExtractColumn(i);
        //        //v = v * Math.Sqrt(EV[i]);
        //        v = v * EV[i];
        //        float d = v.Length;
        //        Vector3 ve = new Vector3(i, v);
        //        pointCloud.PCAAxes.Add(ve);
        //    }

        //    mypointCloudSourceCentered.PCAAxes = pointCloud.PCAAxes;


        ////}
        ////}
        ////}
        ///// <summary>
        ///// PCA are center of mass - centered
        ///// </summary>
        ///// <param name="pointCloud"></param>
        ///// <param name="myCentroid"></param>
        //private void AssignPCVectors(PointCloud pointCloud, PointCloud mypointCloudSourceCentered)
        //{
        //    pointCloud.CentroidPCA = Centroid;
        //    pointCloud.PCAAxes = new PointCloud();
        //    for (int i = 0; i < 3; i++)
        //    {
        //        Vector3 v = VT_NotNormalized.ExtractColumn(i);
        //        //v = v * Math.Sqrt(EV[i]);
        //        v = v * EV_NotNormalized[i];
        //        float d = v.Length;
        //        Vector3 ve = new Vector3(i, v);
        //        pointCloud.PCAAxes.Add(ve);
        //    }

        //    mypointCloudSourceCentered.PCAAxes = pointCloud.PCAAxes;


        //}
       

   
         
   
        private static PointCloud CalculateResults(Matrix3 Ub, Matrix3 Ua, PointCloud pointCloudSource, Vector3 centroidB, Vector3 centroidA)
        {
            Matrix3 R;
            Matrix3.Mult(ref Ub, ref Ua, out R);
            

            PointCloud pointCloudResult = PointCloud.CloneAll(pointCloudSource);
            PointCloud.Rotate(pointCloudResult, R);

            Vector3 t = centroidB - R.MultiplyVector(centroidA);
            //Vertices.AddVector(pointCloudResult, t);
            return pointCloudResult;

        }

        
       
   

        ///// <summary>
        ///// compute normals for a all vectors of pointSource
        ///// </summary>
        ///// <param name="pointsSource"></param>
        ///// <returns></returns>
        //public List<Vector3> Normals(PointCloud pointsSource, bool centerOfMassMethod, bool flipNormalWithOriginVector)
        //{
        //    Vector3 normalPrevious = new Vector3();
        //    List<Vector3> normals = new List<Vector3>();
        //    for(int i = 0; i < pointsSource.Count; i++)
        //    {
        //        Vector3 v = pointsSource[i];
        //        List<Vector3> sublist = new List<Vector3>();
        //        for (int j = 0; j < v.KDTreeSearch.Count; j++)
        //        {
        //            Vector3 vNearest = pointsSource[v.KDTreeSearch[j].Key];
        //            sublist.Add(pointsSource[v.KDTreeSearch[j].Key].Vector);
                  
        //        }
        //        if (centerOfMassMethod)
        //        {
        //            Vector3 centroid = sublist.CalculateCentroid();
        //            sublist.SubtractVector(centroid);
        //        }
        //        else
        //        {
        //            sublist.SubtractVector(v.Vector);
        //        }
        //        SVD_ForListVectorsMassCentered(sublist, true);
          
        //        Vector3 normal = V.ExtractRow(2).ToVector();
        //        if (flipNormalWithOriginVector)
        //            AdjustOrientationWithVector(ref normal, v.Vector.ToVector());

        //        //if (i > 0)
        //        //    AdjustOrientation(ref normal, normalPrevious);
        //        normalPrevious = normal;


        //        normal.Normalize();
        //        normals.Add(normal);

        //        //EDGAR TODO
        //        //v.IndexNormals.Add(normals.Count - 1);

        //       // to show ALL vectors (including the 2 vectors on the plane:
        //       //AddAllPlaneVectors(v, normals);

        //    }

        //    return normals;

        //}
   
      
        //private void AddAllPlaneVectors(Vector3 v, List<Vector3> normals)
        //{
        //    //to show ALL vectors (including the 2 vectors on the plane:
        //    for (int j = 0; j < 3; j++)
        //    {
        //        Vector3 normal = V.ExtractRow(j);

        //        normal.Normalize();
        //        normals.Add(normal);
        //        v.IndexNormals.Add(normals.Count - 1);

        //    }

        //}
        private void AdjustOrientation(ref Vector3 normal, Vector3 previousNormal)
        {
            //return;

            float d1 = Vector3.Dot(previousNormal, normal);
            
         
            if(d1 < 0 && d1 < -1e-3f)
            {
                //normal = normalFlipped;
                normal = Vector3.Multiply(normal, -1);
            }
            //else
            //{
            //    if (d2 < d1)
            //        normal = normalFlipped;
            //}

        }
        private void AdjustOrientationWithVector(ref Vector3 normal, Vector3 v)
        {
           
            float d1 = Vector3.Dot(v, normal);

            if (d1 < 0 && d1 < -1e-3f)
            {
               
                normal = Vector3.Multiply(normal, -1);
            }
          

        }
      
        private Matrix3 ExtractMatrixColumnN(int N, Matrix3 W)
        {
            Matrix3 Vnew = new Matrix3();
            Vnew = W.Copy();


            for (int i = 0; i < 3; i++)
            {
               
                if(i != N)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        Vnew[i, j] = 0;
                    }
                }
            }
            return Vnew;


        }
        private Matrix3 ExtractMatrixRowN(int N, Matrix3 W)
        {
            Matrix3 Vnew = new Matrix3();
            Vnew = W.Copy();


            for (int i = 0; i < 3; i++)
            {

                if (i != N)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        Vnew[j, i] = 0;
                    }
                }
            }
            return Vnew;


        }
       
        private List<Vector3> TransformPoints(List<Vector3> pointsToTransform, Matrix3 mat)
        {
            
            listTransformed = new List<Vector3>();
            for (int i = 0; i < pointsToTransform.Count; i++)
            {
                Vector3 v = pointsToTransform[i];
                Vector3 result = mat.MultiplyVector(v);
                listTransformed.Add(result);
            }
            return listTransformed;

        }
        private void SVD_ForListVectorsMassCentered(List<Vector3> pointsSource, bool normalsCovariance)
        {
           
            //calculate correlation matrix
            Matrix3 C = PointCloud.CovarianceMatrix(pointsSource, normalsCovariance);
          
            SVD_Float.Eigenvalues_Helper(C);
            EV = SVD_Float.EV;
            VT = SVD_Float.VT;
            U = SVD_Float.U;

            V = Matrix3.Transpose(VT);

            
        }
     
        private void SVD_OfPointCloud_Double(PointCloud pointsSource, bool normalsCovariance)
        {
            //calculate correlation matrix

            //SVD.Eigenvalues_Helper(C);
            //Matrix3d C = PointCloud.CovarianceMatrix_Double(pointsSource, normalsCovariance);

            Matrix3d C = PointCloud.CovarianceMatrix3d(pointsSource, normalsCovariance);
            SVD.Eigenvalues_Helper(C);
           
         
            EV = new Vector3(Convert.ToSingle(SVD.EV.X), Convert.ToSingle(SVD.EV.Y), Convert.ToSingle(SVD.EV.Z));
            VT = SVD.VT.ToMatrix3();
            U = SVD.U.ToMatrix3();

        }
        private void SVD_OfPointCloud_Float(PointCloud pointsSource, bool normalsCovariance)
        {
            //calculate correlation matrix

            //SVD.Eigenvalues_Helper(C);
            //Matrix3d C = PointCloud.CovarianceMatrix_Double(pointsSource, normalsCovariance);

            Matrix3 C = PointCloud.CovarianceMatrix3(pointsSource, normalsCovariance);
            SVD_Float.Eigenvalues_Helper(C);


            EV = SVD_Float.EV;
            VT = SVD_Float.VT;
            U = SVD_Float.U;

        }

        private void CheckSVD(Matrix3 C)
        {
            //check transformation
            Matrix3 R = Matrix3.Mult(U, VT);
            Matrix3 testRight = VT.MultiplyDiagonalElements(EV);
            //R should be now C
            Matrix3 testleft = Matrix3.Mult(U, C);
            testleft = Matrix3.Mult(testleft, VT);
            testRight.CompareMatrices(testleft, 1e-2f);
            //PointCloud.Rotate(pointsSource, VT);

        }
        private void CheckSVD_NotNormalized(Matrix3 C)
        {
            //check transformation
            Matrix3 R = Matrix3.Mult(U_NotNormalized, VT_NotNormalized);
            R = R.MultiplyDiagonalElements(EV_NotNormalized);
            //R should be now C
            R.CompareMatrices(C, 1e-2f);
            //PointCloud.Rotate(pointsSource, VT);

        }
        private List<Vector3> GetResultOfEigenvector(int eigenvectorUsed, List<Vector3> listTranslated)
        {
            Matrix3 VTNew = ExtractMatrixColumnN(eigenvectorUsed, VT);
            Matrix3 UNew = ExtractMatrixRowN(eigenvectorUsed, U);
            
            Matrix3 R = Matrix3.Mult(UNew, VTNew);
            List<Vector3> resultList = TransformPoints(listTranslated, R);


            resultList.Add(Centroid);

            return resultList;

        }
       
  
        public List<Vector3> ProjectPointsOnPCAAxes()
        {
            List<Vector3> listProjected = new List<Vector3>();
            listProjected.AddRange(PointsResult0);
            listProjected.AddRange(PointsResult1);
            listProjected.AddRange(PointsResult2);

            
            return listProjected;


        }
        private List<Vector3> TransformPointsAfterPCA(List<Vector3> listVector3)
        {

            PointCloud pc = PointCloud.FromListVector3(listVector3);
            List<Vector3> listResult = PointCloud.CloneAll(pc).ListVectors;
           
          
            Matrix3 R = Matrix3.Mult(U, VT);
            listResult = TransformPoints(listResult, R);

            return listResult;

        }

    
        
       
        public PointCloud CalculatePCA(PointCloud pointsSource, int eigenvectorUsed)
        {
            List<Vector3> Vector3Source = pointsSource.ListVectors;

            Centroid = pointsSource.CentroidVector;

            List<Vector3> listTranslated = pointsSource.Clone().ListVectors;

            Vector3Source.SubtractVector(Centroid);

            SVD_ForListVectorsMassCentered(listTranslated, false);
            List<Vector3> vectors = GetResultOfEigenvector(eigenvectorUsed, listTranslated);

            return PointCloud.FromListVector3(Vector3Source);

        }
   
        private List<Vector3> CheckTransformation(List<Vector3> pointsSource)
        {
            List<Vector3> listResult = TransformPointsAfterPCA(pointsSource);
            listResult.Add(this.Centroid);

            return listResult; // should be pointsSource
        }
      

        public List<Vector3> PointsResult0
        {
            get
            {
                List<Vector3> list = GetResultOfEigenvector(0, pcSourceCentered.ListVectors);
                return list;

            }
        }
        public List<Vector3> PointsResult1
        {
            get
            {
                List<Vector3> list = GetResultOfEigenvector(1, pcSourceCentered.ListVectors);
                return list;

            }
        }
        public List<Vector3> PointsResult2
        {
            get
            {
                List<Vector3> list = GetResultOfEigenvector(2,  pcSourceCentered.ListVectors);
                return list;

            }
        }
        private Matrix4 AdjustSourceTargetByTranslation(Matrix4 myMatrixFound, PointCloud pointCloudSource, PointCloud pointCloudTarget)
        {
            Matrix3 R = myMatrixFound.ExtractMatrix3();
            Vector3 T = SVD_Float.CalculateTranslation(pointCloudSource.CentroidVector, pointCloudTarget.CentroidVector, R);
            myMatrixFound = myMatrixFound.AddTranslation(T);
            return myMatrixFound;

        }

        
    }
}
