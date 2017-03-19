
/*----------------------------------------------------------------------
  class cChain (class polygon, integer).
  
  Doesn't have any direct correspondent in the C code as a class. 
  Nevertheless, it consists of a circular list (instance of cVertexList) 
  called "list", which contains all pointCloud in the chain.

  It has specific methods. The main one is Solven, colorresponding to its 
  homonymous in the C code. This method determines if a certain point in the 
  plane is reachable by the arm formed from the chain that was introduced. 
  It also permits the normal editing for the points: deletion, moving, adding. 
  Most of the methods I used here are similar to the ones from the C code and 
  pretty straightfoerward. 

  For drawing, there is an extra feathure: the applet will display all the 
  points introduced at the previous step, although the C code just 
  "straightens" the arm in two or three links (depends on the position of the 
  point). The extra points dissapear after one editing, providing the fact 
  that the chain is "cleaned" after each edit operation. 

  There is also a pop-up window that warns the user when the point is out
  of reach. 



*********************************************************************/

using OpenTK;
using OpenTKExtension;
using System;


namespace OpenTKExtension
{

    public class cChain
    {
        static int SCREENWIDTH = 350;	//max number of intersections 
        cVertexList list, listcopy;		//pointCloud of polygon
        // or pt of intersection
        private int[] inters = new int[SCREENWIDTH];

        private float[] linklen = new float[1000];
        private float[] linklenback = new float[1000];

        //private cVertex newpoint, c, q;

        public cVertex v1, v2;

        float L1, L2, L3;		//Length of links between kinks
        float totLength;		//total Length of all links
        //float halfLength;		//floor of half os total

        int i = 0;
        int m; 			//index of median link
        int firstlinks = 0;
        int nlinks;
        int nlinksback = 0;
        //private bool toClose = false;
        //int r1, r2;
        public int intersection = 0;
        /*intersection is used for determining the number of intersections of the
        two circles: 0 for point out of reach, 1 for two tangent circles, 
        2 for 2 intersections, 3 for identical circles. */

        /*constructor*/
        public cChain(cVertexList list)
        {
            this.list = list;
        }


        public void SetAChain(cPointd Jk, cPointi target)
        {
            cPointi vaux1;
            cPointi vaux2;
            vaux1 = new cPointi(list.head.Point.X, list.head.Point.Y);
            vaux2 = new cPointi(target.X, target.Y);
            list.ClearVertexList();
            list.SetVertex(vaux1.X, vaux1.Y);
            list.SetVertex((int)(Jk.x + .5), (int)(Jk.y + .5));
            list.SetVertex(vaux2.X, vaux2.Y);

        }

        public void ClearChain()
        /*for cleaning the chain after each edit operation*/
        {

            nlinksback = 0;
            firstlinks = 0;
            listcopy = new cVertexList();

        }

        public float Length(cPointi point1, cPointi point2)
        /*Computes the Length of the link between two points
          Used for the Solven (and the subsequent) methods.*/
        {

            return (Convert.ToSingle(Math.Sqrt((float)Math.Abs(((point1.X - point2.X) * (point1.X - point2.X)
                               + (point1.Y - point2.Y) * (point1.Y - point2.Y))))));

        }


        //****************************************************

        public float Length2(cPointi v)
        /* Returns the squared distance in between two points*/
        {
            float ss;
            ss = 0;
            ss = (float)(v.X * v.X + v.Y * v.Y);
            return ss;
        }

        //********************************

        public void SubVec(cPointi a, cPointi b, cPointi c)
        /* has the same result as  the SubVec method in the C code.*/
        {

            c.X = a.X - b.X;
            c.Y = a.Y - b.Y;

        }
        //**************************************

        public bool Solven(float x, float y)

