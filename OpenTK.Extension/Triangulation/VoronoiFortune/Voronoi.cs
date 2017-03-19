/*
 * Created by SharpDevelop.
 * User: Burhan
 * Date: 17/06/2014
 * Time: 11:30 م
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
 
 /*
 * The author of this software is Steven Fortune.  Copyright (c) 1994 by AT&T
 * Bell Laboratories.
 * Permission to use, copy, modify, and distribute this software for any
 * purpose without fee is hereby granted, provided that this entire notice
 * is included in all copies of any software which is or includes a copy
 * or modification of this software and in all copies of the supporting
 * documentation for such software.
 * THIS SOFTWARE IS BEING PROVIDED "AS IS", WITHOUT ANY EXPRESS OR IMPLIED
 * WARRANTY.  IN PARTICULAR, NEITHER THE AUTHORS NOR AT&T MAKE ANY
 * REPRESENTATION OR WARRANTY OF ANY KIND CONCERNING THE MERCHANTABILITY
 * OF THIS SOFTWARE OR ITS FITNESS FOR ANY PARTICULAR PURPOSE.
 */

/* 
 * This code was originally written by Stephan Fortune in C code.  I, Shane O'Sullivan,
 * have since modified it, encapsulating it in a C++ class and, fixing memory leaks and
 * adding accessors to the Voronoi Edges.
 * Permission to use, copy, modify, and distribute this software for any
 * purpose without fee is hereby granted, provided that this entire notice
 * is included in all copies of any software which is or includes a copy
 * or modification of this software and in all copies of the supporting
 * documentation for such software.
 * THIS SOFTWARE IS BEING PROVIDED "AS IS", WITHOUT ANY EXPRESS OR IMPLIED
 * WARRANTY.  IN PARTICULAR, NEITHER THE AUTHORS NOR AT&T MAKE ANY
 * REPRESENTATION OR WARRANTY OF ANY KIND CONCERNING THE MERCHANTABILITY
 * OF THIS SOFTWARE OR ITS FITNESS FOR ANY PARTICULAR PURPOSE.
 */

/* 
 * Java Version by Zhenyu Pan
 * Permission to use, copy, modify, and distribute this software for any
 * purpose without fee is hereby granted, provided that this entire notice
 * is included in all copies of any software which is or includes a copy
 * or modification of this software and in all copies of the supporting
 * documentation for such software.
 * THIS SOFTWARE IS BEING PROVIDED "AS IS", WITHOUT ANY EXPRESS OR IMPLIED
 * WARRANTY.  IN PARTICULAR, NEITHER THE AUTHORS NOR AT&T MAKE ANY
 * REPRESENTATION OR WARRANTY OF ANY KIND CONCERNING THE MERCHANTABILITY
 * OF THIS SOFTWARE OR ITS FITNESS FOR ANY PARTICULAR PURPOSE.
 */
 
 /*
 * C# Version by Burhan Joukhadar
 * 
 * Permission to use, copy, modify, and distribute this software for any
 * purpose without fee is hereby granted, provided that this entire notice
 * is included in all copies of any software which is or includes a copy
 * or modification of this software and in all copies of the supporting
 * documentation for such software.
 * THIS SOFTWARE IS BEING PROVIDED "AS IS", WITHOUT ANY EXPRESS OR IMPLIED
 * WARRANTY.  IN PARTICULAR, NEITHER THE AUTHORS NOR AT&T MAKE ANY
 * REPRESENTATION OR WARRANTY OF ANY KIND CONCERNING THE MERCHANTABILITY
 * OF THIS SOFTWARE OR ITS FITNESS FOR ANY PARTICULAR PURPOSE.
 */
 
using System;
using System.Collections.Generic;
using OpenTK;
using OpenTKExtension;

namespace VoronoiFortune
{
    /// <summary>
    /// Description of Voronoi.
    /// </summary>
    public class Voronoi
    {
        // ************* Private members ******************
        List<VertexKDTree> vectors;
        List<EdgeFortune> allEdges;
        Halfedge[] PQhash;
        Halfedge[] ELhash;

