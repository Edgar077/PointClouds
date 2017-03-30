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
        public void Triangulate25D(float distMin)
        {
            this.Triangles = new List<Triangle>();
           
            List<List<VertexKDTree>> listNew = SortVectorsWithIndex();
            for (int i = listNew.Count - 1; i > 0; i--)
            {
                List<VertexKDTree> columnx = listNew[i];
                List<VertexKDTree> columny = listNew[i - 1];
                for (int j = 1; j < columnx.Count; j++)
                {
                    VertexKDTree vx = columnx[j];
                    float dist_x = columnx[j - 1].Vector.Distance(vx.Vector);
                    if (dist_x < distMin)
                    {
                        foreach (VertexKDTree vy in columny)
                        {
                            float dist_xy = vx.Vector.Distance(vy.Vector);
                            if (dist_xy < distMin)
                            {
                                Triangles.Add(new Triangle(columnx[j - 1].Index, vx.Index, vy.Index));
                            }
                        }
                    }
                }
            }
            CreateIndicesFromTriangles();
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

        public void Triangulate_KDTree(int numberNeighbours)
        {

            KDTreeKennell kdTree = new KDTreeKennell();
            kdTree.Build(this);

            List<Triangle> listTriangles = new List<Triangle>();

            for (int i = 0; i < this.Vectors.Length; i++)
            {
                VertexKDTree vSource = new VertexKDTree(this.Vectors[i], this.Colors[i], i);
                uint indexI = Convert.ToUInt32(i);

                ListKDTreeResultVectors listResult = kdTree.Find_N_Nearest(vSource.Vector, numberNeighbours);
                for(int j = 1; j < listResult.Count; j++ )
                {
                    for (int k = j + 1; k < listResult.Count; k++)
                    {
                        Triangle t = new Triangle(indexI, listResult[j].IndexNeighbour, listResult[k].IndexNeighbour);
                        listTriangles.Add(t);
                    }

                }
               

            }
            this.Triangles = listTriangles;


            CreateIndicesFromTriangles();

        }
    }
}
