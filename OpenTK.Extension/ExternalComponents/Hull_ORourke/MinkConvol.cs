/*----------------------------------------------------------------------------
 * Class MinkCovol  -- computes Minkowski Convolution of two polygons
 *
 * the second polygon, which is moved around the first, must be convex
 *---------------------------------------------------------------------------*/

using OpenTK;
using OpenTKExtension;
using System;

namespace OpenTKExtension
{

    public class MinkConvol
    {

        private cVertexList P, B;   /* list of vectors, list of the 2nd polygon */
        private cVertexList output; /* list capturing  the enlarged, resulting polygon */
        private int m;         /* Total number of points in both polygons */
        private int n;         /* Number of points in primary polygon */
        private int s;         /* Number of points in secondary polygon */
        private int j0;
        private cPointi p0;
        private cPointi last;  /* Holds the last vector difference. */

        public MinkConvol()
        {
            p0 = new cPointi();
            last = new cPointi();
        }

        public void ClearMinkConvol()
        {
            P.ClearVertexList();
            output.ClearVertexList();
            m = n = s = 0;
            p0.X = p0.Y = 0;
        }

        public bool Start(cVertexList p, cVertexList q)
        {
            p0.X = p0.Y = 0;
            if (!CheckForConvexity(p, q))
            {
                System.Diagnostics.Debug.WriteLine("Second polygon is  not convex...");
                return false;
            }
            else
            {
                B = new cVertexList();
                q.ListCopy(B);
                n = p.n;
                s = q.n;
                m = n + m;
                P = new cVertexList();
                cVertex v = p.head;
                do
                {
                    cVertex t = new cVertex(v.Point.X, v.Point.Y);
                    P.InsertBeforeHead(t);
                    v = v.NextVertex;
                } while (v != p.head);
                j0 = ReadVertices();
                output = new cVertexList();
            }
            Vectorize();
            System.Diagnostics.Debug.WriteLine("Before sorting ...");
            PrintPoints();
            Qsort();
            System.Diagnostics.Debug.WriteLine("After sorting ... ");
            PrintPoints();
            Convolve();
            return true;
        }

        private bool CheckForConvexity(cVertexList A, cVertexList B)
        {
            if (A.Ccw() != 1)
                A.ReverseList();
            if (B.Ccw() != 1)
                B.ReverseList();

            if (!B.CheckForConvexity())  /* Second polygon must be convex */
                return false;

            B.ReverseList();

            return true;
        }

