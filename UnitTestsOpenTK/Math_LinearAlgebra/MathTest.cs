using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKExtension;
using OpenTK;

namespace UnitTestsOpenTK.LinearAlgebra
{
    [TestFixture]
    [Category("UnitTest")]
    public class MathTest
    {
         private static string path;
         public MathTest()
        {
            path = AppDomain.CurrentDomain.BaseDirectory + "\\Models\\UnitTests";
            //string str = 

        }


         [Test]
   
        
         public void Matrix3Test()
         {
             Matrix3 a = new Matrix3();
             

             a[0, 0] = 2.8f;
             a[0, 1] = 1.26f;
             a[0, 2] = -2.04f;
             
             a[1, 0] = 1.26f;
             a[1, 1] = 1.15f;
             a[1, 2] = -1.877f;

             a[2, 0] = -2.04f;
             a[2, 1] = -1.877f;
             a[2, 2] = 3.04f;


             //Matrix3 a = new Matrix3();

             Matrix3 c;

             double[,] Harray = a.ToDoubleArray();
             double[,] Uarray = new double[3, 3];
             double[,] VTarray = new double[3, 3];
             double[] eigenvalues = new double[3];
             

             //trial 3:
             alglib.svd.rmatrixsvd(Harray, 3, 3, 2, 2, 2, ref eigenvalues, ref Uarray, ref VTarray);

             Vector3 EV = new Vector3(Convert.ToSingle(eigenvalues[0]), Convert.ToSingle(eigenvalues[1]), Convert.ToSingle(eigenvalues[2]));
             Matrix3 S = new Matrix3();
             S[0, 0] = EV.X;
             S[1, 1] = EV.Y;
             S[2, 2] = EV.Y;

             Matrix3 U = new Matrix3();
             U.FromDoubleArray(Uarray);
             Matrix3 UT = Matrix3.Transpose(U);
             c = Matrix3.Mult(U, UT);//should give I Matrix
             Matrix3 VT = new Matrix3();
             VT.FromDoubleArray(VTarray);
             
             Matrix3 V = Matrix3.Transpose(VT);
             c = Matrix3.Mult(V, VT);//should give I Matrix
             //check solution

             //Matrix3 checkShouldGiveI = Matrix3.Mult(U, VT);
             Matrix3 R = Matrix3.Mult(U, VT);

             Matrix3 test = Matrix3.Mult(S, VT);
             test = Matrix3.Mult(U, test);
             Assert.That(test, Is.EqualTo(a).Within(1e-7f));

             Matrix3 RT = Matrix3.Transpose(R);

             c = Matrix3.Mult(RT, R);//should give I Matrix
             
             test = Matrix3.Mult(a, V);
             test = Matrix3.Mult(U, a);

         }
     
    }
}