        double borderMinX, borderMaxX, borderMinY, borderMaxY;
        int siteidx;
        double xmin, xmax, ymin, ymax, deltax, deltay;
        int numberPoints;
        int nedges;
        int numberPointsStart;
        
        VertexKDTree bottomsite;
        int sqrt_nsites;
        double minDistanceBetweenVectorWithIndexs;
        int PQcount;
        int PQmin;
        int PQhashsize;

        

        const int LE = 0;
        const int RE = 1;

        int ELhashsize;
        
        Halfedge ELleftend, ELrightend;
        


        // ************* Public methods ******************
        // ******************************************

        // constructor
        public Voronoi(double minDistanceBetweenVectorWithIndexs)
        {
            siteidx = 0;
            vectors = null;

            allEdges = null;
            this.minDistanceBetweenVectorWithIndexs = minDistanceBetweenVectorWithIndexs;
        }

        /// <summary>
        /// Generate a Voronoi List of Edges
        /// </summary>
        /// <param name="mypoints"></param>
        /// <param name="minX: The minimum X of the bounding box around the voronoi"></param>
        /// <param name="maxX: The maximum X of the bounding box around the voronoi"></param>
        /// <param name="minY: The minimum Y of the bounding box around the voronoi"></param>
        /// <param name="maxY: The maximum Y of the bounding box around the voronoi"></param>
        /// <returns></returns>
        public List<EdgeFortune> GenerateVoronoi(List<VertexKDTree> mypoints ) 
        {
            this.vectors = mypoints;

            VertexKDTree max = new VertexKDTree();
            VertexKDTree min = new VertexKDTree();

            CalculateMinMax(ref max, ref min);


            
            sort();

            //SwapValues(ref minX, ref maxX, ref minY, ref maxY);

            borderMinX = min.Vector.X;
            borderMinY = min.Vector.Y;
            borderMaxX = max.Vector.X;
            borderMaxY = max.Vector.Y;

            siteidx = 0;
            calculateVoronoi();
            return allEdges;
        }
        private void CalculateMinMax(ref VertexKDTree max, ref VertexKDTree min)
        {

            if (vectors.Count < 1)
                return;

            float xMax = vectors[0].Vector.X;
            float yMax = vectors[0].Vector.Y;
            float xMin = vectors[0].Vector.X;
            float yMin = vectors[0].Vector.Y;

            foreach (VertexKDTree ver in vectors)
            {
                if (ver.Vector.X > xMax)
                    xMax = ver.Vector.X;
                if (ver.Vector.Y > yMax)
                    yMax = ver.Vector.Y;

                if (ver.Vector.X < xMin)
                    xMin = ver.Vector.X;
                if (ver.Vector.Y < yMin)
                    yMin = ver.Vector.Y;
               
            }
            max.Vector.X = xMax;
            max.Vector.Y = yMax;
           
            min.Vector.X = xMin;
            min.Vector.Y = yMin;
           
        }
        private void SwapValues(ref float minX, ref float maxX, ref float minY, ref float maxY)
        {
            // Check bounding box inputs - if mins are bigger than maxes, swap them
            float temp = 0;
            if (minX > maxX)
            {
                temp = minX;
                minX = maxX;
                maxX = temp;
            }
            if (minY > maxY)
            {
                temp = minY;
                minY = maxY;
                maxY = temp;
            }


        }

        /*********************************************************
         * Private methods - implementation details
         ********************************************************/

        private void sort()
        {

            allEdges = new List<EdgeFortune>();

            //int count = points.Count;
            numberPoints = 0;
            nedges = 0;

            double sn = (double)numberPointsStart + 4;
            sqrt_nsites = (int)Math.Sqrt(sn);

            
            sortNode();
        }


        private void sortNode()
        {
            numberPointsStart = vectors.Count;
            xmin = vectors[0].Vector.X;
            ymin = vectors[0].Vector.Y;
            xmax = vectors[0].Vector.X;
            ymax = vectors[0].Vector.Y;

            for (int i = 0; i < numberPointsStart; i++)
            {
                VertexKDTree p = vectors[i];

                if (p.Vector.X < xmin)
                    xmin = p.Vector.X;
                else if (p.Vector.X > xmax)
                    xmax = p.Vector.X;

                if (p.Vector.Y < ymin)
                    ymin = p.Vector.Y;
                else if (p.Vector.Y > ymax)
                    ymax = p.Vector.Y;
            }

            vectors.Sort(new VectorWithIndexSorterYX());

            deltax = xmax - xmin;
            deltay = ymax - ymin;
        }

