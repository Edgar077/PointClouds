///////////////////////////////////////////////////////////////////////////////
//
// Bowyer–Watson algorithm for Delaunay triangulation
//  https://en.wikipedia.org/wiki/Bowyer–Watson_algorithm
//  By Philip R. Braica (HoshiKata@aol.com, VeryMadSci@gmail.com)
// from https://www.codeproject.com/Articles/492435/Delaunay-Triangulation-For-Fast-Mesh-Generation
//  Distributed under the The Code Project Open License (CPOL)
//  http://www.codeproject.com/info/cpol10.aspx
///////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTKExtension;
using OpenTK;

namespace OpenTKExtension.Triangulation
{
    

    /// <summary>
    /// Mesh generation.
    /// </summary>
    /// 

    public class Mesh
    {
        //private PointCloud pointCloud;

        #region Protected data
        /// <summary>
        /// Recursion limit.
        /// </summary>
        protected int recursion ;

        /// <summary>
        /// The points.
        /// </summary>
        public List<Vertex> Vertices;

        /// <summary>
        /// The facets.
        /// </summary>
        public List<TriangleVectors> Triangles = new List<TriangleVectors>();

        /// <summary>
        /// Bounds
        /// </summary>
        
        #endregion

        #region Properties: Points, Facets, Bounds, Recursion.
      

        ///// <summary>
        ///// Bounds.
        ///// </summary>
        //public System.Drawing.RectangleF Bounds
        //{
        //    get { return m_bounds; }
        //    set { m_bounds = value; }
        //}

        /// <summary>
        /// Recursion level.
        /// </summary>
        public int Recursion 
        { 
            get { return recursion; } 
            set { if (value < 0) value = 0; recursion = value; } 
        }

        #endregion

        public static Mesh Triangulate(PointCloud pointCloud, int recursion)
        {
            Mesh m = new Mesh();
            m.Recursion = recursion;
            System.DateTime start = System.DateTime.Now;

            m.ComputeMesh(pointCloud);

            return m;
        }
      
        /// <summary>
        /// Compute.
        /// </summary>
        /// <param name="set"></param>
        /// <param name="bounds"></param>
        public void ComputeMesh (PointCloud mypointCloud)
        {
            this.Vertices = PointCloud.ToListVertices(mypointCloud);

            
            List<Vertex> cornerVectors = AddCornerTriangles(mypointCloud);

            TriangleVectors t1 = new TriangleVectors(this.Vertices[0], this.Vertices[1], this.Vertices[2]);

            //t1.A = mypointCloud.Vectors[0];
            //t1.B = mypointCloud.Vectors[1];
            //t1.C = mypointCloud.Vectors[2];
            Triangles.Add(t1);

            for (int i = 0; i < Vertices.Count; i++)
            {
                //System.Diagnostics.Debug.WriteLine("In Loop: " + i.ToString() + " : From " + mypointCloud.Vectors.Length.ToString());
                Append(Vertices[i]);
            }

            //for (int i = Triangles.Count - 1; i >= 0; i-- )
            //{
            //    Triangle t = Triangles[i];
            //    if (t.Area > 0.00001)
            //    {
            //        Triangles.RemoveAt(i);
            //    }
            //}

            //remove triangles containing the corners

            RemoveTriangles(cornerVectors);
           
            //AddCornersToPointCloud(cornerVectors);

        }