        /*Is called when the user drags the last point of the link or releases it. 
          Corresponds to the Solven method in C*/
        {
            float halfLength;	//floor of half os total
            cPointi target;   //point for storing the target
            cPointd Jk;      // coords of kinked joint returned by Solve2 
           // cPointi J1;      // Joint1 on x-axis 



            //create target
            target = new cPointi(x, y);

            //Compute Length array and # of links
            //cVertex v0;
            v1 = list.head;

            for (i = 0; i < list.n - 1; i++)
            {
                linklen[i] = (Length(v1.Point, v1.NextVertex.Point) + .5f);
                v1 = v1.NextVertex;
            }
            nlinks = list.n - 1;


            //Compute total&half Length

            totLength = 0;

            for (i = 0; i < nlinks; i++)
                totLength += linklen[i];
            halfLength = totLength / 2;

            //Find median link
            if (nlinks > 2)
            {
                L1 = 0;
                for (m = 0; m < nlinks; m++)
                {
                    if ((L1 + linklen[m]) > halfLength)
                        break;
                    L1 += linklen[m];
                }//end for

                L2 = linklen[m];
                L3 = totLength - L1 - L2;
                firstlinks = m;
                for (i = 0; i < nlinks; i++)
                    linklenback[i] = linklen[i];
                nlinksback = nlinks;

            }
            else if (nlinks == 2)
            {
                L1 = linklen[0];
                L2 = linklen[1];
                L3 = 0;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Just one link!!!");
                L1 = L2 = L3 = 0;
            }

            if ((nlinks == 3) && (nlinksback == 0)) nlinksback = 3;
            if (nlinks == 2)
            {
                Jk = new cPointd(0, 0);
                if (Solve2(L1, L2, target, Jk))
                {
                    System.Diagnostics.Debug.WriteLine("Solve2 for 2 links: link1= " + L1 + ", link2= " + L2 + ", joint=\n");
                    LineTo_d(Jk);
                    SetAChain(Jk, target);
                    return true;
                }
                else return false;
            }//end if nlinks==2
            else
            {
                if (Solve3(L1, L2, L3, target))
                    return true;
                else return false;
            }

        }//end Solve

        public bool Solve3(float L1, float L2, float L3, cPointi target0)
        {
            cPointi target;
            cPointd Jk;      // coords of kinked joint returned by Solve2 
            cPointi J1;      // Joint1 on x-axis 
            cPointi Ttarget; // translated target

            //cPointi vaux1;
            //cPointi vaux2;

            target = new cPointi(target0.X, target0.Y);


            System.Diagnostics.Debug.WriteLine("==>Solve3: links = " + L1 + ", " + L2 + ", " + L3);

            Jk = new cPointd(0, 0);

            if (Solve2(L1 + L2, L3, target, Jk))
            {
                firstlinks++;
                nlinks = 2;

                System.Diagnostics.Debug.WriteLine("Solve3: link1=" + (L1 + L2) + ", link2=" + L3 + ", joint=\n");
                LineTo_d(Jk);
                SetAChain(Jk, target);
                return true;
            }
            else if (Solve2(L1, L2 + L3, target, Jk))
            {
                System.Diagnostics.Debug.WriteLine("Solve3: link1= " + L1 + ", link2= " + (L2 + L3) + ", joint=\n");
                nlinks = 2;
                LineTo_d(Jk);
                SetAChain(Jk, target);
                return true;
            }
            else
            {   // pin J0 to 0. 
                // Shift so J1 is origin. 
                //J1.x = L1;   J1.y = 0;
                J1 = new cPointi(L1, 0);
                Ttarget = new cPointi(0, 0);
                SubVec(target, J1, Ttarget);
                if (Solve2(L2, L3, Ttarget, Jk))
                {
                    // Shift solution back to origin. 
                    Jk.x += L1;
                    System.Diagnostics.Debug.WriteLine("Solve3: link1=" + L1 + ", link2= " + L2 + ", link3= " + L1 + ", joints=\n");
                    nlinks = 3;
                    LineTo_i(J1);
                    LineTo_d(Jk);
                    SetAChain(Jk, target);
                    cVertex VJ1 = new cVertex(list.head.Point.X + J1.X, list.head.Point.Y);
                    list.InsertBefore(VJ1, list.head.NextVertex);
                    return true;
                }
                else
                    return false;
            }
        }//end Solve3