        private VertexKDTree nextone()
        {
            VertexKDTree s;
            if (siteidx < numberPointsStart)
            {
                s = vectors[siteidx];
                siteidx++;
                return s;
            }
            return VertexKDTree.Zero;
        }

        private EdgeF bisect(VertexKDTree s1, VertexKDTree s2)
        {
            float dx, dy, adx, ady;
            EdgeF newedge;

            newedge = new EdgeF();

            newedge.reg[0] = s1;
            newedge.reg[1] = s2;

            newedge.ep[0] = VertexKDTree.Zero;
            newedge.ep[1] = VertexKDTree.Zero;

            dx = s2.Vector.X - s1.Vector.X;
            dy = s2.Vector.Y - s1.Vector.Y;

            adx = dx > 0 ? dx : -dx;
            ady = dy > 0 ? dy : -dy;
            newedge.c = (float)(s1.Vector.X * dx + s1.Vector.Y * dy + (dx * dx + dy * dy) * 0.5);

            if (adx > ady)
            {
                newedge.a = 1.0f;
                newedge.b = dy / dx;
                newedge.c /= dx;
            }
            else
            {
                newedge.a = dx / dy;
                newedge.b = 1.0f;
                newedge.c /= dy;
            }

            newedge.edgenbr = nedges;
            nedges++;

            return newedge;
        }

        private void makevertex(VertexKDTree v)
        {
           
            v.Index = numberPoints;
            numberPoints++;
        }

        private bool PQinitialize()
        {
            PQcount = 0;
            PQmin = 0;
            PQhashsize = 4 * sqrt_nsites;
            PQhash = new Halfedge[PQhashsize];

            for (int i = 0; i < PQhashsize; i++)
            {
                PQhash[i] = new Halfedge();
            }
            return true;
        }

        private int PQbucket(Halfedge he)
        {
            int bucket;

            bucket = (int)((he.ystar - ymin) / deltay * PQhashsize);
            if (bucket < 0)
                bucket = 0;
            if (bucket >= PQhashsize)
                bucket = PQhashsize - 1;
            if (bucket < PQmin)
                PQmin = bucket;

            return bucket;
        }

        // push the HalfEdge into the ordered linked list of pointCloud
        private void PQinsert(Halfedge he, VertexKDTree v, float offset)
        {
            Halfedge last, next;

            he.vertex = v;
            he.ystar = (float)(v.Vector.Y + offset);
            last = PQhash[PQbucket(he)];

            while
                (
                    (next = last.PQnext) != null
                    &&
                    (he.ystar > next.ystar || (he.ystar == next.ystar && v.Vector.X > next.vertex.Vector.X))
                )
            {
                last = next;
            }

            he.PQnext = last.PQnext;
            last.PQnext = he;
            PQcount++;
        }

        // remove the HalfEdge from the list of pointCloud
        private void PQdelete(Halfedge he)
        {
            Halfedge last;

            if (he.vertex != null)
            {
                last = PQhash[PQbucket(he)];
                while (last.PQnext != he)
                {
                    last = last.PQnext;
                }

                last.PQnext = he.PQnext;
                PQcount--;
                he.vertex = VertexKDTree.Zero;
            }
        }

        private bool PQempty()
        {
            return (PQcount == 0);
        }

        private VertexKDTree PQ_min()
        {
            VertexKDTree answer = new VertexKDTree();

            while (PQhash[PQmin].PQnext == null)
            {
                PQmin++;
            }

            answer.Vector.X = PQhash[PQmin].PQnext.vertex.Vector.X;
            answer.Vector.Y = PQhash[PQmin].PQnext.ystar;
            return answer;
        }