        /// <summary>
        /// Append point.
        /// </summary>
        /// <param name="v"></param>
        public void Append(Vertex v)
        {
            // Find a triangle containing v.
            for (int i = 0; i < Triangles.Count; i++)
            {
                if (Triangles[i].Contains(v))
                {
                    Insert(v, Triangles[i]);
                }
            }


        }
        /// <summary>
        /// Append point.
        /// </summary>
        /// <param name="v"></param>
        public void RemoveTriangles(List<Vertex> cornerVectors)
        {
            int iRemoved = 0;
            for (int iCorner = 0; iCorner < cornerVectors.Count; iCorner++)
            {
                Vertex v = cornerVectors[iCorner];
                // Find a triangle containing v.
                for (int i = Triangles.Count - 1; i >= 0; i--)
                {
                    if (Triangles[i].A.Equals(v) || Triangles[i].B.Equals(v) || Triangles[i].C.Equals(v))
                    //    if (Triangles[i].Contains(v))
                    {
                        iRemoved++;
                        Triangles.RemoveAt(i);
                        //Insert(v, Triangles[i]);
                    }
                }
            }

            System.Diagnostics.Debug.WriteLine("Removed " + iRemoved.ToString());

        }

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="bounds"></param>
        public List<Vertex> AddCornerTriangles(PointCloud pc)
        {
            TriangleVectors.ResetIndex();
            Triangles = new List<TriangleVectors>();
            //Vertices.Clear();

            Vector3[] corners = pc.BoundingBox.GetCorners();

            Vector3 tl = corners[0];// new Vector3(Bounds.Left, Bounds.Top, 0);
            Vector3 tr = corners[1];// new Vector3(Bounds.Right, Bounds.Top, 0);
            Vector3 br = corners[2];//new Vector3(Bounds.Right, Bounds.Bottom, 0);
            Vector3 bl = corners[3];//new Vector3(Bounds.Left, Bounds.Bottom, 0);

            Vertex vtl = new Vertex(tl, Convert.ToUInt32(this.Vertices.Count));
            this.Vertices.Add(vtl);
            Vertex vtr = new Vertex(tr, Convert.ToUInt32(this.Vertices.Count));
            this.Vertices.Add(vtr);
            Vertex vbr = new Vertex(br, Convert.ToUInt32(this.Vertices.Count));
            this.Vertices.Add(vbr);
            Vertex vbl = new Vertex(bl, Convert.ToUInt32(this.Vertices.Count));
            this.Vertices.Add(vbl);




            TriangleVectors t1 = new TriangleVectors(vbl, vtr, vtl);
            TriangleVectors t2 = new TriangleVectors(vbl, vbr, vtr);

            //TriangleVectors t2 = new TriangleVectors();

            //t1.A = bl; t1.A_Index = Convert.ToUInt32(this.Vertices.Count);

            //t1.B = tr;
            //t1.C = tl;
            //t2.A = bl;
            //t2.B = br;
            //t2.C = tr;
            t1.AB = t2;
            t2.CA = t1;
            Triangles.Add(t1);
            Triangles.Add(t2);


            List<Vertex> listVectors = new List<Vertex>();
            listVectors.Add(vtl);
            listVectors.Add(vtr);
            listVectors.Add(vbr);
            listVectors.Add(vbl);

            return listVectors;

        }
      
   
        /// <summary>
        /// Insert.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="old"></param>
        protected void Insert(Vertex v, TriangleVectors old)
        {
            // Avoid duplicates, if this facet contains v as a Vector3,
            // just return.
            if ((old.A.Vector.X == v.Vector.X) && (old.A.Vector.Y == v.Vector.Y))
                return;
            if ((old.B.Vector.X == v.Vector.X) && (old.B.Vector.Y == v.Vector.Y))
                return;
            if ((old.C.Vector.X == v.Vector.X) && (old.C.Vector.Y == v.Vector.Y))
                return;

            //Vertex.Add(v);

            // Split old into 3 triangles,
            // Because old is counter clockwise, when duplicated,
            // ab, bc, ca is counter clockwise.
            // By changing one point and keeping to the commutation, 
            // they remain counter clockwise.
            TriangleVectors ab = new TriangleVectors(old); // contains old ab, v is new C.
            TriangleVectors bc = new TriangleVectors(old); // contains old bc, v is new A.
            TriangleVectors ca = new TriangleVectors(old); // contains old ca, v is new B.

            ab.C = v;
            bc.A = v; 
            ca.B = v; 

            // This also makes assigning the sides easy.
            ab.BC = bc;
            ab.CA = ca;
            bc.AB = ab;
            bc.CA = ca;
            ca.AB = ab;
            ca.BC = bc;

            // The existing trianges that share an edge with old, 
            // now share an edge with one of the three new triangles.
            // Repair the existing.

            // One way of looking at it:
            // for (int j = 0; j < 3; j++)
            // {
            //    if ((ab.AB != null) && (ab.AB.Edge(j) == old)) ab.AB.SetEdge(j, ab);
            //    if ((bc.BC != null) && (bc.BC.Edge(j) == old)) bc.BC.SetEdge(j, bc);
            //    if ((ca.CA != null) && (ca.CA.Edge(j) == old)) ca.CA.SetEdge(j, ca);
            // } 
            // This is faster, null check is once per edge, and default logic
            // reduces the compares by one. Instead of 3*3*2 comparisons = 18,
            // Worst case is 3*3 = 9, Average is 2+3+3=8.
            TriangleVectors[] ta = { ab.AB, bc.BC, ca.CA };
            TriangleVectors[] tb = { ab, bc, ca };
            for (int j = 0; j < 3; j++)
            {
                if (ta[j] == null) continue;
                if (ta[j].Edge(0) == old)
                {
                    ta[j].SetEdge(0, tb[j]);
                    continue;
                }
                if (ta[j].Edge(1) == old)
                {
                    ta[j].SetEdge(1, tb[j]);
                    continue;
                }
                ta[j].SetEdge(2, tb[j]);
            }

            // Add the new, remove the old.
            Triangles.Add(ab);
            Triangles.Add(bc);
            Triangles.Add(ca);
            Triangles.Remove(old);

            // Check for 1st order flipping.
            // Triangle ab has neighbor ab.AB.
            // Depth of up to recursion deep.
            // Remember that due to commutators, same.same is outward, 
            // same.different is inward.

            flipIfNeeded(ab, ab.AB, recursion);
            flipIfNeeded(bc, bc.BC, recursion);
            flipIfNeeded(ca, ca.CA, recursion);

            return;
        }


