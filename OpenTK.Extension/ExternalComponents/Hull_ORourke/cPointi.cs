/*----------------------------------------------------------------------
 * class cPointi.
 *
 * This corresponds to the C struct cPointi -- type point, integer.
 * It holds the x and y coordinates as integer data fields.
 * It contains all the methods that specifically work on points
 * (as opposed to those that are vertex-specific): triangle area
 * (Triangle2), Collinear, Between, etc.
 * Also there are routines that compute the distance between
 * two points (Dist), the distance between a point and a segment
 * (DistEdgePoint); these functions are called during polygon
 * editing, to determine which point or edge the user has selected
 * with a mouse click.
 * SegSegInt: Finds the point of intersection p between two closed
 *
 *---------------------------------------------------------------------*/


using OpenTK;
using OpenTKExtension;
using System;

namespace OpenTKExtension
{

    public class cPointi
    {
        public float X;
        public float Y;
        public float Z;

        public cPointi()
        {
            X = Y = Z = 0;
        }

        public cPointi(float x, float y)
        {
            this.X = x;
            this.Y = y;
            this.Z = 0;
        }

        public cPointi(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        ///*Returns the distance of the input point 
        // *from its perp. proj. to the e1 edge.
        // *Uses method detailed in comp.graphics.algorithms FAQ 
        // */
        public float DistEdgePoint(cPointi a, cPointi b, cPointi c)
        {
            float r, s;
            float Length;
            float dproj = 0.0f;
            Length = Convert.ToSingle(Math.Sqrt(Math.Pow((b.X - a.X), 2) +
                    Math.Pow((b.Y - a.Y), 2))  );

            if (Length == 0)
            {
                System.Diagnostics.Debug.WriteLine("DistEdgePoint: Length = 0");
                a.PrintPoint();
                b.PrintPoint();
                c.PrintPoint();
            }
            r = (((a.Y - c.Y) * (a.Y - b.Y))
               - ((a.X - c.X) * (b.X - a.X))) / (Length * Length);
            s = (((a.Y - c.Y) * (b.X - a.X))
               - ((a.X - c.X) * (b.Y - a.Y))) / (Length * Length);

            dproj = Math.Abs(s * Length);

            //    System.Diagnostics.Debug.WriteLine("XI = " + (a.x + r *(b.x-a.x))+" YI = "+(a.y+r*(b.y-a.y)));
            if ((s != 0) && ((0.0 <= r) && (r <= 1)))
                return dproj;
            if ((s == 0) && Between(a, b, c))
                return 0.0f;
            else
            {
                float ca = Dist(a, c);
                float cb = Dist(b, c);
                return Math.Min(ca, cb);
            }
        }

        public float Dist(cPointi p, cPointi p1) //returns the distance of two points
        {
            float l =Convert.ToSingle( Math.Sqrt(Math.Pow((p.X - p1.X), 2) + Math.Pow((p.Y - p1.Y), 2)));
            return l;
        }

        public float Dist(cPointi p) //returns the distance of two points
        {
            float l = Convert.ToSingle(Math.Sqrt(Math.Pow((p.X - this.X), 2) + Math.Pow((p.Y - this.Y), 2)));
            return l;
        }

        /*Centroid of triangle is just an average of pointCloud
         */
        public cPointi Centroid3(cPointi p1, cPointi p2, cPointi p3)
        {
            cPointi c = new cPointi();
            c.X = p1.X + p2.X + p3.X;
            c.Y = p1.Y + p2.Y + p3.Y;
            return c;
        }

        /*The signed area of the triangle det. by a,b,c; pos. if ccw, neg. if cw
         */
        public float Triangle2(cPointi a, cPointi b, cPointi c)
        {
            float area = ((c.X - b.X) * (a.Y - b.Y)) - ((a.X - b.X) * (c.Y - b.Y));
            return area;
        }

        public int TriangleSign(cPointi a, cPointi b, cPointi c)
        {
            float area2;

            area2 = (b.X - a.X) * (float)(c.Y - a.Y) -
                    (c.X - a.X) * (float)(b.Y - a.Y);


            /* The area should be an integer. */
            if (area2 > 0.5) return 1;
            else if (area2 < -0.5) return -1;
            else return 0;
        }

        /*---------------------------------------------------------------------
         *Returns true if c is strictly to the left of the directed
         *line through a to b.
         */
        public bool Left(cPointi a, cPointi b, cPointi c)
        {
            return TriangleSign(a, b, c) > 0;
        }

        public bool LeftOn(cPointi a, cPointi b, cPointi c)
        {
            return TriangleSign(a, b, c) >= 0;
        }

        public bool Collinear(cPointi a, cPointi b, cPointi c)
        {
            return TriangleSign(a, b, c) == 0;
        }

        /*---------------------------------------------------------------------
         *Returns true iff point c lies on the closed segement ab.
         *First checks that c is collinear with a and b.
         */
        public bool Between(cPointi a, cPointi b, cPointi c)
        {
            //cPointi ba, ca;

            if (!Collinear(a, b, c))
                return false;

            /* If ab not vertical, check betweenness on x; else on y. */
            if (a.X != b.X)
                return ((a.X <= c.X) && (c.X <= b.X)) ||
                   ((a.X >= c.X) && (c.X >= b.X));
            else
                return ((a.Y <= c.Y) && (c.Y <= b.Y)) ||
                   ((a.Y >= c.Y) && (c.Y >= b.Y));
        }

        /*---------------------------------------------------------------------
         *Returns TRUE iff segments ab & cd intersect, properly or improperly.
         */
        public bool Intersect(cPointi a, cPointi b, cPointi c, cPointi d)
        {
            if (IntersectProp(a, b, c, d))
                return true;

            else if (Between(a, b, c)
                 || Between(a, b, d)
                 || Between(c, d, a)
                 || Between(c, d, b))
                return true;

            else
                return false;
        }

        public bool IntersectProp(cPointi a, cPointi b, cPointi c, cPointi d)
        {
            /* Eliminate improper cases. */
            if (
            Collinear(a, b, c) ||
            Collinear(a, b, d) ||
            Collinear(c, d, a) ||
            Collinear(c, d, b))
                return false;

            return
                 Xor(Left(a, b, c), Left(a, b, d))
              && Xor(Left(c, d, a), Left(c, d, b));
        }

        /*---------------------------------------------------------------------
         *Exclusive or: true iff exactly one argument is true.
         */
        public bool Xor(bool x, bool y)
        {
            /* The arguments are negated to ensure that they are 0/1 values. */
            /* (Idea due to Michael Baldwin.) */
            return !x ^ !y;
        }

        /*---------------------------------------------------------------------
          SegSegInt: Finds the point of intersection p between two closed
          segments ab and cd.  Returns p and a char with the following meaning:
          'e': The segments collinearly overlap, sharing a point.
          'v': An endpoint (vertex) of one segment is on the other segment,
          but 'e' doesn't hold.
          '1': The segments intersect properly (i.e., they share a point and
          neither 'v' nor 'e' holds).
          '0': The segments do not intersect (i.e., they share no points).
          Note that two collinear segments that share just one point, an endpoint
          of each, returns 'e' rather than 'v' as one might expect.
          ---------------------------------------------------------------------*/
        public char SegSegInt(cPointi a, cPointi b, cPointi c, cPointi d, cPointd p, cPointd q)
        {
            float s, t;       /* The two parameters of the parametric eqns. */
            float num, denom;  /* Numerator and denoninator of equations. */
            char code = '?';    /* Return char characterizing intersection. */
            p.x = p.y = 100.0f;  /* For testing purposes only... */

            denom = a.X * (float)(d.Y - c.Y) +
                    b.X * (float)(c.Y - d.Y) +
                    d.X * (float)(b.Y - a.Y) +
                    c.X * (float)(a.Y - b.Y);

            /* If denom is zero, then segments are parallel: handle separately. */
            if (denom == 0)
                return ParallelInt(a, b, c, d, p, q);

            num = a.X * (float)(d.Y - c.Y) +
                 c.X * (float)(a.Y - d.Y) +
                     d.X * (float)(c.Y - a.Y);
            if ((num == 0) || (num == denom)) code = 'v';
            s = num / denom;
            System.Diagnostics.Debug.WriteLine("SegSegInt: num=" + num + ",denom=" + denom + ",s=" + s);

            num = -(a.X * (float)(c.Y - b.Y) +
                 b.X * (float)(a.Y - c.Y) +
                 c.X * (float)(b.Y - a.Y));
            if ((num == 0) || (num == denom)) code = 'v';
            t = num / denom;
            System.Diagnostics.Debug.WriteLine("SegSegInt: num=" + num + ",denom=" + denom + ",t=" + t);

            if ((0.0 < s) && (s < 1) &&
                  (0.0 < t) && (t < 1))
                code = '1';
            else if ((0.0 > s) || (s > 1) ||
                  (0.0 > t) || (t > 1))
                code = '0';

            p.x = a.X + s * (b.X - a.X);
            p.y = a.Y + s * (b.Y - a.Y);

            return code;
        }

        public char ParallelInt(cPointi a, cPointi b, cPointi c, cPointi d, cPointd p, cPointd q)
        {
            if (!a.Collinear(a, b, c))
                return '0';

            if (Between1(a, b, c) && Between1(a, b, d))
            {
                Assigndi(p, c);
                Assigndi(q, d);
                return 'e';
            }
            if (Between1(c, d, a) && Between1(c, d, b))
            {
                Assigndi(p, a);
                Assigndi(q, b);
                return 'e';
            }
            if (Between1(a, b, c) && Between1(c, d, b))
            {
                Assigndi(p, c);
                Assigndi(q, b);
                return 'e';
            }
            if (Between1(a, b, c) && Between1(c, d, a))
            {
                Assigndi(p, c);
                Assigndi(q, a);
                return 'e';
            }
            if (Between1(a, b, d) && Between1(c, d, b))
            {
                Assigndi(p, d);
                Assigndi(q, b);
                return 'e';
            }
            if (Between1(a, b, d) && Between1(c, d, a))
            {
                Assigndi(p, d);
                Assigndi(q, a);
                return 'e';
            }
            return '0';
            /*    
             if ( Between1( a, b, c ) ) {
               Assigndi( p, c );
               return 'e';
             }
             if ( Between1( a, b, d ) ) {
               Assigndi( p, d );
               return 'e';
             }
             if ( Between1( c, d, a ) ) {
               Assigndi( p, a );
               return 'e';
             }
             if ( Between1( c, d, b ) ) {
               Assigndi( p, b );
               return 'e';
             }
             return '0';
            */
        }

        public void Assigndi(cPointd p, cPointi a)
        {
            p.x = a.X;
            p.y = a.Y;
        }

        /*---------------------------------------------------------------------
          Returns TRUE iff point c lies on the closed segement ab.
          Assumes it is already known that abc are collinear.
          (This is the only difference with Between().)
          ---------------------------------------------------------------------*/
        public bool Between1(cPointi a, cPointi b, cPointi c)
        {
            //cPointi ba, ca;

            /* If ab not vertical, check betweenness on x; else on y. */
            if (a.X != b.X)
                return ((a.X <= c.X) && (c.X <= b.X)) ||
              ((a.X >= c.X) && (c.X >= b.X));
            else
                return ((a.Y <= c.Y) && (c.Y <= b.Y)) ||
              ((a.Y >= c.Y) && (c.Y >= b.Y));
        }

        public void PrintPoint()
        {
            System.Diagnostics.Debug.WriteLine(" (" + X + "," + Y + ")");
        }
    }







}