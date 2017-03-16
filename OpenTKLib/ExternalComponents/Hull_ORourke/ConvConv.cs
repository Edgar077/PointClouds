/*------------------------------------------------------------------------
Class ConvConv - used for computing intersection of two convex polygons
This code is described in "Computational Geometry in C" (Second Edition),
Chapter 7.  It is not written to be comprehensible without the
explanation in that book.
-------------------------------------------------------------------------*/


using OpenTK;
using OpenTKExtension;
using System;


namespace OpenTKExtension
{

    public class ConvConv
    {

        private int n, m;
        private cVertexList P, Q;
        private cVertexList inters;  /* intersection of the two polygons */
        private cVertex a, b;        /* indices on P and Q (resp.) */
        private cPointi A, B;        /* directed edges on P and Q (resp.) */
        private int cross;       /* sign of z-component of A x B */
        private int bHA, aHB;    /* b in H(A); a in H(b). */
        private cPointi Origin;      /* (0,0) */
        private cPointd p;           /* float point of intersection */
        private cPointd q;           /* second point of intersection */
        private cInFlag inflag;      /* {Pin, Qin, Unknown}: which inside */
        private int aa, ba;      /* # advances on a & b indices (after 1st inter.) */
        private bool FirstPoint;  /* Is this the first point? (used to initialize).*/
        private cPointd p0;          /* The first point. */
        private int code;        /* SegSegInt return code. */
        public bool intersection = true;

        public ConvConv()
        {

        }

        /* Equivalent of main() function in the C code,
         * returns false if polygons are not convex
         */
        public bool Start(cVertexList p, cVertexList q)
        {
            intersection = true;
            this.P = new cVertexList();
            this.Q = new cVertexList();
            p.ListCopy(P);
            q.ListCopy(Q);
            if (!CheckForConvexity())
            {
                System.Diagnostics.Debug.WriteLine("Polygons are not convex...");
                return false;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Polygons are convex...");
                n = P.n;
                m = Q.n;
                inters = new cVertexList();
                ConvexIntersect(P, Q, n, m);
            }
            return true;
        }

        public void ClearConvConv()
        {
            P.ClearVertexList();
            Q.ClearVertexList();
            inters.ClearVertexList();
        }

        private bool CheckForConvexity()
        {
            if (P.Ccw() != 1)
                P.ReverseList();
            if (Q.Ccw() != 1)
                Q.ReverseList();
            if (!P.CheckForConvexity())
                return false;

            if (!Q.CheckForConvexity())
                return false;

            return true;
        }

