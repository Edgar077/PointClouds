/*----------------------------------------------------------------------
class cVertexList.

This has no direct correspondent in the C code, as a cVertex cell
is a cVertex list in C.  This object has two data members: n, the
number of elements (with zero meaning an empty list), and head,
one cVertex cell, and arbitrary one in the list (in C this is a
pointer to a cell).
The methods are all list methods: InsertBeforeHead, Delete, etc.
MakeNullVertex -- makes a default vertex and inserts it into 
the list.
GetElement (int index) gets element with wanted index.
ReverseList reverses a list in place.
ReverseListCompletely -- makes the last element to be head and 
head to be last.
GetNearVertex finds the vertex in the list closest to a particular
point (specified by a user mouseclick elsewhere).
----------------------------------------------------------------------*/

using OpenTK;
using OpenTKExtension;
using System;
using System.Collections.Generic;




namespace OpenTKExtension
{


    public class cVertexList
    {
        public int n;              // 0 means empty; 1 means one vertex; etc.
        public cVertex head;

        public cVertexList()
        {
            head = null;
            n = 0;
        }
        public List<cVertex> ListVertices
        {
            get
            {
                List<cVertex> myList = new List<cVertex>();
                int i = 0;
                cVertex f = this.head;
                //myList.Add(f);

                do
                {
                    ++i;
                    myList.Add(f);
                    f = f.NextVertex;

                } while (f != this.head);
                return myList;
            }
        }
        public cVertex GetElement(int index)
        {

            cVertex v = new cVertex();
            if (index <= n)
            {
                v = head;
                for (int i = 0; i < index; i++)
                    v = v.NextVertex;

            }
            else v = new cVertex(10000, 10000);

            return v;
        }

        public cVertex MakeNullVertex()
        {
            cVertex v = new cVertex();
            InsertBeforeHead(v);
            return v;
        }

        public void InitHead(cVertex h)
        {
            head = new cVertex();
            head = h;
            head.NextVertex = head.PrevVertex = head;
            n = 1;
        }

        public void ClearVertexList()
        {
            if (head != null)
                head = null;
            n = 0;
        }

        /*Inserts newV before oldV
         */
        public void InsertBeforeHead(cVertex ver)
        {
            if (head == null)
                InitHead(ver);
            else
            {
                InsertBefore(ver, head);
            }
        }

        public void InsertBefore(cVertex newV, cVertex oldV)
        {
            if (head == null)
                InitHead(newV);
            else
            {
                oldV.PrevVertex.NextVertex = newV;
                newV.PrevVertex = oldV.PrevVertex;
                newV.NextVertex = oldV;
                oldV.PrevVertex = newV;
                n++;
            }
        }

        public void SetVertex(float x, float y)
        {
            cVertex v = new cVertex(x, y);
            InsertBeforeHead(v);
        }

        public void SetVertex3D(float x, float y, float z)
        {
            cVertex v = new cVertex(x, y, z);
            InsertBeforeHead(v);
        }

        /* Adds vertex, inserting in between pointCloud of the closes edge */
        public void AddVertex(float x, float y)
        {
            cVertex v = new cVertex(x, y);
            //gets vertex of 1st vertex of the closest edge to the point	
            cVertex vNear = GetEdge(x, y);
            if (vNear != null)
                InsertBefore(v, vNear.NextVertex);
        }

        public void ResetVertex(cVertex resV, float x, float  y)
        {
            resV.Point.X = x;
            resV.Point.Y = y;
        }

        public void ResetVertex(cVertex resV, float x, float y, int vnum, bool mark)
        {
            resV.Point.X = x;
            resV.Point.Y = y;
            resV.IndexInPointCloud = vnum;
            resV.IsProcessed = mark;
        }

        public void Delete(cVertex ver)
        {
            if (head == head.NextVertex)
                head = null;
            else if (ver == head)
                head = head.NextVertex;

            ver.PrevVertex.NextVertex = ver.NextVertex;
            ver.NextVertex.PrevVertex = ver.PrevVertex;
            n--;
        }

