// Pogramming by
//     Douglas Andrade ( http://www.cmsoft.com.br, email: cmsoft@cmsoft.com.br)
//               Implementation of most of the functionality
//     Edgar Maass: (email: maass@logisel.de)
//               Code adaption, changed to user control
//
//Software used: 
//    OpenGL : http://www.opengl.org
//    OpenTK : http://www.opentk.com
//
// DISCLAIMER: Users rely upon this software at their own risk, and assume the responsibility for the results. Should this software or program prove defective, 
// users assume the cost of all losses, including, but not limited to, any necessary servicing, repair or correction. In no event shall the developers or any person 
// be liable for any loss, expense or damage, of any type or nature arising out of the use of, or inability to use this software or program, including, but not
// limited to, claims, suits or causes of action involving alleged infringement of copyrights, patents, trademarks, trade secrets, or unfair competition. 
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.IO;
using System.Text.RegularExpressions;
using System.Data;


namespace OpenTKExtension
{
  
    //Extensios attached to the object which folloes the "this" 
    public static class Matrix3Extension
    {
        public static Matrix3 MultiplyScalar(this Matrix3 A, float val)
        {
            Matrix3 mReturn = new Matrix3();
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    mReturn[i, j] = A[i, j] * val;
            return mReturn;

        }
        public static float[,] ToFloatArray(this Matrix3 m)
        {
            float[,] floatArray = new float[3, 3];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    floatArray[i, j] = m[i, j];

            return floatArray;
        }

        public static double[,] ToDoubleArray(this Matrix3 m)
        {
            double[,] doubleArray = new double[3, 3];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    doubleArray[i, j] = m[i, j];

            return doubleArray;
        }
        /// <summary>Transform a direction vector by the given Matrix
        /// Assumes the matrix has a bottom row of (0,0,0,1), that is the translation part is ignored.
        /// </summary>
        /// <param name="vec">The vector to transform</param>
        /// <param name="mat">The desired transformation</param>
        /// <returns>The transformed vector</returns>
        public static Vector3 MultiplyVector_Test(this Matrix3 mat, Vector3 vec)
        {
            //Vector3 vNew = new Vector3(mat.Row2);
            //float val = Vector3.Dot(vNew, vec);

            return new Vector3(
                Vector3.Dot(new Vector3(mat.Column0), vec),
                Vector3.Dot(new Vector3(mat.Column1), vec),
                Vector3.Dot(new Vector3(mat.Column2), vec));
        }
        /// <summary>Transform a direction vector by the given Matrix
        /// Assumes the matrix has a bottom row of (0,0,0,1), that is the translation part is ignored.
        /// </summary>
        /// <param name="vec">The vector to transform</param>
        /// <param name="mat">The desired transformation</param>
        /// <returns>The transformed vector</returns>
        public static Vector3 MultiplyVector(this Matrix3 mat, Vector3 vec)
        {
            //Vector3 vNew = new Vector3(mat.Row2);
            //float val = Vector3.Dot(vNew, vec);

            return new Vector3(
                Vector3.Dot(new Vector3(mat.Row0), vec),
                Vector3.Dot(new Vector3(mat.Row1), vec),
                Vector3.Dot(new Vector3(mat.Row2), vec));
        }
        /// <summary>Transform a direction vector by the given Matrix
        /// Assumes the matrix has a bottom row of (0,0,0,1), that is the translation part is ignored.
        /// </summary>
        /// <param name="vec">The vector to transform</param>
        /// <param name="mat">The desired transformation</param>
        /// <returns>The transformed vector</returns>
        public static Matrix3 MultiplyDiagonalElements(this Matrix3 mat, Vector3 vec)
        {
            //Vector3 vNew = new Vector3(mat.Row2);
            //float val = Vector3.Dot(vNew, vec);
            mat[0, 0] *= vec.X;
            mat[1, 1] *= vec.Y;
            mat[2, 2] *= vec.Z;

            return mat;
        }

        public static Matrix3 MultiplyRow(this Matrix3 mat, int N, float d)
        {
            //Vector3 vNew = new Vector3(mat.Row2);
            //float val = Vector3.Dot(vNew, vec);

            mat[0, N] *= d;
            mat[1, N] *= d;
            mat[2, N] *= d;

            return mat;
        }

        public static Matrix3 MultiplyColumn(this Matrix3 mat, int N, float d)
        {
            //Vector3 vNew = new Vector3(mat.Row2);
            //float val = Vector3.Dot(vNew, vec);

            mat[N, 0] *= d;
            mat[N, 1] *= d;
            mat[N, 2] *= d;

            return mat;
        }

