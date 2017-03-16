/*
class InPoly

Determines if a point is inside or outside a polyhedron. 
The code is really close to the one in C. The structures are also similar. 
The input is very strict though.
*/



using OpenTK;
using OpenTKExtension;
using System;

namespace OpenTKExtension
{

    public class InPolyh
    {


       // private static int EXIT_FAILURE = 1;
        private static int Xindex = 0;
        private static int Yindex = 1;
        private static int Zindex = 2;

        private static int PMAX = 10000;

        //private int MAX_INT = int.MaxValue;
        private static int SAFE = 1000000;
        private static int DIM = 3;                  /* Dimension of points */
        public tPointi[] Vertices;        /* All the points */
        public tPointi[] Faces;           /* Each triangle face is 3 indices */
        public int check = 0;
        public tPointi[][] Box;          /* Box around each face */
        public int n, f;
        public int m;
        public float D = 0;


        public static void TestInPoly()
        {
            InPolyh ip = new InPolyh();

        }


        public InPolyh()
        {

            //int F = 0;
            tPointi bmin, bmax;
            float radius;

            //String s;
            //char c;
            //bool flag;
            //int counter;
            //float x, y, z;
            char[] line = new char[20];
            int i = 0;
            
          

            Vertices = new tPointi[PMAX];
            Faces = new tPointi[PMAX];
            Box = new tPointi[PMAX][];

            //n = ReadVertices();
           // F = ReadFaces();
            VerifVertices();

            /* Initialize the bounding box */
            bmin = new tPointi();
            bmax = new tPointi();

            for (i = 0; i < DIM; i++)
                bmin.p[i] = bmax.p[i] = Vertices[0].p[i];

            radius = ComputeBox(n, bmin, bmax);

            System.Diagnostics.Debug.WriteLine("radius=" + radius);

            System.Diagnostics.Debug.WriteLine("Please input query point");
            i = 0;
            //counter = 0;
            //flag = false;

            //try{
            //         System.Diagnostics.Debug.WriteLine("\n\nInput query point:\nCoord-s must be seperated by a *tab*\n"+
            //              "ENTER after each point"+"\nTo finish input type end + "+
            //              "ENTER at the end"+
            //              "\nExample:\n17      23      123\n34      5      1\nend\n"+
            //              "-----------------start entering data-------------------");
            //   //do {

            //  do {
            //c = (char) System.in.read();
            //line[i] = c;
            //i++;
            //  } while (c !='\n' );
            //  s = new String(line);
            //  s = s.Substring(0,i-1);
            //  if (s.Equals("end"))
            //break;
            //  flag = false;
            //  counter = 0;
            //  for (int j=0; j < s.Length(); j++) {
            //if (s.charAt(j) == '\t') 
            //  counter++; 
            //if (counter == 2) {
            //  flag = true; break; 
            //}
            //  }
            //  if (flag) {
            //int t = s.IndexOf('\t');
            //int t1 = s.LastIndexOf('\t');
            //x = float.TryParse(s.Substring(0,t));
            //y = float.TryParse(s.Substring(t+1,t1));
            //z = float.TryParse(s.Substring(t1+1,s.Length()));
            //q = new tPointi(x, y, z);

            //	q.x = x;
            //	q.y = y;
            //	q.z = z;
            //    i=0;
            //      }
            //      else 
            //    break;

            //      char ans = InPolyhedron(F, q, bmin, bmax, radius);
            //      System.Diagnostics.Debug.WriteLine("InPolyhedron returned:  "+ans);

            //    } while (!s.Equals("end"));
            // }
            //catch (NumberFormatException e) {System.Diagnostics.Debug.WriteLine ("Invalid input"); return;}
        }


        //**********************
        public void VerifVertices()
        {

            for (int v = 0; v < n; v++)
            {
                if ((Math.Abs(Vertices[v].p[Xindex]) > SAFE) || (Math.Abs(Vertices[v].p[Yindex]) > SAFE) || (Math.Abs(Vertices[v].p[Zindex]) > SAFE))
                {
                    System.Diagnostics.Debug.WriteLine("Coordinate of vertex below might be too large...");
                    System.Diagnostics.Debug.WriteLine(v);
                }
            }
        }

