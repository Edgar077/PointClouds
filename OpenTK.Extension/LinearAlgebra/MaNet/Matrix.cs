// original from the Java matrix package JAMA http://math.nist.gov/javanumerics/jama/
// converted to C# by Ken Johnson, added units tests, (2010)
// http://www.codeproject.com/Articles/91458/MaNet-A-matrix-library-for-NET-Rational-Computing
//adapted to use OpenTK 

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Data;
namespace OpenTKExtension.old
{
   
    /// <summary>
   ///The Java Matrix Class provides the fundamental operations of numerical
   ///linear algebra.  Various constructors create Matrices from two dimensional
   ///arrays of float precision floating point numbers.  Various "gets" and
   ///"sets" provide access to submatrices and matrix elements.  Several methods 
   ///implement basic matrix arithmetic, including matrix addition and
   ///multiplication, matrix norms, and element-by-element array operations.
   ///Methods for reading and printing matrices are also included.  All the
   ///operations in this version of the Matrix Class involve real matrices.
    /// </summary>
    [Serializable]
public class MatrixOld
    {

/** Original introduction
   Jama = Java Matrix class.
<P>
   The Java Matrix Class provides the fundamental operations of numerical
   linear algebra.  Various constructors create Matrices from two dimensional
   arrays of float precision floating point numbers.  Various "gets" and
   "sets" provide access to submatrices and matrix elements.  Several methods 
   implement basic matrix arithmetic, including matrix addition and
   multiplication, matrix norms, and element-by-element array operations.
   Methods for reading and printing matrices are also included.  All the
   operations in this version of the Matrix Class involve real matrices.
   Complex matrices may be handled in a future version.
<P>
   Five fundamental matrix decompositions, which consist of pairs or triples
   of matrices, permutation vectors, and the like, produce results in five
   decomposition classes.  These decompositions are accessed by the Matrix
   class to compute solutions of simultaneous linear equations, determinants,
   inverses and other matrix functions.  The five decompositions are:
<P><UL>
   <LI>Cholesky Decomposition of symmetric, positive definite matrices.
   <LI>LU Decomposition of rectangular matrices.
   <LI>QR Decomposition of rectangular matrices.
   <LI>Singular Value Decomposition of rectangular matrices.
   <LI>Eigenvalue Decomposition of both symmetric and nonsymmetric square matrices.
</UL>
<DL>
<DT><B>Example of use:</B></DT>
<P>
<DD>Solve a linear system A x = b and compute the residual norm, ||b - A x||.
<P><PRE>
      float[,] vals = {{1.,2.,3},{4.,5.,6.},{7.,8.,10.}};
      Matrix A = new Matrix(vals);
      Matrix b = Matrix.random(3,1);
      Matrix x = A.solve(b);
      Matrix r = A.times(x).minus(b);
      float rnorm = r.normInf();
</PRE></DD>
</DL>

@author The MathWorks, Inc. and the National Institute of Standards and Technology.
@version 5 August 1998
*/





/* ------------------------
   Class variables
 * ------------------------ */
        
   /** Array for internal storage of elements.
   @serial internal array storage.
   */
   private float[,] A;

   /** Row and column dimensions.
   @serial row dimension.
   @serial column dimension.
   */
   private int m, n;

 
   #region Constructors 
   ///<summary>Construct an m-by-n matrix of zeros</summary>
    ///<param name="m">Number of rows.</param>
    ///<param name="n">Number of colums</param>
   public MatrixOld (int m, int n) 
   {
      this.m = m;
      this.n = n;
      A = new float[n,m];
     
   }

   /// <summary>
   /// Construct an m-by-M matrix of zeros
   /// </summary>
   /// <param name="m">Number of rows and columns</param>
   public MatrixOld(int m):this(m,m)
   {

   }

   ///<summary>Construct an m-by-n constant matrix.</summary>
   ///<param name="m">Number of rows.</param>
   ///<param name="n">Number of colums.</param>
   ///<param name="s">Fill the matrix with this scalar value.</param>
   public MatrixOld(int m, int n, float s)
   {
      this.m = m;
      this.n = n;
      A = new float[m,n];
      for (int i = 0; i < m; i++) 
      {
         
         for (int j = 0; j < n; j++) {
            A[i,j] = s;
         }
      }
   }
 

 
   /////<summary>Construct a matrix quickly without checking arguments.</summary>    
   /////<param name="A">Two-dimensional array of floats.</param>
   /////<param name="m">Number of rows.</param>
   /////<param name="n">Number of colums.</param>
   //public Matrix3 (float[,] A, int m, int n) {
   //   this.A = A;
   //   this.m = m;
   //   this.n = n;
   //}

 
   /////<summary>Construct a matrix from a one-dimensional packed array</summary>
   /////<param name="vals">One-dimensional array of floats, packed by columns (ala Fortran).</param>
   /////<param name="m">Number of rows.</param>
   //public Matrix3 (float[] vals, int m) 
   //{
   //   this.m = m;
   //   n = (m != 0 ? vals.Length/m : 0);
   //   if (m*n != vals.Length) {
   //      throw new System.ArgumentException("Array length must be a multiple of m.");
   //   }
   //   A = new float[m,];
   //   for (int i = 0; i < m; i++) {
   //       A[i] = new float[n]; //Added by KJ
   //      for (int j = 0; j < n; j++) {
   //         A[i,j] = vals[i+j*m];
   //      }
   //   }
   //}


