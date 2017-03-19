///////////////////////////////////////////////////////////////////////////////
//
//  Mesh.cs
//
//  By Philip R. Braica (HoshiKata@aol.com, VeryMadSci@gmail.com)
//
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
    public class Mesh
    {
        private PointCloud pointCloud;
        #region Protected data
        /// <summary>
        /// Recursion limit.
        /// </summary>
        protected int recursion ;

        /// <summary>
        /// The points.
        /// </summary>
        protected List<Vector3> vertices = new List<Vector3>();

        /// <summary>
        /// The facets.
        /// </summary>
        protected List<Triangle> triangles = new List<Triangle>();

        /// <summary>
        /// Bounds
        /// </summary>
        
        #endregion

        #region Properties: Points, Facets, Bounds, Recursion.
        /// <summary>
        /// The points.
        /// </summary>
        public List<Vector3> Vertices
        {
            get { return vertices; }
            set { vertices = value; }
        }

        /// <summary>
        /// The facets.
        /// </summary>
        public List<Triangle> Triangles
        {
            get { return triangles; }
            set { triangles = value; }
        }

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

   

        /// <summary>
        /// Compute.
        /// </summary>
        /// <param name="set"></param>
        /// <param name="bounds"></param>
        public void Compute(PointCloud mypointCloud)
        {
            pointCloud = mypointCloud;
            List<Vector3> cornerVectors = AddCornerTriangles();
            Triangle t1 = new Triangle();
            t1.A = mypointCloud.Vectors[0];
            t1.B = mypointCloud.Vectors[1];
            t1.C = mypointCloud.Vectors[2];
            Triangles.Add(t1);
            
            for (int i = 0; i < mypointCloud.Vectors.Length; i++)
            {
                //System.Diagnostics.Debug.WriteLine("In Loop: " + i.ToString() + " : From " + mypointCloud.Vectors.Length.ToString());
                Append(mypointCloud.Vectors[i]);
            }

            //for (int i = Triangles.Count - 1; i >= 0; i-- )
            //{
            //    Triangle t = Triangles[i];
            //    if (t.Area > 0.00001)
            //    {
            //        Triangles.RemoveAt(i);
            //    }
            //}
                AdjustPointCloud(cornerVectors);

        }

        /// <summary>
        /// Append point.
        /// </summary>
        /// <param name="v"></param>
        public void Append(Vector3 v)
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
        /// Setup.
        /// </summary>
        /// <param name="bounds"></param>
        public List<Vector3> AddCornerTriangles()
        {
            Triangle.ResetIndex();
            Triangles.Clear();
            Vertices.Clear();

            Vector3[] corners = pointCloud.BoundingBox.GetCorners();

            Vector3 tl = corners[0];// new Vector3(Bounds.Left, Bounds.Top, 0);
            
            Vector3 tr = corners[1];// new Vector3(Bounds.Right, Bounds.Top, 0);
            
            Vector3 br = corners[2];//new Vector3(Bounds.Right, Bounds.Bottom, 0);
            
            Vector3 bl = corners[3];//new Vector3(Bounds.Left, Bounds.Bottom, 0);

           
            Triangle t1 = new Triangle();
            Triangle t2 = new Triangle();
            t1.A = bl;
            t1.B = tr;
            t1.C = tl;
            t2.A = bl;
            t2.B = br;
            t2.C = tr;
            t1.AB = t2;
            t2.CA = t1;
            Triangles.Add(t1);
            Triangles.Add(t2);


            List<Vector3> listVectors = new List<Vector3>();
            listVectors.Add(tl);
            listVectors.Add(tr);
            listVectors.Add(br);
            listVectors.Add(bl);

            return listVectors;

        }
        private void AdjustPointCloud(List<Vector3> vToAdd)
        {
            //EDGAR INDEX
            if (vToAdd.Count > 0)
            {
                //extend point cloud
                List<Vector3> listVectors = new List<Vector3>(pointCloud.Vectors);
                List<Vector3> listColors = new List<Vector3>(pointCloud.Colors);

                for (int i = 0; i < vToAdd.Count; i++)
                {
                    listVectors.Add(vToAdd[i]);
                    listColors.Add(Vector3.Zero);
                }
                
              

                this.pointCloud.Vectors = listVectors.ToArray();
                this.pointCloud.Colors = listColors.ToArray();
            }
        }

        /// <summary>
        /// Draw the mesh.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="minx"></param>
        /// <param name="miny"></param>
        /// <param name="maxx"></param>
        /// <param name="maxy"></param>
        public void Draw(System.Drawing.Graphics g, int minx, int miny, int maxx, int maxy)
        {
            System.Drawing.Pen[] pens = { 
                System.Drawing.Pens.Red, 
                System.Drawing.Pens.Green,
                System.Drawing.Pens.Blue, 
                System.Drawing.Pens.Orange,
                System.Drawing.Pens.Purple, 
                System.Drawing.Pens.Brown,
                System.Drawing.Pens.Violet, 
                System.Drawing.Pens.Lime,
                System.Drawing.Pens.DarkBlue, 
                System.Drawing.Pens.Magenta,
                System.Drawing.Pens.Cyan, 
                System.Drawing.Pens.DarkRed};

            maxx -= 2;
            maxy -= 2;
            for (int i = 0; i < Triangles.Count; i++)
            {
                float x = Triangles[i].OpositeOfEdge(0).X;
                float y = Triangles[i].OpositeOfEdge(0).Y;
                int k = i % pens.Length;
                for (int j = 1; j < 4; j++)
                {
                    x = x < minx ? minx : x;
                    y = y < miny ? miny : y;
                    x = x > maxx ? maxx : x;
                    y = y > maxy ? maxy : y;

                    float nx = Triangles[i].OpositeOfEdge(j).X;
                    float ny = Triangles[i].OpositeOfEdge(j).Y;
                    nx = nx < minx ? minx : nx;
                    ny = ny < miny ? miny : ny;
                    nx = nx > maxx ? maxx : nx;
                    ny = ny > maxy ? maxy : ny;
                    g.DrawLine(pens[k], x, y, nx, ny);
                    x = nx;
                    y = ny;
                }
            }

        }
   
        /// <summary>
        /// Insert.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="old"></param>
        protected void Insert(Vector3 v, Triangle old)
        {
            // Avoid duplicates, if this facet contains v as a Vector3,
            // just return.
            if ((old.A.X == v.X) && (old.A.Y == v.Y)) return;
            if ((old.B.X == v.X) && (old.B.Y == v.Y)) return;
            if ((old.C.X == v.X) && (old.C.Y == v.Y)) return;

            vertices.Add(v);

            // Split old into 3 triangles,
            // Because old is counter clockwise, when duplicated,
            // ab, bc, ca is counter clockwise.
            // By changing one point and keeping to the commutation, 
            // they remain counter clockwise.
            Triangle ab = new Triangle(old); // contains old ab, v is new C.
            Triangle bc = new Triangle(old); // contains old bc, v is new A.
            Triangle ca = new Triangle(old); // contains old ca, v is new B.
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
            Triangle[] ta = { ab.AB, bc.BC, ca.CA };
            Triangle[] tb = { ab, bc, ca };
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
        protected void flipIfNeeded(Triangle a, Triangle b, int depth)
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

            Triangle[] ts = { a.Edge(0), a.Edge(1), a.Edge(2), b.Edge(0), b.Edge(1), b.Edge(2) };

            // The oposite is simple, if the edges are 0=AB, 1=BC, 2=CA, then 
            // the oposites are C, A, B (counter clockwise conjugate). 
            Vector3 aOp = a.OpositeOfEdge(ai);
            Vector3 bOp = b.OpositeOfEdge(bi);

            a.SetVector3(ai + 1, bOp);
            b.SetVector3(bi + 1, aOp);

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
