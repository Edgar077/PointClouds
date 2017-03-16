using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKExtension;
using UnitTestsOpenTK;
using ICPLib;
using OpenTK;


namespace Automated
{
    [TestFixture]
    [Category("UnitTest")]
    public class SVD_Test : TestBaseICP
    {
         
        
        //[Test]
        //public void SVD_GenMatrixWikipedia()
        //{

        //    double[][] m = new double[4][];
        //    m[0] = new double[5] { 1, 0, 0, 0, 2 };
        //    m[1] = new double[5] { 0, 0, 3, 0, 0 };
        //    m[2] = new double[5] { 0, 0, 0, 0, 0 };
        //    m[3] = new double[5] { 0, 2, 0, 0, 0 };

        //    DotNetMatrix.GeneralMatrix mNew = new DotNetMatrix.GeneralMatrix(m);

        //    //alternative 1
        //    DotNetMatrix.SingularValueDecomposition svd = new DotNetMatrix.SingularValueDecomposition(mNew);

        //}
        private Matrix3 CreateH()
        {

            Matrix3 H = new Matrix3();
            H[0, 0] = 1;
            H[0, 1] = 2;
            H[0, 2] = 3;
            H[1, 0] = 0;
            H[1, 1] = 2;
            H[1, 2] = 0;
            H[2, 0] = -1;
            H[2, 1] = 0;
            H[2, 2] = 0;
            return H;
        }
     
        private void TestResults(Matrix3 U, Matrix3 VT, Vector3 EV)
          {
              Matrix3 UT = Matrix3.Transpose(U);
              Matrix3 V = Matrix3.Transpose(VT);

              Matrix3 I = Matrix3.Mult(U, UT);


              I = Matrix3.Mult(V, VT);

              //test!!
              VT.Transpose();

           


          }
         [Test]
          public void SVD_Alglib()
          {
              
              Matrix3 H = CreateH();


              double[,] Harray = H.ToDoubleArray();
             

              double[,] Uarray = new double[3, 3];
              double[,] VTarray = new double[3, 3];
              double[] EVarray = new double[3];


              alglib.svd.rmatrixsvd(Harray, 3, 3, 2, 2, 2, ref EVarray, ref Uarray, ref VTarray);

              Matrix3 U = new Matrix3();
              U = U.FromDoubleArray(Uarray);



              Matrix3 VT = new Matrix3();
              VT = VT.FromDoubleArray(VTarray);
              Matrix3 R = Matrix3.Mult(U, VT);

              Vector3 EV = new Vector3();
              EV = EV.FromDoubleArray(EVarray);

              TestResults(U, VT, EV);

          }
         [Test]
        public void SVD_AlglibWiki()
        {
            //sample from: https://en.wikipedia.org/wiki/Singular_value_decomposition

            double[,] doubleArray = new double[3, 3];

            double[,] Harray = new double[4,5];
            Harray[0,0] = 1;
            Harray[0,1] = 0;
            Harray[0,2] = 0;
            Harray[0,3] = 0;
            Harray[0,4] = 2;

            Harray[1,0 ] = 0;
            Harray[1,1] = 0;
            Harray[1,2] = 3;
            Harray[1,3] = 0;
            Harray[1,4] = 0;

            Harray[2,0] = 0;
            Harray[2,1] = 0;
            Harray[2,2] = 0;
            Harray[2,3] = 0;
            Harray[2,4] = 0;

            Harray[3,0] = 0;
            Harray[3,1] = 2;
            Harray[3,2] = 0;
            Harray[3,3] = 0;
            Harray[3,4] = 0;

            

          
            double[,] Uarray = new double[5, 5];
            double[,] VTarray = new double[5, 5];
            double[] EVarray = new double[4];


            alglib.svd.rmatrixsvd(Harray, 4, 5, 2, 2, 2, ref EVarray, ref Uarray, ref VTarray);
            
             //the results differ in that of the Wikipedia results
             //size of U is one larger
             //eigenvalues are sorted - so the largest eigenvalue is first. 
             //the eigenvectors U and are also in other other (due to sorting of the eigenvalues
             //the VT values differ 
             //as the solution is not unique, this is OK
             //test: U*UT = 1
             // V*VT = 1
            Uarray[0, 0] = 0;
            Uarray[0, 1] = 1;
            Uarray[0, 2] = 0;
            Uarray[0, 3] = 0;

            Uarray[1, 0] = 1;
            Uarray[1, 1] = 0;
            Uarray[1, 2] = 0;
            Uarray[1, 3] = 0;

            Uarray[2, 0] = 0;
            Uarray[2, 1] = 0;
            Uarray[2, 2] = 0;
            Uarray[2, 3] = -1;

            Uarray[3, 0] = 0;
            Uarray[3, 1] = 0;
            Uarray[3, 2] = 1;
            Uarray[3, 3] = 0;

            VTarray[0, 0] = 0;
            VTarray[0, 1] = 0;
            VTarray[0, 2] = 1;
            VTarray[0, 3] = 0;
            VTarray[0, 4] = 0;

            VTarray[1, 0] = 0;
            VTarray[1, 1] = 0;
            VTarray[1, 2] = 1;
            VTarray[1, 3] = 0;
            VTarray[1, 4] = 0;

            VTarray[2, 0] = 1.4142135623730950488016887242097;
            VTarray[2, 1] = 0;
            VTarray[2, 2] = 0;
            VTarray[2, 3] = 0;
            VTarray[2, 4] = 2.8284271247461900976033774484194;

            VTarray[3, 0] = 0;
            VTarray[3, 1] = 0;
            VTarray[3, 2] = 0;
            VTarray[3, 3] = 1;
            VTarray[3, 4] = 0;

            VTarray[4, 0] = -2.8284271247461900976033774484194;
            VTarray[4, 1] = 0;
            VTarray[4, 2] = 0;
            VTarray[4, 3] = 0;
            VTarray[4, 4] = 1.4142135623730950488016887242097;


            EVarray[0] = 2;
            EVarray[1] = 3;
            EVarray[2] = 2.2360679774997896964091736687313;
            EVarray[3] = 0;

        }
        
   
     
    }
}
