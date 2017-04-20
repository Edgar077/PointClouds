using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKExtension;
using OpenTK;
using System.Diagnostics;
using UnitTestsOpenTK;

namespace UnitTestsOpenTK
{
    [TestFixture]
    [Category("UnitTest")]
    public class KDTreeBaseTest : TestBase
    {
        protected IKDTree tree;
        protected List<VertexKDTree> vectors;
        public KDTreeBaseTest()
        {
            tree = new KDTreeJeremyC();
        }

        /// <summary>
        ///A test for Build
        ///</summary>
        [Test]
        public void _Build_Tree_CubeCorners()
        {
            pointCloudTarget = PointCloud.CreateCube_Corners_StartAt0(1);


            bool build_result = tree.Build(pointCloudTarget);
            Assert.IsTrue(build_result);


        }

        /// <summary>
        ///A test for FindClosestPoint
        ///</summary>
        [Test]
        public void Cube_8p_CheckAll()
        {

            _Build_Tree_CubeCorners();
            vectors = pointCloudTarget.VectorsWithIndex;

            // First test that loaded points can refind themselves.
            Stopwatch st = new Stopwatch();
            st.Start();

            this.pointCloudResult = new PointCloud();
            bool match_result = true;
            for (int i = 0; i < vectors.Count; i++)
            {
                // int index = i;
                int nearest_index = i;
                float nearest_distance = 0f;
                VertexKDTree tmp = tree.FindClosestPoint(vectors[i], ref nearest_distance, ref nearest_index);
                if (nearest_index != i)
                {
                    match_result = false;
                    Console.WriteLine("Vertex: " + vectors[i].ToString());
                    Console.WriteLine("Mis-Match: " + vectors[nearest_index].ToString());
                    Assert.Fail("Mismatched closest pair: " + i.ToString() + "," + nearest_index.ToString());
                }
                pointCloudResult.AddVector(tmp.Vector);
            }
            st.Stop();

            Console.WriteLine("Elapsed Exact Set = {0}", st.Elapsed.ToString());
            Assert.IsTrue(match_result);

        }
        [Test]
        public void Cube_8()
        {


            _Build_Tree_CubeCorners();
            //pointCloudTarget = PointCloud.CreateCube_RandomPointsOnPlanes(1, 1000);
            pointCloudSource = pointCloudTarget.Clone();

            //-------------------
            GlobalVariables.ResetTime();

            this.pointCloudResult = tree.BuildAndFindClosestPoints(pointCloudSource, pointCloudTarget, false);
            GlobalVariables.ShowLastTimeSpan("Search ");

            Assert.IsTrue(tree.MeanDistance == 0);
        }

        /// <summary>
        ///A test for FindClosestPoint
        ///</summary>
        [Test]
        public void CubeTranslated_TakenAlgorithm()
        {

            //-------------------
            pointCloudTarget = PointCloud.CreateCube_Corners_CenteredAt0(1);
            pointCloudSource = pointCloudTarget.Clone();
            this.pointCloudSource.Translate(2, 0, 0);

            this.pointCloudResult = tree.BuildAndFindClosestPoints(pointCloudSource, pointCloudTarget, true);
            GlobalVariables.ShowLastTimeSpan("Search ");



            Assert.IsTrue(tree.MeanDistance < 2.200317f);
        }
        [Test]
        public void CubeTranslated()
        {

            //-------------------
            pointCloudTarget = PointCloud.CreateCube_Corners_CenteredAt0(1);
            pointCloudSource = pointCloudTarget.Clone();
            this.pointCloudSource.Translate(2, 0, 0);


            this.pointCloudResult = tree.BuildAndFindClosestPoints(pointCloudSource, pointCloudTarget, false);
            GlobalVariables.ShowLastTimeSpan("Search ");

            Assert.IsTrue(tree.MeanDistance <= 1.5f);

        }

  