        private Halfedge PQextractmin()
        {
            Halfedge curr;

            curr = PQhash[PQmin].PQnext;
            PQhash[PQmin].PQnext = curr.PQnext;
            PQcount--;

            return curr;
        }

        private Halfedge HEcreate(EdgeF e, int pm)
        {
            Halfedge answer = new Halfedge();
            answer.ELedge = e;
            answer.ELpm = pm;
            answer.PQnext = null;
            answer.vertex = VertexKDTree.Zero;

            return answer;
        }

        private bool ELinitialize()
        {
            ELhashsize = 2 * sqrt_nsites;
            ELhash = new Halfedge[ELhashsize];

            for (int i = 0; i < ELhashsize; i++)
            {
                ELhash[i] = null;
            }

            ELleftend = HEcreate(null, 0);
            ELrightend = HEcreate(null, 0);
            ELleftend.ELleft = null;
            ELleftend.ELright = ELrightend;
            ELrightend.ELleft = ELleftend;
            ELrightend.ELright = null;
            ELhash[0] = ELleftend;
            ELhash[ELhashsize - 1] = ELrightend;

            return true;
        }

        private Halfedge ELright(Halfedge he)
        {
            return he.ELright;
        }

        private Halfedge ELleft(Halfedge he)
        {
            return he.ELleft;
        }

        private VertexKDTree leftreg(Halfedge he)
        {
            if (he.ELedge == null)
            {
                return bottomsite;
            }
            return (he.ELpm == LE ? he.ELedge.reg[LE] : he.ELedge.reg[RE]);
        }

        private void ELinsert(Halfedge lb, Halfedge newHe)
        {
            newHe.ELleft = lb;
            newHe.ELright = lb.ELright;
            (lb.ELright).ELleft = newHe;
            lb.ELright = newHe;
        }

        /*
         * This delete routine can't reclaim node, since pointers from hash table
         * may be present.
         */
        private void ELdelete(Halfedge he)
        {
            (he.ELleft).ELright = he.ELright;
            (he.ELright).ELleft = he.ELleft;
            he.deleted = true;
        }

        /* Get entry from hash table, pruning any deleted nodes */
        private Halfedge ELgethash(int b)
        {
            Halfedge he;
            if (b < 0 || b >= ELhashsize)
                return null;

            he = ELhash[b];
            if (he == null || !he.deleted)
                return he;

            /* Hash table points to deleted half edge. Patch as necessary. */
            ELhash[b] = null;
            return null;
        }

        private Halfedge ELleftbnd(VertexKDTree p)
        {
            int bucket;
            Halfedge he;

            /* Use hash table to get close to desired halfedge */
            // use the hash function to find the place in the hash map that this
            // HalfEdge should be
            bucket = (int)((p.Vector.X - xmin) / deltax * ELhashsize);

            // make sure that the bucket position is within the range of the hash
            // array
            if (bucket < 0) bucket = 0;
            if (bucket >= ELhashsize) bucket = ELhashsize - 1;

            he = ELgethash(bucket);

            // if the HE isn't found, search backwards and forwards in the hash map
            // for the first non-null entry
            if (he == null)
            {
                for (int i = 1; i < ELhashsize; i++)
                {
                    if ((he = ELgethash(bucket - i)) != null)
                        break;
                    if ((he = ELgethash(bucket + i)) != null)
                        break;
                }
            }

            /* Now search linear list of halfedges for the correct one */
            if (he == ELleftend || (he != ELrightend && right_of(he, p)))
            {
                // keep going right on the list until either the end is reached, or
                // you find the 1st edge which the point isn't to the right of
                do
                {
                    he = he.ELright;
                }
                while (he != ELrightend && right_of(he, p));
                he = he.ELleft;
            }
            else
            // if the point is to the left of the HalfEdge, then search left for
            // the HE just to the left of the point
            {
                do
                {
                    he = he.ELleft;
                }
                while (he != ELleftend && !right_of(he, p));
            }

            /* Update hash table and reference counts */
            if (bucket > 0 && bucket < ELhashsize - 1)
            {
                ELhash[bucket] = he;
            }

            return he;
        }