        //***********************
        /*
          This function returns a char:
            'V': the query point a coincides with a Vertex of polyhedron P.
            'E': the query point a is in the relative interior of an Edge of polyhedron P.
            'F': the query point a is in the relative interior of a Face of polyhedron P.
            'i': the query point a is strictly interior to polyhedron P.
            'o': the query point a is strictly exterior to( or outside of) polyhedron P.
        */

        char InPolyhedron(int F, tPointi q, tPointi bmin, tPointi bmax, int radius)
        {
            tPointi r;  /* Ray endpoint. */
            tPointd p;  /* Intersection point; not used. */
            int f, k = 0, crossings = 0;
            char code = '?';

            r = new tPointi();
            p = new tPointd();

            /* If query point is outside bounding box, finished. */
            if (!InBox(q, bmin, bmax))
                return 'o';

        
            while (k++ < F)
            {
                crossings = 0;

                RandomRay(r, radius);
                AddVec(q, r);
                System.Diagnostics.Debug.WriteLine("Ray endpoint: (" + r.p[0] + " , " + r.p[1] + " , " + r.p[2] + " )");

                for (f = 0; f < F; f++)
                {  /* Begin check each face */
                    if (BoxTest(f, q, r) == '0')
                    {
                        code = '0';
                        System.Diagnostics.Debug.WriteLine("BoxTest = 0!");
                    }
                    else code = SegTriInt(Faces[f], q, r, p);
                    System.Diagnostics.Debug.WriteLine("Face = " + f + ": BoxTest/SegTriInt returns " + code);

                    /* If ray is degenerate, then goto outer while to generate another. */
                    if (code == 'p' || code == 'v' || code == 'e')
                    {
                        System.Diagnostics.Debug.WriteLine("Degenerate ray");
                        continue;
                    }

                    /* If ray hits face at interior point, increment crossings. */

                    else if (code == 'f')
                    {
                        crossings++;
                        System.Diagnostics.Debug.WriteLine("crossings = " + crossings);
                    }

                    /* If query endpoint q sits on a V/E/F, return that code. */
                    else if (code == 'V' || code == 'E' || code == 'F')
                        return (code);

                    /* If ray misses triangle, do nothing. */
                    else if (code == '0')
                        continue;

                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Error");
                        return ' ';
                    }

                } /* End check each face */

                /* No degeneracies encountered: ray is generic, so finished. */
                break;

            } /* End while loop */

            System.Diagnostics.Debug.WriteLine("Crossings at the end = " + crossings);
            /* q strictly interior to polyhedron iff an odd number of crossings. */
            if ((crossings % 2) == 1)
                return 'i';
            else return 'o';
        }


        public int ComputeBox(int F, tPointi bmin, tPointi bmax)
        {
            int i, j;//, k;
            float radius;

            for (i = 0; i < F; i++)
                for (j = 0; j < DIM; j++)
                {
                    if (Vertices[i].p[j] < bmin.p[j])
                        bmin.p[j] = Vertices[i].p[j];
                    if (Vertices[i].p[j] > bmax.p[j])
                        bmax.p[j] = Vertices[i].p[j];
                }

            radius = Convert.ToSingle(Math.Sqrt(Math.Pow((float)(bmax.p[Xindex] - bmin.p[Xindex]), 2) +
                            Math.Pow((float)(bmax.p[Yindex] - bmin.p[Yindex]), 2) +
                            Math.Pow((float)(bmax.p[Zindex] - bmin.p[Zindex]), 2)  ));
            System.Diagnostics.Debug.WriteLine("radius =  " + radius);

            return (int)((radius + 1 + .5)) + 1;
        }

        /* Return a random ray endpoint */

