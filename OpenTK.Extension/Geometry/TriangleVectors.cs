///////////////////////////////////////////////////////////////////////////////
//
//  Triangle.cs
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

namespace OpenTKExtension
{

    /// <summary>
    /// Triangle class.
    /// </summary>
    public class TriangleVectors
    {
        // Vector3es.
        protected Vertex a;//= Vector3.Zero;
        protected Vertex b;// = Vector3.Zero;
        protected Vertex c;// = Vector3.Zero;

      



        // Lengths.
        protected float abLen = 0;
        protected float bcLen = 0;
        protected float caLen = 0;
        protected bool abLenCalcd = false;
        protected bool bcLenCalcd = false;
        protected bool caLenCalcd = false;

        // Side determinations.
        protected bool m_abDet = false;
        protected bool m_bcDet = false;
        protected bool m_caDet = false;
        protected bool m_abDetCalcd = false;
        protected bool m_bcDetCalcd = false;
        protected bool m_caDetCalcd = false;

        /// <summary>
        /// Index of this triangle for debug.
        /// </summary>
        protected static int m_index = 0;

        // Sides
        //protected TriangleVectors m_ab = null;
        //protected TriangleVectors m_bc = null;
        //protected TriangleVectors m_ca = null;

        // Center
        protected bool m_centerComputed = false;
        protected Vector3 m_center = Vector3.Zero;
        /// <summary>
        /// Reset the index, indexing gives each triangle an ID
        /// unique since construction of the mesh.
        /// </summary>
        public static void ResetIndex() {m_index = 0;}

        /// <summary>
        /// Index.
        /// </summary>
        public int Index { get; protected set; }

        /// <summary>
        /// Which search region it is in.
        /// </summary>
        public int RegionCode { get; set; }


        #region Construcors
        /// <summary>
        /// Constructor.
        /// </summary>
        public TriangleVectors() 
        {
            Index = m_index;
            m_index++;
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="src"></param>
        public TriangleVectors(TriangleVectors src)
        {
            A = src.A;
            B = src.B;
            C = src.C;

          

            AB = src.AB;
            BC = src.BC;
            CA = src.CA;
            Index = m_index;
            m_index++;
        }

        ///// <summary>
        ///// Constructor by Vector3.
        ///// </summary>
        ///// <param name="a"></param>
        ///// <param name="b"></param>
        ///// <param name="c"></param>
        //public TriangleVectors(Vector3 a, Vector3 b, Vector3 c)
        //{
        //    A = a;
        //    B = b;
        //    C = c;
        //    Index = m_index;
        //    m_index++;
        //}
        /// <summary>
        /// Constructor by Vector3.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        public TriangleVectors(Vertex a, Vertex b, Vertex c)
        {
            A = a;
            B = b;
            C = c;

            

            Index = m_index;
            m_index++;
        }
        /// <summary>
        /// Constructor by Vector3.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        public TriangleVectors(Vertex a, Vertex b, Vertex c, uint aIndex, uint bIndex, uint cIndex )
        {
            A = a;
            B = b;
            C = c;
            Index = m_index;
            m_index++;
        }
        #endregion



        /// <summary>
        /// To string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Index + ": " + A.ToString() + " => " + B.ToString() + " => " + C.ToString();
        }

        /// <summary>
        /// Compute the center
        /// </summary>
        public Vector3 Center
        {
            get
            {
                if (m_centerComputed) return m_center;
                m_center = new Vector3(
                    (A.Vector.X + B.Vector.X + C.Vector.X) / 3f,
                    (A.Vector.Y + B.Vector.Y + C.Vector.Y) / 3f,
                    (A.Vector.Z + B.Vector.Z + C.Vector.Z) / 3f);

                float delta = m_center.DeltaSquaredXY(A.Vector);
                float tmp = m_center.DeltaSquaredXY(B.Vector);
                delta = delta > tmp ? delta : tmp;
                tmp = m_center.DeltaSquaredXY(C.Vector);
                delta = delta > tmp ? delta : tmp;
                FarthestFromCenter = delta;
                m_centerComputed = true;

                return m_center;
            }
        }

        /// <summary>
        /// Farthest distance a point is from center is distance squared.
        /// </summary>
        public float FarthestFromCenter { get; protected set; }

        /// <summary>
        /// Vector3 A
        /// </summary>
        public Vertex A
        {
            get { return a; }
            set 
            {
                if (a == value)
                    return;
                m_abDetCalcd = false;
                m_caDetCalcd = false;
                abLenCalcd = false;
                caLenCalcd = false;
                m_centerComputed = false;
                a = value;
            }
        }

        /// <summary>
        /// Vector3 B
        /// </summary>
        public Vertex B
        {
            get { return b; }
            set
            {
                if (b == value) return;
                m_abDetCalcd = false;
                m_bcDetCalcd = false;
                abLenCalcd = false;
                bcLenCalcd = false;
                m_centerComputed = false;
                b = value;
            }
        }