        private void pushGraphEdge(VertexKDTree leftVectorWithIndex, VertexKDTree rightVectorWithIndex, double x1, double y1, double x2, double y2)
        {
            EdgeFortune newEdge = new EdgeFortune();
            allEdges.Add(newEdge);
            newEdge.x1 = x1;
            newEdge.y1 = y1;
            newEdge.x2 = x2;
            newEdge.y2 = y2;

            //EDGAR INDEX
            //newEdge.PointIndex1 = leftVectorWithIndex.Index;
            //newEdge.PointIndex2 = rightVectorWithIndex.Index;
        }

        private void clip_line(EdgeF e)
        {
            double pxmin, pxmax, pymin, pymax;
            VertexKDTree s1, s2;

            double x1 = e.reg[0].Vector.X;
            double y1 = e.reg[0].Vector.Y;
            double x2 = e.reg[1].Vector.X;
            double y2 = e.reg[1].Vector.Y;
            double x = x2 - x1;
            double y = y2 - y1;

            // if the distance between the two points this line was created from is
            // less than the square root of 2, then ignore it
            if (Math.Sqrt((x * x) + (y * y)) < minDistanceBetweenVectorWithIndexs)
            {
                return;
            }
            pxmin = borderMinX;
            pymin = borderMinY;
            pxmax = borderMaxX;
            pymax = borderMaxY;

            if (e.a == 1&& e.b >= 0)
            {
                s1 = e.ep[1];
                s2 = e.ep[0];
            }
            else
            {
                s1 = e.ep[0];
                s2 = e.ep[1];
            }

            if (e.a == 1)
            {
                y1 = pymin;

                if (s1 != null && s1.Vector.Y > pymin)
                    y1 = s1.Vector.Y;
                if (y1 > pymax)
                    y1 = pymax;
                x1 = e.c - e.b * y1;
                y2 = pymax;

                if (s2 != null && s2.Vector.Y < pymax)
                    y2 = s2.Vector.Y;
                if (y2 < pymin)
                    y2 = pymin;
                x2 = e.c - e.b * y2;
                if (((x1 > pxmax) & (x2 > pxmax)) | ((x1 < pxmin) & (x2 < pxmin)))
                    return;

                if (x1 > pxmax)
                {
                    x1 = pxmax;
                    y1 = (e.c - x1) / e.b;
                }
                if (x1 < pxmin)
                {
                    x1 = pxmin;
                    y1 = (e.c - x1) / e.b;
                }
                if (x2 > pxmax)
                {
                    x2 = pxmax;
                    y2 = (e.c - x2) / e.b;
                }
                if (x2 < pxmin)
                {
                    x2 = pxmin;
                    y2 = (e.c - x2) / e.b;
                }

            }
            else
            {
                x1 = pxmin;
                if (s1 != null && s1.Vector.X > pxmin)
                    x1 = s1.Vector.X;
                if (x1 > pxmax)
                    x1 = pxmax;
                y1 = e.c - e.a * x1;

                x2 = pxmax;
                if (s2 != null && s2.Vector.X < pxmax)
                    x2 = s2.Vector.X;
                if (x2 < pxmin)
                    x2 = pxmin;
                y2 = e.c - e.a * x2;

                if (((y1 > pymax) & (y2 > pymax)) | ((y1 < pymin) & (y2 < pymin)))
                    return;

                if (y1 > pymax)
                {
                    y1 = pymax;
                    x1 = (e.c - y1) / e.a;
                }
                if (y1 < pymin)
                {
                    y1 = pymin;
                    x1 = (e.c - y1) / e.a;
                }
                if (y2 > pymax)
                {
                    y2 = pymax;
                    x2 = (e.c - y2) / e.a;
                }
                if (y2 < pymin)
                {
                    y2 = pymin;
                    x2 = (e.c - y2) / e.a;
                }
            }

            pushGraphEdge(e.reg[0], e.reg[1], x1, y1, x2, y2);
        }

        private void endpoint(EdgeF e, int lr, VertexKDTree s)
        {
            e.ep[lr] = s;
            if (e.ep[RE - lr] == null)
                return;
            clip_line(e);
        }

