using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKExtension;
using OpenTK;
using Automated;
using System.Diagnostics;
using UnitTestsOpenTK;
namespace Automated.KDTree
{
    [TestFixture]
    [Category("UnitTest")]
    public class KDTreeComparison : TestBase
    {

        protected IKDTree tree;
        protected List<VertexKDTree> vectors;

        [SetUp]
        public void Prepare()
        {
            //treeJerome = new KDTreeJeremyC();

            //tree = new KDTreeKennell();
        }
        private void PerformTest(bool alsoBruteForce, bool taken)
        {
            System.Diagnostics.Debug.WriteLine("Number of points: " + pointCloudSource.Vectors.Length.ToString());

            if (alsoBruteForce)
            {
                GlobalVariables.ResetTime();
                //--------------------------
                tree = new KDTreeBruteForce();
                tree.TakenAlgorithm = taken;
                tree.Build(pointCloudTarget);
                GlobalVariables.ShowLastTimeSpan("Build BruteForce :");
                tree.FindClosestPointCloud_NotParallel(pointCloudSource);
                GlobalVariables.ShowLastTimeSpan("--> Find BruteForce          :");
                tree.FindClosestPointCloud_Parallel(pointCloudSource);
                GlobalVariables.ShowLastTimeSpan("--> Find BruteForce Parallel :");

                Assert.IsTrue(tree.MeanDistance == 0);

            }

            //--------------------------
            GlobalVariables.ResetTime();
            tree = new KDTreeJeremyC();
            tree.TakenAlgorithm = taken;
            tree.Build(pointCloudTarget);
            GlobalVariables.ShowLastTimeSpan("Build Jerome :");
            tree.FindClosestPointCloud_NotParallel(pointCloudSource);
            GlobalVariables.ShowLastTimeSpan("--> Find Jerome                :");
            tree.FindClosestPointCloud_Parallel(pointCloudSource);
            GlobalVariables.ShowLastTimeSpan("--> Find Jerome Parallel       :");

            Assert.IsTrue(tree.MeanDistance == 0);


            //--------------------------
            GlobalVariables.ResetTime();
            tree = new KDTreeKennell();
            tree.TakenAlgorithm = taken;
            tree.Build(pointCloudTarget);
            GlobalVariables.ShowLastTimeSpan("Build Kennell :");
            tree.FindClosestPointCloud_NotParallel(pointCloudSource);
            GlobalVariables.ShowLastTimeSpan("--> Find Kennell               :");
            tree.FindClosestPointCloud_Parallel(pointCloudSource);
            GlobalVariables.ShowLastTimeSpan("--> Find Kennell Parallel      :");

            Assert.IsTrue(tree.MeanDistance == 0);





        }
        //private void PerformTest_Parallel(bool alsoBruteForce)
        //{

        //    if (alsoBruteForce)
        //    {
        //        //-------------------
        //        GlobalVariables.ResetTime();
        //        tree = new KDTreeBruteForce();
        //        this.pointCloudResult = tree.BuildAndFindClosestPoints(pointCloudSource, pointCloudTarget, false);
        //        GlobalVariables.ShowLastTimeSpan("BruteForce - Parallel:");
        //        Assert.IsTrue(tree.MeanDistance == 0);
        //    }


        //    //--------------------------
        //    GlobalVariables.ResetTime();
        //    tree = new KDTreeJeremyC();
        //    this.pointCloudResult = tree.BuildAndFindClosestPoints(pointCloudSource, pointCloudTarget, false);
        //    GlobalVariables.ShowLastTimeSpan("JeremyC parallel:");
        //    Assert.IsTrue(tree.MeanDistance == 0);

         

        //    //--------------------------
        //    GlobalVariables.ResetTime();
        //    tree = new KDTreeKennell();
        //    this.pointCloudResult = tree.BuildAndFindClosestPoints(pointCloudSource, pointCloudTarget, false);
        //    GlobalVariables.ShowLastTimeSpan("Kennell parallel :");
        //    Assert.IsTrue(tree.MeanDistance == 0);
            





        //}
        [Test]
        public void Cube100()
        {
            pointCloudTarget = PointCloud.CreateCube_RandomPointsOnPlanes(1, 16);
            pointCloudSource = pointCloudTarget.Clone();
            PerformTest(true, false);

            
        }
        [Test]
        public void Cube1000()
        {


            pointCloudTarget = PointCloud.CreateCube_RandomPointsOnPlanes(1, 160);
            pointCloudSource = pointCloudTarget.Clone();
            PerformTest(true, false);
            //PerformTest_Parallel(false);
        }
        [Test]
        public void Cube1000_Taken()
        {


            pointCloudTarget = PointCloud.CreateCube_RandomPointsOnPlanes(1, 160);
            pointCloudSource = pointCloudTarget.Clone();
            PerformTest(false, true);
            //PerformTest_Parallel(false);
        }
        [Test]
        public void Cube10000()
        {


            pointCloudTarget = PointCloud.CreateCube_RandomPointsOnPlanes(1, 1600);
            pointCloudSource = pointCloudTarget.Clone();
            PerformTest(true, false);
            //PerformTest_Parallel(false);
        }
        [Test]
        public void Cube100000()
        {


            pointCloudTarget = PointCloud.CreateCube_RandomPointsOnPlanes(1, 16500);
            pointCloudSource = pointCloudTarget.Clone();
            PerformTest(true, false);
            //PerformTest_Parallel(false);
        }
     
        [Test]
        public void Cube1000000()
        {


            pointCloudTarget = PointCloud.CreateCube_RandomPointsOnPlanes(1, 167000);
            pointCloudSource = pointCloudTarget.Clone();
            PerformTest(false, false);
            //PerformTest_Parallel(false);
        }
        [Test]
        public void Cube10000000()
        {


            pointCloudTarget = PointCloud.CreateCube_RandomPointsOnPlanes(1, 1670000);
            pointCloudSource = pointCloudTarget.Clone();
            PerformTest(false, false);
            //PerformTest_Parallel(false);
        }

        [Test]
        public void Cube6000_TakenAlgorithm()
        {

            pointCloudTarget = PointCloud.CreateCube_RandomPointsOnPlanes(1, 1000);
            pointCloudSource = pointCloudTarget.Clone();
            PerformTest(false, false);
        }
        [Test]
        public void Bunny_FindItself()
        {


            string fileNameLong = pathUnitTests + "\\Bunny.obj";
            pointCloudTarget = PointCloud.FromObjFile(fileNameLong);
            pointCloudSource = pointCloudTarget.Clone();

            PerformTest(false, false);
            //PerformTest_Parallel(false);
        }
    }
}
