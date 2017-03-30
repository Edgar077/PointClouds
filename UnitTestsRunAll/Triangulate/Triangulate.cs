using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKExtension;
using OpenTK;
using System.Diagnostics;
using UnitTestsOpenTK;
using OpenTKExtension.Triangulation;

//OpenTKExtension.DelaunayVoronoi;
namespace UnitTestsOpenTK
{
    [TestFixture]
    [Category("UnitTest")]
    public class Triangulate : TestBase
    {
        
        public Triangulate()
        {
            
        }
      
        [Test]
        public void Delaunay_Simple()
        {
          
            List<Vector3> listVectors = new List<Vector3>() {new Vector3(158, 507, 0), new Vector3(142, 393, 0), new Vector3(100, 317, 0), new Vector3(92, 215, 0), new Vector3(98, 197, 0), new Vector3(151, 261, 0), new Vector3(143, 244, 0), new Vector3(170, 255, 0), new Vector3(209, 272, 0),
                new Vector3(198, 257, 0), new Vector3(214, 243, 0), new Vector3(223, 223, 0), new Vector3(214, 199, 0), new Vector3(234, 201, 0), new Vector3(264, 196, 0), new Vector3(159, 175, 0), new Vector3(158, 148, 0), new Vector3(143, 144, 0), new Vector3(141, 93, 0),
                new Vector3(166, 73, 0), new Vector3(136, 32, 0), new Vector3(179, 29, 0), new Vector3(207, 39, 0), new Vector3(233, 47, 0), new Vector3(257, 61, 0), new Vector3(267, 43, 0), new Vector3(271, 89, 0), new Vector3(292, 81, 0), new Vector3(234, 106, 0),
                new Vector3(214, 106, 0), new Vector3(221, 321, 0), new Vector3(235, 313, 0), new Vector3(247, 296, 0), new Vector3(265, 341, 0), new Vector3(283, 326, 0), new Vector3(307, 329, 0), new Vector3(320, 317, 0), new Vector3(340, 286, 0), new Vector3(327, 266, 0),
                new Vector3(322, 206, 0), new Vector3(337, 194, 0), new Vector3(348, 163, 0), new Vector3(320, 161, 0), new Vector3(370, 142, 0), new Vector3(350, 129, 0), new Vector3(389, 108, 0), new Vector3(355, 341, 0), new Vector3(384, 381, 0), new Vector3(421, 423, 0),
                new Vector3(441, 414, 0), new Vector3(391, 307, 0), new Vector3(416, 301, 0), new Vector3(391, 283, 0), new Vector3(490, 238, 0), new Vector3(460, 225, 0), new Vector3(474, 174, 0), new Vector3(453, 145, 0), new Vector3(467, 104, 0), new Vector3(543, 255, 0),
                new Vector3(618, 242, 0), new Vector3(611, 129, 0)};

            PointCloud pc = PointCloud.FromListVector3(listVectors);

            OpenTKExtension.Triangulation.Mesh m =  OpenTKExtension.Triangulation.Mesh.Triangulate(pc, 5);
            
            pc.CreateIndicesFromTriangles(m.Triangles);
            
            ShowPointCloud(pc);

            //Assert.IsTrue(build_result);


        }

    

        [Test]
        public void Delaunay_Scanned()
        {

            string path = AppDomain.CurrentDomain.BaseDirectory + "Models\\UnitTests";
            PointCloud pc = PointCloud.FromObjFile(path + "\\KinectFace_1_15000.obj");

            OpenTKExtension.Triangulation.Mesh m = OpenTKExtension.Triangulation.Mesh.Triangulate(pc, 6);

           
            pc.CreateIndicesFromTriangles(m.Triangles);

            ShowPointCloud(pc);
            
            

        }
        /// <summary>
        ///A test for Build
        ///</summary>
        //[Test]
        //public void Delaunay_Junk()
        //{
        //    string path = AppDomain.CurrentDomain.BaseDirectory + "Models\\UnitTests";