        /* returns true if p is to right of halfedge e */
        private bool right_of(Halfedge el, VertexKDTree p)
        {
            EdgeF e;
            VertexKDTree topsite;
            bool right_of_site;
            bool above, fast;
            double dxp, dyp, dxs, t1, t2, t3, yl;

            e = el.ELedge;
            topsite = e.reg[1];

            if (p.Vector.X > topsite.Vector.X)
                right_of_site = true;
            else
                right_of_site = false;

            if (right_of_site && el.ELpm == LE)
                return true;
            if (!right_of_site && el.ELpm == RE)
                return false;

            if (e.a == 1)
            {
                dxp = p.Vector.X - topsite.Vector.X;
                dyp = p.Vector.Y - topsite.Vector.Y;
                fast = false;

                if ((!right_of_site & (e.b < 0)) | (right_of_site & (e.b >= 0)))
                {
                    above = dyp >= e.b * dxp;
                    fast = above;
                }
                else
                {
                    above = p.Vector.X + p.Vector.Y * e.b > e.c;
                    if (e.b < 0)
                        above = !above;
                    if (!above)
                        fast = true;
                }
                if (!fast)
                {
                    dxs = topsite.Vector.X - (e.reg[0]).Vector.X;
                    above = e.b * (dxp * dxp - dyp * dyp)
                    < dxs * dyp * (1+ 2* dxp / dxs + e.b * e.b);

                    if (e.b < 0)
                        above = !above;
                }
            }
            else // e.b == 1.0
            {
                yl = e.c - e.a * p.Vector.X;
                t1 = p.Vector.Y - yl;
                t2 = p.Vector.X - topsite.Vector.X;
                t3 = yl - topsite.Vector.Y;
                above = t1 * t1 > t2 * t2 + t3 * t3;
            }
            return (el.ELpm == LE ? above : !above);
        }

        private VertexKDTree rightreg(Halfedge he)
        {
            if (he.ELedge == (EdgeF)null)
            // if this halfedge has no edge, return the bottom site (whatever
            // that is)
            {
                return (bottomsite);
            }

            // if the ELpm field is zero, return the site 0 that this edge bisects,
            // otherwise return site number 1
            return (he.ELpm == LE ? he.ELedge.reg[RE] : he.ELedge.reg[LE]);
        }

        private float dist(VertexKDTree s, VertexKDTree t)
        {
            float dx, dy;
            dx = s.Vector.X - t.Vector.X;
            dy = s.Vector.Y - t.Vector.Y;
            return Convert.ToSingle( Math.Sqrt(dx * dx + dy * dy));
        }

        // create a new site where the HalfEdges el1 and el2 intersect - note that
        // the Point in the argument list is not used, don't know why it's there
        private VertexKDTree intersect(Halfedge el1, Halfedge el2)
        {
            EdgeF e1, e2, e;
            Halfedge el;
            float d, xint, yint;
            bool right_of_site;
            VertexKDTree v; // vertex

            e1 = el1.ELedge;
            e2 = el2.ELedge;

            if (e1 == null || e2 == null)
                return VertexKDTree.Zero; 

            // if the two edges bisect the same parent, return null
            if (e1.reg[1] == e2.reg[1])
                return VertexKDTree.Zero; 

            d = e1.a * e2.b - e1.b * e2.a;
            if (-1.0e-10 < d && d < 1.0e-10)
                return VertexKDTree.Zero; 

            xint = (e1.c * e2.b - e2.c * e1.b) / d;
            yint = (e2.c * e1.a - e1.c * e2.a) / d;

            if ((e1.reg[1].Vector.Y < e2.reg[1].Vector.Y)
                || (e1.reg[1].Vector.Y == e2.reg[1].Vector.Y && e1.reg[1].Vector.X < e2.reg[1].Vector.X))
            {
                el = el1;
                e = e1;
            }
            else
            {
                el = el2;
                e = e2;
            }

            right_of_site = xint >= e.reg[1].Vector.X;
            if ((right_of_site && el.ELpm == LE)
                || (!right_of_site && el.ELpm == RE))
                return VertexKDTree.Zero; 

            // create a new site at the point of intersection - this is a new vector
            // event waiting to happen
            v = new VertexKDTree();
            v.Vector.X = xint;
            v.Vector.Y = yint;
            return v;
        }