        /*Makes a copy of present list
         */
        public void ListCopy(cVertexList list)
        {
            cVertex temp1 = head, temp2;
            do
            {
                temp2 = new cVertex(); // Create a new vertex cell
                temp2.Point = temp1.Point;     // Fill it with the same cPointi as in list
                temp2.IsProcessed = temp1.IsProcessed;
                temp2.IsEar = temp1.IsEar;
                temp2.Edge = temp1.Edge;
                temp2.IsOnHull = temp1.IsOnHull;
                temp2.IndexInPointCloud = temp1.IndexInPointCloud;
                list.InsertBeforeHead(temp2);
                temp1 = temp1.NextVertex;
            } while (temp1 != head);
        }

        /* Reverses the pointCloud, in order to get a ccw orientation	
         * 1234 becomes 1432
         */
        public void ReverseList()
        {
            cVertexList listcopy = new cVertexList();
            cVertex temp1, temp2;
            ListCopy(listcopy);
            this.ClearVertexList();

            //Fill this list in proper order:
            temp1 = listcopy.head;
            do
            {
                temp2 = new cVertex();
                temp2.Point = temp1.Point;
                InsertBeforeHead(temp2);
                temp1 = temp1.PrevVertex;
            } while (temp1 != listcopy.head);
            System.Diagnostics.Debug.WriteLine("Reversing list...");
        }

        /* Makes the last element to be the head and head to be the last, 
         * e.g., 0123 becomes 3210
         */
        public void ReverseListCompletely()
        {
            cVertexList listcopy = new cVertexList();
            cVertex temp1, temp2;
            ListCopy(listcopy);
            this.ClearVertexList();

            //Fill this list in proper order:
            temp1 = listcopy.head.PrevVertex;
            do
            {
                temp2 = new cVertex();
                temp2.Point = temp1.Point;
                temp2.IsProcessed = temp1.IsProcessed;
                temp2.IndexInPointCloud = temp1.IndexInPointCloud;
                InsertBeforeHead(temp2);
                temp1 = temp1.PrevVertex;
            } while (temp1 != listcopy.head.PrevVertex);
            System.Diagnostics.Debug.WriteLine("Reversing list completely...");
        }

        /* Returns the closest vertex to (x,y)
         */
        public cVertex GetNearVertex(float x, float y)
        {
            cVertex vnear = null, vtemp = head;
            float dist = -1, dx, dy, mindist = 0.0f;

            if (vtemp == null) return vnear;

            do
            {
                dx = vtemp.Point.X - x;
                dy = vtemp.Point.Y - y;
                dist = dx * dx + dy * dy;

                //Initialize on first pass (when vnear==null);
                //otherwise update if new winner
                if (vnear == null || dist < mindist)
                {
                    mindist = dist;
                    vnear = vtemp;
                }
                vtemp = vtemp.NextVertex;
            } while (vtemp != head);

            return vnear;
        }


        /*Finds the vertex that was clicked on (in a given boundary)
         */
        public cVertex FindVertex(float x, float y, float w, float h)
        {
            cVertex notfound = null;
            cVertex temp = head;

            if (n > 0)
            {
                do
                {
                    temp = temp.NextVertex;
                    if ((temp.Point.X <= x + (w / 2)) && (temp.Point.X >= x - (w / 2))
                       && (temp.Point.Y <= y + (h / 2)) && (temp.Point.Y >= y - (h / 2)))
                        return temp;
                } while (temp != head);
            }
            return notfound;
        }

        /*Returns nearest edge to (x,y) by returning prior vertex
         */
        public cVertex GetEdge(float x, float y)
        {
            cVertex vnear = null, vtemp = head;
            float mindist = 0 , dist = -1.0f;
            //int k;
            cPointi p = new cPointi();

            // input query point
            p.X = x;
            p.Y = y;

            if (vtemp == null)
                return vnear;

            do
            {
                dist = p.DistEdgePoint(vtemp.Point, vtemp.NextVertex.Point, p);
                vtemp.Point.PrintPoint();
                if (vnear == null || dist < mindist)
                {
                    mindist = dist;
                    vnear = vtemp;
                }
                vtemp = vtemp.NextVertex;
            } while (vtemp != head);

            return vnear;
        }

        /* Returns area of a polygon formed by the list of pointCloud
         */
        public float TrianglePoly2()
        {
            float sum = 0;
            cVertex a, p;

            p = head;      /* Fixed   */
            a = p.NextVertex;    /* Moving. */
            do
            {
                sum += p.Point.Triangle2(p.Point, a.Point, a.NextVertex.Point);
                a = a.NextVertex;
            } while (a.NextVertex != head);
            return sum;
        }

