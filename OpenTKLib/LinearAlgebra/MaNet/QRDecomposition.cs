// original from the Java matrix package JAMA http://math.nist.gov/javanumerics/jama/
// converted to C# by Ken Johnson, added units tests, (2010)
// http://www.codeproject.com/Articles/91458/MaNet-A-matrix-library-for-NET-Rational-Computing
//adapted to use OpenTK 

using System;
using OpenTK;

namespace OpenTKExtension
{

    
    /// <summary>
    /// QR Decomposition.
    /// For an m-by-n matrix A with m >= n, the QR decomposition is an m-by-n
    ///  orthogonal matrix Q and an n-by-n upper triangular matrix R so that
    ///  A = Q*R.
    ///  
    /// The QR decompostion always exists, even if the matrix does not have
    ///      full rank, so the constructor will never fail.  The primary use of the
    ///     QR decomposition is in the least squares solution of nonsquare systems
    ///     of simultaneous linear equations.  This will fail if isFullRank()
    ///      returns false.
    /// </summary>
    [Serializable]
    public class QRDecomposition
    {
        /** QR Decomposition.
        <P>
           For an m-by-n matrix A with m >= n, the QR decomposition is an m-by-n
           orthogonal matrix Q and an n-by-n upper triangular matrix R so that
           A = Q*R.
        <P>
           The QR decompostion always exists, even if the matrix does not have
           full rank, so the constructor will never fail.  The primary use of the
           QR decomposition is in the least squares solution of nonsquare systems
           of simultaneous linear equations.  This will fail if isFullRank()
           returns false.
        */
        /* ------------------------
           Class variables
         * ------------------------ */

        /** Array for internal storage of decomposition.
        @serial internal array storage.
        */
        private float[,] QR;

        /** Row and column dimensions.
        @serial column dimension.
        @serial row dimension.
        */
        private int m, n;

        /** Array for internal storage of diagonal of R.
        @serial diagonal of R.
        */
        private float[] Rdiag;



        private float mRankTolerance;

        /// <summary>
        /// How close a value needs to be to zero in order to be considered zero for the purpose of 
        /// full rank determination;
        /// </summary>
        public float RankTolerance
        {
            get { return mRankTolerance; }
            set { mRankTolerance = value; }
        }



        /* ------------------------
           Constructor
         * ------------------------ */


        ///<summary>QR Decomposition, computed by Householder reflections.</summary>
        ///<param name="A">Rectangular matrix</param>
        ///<returns>Structure to access R and the Householder vectors and compute Q.</returns>
        public QRDecomposition(Matrix3 A)
        {
            // Initialize.
            QR = A.ToFloatArray();
            m = A.RowLength();
            n = A.ColumnLength();
            Rdiag = new float[n];

            // Main loop.
            for (int k = 0; k < n; k++)
            {
                // Compute 2-norm of k-th column without under/overflow.
                float nrm = 0;
                for (int i = k; i < m; i++)
                {
                    nrm = Convert.ToSingle(Maths.Hypot(nrm, QR[i, k]));
                }

                if (nrm != 0)
                {
                    // Form k-th Householder vector.
                    if (QR[k, k] < 0)
                    {
                        nrm = -nrm;
                    }
                    for (int i = k; i < m; i++)
                    {
                        QR[i, k] /= nrm;
                    }
                    QR[k, k] += 1.0f;

                    // Apply transformation to remaining columns.
                    for (int j = k + 1; j < n; j++)
                    {
                        float s = 0.0f;
                        for (int i = k; i < m; i++)
                        {
                            s += QR[i, k] * QR[i, j];
                        }
                        s = -s / QR[k, k];
                        for (int i = k; i < m; i++)
                        {
                            QR[i, j] += s * QR[i, k];
                        }
                    }
                }
                Rdiag[k] = -nrm;
            }
        }

        /* ------------------------
           Public Methods
         * ------------------------ */


