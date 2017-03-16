using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;


using OpenTKExtension;
using OpenTK;
using NLinear;
using System.Numerics;

namespace UnitTestsOpenTK.LinearAlgebra
{
    [TestFixture]
    [Category("UnitTest")]
    public class NLinearTest : TestBase
    {


        [Test]
        public void MatrixReal()
        {
            //Declare a 4x4 matrix
            Matrix44<float> m44 = new Matrix44<float>(6, -7, 10, 4, 0, 3, -1, 8, 0, 5, -7, 0, 1, 2, 7, 6);
            //Declare an identity matrix
            Matrix44<float> id = Matrix44<float>.Identity(1);

            System.Diagnostics.Debug.WriteLine(m44 * id == m44); //True
            System.Diagnostics.Debug.WriteLine(id * m44 == m44); //True

            //Declare a 3x3 matrix
            Matrix33<float> m33 = new Matrix33<float>(6, -7, 10, 0, 3, -1, 0, 5, -7);

            Matrix33<float> m33Inv = m33.Inverse(1);
            Matrix33<float> id2 = m33 * m33Inv;

            System.Diagnostics.Debug.WriteLine(id2 == Matrix33<float>.Identity(1)); //True

            //Console.ReadKey();

        }

        [Test]
        public void MatrixComplex()
        {
            var i = Complex.ImaginaryOne;

            //Declare a 4x4 matrix
            Matrix44<Complex> m44 = new Matrix44<Complex>(1, 1, 1, 1, 1, i, -1, -i, 1, -i, 1, -1, 1, -i, -1, i);

            //Declare an identity matrix
            Matrix44<Complex> id = Matrix44<Complex>.Identity(1);

            System.Diagnostics.Debug.WriteLine(id * m44 == m44);//true

            //Console.ReadKey();
        }
        [Test]
        public void BigInteger()
        {
            //Declare two unit vectors e1, e2
            Vector3<BigInteger> e1 = new Vector3<BigInteger>(1, 0, 0);
            Vector3<BigInteger> e2 = new Vector3<BigInteger>(0, 1, 0);

            Vector3<BigInteger> e3 = e1.Cross(e2);

            System.Diagnostics.Debug.WriteLine(e3);

           // Console.ReadKey();
        }
        [Test]
        public void Vector3Float()
        {
            //Declare two unit vectors e1, e2
            Vector3<float> e1 = new Vector3<float>(1, 0, 0);
            Vector3<float> e2 = new Vector3<float>(0, 1, 0);

            Vector3<float> e3 = e1.Cross(e2);

            System.Diagnostics.Debug.WriteLine(e3);

            //Console.ReadKey();
        }
        [Test]
        public void Vector2Int()
        {
            //Declare two unit vectors e1, e2
            Vector2<int> v1 = new Vector2<int>(1, 0);
            Vector2<int> v2 = new Vector2<int>(0, 1);

            int proj = v1.Dot(v2);
            proj = v1 ^ v2;

            //Check if proj == 0
            System.Diagnostics.Debug.WriteLine(proj == 0); //True

            //Console.ReadKey();
        }
    }
}