        /// <summary>
        /// Flip if needed.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="depth"></param>
        protected void flipIfNeeded(TriangleVectors a, TriangleVectors b, int depth)
        {
            if (depth <= 0) return;
            if (a == null) return;
            if (b == null) return;
            depth--;

            // Triangle a and b share a border, together there are 4 points.
            // As they are counter clockwise, the directions on the boarder are different
            // and the indexing on one border is different than the other.
            // For example if "a" has edge AB in common we know the same direction on that edge
            // is clockwise in triangle "b", but we don't know which Vector3 in b starts that
            // edge. Luckily edges also contain the reference to the next triangle
            // That makes things easy.
            int ai = 0;
            int bi = 0;
            // Rather than roll a loop, since only 3, and default is zero.
            if (a.Edge(1) == b) ai = 1;
            if (a.Edge(2) == b) ai = 2;
            if (b.Edge(1) == a) bi = 1;
            if (b.Edge(2) == a) bi = 2;

            // The Vector3 index of the oposite is:
            //     edge index (ai,bi)    Vector3 index (vai, vbi)
            //       0                      2
            //       1                      0
            //       2                      1
            //       x                      (x+2)%3
            int[] table = { 2, 0, 1 };
            int vai = table[ai];
            int vbi = table[bi];

            // The delaunay condition is that the sum of the interior angles that span
            // the oposite Vector3es must be less than 180 degrees (pi in radians) 
            // if it is, then no need to flip.
            float fa = a.Vector3AngleRadians(vai);
            float fb = b.Vector3AngleRadians(vbi);
            if (fa + fb <= System.Math.PI)
            {
                return;
            }

            // Replace a and b, flipping replacements as needed.
            // opposite and next of opposite remains as an edge in each, and the oposites switch!

            TriangleVectors[] ts = { a.Edge(0), a.Edge(1), a.Edge(2), b.Edge(0), b.Edge(1), b.Edge(2) };

            // The oposite is simple, if the edges are 0=AB, 1=BC, 2=CA, then 
            // the oposites are C, A, B (counter clockwise conjugate). 
            Vertex aOp = a.OppositeOfEdge(ai);
            Vertex bOp = b.OppositeOfEdge(bi);

            a.SetVector(ai + 1, bOp);
            b.SetVector(bi + 1, aOp);

            a.AB = null;
            a.BC = null;
            a.CA = null;
            b.AB = null;
            b.BC = null;
            b.CA = null;

            // Remake edge a.AB.
            for (int i = 0; i < 6; i++)
            {
                if (ts[i] == null) continue;
                ts[i].RepairEdges(a);
                ts[i].RepairEdges(b);
            }

            // Check if -1, 0 need flipping.
            // for (int j = 0; j < 2; j++)
            // {
            //    if (j != ai) flipIfNeeded(a, a.Edge(j), depth);
            //    if (j != bi) flipIfNeeded(b, b.Edge(j), depth);
            // }
            // With a commutator index, to get the other two, add +1, +2.
            flipIfNeeded(a, a.Edge(ai + 1), depth);
            flipIfNeeded(b, b.Edge(bi + 1), depth);
            flipIfNeeded(a, a.Edge(ai + 2), depth);
            flipIfNeeded(b, b.Edge(bi + 2), depth);
        }        
    }
}
