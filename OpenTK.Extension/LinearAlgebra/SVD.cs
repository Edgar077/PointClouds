using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTKExtension;

namespace OpenTKExtension
{
    public class SVD
    {
        public static Matrix3d U = new Matrix3d();
        public static Matrix3d VT = new Matrix3d();
        public static Matrix3d R = new Matrix3d();
        public static Vector3d EV;
        //public static double[] EV ;

    
        ///// <summary>
        ///// //gives R, the rotation matrix, U - the left eigenvectors, VT - right transposed eigenvectors, EV  - the eigenvalues
        ///// </summary>
        ///// <param name="H"></param>
        //public static void Eigenvalues_Helper_Alglib(Matrix3d H)
        //{
        //    double[,] Harray = H.ToDoubleArray();
        //    double[,] Uarray = new double[3, 3];
        //    double[,] VTarray = new double[3, 3];
        //    double[] EVarray = new double[3];


        //    alglib.svd.rmatrixsvd(Harray, 3, 3, 2, 2, 2, ref EVarray, ref Uarray, ref VTarray);
        //   //U = new Matrix3d();
        //    U = U.FromDoubleArray(Uarray);

        //    //VT = new Matrix3d();
        //    VT = VT.FromDoubleArray(VTarray);
        //    R = Matrix3d.Mult(U, VT);

        //    EV = new Vector3d();
        //    EV = EV.FromDoubleArray(EVarray);
        
