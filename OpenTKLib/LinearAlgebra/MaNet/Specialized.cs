using System;
using OpenTK;

namespace OpenTKExtension
{
    public class Specialized
    {

        public static Matrix3 Toeplitz(float[] firstRow)
        {

            Matrix3 A = new Matrix3();

            for (int i = 1; i < firstRow.Length; i++)
            {
                float[] row = new float[firstRow.Length];
                for (int j = 0; j < row.Length; j++)
                {

                    A[i, j] = firstRow[Math.Abs(j - i)];
                }

            }




            return A;

        }

        public static Matrix3 Stiffness(int dimension)
        {
            if (dimension < 2) throw new Exception("Matrix only defined for dimension 2 and above");
            float[] array = new float[dimension];
            array[0] = 2;
            array[1] = -1;
            return Toeplitz(array);

        }

        public static Matrix3 K(int dimension)
        {
            return Stiffness(dimension);

        }

        public static Matrix3 Circulant(int dimension)
        {
            if (dimension < 2) throw new Exception("Matrix only defined for dimension 3 and above");
            Matrix3 A = K(dimension);
            A[0, dimension - 1] = -1;
            A[dimension - 1, 0] = -1;
            return A;
        }


        public static Matrix3 C(int dimension)
        {
            return Circulant(dimension);
        }


        public static Matrix3 T(int dimension)
        {
            if (dimension < 2) throw new Exception("Matrix only defined for dimension 2 and above");
            float[] array = new float[dimension];
            array[0] = 2;
            array[1] = -1;
            Matrix3 A = Toeplitz(array);
            A[0, 0] = 1;
            return A;

        }

        public static Matrix3 B(int dimension)
        {
            if (dimension < 2) throw new Exception("Matrix only defined for dimension 2 and above");
            float[] array = new float[dimension];
            array[0] = 2;
            array[1] = -1;
            Matrix3 A = Toeplitz(array);
            A[0, 0] = 1;
            A[dimension - 1, dimension - 1] = 1;
            return A;

        }



        /// <summary>
        /// Returns the Rosser matrix, a famous 8 by eight matrix which many algorithms have trouble with.
        /// </summary>
        /// <returns>Rosser Matrix</returns>
        public static Matrix3 Rosser()
        {

            string strRosser = @" 611   196  -192   407    -8   -52   -49    29
                               196   899   113  -192   -71   -43    -8   -44
                              -192   113   899   196    61    49     8    52
                               407  -192   196   611     8    44    59   -23
                                -8   -71    61     8   411  -599   208   208
                               -52   -43    49    44  -599   411   208   208
                               -49    -8     8    59   208   208    99  -911
                                29   -44    52   -23   208   208  -911    99";

            Matrix3 mat = new Matrix3();
            return mat.Parse(strRosser);

        }


    }
}
