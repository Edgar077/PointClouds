// Pogramming by
//     Douglas Andrade ( http://www.cmsoft.com.br, email: cmsoft@cmsoft.com.br)
//               Implementation of most of the functionality
//     Edgar Maass: (email: maass@logisel.de)
//               Code adaption, changed to user control
//
//Software used: 
//    OpenGL : http://www.opengl.org
//    OpenTK : http://www.opentk.com
//
// DISCLAIMER: Users rely upon this software at their own risk, and assume the responsibility for the results. Should this software or program prove defective, 
// users assume the cost of all losses, including, but not limited to, any necessary servicing, repair or correction. In no event shall the developers or any person 
// be liable for any loss, expense or damage, of any type or nature arising out of the use of, or inability to use this software or program, including, but not
// limited to, claims, suits or causes of action involving alleged infringement of copyrights, patents, trademarks, trade secrets, or unfair competition. 
//


using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Reflection;
using OpenTK;

namespace OpenTKExtension
{
 

    public class Triangle
    {
        public List<uint> IndVertices;
        public List<uint> IndVerticesInCloud;

        public List<int> IndNormals;
        public List<int> IndTextures;
        public Vector3 Normal;
        public int NormalIndex;

        public Triangle()
        {
            IndVertices = new List<uint>();
            IndVerticesInCloud = new List<uint>();
            IndNormals = new List<int>();
            IndTextures = new List<int>();
            //Normal = new Vector3();

        }
        public bool Equals(Triangle obj)
        {
            if (obj.IndVertices[0] == this.IndVertices[0] && obj.IndVertices[1] == this.IndVertices[1] && obj.IndVertices[2] == this.IndVertices[2])
                return true;
            return false;
        }
        public override bool Equals(object obj)
        {
            Triangle t = obj as Triangle;
            return Equals(t);
           
        }
        public Triangle(int i, int j, int k) : this()
        {
            this.IndVertices.Add(Convert.ToUInt32(i));
            this.IndVertices.Add(Convert.ToUInt32(j));
            this.IndVertices.Add(Convert.ToUInt32(k));
            //Normal = new Vector3();
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static Triangle CreateTriangle(int i, int j, int k)
        {
            Triangle a = new Triangle();
           
            a.IndVertices.Add(Convert.ToUInt32(i));
            a.IndVertices.Add(Convert.ToUInt32(j));
            a.IndVertices.Add(Convert.ToUInt32(k));

            return a;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="k"></param>
        /// <param name="l"></param>
        /// <returns></returns>
        public static Triangle CreateTriangle(int i, int j, int k, int l)
        {
            Triangle a = CreateTriangle(i, j, k);
            a.IndVertices.Add(Convert.ToUInt32(l));

            return a;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vi"></param>
        /// <param name="vj"></param>
        /// <param name="vk"></param>
        /// <returns></returns>
        private Triangle CreateTriangle(Vertex vi, Vertex vj, Vertex vk)
        {
            Triangle a = new Triangle();
           
            a.IndVertices.Add(Convert.ToUInt32(vi.Index));
            a.IndVertices.Add(Convert.ToUInt32(vj.Index));
            a.IndVertices.Add(Convert.ToUInt32(vk.Index));

            return a;

        }

        /// <summary>
        /// Helper method
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="k"></param>
        /// <param name="listTriangle"></param>
        public static void AddTriangleToList(int i, int j, int k, List<Triangle> listTriangle, Vertex v)
        {
            Triangle a = Triangle.CreateTriangle(i, j, k);
            a.IndVertices.Sort();

            if(!listTriangle.Contains(a))
            {
                listTriangle.Add(a);
                v.IndexTriangles.Add(listTriangle.Count - 1);
            }
            else
            {

            }
            
            
           
            
        }
   
        /// <summary>
        /// 
        /// </summary>
        /// <param name="areas"></param>
        public static void SortIndexVerticesWithinAllTriangles(List<Triangle> areas)
        {
            for (int i = 0; i < areas.Count; i++)
            {
                Triangle a = areas[i];
                a.IndVertices.Sort();

            }

        }
        ///// <summary>
        ///// Check by IndVertices. The areas are already sorted  (performing sort here would make the performance be bad)
        ///// </summary>
        ///// <param name="b"></param>
        ///// <returns></returns>
        //public bool Equals(Triangle b)
        //{
        //    if (this.IndVertices.Count != b.IndVertices.Count)
        //        return false;
        //    for (int i = 0; i < this.IndVertices.Count; i++ )
        //    {
        //        if (this.IndVertices[i] != b.IndVertices[i])
        //            return false;
        //    }
        //    return true;

        //}
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            for (int i = 0; i < this.IndVertices.Count; i++)
            {
                sb.Append(" : " + this.IndVertices[i].ToString() );

            }

            
            return sb.ToString();
        }
        /// <summary>
        /// is a very time consuming method - use only with list size smaller than 10,000
        /// </summary>
        /// <param name="listTriangle"></param>
        public static void CheckForDuplicates(List<Triangle> listTriangle)
        {

            System.Diagnostics.Debug.WriteLine("Number of areas before check: " + listTriangle.Count.ToString());


            for (int i = listTriangle.Count - 1; i >= 0; i--)
            {
                Triangle ai = listTriangle[i];
                for (int j = 0; j < i; j++)
                {
                    if (ai.Equals(listTriangle[j]))
                    {
                        listTriangle.RemoveAt(i);
                        break;
                    }
                }

            }
            System.Diagnostics.Debug.WriteLine("Number of areas AFTER check: " + listTriangle.Count.ToString());
        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="myModel"></param>
        ///// <param name="t"></param>
        //public static void CalculateNormal_UpdateNormalList(Model3D myModel, Triangle t)
        //{

        //    Vector3 normal = CalculateNormalForTriangle(myModel.VertexList, t);
            
        //    if (normal != null)
        //    {
        //        myModel.Normals.Add(normal);
        //        int indNewNormal = myModel.Normals.Count - 1;
                
                
        //        t.IndNormals.Add(indNewNormal);
        //        //adds the normal to each of the pointCloud in the triangle
        //        for (int i = 0; i < t.IndVertices.Count; i++ )
        //        {
        //            int indVertex = t.IndVertices[i];
        //            myModel.VertexList[indVertex].IndexNormals.Add(indNewNormal);
        //        }
                   
        //    }


        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pointCloud"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Vector3 CalculateNormalForTriangle(PointCloud pointCloud, Triangle t, Vector3 v)
        {

            Vector3 a = pointCloud.Vectors[Convert.ToInt32(t.IndVertices[0])];
            Vector3 b = pointCloud.Vectors[Convert.ToInt32(t.IndVertices[1])];
            Vector3 c = pointCloud.Vectors[Convert.ToInt32(t.IndVertices[2])];

            Vector3 normal = Vector3.Cross(b - a, c - a);
            //important: The direction of the normal
            //I want the normal to be in the direction of the Vertex v
            //compare if v and the normal are in the same direction
            if(Vector3.Dot(normal, v) < 0)
            {
                //they are not in the same direction => Flip normal:
                normal = Vector3.Cross(b - c, a - c);
            }


            //alternative:
            //Vector3 normal = Vector3.Cross(b - a, c - b);

            Vector3 temp = normal;
            normal = normal.NormalizeV();
            //if (!CheckVector(normal))
            //{
            //    System.Windows.Forms.MessageBox.Show("SW Error calculating normal");
            //    return new Vector3(0, 0, 0);

            //}
            
            return normal;
        }
       
        //public static Vector3 CalculateNormal(Model myModel, Triangle t)
        //{
        //    if (t.IndNormals == null || t.IndNormals.Count == 0)
        //    {
        //        Vector3 a = myModel.PointCloud[t.IndVertices[0]].Vector;
        //        Vector3 b = myModel.PointCloud[t.IndVertices[1]].Vector;
        //        Vector3 c = myModel.PointCloud[t.IndVertices[2]].Vector;

        //        Vector3 normal = Vector3.Cross(b - a, c - a);
        //        //alternative:
        //        //Vector3 normal = Vector3.Cross(b - a, c - b);

        //        Vector3 temp = normal;
        //        normal = normal.NormalizeV();
        //        //if (!CheckVector(normal))
        //        //{
        //        //    System.Windows.Forms.MessageBox.Show("SW Error calculating normal");
        //        //    return new Vector3(0, 0, 0);

        //        //}
        //        return normal;

        //    }
        //    return myModel.Normals[t.IndNormals[0]];
        //}
        private static bool CheckVector(Vector3 v)
        {
            if (float.IsInfinity(v.X) || float.IsNaN(v.X) || float.IsInfinity(v.Y) || float.IsNaN(v.Y) || float.IsInfinity(v.Z) || float.IsNaN(v.Z))
                return false;

       

            return true;


        }
        /// <summary>
        /// Calculate the Tangent array based on the Vertex, Face, Normal and UV data.
        /// </summary>
        public static Vector3[] CalculateTangents(Vector3[] vertices, Vector3[] normals, int[] triangles, Vector2[] uvs)
        {
            Vector3[] tangents = new Vector3[vertices.Length];
            Vector3[] tangentData = new Vector3[vertices.Length];

            for (int i = 0; i < triangles.Length / 3; i++)
            {
                Vector3 v1 = vertices[triangles[i * 3]];
                Vector3 v2 = vertices[triangles[i * 3 + 1]];
                Vector3 v3 = vertices[triangles[i * 3 + 2]];

                Vector2 w1 = uvs[triangles[i * 3]];
                Vector2 w2 = uvs[triangles[i * 3] + 1];
                Vector2 w3 = uvs[triangles[i * 3] + 2];

                float x1 = v2.X - v1.X;
                float x2 = v3.X - v1.X;
                float y1 = v2.Y - v1.Y;
                float y2 = v3.Y - v1.Y;
                float z1 = v2.Z - v1.Z;
                float z2 = v3.Z - v1.Z;

                float s1 = w2.X - w1.X;
                float s2 = w3.X - w1.X;
                float t1 = w2.Y - w1.Y;
                float t2 = w3.Y - w1.Y;
                float r = 1.0f / (s1 * t2 - s2 * t1);
                Vector3 sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);

                tangents[triangles[i * 3]] += sdir;
                tangents[triangles[i * 3 + 1]] += sdir;
                tangents[triangles[i * 3 + 2]] += sdir;
            }

            for (int i = 0; i < vertices.Length; i++)
            {
                float d = Vector3.Dot(normals[i], tangents[i]);//.Normalize();
                Vector3 tempV = normals[i] * d;
                tempV = tangents[i] - tempV;
                tempV.Normalize();
                tangentData[i] = tempV;
                //tangentData[i] = (tangents[i] - normals[i] * Vector3.Dot(normals[i], tangents[i])).Normalize();

            }
            return tangentData;
        }
    }

    /// <summary>
    /// compares according to INDEX of first, second, third vertex
    /// </summary>
    public class TriangleComparerVertices : IComparer<Triangle>
    {

        public int Compare(Triangle a, Triangle b)
        {
            if (a.IndVertices.Count != b.IndVertices.Count)
                return 0;

            for (int i = 0; i < a.IndVertices.Count; i++)
            {
                uint ai = a.IndVertices[i];
                uint bi = b.IndVertices[i];
                if (ai < bi)
                    return -1;
                else if (ai > bi)
                    return 1;

            }
            return 0;

        }
    }
    //}

 
}