        void RandomRay(tPointi ray, int radius)
        {
            float x, y, z, w, t;

            /* Generate a random point on a sphere of radius 1. */
            /* the sphere is sliced at z, and a random point at angle t
               generated on the circle of intersection. */
            Random r = new Random();

            z = 2* (float)r.NextDouble() - 1.0f;
            t = 2*  Convert.ToSingle(Math.PI * r.NextDouble());
            w = Convert.ToSingle( Math.Sqrt(1 - z * z));
            x = w *  Convert.ToSingle( Math.Cos(t));
            y = w *  Convert.ToSingle(Math.Sin(t));

            ray.p[Xindex] = (int)(radius * x + .5);
            ray.p[Yindex] = (int)(radius * y + .5);
            ray.p[Zindex] = (int)(radius * z + .5);

            System.Diagnostics.Debug.WriteLine("RandomRay returns" + ray.p[Xindex] + " , " + ray.p[Yindex] + " , " + ray.p[Zindex]);
        }

        public void AddVec(tPointi q, tPointi ray)
        {
            int i;

            for (i = 0; i < DIM; i++)
                ray.p[i] = q.p[i] + ray.p[i];
        }

        public bool InBox(tPointi q, tPointi bmin, tPointi bmax)
        {
            //int i;

            if ((bmin.p[Xindex] <= q.p[Xindex]) && (q.p[Xindex] <= bmax.p[Xindex]) &&
                (bmin.p[Yindex] <= q.p[Yindex]) && (q.p[Yindex] <= bmax.p[Yindex]) &&
                (bmin.p[Zindex] <= q.p[Zindex]) && (q.p[Zindex] <= bmax.p[Zindex]))
                return true;
            return false;
        }


        /*---------------------------------------------------------------------
            'p': The segment lies wholly within the plane.
            'q': The q endpoint is on the plane (but not 'p').
            'r': The r endpoint is on the plane (but not 'p').
            '0': The segment lies strictly to one side or the other of the plane.
            '1': The segement intersects the plane, and 'p' does not hold.
        ---------------------------------------------------------------------*/
        public char SegPlaneInt(tPointi T, tPointi q, tPointi r, tPointd p, int m)
        {
            tPointd N;
            int D0 = 0;
            tPointi rq;
            float num, denom, t;
            int i;

            N = new tPointd();
            rq = new tPointi();

            m = PlaneCoeff(T, N, D0);

            System.Diagnostics.Debug.WriteLine("m= " + m + "; plane=( " + N.p[Xindex] + " , " + N.p[Yindex] + " , " + N.p[Zindex] + " , " + D + " )");
            num = D - Dot(q, N);
            SubVec(r, q, rq);
            denom = Dot(rq, N);

            System.Diagnostics.Debug.WriteLine("SegPlaneInt: num=" + num + " , denom= " + denom);

            if (denom == 0)
            {  /* Segment is parallel to plane. */
                if (num == 0)   /* q is on plane. */
                    return 'p';
                else
                    return '0';
            }
            else
                t = num / denom;
            System.Diagnostics.Debug.WriteLine("SegPlaneInt: t= " + t);

            System.Diagnostics.Debug.WriteLine("p in seg plane int is: p=()");
            for (i = 0; i < DIM; i++)
            {
                p.p[i] = q.p[i] + t * (r.p[i] - q.p[i]);
                System.Diagnostics.Debug.WriteLine(p.p[i]);
            }


            if ((0.0 < t) && (t < 1))
                return '1';
            else if (num == 0)   /* t == 0 */
                return 'q';
            else if (num == denom) /* t == 1 */
                return 'r';
            else return '0';
        }
        /*---------------------------------------------------------------------
        Computes N & D and returns index m of largest component.
        ---------------------------------------------------------------------*/
        public int PlaneCoeff(tPointi T, tPointd N, float D0)
        {
            int i;
            float t;              /* Temp storage */
            float biggest = 0.0f;  /* Largest component of normal vector. */
            m = 0;             /* Index of largest component. */

            NormalVec(Vertices[T.p[0]], Vertices[T.p[1]], Vertices[T.p[2]], N);
            System.Diagnostics.Debug.WriteLine("PlaneCoeff: N=()" + N.p[Xindex] + " , " + N.p[Yindex] + " , " + N.p[Zindex]);
            D = Dot(Vertices[T.p[0]], N);
            System.Diagnostics.Debug.WriteLine("D should be in planecoeff" + D);

            /* Find the largest component of N. */
            for (i = 0; i < DIM; i++)
            {
                t = (float)(Math.Abs(N.p[i]));
                if (t > biggest)
                {
                    biggest = t;
                    m = i;
                }
            }
            return m;
        }
   
