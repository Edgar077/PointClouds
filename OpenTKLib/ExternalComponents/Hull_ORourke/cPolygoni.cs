/*----------------------------------------------------------------------
class cPolygoni (class polygon, integer).

This corresponds to cPolygoni in the C code: type polygon, integer.
It is primarily a circular list of its pointCloud, an object of
type cVertexList called "list."  Other data members are used for
various polygon computations: diaglist for a (circular) list
of diagonals, CG for the coordinates of the center of gravity,
and various structures for point-in-polygon queries not yet
fully implemented.

There are many methods for this class.  We tried to include those
here that are specific to polygons, as opposed to list routines,
or basic geometric routines based on a constant number of points.
They can be classified into three groups:

  1. Those relating to input of the polygon, especially editing:
     ReadPoly() for reading from sysin.
     ClearPolygon() to clean out the structures when the user clicks on
        the "clear" button.
     SetVertex() to fix the coordinates of a vertex.
     AddVertex() to add a new vertex in response to a user mouseclick.
  2. Those that perform some high-level polygon computation:
     FindCG() to compute the CG.
     Triangulate() to triangulate.
     InPoly1() to determine if a point is in the polygon (not yet finished).
  3. Subsidiary methods needed by those in the second group:
     EarInit(), etc. [a long list].

----------------------------------------------------------------------*/

using OpenTK;
using OpenTKExtension;
using System;


namespace OpenTKExtension
{

    public class cPolygoni
    {
        static int SCREENWIDTH = 400;	//max number of intersections 
        private cVertexList list, listcopy;		//pointCloud of polygon
        public cDiagonalList diaglist;
        public cPointd CG;      	       	        //center of gravity;
        // or pt of intersection
        float[] inters = new float[SCREENWIDTH];
        private int intCount = 0;		//counts intersections 
        //when shooting the array in inPoly1
        private bool diagdrawn = true;     //diag-s've been drawn after triang?

        public cPolygoni(cVertexList list)
        {
            this.list = list;
            listcopy = new cVertexList();
            diaglist = new cDiagonalList();
            CG = new cPointd(0, 0);
            intCount = 0;
            diagdrawn = true;
        }

        /* Used to free up resourses */
        public void ClearPolygon()
        {
            listcopy.ClearVertexList();
            diaglist.ClearDiagonalList();
            CG.x = CG.y = 0;
            intCount = 0;
            for (int i = 0; i < SCREENWIDTH; i++)
                inters[i] = 0;
            //    System.Diagnostics.Debug.WriteLine("Polygon cleared");
            diagdrawn = true;
            PrintPoly();
        }

        public void ClearDiagList()
        {
            diaglist.ClearDiagonalList();
        }

        public void ClearListCopy()
        {
            listcopy.ClearVertexList();
        }

        public bool DiagDrawn()
        {
            return diagdrawn;
        }

        public void SetDiagDrawn(bool boolv)
        {
            diagdrawn = boolv;
        }

        /* Returns x coordinate of intersection of the InPoly line with polygon */
        public float GetInters(int i)
        {
            return inters[i];
        }

        /*See Chapter 7 for an explanation of this code
         *The only modification we make is that the polygon is *not* translated 
         *to place q=(0,0)
         */
        public char InPoly1(cPointi q)
        {
            int i = 0;//, i1;
            //int d;
            float x;
            int Rcross = 0;
            int Lcross = 0;
            bool Rstrad, Lstrad;
            cVertex vtemp = list.head;
            cVertex vtemp1;
            vtemp1 = list.head;
            intCount = 0;

            //For each edge e = (i-1,i), see if crosses rays*/
            //This code deviates from that in Chapter 7: 
            //the polygon is *not* translated to move q to the origin
            do
            {
                /*First check if q =(0,0) is a vertex.*/
                if (vtemp.Point.X == q.X && vtemp.Point.Y == q.Y)
                    return 'v';

                vtemp1 = new cVertex();
                vtemp1 = vtemp.PrevVertex;

                /*Check if e straddles x-axis, with bias above/below.*/
                Rstrad = ((vtemp.Point.Y - q.Y) > 0) != ((vtemp1.Point.Y - q.Y) > 0);
                Lstrad = ((vtemp.Point.Y - q.Y) < 0) != ((vtemp1.Point.Y - q.Y) < 0);

                if (Rstrad || Lstrad)
                {
                    /*Compute intersection of e with x-axis.*/
                    x = ((vtemp.Point.X - q.X) * (vtemp1.Point.Y - q.Y) -
                         (vtemp1.Point.X - q.X) * (vtemp.Point.Y - q.Y))
                          / (float)((vtemp1.Point.Y - q.Y) - (vtemp.Point.Y - q.Y));
                    /* saving the x-coordinates for the intersections;*/
                    inters[intCount] = (float)x + q.X;
                    intCount += 1;

                    if (Rstrad && x > 0) Rcross++;
                    if (Lstrad && x < 0) Lcross++;
                }
                vtemp = vtemp.NextVertex;
                i++;
            } while (vtemp != list.head);

            /*q on an edge if L/Rcross counts are not the same parity*/
            if ((Rcross % 2) != (Lcross % 2)) return 'e';
            /*q inside iff an odd number of crossings*/
            if ((Rcross % 2) == 1) return 'i';
            else
                return 'o';
        }

