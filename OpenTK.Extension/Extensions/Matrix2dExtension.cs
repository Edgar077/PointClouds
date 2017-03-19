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
    public static class Matrix2dExtension
    {
       
        public static double[,] ToFloatArray(this Matrix2d m)
        {
            double[,] doubleArray = new double[2, 2];
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    doubleArray[i, j] = m[i, j];

            return doubleArray;
        }
        public static double[,] ToDoubleArray(this Matrix2d m)
        {
            double[,] doubleArray = new double[2, 2];
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    doubleArray[i, j] = m[i, j];

            return doubleArray;
        }

        public static Matrix2d FromFloatArray(this Matrix2d mat, double[,] arr)
        {

            Matrix2d m = new Matrix2d();
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    m[i, j] = arr[i, j];

            return m;
        }
        public static Matrix2d FromDoubleArray(this Matrix2d mat, double[,] arr)
        {

            Matrix2d m = new Matrix2d();
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    m[i, j] = Convert.ToSingle(arr[i, j]);

            return m;
        }
    }
}
