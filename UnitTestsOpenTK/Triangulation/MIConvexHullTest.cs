using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using OpenTKExtension;
using OpenTK;

using MIConvexHull;

namespace UnitTestsOpenTK.Triangulation.MIConvexHull
{
    public class MIConvexHullTest : TestBase
    {
        //private List<Vertex2D> vertices;
       

        [Test]
        public void Face_Delaunay_New()
        {

            string fileNameLong = pathUnitTests + "\\KinectFace_1_15000.obj";
            pointCloudSource = IOUtils.ReadObjFile_ToPointCloud(fileNameLong);

            PointCloud pointCloudTemp = PointCloud.CloneAll(pointCloudSource);
            for(int i = 0; i < pointCloudTemp.Count; i++)
            {
                pointCloudTemp[i].Vector.Z = 0;
            }

            //List<Vertex2D> pointCloudDelaunay = new List<Vertex2D>();

            //for (int i = 0; i < pointCloudSource.Count; i++)
            //{
            //    Vertex2D v = new Vertex2D(i, pointCloudSource[i].Vector.X, pointCloudSource[i].Vector.Y);
            //    pointCloudDelaunay.Add(v);

            //}
            //--------------------------------


            //------------------------------------------------

            //VoronoiMesh<Vertex2D, Cell2D, VoronoiEdge<Vertex2D, Cell2D>> voronoiMesh;
            VoronoiMesh<Vertex, CellVertex, VoronoiEdge<Vertex, CellVertex>> voronoiMesh;

            voronoiMesh = VoronoiMesh.Create<Vertex, CellVertex>(pointCloudTemp);
            List<Triangle> listTriangle = new List<Triangle>();
            int indexTriangle = 0;
            foreach (CellVertex cell in voronoiMesh.Cells)
            {
                Triangle t = new Triangle();

                t.IndVertices.Add(Convert.ToUInt32(cell.Vertices[0].Index));
                t.IndVertices.Add(Convert.ToUInt32(cell.Vertices[1].Index));
                t.IndVertices.Add(Convert.ToUInt32(cell.Vertices[2].Index));
                listTriangle.Add(t);

                cell.Vertices[0].IndexTriangles.Add(indexTriangle);
                cell.Vertices[1].IndexTriangles.Add(indexTriangle);
                cell.Vertices[2].IndexTriangles.Add(indexTriangle);
                indexTriangle++;

                //Vertex2D[] vert = cell.Vertices;
            }

            //set triangle indices for normal calculation
            for (int i = 0; i < pointCloudTemp.Count; i++)
            {
                pointCloudSource[i].IndexTriangles = pointCloudTemp[i].IndexTriangles;
                
            }

            //-------------------
            Model myModel = new Model();
            myModel.PointCloud = pointCloudSource;

            
            //myModel.Triangles = listTriangle;
            //myModel.PointCloud.CalculateCentroidBoundingBox();

            //myModel.CalculateNormals_FromExistingTriangles();

            
           

            myModel.PointCloud.ToObjFile(pathUnitTests, "1_triangulated.obj");
            ShowModel(myModel);

        }