        //}

   
        /// <summary>
        /// //gives R, the rotation matrix, U - the left eigenvectors, VT - right transposed eigenvectors, EV  - the eigenvalues
        /// </summary>
        /// <param name="H"></param>
        /// <param name="pointsSourceTranslated"></param>
        public static void Eigenvalues_Helper(Matrix3d H)
        {
            //ALGLIB_Method = true;
            Eigenvalues_Helper_Alglib(H);
            CheckSVD(H, 1.0E-4);

            //Eigenvalues_Helper_VTK(H);
            //Matrix3d rAlglig = SVD.R.Clone();

        

        }
        /// <summary>
        /// //gives R, the rotation matrix, U - the left eigenvectors, VT - right transposed eigenvectors, EV  - the eigenvalues
        /// </summary>
        /// <param name="H"></param>
        public static void Eigenvalues_Helper_Alglib(Matrix3d H)
        {
            double[,] Harray = H.ToDoubleArray();
            double[,] Uarray = new double[3, 3];
            double[,] VTarray = new double[3, 3];
            double[] EVarray = new double[3];


            alglib.svd.rmatrixsvd(Harray, 3, 3, 2, 2, 2, ref EVarray, ref Uarray, ref VTarray);
            
            U = U.FromDoubleArray(Uarray);
            VT = VT.FromDoubleArray(VTarray);
            EV = EV.FromDoubleArray(EVarray);

            R = Matrix3d.Mult(U, VT);


        }
        /// <summary>
        /// //gives R, the rotation matrix, U - the left eigenvectors, VT - right transposed eigenvectors, EV  - the eigenvalues
        /// </summary>
        /// <param name="H"></param>
        public static void Eigenvalues_Helper_VTK(Matrix3d H)
        {
            double[,] Harray = H.ToDoubleArray();
            double[,] Uarray = new double[3, 3];
            double[,] VTarray = new double[3, 3];
            double[] EVarray = new double[3];

            //alglib.svd.rmatrixsvd(Harray, 3, 3, 2, 2, 2, ref EVarray, ref Uarray, ref VTarray);
            csharpMath.SingularValueDecomposition3x3(Harray, Uarray, EVarray, VTarray);

                        
            //U = new Matrix3d();
            U = U.FromDoubleArray(Uarray);

            U = U.FromDoubleArray(Uarray);
            VT = VT.FromDoubleArray(VTarray);
            EV = EV.FromDoubleArray(EVarray);

            R = Matrix3d.Mult(U, VT);

        }
        //other methods for SVD, for possible later usage
        //MathUtils.SingularValueDecomposition3x3(Harray, Uarray, warray, VTarray);
        //MathNet.Numerics.Providers.LinearAlgebra.Mkl.MklLinearAlgebraProvider svd = new MathNet.Numerics.Providers.LinearAlgebra.Mkl.MklLinearAlgebraProvider();
        //double[] a = MatrixUtilsNumerics.doubleFromArraydouble(Harray);
        //double[] u = MatrixUtilsNumerics.doubleFromArraydouble(Uarray);
        //double[] vt = MatrixUtilsNumerics.doubleFromArraydouble(Uarray);
        //double[] s = new double[3];
        //svd.SingularValueDecomposition(true, a, 3, 3, s, u, vt);
        private static Matrix3d CalculateRotationBySingularValueDecomposition(Matrix3d H, PointCloud pointsSourceTranslated, ICP_VersionUsed icpVersionUsed)
        {
           
            Eigenvalues_Helper(H);
            //gives R, the rotation matrix, U - the left eigenvectors, VT - right transposed eigenvectors, EV  - the eigenvalues



            //see article by Umeyama

            //if (H.Determinant < 0)
            //{
            //    R[2, 2] = -R[2, 2];
            //    //S[2, 2] = -1;
            //}

            //calculate the sign matrix for using the scale factor
            Matrix3d K2 = Matrix3d.Identity;
            
            //double check = U.Determinant * VT.Determinant;
            //if (check < 0 && Math.Abs(check) > 1E-3)
            //{
            //    K2[2, 2] = -1;
            //}
            
         
            double scale = CalculateScale_Umeyama(pointsSourceTranslated, EV, K2);
            R = Matrix3d.Mult(R, K2);
            if (icpVersionUsed == ICP_VersionUsed.Umeyama)
            {
                //R = Matrix3d.Mult(R, K2);
                R = R.MultiplyScalar(scale);
            }

            
            return R;
        }
        private static void CheckSVD(Matrix3d H, double threshold)
        {
            //check : H = U * EV * VT

            Matrix3d EVMat = new Matrix3d();
            EVMat = EVMat.FromVector(EV);
            
            //check
            // EV * VT
            Matrix3d test = Matrix3d.Mult(EVMat, VT);
            // U * EV * VT
            test = Matrix3d.Mult(U, test);

            test.CompareMatrices(H, threshold);



        }
        private static void CheckDiagonalizationResult_Base()
        {

            Matrix3d UT = Matrix3d.Transpose(U);
            Matrix3d V = Matrix3d.Transpose(VT);
            //Matrix3d Rtest = Matrix3d.Mult(UT, V);
            

            Matrix3d mat_checkShouldGiveI = Matrix3d.Mult(UT, U);
            mat_checkShouldGiveI = Matrix3d.Mult(VT, V);


            Matrix3d RT = Matrix3d.Transpose(R);
            mat_checkShouldGiveI = Matrix3d.Mult(RT, R);
            
        }

