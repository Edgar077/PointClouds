using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTKExtension;

namespace OpenTKExtension
{
    public class SVD_Float
    {
        public static Matrix3 U = new Matrix3();
        public static Matrix3 VT = new Matrix3();
        public static Matrix3 R = new Matrix3();
        public static Vector3 EV;
       



   
        /// <summary>
        /// //gives R, the rotation matrix, U - the left eigenvectors, VT - right transposed eigenvectors, EV  - the eigenvalues
        /// </summary>
        /// <param name="H"></param>
        /// <param name="pointsSourceTranslated"></param>
        public static void Eigenvalues_Helper(Matrix3 H)
        {
            //ALGLIB_Method = true;
            Eigenvalues_Helper_Alglib(H);
            CheckSVD(H, Convert.ToSingle(1E-3));

        
        }
        /// <summary>
        /// //gives R, the rotation matrix, U - the left eigenvectors, VT - right transposed eigenvectors, EV  - the eigenvalues
        /// </summary>
        /// <param name="H"></param>
        public static void Eigenvalues_Helper_Alglib(Matrix3 H)
        {
            double[,] Harray = H.ToDoubleArray();
            double[,] Uarray = new double[3, 3];
            double[,] VTarray = new double[3, 3];
            double[] EVarray = new double[3];


            alglib.svd.rmatrixsvd(Harray, 3, 3, 2, 2, 2, ref EVarray, ref Uarray, ref VTarray);
            
            U = U.FromDoubleArray(Uarray);
            VT = VT.FromDoubleArray(VTarray);
            EV = EV.FromDoubleArray(EVarray);

            R = Matrix3.Mult(U, VT);


        }
        /// <summary>
        /// //gives R, the rotation matrix, U - the left eigenvectors, VT - right transposed eigenvectors, EV  - the eigenvalues
        /// </summary>
        /// <param name="H"></param>
        public static void Eigenvalues_Helper_VTK(Matrix3 H)
        {
            double[,] Harray = H.ToDoubleArray();
            double[,] Uarray = new double[3, 3];
            double[,] VTarray = new double[3, 3];
            double[] EVarray = new double[3];

            //alglib.svd.rmatrixsvd(Harray, 3, 3, 2, 2, 2, ref EVarray, ref Uarray, ref VTarray);
            csharpMath.SingularValueDecomposition3x3(Harray, Uarray, EVarray, VTarray);

                        
            //U = new Matrix3();
            U = U.FromDoubleArray(Uarray);

            U = U.FromDoubleArray(Uarray);
            VT = VT.FromDoubleArray(VTarray);
            EV = EV.FromDoubleArray(EVarray);

            R = Matrix3.Mult(U, VT);

        }
      
        private static Matrix3 CalculateRotationBySingularValueDecomposition(Matrix3 H, PointCloud pointsSourceTranslated, ICP_VersionUsed icpVersionUsed)
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
            Matrix3 K2 = Matrix3.Identity;
            
            //float check = U.Determinant * VT.Determinant;
            //if (check < 0 && Math.Abs(check) > 1E-3)
            //{
            //    K2[2, 2] = -1;
            //}
            
         
            float scale = CalculateScale_Umeyama(pointsSourceTranslated, EV, K2);
            R = Matrix3.Mult(R, K2);
            if (icpVersionUsed == ICP_VersionUsed.Umeyama)
            {
                //R = Matrix3.Mult(R, K2);
                R = R.MultiplyScalar(scale);
            }

            
            return R;
        }
        private static void CheckSVD(Matrix3 H, float threshold)
        {
            //check : H = U * EV * VT

            Matrix3 EVMat = new Matrix3();
            EVMat = EVMat.FromVector(EV);
            
            //check
            // EV * VT
            Matrix3 test = Matrix3.Mult(EVMat, VT);
            // U * EV * VT
            test = Matrix3.Mult(U, test);

            test.CompareMatrices(H, threshold);



        }
        private static void CheckDiagonalizationResult_Base()
        {

            Matrix3 UT = Matrix3.Transpose(U);
            Matrix3 V = Matrix3.Transpose(VT);
          
            Matrix3 mat_checkShouldGiveI = Matrix3.Mult(UT, U);
            mat_checkShouldGiveI = Matrix3.Mult(VT, V);


            Matrix3 RT = Matrix3.Transpose(R);
            mat_checkShouldGiveI = Matrix3.Mult(RT, R);
            
        }

