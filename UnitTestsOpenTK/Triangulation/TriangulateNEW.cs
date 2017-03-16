//using System;
//using NUnit.Framework;
//using System.Collections.Generic;
//using System.Text;
//using System.Drawing;

//using OpenTKExtension;
//using OpenTK;
//using VoronoiFortune;

//namespace UnitTestsOpenTK.Triangulation
//{
//    [TestFixture]
//    [Category("UnitTest")]
//    public class TriangulateTest2 : TestBase
//    {
        
//          [Test]
//        public void KDTree()
//        {


//            string path = AppDomain.CurrentDomain.BaseDirectory + "Models\\UnitTests";
//            PointCloud pcl = PointCloud.FromObjFile(path + "\\Gesicht.obj");

        
//              //EDGAR INDEX
//            //for (int i = 0; i < pcl.Vectors.Length; i++)
//            //{
//            //    pcl.Vectors[i].Index = i;

//            //}

//            pcl.TriangulateVertices_Rednaxela(0.02f);


//            pcl.CreateIndicesFromTriangles();
//            //pcl.Triangles = triangles;

//            pcl.ToObjFile(path + "\\GesichtTriangulated.obj");


//            TestForm fOTK = new TestForm();
//            fOTK.OpenGL_UControl.ShowPointCloud(pcl);
//            fOTK.ShowDialog();



//        }

//        [Test]
//        public void FaceKinect()
//        {


//            string path = AppDomain.CurrentDomain.BaseDirectory + "Models\\UnitTests";
//            PointCloud pcl = PointCloud.FromObjFile(path + "\\Gesicht.obj");


//            //EDGAR INDEX
//            //for (int i = 0; i < pcl.Vectors.Length; i++)
//            //{
//            //    pcl.Vectors[i].Index = i;

//            //}
//            pcl.TriangulateKinect();

//            pcl.CreateIndicesFromTriangles();
//            //pcl.Triangles = triangles;

//            pcl.ToObjFile(path + "\\GesichtTriangulated.obj");


//            TestForm fOTK = new TestForm();
//            fOTK.OpenGL_UControl.ShowPointCloud(pcl);
//            fOTK.ShowDialog();



//        }

//        [Test]
//        public void FaceFortune()
//        {


//            string path = AppDomain.CurrentDomain.BaseDirectory + "Models\\UnitTests";
//            PointCloud pcl = PointCloud.FromObjFile(path + "\\Gesicht.obj");

//            //EDGAR INDEX
//            //for (int i = 0; i < pcl.Vectors.Length; i++)
//            //{
//            //    pcl.Vectors[i].Index = i;

//            //}

         
//            List<VectorWithIndex> listPoints = pcl.VectorsWithIndex;
//            List<Triangle> triangles = FortuneVoronoi.Fortune.GetDelaunayTriangulation(listPoints);

//            pcl.Triangles = triangles;

//            pcl.CreateIndicesFromTriangles();
            
//            pcl.ToObjFile(path + "\\GesichtTriangulated.obj");


//            TestForm fOTK = new TestForm();
//            fOTK.OpenGL_UControl.ShowPointCloud(pcl);
//            fOTK.ShowDialog();



//        }

//        [Test]
//        public void FaceDelaunay()
//        {


//            string path = AppDomain.CurrentDomain.BaseDirectory + "Models\\UnitTests";
//            PointCloud pcl = PointCloud.FromObjFile(path + "\\Gesicht.obj");

//            //EDGAR INDEX
//            //for(int i = 0; i < pcl.Vectors.Length; i++)
//            //{
//            //    pcl.Vectors[i].Index = i;
                
//            //}
           
//            OpenTKExtension.Triangulation.Mesh m = OpenTKExtension.Triangulation.TriangulateTest.Triangulate(pcl, 2);
            
//            pcl.CreateIndicesFromNewTriangles(m.Triangles);

//            pcl.ToObjFile(path + "\\GesichtTriangulated.obj");

//            TestForm fOTK = new TestForm();
//            fOTK.OpenGL_UControl.ShowPointCloud(pcl);
//            fOTK.ShowDialog();

           
//        }
//            [Test]
//        public void Face1()
//        {


//            string path = AppDomain.CurrentDomain.BaseDirectory + "Models\\UnitTests";
//            PointCloud pcl = PointCloud.FromObjFile(path + "\\Gesicht.obj");


//            //EDGAR INDEX
//            //for(int i = 0; i < pcl.Vectors.Length; i++)
//            //{
//            //    pcl.Vectors[i].Index = i;
                
//            //}

//            List<EdgeFortune> listEdges;

//            Voronoi voronoi = new Voronoi(0.1f);

//            List<VectorWithIndex> listVectors = pcl.VectorsWithIndex;

//            listEdges = voronoi.GenerateVoronoi(listVectors);

//            List<Triangle> listTriangle = new List<Triangle>();
//            for (int i = 0; i < listEdges.Count; i += 3)
//            {
//                EdgeFortune edge = listEdges[i];

//                Triangle t = new Triangle();
//                //t.IndVertices.Add(edge.PointIndex1);
//                //edge.PointIndex1].Vector, pointCloudSource[edge.PointIndex2].Vector));

//                //t.IndVertices.Add(cell.Vertices[0].IndexInModel);
//                //t.IndVertices.Add(cell.Vertices[1].IndexInModel);
//                //t.IndVertices.Add(cell.Vertices[2].IndexInModel);
//                listTriangle.Add(t);

//                //myLines.Add(pointCloud[edge.PointIndex1]);
//                //myLinesTo.Add(pointCloud[edge.PointIndex2]);

//            }
//            //OpenTKExtension.Triangulation.Mesh m = OpenTKExtension.Triangulation.TriangulateTest.Triangulate(pcl);
//            //pcl.CreateIndicesFromNewTriangles(m.Triangles);

//            TestForm fOTK = new TestForm();
//            fOTK.OpenGL_UControl.ShowPointCloud(pcl);
//            fOTK.ShowDialog();



//        }
        

     
     
//    }
//}
