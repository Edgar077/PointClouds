// original from the Java matrix package JAMA http://math.nist.gov/javanumerics/jama/
// converted to C# by Ken Johnson, added units tests, (2010)
// http://www.codeproject.com/Articles/91458/MaNet-A-matrix-library-for-NET-Rational-Computing
//adapted to use OpenTK 

using System;

using OpenTK;
using OpenTKExtension;


namespace UnitTestsOpenTK.LinearAlgebra
{
  public static class StandardMatrixTests
    {
        
        public static bool IsUpperTriangular(Matrix3 m)
        {
            for (int iRow = 0; iRow < m.RowLength(); iRow++)
            {
                for (int iCol = 0; iCol < m.ColumnLength(); iCol++)
                {
                    if (iRow > iCol && m[iRow, iCol] != 0) return false;
                }
            }
            return true;
        }


        public static bool IsLowerTriangular(Matrix3 m)
        {
            for (int iRow = 0; iRow < m.RowLength(); iRow++)
            {
                for (int iCol = 0; iCol < m.ColumnLength(); iCol++)
                {
                    if (iRow < iCol && m[iRow, iCol] != 0) return false;
                }
            }
            return true;
        }

        public static bool IsDiagonal(Matrix3 m)
        {
            for (int iRow = 0; iRow < m.RowLength(); iRow++)
            {
                for (int iCol = 0; iCol < m.ColumnLength(); iCol++)
                {
                    if (iRow != iCol && m[iRow, iCol] != 0) return false;
                }
            }
            return true;
        }


        public static bool IsSymetric(Matrix3 m)
        {
            if (m.RowLength() != m.ColumnLength()) return false;
            int n = m.ColumnLength();
          bool  issymmetric = true;
            for (int j = 0; (j < n) & issymmetric; j++)
            {
                for (int i = 0; (i < n) & issymmetric; i++)
                {
                    issymmetric = (m[i, j] == m[j, i]);
                }
            }
            return issymmetric;
        }


        public static  bool IsNonnegativeDiagonal(Matrix3 mat)
        {
            for (int i = 0; i < mat.RowLength(); i++)
            {
                for (int j = 0; j < mat.ColumnLength(); j++)
                {
                    if (i == j)
                    {
                        if (mat[i, j] < 0) return false;
                    }
                    else
                    {
                       
                        if (mat[i, j] != 0) return false;

                    }

                }
            }
            return true;

        }


        public static bool IsIntegerValued(Matrix3 mat)
        {
            for (int i = 0; i < mat.RowLength(); i++)
            {
                for (int j = 0; j < mat.ColumnLength(); j++)
                {
                    if (Math.Truncate(mat[i, j]) != mat[i, j]) return false;
                }
            }
            return true;

        }

    }
}