        /*---------------------------------------------------------------------
        a - b ==> c.
        ---------------------------------------------------------------------*/
        public void SubVec(tPointi a, tPointi b, tPointi c)
        {
            int i;

            for (i = 0; i < DIM; i++)
                c.p[i] = a.p[i] - b.p[i];
        }


        /*---------------------------------------------------------------------
        Returns the dot product of the two input vectors.
        ---------------------------------------------------------------------*/
        public float Dot(tPointi a, tPointd b)
        {
            int i;
            float sum = 0.0f;

            for (i = 0; i < DIM; i++)
                sum += a.p[i] * b.p[i];

            return sum;
        }


        /*---------------------------------------------------------------------
        Compute the cross product of (b-a)x(c-a) and place into N.
        ---------------------------------------------------------------------*/
        public void NormalVec(tPointi a, tPointi b, tPointi c, tPointd N)
        {
            N.p[Xindex] = (c.p[Zindex] - a.p[Zindex]) * (b.p[Yindex] - a.p[Yindex]) -
                   (b.p[Zindex] - a.p[Zindex]) * (c.p[Yindex] - a.p[Yindex]);
            N.p[Yindex] = (b.p[Zindex] - a.p[Zindex]) * (c.p[Xindex] - a.p[Xindex]) -
                   (b.p[Xindex] - a.p[Xindex]) * (c.p[Zindex] - a.p[Zindex]);
            N.p[Zindex] = (b.p[Xindex] - a.p[Xindex]) * (c.p[Yindex] - a.p[Yindex]) -
                   (b.p[Yindex] - a.p[Yindex]) * (c.p[Xindex] - a.p[Xindex]);
        }


        /* Assumption: p lies in the plane containing T.
            Returns a char:
             'V': the query point p coincides with a Vertex of triangle T.
             'E': the query point p is in the relative interior of an Edge of triangle T.
             'F': the query point p is in the relative interior of a Face of triangle T.
             '0': the query point p does not intersect (misses) triangle T.
        */

        public char InTri3D(tPointi T, int m, tPointi p)
        {
            int i;           /* Index for X,Y,Z           */
            int j;           /* Index for X,Y             */
            int k;           /* Index for triangle vertex */
            tPointi pp;      /* projected p */
            tPointi[] Tp;   /* projected T: three new pointCloud */

            pp = new tPointi();
            Tp = new tPointi[3];
            Tp[0] = new tPointi();
            Tp[1] = new tPointi();
            Tp[2] = new tPointi();


            /* Project out coordinate m in both p and the triangular face */
            j = 0;
            for (i = 0; i < DIM; i++)
            {
                if (i != m)
                {    /* skip largest coordinate */
                    pp.p[j] = p.p[i];
                    for (k = 0; k < 3; k++)
                        Tp[k].p[j] = Vertices[T.p[k]].p[i];
                    j++;
                }
            }

            return (InTri2D(Tp, pp));

        }