   #endregion

  




   /* ------------------------
   Public Methods
 * ------------------------ */

  



 

 

  /////<summary>Copy the internal two-dimensional array.</summary>
  /////<returns>Two-dimensional array copy of matrix elements.</returns>
  // public float[,] ArrayCopy ()
  // {
  //    float[,] C = new float[m,n];
  //    for (int i = 0; i < m; i++) 
  //    {
  //       for (int j = 0; j < n; j++) 
  //       {
  //          C[i,j] = A[i,j];
  //       }
  //    }
  //    return C;
  // }

 

   ///<summary>Make a one-dimensional column packed copy of the internal array.</summary>
   ///<returns>Matrix elements packed in a one-dimensional array by columns.</returns>
   public float[] ColumnPackedCopy () {
      float[] vals = new float[m*n];
      for (int i = 0; i < m; i++) {
         for (int j = 0; j < n; j++) {
            vals[i+j*m] = A[i,j];
         }
      }
      return vals;
   }


   ///<summary>Make a one-dimensional row packed copy of the internal array.</summary>
   ///<returns>Matrix elements packed in a one-dimensional array by rows.</returns>
   public float[] RowPackedCopy () {
      float[] vals = new float[m*n];
      for (int i = 0; i < m; i++) {
         for (int j = 0; j < n; j++) {
            vals[i*n+j] = A[i,j];
         }
      }
      return vals;
   }

 
   
   
 
///<summary>Get a single element</summary>
///<param name="i" >Row index. </param>
///<param name="j"> Column index.</param>
///<returns> A(i,j)</returns>
   public float Get (int i, int j) 
   {
      return A[i,j];
   }



   ///<summary>Set a single element.</summary>
   ///<param name="i">Row index.</param>
   ///<param name="j">Column index.</param>
   ///<param name="s">A(i,j).</param>
   public void Set (int i, int j, float s) 
   {
      A[i,j] = s;
   }

   /** Set a submatrix.
   @param i0   Initial row index
   @param i1   Final row index
   @param j0   Initial column index
   @param j1   Final column index
   @param X    A(i0:i1,j0:j1)
   @exception  System.IndexOutOfRangeException Submatrix indices
   */

   //public void SetMatrix (int i0, int i1, int j0, int j1, Matrix3 X) {
   //   try {
   //      for (int i = i0; i <= i1; i++) {
   //         for (int j = j0; j <= j1; j++) {
   //            A[i,j] = X[i-i0,j-j0];
   //         }
   //      }
   //   } catch(IndexOutOfRangeException e) {
   //      throw new IndexOutOfRangeException("Submatrix indices");
   //   }
   //}

   /////<summary>Set a submatrix.</summary>
   /////<param name="r">Array of row indices.</param>
   /////<param name="c">Array of column indices.</param>
   /////<param name="X">A(r(:),c(:))</param>
   //public void SetMatrix (int[] r, int[] c, Matrix3 X) {
   //   try {
   //      for (int i = 0; i < r.Length; i++) {
   //         for (int j = 0; j < c.Length; j++) {
   //            A[r[i],c[j]] = X[i,j];
   //         }
   //      }
   //   } catch(IndexOutOfRangeException) {
   //      throw new IndexOutOfRangeException("Submatrix indices");
   //   }
   //}


   /////<summary>Set a submatrix</summary>
   /////<param name="r">Array of row indices.</param>
   /////<param name="j0">Initial column index</param>
   /////<param name="j1">Final column index</param>
   /////<param name="X">A(r(:),j0:j1)</param>
   //public void SetMatrix (int[] r, int j0, int j1, Matrix3 X) {
   //   try {
   //      for (int i = 0; i < r.Length; i++) {
   //         for (int j = j0; j <= j1; j++) {
   //            A[r[i],j] = X[i,j-j0];
   //         }
   //      }
   //   } catch(IndexOutOfRangeException) {
   //      throw new IndexOutOfRangeException("Submatrix indices");
   //   }
   //}

 
   /////<summary>Set a submatrix</summary>
   /////<param name="i0">Initial row index</param>
   /////<param name="i1">Final row index</param>
   /////<param name="c">Array of column indices</param>
   /////<param name="X">A(i0:i1,c(:))</param>
   //public void SetMatrix (int i0, int i1, int[] c, Matrix3 X) 
   //{
   //   try {
   //      for (int i = i0; i <= i1; i++) {
   //         for (int j = 0; j < c.Length; j++) {
   //            A[i,c[j]] = X[i-i0,j];
   //         }
   //      }
   //   } catch(IndexOutOfRangeException e) {
   //      throw new IndexOutOfRangeException("Submatrix indices");
   //   }
   //}



///<summary>One norm</summary>
///<returns>maximum column sum</returns>
   public float Norm1 () 
   {
      float f = 0;
      for (int j = 0; j < n; j++) 
      {
         float s = 0;
         for (int i = 0; i < m; i++)
         {
            s += Math.Abs(A[i,j]);
         }
         f = Math.Max(f,s);
      }
      return f;
   }

 


