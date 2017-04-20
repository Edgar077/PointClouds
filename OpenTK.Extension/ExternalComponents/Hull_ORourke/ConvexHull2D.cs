

/*-------------------------------------------------------------------
  Class for computing the CH in 2D
  No direct class in the C code. 
  Takes as input a list of points and produces a list called "top" 
  which contains the points on the CH.
  Methods:
  * RunHull corresponds to the "main" function in the C code; all the
  other methods are "translations" from C to Java of the code. 
  * qsort implements a QuickSort Algorithm in Java so that it uses the 
  Compare function. 
------------------------------------------------------------------*/


using OpenTK;
using OpenTKExtension;
using System;

namespace OpenTKExtension
{

    public class ConvexHull2D
    {

        private cVertexList list, top;
        //private cVertex ;//newpoint;
        private int ndelete = 0;
        private int i = 0;

        public ConvexHull2D(cVertexList list)
        {
            this.list = list;
        }

        public void ClearHull()
        {
            top = new cVertexList();

        }

        public void RunHull()
        {

            //initialization:

            cVertex v = new cVertex();
            v = list.head;
            for (i = 0; i < list.n; i++)
            {
                v.IndexInPointCloud = i;
                v = v.NextVertex;
            }
            //*********************
            FindLowest();
            qsort(list);
            if (ndelete > 0) Squash();
            top = Graham();
            top.PrintVertices();
        }



        /*---------------------------------------------------------------------
          Performs the Graham scan on an array of angularly sorted points P.
          ---------------------------------------------------------------------*/

        private cVertexList Graham()
        {
            cVertexList top;
            int i;
          //  cVertex p1, p2;  /* Top two points on stack. */


            /* Initialize stack. */
            top = new cVertexList();
            cVertex v1 = new cVertex(list.head.Point.X, list.head.Point.Y);
            v1.IndexInPointCloud = list.head.IndexInPointCloud;
            v1.IsProcessed = list.head.IsProcessed;

            cVertex v2 = new cVertex(list.head.NextVertex.Point.X, list.head.NextVertex.Point.Y);
            v2.IndexInPointCloud = list.head.NextVertex.IndexInPointCloud;
            v2.IsProcessed = list.head.NextVertex.IsProcessed;


            Push(v1, top);
            Push(v2, top);

            // Bottom two elements will never be removed. 
            i = 2;

            while (i < list.n)
            {
                cVertex v3 = new cVertex(list.GetElement(i).Point.X, list.GetElement(i).Point.Y);
                v3.IsProcessed = list.GetElement(i).IsProcessed;
                v3.IndexInPointCloud = list.GetElement(i).IndexInPointCloud;

                if (v1.Point.Left(top.head.PrevVertex.Point, top.head.PrevVertex.PrevVertex.Point, v3.Point))
                {
                    Push(v3, top);
                    i++;
                }
                else
                {
                    if (top.n > 2)
                    {
                        Pop(top);
                    }
                }

            }

            return top;

        }

        /*---------------------------------------------------------------------
          Squash removes all elements from list marked delete.
          ---------------------------------------------------------------------*/
        private void Squash()
        {
            cVertex v = new cVertex();
            v = list.head;
            for (i = 0; i < list.n; i++)
            {
                if (v.IsProcessed) list.Delete(v);
                v = v.NextVertex;
            }
        }


        private void Sort(cVertexList a, int lo0, int hi0)
        {
            if (lo0 >= hi0)
            {
                return;
            }
            cVertex mid = new cVertex();
            mid = a.GetElement(hi0);
            int lo = lo0;
            int hi = hi0 - 1;
            while (lo <= hi)
            {
                while (lo <= hi && ((Compare(a.GetElement(lo), mid) == 1) || (Compare(a.GetElement(lo), mid) == 0)))
                {
                    lo++;
                }

                while (lo <= hi && ((Compare(a.GetElement(hi), mid) == -1) || (Compare(a.GetElement(hi), mid) == 0)))
                {
                    hi--;
                }

                if (lo < hi)
                {
                    Swap(a.GetElement(lo), a.GetElement(hi));
                }

            }
            Swap(a.GetElement(lo), a.GetElement(hi0));
            Sort(a, lo0, lo - 1);
            Sort(a, lo + 1, hi0);
        }