        [Test]
        public void Cube14()
        {
            pointCloudTarget = PointCloud.CreateCube_RandomPointsOnPlanes(1, 1);
            pointCloudSource = pointCloudTarget.Clone();

            //-------------------
            GlobalVariables.ResetTime();

            this.pointCloudResult = tree.BuildAndFindClosestPoints(pointCloudSource, pointCloudTarget, false);
            GlobalVariables.ShowLastTimeSpan("Search ");

            Assert.IsTrue(tree.MeanDistance == 0);
        }
        [Test]
        public void Cube20()
        {
            pointCloudTarget = PointCloud.CreateCube_RandomPointsOnPlanes(1, 2);
            pointCloudSource = pointCloudTarget.Clone();

            //-------------------
            GlobalVariables.ResetTime();

            this.pointCloudResult = tree.BuildAndFindClosestPoints(pointCloudSource, pointCloudTarget, false);
            GlobalVariables.ShowLastTimeSpan("Search ");

            Assert.IsTrue(tree.MeanDistance == 0);
        }
        [Test]
        public void Cube200()
        {


            pointCloudTarget = PointCloud.CreateCube_RandomPointsOnPlanes(1, 32);
            pointCloudSource = pointCloudTarget.Clone();

            //-------------------
            GlobalVariables.ResetTime();

            this.pointCloudResult = tree.BuildAndFindClosestPoints(pointCloudSource, pointCloudTarget, false);
            GlobalVariables.ShowLastTimeSpan("Search ");

            Assert.IsTrue(tree.MeanDistance == 0);
        }
        [Test]
        public void Cube6000()
        {


            pointCloudTarget = PointCloud.CreateCube_RandomPointsOnPlanes(1, 1000);
            pointCloudSource = pointCloudTarget.Clone();

            //-------------------
            GlobalVariables.ResetTime();

            this.pointCloudResult = tree.BuildAndFindClosestPoints(pointCloudSource, pointCloudTarget, false);
            GlobalVariables.ShowLastTimeSpan("Search ");

            Assert.IsTrue(tree.MeanDistance == 0);
        }
        [Test]
        public void Cube200000()
        {


            pointCloudTarget = PointCloud.CreateCube_RandomPointsOnPlanes(1, 34000);
            pointCloudSource = pointCloudTarget.Clone();

            //-------------------
            GlobalVariables.ResetTime();

            this.pointCloudResult = tree.BuildAndFindClosestPoints(pointCloudSource, pointCloudTarget, false);
            GlobalVariables.ShowLastTimeSpan("Search ");

            Assert.IsTrue(tree.MeanDistance == 0);
        }
        [Test]
        public void Cube6000_TakenAlgorithm()
        {

            pointCloudTarget = PointCloud.CreateCube_RandomPointsOnPlanes(1, 1000);
            pointCloudSource = pointCloudTarget.Clone();

            //-------------------
            GlobalVariables.ResetTime();

            this.pointCloudResult = tree.BuildAndFindClosestPoints(pointCloudSource, pointCloudTarget, true);
            GlobalVariables.ShowLastTimeSpan("Search ");


            Assert.IsTrue(tree.MeanDistance == 0);
        }






        [Test]
        public void Face_15000()
        {

            string fileNameLong = pathUnitTests + "\\KinectFace_1_15000.obj";
            pointCloudTarget = PointCloud.FromObjFile(fileNameLong);


            pointCloudSource = pointCloudTarget.Clone();
            //pointCloudSource.Translate(2, 0, 0);

            GlobalVariables.ResetTime();
            //-------------------

            this.pointCloudResult = tree.BuildAndFindClosestPoints(pointCloudSource, pointCloudTarget, false);
            GlobalVariables.ShowLastTimeSpan("Search ");

            float meanDistance = PointCloud.MeanDistance(pointCloudSource, pointCloudResult);
            float meanDistance1 = PointCloud.MeanDistance(pointCloudSource, pointCloudTarget);
            float meanDistance2 = PointCloud.MeanDistance(pointCloudTarget, pointCloudResult);


            //  ShowPointCloudsInWindow_PCAVectors(true);
            Assert.IsTrue(meanDistance == 0);


        }
        [Test]
        public void Face_15000_Centered()
        {


            string fileNameLong = pathUnitTests + "\\KinectFace_1_15000.obj";
            pointCloudTarget = PointCloud.FromObjFile(fileNameLong);
            pointCloudTarget = PointCloud.ShiftByCenterOfMass(pointCloudTarget);

            pointCloudSource = pointCloudTarget.Clone();
            //pointCloudSource.Translate(2, 0, 0);

            GlobalVariables.ResetTime();
            //-------------------

            this.pointCloudResult = tree.BuildAndFindClosestPoints(pointCloudSource, pointCloudTarget, false);
            GlobalVariables.ShowLastTimeSpan("Search ");

            float meanDistance = PointCloud.MeanDistance(pointCloudSource, pointCloudResult);
            float meanDistance1 = PointCloud.MeanDistance(pointCloudSource, pointCloudTarget);
            float meanDistance2 = PointCloud.MeanDistance(pointCloudTarget, pointCloudResult);


            //ShowPointCloudsInWindow_PCAVectors(true);
            Assert.IsTrue(meanDistance == 0);


        }