        /* Finds polygon's center of gravity */
        public void FindCG()
        {
            float A2, areaSum2 = 0;  	//partial area sum
            cPointi Cent3 = new cPointi();
            cVertex temp = list.head;
            cPointi fixedv = list.head.Point;
            this.CG.x = 0;
            this.CG.y = 0;

            do
            {
                Cent3 = fixedv.Centroid3(fixedv, temp.Point, temp.NextVertex.Point);
                A2 = fixedv.Triangle2(fixedv, temp.Point, temp.NextVertex.Point);
                this.CG.x = CG.x + (A2 * Cent3.X);
                this.CG.y = CG.y + (A2 * Cent3.Y);
                areaSum2 = areaSum2 + A2;
                temp = temp.NextVertex;
            } while (temp != list.head.PrevVertex);
            //Division by 3 is delayed to the last moment.
            this.CG.x = this.CG.x / (3 * areaSum2);
            this.CG.y = this.CG.y / (3 * areaSum2);
        }

        /*---------------------------------------------------------------------
         *Returns true iff (a,b) is a proper internal *or* external	
         *diagonal of P, *ignoring edges incident to a and b*.
         */
        public bool Diagonalie(cVertex a, cVertex b)
        {
            cVertex c, c1;

            /* For each edge (c,c1) of P */
            c = listcopy.head;
            do
            {
                c1 = c.NextVertex;
                /* Skip edges incident to a or b */
                if ((c != a) && (c1 != a)
                    && (c != b) && (c1 != b)
                    && a.Point.Intersect(a.Point, b.Point, c.Point, c1.Point)
                    )
                    return false;
                c = c.NextVertex;
            } while (c != listcopy.head);
            return true;
        }


        /*---------------------------------------------------------------------
         *This function initializes the data structures, and calls
         *Triangulate2 to clip off the ears one by one.
         *Note that it operates on listcopy rather than list,
         *to coordinate with Triangulate()
         */
        public void EarInit()
        {
            cVertex v0, v1, v2;   /* three consecutive pointCloud */

            /* Initialize v1->ear for all pointCloud. */
            v1 = listcopy.head;
            do
            {
                v2 = v1.NextVertex;
                v0 = v1.PrevVertex;
                v1.IsEar = Diagonal(v0, v2);
                v1 = v1.NextVertex;
            } while (v1 != listcopy.head);
        }