        public static Matrix3d FindRotationMatrix(PointCloud pointsSourceTranslated, PointCloud pointsTargetTranslated, ICP_VersionUsed icpVersionUsed)
        {

            try
            {
                Matrix3d H = PointCloud.CorrelationMatrix_Double(pointsSourceTranslated, pointsTargetTranslated);
                Matrix3d R = CalculateRotationBySingularValueDecomposition(H, pointsSourceTranslated, icpVersionUsed);

                //now scaling factor:
                double c;
                if (icpVersionUsed == ICP_VersionUsed.Zinsser)
                {
                    c = CalculateScale_Zinsser(pointsSourceTranslated, pointsTargetTranslated, ref R);
                }
                if (icpVersionUsed == ICP_VersionUsed.Du)
                {
                    Matrix3d C = CalculateScale_Du(pointsSourceTranslated, pointsTargetTranslated, R);
                    R = Matrix3d.Mult(R, C);
                }

                return R;
            }
            catch(Exception err)
            {
                System.Windows.Forms.MessageBox.Show("Error in finding rotation matrix: " + err.Message);
                return Matrix3d.Identity;
            }
            
        }
        public static Matrix4d FindTransformationMatrix_MinimumDistance(PointCloud pointsSource, PointCloud pointsTarget, ICP_VersionUsed icpVersionUsed, float minimumDistance)
        {
            minimumDistance *= minimumDistance * 2; 
            List<Vector3> sourceList = new List<Vector3>();
            List<Vector3> targetList = new List<Vector3>();

            int numberOfVectorsNotTaken = 0;
            for(int i = 0; i < pointsSource.Vectors.Length; i++)
            {
                float distance = pointsSource.Vectors[i].DistanceSquared(pointsTarget.Vectors[i]);
                if(distance < minimumDistance)
                {
                    sourceList.Add(pointsSource.Vectors[i]);
                    targetList.Add(pointsTarget.Vectors[i]);

                }
                else
                {
                    numberOfVectorsNotTaken++;
                }



            }

            if(numberOfVectorsNotTaken > 0)
                System.Diagnostics.Debug.WriteLine("Ignored vectors in SVD because too far: " + numberOfVectorsNotTaken.ToString() + " :  out of : " + pointsSource.Count.ToString());

           

            PointCloud pSourceNew = PointCloud.FromListVector3(sourceList);
            PointCloud pTargetNew = PointCloud.FromListVector3(targetList);

            if (pSourceNew == null || pTargetNew == null)
                return Matrix4d.Identity;

            return FindTransformationMatrix(pSourceNew, pTargetNew, icpVersionUsed);



        }
        public static Matrix4d FindTransformationMatrix(PointCloud pointsSource, PointCloud pointsTarget, ICP_VersionUsed icpVersionUsed)
        {


            //shift points to the center of mass (centroid) 
            Vector3 centroidTarget = pointsTarget.CentroidVector;
            //Vector3d centroidTarget_Double = new Vector3d(pointsTarget.CentroidVector.X, pointsTarget.CentroidVector.Y, pointsTarget.CentroidVector.Z);

            PointCloud pointsTargetTranslated = pointsTarget.Clone();
            pointsTargetTranslated.SubtractVector(centroidTarget);

            Vector3 centroidSource = pointsSource.CentroidVector;
            PointCloud pointsSourceTranslated = pointsSource.Clone();
            pointsSourceTranslated.SubtractVector(centroidSource);

            Matrix3d R = FindRotationMatrix(pointsSourceTranslated, pointsTargetTranslated, icpVersionUsed);

            Vector3d T = CalculateTranslation(centroidSource, centroidTarget, R);
            Matrix4d myMatrix = new Matrix4d();
            myMatrix = myMatrix.PutTheMatrix4dtogether(T, R);
          
            return myMatrix;

        }
        public static Matrix4d FindTransformationMatrix_WithoutCentroids(PointCloud pointsSource, PointCloud pointsTarget, ICP_VersionUsed icpVersionUsed)
        {

            Matrix3d R = FindRotationMatrix(pointsSource, pointsTarget, icpVersionUsed);

            //Vector3d T = new Vector3d();
            Matrix4d myMatrix = new Matrix4d();
            myMatrix = myMatrix.FromMatrix3d(R);

            return myMatrix;

        }
        public static Vector3d CalculateTranslation(Vector3d centroidSource, Vector3d centroidTarget, Matrix3d Rotation)
        {

            //Vector3d T = Rotation.MultiplyVector(centroidSource);
            //T = Vector3d.Subtract(centroidTarget, T);
            Vector3d T = Rotation.MultiplyVector(centroidSource);
            T = centroidTarget - T;



            return T;

        }
        public static Vector3d CalculateTranslation(Vector3 centroidSource, Vector3 centroidTarget, Matrix3d Rotation)
        {

            //Vector3d T = Rotation.MultiplyVector(centroidSource);
            //T = Vector3d.Subtract(centroidTarget, T);
            Vector3d T = Rotation.MultiplyVector(centroidSource);
            T.X = centroidTarget.X - T.X;
            T.Y = centroidTarget.Y - T.Y;
            T.Z = centroidTarget.Z - T.Z;

            
            return T;

        }
      
       
    