        /// <summary>
        /// Rotates around x,y and z axis by the given amount in Degrees 
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static Matrix3 RotationXYZDegrees(this Matrix3 mat, float x, float y, float z)
        {
            Matrix3 Rx = Matrix3.CreateRotationX(Convert.ToSingle(Math.PI * x / 180));
            Matrix3 Ry = Matrix3.CreateRotationY(Convert.ToSingle(Math.PI * y / 180));
            Matrix3 Rz = Matrix3.CreateRotationZ(Convert.ToSingle(Math.PI * z / 180));
            Rx = Matrix3.Mult(Rx, Ry);
            Rx = Matrix3.Mult(Rx, Rz);

            return Rx;
        }
        /// <summary>
        /// Rotates around x,y and z axis by the given amount in radiants 
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static Matrix3 RotationXYZRadiants(this Matrix3 mat, float x, float y, float z)
        {
            Matrix3 Rx = Matrix3.CreateRotationX(x);
            Matrix3 Ry = Matrix3.CreateRotationY(y);
            Matrix3 Rz = Matrix3.CreateRotationZ(z);
            Rx = Matrix3.Mult(Rx, Ry);
            Rx = Matrix3.Mult(Rx, Rz);

            return Rx;
        }

        public static void WriteMatrix(this Matrix3 m)
        {
            //System.Diagnostics.Debug.WriteLine(name);

            for (int i = 0; i < 3; i++)
            {
                System.Diagnostics.Debug.WriteLine(m[i, 0].ToString("0.00") + " " + m[i, 1].ToString("0.00") + " " + m[i, 2].ToString("0.00"));

            }
        }
        public static Matrix3 FromFloatArray(this Matrix3 mat, float[,] arr)
        {


            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    mat[i, j] = arr[i, j];

            return mat;
        }
        public static Matrix3 FromDoubleArray(this Matrix3 mat, double[,] arr)
        {

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    mat[i, j] = Convert.ToSingle(arr[i, j]);

            return mat;
        }


        public static Matrix3 FromRowsList(this Matrix3 mat, List<float[]> rows)
        {


            for (int i = 0; i < rows.Count; i++)
            {
                int nJ = rows[0].Length;
                for (int j = 0; j < nJ; j++)
                {
                    mat[i, j] = rows[i][j];
                }

            }

            return mat;

        }