        /*---------------------------------------------------------------------
         *Prints out n-3 diagonals (as pairs of integer indices)
         *which form a triangulation of P.  This algorithm is O(n^2).
         *See Chapter 1 for a full explanation.
         *Triangulate operates on listcopy rather than list, so that
         *the original polygon is not destroyed.
         */
        public void Triangulate()
        {
            cVertex v0, v1, v2, v3, v4;   // five consecutive pointCloud 
            cDiagonal diag;
            int n = listcopy.n;         //number of pointCloud; shrinks to 3
            bool earfound = false;     //to prevent infinite loop on improper input

            EarInit();

            /* Each step of outer loop removes one ear. */
            while (n > 3)
            {
                /* Inner loop searches for an ear. */
                v2 = listcopy.head;
                do
                {
                    if (v2.IsEar)
                    {
                        /* Ear found. Fill variables. */
                        v3 = v2.NextVertex; v4 = v3.NextVertex;
                        v1 = v2.PrevVertex; v0 = v1.PrevVertex;

                        /* (v1,v3) is a diagonal */
                        earfound = true;
                        diag = new cDiagonal(v1, v3);
                        diag.PrintDiagonal(listcopy.n - n);
                        diaglist.InsertBeforeHead(diag);

                        /* Update earity of diagonal endpoints */
                        v1.IsEar = Diagonal(v0, v3);
                        v3.IsEar = Diagonal(v1, v4);

                        /* Cut off the ear v2 */
                        v1.NextVertex = v3;
                        v3.PrevVertex = v1;
                        listcopy.head = v3;      /* In case the head was v2. */
                        n--;
                        break;   /* out of inner loop; resume outer loop */
                    } /* end if ear found */
                    v2 = v2.NextVertex;
                } while (v2 != listcopy.head);
                if (!earfound)
                {
                    System.Diagnostics.Debug.WriteLine("Polygon is nonsimple: cannot triangulate");
                    break;
                }
                else
                    earfound = false;
                diagdrawn = false;
            } /* end outer while loop */
        }

        /*---------------------------------------------------------------------
         *Creates a copy of list, with each new cell (temp2) pointing to 
         *the same cPointi object as in the corresponding old cell (temp1).
         */
        public void ListCopy()
        {
            cVertex temp1 = list.head, temp2;
            do
            {
                temp2 = new cVertex(); // Create a new vertex cell
                temp2.Point = temp1.Point;     // Fill it with the same cPointi as in list
                listcopy.InsertBeforeHead(temp2);
                temp1 = temp1.NextVertex;
            } while (temp1 != list.head);
        }


        /*---------------------------------------------------------------------
         *Returns TRUE iff the diagonal (a,b) is strictly internal to the 
         *polygon in the neighborhood of the a endpoint.  
         */
        public bool InCone(cVertex a, cVertex b)
        {
            cVertex a0, a1;	/* a0,a,a1 are consecutive pointCloud. */

            a1 = a.NextVertex;
            a0 = a.PrevVertex;

            /* If a is a convex vertex ... */
            if (a.Point.LeftOn(a.Point, a1.Point, a0.Point))
                return a.Point.Left(a.Point, b.Point, a0.Point)
                  && b.Point.Left(b.Point, a.Point, a1.Point);

            /* Else a is reflex: */
            return !(a.Point.LeftOn(a.Point, b.Point, a1.Point)
              && b.Point.LeftOn(b.Point, a.Point, a0.Point));
        }


        /*---------------------------------------------------------------------
         *Returns TRUE iff (a,b) is a proper internal diagonal.
         */
        public bool Diagonal(cVertex a, cVertex b)
        {
            return InCone(a, b) && InCone(b, a) && Diagonalie(a, b);
        }

        public float TrianglePoly2()
        {
            return list.TrianglePoly2();
        }

        public void PrintPoly()
        {
            cVertex v = list.head;
            int i = 0;
            if (v == null)
                System.Diagnostics.Debug.WriteLine("Polygon is empty");
            else
                do
                {
                    System.Diagnostics.Debug.WriteLine("Vertex " + i + " (" + v.Point.X + ", " + v.Point.Y + ")");
                    i++;
                    v = v.NextVertex;
                } while (v != list.head);
        }

        /* Draws a line and the quiry point of the InPoly request
         */
        public void DrawInPoly(System.Drawing.Graphics g, cPointi queryP,
                   int CanW, int CanH, int w, int h)
        {
            //int k; 		      
            //g.setColor(System.Drawing.Color.Black);
            //g.drawLine(0, queryP.y, CanW, queryP.y);
            //for(k = 0; k < intCount; k++)
            //{
            //  g.fillOval(GetInters(k)-(int)(w/2), queryP.y - (int)(h/2), w, h);
            //}			
            //g.setColor(System.Drawing.Color.Red);
            //g.fillOval(queryP.x - (int)(w/2), queryP.y - (int)(h/2), w, h);
        }

        /* Draws Centroid
         */
        public void DrawCentroid(System.Drawing.Graphics g, int w, int h)
        {
            //g.setColor(System.Drawing.Color.Red);
            //g.fillOval((int)CG.x - (int)(w/2), (int)CG.y - (int)(h/2), w,h);
        }

    } //End class cPolygoni
}