        //    List<Vector3> listVectors = new List<Vector3>() {new Vector3(158, 507, 0), new Vector3(142, 393, 0), new Vector3(100, 317, 0), new Vector3(92, 215, 0), new Vector3(98, 197, 0), new Vector3(151, 261, 0), new Vector3(143, 244, 0), new Vector3(170, 255, 0), new Vector3(209, 272, 0),
        //        new Vector3(198, 257, 0), new Vector3(214, 243, 0), new Vector3(223, 223, 0), new Vector3(214, 199, 0), new Vector3(234, 201, 0), new Vector3(264, 196, 0), new Vector3(159, 175, 0), new Vector3(158, 148, 0), new Vector3(143, 144, 0), new Vector3(141, 93, 0),
        //        new Vector3(166, 73, 0), new Vector3(136, 32, 0), new Vector3(179, 29, 0), new Vector3(207, 39, 0), new Vector3(233, 47, 0), new Vector3(257, 61, 0), new Vector3(267, 43, 0), new Vector3(271, 89, 0), new Vector3(292, 81, 0), new Vector3(234, 106, 0),
        //        new Vector3(214, 106, 0), new Vector3(221, 321, 0), new Vector3(235, 313, 0), new Vector3(247, 296, 0), new Vector3(265, 341, 0), new Vector3(283, 326, 0), new Vector3(307, 329, 0), new Vector3(320, 317, 0), new Vector3(340, 286, 0), new Vector3(327, 266, 0),
        //        new Vector3(322, 206, 0), new Vector3(337, 194, 0), new Vector3(348, 163, 0), new Vector3(320, 161, 0), new Vector3(370, 142, 0), new Vector3(350, 129, 0), new Vector3(389, 108, 0), new Vector3(355, 341, 0), new Vector3(384, 381, 0), new Vector3(421, 423, 0),
        //        new Vector3(441, 414, 0), new Vector3(391, 307, 0), new Vector3(416, 301, 0), new Vector3(391, 283, 0), new Vector3(490, 238, 0), new Vector3(460, 225, 0), new Vector3(474, 174, 0), new Vector3(453, 145, 0), new Vector3(467, 104, 0), new Vector3(543, 255, 0),
        //        new Vector3(618, 242, 0), new Vector3(611, 129, 0)};

        //    PointCloud pc = PointCloud.FromListVector3(listVectors);
        //    List<TriangleVectors> listTrianglesDelaunay = Delaunay.DelaunayTriangulation(new List<Vector3>(pc.Vectors));






        //    List<Triangle> listTriangles = new List<Triangle>();
        //    List<Vector3> newVectors = new List<Vector3>();

        //    for (int i = 0; i < listTrianglesDelaunay.Count; i++)
        //    {
        //        Triangle t = new Triangle(newVectors.Count, newVectors.Count + 1, newVectors.Count + 2);
        //        listTriangles.Add(t);
        //        newVectors.Add(listTrianglesDelaunay[i].P1);
        //        newVectors.Add(listTrianglesDelaunay[i].P2);
        //        newVectors.Add(listTrianglesDelaunay[i].P3);

        //    }
        //    List<Vector3> newColors = new List<Vector3>();
        //    //for(int i = 0; i < newVectors.Count; i)
        //    //merge the two clouds
        //    List<Vector3> oldVectors = new List<Vector3>(pc.Vectors);
        //    List<Vector3> oldColors = new List<Vector3>(pc.Colors);

        //    oldVectors.AddRange(newVectors);

        //    PointCloud pcNew = PointCloud.FromListVector3(oldVectors);
        //    pcNew.Triangles = listTriangles;
        //    pcNew.CreateIndicesFromTriangles();
        //    //ShowPointCloud(pcNew);
        //    ShowPointCloud(pcNew);

        //    //Assert.IsTrue(build_result);


        //}

        ///// <summary>
        /////A test for Build
        /////</summary>
        //[Test]
        //public void Delaunay_Scan()
        //{
        //    string path = AppDomain.CurrentDomain.BaseDirectory + "Models\\UnitTests";
        //    PointCloud pc = PointCloud.FromObjFile(path + "\\KinectFace_1_15000.obj");


        //    List<TriangleVectors> listTrianglesDelaunay = Delaunay.DelaunayTriangulation(new List<Vector3>(pc.Vectors));





        //    List<Triangle> listTriangles = new List<Triangle>();
        //    List<Vector3> newVectors = new List<Vector3>();

        //    for (int i = 0; i < listTrianglesDelaunay.Count; i++)
        //    {
        //        Triangle t = new Triangle(newVectors.Count, newVectors.Count + 1, newVectors.Count +2);
        //        listTriangles.Add(t);
        //        newVectors.Add(listTrianglesDelaunay[i].P1);
        //        newVectors.Add(listTrianglesDelaunay[i].P2);
        //        newVectors.Add(listTrianglesDelaunay[i].P3);

        //    }
        //    List<Vector3> newColors = new List<Vector3>();
        //    //for(int i = 0; i < newVectors.Count; i)
        //    //merge the two clouds
        //    List<Vector3> oldVectors = new List<Vector3>(pc.Vectors);
        //    List<Vector3> oldColors = new List<Vector3>(pc.Colors);

        //    oldVectors.AddRange(newVectors);

        //    PointCloud pcNew = PointCloud.FromListVector3(oldVectors);
        //    pcNew.Triangles = listTriangles;
        //    pcNew.CreateIndicesFromTriangles();
        //    //ShowPointCloud(pcNew);
        //    ShowPointCloud(pcNew);

        //    //Assert.IsTrue(build_result);


        //}


        //[Test]
        //public void Cube_8()
        //{
        //    string path = AppDomain.CurrentDomain.BaseDirectory + "Models\\UnitTests";
        //    PointCloud pc = PointCloud.FromObjFile(path + "\\KinectFace_1_15000.obj");
        //    IList<Vector3> listV = new List<Vector3> (pc.Vectors);

        //    OpenTKExtension.DelaunayVoronoi.Delaunay.DelaunayTriangulation(listV);



        //}


    }
}