        public char InTri2D(tPointi[] Tp, tPointi pp)
        {
            int area0, area1, area2;

            /* compute three TriangleSign() values for pp w.r.t. each edge of the face in 2D */
            System.Diagnostics.Debug.WriteLine("In tri 2d pp, tp 0,1,2" + pp.p[0] + ", " + pp.p[1] + " , " + pp.p[2]);
            for (int b = 0; b < 3; b++)
                for (int r = 0; r < 3; r++)
                    System.Diagnostics.Debug.WriteLine(Tp[b].p[r]);


            area0 = TriangleSign(pp, Tp[0], Tp[1]);
            area1 = TriangleSign(pp, Tp[1], Tp[2]);
            area2 = TriangleSign(pp, Tp[2], Tp[0]);

            System.Diagnostics.Debug.WriteLine("area0= " + area0 + "  area1= " + area1 + " area2= " + area2);

            if ((area0 == 0) && (area1 > 0) && (area2 > 0) ||
                 (area1 == 0) && (area0 > 0) && (area2 > 0) ||
                 (area2 == 0) && (area0 > 0) && (area1 > 0))
                return 'E';

            if ((area0 == 0) && (area1 < 0) && (area2 < 0) ||
                 (area1 == 0) && (area0 < 0) && (area2 < 0) ||
                 (area2 == 0) && (area0 < 0) && (area1 < 0))
                return 'E';

            if ((area0 > 0) && (area1 > 0) && (area2 > 0) ||
                 (area0 < 0) && (area1 < 0) && (area2 < 0))
                return 'F';

            if ((area0 == 0) && (area1 == 0) && (area2 == 0))
            {
                System.Diagnostics.Debug.WriteLine("Error in InTriD");
                return ' ';
            }
            if ((area0 == 0) && (area1 == 0) ||
                 (area0 == 0) && (area2 == 0) ||
                 (area1 == 0) && (area2 == 0))
                return 'V';

            else
                return '0';
        }

        public int TriangleSign(tPointi a, tPointi b, tPointi c)
        {
            float area2;

            area2 = (b.p[0] - a.p[0]) * (float)(c.p[1] - a.p[1]) -
                    (c.p[0] - a.p[0]) * (float)(b.p[1] - a.p[1]);

            /* The area should be an integer. */
            if (area2 > 0.5) return 1;
            else if (area2 < -0.5) return -1;
            else return 0;
        }

        public char SegTriInt(tPointi T, tPointi q, tPointi r, tPointd p)
        {
            char code = '?';
            m = -1;

            code = SegPlaneInt(T, q, r, p, m);
            System.Diagnostics.Debug.WriteLine("****M is now after segplaneint: " + m);
            System.Diagnostics.Debug.WriteLine("SegPlaneInt code= " + code + " , m= " + m + "; p=()" + p.p[Xindex] + " , " + p.p[Yindex] + " , " + p.p[Zindex]);

            if (code == '0')
                return '0';
            else if (code == 'q')
                return InTri3D(T, m, q);
            else if (code == 'r')
                return InTri3D(T, m, r);
            else if (code == 'p')
                return InPlane(T, m, q, r, p);
            else if (code == '1')
                return SegTriCross(T, q, r);
            else /* Error */
                return code;
        }

        public char InPlane(tPointi T, int m, tPointi q, tPointi r, tPointd p)
        {
            /* NOT IMPLEMENTED */
            return 'p';
        }

        /*---------------------------------------------------------------------
        The signed volumes of three tetrahedra are computed, determined
        by the segment qr, and each edge of the triangle.  
        Returns a char:
           'v': the open segment includes a vertex of T.
           'e': the open segment includes a point in the relative interior of an edge
           of T.
           'f': the open segment includes a point in the relative interior of a face
           of T.
           '0': the open segment does not intersect triangle T.
        ---------------------------------------------------------------------*/