        public static Matrix3 FindRotationMatrix(PointCloud pointsSourceTranslated, PointCloud pointsTargetTranslated, ICP_VersionUsed icpVersionUsed)
        {

            try
            {
                Matrix3 H = PointCloud.CorrelationMatrix(pointsSourceTranslated, pointsTargetTranslated);
                Matrix3 R = CalculateRotationBySingularValueDecomposition(H, pointsSourceTranslated, icpVersionUsed);

                //now scaling factor:
                float c;
                if (icpVersionUsed == ICP_VersionUsed.Zinsser)
                {
                    c = CalculateScale_Zinsser(pointsSourceTranslated, pointsTargetTranslated, ref R);
                }
                if (icpVersionUsed == ICP_VersionUsed.Du)
                {
                    Matrix3 C = CalculateScale_Du(pointsSourceTranslated, pointsTargetTranslated, R);
                    R = Matrix3.Mult(R, C);
                }

                return R;
            }
            catch(Exception err)
            {
                System.Windows.Forms.MessageBox.Show("Error in finding rotation matrix: " + err.Message);
                return Matrix3.Identity;
            }
            
        }
        public static Matrix4 FindTransformationMatrix(PointCloud pointsSource, PointCloud pointsTarget, ICP_VersionUsed icpVersionUsed)
        {


            //shift points to the center of mass (centroid) 
            Vector3 centroidTarget = pointsTarget.CentroidVector;
            PointCloud pointsTargetTranslated = pointsTarget.Clone();
            pointsTargetTranslated.SubtractVector(centroidTarget);

            Vector3 centroidSource = pointsSource.CentroidVector;
            PointCloud pointsSourceTranslated = pointsSource.Clone();
            pointsSourceTranslated.SubtractVector(centroidSource);

            Matrix3 R = FindRotationMatrix(pointsSourceTranslated, pointsTargetTranslated, icpVersionUsed);
            
            Vector3 T = SVD_Float.CalculateTranslation(centroidSource, centroidTarget, R);
            Matrix4 myMatrix = new Matrix4();
            myMatrix = myMatrix.PutTheMatrix4together(T, R);
          
            return myMatrix;

        }
        public static Matrix4 FindTransformationMatrix_WithoutCentroids(PointCloud pointsSource, PointCloud pointsTarget, ICP_VersionUsed icpVersionUsed)
        {

            Matrix3 R = FindRotationMatrix(pointsSource, pointsTarget, icpVersionUsed);

            //Vector3 T = new Vector3();
           

            Matrix4 myMatrix = new Matrix4();
            myMatrix = myMatrix.FromMatrix3(R);

            return myMatrix;

        }
        public static Vector3 CalculateTranslation(Vector3 centroidSource, Vector3 centroidTarget, Matrix3 Rotation)
        {

         
            Vector3 T = Rotation.MultiplyVector(centroidSource);
            T = centroidTarget - T;



            return T;

        }
      
       
        private static float CalculateScale_Umeyama(PointCloud pointsSourceShift, Vector3 eigenvalues, Matrix3 K2)
        {
            float sigmaSquared = 0f;
            for (int i = 0; i < pointsSourceShift.Vectors.Length; i++)
            {
                sigmaSquared += pointsSourceShift.Vectors[i].NormSquared();
            }

            sigmaSquared /= pointsSourceShift.Vectors.Length;

            float c = 0.0F;
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
        private static float CalculateScale_Zinsser(PointCloud pointsSourceShift, PointCloud pointsTargetShift, ref Matrix3 R)
        {
            float sum1 = 0;
            float sum2 = 0;
            Matrix3 RT = Matrix3.Transpose(R);
            Matrix3 checkT = Matrix3.Mult(RT, R);

            Matrix4 R4D = new Matrix4();
            R4D = R4D.PutTheMatrix4together(Vector3.Zero, R);

            
     
            for(int i = 0; i < pointsSourceShift.Vectors.Length; i++)
            {
                Vector3 v = Vector3.TransformNormalInverse(pointsSourceShift.Vectors[i], R4D);
                v = R.MultiplyVector(pointsSourceShift.Vectors[i]);

                sum1 += Vector3.Dot(v, pointsTargetShift.Vectors[i]);
                sum2 += Vector3.Dot(pointsSourceShift.Vectors[i], pointsSourceShift.Vectors[i]);
                
            }

            float c = sum1 / sum2;
            R = R.MultiplyScalar(c);
            return c;
        }
        private static Matrix3 CalculateScale_Du(PointCloud pointsSourceShift, PointCloud pointsTargetShift, Matrix3 R)
        {
           
            Matrix3 S = Matrix3.Identity;
            Matrix3 K = Matrix3.Identity;
            for (int i = 0; i < 3; i++)
            {
                K[i, i] = 0;
            }
            for (int i = 0; i < 3; i++)
            {
                K[i, i] = 1;
                float sum1 = 0;
                float sum2 = 0;
                for (int j = 0; j < pointsSourceShift.Vectors.Length; j++)
                {
                    Vector3 vKumultiplied = K.MultiplyVector(pointsSourceShift.Vectors[j]);
                    Vector3 v = R.MultiplyVector(vKumultiplied);
                    sum1 += Vector3.Dot(pointsTargetShift.Vectors[j], v);

                    sum2 += Vector3.Dot(pointsSourceShift.Vectors[j], vKumultiplied);
                }
                K[i, i] = 0;
                S[i, i] = sum1 / sum2;
            }

           
            return S;
        }
    }
}