        private void qsort(cVertexList a)
        {
            Sort(a, 1, a.n - 1);
        }

        /*---------------------------------------------------------------------
        Compare: returns -1,0,+1 if p1 < p2, =, or > respectively;
        here "<" means smaller angle.  Follows the conventions of qsort.
        ---------------------------------------------------------------------*/
        private int Compare(cVertex tpi, cVertex tpj)
        {
            float a;             //area 
            float x, y;          //projections of ri & rj in 1st quadrant 
            cVertex pi, pj;
            pi = tpi;
            pj = tpj;
            cVertex myhead = new cVertex();
            myhead = list.head;
            a = myhead.Point.TriangleSign(myhead.Point, pi.Point, pj.Point);
            if (a > 0)
                return -1;
            else if (a < 0)
                return 1;
            else
            { // Collinear with list.head
                x = Math.Abs(pi.Point.X - list.head.Point.X) - Math.Abs(pj.Point.X - list.head.Point.X);
                y = Math.Abs(pi.Point.Y - list.head.Point.Y) - Math.Abs(pj.Point.Y - list.head.Point.Y);
                ndelete++;

                if ((x < 0) || (y < 0))
                {
                    pi.IsProcessed = true;
                    return -1;
                }
                else if ((x > 0) || (y > 0))
                {
                    pj.IsProcessed = true;
                    return 1;
                }
                else
                { // points are coincident 

                    if (pi.IndexInPointCloud > pj.IndexInPointCloud)
                        pj.IsProcessed = true;
                    else
                        pi.IsProcessed = true;
                    return 0;

                }
            }
        }

        /*---------------------------------------------------------------------
        FindLowest finds the rightmost lowest point and swaps with 0-th.
        The lowest point has the min y-coord, and amongst those, the
        max x-coord: so it is rightmost among the lowest.
        ---------------------------------------------------------------------*/
        private void FindLowest()
        {
            int i;
            //int m = 0;   // Index of lowest so far. 
            cVertex v1;
            v1 = list.head.NextVertex;

            for (i = 1; i < list.n; i++)
            {
                if ((list.head.Point.Y < v1.Point.Y) ||
                     ((v1.Point.Y == list.head.Point.Y) && (v1.Point.X > list.head.Point.X)))
                {
                    Swap(list.head, v1);
                }
                v1 = v1.NextVertex;

            }
        }

        private void Swap(cVertex first, cVertex second)
        {
            cVertex temp = new cVertex();

            temp = new cVertex(first.Point.X, first.Point.Y);
            temp.IndexInPointCloud = first.IndexInPointCloud;
            temp.IsProcessed = first.IsProcessed;

            list.ResetVertex(first, second.Point.X, second.Point.Y, second.IndexInPointCloud, second.IsProcessed);
            list.ResetVertex(second, temp.Point.X, temp.Point.Y, temp.IndexInPointCloud, temp.IsProcessed);

        }

        private void Push(cVertex p, cVertexList top)
        {
            //simulating a stack behavior for cVertexList list
            //Push procedure
            top.InsertBeforeHead(p);
        }

        private void Pop(cVertexList top)
        {
            //simulating a stack behavior for cVertexList list
            //Pop procedure
            cVertex last = new cVertex();
            //last=top0.head.prev;
            top.Delete(top.head.PrevVertex);
        }

        public void DrawHull(System.Drawing.Graphics gContext, int w, int h)
        {

            if (list.head != null)
                list.DrawPoints(gContext, w, h);

            if (top.n == 0 || top.head == null)
                System.Diagnostics.Debug.WriteLine("No drawing is possible.");
            else
            {
                cVertex v1 = top.head;

                if (top.n > 2)
                {
                    //do
                    //{
                    //    gContext.drawLine(v1.v.x, v1.v.y, v1.next.v.x, v1.next.v.y);
                    //    v1 = v1.next;
                    //} while (v1 != top.head);

                }

            }//end else     
        }//end draw

    }//end class 
}