        public char SegTriCross(tPointi T, tPointi q, tPointi r)
        {
            int vol0, vol1, vol2;

            vol0 = VolumeSign(q, Vertices[T.p[0]], Vertices[T.p[1]], r);
            vol1 = VolumeSign(q, Vertices[T.p[1]], Vertices[T.p[2]], r);
            vol2 = VolumeSign(q, Vertices[T.p[2]], Vertices[T.p[0]], r);

            System.Diagnostics.Debug.WriteLine("SegTriCross:  vol0 = " + vol0 + " vol1 = " + vol1 + " vol2 = " + vol2);

            /* Same sign: segment intersects interior of triangle. */
            if (((vol0 > 0) && (vol1 > 0) && (vol2 > 0)) ||
                 ((vol0 < 0) && (vol1 < 0) && (vol2 < 0)))
                return 'f';

            /* Opposite sign: no intersection between segment and triangle */
            if (((vol0 > 0) || (vol1 > 0) || (vol2 > 0)) &&
                 ((vol0 < 0) || (vol1 < 0) || (vol2 < 0)))
                return '0';

            else if ((vol0 == 0) && (vol1 == 0) && (vol2 == 0))
            {
                System.Diagnostics.Debug.WriteLine("Error 1 in SegTriCross");
                return ('b');
            }

            /* Two zeros: segment intersects vertex. */
            else if (((vol0 == 0) && (vol1 == 0)) ||
                      ((vol0 == 0) && (vol2 == 0)) ||
                      ((vol1 == 0) && (vol2 == 0)))
                return 'v';

            /* One zero: segment intersects edge. */
            else if ((vol0 == 0) || (vol1 == 0) || (vol2 == 0))
                return 'e';

            else
            {
                System.Diagnostics.Debug.WriteLine("Error 2 in SegTriCross ");
                return ('b');
            }
        }

        public int VolumeSign(tPointi a, tPointi b, tPointi c, tPointi d)
        {
            float vol;
            float ax, ay, az, bx, by, bz, cx, cy, cz, dx, dy, dz;
            float bxdx, bydy, bzdz, cxdx, cydy, czdz;

            ax = a.p[Xindex];
            ay = a.p[Yindex];
            az = a.p[Zindex];
            bx = b.p[Xindex];
            by = b.p[Yindex];
            bz = b.p[Zindex];
            cx = c.p[Xindex];
            cy = c.p[Yindex];
            cz = c.p[Zindex];
            dx = d.p[Xindex];
            dy = d.p[Yindex];
            dz = d.p[Zindex];

            bxdx = bx - dx;
            bydy = by - dy;
            bzdz = bz - dz;
            cxdx = cx - dx;
            cydy = cy - dy;
            czdz = cz - dz;
            vol = (az - dz) * (bxdx * cydy - bydy * cxdx)
                  + (ay - dy) * (bzdz * cxdx - bxdx * czdz)
                  + (ax - dx) * (bydy * czdz - bzdz * cydy);


            /* The volume should be an integer. */
            if (vol > 0.5) return 1;
            else if (vol < -0.5) return -1;
            else return 0;
        }

        /*
          This function returns a char:
            '0': the segment [ab] does not intersect (completely misses) the 
                 bounding box surrounding the n-th triangle T.  It lies
                 strictly to one side of one of the six supporting planes.
            '?': status unknown: the segment may or may not intersect T.
        */

        public char BoxTest(int n, tPointi a, tPointi b)
        {
            int i; /* Coordinate index */
            int w;

            for (i = 0; i < DIM; i++)
            {
                w = Box[n][0].p[i]; /* min: lower left */
                if ((a.p[i] < w) && (b.p[i] < w)) return '0';
                w = Box[n][1].p[i]; /* max: upper right */
                if ((a.p[i] > w) && (b.p[i] > w)) return '0';
            }
            return '?';
        }

        //*********************************************
    }//end class 

    public class tPointi
    {

        public int[] p;

        public tPointi()
        {
            p = new int[3];
            p[0] = p[1] = p[2] = 0;
        }

        public tPointi(int x, int y, int z)
        {
            p = new int[3];
            p[0] = x;
            p[1] = y;
            p[2] = z;
        }

    }

    public class tPointd
    {

        public float[] p;

        public tPointd()
        {
            p = new float[3];
            p[0] = p[1] = p[2] = 0.0f;
        }

        public tPointd(float x, float y, float z)
        {
            p = new float[3];
            p[0] = x;
            p[1] = y;
            p[2] = z;
        }

    }

}