        /*
         * implicit parameters: nsites, sqrt_nsites, xmin, xmax, ymin, ymax, deltax,
         * deltay (can all be estimates). Performance suffers if they are wrong;
         * better to make nsites, deltax, and deltay too big than too small. (?)
         */
        private bool calculateVoronoi()
        {
            VertexKDTree newsite, bot, top, temp, p;
            VertexKDTree v;
            VertexKDTree newintstar = VertexKDTree.Zero;
            int pm;
            Halfedge leftEdge, rightEdge, llHalfedge, rrHalfedge, bisector;
            EdgeF e;

            PQinitialize();
            ELinitialize();

            bottomsite = nextone();
            newsite = nextone();
            while (true)
            {
                if (!PQempty())
                {
                    newintstar = PQ_min();
                }
                // if the lowest site has a smaller y value than the lowest vector
                // intersection,
                // process the site otherwise process the vector intersection

                if (newsite != null && (PQempty()
                                        || newsite.Vector.Y < newintstar.Vector.Y
                                        || (newsite.Vector.Y == newintstar.Vector.Y
                                            && newsite.Vector.X < newintstar.Vector.X)))
                {
                    /* new site is smallest -this is a site event */
                    // get the first HalfEdge to the LEFT of the new site
                    leftEdge = ELleftbnd((newsite));
                    // get the first HalfEdge to the RIGHT of the new site
                    rightEdge = ELright(leftEdge);
                    // if this halfedge has no edge,bot =bottom site (whatever that
                    // is)
                    bot = rightreg(leftEdge);
                    // create a new edge that bisects
                    e = bisect(bot, newsite);

                    // create a new HalfEdge, setting its ELpm field to 0
                    bisector = HEcreate(e, LE);
                    // insert this new bisector edge between the left and right
                    // vectors in a linked list
                    ELinsert(leftEdge, bisector);

                    // if the new bisector intersects with the left edge,
                    // remove the left edge's vertex, and put in the new one
                    if ((p = intersect(leftEdge, bisector)) != null)
                    {
                        PQdelete(leftEdge);
                        PQinsert(leftEdge, p, dist(p, newsite));
                    }
                    leftEdge = bisector;
                    // create a new HalfEdge, setting its ELpm field to 1
                    bisector = HEcreate(e, RE);
                    // insert the new HE to the right of the original bisector
                    // earlier in the IF stmt
                    ELinsert(leftEdge, bisector);

                    // if this new bisector intersects with the new HalfEdge
                    if ((p = intersect(bisector, rightEdge)) != null)
                    {
                        // push the HE into the ordered linked list of pointCloud
                        PQinsert(bisector, p, dist(p, newsite));
                    }
                    newsite = nextone();
                }
                else if (!PQempty())
                /* intersection is smallest - this is a vector event */
                {
                    // pop the HalfEdge with the lowest vector off the ordered list
                    // of vectors
                    leftEdge = PQextractmin();
                    // get the HalfEdge to the left of the above HE
                    llHalfedge = ELleft(leftEdge);
                    // get the HalfEdge to the right of the above HE
                    rightEdge = ELright(leftEdge);
                    // get the HalfEdge to the right of the HE to the right of the
                    // lowest HE
                    rrHalfedge = ELright(rightEdge);
                    // get the VectorWithIndex to the left of the left HE which it bisects
                    bot = leftreg(leftEdge);
                    // get the VectorWithIndex to the right of the right HE which it bisects
                    top = rightreg(rightEdge);

                    v = leftEdge.vertex; // get the vertex that caused this event
                    makevertex(v); // set the vertex number - couldn't do this
                    // earlier since we didn't know when it would be processed
                    endpoint(leftEdge.ELedge, leftEdge.ELpm, v);
                    // set the endpoint of
                    // the left HalfEdge to be this vector
                    endpoint(rightEdge.ELedge, rightEdge.ELpm, v);
                    // set the endpoint of the right HalfEdge to
                    // be this vector
                    ELdelete(leftEdge); // mark the lowest HE for
                    // deletion - can't delete yet because there might be pointers
                    // to it in Hash Map
                    PQdelete(rightEdge);
                    // remove all vertex events to do with the right HE
                    ELdelete(rightEdge); // mark the right HE for
                    // deletion - can't delete yet because there might be pointers
                    // to it in Hash Map
                    pm = LE; // set the pm variable to zero

                    if (bot.Vector.Y > top.Vector.Y)
                    // if the site to the left of the event is higher than the
                    // VectorWithIndex
                    { // to the right of it, then swap them and set the 'pm'
                        // variable to 1
                        temp = bot;
                        bot = top;
                        top = temp;
                        pm = RE;
                    }
                    e = bisect(bot, top); // create an Edge (or line)
                    // that is between the two VectorWithIndexs. This creates the formula of
                    // the line, and assigns a line number to it
                    bisector = HEcreate(e, pm); // create a HE from the Edge 'e',
                    // and make it point to that edge
                    // with its ELedge field
                    ELinsert(llHalfedge, bisector); // insert the new bisector to the
                    // right of the left HE
                    endpoint(e, RE - pm, v); // set one endpoint to the new edge
                    // to be the vector point 'v'.
                    // If the site to the left of this bisector is higher than the
                    // right VectorWithIndex, then this endpoint
                    // is put in position 0; otherwise in pos 1

                    // if left HE and the new bisector intersect, then delete
                    // the left HE, and reinsert it
                    if ((p = intersect(llHalfedge, bisector)) != null)
                    {
                        PQdelete(llHalfedge);
                        PQinsert(llHalfedge, p, dist(p, bot));
                    }

                    // if right HE and the new bisector intersect, then
                    // reinsert it
                    if ((p = intersect(bisector, rrHalfedge)) != null)
                    {
                        PQinsert(bisector, p, dist(p, bot));
                    }
                }
                else
                {
                    break;
                }
            }

            for (leftEdge = ELright(ELleftend); leftEdge != ELrightend; leftEdge = ELright(leftEdge))
            {
                e = leftEdge.ELedge;
                clip_line(e);
            }

            return true;
        }

    }
    //In the delaunay triangulation of a set of points, P, all triangles are "delaunay", meaning that there are no points inside the circumcircle corresponding to any of the triangles.