        public static Matrix3 Copy(this Matrix3 mat)
        {
            Matrix3 newMat = new Matrix3();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    newMat[i, j] = mat[i, j];
                }
            }
            return newMat;
        }

        //public static float[,] ToFloatArray(this Matrix3 m)
        //{
        //    float[,] floatArray = new float[3, 3];
        //    for (int i = 0; i < 3; i++)
        //        for (int j = 0; j < 3; j++)
        //            floatArray[i, j] = m[i, j];

        //    return floatArray;
        //}
        //public static double[,] ToArrayDouble(this Matrix3 m)
        //{
        //    double[,] floatArray = new double[3, 3];
        //    for (int i = 0; i < 3; i++)
        //        for (int j = 0; j < 3; j++)
        //            floatArray[i, j] = m[i, j];

        //    return floatArray;
        //}

        /// <summary>
        /// Parses strig to 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Matrix3 Parse(this Matrix3 mat, string str)
        {
            string working = str.Trim();

            if (str.StartsWith("["))
            {
                return mat.ParseMatLab(str);
            }
            else if (str.StartsWith("{"))
            {
                return mat.ParseMathematica(str);
            }
            else
            {
                System.IO.StringReader reader = new System.IO.StringReader(str);
                mat = mat.Load(reader);
                reader.Close();
                reader.Dispose();
                return mat;
            }
        }
        public static Matrix3 ParseMathematica(this Matrix3 mat, string str)
        {
            return mat.Parse(str, "{", "{", ", ", ", ", "}", "}");

        }

        /// <summary>
        /// parses a string to produce a matrix
        /// </summary>
        /// <param name="str">The string to parse</param>
        /// <param name="matrixFrontCap">placed at the beginning of the string</param>
        /// <param name="rowFrontCap">placed at the beginning of each row</param>
        /// <param name="rowDelimiter">delimiter for rows</param>
        /// <param name="columnDelimiter">delimiter for Columns</param>
        /// <param name="rowEndCap">placed at the end of each row</param>
        /// <param name="matrixEndCap">placed at the beginning of the string</param>
        /// <returns></returns>
        public static Matrix3 Parse(this Matrix3 mat, string str, string matrixFrontCap, string rowFrontCap, string rowDelimiter, string columnDelimiter, string rowEndCap, string matrixEndCap)
        {
            if (!str.StartsWith(matrixFrontCap))
            {
                throw new Exception("string does not begin with proper value: " + matrixFrontCap + " was expected, but " + str.Substring(0, matrixFrontCap.Length) + " was found.");
            }

            if (!str.StartsWith(matrixFrontCap))
            {
                throw new Exception("string does not end with proper value: " + matrixEndCap + " was expected, but " + str.Substring(str.Length - matrixEndCap.Length, matrixEndCap.Length) + " was found.");
            }

            string working = str.Substring(matrixFrontCap.Length, str.Length - matrixFrontCap.Length - matrixEndCap.Length);
            string[] rDelim = { rowEndCap + rowDelimiter + rowFrontCap };
            string[] rows = working.Split(rDelim, StringSplitOptions.RemoveEmptyEntries);
            if (rows.Length == 0) { throw new Exception("No rows present"); }

            rows[0] = rows[0].Substring(rowFrontCap.Length);
            rows[rows.Length - 1] = rows[rows.Length - 1].Substring(0, rows[rows.Length - 1].Length - rowEndCap.Length);

            float[,] matrixArray = new float[rows.Length, rows.Length];
            int columnCount = 0;
            for (int iRow = 0; iRow < rows.Length; iRow++)
            {
                string[] cols = rows[iRow].Split(new string[] { columnDelimiter }, StringSplitOptions.RemoveEmptyEntries);
                if (columnCount != 0)
                {
                    if (columnCount != cols.Length)
                    {
                        throw new Exception("Rows are not of consistant lenght");
                    }
                }
                else { columnCount = cols.Length; }

                float[] row = new float[columnCount];
                for (int iCol = 0; iCol < columnCount; iCol++)
                {
                    row[iCol] = float.Parse(cols[iCol]);
                }
                for (int i = 0; i < cols.Length; i++)
                    matrixArray[iRow, i] = row[i];
            }
            mat = mat.FromFloatArray(matrixArray);
            return mat;

        }


        /// <summary>
        /// Converts matrix to a string representation that can be cut and pasted into matlab;
        /// </summary>
        /// <returns>Mathematic formatted array</returns>
        public static string ToMatLabString(this Matrix3 mat)
        {
            return mat.ToString("[", "", ";", " ", "", "]");

        }
        /// <summary>
        /// Converts matrix to a string
        /// </summary>
        /// <param name="matrixFrontCap">placed at the beginning of the string</param>
        /// <param name="rowFrontCap">placed at the beginning of each row</param>
        /// <param name="rowDelimiter">delimiter for rows</param>
        /// <param name="columnDelimiter">delimiter for Columns</param>
        /// <param name="rowEndCap">placed at the end of each row</param>
        /// <param name="matrixEndCap">placed at the beginning of the string</param>
        /// <returns></returns>
        public static string ToString(this Matrix3 mat, string matrixFrontCap, string rowFrontCap, string rowDelimiter, string columnDelimiter, string rowEndCap, string matrixEndCap)
        {
            StringBuilder sb = new StringBuilder();  // start on new line.
            sb.Append(matrixFrontCap);
            int n = mat.ColumnLength();
            int m = mat.RowLength();

            for (int i = 0; i < m; i++)
            {
                sb.Append(rowFrontCap);
                for (int j = 0; j < n; j++)
                {

                    sb.Append(mat[i, j].ToString("R")); //Round-trip is necessary
                    sb.Append((j < n - 1) ? columnDelimiter : "");
                }
                sb.Append(rowEndCap);
                sb.Append((i < m - 1) ? rowDelimiter : "");
            }
            sb.Append(matrixEndCap);
            return sb.ToString();
        }

        /// <summary>
        /// Returns matrix as string in MatLab format
        /// </summary>
        /// <returns>string representation of Matrix</returns>
        public static string ToString(this Matrix3 mat)
        {
            return mat.ToMatLabString();
        }



        ///<summary>
        ///</summary>
        ///<param name="str">String in matlab format</param>
        ///<returns></returns>
        public static Matrix3 ParseMatLab(this Matrix3 mat, string str)
        {

            return mat.Parse(str, "[", "", ";", " ", "", "]");
        }


        public static Matrix3 Load(this Matrix3 mat, string path)
        {
            String file = File.ReadAllText(path);
            return mat.Parse(file);
        }


        /// <summary>
        /// Converts matrix to a string representation that can be cut and pasted into mathematica;
        /// </summary>
        /// <returns>Mathematic formatted array</returns>
        public static string ToMathematicaString(this Matrix3 mat)
        {
            // Output should look something like this.
            // {{0.894427190999916, -0.447213595499958}, {0.447213595499958,   0.894427190999916}}
            return mat.ToString("{", "{", ", ", ", ", "}", "}");
        }


        /// <summary>
        /// Parses multiline text input  from a textreader into a Matrix where each line 
        /// corresponsd to a row and the items on the line are space deliminted
        /// </summary>
        /// <param name="reader">The TextReader</param>
        /// <returns>The Matrix</returns>
        public static Matrix3 Load(this Matrix3 mat, TextReader reader)
        {

            List<float[]> rows = new List<float[]>();
            Regex rx = new Regex(@"\s+");
            while (reader.Peek() != -1)
            {
                string line = reader.ReadLine().Trim();

                if (line != "")
                {
                    string[] rowStrs = rx.Split(line);
                    float[] matrixRow = new float[rowStrs.Length];
                    for (int i = 0; i < rowStrs.Length; i++)
                    {
                        float val = 0;
                        if (float.TryParse(rowStrs[i], out val))
                        {

                            matrixRow[i] = val;
                        }
                        else
                        {

                            throw new ArgumentException("Invalid string");
                        }
                    }
                    rows.Add(matrixRow);
                }
            }

            if (rows.Count > 1)
            {
                //Check that all rows have the same length
                int rowSize = rows[0].Length;
                for (int i = 1; i < rows.Count; i++)
                {
                    if (rows[i].Length != rowSize)
                    {
                        throw new ArgumentException("Rows of inconsistant length");
                    }
                }

                mat = mat.FromRowsList(rows);
                return mat;
            }
            else if (rows.Count == 1)
            {

                mat = mat.FromRowsList(rows);
                return mat;
            }
            else
            {

                return mat;
            }
        }



        public static DataTable ToDataTable(this Matrix3 mat)
        {
            DataTable dt = new DataTable();
            for (int i = 0; i < mat.ColumnLength(); i++)
            {
                dt.Columns.Add("col" + i, typeof(float));
            }
            for (int iRow = 0; iRow < 3; iRow++)
            {
                DataRow dr = dt.NewRow();

                for (int iCol = 0; iCol < mat.ColumnLength(); iCol++)
                {
                    dr[iCol] = (float)mat[iCol, iRow];
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public static Matrix3 FromVector(this Matrix3 mat, Vector3 v)
        {
            mat = Matrix3.Identity;
            for (int i = 0; i < 3; i++)
            {
                mat[i, i] = v[i];
            }
            return mat;


        }
        public static Matrix3 FromDataTable(this Matrix3 mat, DataTable dt)
        {
            float[,] X = new float[dt.Rows.Count, dt.Columns.Count];
            for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
            {
                float[] row = new float[dt.Columns.Count];
                for (int iCol = 0; iCol < dt.Columns.Count; iCol++)
                {

                    X[iRow, iCol] = (float)dt.Rows[iRow][iCol];
                }


            }
            mat = mat.FromFloatArray(X);

            return mat;


        }

        ///<summary>Get a submatrix</summary>
        ///<param name="i0">Initial row index</param>
        ///<param name="i1">Final row index</param>
        ///<param name="j0">Initial column index</param>
        ///<param name="j1">Final column index</param>
        ///<returns>A(i0:i1,j0:j1)</returns>
        public static Matrix3 GetMatrix(this Matrix3 mat, int i0, int i1, int j0, int j1)
        {
            //Matrix3 X = new Matrix3(i1 - i0 + 1, j1 - j0 + 1);
            Matrix3 X = new Matrix3();
            float[,] B = X.ToFloatArray();
            try
            {
                for (int i = i0; i <= i1; i++)
                {
                    for (int j = j0; j <= j1; j++)
                    {
                        B[i - i0, j - j0] = mat[i, j];
                    }
                }
            }
            catch (IndexOutOfRangeException e)
            {
                throw new IndexOutOfRangeException("Submatrix indices: " + e.Message);
            }
            return X;
        }


        ///<summary>Get a submatrix.</summary>
        ///<param name="r">Array of row indices.</param>
        ///<param name="c">Array of column indices.</param>
        ///<returns>A(r(:),c(:))</returns>
        public static Matrix3 GetMatrix(this Matrix3 mat, int[] r, int[] c)
        {
            //Matrix3 X = new Matrix3(r.Length, c.Length);
            Matrix3 X = new Matrix3();
            float[,] B = X.ToFloatArray();
            try
            {
                for (int i = 0; i < r.Length; i++)
                {
                    for (int j = 0; j < c.Length; j++)
                    {
                        B[i, j] = mat[r[i], c[j]];
                    }
                }
            }
            catch (IndexOutOfRangeException e)
            {
                throw new IndexOutOfRangeException("Submatrix indices: " + e.Message);
            }
            return X;
        }


        ///<summary>Get a submatrix</summary>
        ///<param name="i0">Initial row index</param>
        ///<param name="i1">Final row index</param>
        ///<param name="c">Array of column indices</param>
        ///<returns>A(i0:i1,c(:))</returns>
        public static Matrix3 GetMatrix(this Matrix3 mat, int i0, int i1, int[] c)
        {
            //Matrix3 X = new Matrix3(i1 - i0 + 1, c.Length);
            Matrix3 X = new Matrix3();

            float[,] B = X.ToFloatArray();
            try
            {
                for (int i = i0; i <= i1; i++)
                {
                    for (int j = 0; j < c.Length; j++)
                    {
                        B[i - i0, j] = mat[i, c[j]];
                    }
                }
            }
            catch (IndexOutOfRangeException e)
            {
                throw new IndexOutOfRangeException("Submatrix indices: " + e.Message);
            }
            return X;
        }



        ///<summary>Get a submatrix</summary>
        ///<param name="r">Array of row indices</param>
        ///<param name="j0">Initial column index</param>
        ///<param name="j1">Final column index</param>
        ///<returns>A(r(:),j0:j1)</returns>
        public static Matrix3 GetMatrix(this Matrix3 mat, int[] r, int j0, int j1)
        {
            //Matrix3 X = new Matrix3(r.Length, j1 - j0 + 1);
            Matrix3 X = new Matrix3();

            float[,] B = X.ToFloatArray();
            try
            {
                for (int i = 0; i < r.Length; i++)
                {
                    for (int j = j0; j <= j1; j++)
                    {
                        B[i, j - j0] = mat[r[i], j];
                    }
                }
            }
            catch (System.IndexOutOfRangeException e)
            {
                throw new IndexOutOfRangeException("Submatrix indices: " + e.Message);
            }
            return X;
        }




        public static int ColumnLength(this Matrix3 mat)
        {
            return 3;

        }
        public static int RowLength(this Matrix3 mat)
        {
            return 3;

        }

        public static bool CheckRotationMatrix(this Matrix3 R)
        {
            //test
            Matrix3 RT = R.Copy();
            RT.Transpose();
            RT = Matrix3.Mult(R, RT);
            return RT.CompareMatrices(Matrix3.Identity, 1e-10f);
            //RT should be unit matrix

        }
        public static bool CompareMatrices(this Matrix3 mat, Matrix3 other, float threshold)
        {

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    float f1 = Math.Abs(mat[i, j]);
                    float f2 = Math.Abs(mat[i, j] - other[i, j]);
                    float f3 = Math.Abs(f2 / f1);
                    if (f1 > threshold && f2 > threshold && f3 > threshold)
                    {

                        System.Diagnostics.Debug.Assert(false, "Matrix3 check: Elements are not equal: " + i.ToString() + " : " + j.ToString() + " : Values: " + mat[i, j].ToString() + " : " + other[i, j].ToString());
                        return false;
                    }
                }
            }

            return true;

        }
        public static Matrix3 ReplaceColumn(this Matrix3 W, int iSource, int iTarget)
        {

            Matrix3 Vnew = new Matrix3();
            Vnew = W.Copy();

            float[] col = new float[3];
            for (int k = 0; k < 3; k++)
            {
                col[k] = Vnew[k, iTarget];
            }
            for (int k = 0; k < 3; k++)
            {
                Vnew[k, iTarget] = Vnew[k, iSource];
            }
            for (int k = 0; k < 3; k++)
            {
                Vnew[k, iSource] = col[k];
            }

            return Vnew;


        }
        public static Matrix3 ReplaceRow(this Matrix3 W, int iSource, int iTarget)
        {

            Matrix3 Vnew = new Matrix3();
            Vnew = W.Copy();

            float[] col = new float[3];
            for (int k = 0; k < 3; k++)
            {
                col[k] = Vnew[iTarget, k];
            }
            for (int k = 0; k < 3; k++)
            {
                Vnew[iTarget, k] = Vnew[iSource, k];
            }
            for (int k = 0; k < 3; k++)
            {
                Vnew[iSource, k] = col[k];
            }

            return Vnew;


        }
        public static Vector3 ExtractRow(this Matrix3 mat, int N)
        {
            Vector3 vEigen = new Vector3();
            //select eigenvector N

            vEigen.X = mat[0, N];
            vEigen.Y = mat[1, N];
            vEigen.Z = mat[2, N];

            return vEigen;
        }
        public static Vector3 ExtractColumn(this Matrix3 mat, int N)
        {
            Vector3 vEigen = new Vector3();
            //select eigenvector N

            vEigen.X = mat[N, 0];
            vEigen.Y = mat[N, 1];
            vEigen.Z = mat[N, 2];


            return vEigen;
        }
        /// <summary>
        /// matrox for u -> v rotation (source -> target)
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="sourceV"></param>
        /// <param name="targetV"></param>
        /// <returns></returns>
        public static Matrix3 RotationMatrix_NotReallyWorking(this Matrix3 mat, Vector3 sourceV, Vector3 targetV)
        {

            sourceV = sourceV.NormalizeV();
            targetV = targetV.NormalizeV();
            // make sure that we actually have two unique vectors.
            if (sourceV != targetV)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        mat[i, j] = targetV[i] * sourceV[j];
                    }
                }

            }
            //mat.Transpose();
            return mat;
        }
     
        /// <summary>
        /// matrix for u -> v rotation (source -> target)
        /// After: http://cs.brown.edu/~jfh/papers/Moller-EBA-1999/paper.pdf
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="sourceV"></param>
        /// <param name="targetV"></param>
        /// <returns></returns>
        public static Matrix3 RotationOneVectorToAnother(this Matrix3 mat, Vector3 sourceV, Vector3 targetV)
        {

            sourceV = sourceV.NormalizeV();
            targetV = targetV.NormalizeV();
            Vector3 p = new Vector3();
            if (Math.Abs(sourceV.X) < Math.Abs(sourceV.Y) && Math.Abs(sourceV.X) < Math.Abs(sourceV.Z))
            {
                p.X = 1;
            }
            else if (Math.Abs(sourceV.Y) < Math.Abs(sourceV.X) && Math.Abs(sourceV.Y) < Math.Abs(sourceV.Z))
            {
                p.Y = 1;
            }
            else if (Math.Abs(sourceV.Z) < Math.Abs(sourceV.X) && Math.Abs(sourceV.Z) < Math.Abs(sourceV.Y))
            {
                p.Z = 1;
            }

            Vector3 u = p - sourceV;
            Vector3 v = p - targetV;

            float uu = Vector3.Dot(u, u);
            float vv = Vector3.Dot(v, v);
            float uv = Vector3.Dot(u, v);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (i == j)
                        mat[i, j] = 1;
                    if (uu != 0)
                        mat[i, j] = mat[i, j] - 2 / uu * u[i] * u[j];
                    if (vv != 0)
                        mat[i, j] = mat[i, j] - 2 / vv * v[i] * v[j];
                    if (vv != 0 && uu != 0)
                        mat[i, j] = mat[i, j] + 4 * uv / (uu * vv) * v[i] * u[j];

                }
            }


            //mat.Transpose();
            return mat;
        }

        /// <summary>
        /// matrix for u -> v rotation (source -> target)
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="sourceV"></param>
        /// <param name="targetV"></param>
        /// <returns></returns>
        public static Matrix3 RotationChangeBasis(this Matrix3 mat, List<Vector3> newCoordinates)
        {
            for (int i = 0; i < 3; i++)
                newCoordinates[i] = newCoordinates[i].NormalizeV();

            mat.Row0 = newCoordinates[0];
            mat.Row1 = newCoordinates[1];
            mat.Row2 = newCoordinates[2];

            mat.Transpose();

            //mat.Transpose();
            return mat;
        }
     
        public static Matrix3 Rotation_ToOriginAxes(this Matrix3 mat, PointCloud  cloudOrigin)
        {
            
            List<Vector3> list = new List<Vector3>();
            list.Add(new Vector3(1, 0, 0));
            list.Add(new Vector3(0, 1, 0));
            list.Add(new Vector3(0, 0, 1));

            PointCloud target = PointCloud.FromListVector3(list);
            return mat.RotationCoordinateChange(cloudOrigin, target);



        }
        /// <summary>
        /// u - > v translation
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="sourceV"></param>
        /// <param name="targetV"></param>
        /// <returns></returns>
        public static Matrix3 RotationCoordinateChange(this Matrix3 mat, PointCloud cloudOrigin, PointCloud target)
        {
            for (int i = 0; i < 3; i++)
            {
                cloudOrigin[i].Vector = cloudOrigin[i].Vector.NormalizeV();
                target[i].Vector = target[i].Vector.NormalizeV();

            }
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {

                    //float angle = Vector3.CalculateAngle(u[j], v[i]);
                    //float val = Math.Cos(angle);
                    mat[i, j] = Vector3.Dot(cloudOrigin[i].Vector, target[j].Vector);
                }
            }
            //    mat.Transpose();
            //mat.Invert();
            //mat.Transpose();
            return mat;
        }

        /// <summary>
        /// u - > v translation
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="sourceV"></param>
        /// <param name="targetV"></param>
        /// <returns></returns>
        public static Matrix3 RotationCoordinateChange(this Matrix3 mat, List<Vector3> cloudOrigin, List<Vector3> target)
        {
            for (int i = 0; i < 3; i++)
            {
                cloudOrigin[i] = cloudOrigin[i].NormalizeV();
                target[i] = target[i].NormalizeV();

            }
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {

                    //float angle = Vector3.CalculateAngle(u[j], v[i]);
                    //float val = Math.Cos(angle);
                    mat[i, j] = Vector3.Dot(cloudOrigin[i], target[j]);
                }
            }
            //    mat.Transpose();
            //mat.Invert();
            //mat.Transpose();
            return mat;
        }
        ///// <summary>
        ///// u - > v translation
        ///// </summary>
        ///// <param name="mat"></param>
        ///// <param name="sourceV"></param>
        ///// <param name="targetV"></param>
        ///// <returns></returns>
        //public static Matrix3 RotationCoordinateChange(this Matrix3 mat, PointCloud cloudOrigin, PointCloud target)
        //{
        //    for (int i = 0; i < 3; i++)
        //    {
        //        cloudOrigin.Vectors[i] = cloudOrigin.Vectors[i].NormalizeV();
        //        target.Vectors[i] = target.Vectors[i].NormalizeV();

        //    }
        //    for (int i = 0; i < 3; i++)
        //    {
        //        for (int j = 0; j < 3; j++)
        //        {

        //            //float angle = Vector3.CalculateAngle(u[j], v[i]);
        //            //float val = Math.Cos(angle);
        //            mat[i, j] = Vector3.Dot(cloudOrigin.Vectors[i], target.Vectors[j]);
        //        }
        //    }
            
        //    return mat;
        //}

        //public static float[,] TofloatArray(this Matrix3 m)
        //{
        //    float[,] floatArray = new float[3, 3];
        //    for (int i = 0; i < 3; i++)
        //        for (int j = 0; j < 3; j++)
        //            floatArray[i, j] = m[i, j];

        //    return floatArray;
        //}
        public static float RotationAnglesSum(this Matrix3 r)
        {
            Vector3 v = r.RotationAngles();
            return v.X + v.Y + v.Z;

        }
        public static Vector3 RotationAngles(this Matrix3 r)
        {
            float thetaX, thetaY, thetaZ;
            if (r[0, 2] < 1)
            {
                if (r[0, 2] > -1)
                {
                    thetaY = Convert.ToSingle(Math.Asin(r[0, 2]));
                    thetaX = Convert.ToSingle(Math.Atan2(-r[1, 2], r[2, 2]));
                    thetaZ = Convert.ToSingle(Math.Atan2(-r[0, 1], r[0, 0]));
                }
                else // r 0 2 = −1
                {
                    // Not a u n i q u e s o l u t i o n : thetaZ −thetaaX =  Convert.ToSingle(Math.Atan2( r10 , r 1 1 )
                    thetaY = -Convert.ToSingle(Math.PI / 2);
                    thetaX = -Convert.ToSingle(Math.Atan2(r[1, 0], r[1, 1]));
                    thetaZ = 0;
                }
            }

            else // r 0 2 = +1
            {
                // Not a u n i q u e s o l u t i o n : thetaZ +thetaaX =  Convert.ToSingle(Math.Atan2( r10 , r 1 1 )
                thetaY = Convert.ToSingle(Math.PI / 2);
                thetaX = Convert.ToSingle(Math.Atan2(r[1, 0], r[1, 1]));
                thetaZ = 0;
            }

            Vector3 v = new Vector3(thetaX, thetaY, thetaZ);
            return v;

        }
        public static Vector3 Row(this Matrix3 mat, int i)
        {
            Vector3 v = new Vector3(mat[i, 0], mat[i, 1], mat[i, 2]);
            return v;
        }
        public static Vector3 RowSet(this Matrix3 mat, int i, Vector3 v)
        {
            mat[i, 0] = v.X;
            mat[i, 1] = v.Y;
            mat[i, 2] = v.Z;
            return v;
        }
        public static Vector3 Column(this Matrix3 mat, int i)
        {
            Vector3 v = new Vector3(mat[0, i], mat[1, i], mat[2, i]);
            return v;
        }
        public static Matrix3 MatrixDecompose(this Matrix3 matrix, out int[] perm, out int toggle)
        {

            // Doolittle LUP decomposition with partial pivoting.
            // rerturns: result is L (with 1s on diagonal) and U; perm holds row permutations; toggle is +1 or -1 (even or odd)
            int rows = 3;// matrix.Length;
            int cols = 3; // matrix[0].Length; // assume all rows have the same number of columns so just use row [0].
            if (rows != cols)
                throw new Exception("Attempt to MatrixDecompose a non-square mattrix");

            int n = rows; // convenience

            Matrix3 result = matrix.Copy(); // make a copy of the input matrix

            perm = new int[n]; // set up row permutation result
            for (int i = 0; i < n; ++i) { perm[i] = i; }

            toggle = 1; // toggle tracks row swaps. +1 -> even, -1 -> odd. used by MatrixDeterminant

            for (int j = 0; j < n - 1; ++j) // each column
            {
                float colMax = Math.Abs(result[j, j]); // find largest value in col j
                int pRow = j;
                for (int i = j + 1; i < n; ++i)
                {
                    if (result[i, j] > colMax)
                    {
                        colMax = result[i, j];
                        pRow = i;
                    }
                }

                if (pRow != j) // if largest value not on pivot, swap rows
                {
                    Vector3 rowPtr = result.Row(pRow);
                    result.RowSet(pRow, result.Row(j));
                    result.RowSet(j, rowPtr);



                    int tmp = perm[pRow]; // and swap perm info
                    perm[pRow] = perm[j];
                    perm[j] = tmp;

                    toggle = -toggle; // adjust the row-swap toggle
                }

                if (Math.Abs(result[j, j]) < 1.0E-20) // if diagonal after swap is zero . . .
                    throw new Exception("Matric Decomposition error"); // consider a throw

                for (int i = j + 1; i < n; ++i)
                {
                    result[i, j] /= result[j, j];
                    for (int k = j + 1; k < n; ++k)
                    {
                        result[i, k] -= result[i, j] * result[j, k];
                    }
                }
            } // main j column loop

            return result;
        } // MatrixDecompose

        //public static Matrix3 MultiplyScalar(this Matrix3 A, float val)
        //{
        //    Matrix3 mReturn = new Matrix3();
        //    for (int i = 0; i < 3; i++)
        //        for (int j = 0; j < 3; j++)
        //            mReturn[i, j] = A[i, j] * val;
        //    return mReturn;

        //}
        public static Matrix3 Clone(this Matrix3 a)
        {

            Matrix3 m = new Matrix3();
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    m[i, j] = a[i, j];

            return m;
        }

   
        public static Vector3 Multiply(this Matrix3 A, Vector3 v)
        {
            Vector3 u = new Vector3();
            u.X = A[0, 0] * v[0] + A[0, 1] * v[1] + A[0, 2] * v[2];
            u.Y = A[1, 0] * v[0] + A[1, 1] * v[1] + A[1, 2] * v[2];
            u.Z = A[2, 0] * v[0] + A[2, 1] * v[1] + A[2, 2] * v[2];


            return u;
        }

        public static float[] Multiply3x3(this float[,] A, float[] v)
        {
            float[] u = new float[3];
            float x = A[0, 0] * v[0] + A[0, 1] * v[1] + A[0, 2] * v[2];
            float y = A[1, 0] * v[0] + A[1, 1] * v[1] + A[1, 2] * v[2];
            float z = A[2, 0] * v[0] + A[2, 1] * v[1] + A[2, 2] * v[2];

            u[0] = x;
            u[1] = y;
            u[2] = z;
            return u;
        }

    }
}