        private static double CalculateScale_Umeyama(PointCloud pointsSourceShift, Vector3d eigenvalues, Matrix3d K2)
        {
            double sigmaSquared = 0f;
            for (int i = 0; i < pointsSourceShift.Count; i++)
            {
                sigmaSquared += pointsSourceShift.Vectors[i].NormSquared();
            }

            sigmaSquared /= pointsSourceShift.Count;

            double c = 0.0F;
            for (int i = 0; i < 3; i++)
            {
                c += eigenvalues[i];
            }
            if (K2[2, 2] < 0)
            {
                c -= 2 * eigenvalues[2];
            }
            c = c / sigmaSquared;
            return c;
        }
        private static double CalculateScale_Zinsser(PointCloud pointsSourceShift, PointCloud pointsTargetShift, ref Matrix3d R)
        {
            double sum1 = 0;
            double sum2 = 0;
            Matrix3d RT = Matrix3d.Transpose(R);
            Matrix3d checkT = Matrix3d.Mult(RT, R);

            Matrix4d R4D = new Matrix4d();
            R4D = R4D.PutTheMatrix4dtogether(Vector3d.Zero, R);

            
     
            for(int i = 0; i < pointsSourceShift.Count; i++)
            {
                Vector3d vSource = new Vector3d(pointsSourceShift.Vectors[i].X, pointsSourceShift.Vectors[i].Y, pointsSourceShift.Vectors[i].Z);
                Vector3d vTarget = new Vector3d(pointsTargetShift.Vectors[i].X, pointsTargetShift.Vectors[i].Y, pointsTargetShift.Vectors[i].Z);

                Vector3d vd = Vector3d.TransformNormalInverse(new Vector3d(pointsSourceShift.Vectors[i].X, pointsSourceShift.Vectors[i].Y, pointsSourceShift.Vectors[i].Z), R4D);
                vd = R.MultiplyVector(vSource);
                
                //Vector3 v = new Vector3(vd.X, vd.Y, vd.Z); 

                //sum1 += Vector3.Dot(v, pointsTargetShift.Vectors[i]);
                //sum2 += Vector3.Dot(pointsSourceShift.Vectors[i], pointsSourceShift.Vectors[i]);
                
                sum1 += Vector3d.Dot(vd, vTarget);
                sum2 += Vector3d.Dot(vSource, vSource);
                
            }

            double c = sum1 / sum2;
            R = R.MultiplyScalar(c);
            return c;
        }
        private static Matrix3d CalculateScale_Du(PointCloud pointsSourceShift, PointCloud pointsTargetShift, Matrix3d R)
        {
           
            Matrix3d S = Matrix3d.Identity;
            Matrix3d K = Matrix3d.Identity;
            for (int i = 0; i < 3; i++)
            {
                K[i, i] = 0;
            }
            for (int i = 0; i < 3; i++)
            {
                K[i, i] = 1;
                double sum1 = 0;
                double sum2 = 0;
                for (int j = 0; j < pointsSourceShift.Count; j++)
                {
                    Vector3d vKumultiplied = K.MultiplyVector(pointsSourceShift.Vectors[j]);
                    Vector3d v = R.MultiplyVector(vKumultiplied);
                    Vector3d vTarget = new Vector3d(pointsTargetShift.Vectors[j].X, pointsTargetShift.Vectors[j].Y, pointsTargetShift.Vectors[j].Z);
                    sum1 += Vector3d.Dot(vTarget, v);

                    Vector3d vSource = new Vector3d(pointsSourceShift.Vectors[j].X, pointsSourceShift.Vectors[j].Y, pointsSourceShift.Vectors[j].Z);
                    sum2 += Vector3d.Dot(vSource, vKumultiplied);
                }
                K[i, i] = 0;
                S[i, i] = sum1 / sum2;
            }

           
            return S;
        }
    }
}
