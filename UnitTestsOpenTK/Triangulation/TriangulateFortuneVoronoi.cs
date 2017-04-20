using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using OpenTKExtension;
using OpenTK;
using KDTreeRednaxela;

using MIConvexHull;
using VoronoiFortune;


namespace UnitTestsOpenTK.Triangulation
{
    [TestFixture]
    [Category("UnitTest")]
    public class TriangulateFortuneVoronoi : TestBase
    {
        [Test]
        public void Face_VoronoiFortune_AsTriangles()
        {
            List<VertexKDTree> listPointsFortune = new List<VertexKDTree>();
            string fileNameLong = pathUnitTests + "\\KinectFace_1_15000.obj";

            pointCloudSource = IOUtils.ReadObjFile_ToPointCloud(fileNameLong);

            for (int i = 0; i < pointCloudSource.Count; i++)
            {
                VertexKDTree v = new VertexKDTree(new Vector3(pointCloudSource[i].Vector.X, pointCloudSource[i].Vector.Y, pointCloudSource[i].Vector.Z), i);
                listPointsFortune.Add(v);

            }

            List<EdgeFortune> listEdges;

            Voronoi voronoi = new Voronoi(0.1f);

            listEdges = voronoi.GenerateVoronoi(listPointsFortune);
            
            List<Triangle> listTriangle = new List<Triangle>();
            for (int i = 0; i < listEdges.Count; i+=3)
            {
                EdgeFortune edge = listEdges[i];

                Triangle t = new Triangle();
               
                //t.IndVertices.Add(cell.Vertices[0].IndexInPointCloud);
                //t.IndVertices.Add(cell.Vertices[1].IndexInPointCloud);
                //t.IndVertices.Add(cell.Vertices[2].IndexInPointCloud);
                listTriangle.Add(t);

                //myLines.Add(pointCloud[edge.PointIndex1]);
                //myLinesTo.Add(pointCloud[edge.PointIndex2]);

            }

            //-------------------
           
            ShowPointCloud(pointCloudSource);

        }
         [Test]
        public void Face_VoronoiFortune()
        {
            List<VertexKDTree> listPointsFortune = new List<VertexKDTree>();
            string fileNameLong = pathUnitTests + "\\KinectFace_1_15000.obj";
            
            pointCloudSource = IOUtils.ReadObjFile_ToPointCloud(fileNameLong);

            
            for (int i = 0; i < pointCloudSource.Count; i++)
            {
                VertexKDTree v = new VertexKDTree(new Vector3(pointCloudSource[i].Vector.X, pointCloudSource[i].Vector.Y, pointCloudSource[i].Vector.Z), i);
                listPointsFortune.Add(v);

            }

            List<EdgeFortune> listEdges;
            
          
            Voronoi voronoi = new Voronoi(0.1f);

            listEdges = voronoi.GenerateVoronoi(listPointsFortune);
            List<LineD> myLines = new List<LineD>(); 
            
             
            for (int i = 0; i < listEdges.Count; i++)
            {
                EdgeFortune edge = listEdges[i];

                myLines.Add(new LineD( pointCloudSource[edge.PointIndex1].Vector, pointCloudSource[edge.PointIndex2].Vector));

            }

            //-------------------
            ShowPointCloud(pointCloudSource);

        }
       
      
     
    }
}