        public bool Solve2(float L1, float L2, cPointi target, cPointd J)
        {
            cPointi c1 = new cPointi(list.head.Point.X, list.head.Point.Y);  // center of circle 1 
            int nsoln;           // # of solns: 0,1,2,3(infinite) 
            nsoln = TwoCircles(c1, L1, target, L2, J);
            return (nsoln != 0);
        }// end Solve2


        //---------------------------------------------------------------------
        //TwoCircles finds an intersection point between two circles.
        //General routine: no assumptions. Returns # of intersections; point in p.
        //---------------------------------------------------------------------

        public int TwoCircles(cPointi c1, float r1, cPointi c2, float r2, cPointd p)
        {
            cPointi c;
            cPointd q;
            int nsoln = -1;
            // Translate so that c1={0,0}. 
            c = new cPointi(0, 0);
            SubVec(c2, c1, c);
            q = new cPointd(0, 0);
            nsoln = TwoCircles0a(r1, c, r2, p);//p instead of 

            // Translate back. 
            p.x = p.x + c1.X;
            p.y = p.y + c1.Y;
            return nsoln;
        }

        //---------------------------------------------------------------------
        //TwoCircles0a assumes that the first circle is centered on the origin.
        //Returns # of intersections: 0, 1, 2, 3 (inf); point in p.
        //----------------------------------------------------------------------- 
        public int TwoCircles0a(float r1, cPointi c2, float r2, cPointd p)
        {
            float dc2;              // dist to center 2 squared 
            float rplus2, rminus2;  // (r1 +/- r2)^2 
            float f;                // fraction along c2 for nsoln=1 

            // Handle special cases. 
            dc2 = Length2(c2);
            rplus2 = (r1 + r2) * (r1 + r2);
            rminus2 = (r1 - r2) * (r1 - r2);

            // No solution if c2 out of reach + or -. 
            if ((dc2 > rplus2) || (dc2 < rminus2))
                return 0;

            // One solution if c2 just reached. 
            // Then solution is r1-of-the-way (f) to c2. 
            if (dc2 == rplus2)
            {
                f = r1 / (float)(r1 + r2);
                p.x = f * c2.X; p.y = f * c2.Y;
                return 1;
            }
            if (dc2 == rminus2)
            {
                if (rminus2 == 0)
                {   // Circles coincide. 
                    p.x = r1; p.y = 0;
                    return 3;
                }
                f = r1 / (float)(r1 - r2);
                p.x = f * c2.X; p.x = f * c2.Y;
                return 1;
            }
            // Two intersections. 

            int auxint = TwoCircles0b(r1, c2, r2, p);
            return auxint;
        }//end TwoCircles0a

        //---------------------------------------------------------------------
        //TwoCircles0b also assumes that the 1st circle is origin-centered.
        //---------------------------------------------------------------------         
        public int TwoCircles0b(float r1, cPointi c2, float r2, cPointd p)
        {
            float a2;          // center of 2nd circle when rotated to x-axis  
            cPointd q;          // one solution when c2 on x-axis  
            float cost, sint;  // sine and cosine of angle of c2  

            // Rotate c2 to a2 on x-axis.  
            a2 = Convert.ToSingle(Math.Sqrt(Length2(c2)));
            cost = c2.X / a2;
            sint = c2.Y / a2;
            q = new cPointd(0, 0);
            TwoCircles00(r1, a2, r2, q);

            // Rotate back  
            p.x = cost * q.x + -sint * q.y;
            p.y = sint * q.x + cost * q.y;

            return 2;
        }

