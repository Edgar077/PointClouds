// original from the Java matrix package JAMA http://math.nist.gov/javanumerics/jama/
// converted to C# by Ken Johnson, added units tests, (2010)
// http://www.codeproject.com/Articles/91458/MaNet-A-matrix-library-for-NET-Rational-Computing
//adapted to use OpenTK 

using NUnit.Framework;
using OpenTK;
using OpenTKExtension;

namespace UnitTestsOpenTK.LinearAlgebra
{
    [TestFixture]
 public   class SingularValueDecomposition_Tests
    {
        [Test]
        public void ThreeByThree_Identity()
        {
            
            Matrix3 H = Matrix3.Identity;
           
            SingularValueDecomposition mySVD = new SingularValueDecomposition(H);


            //Matrix3 U = mySVD.U;
            //Matrix3 VT = mySVD.V;
            //Vector3 EV = mySVD.EV;

            //// A = U*S*V'
            //Matrix3 test = Matrix3.Mult(mySVD.EV_Mat, Matrix3.Transpose(VT));
            //test = Matrix3.Mult(U, test);
            //Assert.That(test.ToArray(), Is.EqualTo(H.ToArray()).Within(.00000001));



        }

        [Test]
        public void ThreeByThree()
        {
            string strA = @"2,8 1,26 -2,04
                            1,26 1,15 -1,87
                            -2,04 -1,87 3,04";
            Matrix3 H = new Matrix3();
            H = H.Parse(strA);
          
            SingularValueDecomposition mySVD = new SingularValueDecomposition(H);


            //Matrix3 U = mySVD.U;
            //Matrix3 VT = mySVD.V;
            //Vector3 EV = mySVD.EV;

            // A = U*S*V'
           // Matrix3 test = Matrix3.Mult(mySVD.EV_Mat, Matrix3.Transpose(VT));
            //test = Matrix3.Mult(U, test);
            //Assert.That(test.ToArray(), Is.EqualTo(H.ToArray()).Within(.00000001));


        }
       
       
    }
}