        private int ReadVertices()
        {
            cVertex v = P.head;
            int i = 0;
            do
            {
                v.IndexInPointCloud = i++;
                v.IsProcessed = true;
                v = v.NextVertex;
            } while (v != P.head);

            v = B.head;
            do
            {
                cVertex temp = new cVertex(v.Point.X, v.Point.Y);
                P.InsertBeforeHead(temp);
                v = v.NextVertex;
            } while (v != B.head);

            v = P.GetElement(n); i = 0;
            do
            {
                /* Reflect secondary polygon */
                v.Point.X = -v.Point.X;
                v.Point.Y = -v.Point.Y;
                v.IndexInPointCloud = i++;
                v.IsProcessed = false;
                v = v.NextVertex;
            } while (v != P.head);

            float xmin, ymin, xmax, ymax;     /* Primary min & max */
            float sxmin, symin, sxmax, symax; /* Secondary min & max */
            int mp, ms;   /* i index of max (u-r) primary and secondary points */
            xmin = ymin = xmax = ymax = 0;
            sxmin = symin = sxmax = symax = 0;
            mp = ms = 0; v = P.head;
            xmin = xmax = v.Point.X;
            ymin = ymax = v.Point.Y;
            mp = 0; i = 1;
            v = v.NextVertex;
            cVertex startB = P.GetElement(n);
            do
            {
                if (v.Point.X > xmax) xmax = v.Point.X;
                else if (v.Point.X < xmin) xmin = v.Point.X;
                if (v.Point.Y > ymax) { ymax = v.Point.Y; mp = i; }
                else if (v.Point.Y == ymax && (v.Point.X > P.GetElement(mp).Point.X)) mp = i;
                else if (v.Point.Y < ymin) ymin = v.Point.Y;
                v = v.NextVertex; i++;
            } while (v != startB);
            /*System.Diagnostics.Debug.WriteLine("Index of upper rightmost primary, i=mp = "+mp);*/
            v = startB;
            sxmin = sxmax = v.Point.X;
            symin = symax = v.Point.Y;
            ms = n; v = v.NextVertex; i = 1;
            do
            {
                if (v.Point.X > sxmax) sxmax = v.Point.X;
                else if (v.Point.X < sxmin) sxmin = v.Point.X;
                if (v.Point.Y > symax) { symax = v.Point.Y; ms = i; }
                else if (v.Point.Y == symax && (v.Point.X > P.GetElement(ms).Point.X)) ms = i;
                else if (v.Point.Y < symin) symin = v.Point.Y;
                v = v.NextVertex; i++;
            } while (v != P.head.NextVertex);
            /*System.Diagnostics.Debug.WriteLine("Index of upper rightmost secondary, i=ms = "+ms);*/

            /* Compute the start point: upper rightmost of both. */
            System.Diagnostics.Debug.WriteLine("p0:");
            p0.PrintPoint();
            System.Diagnostics.Debug.WriteLine("mp is: " + mp);
            System.Diagnostics.Debug.WriteLine("mp element:" + P.GetElement(mp).Point.X + "," + P.GetElement(mp).Point.Y);
            AddVec(p0, P.GetElement(mp).Point, p0);
            System.Diagnostics.Debug.WriteLine("p0 after addvec:");
            p0.PrintPoint();
            System.Diagnostics.Debug.WriteLine("ms is: " + ms);
            System.Diagnostics.Debug.WriteLine("ms element:" + P.GetElement(ms).Point.X + "," + P.GetElement(ms).Point.Y);
            //   AddVec( p0, P.GetElement(ms).v, p0 );
            System.Diagnostics.Debug.WriteLine("p0 after another addvec:");
            p0.PrintPoint();
            return mp;
        }

        private void PrintPoints()
        {
            System.Diagnostics.Debug.WriteLine("Combined list of points, P: ");
            P.PrintDetailed();
        }

        private void Qsort()
        {
            P.Sort2(0, P.n - 1);
            PrintPoints();
            P.ReverseListCompletely();
            System.Diagnostics.Debug.WriteLine("list reversed...");
        }

        private void Vectorize()
        {
            //int i;
            cVertex v;
            v = P.head;
            System.Diagnostics.Debug.WriteLine("Vectorize: ");
            System.Diagnostics.Debug.WriteLine("list before victorization");
            P.PrintVertices();
            cVertex startB = P.GetElement(n);
            System.Diagnostics.Debug.WriteLine("startB !!!: ");
            startB.PrintVertex();

            SubVec(P.head.Point, startB.PrevVertex.Point, last);
            do
            {
                cPointi c = SubVec(v.NextVertex.Point, v.Point);
                System.Diagnostics.Debug.WriteLine("(" + v.NextVertex.Point.X + "," + v.NextVertex.Point.Y + ") - (" + v.Point.X + "," + v.Point.Y + ")");
                v.Point.X = c.X;
                v.Point.Y = c.Y;
                v = v.NextVertex;
            } while (v != startB.PrevVertex);
            startB.PrevVertex.Point.X = last.X;
            startB.PrevVertex.Point.Y = last.Y;

            SubVec(startB.Point, P.head.PrevVertex.Point, last);
            v = startB;
            do
            {
                cPointi c = SubVec(v.NextVertex.Point, v.Point);
                System.Diagnostics.Debug.WriteLine("(" + v.NextVertex.Point.X + "," + v.NextVertex.Point.Y + ") - (" + v.Point.X + "," + v.Point.Y + ")");
                v.Point.X = c.X;
                v.Point.Y = c.Y;
                v = v.NextVertex;
            } while (v != P.head.PrevVertex);
            P.head.PrevVertex.Point.X = last.X;
            P.head.PrevVertex.Point.Y = last.Y;
        }