        //---------------------------------------------------------------------
        //TwoCircles00 assumes circle centers are (0,0) and (a2,0).
        //--------------------------------------------------------------------- 
        public void TwoCircles00(float r1, float a2, float r2, cPointd p)
        {
            float r1sq, r2sq;
            r1sq = r1 * r1;
            r2sq = r2 * r2;

            // Return only positive-y soln in p.  
            p.x = (a2 + (r1sq - r2sq) / a2) / 2;
            p.y = Convert.ToSingle( Math.Sqrt(r1sq - p.x * p.x));
        }//end TwoCircles00


        /*Method used for cretaing the "extra-points" that will be displayed
          on the screen after the arm is straightened. Called from DrawPoints */

        public cVertexList MakePoints(int lo, int hi1, int hi2, cVertex first, cVertex last, cVertexList listcopy)
        {

            float xaux;     //auxiliary variable for storin the info
            float lenaux = 0;    //auxiliary variable for storing the Length of the
            //current link
            cPointi v1 = new cPointi(0, 0);   //aux variable for computing the values 
            //of the new points
            float sum = 0;   //the sum of the previous link Lengths

            for (i = lo; i < hi1; i++)
            {
                lenaux += linklenback[i];
            }
            sum = 0;

            for (i = lo; i < hi2; i++)
            {
                sum += linklenback[i];
                xaux = sum / (float)lenaux;
                v1.X = (int)(.5 + (1 - xaux) * first.Point.X + xaux * last.Point.X);
                v1.Y = (int)(.5 + (1 - xaux) * first.Point.Y + xaux * last.Point.Y);
                listcopy.SetVertex(v1.X, v1.Y);

            }//end for

            return listcopy;
        }

        public void DrawDots(System.Drawing.Graphics gContext, int w, int h)
        {

            cVertexList listcopy = new cVertexList();
            listcopy.SetVertex(list.head.Point.X, list.head.Point.Y);

            if (nlinks == 3)
            {
                /*for the first link:*/
                listcopy = MakePoints(0, firstlinks, firstlinks - 1, list.head, list.head.NextVertex, listcopy);
                /*set the middle link*/
                listcopy.SetVertex(list.head.NextVertex.Point.X, list.head.NextVertex.Point.Y);
                listcopy.SetVertex(list.head.NextVertex.NextVertex.Point.X, list.head.NextVertex.NextVertex.Point.Y);
                /* for the last link, the third one*/
                listcopy = MakePoints(firstlinks + 1, nlinksback, nlinksback, list.head.NextVertex.NextVertex, list.head.PrevVertex, listcopy);

            }

            else
            {
                if (nlinksback > 2)
                /*if we have any extra - points*/
                {
                    /*first link:*/
                    listcopy = MakePoints(0, firstlinks, firstlinks - 1, list.head, list.head.NextVertex, listcopy);
                    /*set the middle point:*/
                    listcopy.SetVertex(list.head.NextVertex.Point.X, list.head.NextVertex.Point.Y);
                    /*set the last link*/
                    listcopy = MakePoints(firstlinks, nlinksback + 1, nlinksback - 1, list.head.PrevVertex.PrevVertex, list.head.PrevVertex, listcopy);


                }//end if nlinksback > 2
            }// end else
            /*set the last point and draw everything*/
            listcopy.SetVertex(list.head.PrevVertex.Point.X, list.head.PrevVertex.Point.Y);
            listcopy.DrawPoints(gContext, w, h);

        }


        //********************************************************
        /*series of methods corresponding to the C code; just printing on the
          standard output */
        public void LineTo_i(cPointi p)
        {
            System.Diagnostics.Debug.WriteLine(" Line to i" + p.X + ", " + p.Y);
        }

        public void MoveTo_i(cPointi p)
        {
            System.Diagnostics.Debug.WriteLine(" Move to i" + p.X + ", " + p.Y);
        }

        public void LineTo_d(cPointd p)
        {
            System.Diagnostics.Debug.WriteLine(" Line to d" + p.x + ", " + p.y);
        }


    }//end class

}