using System;
using System.Runtime.Serialization;

namespace DotNetMatrix
{
	
	/// <summary>.NET GeneralMatrix class.
	/// 
	/// The .NET GeneralMatrix Class provides the fundamental operations of numerical
	/// linear algebra.  Various constructors create Matrices from two dimensional
	/// arrays of float precision floating point numbers.  Various "gets" and
	/// "sets" provide access to submatrices and matrix elements.  Several methods 
	/// implement basic matrix arithmetic, including matrix addition and
	/// multiplication, matrix norms, and element-by-element array operations.
	/// Methods for reading and printing matrices are also included.  All the
	/// operations in this version of the GeneralMatrix Class involve real matrices.
	/// Complex matrices may be handled in a future version.
	/// 
	/// Five fundamental matrix decompositions, which consist of pairs or triples
	/// of matrices, permutation vectors, and the like, produce results in five
	/// decomposition classes.  These decompositions are accessed by the GeneralMatrix
	/// class to compute solutions of simultaneous linear equations, determinants,
	/// inverses and other matrix functions.  The five decompositions are:
	/// <P><UL>
	/// <LI>Cholesky Decomposition of symmetric, positive definite matrices.
	/// <LI>LU Decomposition of rectangular matrices.
	/// <LI>QR Decomposition of rectangular matrices.
	/// <LI>Singular Value Decomposition of rectangular matrices.
	/// <LI>Eigenvalue Decomposition of both symmetric and nonsymmetric square matrices.
	/// </UL>
	/// <DL>
	/// <DT><B>Example of use:</B></DT>
	/// <P>
	/// <DD>Solve a linear system A x = b and compute the residual norm, ||b - A x||.
	/// <P><PRE>
	/// float[][] vals = {{1.,2.,3},{4.,5.,6.},{7.,8.,10.}};
	/// GeneralMatrix A = new GeneralMatrix(vals);
	/// GeneralMatrix b = GeneralMatrix.Random(3,1);
	/// GeneralMatrix x = A.Solve(b);
	/// GeneralMatrix r = A.Multiply(x).Subtract(b);
	/// float rnorm = r.NormInf();
	/// </PRE></DD>
	/// </DL>
	/// </summary>
	/// <author>  
	/// The MathWorks, Inc. and the National Institute of Standards and Technology.
	/// </author>
	/// <version>  5 August 1998
	/// </version>
	
	[Serializable]
	public class GeneralMatrix : System.ICloneable, System.Runtime.Serialization.ISerializable, System.IDisposable
	{
		#region Class variables
		
		/// <summary>Array for internal storage of elements.
		/// @serial internal array storage.
		/// </summary>
		private float[][] A;
		
		/// <summary>Row and column dimensions.
		/// @serial row dimension.
		/// @serial column dimension.
		/// </summary>
		private int m, n;

		#endregion //  Class variables
		
		#region Constructors
		
		/// <summary>Construct an m-by-n matrix of zeros. </summary>
		/// <param name="m">   Number of rows.
		/// </param>
		/// <param name="n">   Number of colums.
		/// </param>
		
		public GeneralMatrix(int m, int n)
		{
			this.m = m;
			this.n = n;
			A = new float[m][];
			for (int i = 0; i < m; i++)
			{
				A[i] = new float[n];
			}
		}
		
		/// <summary>Construct an m-by-n constant matrix.</summary>
		/// <param name="m">   Number of rows.
		/// </param>
		/// <param name="n">   Number of colums.
		/// </param>
		/// <param name="s">   Fill the matrix with this scalar value.
		/// </param>
		