        /// <summary>
        /// Vector3 C
        /// </summary>
        public Vertex C
        {
            get { return c; }
            set
            {
                if (c == value) return;
                m_caDetCalcd = false;
                m_bcDetCalcd = false;
                caLenCalcd = false;
                bcLenCalcd = false;
                m_centerComputed = false;
                c = value;
            }
        }

        /// <summary>
        /// Triangle AB shares side AB.
        /// </summary>
        public TriangleVectors AB;

        /// <summary>
        /// Triangle BC shares side BC.
        /// </summary>
        public TriangleVectors BC;

        /// <summary>
        /// Triangle CA shares side CA.
        /// </summary>
        public TriangleVectors CA;


        /// <summary>
        /// AB det.
        /// </summary>
        protected bool abDet
        {
            get
            {
                if (!m_abDetCalcd)
                {
                    m_abDet = sidednessTest(A, B, C);
                }
                return m_abDet;
            }
        }

        /// <summary>
        /// BC det.
        /// </summary>
        protected bool bcDet
        {
            get
            {
                if (!m_bcDetCalcd)
                {
                    m_bcDet = sidednessTest(B, C, A);
                }
                return m_bcDet;
            }
        }

        /// <summary>
        /// CA det.
        /// </summary>
        protected bool caDet
        {
            get
            {
                if (!m_caDetCalcd)
                {
                    m_caDet = sidednessTest(C, A, B);
                }
                return m_caDet;
            }
        }

        /// <summary>
        /// Vector3 sidedness test.
        /// </summary>
        /// <param name="la"></param>
        /// <param name="lb"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        protected bool sidednessTest(Vertex la, Vertex lb, Vertex t)
        {
            // y = mx + b
            if (la.Vector.X == lb.Vector.X)
            {
                // Vertical at X.
                return t.Vector.X > la.Vector.X;
            }
            if (la.Vector.Y == lb.Vector.Y)
            {
                return t.Vector.Y > la.Vector.Y;
            }
            float m = (la.Vector.Y - lb.Vector.Y)/(la.Vector.X - lb.Vector.X);
            float b = la.Vector.Y - (m * la.Vector.X);
            return (m * t.Vector.X + b - t.Vector.Y) > 0;
        }

        ///// <summary>
        ///// Does this contain t.
        ///// </summary>
        ///// <param name="v"></param>
        ///// <returns></returns>
        //public bool Contains(Vector3 v)
        //{
        //    float delta = v.DeltaSquaredXY(Center);
        //    if (delta > FarthestFromCenter) return false;
        //    if (abDet != sidednessTest(A, B, v)) return false;
        //    if (bcDet != sidednessTest(B, C, v)) return false;
        //    if (caDet != sidednessTest(C, A, v)) return false;
        //    return true;
        //}
        /// <summary>
        /// Does this contain t.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public bool Contains(Vertex v)
        {
            float delta = v.Vector.DeltaSquaredXY(Center);
            if (delta > FarthestFromCenter) return false;
            if (abDet != sidednessTest(A, B, v))
                return false;
            if (bcDet != sidednessTest(B, C, v))
                return false;
            if (caDet != sidednessTest(C, A, v))
                return false;
            return true;
        }


        /// <summary>
        /// Length of AB, cached and lazy calculated.
        /// </summary>
        public float AB_Length
        {
            get
            {
                if (abLenCalcd == true)
                {
                    return abLen;
                }
                if ((A == null) || (B == null)) return -1;
                abLen = A.Vector.DeltaSquaredXY(B.Vector);
                abLenCalcd = true;
                return abLen;
            }
        }

        /// <summary>
        /// Length of BC, cached and lazy calculated.
        /// </summary>
        public float BC_Length
        {
            get
            {
                if (bcLenCalcd == true)
                {
                    return bcLen;
                }
                if ((B == null) || (C == null)) return -1;
                bcLen = B.Vector.DeltaSquaredXY(C.Vector);
                bcLenCalcd = true;
                return bcLen;
            }
        }

        /// <summary>
        /// Length of CA, cached and lazy calculated.
        /// </summary>
        public float CA_Length
        {
            get
            {
                if (caLenCalcd == true)
                {
                    return caLen;
                }
                if ((C == null) || (A == null)) return -1;
                caLen = C.Vector.DeltaSquaredXY(A.Vector);
                caLenCalcd = true;
                return caLen;
            }
        }

        /// <summary>
        /// Area of the triangle.
        /// </summary>
        public float Area
        {
            get
            {
                float a = AB_Length;
                float b = BC_Length;
                float c = CA_Length;
                a = (float)System.Math.Sqrt(a);
                b = (float)System.Math.Sqrt(b);
                c = (float)System.Math.Sqrt(c);

                // Herons formula.
                float s = 0.5f * (a + b + c);
                return (float)System.Math.Sqrt(s * (s - a) * (s - b) * (s - c));
            }
        }

