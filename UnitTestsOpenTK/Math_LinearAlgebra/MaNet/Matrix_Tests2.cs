//// original from the Java matrix package JAMA http://math.nist.gov/javanumerics/jama/
//// converted to C# by Ken Johnson, added units tests, (2010)
//// http://www.codeproject.com/Articles/91458/MaNet-A-matrix-library-for-NET-Rational-Computing
////adapted to use OpenTK 

//using System;
//using NUnit.Framework;
//using OpenTK;
//using OpenTKExtension;

//namespace UnitTestsOpenTK.LinearAlgebra
//{
//    [TestFixture]
//  public  class Matrix_Tests2
//    {
//    [Test]
//     public void Solve3by3()
//    {
//        // For equations
//        // 2x + y + z  = 5
//        // 4x -6y      = 2
//        //-2x + 7y + 2 = 9
//        // with solution x = 1, y = 1, z = 2

//        String strMat = @"2  1  1
//                          4 -6  0
//                         -2  7  2";

//        String strVals = @"5
//                          -2
//                           9";

//        String strExpectedSoln = @"1
//                                   1
//                                   2";

//        Matrix3 mat = new Matrix3();
//        mat.Parse(strMat);
//        Matrix3 vals = mat.Parse(strVals);

//        Matrix3 expectedSoln = mat.Parse(strExpectedSoln);

//        Matrix3 soln = mat.Solve(vals);
        
//       //Checks that solution solves matrix equation. 
//       //Note that I can do this even if I don't know the solution.
//        Matrix3 test = Matrix3.Mult(mat, soln);

//        Assert.That(test.ToArray(), Is.EqualTo(vals.ToArray())); 

//        //Check against expected solution
//        Assert.That(soln.ToArray(), Is.EqualTo(expectedSoln.ToArray()));
//    }


//    [Test]
//    public void LeastSquares4by2()
//    {
//        // For equations
//        // a +  b =  6
//        // a + 2b =  5
//        // a + 3b =  7
//        // a + 4b = 10
//        // with leastsqure approximate solution a=3.5 , b=1.4

//        String strMat = @"1  1  
//                          1  2   
//                          1  3  
//                          1  4";

//        String strVals = @"6
//                           5
//                           7
//                          10";

//        String strExpectedSoln = @"3.5
//                                   1.4";


//        Matrix3 mat = new Matrix3();
//        mat.Parse(strMat);
//        Matrix3 vals = mat.Parse(strVals);

//        Matrix3 expectedSoln = mat.Parse(strExpectedSoln);


//        Matrix3 soln = mat.Solve(vals);

//        //Check against expected solution
//        Assert.That(soln.ToArray(), Is.EqualTo(expectedSoln.ToArray()).Within(.001));
//    }

//    }
//}