    //The voronoi tessellation for a set of points, P, consists of the set of voronoi cells R, such that for every point in Ri are closer to Pi then to any other point in P.


   
    
    //Voronoi -> Delaunay
    //1. Given the set of points P and the voronoi tessellation simply connect neighboring cells points. 
    //This is of course given that you know the set of point P used when constructing the voronoi tessellation.
    //2. If you have a voronoi diagram, all you need to do is:
    //Connect the points who share an edge and you'll have the delaunay triangulation (and vice versa).
    //3. Um die Delaunay-Triangulation zu berechnen, wird der entsprechende duale Graph zum Voronoi-Diagramm gebildet. 
    //Die Zentren der Polygone werden miteinander verbunden, so dass zu jeder Voronoi-Kante eine orthogonale Linie eingezeichnet wird, 
    //die die entsprechenden zwei Zentren miteinander verbindet (siehe Abbildung).


    //Delaunay -> Voronoi  
    // 1. Given the delaunay triangulation simply connect the neighboring triangles circumcircle centers.

    //2. Die Delaunay-Triangulierung ist der duale Graph des Voronoi-Diagramms der Punktemenge: 
    //Die Ecken der Voronoizellen sind die Umkreismittelpunkte der Dreiecke der Delaunay-Triangulation 
    //Man erhält die Voronoi-Zellen, wenn man von allen Dreieckseiten die Mittelsenkrechten bis zum gemeinsamen Schnittpunkt mit den anderen beiden
    //Mittelsenkrechten desselben Dreiecks einzeichnet; dieser Punkt kann bei stumpfwinkligen Dreiecken durchaus außerhalb der Dreiecksfläche liegen, 
    //bei rechtwinkligen Dreiecken ist es der Punkt, der die Hypotenuse halbiert).

}