        [Test]
        public void Face_Delaunay()
        {

            string fileNameLong = pathUnitTests + "\\KinectFace_1_15000.obj";
            pointCloudSource = IOUtils.ReadObjFile_ToPointCloud(fileNameLong);


            List<Vertex2D> pointCloudDelaunay = new List<Vertex2D>();

            for (int i = 0; i < pointCloudSource.Count; i++)
            {
                Vertex2D v = new Vertex2D(i, pointCloudSource[i].Vector.X, pointCloudSource[i].Vector.Y);
                pointCloudDelaunay.Add(v);

            }
            //--------------------------------


            //------------------------------------------------

            VoronoiMesh<Vertex2D, Cell2D, VoronoiEdge<Vertex2D, Cell2D>> voronoiMesh;
            voronoiMesh = VoronoiMesh.Create<Vertex2D, Cell2D>(pointCloudDelaunay);
            List<Triangle> listTriangle = new List<Triangle>();
            foreach (Cell2D cell in voronoiMesh.Cells)
            {
                Triangle t = new Triangle();

                t.IndVertices.Add(Convert.ToUInt32(cell.Vertices[0].IndexInModel));
                t.IndVertices.Add(Convert.ToUInt32(cell.Vertices[1].IndexInModel));
                t.IndVertices.Add(Convert.ToUInt32(cell.Vertices[2].IndexInModel));
                listTriangle.Add(t);
                //Vertex2D[] vert = cell.Vertices;
            }

            ////adapt Vertex3d for normal calculation
            //for (int i = 0; i < pointCloudSource.Count; i++)
            //{
            //    Vertex v = pointCloudSource[i]
            //    Vertex2D v = new Vertex2D(i, pointCloudSource[i].Vector.X, pointCloudSource[i].Vector.Y);
            //    pointCloudDelaunay.Add(v);

            //}

            //-------------------
            Model myModel = new Model();
            myModel.PointCloud = pointCloudSource;
            //myModel.CalculateNormals_FromExistingTriangles();

            //myModel.Triangles = listTriangle;
            //myModel.PointCloud.CalculateCentroidBoundingBox();


            myModel.PointCloud.ToObjFile( pathUnitTests, "1_triangulated.obj");
            ShowModel(myModel);

        }
        [Test]
        public void Face_Voronoi_TODO()
        {

            string fileNameLong = pathUnitTests + "\\KinectFace_1_15000.obj";
            pointCloudSource = IOUtils.ReadObjFile_ToPointCloud(fileNameLong);


            List<Vertex2D> pointCloudDelaunay = new List<Vertex2D>();

            for (int i = 0; i < pointCloudSource.Count; i++)
            {
                Vertex2D v = new Vertex2D(i, pointCloudSource[i].Vector.X, pointCloudSource[i].Vector.Y);
                pointCloudDelaunay.Add(v);

            }


            VoronoiMesh<Vertex2D, Cell2D, VoronoiEdge<Vertex2D, Cell2D>> voronoiMesh;
            voronoiMesh = VoronoiMesh.Create<Vertex2D, Cell2D>(pointCloudDelaunay);
            //List<Triangle> listTriangle = new List<Triangle>();

            foreach (var edge in voronoiMesh.Edges)
            {
                var from = edge.Source.Circumcenter;
                var to = edge.Target.Circumcenter;
                //drawingCanvas.Children.Add(new Line { X1 = from.X, Y1 = from.Y, X2 = to.X, Y2 = to.Y, Stroke = Brushes.Black });
            }

            foreach (var cell in voronoiMesh.Cells)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (cell.Adjacency[i] == null)
                    {
                        var from = cell.Circumcenter;
                        //var t = cell.Vertices.Where((_, j) => j != i).ToArray();
                        //var factor = 100 * IsLeft(t[0].ToPoint(), t[1].ToPoint(), from) * IsLeft(t[0].ToPoint(), t[1].ToPoint(), Center(cell));
                        //var dir = new Point(0.5f * (t[0].Position[0] + t[1].Position[0]), 0.5f * (t[0].Position[1] + t[1].Position[1])) - from;
                        //var to = from + factor * dir;
                        //drawingCanvas.Children.Add(new Line { X1 = from.X, Y1 = from.Y, X2 = to.X, Y2 = to.Y, Stroke = Brushes.Black });
                    }
                }
            }

            //-------------------
            Model myModel = new Model();
            myModel.PointCloud = pointCloudSource;

            //myModel.Triangles = listTriangle;
            //myModel.PointCloud.CalculateCentroidBoundingBox();

            myModel.PointCloud.ToObjFile(pathUnitTests, "1_triangulated.obj");

            
            ShowModel(myModel);

        }



    }
}