        ///<summary> Is the matrix full rank?</summary>
        ///<remarks>It is mathematically true that one can find the rank by checking the diagonal values of R, 
        ///and on an machine using some sort of infinite precision numerics it would be dependable. However on 
        ///a machine with rounding errors it is easy to get situation where the result is supposed to be zero 
        ///but comes across as something like 1.0E-16. Be careful!</remarks>
        ///<returns>true if R, and hence A, has full rank.</returns>
        public bool IsFullRank()
        {
            for (int j = 0; j < n; j++)
            {
                //if (Rdiag[j] == 0) return false; original 
                if (Math.Abs(Rdiag[j]) <= mRankTolerance) return false; //kj version
            }
            return true;
        }


        ///<summary>Return the Householder vectors</summary>
        ///<returns>Lower trapezoidal matrix whose columns define the reflections</returns>
        public Matrix3 GetH()
        {
            Matrix3 X = new Matrix3();
            float[,] H = X.ToFloatArray();
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i >= j)
                    {
                        H[i, j] = QR[i, j];
                    }
                    else
                    {
                        H[i, j] = 0.0f;
                    }
                }
            }
            return X;
        }


        ///<summary>Return the upper triangular factor</summary>
        public Matrix3 GetR()
        {
            Matrix3 X = new Matrix3();
            float[,] R = X.ToFloatArray();
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i < j)
                    {
                        R[i, j] = QR[i, j];
                    }
                    else if (i == j)
                    {
                        R[i, j] = Rdiag[i];
                    }
                    else
                    {
                        R[i, j] = 0.0f;
                    }
                }
            }
            return X;
        }


        ///<summary>Generate and return the (economy-sized) orthogonal factor</summary>
        ///<returns>Q</returns>
        public Matrix3 GetQ()
        {
            Matrix3 X = new Matrix3();
            float[,] Q = X.ToFloatArray();
            for (int k = n - 1; k >= 0; k--)
            {
                for (int i = 0; i < m; i++)
                {
                    Q[i, k] = 0.0f;
                }
                Q[k, k] = 1.0f;
                for (int j = k; j < n; j++)
                {
                    if (QR[k, k] != 0)
                    {
                        float s = 0.0f;
                        for (int i = k; i < m; i++)
                        {
                            s += QR[i, k] * Q[i, j];
                        }
                        s = -s / QR[k, k];
                        for (int i = k; i < m; i++)
                        {
                            Q[i, j] += s * QR[i, k];
                        }
                    }
                }
            }
            return X;
        }


        ///<Sumary>Least squares solution of A*X = B</Sumary>
        ///<param name="B">A Matrix with as many rows as A and any number of columns.</param>
        ///<returns> X that minimizes the two norm of Q*R*X-B.</returns>
        public Matrix3 Solve(Matrix3 B)
        {
            if (B.RowLength() != m)
            {
                throw new ArgumentException("Matrix row dimensions must agree.");
            }
            if (!IsFullRank())
            {
                throw new Exception("Matrix is rank deficient.");
            }

            // Copy right hand side
            int nx = B.ColumnLength();
            float[,] X = B.ToFloatArray();

            // Compute Y = transpose(Q)*B
            for (int k = 0; k < n; k++)
            {
                for (int j = 0; j < nx; j++)
                {
                    float s = 0.0f;
                    for (int i = k; i < m; i++)
                    {
                        s += QR[i, k] * X[i, j];
                    }
                    s = -s / QR[k, k];
                    for (int i = k; i < m; i++)
                    {
                        X[i, j] += s * QR[i, k];
                    }
                }
            }
            // Solve R*X = Y;
            for (int k = n - 1; k >= 0; k--)
            {
                for (int j = 0; j < nx; j++)
                {
                    X[k, j] /= Rdiag[k];
                }
                for (int i = 0; i < k; i++)
                {
                    for (int j = 0; j < nx; j++)
                    {
                        X[i, j] -= X[k, j] * QR[i, k];
                    }
                }
            }
            Matrix3 mat = new Matrix3();
            mat = mat.FromFloatArray(X);

            return mat.GetMatrix(0, n - 1, 0, nx - 1);
        }
    }


}
