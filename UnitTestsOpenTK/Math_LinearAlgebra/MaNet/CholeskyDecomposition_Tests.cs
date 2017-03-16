//// original from the Java matrix package JAMA http://math.nist.gov/javanumerics/jama/
//// converted to C# by Ken Johnson, added units tests, (2010)
//// http://www.codeproject.com/Articles/91458/MaNet-A-matrix-library-for-NET-Rational-Computing
////adapted to use OpenTK 

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using NUnit.Framework;

//using System.Diagnostics;
//using OpenTK;
//using OpenTKExtension;

//namespace UnitTestsOpenTK.LinearAlgebra
//{
//    [TestFixture]
//  public  class CholeskyDecomposition_Tests
//    {
//        [Test]
//        public void CholeskyDecomposition_Test()
//        {
//            string strD = @"  2    -1     0
//                             -1     2    -1
//                              0    -1     2";
//            Matrix3 D = new Matrix3();
//            D.Parse(strD); // D is positive definite

//        //L created in matlab however note that Matlab's convention is that the 
//        // matrix is upper rather than lower triangular
//        String strL = @"1.4142         0         0
//                       -0.7071    1.2247         0
//                             0   -0.8165    1.1547";

//        string strB = @"1
//                        2
//                        3";

//        string strExSln = @"2.5
//                          4.0 
//                          3.5"; //Expected Solution


//        Matrix3 L = new Matrix3();
//            L.Parse(strL);


//        CholeskyDecomposition chol = new CholeskyDecomposition(D);
//        Assert.That(chol.getL().ToArray(), Is.EqualTo(L.ToArray()).Within(.0001));

//          //This is the same as callin  D.chol();
//        Assert.That(D.Chol().getL().ToArray(), Is.EqualTo(L.ToArray()).Within(.0001));

//        //Definition of Decomposition A = LL'
//        Matrix3 test = Matrix3.Mult(L, Matrix3.Transpose(L));
//        ;

//        Assert.That(test.ToArray(), Is.EqualTo(D.ToArray()).Within(.001));

//        //Checking verification of positive definiteness
//        Assert.That(chol.IsSPD() , Is.EqualTo(true));


//        Matrix3 sln = new Matrix3(); 
//            chol.Solve(sln.Parse(strB));
//      Matrix3 eSln = new Matrix3(); 
//            eSln.Parse(strExSln);
//      Assert.That(sln, Is.EqualTo(eSln).Within(.0000000000001));

//        }

//        [Test]
//        public void CholeskyDecompositionException_Test()
//        {
//            string strD = @"  2    -1     22
//                             -1     2    -1
//                              0    -1     2";

//            string strBWrong = @"1
//                                2
//                                3
//                                4";

//            string strB = @"1
//                            2
//                            3";
//            Matrix3 D = new Matrix3(); 
//            D.Parse(strD); // D is not positive definite
//            CholeskyDecomposition chol = new CholeskyDecomposition( D);
//            Matrix3 m = new Matrix3();

//            Assert.Throws(typeof(ArgumentException), delegate { chol.Solve(m.Parse(strBWrong)); });

//            Assert.Throws(typeof( Exception), delegate { chol.Solve(m.Parse(strB )); });

//        }
//    }
//}
