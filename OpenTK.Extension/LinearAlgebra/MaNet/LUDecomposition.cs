// original from the Java matrix package JAMA http://math.nist.gov/javanumerics/jama/
// converted to C# by Ken Johnson, added units tests, (2010)
// http://www.codeproject.com/Articles/91458/MaNet-A-matrix-library-for-NET-Rational-Computing
//adapted to use OpenTK 

using System;
using OpenTK;

namespace OpenTKExtension
{



    /// <summary>
    /// LU Decomposition of a Matrix
    /// 
    ///For an m-by-n matrix A with m >= n, the LU decomposition is an m-by-n
    ///unit lower triangular matrix L, an n-by-n upper triangular matrix U,
    ///and a permutation vector piv of length m so that A(piv,:) = L*U.
    ///If m  &lt; n, then L is m-by-m and U is m-by-n.
    ///
    /// The LU decompostion with pivoting always exists, even if the matrix is
    /// singular, so the constructor will never fail.  The primary use of the
    ///LU decomposition is in the solution of square systems of simultaneous
    ///linear equations.  This will fail if isNonsingular() returns false.
    /// </summary>
    [Serializable]
    public class LUDecomposition
    {
        /** 
          <P>
          For an m-by-n matrix A with m >= n, the LU decomposition is an m-by-n
          unit lower triangular matrix L, an n-by-n upper triangular matrix U,
          and a permutation vector piv of length m so that A(piv,:) = L*U.
          If m < n, then L is m-by-m and U is m-by-n.
          <P>
          The LU decompostion with pivoting always exists, even if the matrix is
          singular, so the constructor will never fail.  The primary use of the
          LU decomposition is in the solution of square systems of simultaneous
          linear equations.  This will fail if isNonsingular() returns false.
          */

        /** IMPORTANT WARNING
   
         */
        /* ------------------------
           Class variables
         * ------------------------ */

        /** Array for internal storage of decomposition.
        @serial internal array storage.
        */
        private float[,] LU;

        /** Row and column dimensions, and pivot sign.
        @serial column dimension.
        @serial row dimension.
        @serial pivot sign.
        */
        private int m, n, pivsign;

        /** Internal storage of pivot vector.
        @serial pivot vector.
        */
        private int[] piv;

        /* ------------------------
           Constructor
         * ------------------------ */

        /** LU Decomposition
        @param  A   Rectangular matrix
        @return     Structure to access L, U and piv.
        */

        public LUDecomposition()
        {
        }
        public Matrix3 LUDecompose(Matrix3 A)
        {
            // Use a "left-looking", dot-product, Crout/Doolittle algorithm.

            LU = A.ToFloatArray();

            m = A.RowLength();
            n = A.ColumnLength();
            piv = new int[m];
            for (int i = 0; i < m; i++)
            {
                piv[i] = i;
            }
            pivsign = 1;
            //float[] LUrowi;
            //float[] LUcolj = new float[m];

            // Outer loop.

            for (int j = 0; j < n; j++)
            {

                // Make a copy of the j-th column to localize references.

                //for (int i = 0; i < m; i++) 
                //{
                //   LUcolj[i] = LU[i,j];
                //}

                // Apply previous transformations.

                for (int i = 0; i < m; i++)
                {


                    // Most of the time is spent in the following dot product.

                    int kmax = Math.Min(i, j);
                    float s = 0.0f;
                    for (int k = 0; k < kmax; k++)
                    {
                        s += LU[i, k] * LU[k, i];
                    }

                    LU[j, i] = LU[i, j] -= s;
                }

                // Find pivot and exchange if necessary.

                int p = j;
                for (int i = j + 1; i < m; i++)
                {
                    if (Math.Abs(LU[i, j]) > Math.Abs(LU[p, j]))
                    {
                        p = i;
                    }
                }
                if (p != j)
                {
                    for (int k = 0; k < n; k++)
                    {
                        float t = LU[p, k]; LU[p, k] = LU[j, k]; LU[j, k] = t;
                    }
                    int k2 = piv[p]; piv[p] = piv[j]; piv[j] = k2;
                    pivsign = -pivsign;
                }

                // Compute multipliers.

                if (j < m && LU[j, j] != 0)
                {
                    for (int i = j + 1; i < m; i++)
                    {
                        LU[i, j] /= LU[j, j];
                    }
                }
            }
            Matrix3 mat = new Matrix3();
            mat = mat.FromFloatArray(LU);
            return mat;
        }

        /* ------------------------
           Temporary, experimental code.
           ------------------------ *\

           \** LU Decomposition, computed by Gaussian elimination.
           <P>
           This constructor computes L and U with the "daxpy"-based elimination
           algorithm used in LINPACK and MATLAB.  In Java, we suspect the dot-product,
           Crout algorithm will be faster.  We have temporarily included this
           constructor until timing experiments confirm this suspicion.
           <P>
           @param  A             Rectangular matrix
           @param  linpackflag   Use Gaussian elimination.  Actual value ignored.
           @return               Structure to access L, U and piv.
           *\

           public LUDecomposition (Matrix A, int linpackflag) {
              // Initialize.
              LU = A.getArrayCopy();
              m = A.RowLength();
              n = A.ColumnLength();
              piv = new int[m];
              for (int i = 0; i < m; i++) {
                 piv[i] = i;
              }
              pivsign = 1;
              // Main loop.
              for (int k = 0; k < n; k++) {
                 // Find pivot.
                 int p = k;
                 for (int i = k+1; i < m; i++) {
                    if (Math.Abs(LU[i,k]) > Math.Abs(LU[p,k])) {
                       p = i;
                    }
                 }
                 // Exchange if necessary.
                 if (p != k) {
                    for (int j = 0; j < n; j++) {
                       float t = LU[p,j]; LU[p,j] = LU[k,j]; LU[k,j] = t;
                    }
                    int t = piv[p]; piv[p] = piv[k]; piv[k] = t;
                    pivsign = -pivsign;
                 }
                 // Compute multipliers and eliminate k-th column.
                 if (LU[k,k] != 0) {
                    for (int i = k+1; i < m; i++) {
                       LU[i,k] /= LU[k,k];
                       for (int j = k+1; j < n; j++) {
                          LU[i,j] -= LU[i,k]*LU[k,j];
                       }
                    }
                 }
              }
           }

        \* ------------------------
           End of temporary code.
         * ------------------------ */

