//using System;
//using NUnit.Framework;
//using System.Collections.Generic;
//using System.Text;
//using System.Drawing;

//using OpenTKExtension;
//using OpenTK;

//namespace UnitTestsOpenTK.Triangulation
//{
//    [TestFixture]
//    [Category("UnitTest")]
//    public class TriangulateKDTree : TestBase
//    {

//        [Test]
//        public void Square()
//        {

            
//            List<Vector3> listVectors = Example3DModels.Square(1, 1, 0);
//            //listVectors.AddRange(Example3DModels.Square(1, 1.5, 0));

//            PointCloud pcl = PointCloud.FromVector3List(listVectors);
//            pcl.TriangulateVertices_Rednaxela(0f);

//            pcl.ToObjFile(TestBase.Path + "\\resultTriangulation.obj");


//            ShowPointCloudForOpenGL(pcl);


//        }
//        [Test]
//        public void Cuboid_100()
//        {

//            this.pointCloudSource = PointCloud.CreateCuboid(20f, 40f, 100);
//            PointCloud pc = pointCloudSource;

//            pc.TriangulateVertices_Rednaxela(0f);
//            pc.ToObjFile(TestBase.Path + "\\resultTriangulation.obj");

////            myModel.Pointcloud.ResizeVerticesTo1();
//            this.ShowResultsInWindow_Cube(true);
//        }

       

//        [Test]
//        public void Cube_Corners()
//        {
            
//            List<Vector3> listVectors = Example3DModels.Cube_Corners(1, 1, 1);
//            PointCloud pcl = PointCloud.FromVector3List(listVectors);
//            pcl.TriangulateVertices_Rednaxela(0f);

          

//            ShowPointCloudForOpenGL(pcl);

            
//        }
//        [Test]
//        public void Cube_56()
//        {

//            PointCloud pcl =  Example3DModels.CreateCube_RegularGrid_Empty(1, 3);
//            pcl.TriangulateVertices_Rednaxela(0f);
//            ShowPointCloudForOpenGL(pcl);


//        }
//        //[Test]
//        //public void Cube_98()
//        //{

            
//        //    PointCloud pcl = Example3DModels.CreateCube_RegularGrid_Empty(1, 4);
//        //    pcl.TriangulateVertices_Rednaxela(0f);
//        //    ShowPointCloudForOpenGL(pcl);


//        //}
//        //[Test]
//        //public void Cube_16()
//        //{
//        //    CreateCube(4);
//        //    pointCloudSource.TriangulateVertices_Rednaxela(0f);

//        //    this.ShowResultsInWindow_Cube(true);

//        //}
//        //[Test]
//        //public void Cube_32()
//        //{
//        //    CreateCube(8);
//        //    pointCloudSource.TriangulateVertices_Rednaxela(0f);

//        //    this.ShowResultsInWindow_Cube(true);

//        //}

    
       

     
     
//    }
//}
