/*----------------------------------------------------------------------------
 * Class cSegSeg  -- used for computing intersection of
 *                   (currently) two segments
 *
 * ClearSegments -- used to clear previous intesection
 * SegSegTopLevel returns code of intersection
 * DrawSegments, DrawInters -- drawing routines;
 * 
 *---------------------------------------------------------------------------*/

using OpenTK;
using OpenTKExtension;
using System;

namespace OpenTKExtension
{

    public class cSegSeg
    {
        public cPointd p;                           /* main point of intersection */
        private cVertexList list;            /* list, in which the segements are stored */
        private cPointd q = new cPointd();   /* 2nd pt. of intersection, if it is a line*/
        char code = '0';                     /* intersection code returned by SegSegInt*/

        public cSegSeg(cVertexList list)
        {
            p = new cPointd(0, 0);
            this.list = list;
        }

        public void ClearSegments()
        {
            code = '0';
            p.x = p.y = q.x = p.y = 0;
        }

        /* -------------------------------------------------------------------------
         *The following set of routines compute the (real) intersection point between
         *two segments.  The two segments are taken to be the first four edges of
         *the input "polygon" list.  A character "code" is returned and printed out.
         */
        public char SegSegTopLevel()
        {
            // Set the segments ab and cd to be the first four points in the list.
            cVertex temp = list.head;
            cPointi a = temp.Point;
            temp = temp.NextVertex;
            cPointi b = temp.Point;
            temp = temp.NextVertex;
            cPointi c = temp.Point;
            temp = temp.NextVertex;
            cPointi d = temp.Point;
            // Store the results in class data field p, and returns code.
            code = a.SegSegInt(a, b, c, d, this.p, q);
            return code;
        }

        /* Draws segments
         */
        public void DrawSegments(System.Drawing.Graphics g, int w, int h)
        {
            //int k = 0;
            if (list.n == 0)
                System.Diagnostics.Debug.WriteLine("No drawing is possible.");
            else
            {
                cVertex v1 = list.head;
               // cVertex v2;

                do
                {
                    //v2 = v1.next;
                    //g.setColor(System.Drawing.Color.Blue);
                    //if (list.n >= 2 && k % 2 == 0)
                    //{
                    //    g.drawLine(v1.v.x, v1.v.y, v2.v.x, v2.v.y);
                    //}
                    //k++;
                    //g.fillOval(v1.v.x - (int)(w / 2), v1.v.y - (int)(h / 2), w, h);
                    //g.fillOval(v2.v.x - (int)(w / 2), v2.v.y - (int)(h / 2), w, h);
                    v1 = v1.NextVertex;
                } while (v1 != list.head.PrevVertex);
            }
        }

        /* Draws Intersection
         */
        public void DrawInters(System.Drawing.Graphics g, int w, int h)
        {
            //g.setColor(System.Drawing.Color.Red);
            //if (code != '0' && code != 'e')
            //{
            //    g.fillOval((int)p.x - (int)(w / 2), (int)p.y - (int)(h / 2), w, h);
            //}
            //else if (code == 'e')
            //{
            //    g.drawLine((int)p.x, (int)p.y, (int)q.x, (int)q.y);
            //    g.fillOval((int)p.x - (int)(w / 2), (int)p.y - (int)(h / 2), w, h);
            //    g.fillOval((int)q.x - (int)(w / 2), (int)q.y - (int)(h / 2), w, h);
            //}
        }
    }
}