        /* ------------------------
           Public Methods
         * ------------------------ */

        /** Is the matrix nonsingular?
        @return     true if U, and hence A, is nonsingular.
        */

        public bool IsNonsingular()
        {
            if (m != n) return false;// Added to deal with fat and skinny matrices
            for (int j = 0; j < n; j++)
            {
                if (LU[j, j] == 0)
                    return false;
            }
            return true;
        }

        /** Return lower triangular factor
        @return     L
        */

        public Matrix3 GetL()
        {
            if (m >= n) //For correct Dimensions with square or skinny matrices
            {
                Matrix3 X = new Matrix3();
                float[,] L = X.ToFloatArray();
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (i > j)
                        {
                            L[i, j] = LU[i, j];
                        }
                        else if (i == j)
                        {
                            L[i, j] = 1.0f;
                        }
                        else
                        {
                            L[i, j] = 0.0f;
                        }
                    }
                }
                return X;
            }
            else// For when n > m, Fat matrix
            {
                Matrix3 X = new Matrix3();
                float[,] L = X.ToFloatArray();
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < m; j++)
                    {
                        if (i > j)
                        {
                            L[i, j] = LU[i, j];
                        }
                        else if (i == j)
                        {
                            L[i, j] = 1.0f;
                        }
                        else
                        {
                            L[i, j] = 0.0f;
                        }
                    }
                }
                return X;

            }
        }

        /** Return upper triangular factor
        @return     U
        */

        public Matrix3 GetU()
        {
            //For correct Dimensions with Fat matrices
            if (m >= n)
            {
                Matrix3 X = new Matrix3();
                float[,] U = X.ToFloatArray();
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (i <= j)
                        {
                            U[i, j] = LU[i, j];
                        }
                        else
                        {
                            U[i, j] = 0.0f;
                        }
                    }
                }
                return X;
            }
            else // this case added for when n > m
            {
                Matrix3 X = new Matrix3();
                float[,] U = X.ToFloatArray();
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (i <= j)
                        {
                            U[i, j] = LU[i, j];
                        }
                        else
                        {
                            U[i, j] = 0.0f;
                        }
                    }
                }
                return X;

            }
        }

        /** Return pivot permutation vector
        @return     piv
        */

        public int[] getPivot()
        {
            int[] p = new int[m];
            for (int i = 0; i < m; i++)
            {
                p[i] = piv[i];
            }
            return p;
        }

        /// <summary>
        /// Returns the Pivot Permutation matrix such that L*U = P*A
        /// </summary>
        /// <returns> Pivot Permutation matrix</returns>
        public Matrix3 GetP()
        {

            int[] pivots = getPivot();
            Matrix3 X = new Matrix3();
            for (int i = 0; i < pivots.Length; i++)
            {
                X[i, pivots[i]] = 1;
            }
            return X;
        }

        /** Return pivot permutation vector as a one-dimensional float array
        @return     (float) piv
        */

        public float[] getfloatPivot()
        {
            float[] vals = new float[m];
            for (int i = 0; i < m; i++)
            {
                vals[i] = (float)piv[i];
            }
            return vals;
        }

        /** Determinant
        @return     det(A)
        @exception  System.ArgumentException  Matrix must be square
        */

        public float det()
        {
            if (m != n)
            {
                throw new System.ArgumentException("Matrix must be square.");
            }
            float d = (float)pivsign;
            for (int j = 0; j < n; j++)
            {
                d *= LU[j, j];
            }
            return d;
        }

        /** Solve A*X = B
        @param  B   A Matrix with as many rows as A and any number of columns.
        @return     X so that L*U*X = B(piv,:)
        @exception  System.ArgumentException Matrix row dimensions must agree.
        @exception  System.Exception  Matrix is singular.
        */

        public Matrix3 solve(Matrix3 B)
        {
            if (B.RowLength() != m)
            {
                throw new ArgumentException("Matrix row dimensions must agree.");
            }
            if (!this.IsNonsingular())
            {
                throw new Exception("Matrix is singular.");
            }

            // Copy right hand side with pivoting
            int nx = B.ColumnLength();
            Matrix3 Xmat = B.GetMatrix(piv, 0, nx - 1);
            float[,] X = Xmat.ToFloatArray();

            // Solve L*Y = B(piv,:)
            for (int k = 0; k < n; k++)
            {
                for (int i = k + 1; i < n; i++)
                {
                    for (int j = 0; j < nx; j++)
                    {
                        X[i, j] -= X[k, j] * LU[i, k];
                    }
                }
            }
            // Solve U*X = Y;
            for (int k = n - 1; k >= 0; k--)
            {
                for (int j = 0; j < nx; j++)
                {
                    X[k, j] /= LU[k, k];
                }
                for (int i = 0; i < k; i++)
                {
                    for (int j = 0; j < nx; j++)
                    {
                        X[i, j] -= X[k, j] * LU[i, k];
                    }
                }
            }
            return Xmat;
        }
    }

}
