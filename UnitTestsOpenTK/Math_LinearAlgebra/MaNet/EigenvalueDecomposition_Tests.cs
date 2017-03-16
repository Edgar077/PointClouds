// original from the Java matrix package JAMA http://math.nist.gov/javanumerics/jama/
// converted to C# by Ken Johnson, added units tests, (2010)
// http://www.codeproject.com/Articles/91458/MaNet-A-matrix-library-for-NET-Rational-Computing
//adapted to use OpenTK 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

using System.Diagnostics;
using OpenTK;
using OpenTKExtension;

namespace UnitTestsOpenTK.LinearAlgebra
{
    [TestFixture]
   public  class EigenvalueDecomposition_Tests
    {

        [Test]
        public void TwoByTwo_Symetric()
        {
 
            string strA = @"2	1
                            1	2";
            Matrix3 A = new Matrix3(); 
            A.Parse(strA);

            string strExpectedD = @"1  0
                                    0  3";
            Matrix3 ExpectedD = new Matrix3(); 
            ExpectedD.Parse(strExpectedD);


            string strExpectedV = @" -0.7071     0.7071
                                      0.7071     0.7071";

            Matrix3 ExpectedV = new Matrix3();
            ExpectedV.Parse(strExpectedV);
            //This is the value that one will get from Matlab. The eigenvalue Decomposition returns 
            //the  following           0.7071     0.7071
            //                        -0.7071     0.7071"
            // (Warning Proof ahead )
            //The problem stems from the fact that the Eigenvalue decomposition is not entirely unique.
            // The A is decomposed such that A = V * D * V';
            // Consider the diagonal  matrix J such that all of its diagonal values are eiher 1 or -1. J*J = I.
            // (V *J) * D * (V * J)' = V * J * D * J' * V'  using (AB)' = B'A' The transpose of the product is the transpose of the elements reversed.
            //  V * J * D * J' * V'  =  V * J * D * J * V'  as J is diagonal
            //  V * J * D * J * V'   = V * J * J * D * V' as J and D are diagonal.
            //  V * J * J * D * V'  = V * D * V' since J*J = I.
            // (Proof finished)
            //  In practical terms this means that    Assert.That(V, Is.EqualTo(ExpectedV) );
            //  is not a good test. and I will need to test if it is equivalent instead. 


            

            Assert.That(StandardMatrixTests.IsSymetric(A), Is.True);

            EigenvalueDecomposition EofA = new EigenvalueDecomposition(A);

           

           float[] realEigenValues= EofA.EV;
           float[] imaginaryEigenValues = EofA.EV;

            Matrix3 V = EofA.V;
            Debug.WriteLine(V.ToString());
           // Assert.That(V, Is.EqualTo(ExpectedV) ); Not enough uniqueness so does not work
            //TestEigenvalueVEquivalent(V, ExpectedV);

            // V is orthogonal V times V transpose is the identity
            
            Assert.That( Matrix3.Mult(V,Matrix3.Transpose(V)).ToFloatArray(), Is.EqualTo(Matrix3.Identity.ToFloatArray()).Within(.0000001));

            Matrix3 D = EofA.getD();
            Assert.That(StandardMatrixTests.IsDiagonal(D), Is.True); //Diagonal which for 2x2 is diagonal
            Assert.That(D.ToFloatArray(), Is.EqualTo(ExpectedD.ToFloatArray()).Within(10).Ulps);

            //V * D * V,transpose = A
            Matrix3 test = Matrix3.Mult(D, Matrix3.Transpose(V));
            test = Matrix3.Mult(V, test);
            Assert.That(test.ToFloatArray(), Is.EqualTo(A.ToFloatArray()).Within(.0000001));

             

        }


//        [Test]
//        public void TwoByTwo_Singular()
//        {
//            string strSingular = @"1     2
//                                   3     6";
//            Matrix S = Matrix.Parse(strSingular);
//            EigenvalueDecomposition EofA = new EigenvalueDecomposition(S);


//        }


        [Test]
        public void TwoByTwo_AntiSymetric()
        {
            string strAnti = @"2    1
                              -1    2";

            Matrix3 A =  new Matrix3();
            A.Parse(strAnti);
            EigenvalueDecomposition EofA = new EigenvalueDecomposition(A);
            Assert.That(EofA.EV , Is.EqualTo(new float[ ]{2,2}));
            Assert.That(EofA.EV_IMAG, Is.EqualTo(new float[] { 1, -1 }));

        }

        [Test]
        public void LargeSymetric()
        {
            Matrix3 K = Specialized.K(100);
            TestDecomposition(K);

        }

        [Test]
        public void Zeromatrix()
        {
            Matrix3 Z = new Matrix3();
            TestDecomposition(Z);


        }
 
 

        [Test]
        public void CloseEigenvaluesTest()
        {
            //This should produce two distinct small eigwnvalues that are close together.
            // http://www.mathworks.com/company/newsletters/news_notes/pdf/sum95cleve.pdf
            string aBase = @"0   1    0   0 
                             1   0   -d   0
                             0   d    0   1 
                             0   0    1   0";

            Matrix3 A = Parser.Substitute(aBase, "d", .0000000001f);
            TestDecomposition(A);



        }