        /* Determine if the polygon/list is oriented counterclockwise (ccw).
         * (A more efficient method is possible, but here we use the available
         * TrianglePoly2())
         */
        public int Ccw()
        {
            float sign = TrianglePoly2();
            if (sign > 0) return 1;
            else return -1;
        }

        /* Returns true if polygon is covex, else returns false
         */
        public bool CheckForConvexity()
        {
            cVertex v = head;
            bool flag = true;

            do
            {
                if (!v.Point.LeftOn(v.Point, v.NextVertex.Point, v.NextVertex.NextVertex.Point))
                {
                    flag = false;
                    break;
                }
                v = v.NextVertex;
            } while (v != head);
            return flag;
        }

        /* QuickSort of elements using Compare2 function, 
         * used for computing Minkowski Convolution
         */
        public void Sort2(int lo0, int hi0)
        {
            if (lo0 >= hi0)
                return;
            cVertex mid = new cVertex();
            mid = GetElement(hi0);

            int lo = lo0;
            int hi = hi0 - 1;

            while (lo <= hi)
            {
                while (lo <= hi && (Compare2(GetElement(lo), mid) != -1))
                    lo++;

                while (lo <= hi && (Compare2(GetElement(hi), mid) != 1))
                    hi--;

                if (lo < hi)
                    Swap(GetElement(lo), GetElement(hi));
            }
            Swap(GetElement(lo), GetElement(hi0));

            Sort2(lo0, lo - 1);
            Sort2(lo + 1, hi0);
        }

        private void Swap(cVertex first, cVertex second)
        {
            cVertex temp;

            temp = new cVertex(first.Point.X, first.Point.Y);
            temp.IndexInPointCloud = first.IndexInPointCloud;
            temp.IsProcessed = first.IsProcessed;

            ResetVertex(first, second.Point.X, second.Point.Y, second.IndexInPointCloud, second.IsProcessed);
            ResetVertex(second, temp.Point.X, temp.Point.Y, temp.IndexInPointCloud, temp.IsProcessed);
        }

        /* Function used for Sort2
         */
        private int Compare2(cVertex tpi, cVertex tpj)
        {
            int a = 0;         /* TriangleSign result */
            float x = 0, y = 0;  /* projections in 1st quadrant */
            cVertex pi, pj;
            pi = tpi;
            pj = tpj;
            cPointi Origin = new cPointi();

            /* A vector in the open   upper halfplane is after
               a vector in the closed lower halfplane. */
            if ((pi.Point.Y > 0) && (pj.Point.Y <= 0))
                return 1;
            else if ((pi.Point.Y <= 0) && (pj.Point.Y > 0))
                return -1;

            /* A vector on the x-axis and one in the lower halfplane
              are handled by the Left computation below. */

            /* Both vectors on the x-axis requires special handling. */
            else if ((pi.Point.Y == 0) && (pj.Point.Y == 0))
            {
                if ((pi.Point.X < 0) && (pj.Point.X > 0))
                    return -1;
                if ((pi.Point.X > 0) && (pj.Point.X < 0))
                    return 1;
                else if (Math.Abs(pi.Point.X) < Math.Abs(pj.Point.X))
                    return -1;
                else if (Math.Abs(pi.Point.X) > Math.Abs(pj.Point.X))
                    return 1;
                else
                    return 0;
            }

            /* Otherwise, both in open upper halfplane, 
               or both in closed lower halfplane, but not both on x-axis. */
            else
            {

                a = Origin.TriangleSign(Origin, pi.Point, pj.Point);
                if (a > 0)
                    return -1;
                else if (a < 0)
                    return 1;
                else
                { /* Begin collinear */
                    x = Math.Abs(pi.Point.X) - Math.Abs(pj.Point.X);
                    y = Math.Abs(pi.Point.Y) - Math.Abs(pj.Point.Y);

                    if ((x < 0) || (y < 0))
                        return -1;
                    else if ((x > 0) || (y > 0))
                        return 1;
                    else /* points are coincident */
                        return 0;
                } /* End collinear */
            }
        }

