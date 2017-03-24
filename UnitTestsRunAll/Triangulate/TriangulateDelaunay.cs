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
    public class TriangulateDelaunay : TestBase
    {
        
        public TriangulateDelaunay()
        {
            
        }
       
        /// <summary>
        ///A test for Build
        ///</summary>
        [Test]
        public void _Build_Tree_CubeCorners()
        {
            pointCloudTarget = PointCloud.CreateCube_Corners_StartAt0(1);
            

           
            //Assert.IsTrue(build_result);

            
        }

      
        [Test]
        public void Cube_8()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "Models\\UnitTests";
            PointCloud pc = PointCloud.FromObjFile(path + "\\KinectFace_1_15000.obj");
            IList<Vector3> listV = new List<Vector3> (pc.Vectors);

            OpenTKExtension.DelaunayVoronoi.Delaunay.DelaunayTriangulation(listV);



        }
       
      
    }
}
