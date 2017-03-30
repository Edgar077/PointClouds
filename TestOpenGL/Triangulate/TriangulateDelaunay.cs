using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKExtension;
using OpenTK;
using System.Diagnostics;
using UnitTestsOpenTK;
using OpenTKExtension.DelaunayVoronoi;
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
        public void Delaunay_Scan()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "Models\\UnitTests";
            PointCloud pc = PointCloud.FromObjFile(path + "\\KinectFace_1_15000.obj");


            List<TriangleVectors> listTrianglesDelaunay = Delaunay.DelaunayTriangulation(new List<Vector3>(pc.Vectors));

            



            List<Triangle> listTriangles = new List<Triangle>();
            List<Vector3> newVectors = new List<Vector3>();

            for (int i = 0; i < listTrianglesDelaunay.Count; i++)
            {
                Triangle t = new Triangle(newVectors.Count, newVectors.Count + 1, newVectors.Count +2);
                listTriangles.Add(t);
                newVectors.Add(listTrianglesDelaunay[i].P1);
                newVectors.Add(listTrianglesDelaunay[i].P2);
                newVectors.Add(listTrianglesDelaunay[i].P3);

            }
            List<Vector3> newColors = new List<Vector3>();
            //for(int i = 0; i < newVectors.Count; i)
            //merge the two clouds
            List<Vector3> oldVectors = new List<Vector3>(pc.Vectors);
            List<Vector3> oldColors = new List<Vector3>(pc.Colors);

            oldVectors.AddRange(newVectors);

            PointCloud pcNew = PointCloud.FromListVector3(oldVectors);
            pcNew.Triangles = listTriangles;
            pcNew.CreateIndicesFromTriangles();
            //ShowPointCloud(pcNew);
            ShowPointCloud(pc);

            //Assert.IsTrue(build_result);


        }


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