        /* Printing to the console:
         */
        public void PrintVertices()
        {
            cVertex temp = head;
            int i = 1;
            if (head != null)
            {
                do
                {
                    temp.PrintVertex(i);
                    temp = temp.NextVertex;
                    i++;
                } while (temp != head);
            }
        }

        public void PrintVertices3D()
        {
            cVertex temp = head;
            System.Diagnostics.Debug.WriteLine("Printing pointCloud...");
            if (head != null)
            {
                do
                {
                    temp.PrintVertex3D();
                    temp = temp.NextVertex;
                } while (temp != head);
            }
        }

        public void PrintDetailed()
        {
            cVertex v = head;
            int i = 0;
            do
            {
                System.Diagnostics.Debug.WriteLine("V" + i + ": primary=" + v.IsProcessed + " | vnum=" + v.IndexInPointCloud);
                v.Point.PrintPoint();
                v = v.NextVertex; i++;
            } while (v != head);
        }

        /* Drawing routines
         */
        public void DrawPoints(System.Drawing.Graphics g, int w, int h)
        {
            ////vertex painting loop	  
            //if (n == 0)
            //    System.Diagnostics.Debug.WriteLine("No drawing is possible.");
            //else
            //{
            //    cVertex v = head;

            //    do
            //    {
            //        g.setColor(System.Drawing.Color.Blue);
            //        g.fillOval(v.v.x - (int)(w / 2), v.v.y - (int)(h / 2), w, h);
            //        v = v.next;
            //    } while (v != head.prev);
            //    g.fillOval(v.v.x - (int)(w / 2), v.v.y - (int)(h / 2), w, h);
            //}
        }

        /* Draws first vertex of the list
         */
        public void DrawHead(System.Drawing.Graphics g, int w, int h)
        {
            //cVertex v1 = head;
            //if (head == null)
            //    return;
            //g.setColor(System.Drawing.Color.Blue);
            //g.fillOval(v1.v.x - (int)(w / 2), v1.v.y - (int)(h / 2), w, h);
        }

        /* Draws polygon, filled or unfilled, 
         * depending on the fill bool variable
         */
        public void DrawPolygon(System.Drawing.Graphics g, int w, int h, System.Drawing.Color inColor, System.Drawing.Color vColor,
                    bool fill)
        {
            //  int[] xPoints = new int[ n+1];
            //  int[] yPoints = new int[n + 1];
            //  cVertex vtemp = head;
            //  int j = 0;
            //  if (head == null)
            //    return;
            //  do {
            //    xPoints[j] = vtemp.v.x;
            //    yPoints[j] = vtemp.v.y;
            //    j++;
            //    vtemp = vtemp.next;	
            //  } while ( vtemp != head );
            //  xPoints[ n] = head.v.x;
            //  yPoints[ n] = head.v.y;
            //  g.setColor( inColor );
            //  if (fill)
            //    g.fillPolygon( xPoints, yPoints, n);
            //  g.setColor( vColor );	
            //  g.drawPolygon( xPoints, yPoints, n+1);
            //  for(int k = 0; k < n; k++)
            //  {			    
            //    g.fillOval(xPoints[k] - (int)(w/2),
            //       yPoints[k] - (int)(h/2), w, h);
            //  }
            //}

            //      /* Draws not closed polygon boundary
            //       */
            //      public void DrawChain(System.Drawing.Graphics g, int w, int h)
            //      {
            //          //vertex painting loop	  
            //          if (head == null)
            //              System.Diagnostics.Debug.WriteLine("No drawing is possible.");
            //          else
            //          {
            //              cVertex v1 = head;
            //              cVertex v2;

            //              //do
            //              //{
            //              //    v2 = v1.next;
            //              //    g.setColor(System.Drawing.Color.Blue);
            //              //    if (n >= 2)
            //              //        g.drawLine(v1.v.x, v1.v.y, v2.v.x, v2.v.y);

            //              //    g.fillOval(v1.v.x - (int)(w / 2), v1.v.y - (int)(h / 2), w, h);
            //              //    g.fillOval(v2.v.x - (int)(w / 2), v2.v.y - (int)(h / 2), w, h);
            //              //    v1 = v1.next;
            //              //} while (v1 != head.prev);
            //          }
            //      }
        }
        public override string ToString()
        {
            return this.n.ToString();
        }

    }


}