        /*---------------------------------------------------------------------
         * Consult the book for explanations
         *--------------------------------------------------------------------*/
        private void ConvexIntersect(cVertexList P, cVertexList Q, int n, int m)
        /* P has n pointCloud, Q has m pointCloud. */
        {

            /* Initialize variables. */
            a = new cVertex();
            b = new cVertex();
            a = P.head; b = Q.head;
            aa = ba = 0;
            Origin = new cPointi();  /* (0,0) */
            inflag = new cInFlag();
            FirstPoint = true;
            cVertex a1, b1;
            A = new cPointi();
            B = new cPointi();
            p = new cPointd();
            q = new cPointd();
            p0 = new cPointd();

            do
            {
                /* System.Diagnostics.Debug.WriteLine("Before Advances:a="+a.v.x+","+a.v.y+
               ", b="+b.v.x+","+b.v.y+"; aa="+aa+", ba="+ba+"; inflag="+
               inflag.f); */
                /* Computations of key variables. */
                a1 = a.PrevVertex;
                b1 = b.PrevVertex;

                SubVec(a.Point, a1.Point, A);
                SubVec(b.Point, b1.Point, B);

                cross = Origin.TriangleSign(Origin, A, B);
                aHB = b1.Point.TriangleSign(b1.Point, b.Point, a.Point);
                bHA = a1.Point.TriangleSign(a1.Point, a.Point, b.Point);
                System.Diagnostics.Debug.WriteLine("cross=" + cross + ", aHB=" + aHB + ", bHA=" + bHA);

                /* If A & B intersect, update inflag. */
                code = a1.Point.SegSegInt(a1.Point, a.Point, b1.Point, b.Point, p, q);
                System.Diagnostics.Debug.WriteLine("SegSegInt: code = " + code);

                if (code == '1' || code == 'v')
                {
                    if (inflag.f == cInFlag.Unknown && FirstPoint)
                    {
                        aa = ba = 0;
                        FirstPoint = false;
                        p0.x = p.x; p0.y = p.y;
                        InsertInters(p0.x, p0.y);
                    }
                    inflag = InOut(p, inflag, aHB, bHA);
                    System.Diagnostics.Debug.WriteLine("InOut sets inflag=" + inflag.f);
                }

                /*-----Advance rules-----*/

                /* Special case: A & B overlap and oppositely oriented. */
                if ((code == 'e') && (Dot(A, B) < 0))
                {
                    InsertSharedSeg(p, q);
                    return;
                }

                /* Special case: A & B parallel and separated. */
                if ((cross == 0) && (aHB < 0) && (bHA < 0))
                {
                    System.Diagnostics.Debug.WriteLine("P and Q are disjoint.");
                    return;
                }


                /* Special case: A & B collinear. */
                else if ((cross == 0) && (aHB == 0) && (bHA == 0))
                {
                    /* Advance but do not output point. */
                    if (inflag.f == cInFlag.Pin)
                        b = Advance(b, "ba", inflag.f == cInFlag.Qin, b.Point);
                    else
                        a = Advance(a, "aa", inflag.f == cInFlag.Pin, a.Point);
                }

                /* Generic cases. */
                else if (cross >= 0)
                {
                    if (bHA > 0)
                        a = Advance(a, "aa", inflag.f == cInFlag.Pin, a.Point);
                    else
                        b = Advance(b, "ba", inflag.f == cInFlag.Qin, b.Point);
                }
                else /* if ( cross < 0 ) */
                {
                    if (aHB > 0)
                        b = Advance(b, "ba", inflag.f == cInFlag.Qin, b.Point);
                    else
                        a = Advance(a, "aa", inflag.f == cInFlag.Pin, a.Point);
                }
                System.Diagnostics.Debug.WriteLine("After advances:a=(" + a.Point.X + ", " + a.Point.Y +
                       "), b=(" + b.Point.X + ", " + b.Point.Y + "); aa=" + aa +
                       ", ba=" + ba + "; inflag=" + inflag.f);

                /* Quit when both adv. indices have cycled, or one has cycled twice. */
            } while (((aa < n) || (ba < m)) && (aa < 2 * n) && (ba < 2 * m));

            if (!FirstPoint) /* If at least one point output, close up. */
                InsertInters(p0.x, p0.y);

            /* Deal with special cases: not implemented. */
            if (inflag.f == cInFlag.Unknown)
            {
                System.Diagnostics.Debug.WriteLine("The boundaries of P and Q do not cross.");
                intersection = false;
            }
        }

        private void InsertInters(float x, float y)
        {
            cVertex v = new cVertex((int)x, (int)y);
            inters.InsertBeforeHead(v);
        }

        /*---------------------------------------------------------------------
          Prints out the float point of intersection, and toggles in/out flag.
          ---------------------------------------------------------------------*/
        public cInFlag InOut(cPointd p, cInFlag inflag, int aHB, int bHA)
        {
            InsertInters(p.x, p.y);

            /* Update inflag. */
            if (aHB > 0)
            {
                inflag.f = cInFlag.Pin; return inflag;
            }
            else if (bHA > 0)
            {
                inflag.f = cInFlag.Qin; return inflag;
            }
            else    /* Keep status quo. */
                return inflag;
        }

        /*---------------------------------------------------------------------
          Advances and prints out an inside vertex if appropriate.
          ---------------------------------------------------------------------*/
        private cVertex Advance(cVertex a, String counter, bool inside, cPointi v)
        {
            if (inside)
                InsertInters(v.X, v.Y);
            if (counter.Equals("aa"))
                aa++;
            else if (counter.Equals("ba"))
                ba++;
            return a.NextVertex;
        }

        /*---------------------------------------------------------------------
          a - b ==> c.
          ---------------------------------------------------------------------*/
        private void SubVec(cPointi a, cPointi b, cPointi c)
        {
            c.X = a.X - b.X;
            c.Y = a.Y - b.Y;
        }

        /*---------------------------------------------------------------------
          Returns the dot product of the two input vectors.
          ---------------------------------------------------------------------*/
        private float Dot(cPointi a, cPointi b)
        {
            //int i;
            float sum = 0.0f;

            sum = a.X * b.X + a.Y * b.Y;

            return sum;
        }

        public void InsertSharedSeg(cPointd p, cPointd q)
        {
            InsertInters((int)p.x, (int)p.y);
            InsertInters((int)q.x, (int)q.y);
        }

        public void DrawIntersection(System.Drawing.Graphics g, int w, int h, System.Drawing.Color fillColor)
        {
            if (!intersection)
                return;
            else
            {
                inters.DrawPolygon(g, w, h, fillColor, System.Drawing.Color.Red, true);
            }
        }
    }

    public class cInFlag
    {
        public int f;
        public static int Pin = -1;
        public static int Qin = 1;
        public static int Unknown = 0;

        public cInFlag()
        {
            f = Unknown;
        }
    }
}