		public GeneralMatrix(int m, int n, float s)
		{
			this.m = m;
			this.n = n;
			A = new float[m][];
			for (int i = 0; i < m; i++)
			{
				A[i] = new float[n];
			}
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					A[i][j] = s;
				}
			}
		}
		
		/// <summary>Construct a matrix from a 2-D array.</summary>
		/// <param name="A">   Two-dimensional array of floats.
		/// </param>
		/// <exception cref="System.ArgumentException">   All rows must have the same length
		/// </exception>
		/// <seealso cref="Create">
		/// </seealso>
		
		public GeneralMatrix(float[][] A)
		{
			m = A.Length;
			n = A[0].Length;
			for (int i = 0; i < m; i++)
			{
				if (A[i].Length != n)
				{
					throw new System.ArgumentException("All rows must have the same length.");
				}
			}
			this.A = A;
		}
		
		/// <summary>Construct a matrix quickly without checking arguments.</summary>
		/// <param name="A">   Two-dimensional array of floats.
		/// </param>
		/// <param name="m">   Number of rows.
		/// </param>
		/// <param name="n">   Number of colums.
		/// </param>
		
		public GeneralMatrix(float[][] A, int m, int n)
		{
			this.A = A;
			this.m = m;
			this.n = n;
		}
		
		/// <summary>Construct a matrix from a one-dimensional packed array</summary>
		/// <param name="vals">One-dimensional array of floats, packed by columns (ala Fortran).
		/// </param>
		/// <param name="m">   Number of rows.
		/// </param>
		/// <exception cref="System.ArgumentException">   Array length must be a multiple of m.
		/// </exception>
		
		public GeneralMatrix(float[] vals, int m)
		{
			this.m = m;
			n = (m != 0?vals.Length / m:0);
			if (m * n != vals.Length)
			{
				throw new System.ArgumentException("Array length must be a multiple of m.");
			}
			A = new float[m][];
			for (int i = 0; i < m; i++)
			{
				A[i] = new float[n];
			}
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
                    A[i][j] = vals[j + i * m];
				}
			}
		}
		#endregion //  Constructors

		
		#region Public Properties
		/// <summary>Access the internal two-dimensional array.</summary>
		/// <returns>     Pointer to the two-dimensional array of matrix elements.
		/// </returns>
		virtual public float[][] Array
		{			
			get
			{
				return A;
			}
		}
		/// <summary>Copy the internal two-dimensional array.</summary>
		/// <returns>     Two-dimensional array copy of matrix elements.
		/// </returns>
		virtual public float[][] ArrayCopy
		{
			get
			{
				float[][] C = new float[m][];
				for (int i = 0; i < m; i++)
				{
					C[i] = new float[n];
				}
				for (int i = 0; i < m; i++)
				{
					for (int j = 0; j < n; j++)
					{
						C[i][j] = A[i][j];
					}
				}
				return C;
			}
			
		}
		/// <summary>Make a one-dimensional column packed copy of the internal array.</summary>
		/// <returns>     Matrix elements packed in a one-dimensional array by columns.
		/// </returns>
		virtual public float[] ColumnPackedCopy
		{
			get
			{
				float[] vals = new float[m * n];
				for (int i = 0; i < m; i++)
				{
					for (int j = 0; j < n; j++)
					{
						vals[i + j * m] = A[i][j];
					}
				}
				return vals;
			}
			
		}

		/// <summary>Make a one-dimensional row packed copy of the internal array.</summary>
		/// <returns>     Matrix elements packed in a one-dimensional array by rows.
		/// </returns>
		virtual public float[] RowPackedCopy
		{
			get
			{
				float[] vals = new float[m * n];
				for (int i = 0; i < m; i++)
				{
					for (int j = 0; j < n; j++)
					{
						vals[i * n + j] = A[i][j];
					}
				}
				return vals;
			}
		}

		/// <summary>Get row dimension.</summary>
		/// <returns>     m, the number of rows.
		/// </returns>
		virtual public int RowDimension
		{
			get
			{
				return m;
			}
		}

		/// <summary>Get column dimension.</summary>
		/// <returns>     n, the number of columns.
		/// </returns>
		virtual public int ColumnDimension
		{
			get
			{
				return n;
			}
		}
		#endregion   // Public Properties
		
		#region	 Public Methods
		
		/// <summary>Construct a matrix from a copy of a 2-D array.</summary>
		/// <param name="A">   Two-dimensional array of floats.
		/// </param>
		/// <exception cref="System.ArgumentException">   All rows must have the same length
		/// </exception>
		
		public static GeneralMatrix Create(float[][] A)
		{
			int m = A.Length;
			int n = A[0].Length;
			GeneralMatrix X = new GeneralMatrix(m, n);
			float[][] C = X.Array;
			for (int i = 0; i < m; i++)
			{
				if (A[i].Length != n)
				{
					throw new System.ArgumentException("All rows must have the same length.");
				}
				for (int j = 0; j < n; j++)
				{
					C[i][j] = A[i][j];
				}
			}
			return X;
		}
		
		/// <summary>Make a deep copy of a matrix</summary>
		
		public virtual GeneralMatrix Copy()
		{
			GeneralMatrix X = new GeneralMatrix(m, n);
			float[][] C = X.Array;
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					C[i][j] = A[i][j];
				}
			}
			return X;
		}
		
		/// <summary>Get a single element.</summary>
		/// <param name="i">   Row index.
		/// </param>
		/// <param name="j">   Column index.
		/// </param>
		/// <returns>     A(i,j)
		/// </returns>
		/// <exception cref="System.IndexOutOfRangeException">  
		/// </exception>
        /// <summary>
        /// Gets or sets the value at a specified row and column.
        /// </summary>
        public float this[int rowIndex, int columnIndex]
        {
            get
            {
               
                if(rowIndex < this.RowDimension && columnIndex < this.ColumnDimension)
                {
                    return A[rowIndex][columnIndex];
                }
                throw new IndexOutOfRangeException("You tried to access this matrix at: (" + rowIndex + ", " + columnIndex + ")");
            }
            set
            {
                if (rowIndex < this.RowDimension && columnIndex < this.ColumnDimension)
                {
                    A[rowIndex][columnIndex] = value;
                    return;
                }
                else throw new IndexOutOfRangeException("You tried to set this matrix at: (" + rowIndex + ", " + columnIndex + ")");
            }
        }

		public virtual float GetElement(int i, int j)
		{
			return A[i][j];
		}
		
		/// <summary>Get a submatrix.</summary>
		/// <param name="i0">  Initial row index
		/// </param>
		/// <param name="i1">  Final row index
		/// </param>
		/// <param name="j0">  Initial column index
		/// </param>
		/// <param name="j1">  Final column index
		/// </param>
		/// <returns>     A(i0:i1,j0:j1)
		/// </returns>
		/// <exception cref="System.IndexOutOfRangeException">   Submatrix indices
		/// </exception>
		
		public virtual GeneralMatrix GetMatrix(int i0, int i1, int j0, int j1)
		{
			GeneralMatrix X = new GeneralMatrix(i1 - i0 + 1, j1 - j0 + 1);
			float[][] B = X.Array;
			try
			{
				for (int i = i0; i <= i1; i++)
				{
					for (int j = j0; j <= j1; j++)
					{
						B[i - i0][j - j0] = A[i][j];
					}
				}
			}
			catch (System.IndexOutOfRangeException e)
			{
				throw new System.IndexOutOfRangeException("Submatrix indices", e);
			}
			return X;
		}
		
		/// <summary>Get a submatrix.</summary>
		/// <param name="r">   Array of row indices.
		/// </param>
		/// <param name="c">   Array of column indices.
		/// </param>
		/// <returns>     A(r(:),c(:))
		/// </returns>
		/// <exception cref="System.IndexOutOfRangeException">   Submatrix indices
		/// </exception>
		
		public virtual GeneralMatrix GetMatrix(int[] r, int[] c)
		{
			GeneralMatrix X = new GeneralMatrix(r.Length, c.Length);
			float[][] B = X.Array;
			try
			{
				for (int i = 0; i < r.Length; i++)
				{
					for (int j = 0; j < c.Length; j++)
					{
						B[i][j] = A[r[i]][c[j]];
					}
				}
			}
			catch (System.IndexOutOfRangeException e)
			{
				throw new System.IndexOutOfRangeException("Submatrix indices", e);
			}
			return X;
		}
		
		/// <summary>Get a submatrix.</summary>
		/// <param name="i0">  Initial row index
		/// </param>
		/// <param name="i1">  Final row index
		/// </param>
		/// <param name="c">   Array of column indices.
		/// </param>
		/// <returns>     A(i0:i1,c(:))
		/// </returns>
		/// <exception cref="System.IndexOutOfRangeException">   Submatrix indices
		/// </exception>
		
		public virtual GeneralMatrix GetMatrix(int i0, int i1, int[] c)
		{
			GeneralMatrix X = new GeneralMatrix(i1 - i0 + 1, c.Length);
			float[][] B = X.Array;
			try
			{
				for (int i = i0; i <= i1; i++)
				{
					for (int j = 0; j < c.Length; j++)
					{
						B[i - i0][j] = A[i][c[j]];
					}
				}
			}
			catch (System.IndexOutOfRangeException e)
			{
				throw new System.IndexOutOfRangeException("Submatrix indices", e);
			}
			return X;
		}
		
		/// <summary>Get a submatrix.</summary>
		/// <param name="r">   Array of row indices.
		/// </param>
		/// <param name="j0">  Initial column index
		/// </param>
		/// <param name="j1">  Final column index
		/// </param>
		/// <returns>     A(r(:),j0:j1)
		/// </returns>
		/// <exception cref="System.IndexOutOfRangeException">   Submatrix indices
		/// </exception>
		
		public virtual GeneralMatrix GetMatrix(int[] r, int j0, int j1)
		{
			GeneralMatrix X = new GeneralMatrix(r.Length, j1 - j0 + 1);
			float[][] B = X.Array;
			try
			{
				for (int i = 0; i < r.Length; i++)
				{
					for (int j = j0; j <= j1; j++)
					{
						B[i][j - j0] = A[r[i]][j];
					}
				}
			}
			catch (System.IndexOutOfRangeException e)
			{
				throw new System.IndexOutOfRangeException("Submatrix indices", e);
			}
			return X;
		}
		
		
		/// <summary>Set a submatrix.</summary>
		/// <param name="i0">  Initial row index
		/// </param>
		/// <param name="i1">  Final row index
		/// </param>
		/// <param name="j0">  Initial column index
		/// </param>
		/// <param name="j1">  Final column index
		/// </param>
		/// <param name="X">   A(i0:i1,j0:j1)
		/// </param>
		/// <exception cref="System.IndexOutOfRangeException">  Submatrix indices
		/// </exception>
		
		public virtual void  SetMatrix(int i0, int i1, int j0, int j1, GeneralMatrix X)
		{
			try
			{
				for (int i = i0; i <= i1; i++)
				{
					for (int j = j0; j <= j1; j++)
					{
						A[i][j] = X.GetElement(i - i0, j - j0);
					}
				}
			}
			catch (System.IndexOutOfRangeException e)
			{
				throw new System.IndexOutOfRangeException("Submatrix indices", e);
			}
		}
		
		/// <summary>Set a submatrix.</summary>
		/// <param name="r">   Array of row indices.
		/// </param>
		/// <param name="c">   Array of column indices.
		/// </param>
		/// <param name="X">   A(r(:),c(:))
		/// </param>
		/// <exception cref="System.IndexOutOfRangeException">  Submatrix indices
		/// </exception>
		
		public virtual void  SetMatrix(int[] r, int[] c, GeneralMatrix X)
		{
			try
			{
				for (int i = 0; i < r.Length; i++)
				{
					for (int j = 0; j < c.Length; j++)
					{
						A[r[i]][c[j]] = X.GetElement(i, j);
					}
				}
			}
			catch (System.IndexOutOfRangeException e)
			{
				throw new System.IndexOutOfRangeException("Submatrix indices", e);
			}
		}
		
		/// <summary>Set a submatrix.</summary>
		/// <param name="r">   Array of row indices.
		/// </param>
		/// <param name="j0">  Initial column index
		/// </param>
		/// <param name="j1">  Final column index
		/// </param>
		/// <param name="X">   A(r(:),j0:j1)
		/// </param>
		/// <exception cref="System.IndexOutOfRangeException"> Submatrix indices
		/// </exception>
		
		public virtual void  SetMatrix(int[] r, int j0, int j1, GeneralMatrix X)
		{
			try
			{
				for (int i = 0; i < r.Length; i++)
				{
					for (int j = j0; j <= j1; j++)
					{
						A[r[i]][j] = X.GetElement(i, j - j0);
					}
				}
			}
			catch (System.IndexOutOfRangeException e)
			{
				throw new System.IndexOutOfRangeException("Submatrix indices", e);
			}
		}
		
		/// <summary>Set a submatrix.</summary>
		/// <param name="i0">  Initial row index
		/// </param>
		/// <param name="i1">  Final row index
		/// </param>
		/// <param name="c">   Array of column indices.
		/// </param>
		/// <param name="X">   A(i0:i1,c(:))
		/// </param>
		/// <exception cref="System.IndexOutOfRangeException">  Submatrix indices
		/// </exception>
		
		public virtual void  SetMatrix(int i0, int i1, int[] c, GeneralMatrix X)
		{
			try
			{
				for (int i = i0; i <= i1; i++)
				{
					for (int j = 0; j < c.Length; j++)
					{
						A[i][c[j]] = X.GetElement(i - i0, j);
					}
				}
			}
			catch (System.IndexOutOfRangeException e)
			{
				throw new System.IndexOutOfRangeException("Submatrix indices", e);
			}
		}
		
		/// <summary>Matrix transpose.</summary>
		/// <returns>    A'
		/// </returns>
		
		public virtual GeneralMatrix Transpose()
		{
			GeneralMatrix X = new GeneralMatrix(n, m);
			float[][] C = X.Array;
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					C[j][i] = A[i][j];
				}
			}
			return X;
		}
		
		/// <summary>One norm</summary>
		/// <returns>    maximum column sum.
		/// </returns>
		
		public virtual float Norm1()
		{
			float f = 0;
			for (int j = 0; j < n; j++)
			{
				float s = 0;
				for (int i = 0; i < m; i++)
				{
					s += System.Math.Abs(A[i][j]);
				}
				f = System.Math.Max(f, s);
			}
			return f;
		}
		
		/// <summary>Two norm</summary>
		/// <returns>    maximum singular value.
		/// </returns>
		
        //public virtual float Norm2()
        //{
        //    return (new SingularValueDecomposition(this).Norm2());
        //}
		
		/// <summary>Infinity norm</summary>
		/// <returns>    maximum row sum.
		/// </returns>
		
		public virtual float NormInf()
		{
			float f = 0;
			for (int i = 0; i < m; i++)
			{
				float s = 0;
				for (int j = 0; j < n; j++)
				{
					s += System.Math.Abs(A[i][j]);
				}
				f = System.Math.Max(f, s);
			}
			return f;
		}
		
		/// <summary>Frobenius norm</summary>
		/// <returns>    sqrt of sum of squares of all elements.
		/// </returns>
		
        //public virtual float NormF()
        //{
        //    float f = 0;
        //    for (int i = 0; i < m; i++)
        //    {
        //        for (int j = 0; j < n; j++)
        //        {
        //            f = MatrixMath.NormChecked(f, A[i][j]);
        //        }
        //    }
        //    return f;
        //}
		
		/// <summary>Unary minus</summary>
		/// <returns>    -A
		/// </returns>
		
		public virtual GeneralMatrix UnaryMinus()
		{
			GeneralMatrix X = new GeneralMatrix(m, n);
			float[][] C = X.Array;
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					C[i][j] = -A[i][j];
				}
			}
			return X;
		}
		
		/// <summary>C = A + B</summary>
		/// <param name="B">   another matrix
		/// </param>
		/// <returns>     A + B
		/// </returns>
		
		public virtual GeneralMatrix Add(GeneralMatrix B)
		{
			CheckMatrixDimensions(B);
			GeneralMatrix X = new GeneralMatrix(m, n);
			float[][] C = X.Array;
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					C[i][j] = A[i][j] + B.A[i][j];
				}
			}
			return X;
		}
		
		/// <summary>A = A + B</summary>
		/// <param name="B">   another matrix
		/// </param>
		/// <returns>     A + B
		/// </returns>
		
		public virtual GeneralMatrix AddEquals(GeneralMatrix B)
		{
			CheckMatrixDimensions(B);
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					A[i][j] = A[i][j] + B.A[i][j];
				}
			}
			return this;
		}
		
		/// <summary>C = A - B</summary>
		/// <param name="B">   another matrix
		/// </param>
		/// <returns>     A - B
		/// </returns>
		
		public virtual GeneralMatrix Subtract(GeneralMatrix B)
		{
			CheckMatrixDimensions(B);
			GeneralMatrix X = new GeneralMatrix(m, n);
			float[][] C = X.Array;
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					C[i][j] = A[i][j] - B.A[i][j];
				}
			}
			return X;
		}
		
		/// <summary>A = A - B</summary>
		/// <param name="B">   another matrix
		/// </param>
		/// <returns>     A - B
		/// </returns>
		
		public virtual GeneralMatrix SubtractEquals(GeneralMatrix B)
		{
			CheckMatrixDimensions(B);
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					A[i][j] = A[i][j] - B.A[i][j];
				}
			}
			return this;
		}
		
		/// <summary>Element-by-element multiplication, C = A.*B</summary>
		/// <param name="B">   another matrix
		/// </param>
		/// <returns>     A.*B
		/// </returns>
		
		public virtual GeneralMatrix ArrayMultiply(GeneralMatrix B)
		{
			CheckMatrixDimensions(B);
			GeneralMatrix X = new GeneralMatrix(m, n);
			float[][] C = X.Array;
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					C[i][j] = A[i][j] * B.A[i][j];
				}
			}
			return X;
		}
		
		/// <summary>Element-by-element multiplication in place, A = A.*B</summary>
		/// <param name="B">   another matrix
		/// </param>
		/// <returns>     A.*B
		/// </returns>
		
		public virtual GeneralMatrix ArrayMultiplyEquals(GeneralMatrix B)
		{
			CheckMatrixDimensions(B);
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					A[i][j] = A[i][j] * B.A[i][j];
				}
			}
			return this;
		}
		
		/// <summary>Element-by-element right division, C = A./B</summary>
		/// <param name="B">   another matrix
		/// </param>
		/// <returns>     A./B
		/// </returns>
		
		public virtual GeneralMatrix ArrayRightDivide(GeneralMatrix B)
		{
			CheckMatrixDimensions(B);
			GeneralMatrix X = new GeneralMatrix(m, n);
			float[][] C = X.Array;
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					C[i][j] = A[i][j] / B.A[i][j];
				}
			}
			return X;
		}
		
		/// <summary>Element-by-element right division in place, A = A./B</summary>
		/// <param name="B">   another matrix
		/// </param>
		/// <returns>     A./B
		/// </returns>
		
		public virtual GeneralMatrix ArrayRightDivideEquals(GeneralMatrix B)
		{
			CheckMatrixDimensions(B);
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					A[i][j] = A[i][j] / B.A[i][j];
				}
			}
			return this;
		}
		
		/// <summary>Element-by-element left division, C = A.\B</summary>
		/// <param name="B">   another matrix
		/// </param>
		/// <returns>     A.\B
		/// </returns>
		
		public virtual GeneralMatrix ArrayLeftDivide(GeneralMatrix B)
		{
			CheckMatrixDimensions(B);
			GeneralMatrix X = new GeneralMatrix(m, n);
			float[][] C = X.Array;
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					C[i][j] = B.A[i][j] / A[i][j];
				}
			}
			return X;
		}
		
		/// <summary>Element-by-element left division in place, A = A.\B</summary>
		/// <param name="B">   another matrix
		/// </param>
		/// <returns>     A.\B
		/// </returns>
		
		public virtual GeneralMatrix ArrayLeftDivideEquals(GeneralMatrix B)
		{
			CheckMatrixDimensions(B);
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					A[i][j] = B.A[i][j] / A[i][j];
				}
			}
			return this;
		}
		
		/// <summary>Multiply a matrix by a scalar, C = s*A</summary>
		/// <param name="s">   scalar
		/// </param>
		/// <returns>     s*A
		/// </returns>
		
		public virtual GeneralMatrix Multiply(float s)
		{
			GeneralMatrix X = new GeneralMatrix(m, n);
			float[][] C = X.Array;
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					C[i][j] = s * A[i][j];
				}
			}
			return X;
		}
		
		/// <summary>Multiply a matrix by a scalar in place, A = s*A</summary>
		/// <param name="s">   scalar
		/// </param>
		/// <returns>     replace A by s*A
		/// </returns>
		
		public virtual GeneralMatrix MultiplyEquals(float s)
		{
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					A[i][j] = s * A[i][j];
				}
			}
			return this;
		}
		
		/// <summary>Linear algebraic matrix multiplication, A * B</summary>
		/// <param name="B">   another matrix
		/// </param>
		/// <returns>     Matrix product, A * B
		/// </returns>
		/// <exception cref="System.ArgumentException">  Matrix inner dimensions must agree.
		/// </exception>
		
		public virtual GeneralMatrix Multiply(GeneralMatrix B)
		{
			if (B.m != n)
			{
				throw new System.ArgumentException("GeneralMatrix inner dimensions must agree.");
			}
			GeneralMatrix X = new GeneralMatrix(m, B.n);
			float[][] C = X.Array;
			float[] Bcolj = new float[n];
			for (int j = 0; j < B.n; j++)
			{
				for (int k = 0; k < n; k++)
				{
					Bcolj[k] = B.A[k][j];
				}
				for (int i = 0; i < m; i++)
				{
					float[] Arowi = A[i];
					float s = 0;
					for (int k = 0; k < n; k++)
					{
						s += Arowi[k] * Bcolj[k];
					}
					C[i][j] = s;
				}
			}
			return X;
		}
		
		#region Operator Overloading

		/// <summary>
		///  Addition of matrices
		/// </summary>
		/// <param name="m1"></param>
		/// <param name="m2"></param>
		/// <returns></returns>
		public static GeneralMatrix operator +(GeneralMatrix m1, GeneralMatrix m2) 
		{ 
			return m1.Add(m2); 
		} 

		/// <summary>
		/// Subtraction of matrices
		/// </summary>
		/// <param name="m1"></param>
		/// <param name="m2"></param>
		/// <returns></returns>
		public static GeneralMatrix operator -(GeneralMatrix m1, GeneralMatrix m2) 
		{ 
			return m1.Subtract(m2); 
		} 

		/// <summary>
		/// Multiplication of matrices
		/// </summary>
		/// <param name="m1"></param>
		/// <param name="m2"></param>
		/// <returns></returns>
		public static GeneralMatrix operator *(GeneralMatrix m1, GeneralMatrix m2) 
		{ 
			return m1.Multiply(m2); 
		} 

		#endregion   //Operator Overloading

		/// <summary>LU Decomposition</summary>
		/// <returns>     LUDecomposition
		/// </returns>
		/// <seealso cref="LUDecomposition">
		/// </seealso>
		
        //public virtual LUDecomposition LUD()
        //{
        //    return new LUDecomposition(this);
        //}
		
        ///// <summary>QR Decomposition</summary>
        ///// <returns>     QRDecomposition
        ///// </returns>
        ///// <seealso cref="QRDecomposition">
        ///// </seealso>
		
        //public virtual QRDecomposition QRD()
        //{
        //    return new QRDecomposition(this);
        //}
		
        ///// <summary>Cholesky Decomposition</summary>
        ///// <returns>     CholeskyDecomposition
        ///// </returns>
        ///// <seealso cref="CholeskyDecomposition">
        ///// </seealso>

        //public virtual CholeskyDecomposition chol()
        //{
        //    return new CholeskyDecomposition(this);
        //}
		
        ///// <summary>Singular Value Decomposition</summary>
        ///// <returns>     SingularValueDecomposition
        ///// </returns>
        ///// <seealso cref="SingularValueDecomposition">
        ///// </seealso>
		
        //public virtual SingularValueDecomposition SVD()
        //{
        //    return new SingularValueDecomposition(this);
        //}
		
        ///// <summary>Eigenvalue Decomposition</summary>
        ///// <returns>     EigenvalueDecomposition
        ///// </returns>
        ///// <seealso cref="EigenvalueDecomposition">
        ///// </seealso>
		
        //public virtual EigenvalueDecomposition Eigen()
        //{
        //    return new EigenvalueDecomposition(this);
        //}
		
        ///// <summary>Solve A*X = B</summary>
        ///// <param name="B">   right hand side
        ///// </param>
        ///// <returns>     solution if A is square, least squares solution otherwise
        ///// </returns>
		
        //public virtual GeneralMatrix Solve(GeneralMatrix B)
        //{
        //    return (m == n ? (new LUDecomposition(this)).Solve(B):(new QRDecomposition(this)).Solve(B));
        //}
		
        ///// <summary>Solve X*A = B, which is also A'*X' = B'</summary>
        ///// <param name="B">   right hand side
        ///// </param>
        ///// <returns>     solution if A is square, least squares solution otherwise.
        ///// </returns>
		
        //public virtual GeneralMatrix SolveTranspose(GeneralMatrix B)
        //{
        //    return Transpose().Solve(B.Transpose());
        //}
		
        ///// <summary>Matrix inverse or pseudoinverse</summary>
        ///// <returns>     inverse(A) if A is square, pseudoinverse otherwise.
        ///// </returns>
		
        //public virtual GeneralMatrix Inverse()
        //{
        //    return Solve(Identity(m, m));
        //}
		
		/// <summary>GeneralMatrix determinant</summary>
		/// <returns>     determinant
		/// </returns>
		
        //public virtual float Determinant()
        //{
        //    return new LUDecomposition(this).Determinant();
        //}
		
        ///// <summary>GeneralMatrix rank</summary>
        ///// <returns>     effective numerical rank, obtained from SVD.
        ///// </returns>
		
        //public virtual int Rank()
        //{
        //    return new SingularValueDecomposition(this).Rank();
        //}
		
		/// <summary>Matrix condition (2 norm)</summary>
		/// <returns>     ratio of largest to smallest singular value.
		/// </returns>
		
        //public virtual float Condition()
        //{
        //    return new SingularValueDecomposition(this).Condition();
        //}
		
		/// <summary>Matrix trace.</summary>
		/// <returns>     sum of the diagonal elements.
		/// </returns>
		
		public virtual float Trace()
		{
			float t = 0;
			for (int i = 0; i < System.Math.Min(m, n); i++)
			{
				t += A[i][i];
			}
			return t;
		}
		
		/// <summary>Generate matrix with random elements</summary>
		/// <param name="m">   Number of rows.
		/// </param>
		/// <param name="n">   Number of colums.
		/// </param>
		/// <returns>     An m-by-n matrix with uniformly distributed random elements.
		/// </returns>
		
		public static GeneralMatrix Random(int m, int n)
		{
			System.Random random = new System.Random();

			GeneralMatrix A = new GeneralMatrix(m, n);
			float[][] X = A.Array;
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					X[i][j] = Convert.ToSingle(random.NextDouble());
				}
			}
			return A;
		}
		
		/// <summary>Generate identity matrix</summary>
		/// <param name="m">   Number of rows.
		/// </param>
		/// <param name="n">   Number of colums.
		/// </param>
		/// <returns>     An m-by-n matrix with ones on the diagonal and zeros elsewhere.
		/// </returns>
		
		public static GeneralMatrix Identity(int m, int n)
		{
			GeneralMatrix A = new GeneralMatrix(m, n);
			float[][] X = A.Array;
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					X[i][j] = (i == j ? 1.0f : 0.0f);
				}
			}
			return A;
		}		
		
		#endregion //  Public Methods

		#region	 Private Methods
		
		/// <summary>Check if size(A) == size(B) *</summary>
		
		private void  CheckMatrixDimensions(GeneralMatrix B)
		{
			if (B.m != m || B.n != n)
			{
				throw new System.ArgumentException("GeneralMatrix dimensions must agree.");
			}
		}
		#endregion //  Private Methods

		#region Implement IDisposable
		/// <summary>
		/// Do not make this method virtual.
		/// A derived class should not be able to override this method.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
		}

		/// <summary>
		/// Dispose(bool disposing) executes in two distinct scenarios.
		/// If disposing equals true, the method has been called directly
		/// or indirectly by a user's code. Managed and unmanaged resources
		/// can be disposed.
		/// If disposing equals false, the method has been called by the 
		/// runtime from inside the finalizer and you should not reference 
		/// other objects. Only unmanaged resources can be disposed.
		/// </summary>
		/// <param name="disposing"></param>
		private void Dispose(bool disposing)
		{
			// This object will be cleaned up by the Dispose method.
			// Therefore, you should call GC.SupressFinalize to
			// take this object off the finalization queue 
			// and prevent finalization code for this object
			// from executing a second time.
			if (disposing)
                GC.SuppressFinalize(this);
		}

		/// <summary>
		/// This destructor will run only if the Dispose method 
		/// does not get called.
		/// It gives your base class the opportunity to finalize.
		/// Do not provide destructors in types derived from this class.
		/// </summary>
		~GeneralMatrix()      
		{
			// Do not re-create Dispose clean-up code here.
			// Calling Dispose(false) is optimal in terms of
			// readability and maintainability.
			Dispose(false);
		}
		#endregion //  Implement IDisposable

		/// <summary>Clone the GeneralMatrix object.</summary>
		public System.Object Clone()
		{
			return this.Copy();
		}
		
		/// <summary>
		/// A method called when serializing this class
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) 
		{
		}
	}
}