        private void Convolve()
        {
            int i = 0;      /* Index into sorted edge vectors P */
            int j = 0;      /* Primary polygon indices */
            cVertex v = P.head;

            System.Diagnostics.Debug.WriteLine("Convolve: Start array i = " + i + ", primary j0=" + j0);
            PutInOutput(p0.X, p0.Y);

            i = 0;  /* Start at angle -pi, rightward vector. */
            j = j0; /* Start searching for j0. */
            v = P.GetElement(i);
            System.Diagnostics.Debug.WriteLine("Convolve, getElement(0)..." + v.Point.X + ", " + v.Point.Y);
            do
            {

                /* Advance around secondary edges until next j reached. */
                while (!(v.IsProcessed && v.IndexInPointCloud == j))
                {
                    if (!v.IsProcessed)
                    {
                        p0 = AddVec(p0, v.Point);
                        PutInOutput(p0.X, p0.Y);
                    }
                    v = v.NextVertex;
                    i = (i + 1) % m;
                    //	System.Diagnostics.Debug.WriteLine("X: i incremented to "+i);
                }

                /* Advance one primary edge. */
                System.Diagnostics.Debug.WriteLine("X: j=" + j + " found at i=" + i);
                p0 = AddVec(p0, v.Point);
                PutInOutput(p0.X, p0.Y);
                j = (j + 1) % n;
                System.Diagnostics.Debug.WriteLine("X: j incremented to " + j);

            } while (j != j0);

            /* Finally, complete circuit on secondary/robot polygon. */
            while (i != 0)
            {
                if (!v.IsProcessed)
                {
                    p0 = AddVec(p0, v.Point);
                    PutInOutput(p0.X, p0.Y);
                }
                i = (i + 1) % m;
            }
            System.Diagnostics.Debug.WriteLine("X: i incremented to " + i + " in  circuit");
        }

        private void PutInOutput(float x, float y)
        {
            cVertex v = new cVertex(x, y);
            output.InsertBeforeHead(v);
        }

        /*---------------------------------------------------------------------
          a - b ==> c.
          ---------------------------------------------------------------------*/
        private void SubVec(cPointi a, cPointi b, cPointi c)
        {
            c.X = a.X - b.X;
            c.Y = a.Y - b.Y;
        }

        private cPointi SubVec(cPointi a, cPointi b)
        {
            cPointi c = new cPointi();
            c.X = a.X - b.X;
            c.Y = a.Y - b.Y;
            return c;
        }

        /*---------------------------------------------------------------------
          a + b ==> c.
          ---------------------------------------------------------------------*/
        private void AddVec(cPointi a, cPointi b, cPointi c)
        {
            c.X = a.X + b.X;
            c.Y = a.Y + b.Y;
        }

        private cPointi AddVec(cPointi a, cPointi b)
        {
            cPointi c = new cPointi();
            c.X = a.X + b.X;
            c.Y = a.Y + b.Y;
            return c;
        }

        /* Draws the Minkowski Convolution (e.g. only the enlarged polygon)
         */
        public void DrawMinkConvol(System.Drawing.Graphics g, int w, int h)
        {
            System.Diagnostics.Debug.WriteLine("before drawing enlarged polygon, its pointCloud:");
            output.PrintVertices();

            cVertex v1 = output.head;
            //cVertex v2;

            //do
            //{
            //    v2 = v1.next;
            //    g.setColor(System.Drawing.Color.Pink);
            //    if (P.n >= 2)
            //        g.drawLine(v1.v.x, v1.v.y, v2.v.x, v2.v.y);
            //    g.fillOval(v1.v.x - (int)(w / 2), v1.v.y - (int)(h / 2), w, h);
            //    g.fillOval(v2.v.x - (int)(w / 2), v2.v.y - (int)(h / 2), w, h);
            //    v1 = v1.next;
            //} while (v1 != output.head.prev);
            //g.drawLine(v1.v.x, v1.v.y, v1.next.v.x, v1.next.v.y);
            //System.Diagnostics.Debug.WriteLine("the enlarged polygon has been drawn");
        }
    }
}