        private void TestDecomposition(Matrix3 A)
        {
            EigenvalueDecomposition EofA = new EigenvalueDecomposition(A);

            Matrix3 V = EofA.V;
            Matrix3 D = EofA.getD();

            if (StandardMatrixTests.IsSymetric(A))
            {

                // V is orthogonal V times V transpose is the identity
                Matrix3 test = Matrix3.Mult(D, Matrix3.Transpose(V));
                test = Matrix3.Mult(V, test);
                Assert.That( Matrix3.Mult(V,Matrix3.Transpose(V)), Is.EqualTo(Matrix3.Identity).Within(.0000001));
                Assert.That(test.ToFloatArray(), Is.EqualTo(A.ToFloatArray()).Within(.0000001));

            }
            else 
            {

                Assert.That((A * V).ToFloatArray(), Is.EqualTo((V * D).ToFloatArray()).Within(.0000001));

            }

        }

        //private void TestEigenvalueVEquivalent(Matrix3 A, Matrix3 B)
        //{
        //    Vector3 diagA = A.Diagonal;
        //    Vector3 diagB = B.Diagonal;

        //    float[] diaSignA = new float[Convert.ToInt32(diagA.Length)];
        //    float[] diaSignB = new float[Convert.ToInt32(diagB.Length)];
        //    for (int i = 0; i < diagA.Length; i++)
        //    {
        //        diaSignA[i] = Math.Sign(diagA[i]);
        //        diaSignB[i] = Math.Sign(diagB[i]);
        //    }
        //    Matrix3 JA = Matrix3.Diagonal(diaSignA);
        //    Matrix3 JB = Matrix3.Diagonal(diaSignB);

        //    Assert.That(A * JA, Is.EqualTo(B * JB).Within(.001));

        //}

        [Test]
        public void TwoByTwo_NonSymetric()
        {

            string strA = @"1	2
                            3	4";
            Matrix3 A =  new Matrix3();
            A.Parse(strA);
 
            Assert.That(StandardMatrixTests.IsSymetric(A), Is.False);

            EigenvalueDecomposition EofA = new EigenvalueDecomposition(A);

            Matrix3 V = EofA.V;
            Matrix3 D = EofA.getD();
            Assert.That(StandardMatrixTests.IsDiagonal(D), Is.True); //Block Diagonal which for 2x2 is diagonal

            // V*D * V.Inverse = A
            Matrix3 test = Matrix3.Mult(D, V.Inverted());
            test = Matrix3.Mult(V, test);
            Assert.That(test.ToFloatArray(), Is.EqualTo(A.ToFloatArray()).Within(.0000001).Percent);

            // A.times(V) equals V.times(D)

            Assert.That(Matrix3.Mult(A, V).ToFloatArray(), Is.EqualTo(Matrix3.Mult(V, D).ToFloatArray()).Within(.0000001).Percent);
        }


        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(12)]
        public void Random_NByN_NonSymetric(int n)
        {
            Rectangular rand = new Rectangular();
            Matrix3 A = rand.Randomfloat(n, n);

            EigenvalueDecomposition EofA = new EigenvalueDecomposition(A);

            Matrix3 V = EofA.V;
         

            Matrix3 D = EofA.getD();
            Matrix3 test = Matrix3.Mult(D, V.Inverted());
            test = Matrix3.Mult(V, test);
            // V*D * V.Inverse = A
            test = Matrix3.Mult(D, V.Inverted());
            test = Matrix3.Mult(V, test);
            Assert.That(test.ToFloatArray(), Is.EqualTo(A.ToFloatArray()).Within(.0000001).Percent);
            // A.times(V) equals V.times(D)
            Assert.That(Matrix3.Mult(A, V).ToFloatArray(), Is.EqualTo(Matrix3.Mult(V, D).ToFloatArray()).Within(.0000001).Percent);

        }


        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(12)]
        public void Random_NByN_Symetric(int n)
        {
            Rectangular rand = new Rectangular();
            Matrix3 A = rand.Randomfloat(n, n);
            A = Matrix3.Add(A, Matrix3.Transpose(A));

            /// Any matrix added to its transpose will be symetric
            Assert.That(StandardMatrixTests.IsSymetric(A), Is.True);

            EigenvalueDecomposition EofA = new EigenvalueDecomposition(A);

            Matrix3 V = EofA.V;
            // V is orthogonal V times V transpose is the identity
            Assert.That(Matrix3.Mult(V, Matrix3.Transpose(V)).ToFloatArray(), Is.EqualTo(Matrix3.Identity.ToFloatArray()).Within(.0000001));

            Matrix3 D = EofA.getD();
            Matrix3 test = Matrix3.Mult(D, Matrix3.Transpose(V));
            test = Matrix3.Mult(V, test);

            Assert.That(test.ToFloatArray(), Is.EqualTo(A.ToFloatArray()).Within(.0000001).Percent);



        }

        [Test]
        public void Rosser_Test()
        {
            Matrix3 R = Specialized.Rosser();


            EigenvalueDecomposition EofA = new EigenvalueDecomposition(R);

            Matrix3 V = EofA.V;
            // V is orthogonal V times V transpose is the identity
            Assert.That(Matrix3.Mult(V, Matrix3.Transpose(V)).ToFloatArray(), Is.EqualTo(Matrix3.Identity.ToFloatArray()).Within(.0000001));

            Matrix3 D = EofA.getD();
            Matrix3 test = Matrix3.Mult(D, V.Inverted());
            test = Matrix3.Mult(V, test);

            Assert.That(test.ToFloatArray(), Is.EqualTo(R.ToFloatArray()).Within(.0000001).Percent);



        }



    }
}