        /// <summary>
        /// Return the indexed edge length;
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public float Edge_Length(int i)
        {
            i = i < 0 ? i + 3 : i > 2 ? i - 3 : i;
            return i == 0 ? AB_Length : i == 1 ? BC_Length : CA_Length;
        }

        /// <summary>
        /// Return the oposite of the edge.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Vertex OppositeOfEdge(int i)
        {
            i = i < 0 ? i + 3 : i > 2 ? i - 3 : i;
            return i == 0 ? C : i == 1 ? A : B;
        }
        
        /// <summary>
        /// Set the Vector3 by index.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="v"></param>
        public void SetVector(int i, Vertex v)
        {
            i = i < 0 ? i + 3 : i > 2 ? i - 3 : i;
            if (i == 0) A = v;
            if (i == 1) B = v;
            if (i == 2) C = v;
        }

        /// <summary>
        /// Get the cosine angle associated with a Vector3.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public float Vector3CosineAngle(int i)
        {
            i = i < 0 ? i + 3 : i > 2 ? i - 3 : i;
            float dx1 = 0;
            float dx2 = 0;
            float dy1 = 0; 
            float dy2 = 0;
            if (i == 0)
            {
                dx1 = B.Vector.X - A.Vector.X;
                dy1 = B.Vector.Y - A.Vector.Y;
                dx2 = C.Vector.X - A.Vector.X;
                dy2 = C.Vector.Y - A.Vector.Y;
            }
            else
            {
                if (i == 1)
                {
                    dx1 = C.Vector.X - B.Vector.X;
                    dy1 = C.Vector.Y - B.Vector.Y;
                    dx2 = A.Vector.X - B.Vector.X;
                    dy2 = A.Vector.Y - B.Vector.Y;
                }
                else
                {
                    dx1 = A.Vector.X - C.Vector.X;
                    dy1 = A.Vector.Y - C.Vector.Y;
                    dx2 = B.Vector.X - C.Vector.X;
                    dy2 = B.Vector.Y - C.Vector.Y;
                }
            }
            float mag1 = (dx1 * dx1) + (dy1 * dy1);
            float mag2 = (dx2 * dx2) + (dy2 * dy2);
            float mag = (float)System.Math.Sqrt(mag1 * mag2);
            float dot = (float)((dx1 * dx2) + (dy1 * dy2)) / mag;

            // dot is 0 to 1 result of the cosine.
            return dot;
        }

        
        /// <summary>
        /// Get the angle of a Vector3 in radians.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public float Vector3AngleRadians(int i)
        {
            return (float)System.Math.Acos(Vector3CosineAngle(i));
        }

        /// <summary>
        /// Is this rectangle within the region.
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        public bool Inside(System.Drawing.RectangleF region)
        {
            if (!A.Vector.InsideXY(region)) return false;
            if (!B.Vector.InsideXY(region)) return false;
            if (!C.Vector.InsideXY(region)) return false;
            return true;
        }

        /// <summary>
        /// Repair any Edge links, both ways.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public void RepairEdges(TriangleVectors a)
        {
            // Check if a.AB is in this.
            if (this.Index == a.Index) return;
            if (bothIn(a, a.A, a.B)) { a.AB = this; return; }
            if (bothIn(a, a.B, a.C)) { a.BC = this; return; }
            if (bothIn(a, a.C, a.A)) { a.CA = this; return; }    
        }

        /// <summary>
        /// Are both Vector3es in?
        /// </summary>
        /// <param name="t"></param>
        /// <param name="vt"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        protected bool bothIn(TriangleVectors t, Vertex a, Vertex b)
        {
            if (a == A) 
            {
                if (b == B) { AB = t; return true; }
                if (b == C) { CA = t; return true; }
            }
            if (a == B)
            {
                if (b == A) { AB = t; return true; }
                if (b == C) { BC = t; return true; }
            }
            if (a == C)
            {
                if (b == A) { CA = t; return true; }
                if (b == B) { BC = t; return true; }
            }
            return false;
        }

        /// <summary>
        /// Vector3.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Vertex GetVertex(int i)
        {
            i = i < 0 ? i + 3 : i > 2 ? i - 3 : i;
            if (i == 0) return A;
            if (i == 1) return B;
            return C;
        }

        /// <summary>
        /// Set the edge by index.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="t"></param>
        public void SetEdge(int i, TriangleVectors t)
        {
            i = i < 0 ? i + 3 : i > 2 ? i - 3 : i;
            if (i == 0) AB = t;
            if (i == 1) BC = t;
            if (i == 2) CA = t;
        }

        /// <summary>
        /// Get the indexed edge.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public TriangleVectors Edge(int i)
        {
            return i == 0 ? AB : i == 1 ? BC : CA;
        }
    }
}
