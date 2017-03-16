// original from the Java matrix package JAMA http://math.nist.gov/javanumerics/jama/
// converted to C# by Ken Johnson, added units tests, (2010)
// http://www.codeproject.com/Articles/91458/MaNet-A-matrix-library-for-NET-Rational-Computing
//adapted to use OpenTK 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Data;

using OpenTK;
using OpenTKExtension;

namespace UnitTestsOpenTK.LinearAlgebra
{
    [TestFixture]
 public class Matrix_IOTests
    {
        [Test]
        public void ToString_Test()
        {
            Matrix3 A = new Matrix3();
            Assert.That(A.ToString() , Is.EqualTo( "[0 0;0 0]"));
            Assert.That(A.ToString("<", "{", "\n", ", ", "}", ">"), Is.EqualTo("<{0, 0}\n{0, 0}>"));
            A = new Matrix3();
            Assert.That(A.ToString(), Is.EqualTo("[9 9;9 9]"));
            Assert.That(A.ToString("<", "{", "\n", ", ", "}", ">"), Is.EqualTo("<{9, 9}\n{9, 9}>"));

        }


        [Test]
        public void Parse_Test()
        {
            Matrix3 A = new Matrix3(); 
            A.Parse("9 9\n9 9");
            Assert.That(A.ToFloatArray(), Is.EqualTo(new Matrix3().ToFloatArray()));
            A = A.Parse("<{9, 9}\n{9, 9}>", "<", "{", "\n", ", ", "}", ">");
            Assert.That(A.ToFloatArray(), Is.EqualTo(new Matrix3().ToFloatArray()));
        }

        [Test]
        [TestCase(2, 2, 1)]
        public void ToStringParse_CycleTest(int m, int n, int timesToRun)
        {
            Rectangular rand = new Rectangular();
            

            for (int i = 0; i < timesToRun; i++)
            {
                Matrix3 A = rand.Randomfloat(m, n);
                string strA = A.ToString();
                Matrix3 AReconstituted = new Matrix3();
                AReconstituted.Parse(strA);
                Assert.That(AReconstituted.ToFloatArray(), Is.EqualTo(A.ToFloatArray()));
            }

            for (int i = 0; i < timesToRun; i++)
            {
                Matrix3 A = rand.Randomfloat(m, n);
                string strA = A.ToString("<", "{", "\n", ", ", "}", ">");
                Matrix3 AReconstituted = new Matrix3(); 
                AReconstituted.Parse(strA, "<", "{", "\n", ", ", "}", ">");
                Assert.That(AReconstituted.ToFloatArray(), Is.EqualTo(A.ToFloatArray()));
            }

        }
        

        [Test]
        public void ToMatLabString_Test()
        {
            Matrix3 A = new Matrix3(); 
            A.Parse("1 2\n3 4");
            Assert.That(A.ToMatLabString(), Is.EqualTo("[1 2;3 4]"));
        }


        [Test]
        public void ParseMatLab_Test()
        {
            Matrix3 A = new Matrix3(); 
            A.ParseMatLab("[1 2;3 4]");
            Assert.That(A.ToFloatArray(), Is.EqualTo(A.Parse("1 2\n3 4").ToFloatArray()));
        }


        [Test]
        [TestCase(2, 2, 1)]
        public void ToMatLabStringParse_CycleTest(int m, int n, int timesToRun)
        {
            Rectangular rand = new Rectangular();

            for (int i = 0; i < timesToRun; i++)
            {
                Matrix3 A = rand.Randomfloat(m, n);
                string strA = A.ToMatLabString();
                Matrix3 AReconstituted = new Matrix3(); 
                AReconstituted.ParseMatLab(strA);
                Assert.That(AReconstituted, Is.EqualTo(A));
            }
        }

        [Test]
        public void ToMathematicaString_Test()
        {
            Matrix3 A = new Matrix3(); 
            A.Parse("1 2\n3 4");
            Assert.That(A.ToMathematicaString(), Is.EqualTo("{{1, 2}, {3, 4}}"));
        }


        [Test]
        public void ParseMathematica_Test()
        {
            Matrix3 A = new Matrix3(); 
            A.ParseMathematica("{{1, 2}, {3, 4}}");
            Assert.That(A.ToFloatArray(), Is.EqualTo(A.Parse("1 2\n3 4").ToFloatArray()));
        }


        [Test]
        [TestCase(2, 2, 1)]
        public void ToMathematicaStringParse_CycleTest(int m, int n, int timesToRun)
        {
            Rectangular rand = new Rectangular();

            for (int i = 0; i < timesToRun; i++)
            {
                Matrix3 A = rand.Randomfloat(m, n);
                string strA = A.ToMathematicaString();
                Matrix3 AReconstituted = new Matrix3(); 
                AReconstituted.ParseMathematica(strA);
                Assert.That(AReconstituted.ToFloatArray(), Is.EqualTo(A.ToFloatArray()));
            }
        }

       
        [Test]
        public void FromDataTable_Test()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Col0", typeof(float));
            dt.Columns.Add("Col1", typeof(float));
            dt.Rows.Add(1, 2);
            dt.Rows.Add(3, 4);
            dt.Rows.Add(5, 6);

            Matrix3 A = new Matrix3(); 
            A.FromDataTable(dt);
            Assert.That(A.ToMatLabString(), Is.EqualTo("[1 2;3 4;5 6]"));


        }


        [Test]
        public void ToDataTable_Test()
        {
            Matrix3 A = new Matrix3(); 
            A.ParseMatLab("[1 2;3 4;5 6]");
            DataTable dt = A.ToDataTable();
            Assert.That((float)dt.Rows[0][0], Is.EqualTo(1)) ;
            Assert.That((float)dt.Rows[0][1], Is.EqualTo(2));
            Assert.That((float)dt.Rows[1][0], Is.EqualTo(3));
            Assert.That((float)dt.Rows[1][1], Is.EqualTo(4));
            Assert.That((float)dt.Rows[2][0], Is.EqualTo(5));
            Assert.That((float)dt.Rows[2][1], Is.EqualTo(6));


        }

        [Test]
        [TestCase(2, 3, 4)]
        public void ToFromDataTable_CycleTest(int m, int n, int timesToRun)
        {
            Rectangular rand = new Rectangular();

               for (int i = 0; i < timesToRun; i++)
               {
                   Matrix3 A = rand.Randomfloat(m, n);
                   DataTable dt = A.ToDataTable();
                   Matrix3 AReconstituted = A.FromDataTable(dt);
                   Assert.That(AReconstituted, Is.EqualTo(A));

               }

        }



     

    }
}
