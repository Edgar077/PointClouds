// original from the Java matrix package JAMA http://math.nist.gov/javanumerics/jama/
// converted to C# by Ken Johnson, added units tests, (2010)
// http://www.codeproject.com/Articles/91458/MaNet-A-matrix-library-for-NET-Rational-Computing
//adapted to use OpenTK 

using System;


namespace OpenTKExtension
{
    /// <summary>
    /// Minor staic class to hold hypot method
    /// </summary>
   static  class Maths
    {

       


       /// <summary>
       ///  sqrt(a^2 + b^2) without under/overflow.
       /// </summary>
       /// <param name="a">a</param>
       /// <param name="b">a</param>
       /// <returns>the length of the radius defined by a and b</returns>
        public static float Hypot(float a, float b)
        {  

            float r;

            if (Math.Abs(a) > Math.Abs(b))
            {

                r = b / a;

                r = Math.Abs(a) *  Convert.ToSingle(Math.Sqrt(1 + r * r));

            }
            else if (b != 0)
            {

                r = a / b;

                r = Math.Abs(b) * Convert.ToSingle(Math.Sqrt(1 + r * r));

            }
            else
            {

                r = 0.0f;

            }

            return r;

         }

    }
}