 ///<summary>Infinity norm</summary>
  ///<returns> maximum row sum.</returns>
   public float NormInf () 
   {
      float f = 0;
      for (int i = 0; i < m; i++) {
         float s = 0;
         for (int j = 0; j < n; j++) {
            s += Math.Abs(A[i,j]);
         }
         f = Math.Max(f,s);
      }
      return f;
   }


///<summary>Frobenius norm</summary>
///<returns>sqrt of sum of squares of all elements.</returns>
   public float NormF () 
   {
      float f = 0;
      for (int i = 0; i < m; i++)
      {
         for (int j = 0; j < n; j++) 
         {
            f = Maths.Hypot(f,A[i,j]);
         }
      }
      return f;
   }

 
 
///<summary>A = A + B</summary>
///<param name="B">another matrix</param>
///<returns> A + B</returns>
   public MatrixOld PlusEquals (MatrixOld B)
   {
      CheckMatrixDimensions(B);
      for (int i = 0; i < m; i++) 
      {
         for (int j = 0; j < n; j++) 
         {
            A[i,j] = A[i,j] + B.A[i,j];
         }
      }
      return this;
   }

 


   ///<summary>A = A - B</summary>
   ///<param name="B">another matrix</param>
   ///<returns> A - B</returns>
   public MatrixOld MinusEquals (MatrixOld B) {
      CheckMatrixDimensions(B);
      for (int i = 0; i < m; i++) {
         for (int j = 0; j < n; j++) {
            A[i,j] = A[i,j] - B.A[i,j];
         }
      }
      return this;
   }

  


   ///<summary>Element-by-element multiplication in place, A = A.*B</summary>
   ///<param name="B">another matrix</param>
   ///<returns>A.*B</returns>
   public MatrixOld ArrayTimesEquals (MatrixOld B) {
      CheckMatrixDimensions(B);
      for (int i = 0; i < m; i++) {
         for (int j = 0; j < n; j++) {
            A[i,j] = A[i,j] * B.A[i,j];
         }
      }
      return this;
   }


  

   ///<summary>Element-by-element right division in place, A = A./B</summary>
   ///<param name="B">another matrix</param>
   ///<returns>A./B</returns>
   public MatrixOld ArrayRightDivideEquals (MatrixOld B) {
      CheckMatrixDimensions(B);
      for (int i = 0; i < m; i++) {
         for (int j = 0; j < n; j++) {
            A[i,j] = A[i,j] / B.A[i,j];
         }
      }
      return this;
   }



 
   ///<summary>Element-by-element left division in place, A = A.\B</summary>
   ///<param name="B">another matrix</param>
   ///<returns>A.\B</returns>
   public MatrixOld ArrayLeftDivideEquals (MatrixOld B) {
      CheckMatrixDimensions(B);
      for (int i = 0; i < m; i++) {
         for (int j = 0; j < n; j++) {
            A[i,j] = B.A[i,j] / A[i,j];
         }
      }
      return this;
   }


  

 
   ///<summary>Multiply a matrix by a scalar in place, A = s*A</summary>
   ///<param name="s">scalar</param>
   ///<returns>replace A by s*A</returns>
   public MatrixOld TimesEquals (float s)
   {
      for (int i = 0; i < m; i++) {
         for (int j = 0; j < n; j++) {
            A[i,j] = s*A[i,j];
         }
      }
      return this;
   }
 
      

///// <summary>
///// Parses strig to 
///// </summary>
///// <param name="str"></param>
///// <returns></returns>
//public static Matrix Parse(string str)
//   {
//       string working = str.Trim();

//       if (str.StartsWith("["))
//       {
//           return ParseMatLab(str);
//       }
//       else if (str.StartsWith("{"))
//       {
//           return ParseMathematica(str);
//       }
//       else
//       {
//              System.IO.StringReader reader = new System.IO.StringReader(str);
//               Matrix mat = Load(reader);
//               reader.Close();
//               reader.Dispose();
//               return mat;
//       }
//   }



//

/* ------------------------
   Private Methods
 * ------------------------ */

   /** Check if size(A) == size(B) **/

   private void CheckMatrixDimensions (MatrixOld B) {
      if (B.m != m || B.n != n) {
         throw new System.ArgumentException("Matrix dimensions must agree.");
      }
   }




   #region IEnumerable<float[]> Members

   public IEnumerator<float[]> GetEnumerator()
   {
       throw new NotImplementedException();
   }

   #endregion

  
    }
}
