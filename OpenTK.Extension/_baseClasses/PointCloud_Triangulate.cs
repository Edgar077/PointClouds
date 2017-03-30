using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.IO;
using System.Globalization;
using System.Drawing;

namespace OpenTKExtension
{

    public partial class PointCloud 
    {
        //assumes a 2.5 D point cloud (i.e. for a given x,y there is only ONE z value
        public void Triangulate25D()
        {
            this.Triangles = new List<Triangle>();
            float distMin = 0.01f;
            List<List<VertexKDTree>> listNew = SortVectorsWithIndex();
            for (int i = listNew.Count - 1; i > 0; i--)
            {
                List<VertexKDTree> columnx = listNew[i];
                List<VertexKDTree> columny = listNew[i - 1];
                for (int j = 1; j < columnx.Count; j++)
                {
                    VertexKDTree vx = columnx[j];
                    foreach (VertexKDTree vy in columny)
                    {
                        if (vx.Vector.Distance(vy.Vector) < distMin)
                        {

                            Triangles.Add(new Triangle(columnx[j - 1].Index, vx.Index, vy.Index));
                        }
                    }

                }
            }

        }

        /// <summary>
        /// Based upon the info of the nearest vertex (of each vertex), the triangles are created
        /// </summary>
        /// <param name="myModel"></param>
        //private static List<Triangle> CreateTrianglesByNearestVertices(PointCloud pointCloud)
        //{

        //    List<Triangle> listTriangles = new List<Triangle>();

        //    //create triangles
        //    //for (int i = pointCloud.Count - 1; i >= 0; i--)
        //    for (int i = 0; i < pointCloud.Count; i++)
        //    {
        //        Vertex v = pointCloud[i];

        //        if (v.KDTreeSearch.Count >= 2)
        //        {
        //            Triangle.AddTriangleToList(v.Index, v.KDTreeSearch[0].Key, v.KDTreeSearch[1].Key, listTriangles, v);


        //        }
        //        if (v.KDTreeSearch.Count >= 3)
        //        {
        //            Triangle.AddTriangleToList(v.Index, v.KDTreeSearch[0].Key, v.KDTreeSearch[2].Key, listTriangles, v);
        //            Triangle.AddTriangleToList(v.Index, v.KDTreeSearch[1].Key, v.KDTreeSearch[2].Key, listTriangles, v);


        //        }
        //        if (v.KDTreeSearch.Count >= 4)
        //        {
        //            Triangle.AddTriangleToList(v.Index, v.KDTreeSearch[0].Key, v.KDTreeSearch[3].Key, listTriangles, v);
        //            Triangle.AddTriangleToList(v.Index, v.KDTreeSearch[1].Key, v.KDTreeSearch[3].Key, listTriangles, v);
        //            Triangle.AddTriangleToList(v.Index, v.KDTreeSearch[2].Key, v.KDTreeSearch[3].Key, listTriangles, v);

        //        }
        //        if (v.KDTreeSearch.Count >= 5)
        //        {
        //            Triangle.AddTriangleToList(v.Index, v.KDTreeSearch[0].Key, v.KDTreeSearch[4].Key, listTriangles, v);
        //            Triangle.AddTriangleToList(v.Index, v.KDTreeSearch[1].Key, v.KDTreeSearch[4].Key, listTriangles, v);
        //            Triangle.AddTriangleToList(v.Index, v.KDTreeSearch[2].Key, v.KDTreeSearch[4].Key, listTriangles, v);
        //            Triangle.AddTriangleToList(v.Index, v.KDTreeSearch[3].Key, v.KDTreeSearch[4].Key, listTriangles, v);

        //        }



        //    }
        //    //RemoveDuplicateTriangles(listTriangles);
        //    listTriangles.Sort(new TriangleComparer());

        //    return listTriangles;
        //}

        //public void TriangulateVertices_Rednaxela(float maxDistance)
        //{

        //    //PointCloud pointCloud = myModel.PointCloud;
        //    KDTreeVertex kv = new KDTreeVertex();
        //    PointCloud vertices = this;

        //    kv.NumberOfNeighboursToSearch = 6;
        //    kv.BuildKDTree_Rednaxela(vertices);
        //    kv.ResetVerticesSearchResult(vertices);


        //    kv.FindNearest_NormalsCheck_Rednaxela(vertices, false, true, maxDistance);
        //  // kv.FindNearest_Rednaxela(vertices, vertices, true);

        //  //  kv.FindNearest_Rednaxela_Parallel(ref pointsSource, pointsTarget, angleThreshold);


        //    Triangles = CreateTrianglesByNearestVertices(vertices);
        //    CreateIndicesFromTriangles();

        //}
    }
}