        [Test]
        public void Bunny_FindItself()
        {


            string fileNameLong = pathUnitTests + "\\Bunny.obj";
            pointCloudTarget = PointCloud.FromObjFile(fileNameLong);


            pointCloudSource = pointCloudTarget.Clone();
            GlobalVariables.ResetTime();
            //-------------------

            this.pointCloudResult = tree.BuildAndFindClosestPoints(pointCloudSource, pointCloudTarget, false);
            GlobalVariables.ShowLastTimeSpan("Search ");


            Assert.IsTrue(tree.MeanDistance == 0);
        }




        [Test]
        public void Bunny_Translate()
        {


            string fileNameLong = pathUnitTests + "\\Bunny.obj";
            pointCloudTarget = PointCloud.FromObjFile(fileNameLong);


            pointCloudSource = pointCloudTarget.Clone();
            pointCloudSource.Translate(2, 0, 0);

            GlobalVariables.ResetTime();
            //-------------------

            this.pointCloudResult = tree.BuildAndFindClosestPoints(pointCloudSource, pointCloudTarget, false);
            GlobalVariables.ShowLastTimeSpan("Search ");

            Assert.IsTrue(tree.MeanDistance < 2);
        }
        [Test]
        public void Bunny_Stitch_00_45()
        {
            string fileNameLong = pathUnitTests + "\\Bunny\\bun000.obj";
            pointCloudTarget = PointCloud.FromObjFile(fileNameLong);

            fileNameLong = pathUnitTests + "\\Bunny\\bun045.obj";
            pointCloudSource = PointCloud.FromObjFile(fileNameLong);



            GlobalVariables.ResetTime();
            //-------------------

            this.pointCloudResult = tree.BuildAndFindClosestPoints(pointCloudSource, pointCloudTarget, false);
            GlobalVariables.ShowLastTimeSpan("Search ");

            //ShowPointCloudsInWindow_PCAVectors(true);

            Assert.IsTrue(tree.MeanDistance < 0.035);
        }
        /// <summary>
        /// Should build the tree displayed in the article:
        /// https://en.wikipedia.org/wiki/K-d_tree
        /// </summary>

        [Test]
        public void WikipediaBuildTests()
        {
            // Should generate the following tree:
            //             7,2
            //              |
            //       +------+-----+
            //      5,4          9,6
            //       |            |
            //   +---+---+     +--+
            //  2,3     4,7   8,1 

            Vector3[] points = new Vector3[]
                          {
                                 new Vector3 (7, 2 ,0), new Vector3(5, 4, 0 ), new Vector3 (2, 3, 0 ),
                                 new Vector3 ( 4, 7, 0), new Vector3 ( 9, 6, 0), new Vector3 ( 8, 1, 0) 
                          };

            List<Vector3> listV = new List<Vector3>(points);

            pointCloudTarget = PointCloud.FromListVector3(listV);
            pointCloudSource = pointCloudTarget.Clone();
            pointCloudSource.ShuffleVectors();
            //-------------------
            GlobalVariables.ResetTime();

            this.pointCloudResult = tree.BuildAndFindClosestPoints(pointCloudSource, pointCloudTarget, false);
        //    this.pointCloudResult = tree.BuildAndFindClosestPoints_NotParallel(pointCloudSource, pointCloudTarget, false);
            GlobalVariables.ShowLastTimeSpan("Search ");

            Assert.IsTrue(tree.MeanDistance == 0);

            for(int i = 0; i < pointCloudTarget.Vectors.Length; i++ )
            {
                //Assert.That(pointCloudResult.Vectors[i], Is.EqualTo(pointCloudTarget.Vectors[i]));
                Assert.IsTrue(pointCloudSource.Vectors[i].Equals(pointCloudResult.Vectors[i]));


            }

            
        }
    }
}
