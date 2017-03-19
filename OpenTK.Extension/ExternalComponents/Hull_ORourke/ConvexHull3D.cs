/*----------------------------------------------------------------------------
 * Class ConvexHull3D
 *
 * Application for computing convex hull of points in 3D.
 *---------------------------------------------------------------------------*/


using OpenTK;
using OpenTKExtension;
using System;

using System.Collections.Generic;

namespace OpenTKExtension
{

    public class ConvexHull3D
    {
        /* Define flags */
        protected static bool ONHULL = true;
        protected static bool REMOVED = true;
        protected static bool VISIBLE = true;
        protected static bool PROCESSED = true;
        protected static int SAFE = 1000000;

        //private bool debug;
        //private bool check;
        //public bool toDraw;
        public cVertexList Vertices;
        public cEdgeList Edges;
        public cFaceList Faces;

        
        public ConvexHull3D()
        {
            Vertices = new cVertexList();
            Edges = new cEdgeList();
            Faces = new cFaceList();
           
        }
        protected bool Hull()
        {

            ReadVertices_SetIndexInModel();

            if (floatTriangle())
            {
                ConstructHull();
                return true;
            }
            else
            {
               System.Windows.Forms.MessageBox.Show("Hull Failed");
                return false;
            }
           

        }
        protected void InitVectors(List<Vector3> myListVectors)
        {
            for (int i = 0; i < myListVectors.Count; i++)
            {
                Vector3 v = myListVectors[i];
                Vertices.SetVertex3D(v.X, v.Y, v.Z);

            }
        }
        public ConvexHull3D(List<Vector3> myListVectors):this()
        {


            InitVectors(myListVectors);
            Hull();

            //Print();
                
        }

        public void ReadVertices_SetIndexInModel()
        {
            cVertex v = Vertices.head;
            int vnum = -1;
            do
            {
                vnum++;
                v.IndexInModel = vnum;
                if ((Math.Abs(v.Point.X) > SAFE) || (Math.Abs(v.Point.Y) > SAFE)
          || (Math.Abs(v.Point.Z) > SAFE))
                {
                    System.Diagnostics.Debug.WriteLine("Coordinate of vertex below might be too large...");
                    v.PrintVertex3D(vnum);
                }
                v = v.NextVertex;
            } while (v != Vertices.head);
        }

      

        /*---------------------------------------------------------------------
          Print: Prints out the pointCloud and the faces.  Uses the vnum indices 
          corresponding to the order in which the pointCloud were input.
          ---------------------------------------------------------------------*/
        protected void Print()
        {
            /* Pointers to pointCloud, edges, faces. */
            cVertex v;
            cEdge e;
            cFace f;
            float xmin, ymin, xmax, ymax;
            float[] a = new float[3];
            float[] b = new float[3];  /*used to compute normal vector */
            /* Counters for Euler's formula. */
            int V = 0, E = 0, F = 0;
            /* Note: lowercase==pointer, uppercase==counter. */

            /*-- find X min & max --*/
            v = Vertices.head;
            xmin = xmax = v.Point.X;
            do
            {
                if (v.Point.X > xmax) xmax = v.Point.X;
                else
                    if (v.Point.X < xmin) xmin = v.Point.X;
                v = v.NextVertex;
            } while (v != Vertices.head);

            /*-- find Y min & max --*/
            v = Vertices.head;
            ymin = ymax = v.Point.Y;
            do
            {
                if (v.Point.Y > ymax) ymax = v.Point.Y;
                else
                    if (v.Point.Y < ymin) ymin = v.Point.Y;
                v = v.NextVertex;
            } while (v != Vertices.head);


            /* Vertices. */
            v = Vertices.head;
            do
            {
                if (v.IsProcessed) V++;
                v = v.NextVertex;
            } while (v != Vertices.head);

            System.Diagnostics.Debug.WriteLine("\nVertices:\tV = " + V);
            System.Diagnostics.Debug.WriteLine("index:\tx\ty\tz");
            do
            {
                System.Diagnostics.Debug.WriteLine(v.IndexInModel + ":\t" + v.Point.X + "\t" + v.Point.Y + "\t" + v.Point.Z + "");
                System.Diagnostics.Debug.WriteLine("newpath");
                System.Diagnostics.Debug.WriteLine(v.Point.X + "\t" + v.Point.Y + " 2 0 360 arc");
                System.Diagnostics.Debug.WriteLine("closepath stroke\n");
                v = v.NextVertex;
            } while (v != Vertices.head);

            /* Faces. */
            /* visible faces are printed as PS output */
            f = Faces.head;
            do
            {
                ++F;
                f = f.next;
            } while (f != Faces.head);
            System.Diagnostics.Debug.WriteLine("\nFaces:\tF = " + F);
            System.Diagnostics.Debug.WriteLine("Visible faces only:");
            do
            {
                /* Print face only if it is lower */
                if (f.lower)
                {
                    System.Diagnostics.Debug.WriteLine("vnums:  " + f.Vertices[0].IndexInModel + "  "
                               + f.Vertices[1].IndexInModel + "  " + f.Vertices[2].IndexInModel);
                    System.Diagnostics.Debug.WriteLine("newpath");
                    System.Diagnostics.Debug.WriteLine(f.Vertices[0].Point.X + "\t" + f.Vertices[0].Point.Y + "\tmoveto");
                    System.Diagnostics.Debug.WriteLine(f.Vertices[1].Point.X + "\t" + f.Vertices[1].Point.Y + "\tlineto");
                    System.Diagnostics.Debug.WriteLine(f.Vertices[2].Point.X + "\t" + f.Vertices[2].Point.Y + "\tlineto");
                    System.Diagnostics.Debug.WriteLine("\n");
                }
                f = f.next;
            } while (f != Faces.head);

            /* prints a list of all faces */
            System.Diagnostics.Debug.WriteLine("List of all faces:");
            System.Diagnostics.Debug.WriteLine("\tv0\tv1\tv2\t(vertex indices)");
            do
            {
                System.Diagnostics.Debug.WriteLine("\t" + f.Vertices[0].IndexInModel +
                       "\t" + f.Vertices[1].IndexInModel +
                       "\t" + f.Vertices[2].IndexInModel);
                f = f.next;
            } while (f != Faces.head);

            /* Edges. */
            e = Edges.head;
            do
            {
                E++;
                e = e.next;
            } while (e != Edges.head);
            System.Diagnostics.Debug.WriteLine("\nEdges:\tE = " + E);
            /* Edges not printed out (but easily added). */

            
            CheckEuler(V, E, F);
        }

        /*---------------------------------------------------------------------
          SubVec:  Computes a - b and puts it into c.
          ---------------------------------------------------------------------*/
        protected void SubVec(float[] a, float[] b, float[] c)
        {
            int i;

            for (i = 0; i < 2; i++)
                c[i] = a[i] - b[i];
        }

        /*---------------------------------------------------------------------
          floatTriangle builds the initial float triangle.  It first finds 3 
          noncollinear points and makes two faces out of them, in opposite order.
          It then finds a fourth point that is not coplanar with that face.  The  
          pointCloud are stored in the face structure in counterclockwise order so 
          that the volume between the face and the point is negative. Lastly, the
          3 newfaces to the fourth point are constructed and the data structures
          are cleaned up. 
          ---------------------------------------------------------------------*/
        protected bool floatTriangle()
        {
            cVertex v0, v1, v2, v3;//, t;
            cFace f0, f1 = null;
            float vol;


            /* Find 3 non-Collinear points. */
            v0 = Vertices.head;
            while (Collinear(v0, v0.NextVertex, v0.NextVertex.NextVertex))
                if ((v0 = v0.NextVertex) == Vertices.head)
                {
                    System.Diagnostics.Debug.WriteLine("floatTriangle:  All points are Collinear!");
                    return false;
                }
            v1 = v0.NextVertex;
            v2 = v1.NextVertex;

            /* Mark the pointCloud as processed. */
            v0.IsProcessed = PROCESSED;
            v1.IsProcessed = PROCESSED;
            v2.IsProcessed = PROCESSED;

            /* Create the two "twin" faces. */
            f0 = MakeFace(v0, v1, v2, f1);
            f1 = MakeFace(v2, v1, v0, f0);

            /* Link adjacent face fields. */
            f0.Edges[0].Adjface[1] = f1;
            f0.Edges[1].Adjface[1] = f1;
            f0.Edges[2].Adjface[1] = f1;
            f1.Edges[0].Adjface[1] = f0;
            f1.Edges[1].Adjface[1] = f0;
            f1.Edges[2].Adjface[1] = f0;

            /* Find a fourth, non-coplanar point to form tetrahedron. */
            v3 = v2.NextVertex;
            vol = VolumeSign(f0, v3);
            while (vol == 0)
            {
                if ((v3 = v3.NextVertex) == v0)
                {
                    System.Diagnostics.Debug.WriteLine("floatTriangle:  All points are coplanar!");
                    return false;
                }
                vol = VolumeSign(f0, v3);
            }

            /* Insure that v3 will be the first added. */
            Vertices.head = v3;
           
            return true;
        }


        /*---------------------------------------------------------------------
          ConstructHull adds the pointCloud to the hull one at a time.  The hull
          pointCloud are those in the list marked as onhull.
          ---------------------------------------------------------------------*/
        protected void ConstructHull()
        {
            cVertex v, vnext;
           // float vol;
            bool changed;	/* T if addition changes hull; not used. */

            v = Vertices.head;
            do
            {
                vnext = v.NextVertex;
                if (!v.IsProcessed)
                {
                    v.IsProcessed = PROCESSED;
                    changed = AddOne(v);
                    CleanUp();

                  
                }
                v = vnext;
            } while (v != Vertices.head);
        }

        /*---------------------------------------------------------------------
          AddOne is passed a vertex.  It first determines all faces visible from 
          that point.  If none are visible then the point is marked as not 
          onhull.  Next is a loop over edges.  If both faces adjacent to an edge
          are visible, then the edge is marked for deletion.  If just one of the
          adjacent faces is visible then a new face is constructed.
          ---------------------------------------------------------------------*/

        protected bool AddOne(cVertex p)
        {
            cFace face;
            cEdge edge;
            float vol;
            bool vis = false;

        

            /* Mark faces visible from p. */
            face = Faces.head;
            do
            {
                vol = VolumeSign(face, p);
               
                if (vol < 0)
                {
                    face.visible = VISIBLE;
                    vis = true;
                }
                face = face.next;
            } while (face != Faces.head);

            /* If no faces are visible from p, then p is inside the hull. */
            if (!vis)
            {
                p.IsOnHull = !ONHULL;
                return false;
            }

            /* Mark edges in interior of visible region for deletion.
               Erect a newface based on each border edge. */
            edge = Edges.head;
            do
            {
                cEdge temp;
                temp = edge.next;
                if (edge.Adjface[0].visible && (edge.Adjface[1] != null && edge.Adjface[1].visible))
                    /* e interior: mark for deletion. */
                    edge.delete = REMOVED;
                else if (edge.Adjface[0].visible || (edge.Adjface[1] != null && edge.Adjface[1].visible))
                    /* e border: make a new face. */
                    edge.newface = MakeConeFace(edge, p);
                edge = temp;
            } while (edge != Edges.head);
            return true;
        }

        /*---------------------------------------------------------------------
          VolumeSign returns the sign of the volume of the tetrahedron determined 
          by f and p.  VolumeSign is +1 iff p is on the negative side of f,
          where the positive side is determined by the rh-rule.  So the volume 
          is positive if the ccw normal to f points outside the tetrahedron.
          The  fewer-multiplications form is due to Robert Fraczkiewicz.
          ---------------------------------------------------------------------*/
        protected float VolumeSign(cFace f, cVertex p)
        {
            float vol;
            
            float ax, ay, az, bx, by, bz, cx, cy, cz, dx, dy, dz;
            float bxdx, bydy, bzdz, cxdx, cydy, czdz;

            ax = f.Vertices[0].Point.X;
            ay = f.Vertices[0].Point.Y;
            az = f.Vertices[0].Point.Z;
            bx = f.Vertices[1].Point.X;
            by = f.Vertices[1].Point.Y;
            bz = f.Vertices[1].Point.Z;
            cx = f.Vertices[2].Point.X;
            cy = f.Vertices[2].Point.Y;
            cz = f.Vertices[2].Point.Z;
            dx = p.Point.X;
            dy = p.Point.Y;
            dz = p.Point.Z;

            bxdx = bx - dx;
            bydy = by - dy;
            bzdz = bz - dz;
            cxdx = cx - dx;
            cydy = cy - dy;
            czdz = cz - dz;
            vol = (az - dz) * (bxdx * cydy - bydy * cxdx)
              + (ay - dy) * (bzdz * cxdx - bxdx * czdz)
              + (ax - dx) * (bydy * czdz - bzdz * cydy);


            return vol;

            ///* The volume should be an integer. */
            //if (vol > 0.5) return 1;
            //else if (vol < -0.5) return -1;
            //else return 0;
        }
        /*---------------------------------------------------------------------*/
        protected float Volumei(cFace f, cVertex p)
        {
            float vol;
            float ax, ay, az, bx, by, bz, cx, cy, cz, dx, dy, dz;
            float bxdx, bydy, bzdz, cxdx, cydy, czdz;
            //float vold;
            //int i;

            ax = f.Vertices[0].Point.X;
            ay = f.Vertices[0].Point.Y;
            az = f.Vertices[0].Point.Z;
            bx = f.Vertices[1].Point.X;
            by = f.Vertices[1].Point.Y;
            bz = f.Vertices[1].Point.Z;
            cx = f.Vertices[2].Point.X;
            cy = f.Vertices[2].Point.Y;
            cz = f.Vertices[2].Point.Z;
            dx = p.Point.X;
            dy = p.Point.Y;
            dz = p.Point.Z;

            bxdx = bx - dx;
            bydy = by - dy;
            bzdz = bz - dz;
            cxdx = cx - dx;
            cydy = cy - dy;
            czdz = cz - dz;
            vol = (az - dz) * (bxdx * cydy - bydy * cxdx)
              + (ay - dy) * (bzdz * cxdx - bxdx * czdz)
              + (ax - dx) * (bydy * czdz - bzdz * cydy);

            return vol;
        }

        /*---------------------------------------------------------------------
          Volumed is the same as VolumeSign but computed with floats.  For 
          protection against overflow.
          ---------------------------------------------------------------------*/
        protected float Volumed(cFace f, cVertex p)
        {
            float vol;
            float ax, ay, az, bx, by, bz, cx, cy, cz, dx, dy, dz;
            float bxdx, bydy, bzdz, cxdx, cydy, czdz;

            ax = f.Vertices[0].Point.X;
            ay = f.Vertices[0].Point.Y;
            az = f.Vertices[0].Point.Z;
            bx = f.Vertices[1].Point.X;
            by = f.Vertices[1].Point.Y;
            bz = f.Vertices[1].Point.Z;
            cx = f.Vertices[2].Point.X;
            cy = f.Vertices[2].Point.Y;
            cz = f.Vertices[2].Point.Z;
            dx = p.Point.X;
            dy = p.Point.Y;
            dz = p.Point.Z;

            bxdx = bx - dx;
            bydy = by - dy;
            bzdz = bz - dz;
            cxdx = cx - dx;
            cydy = cy - dy;
            czdz = cz - dz;
            vol = (az - dz) * (bxdx * cydy - bydy * cxdx)
              + (ay - dy) * (bzdz * cxdx - bxdx * czdz)
              + (ax - dx) * (bydy * czdz - bzdz * cydy);

            return vol;
        }

        /*---------------------------------------------------------------------
          MakeConeFace makes a new face and two new edges between the 
          edge and the point that are passed to it. It returns a pointer to
          the new face.
          ---------------------------------------------------------------------*/
        protected cFace MakeConeFace(cEdge e, cVertex p)
        {
            cEdge[] new_edge = new cEdge[2];
            cFace new_face;
            int i, j;

            /* Make two new edges (if don't already exist). */
            for (i = 0; i < 2; ++i)
            {
                /* If the edge exists, copy it into new_edge. */
                new_edge[i] = e.Endpts[i].Edge;
                if (new_edge[i] == null)
                {
                    /* Otherwise (duplicate is null), MakeNullEdge. */
                    new_edge[i] = Edges.MakeNullEdge();
                    new_edge[i].Endpts[0] = e.Endpts[i];
                    new_edge[i].Endpts[1] = p;
                    e.Endpts[i].Edge = new_edge[i];
                }
            }

            /* Make the new face. */
            new_face = Faces.MakeNullFace();
            new_face.Edges[0] = e;
            new_face.Edges[1] = new_edge[0];
            new_face.Edges[2] = new_edge[1];
            MakeCcw(new_face, e, p);

            /* Set the adjacent face pointers. */
            for (i = 0; i < 2; ++i)
                for (j = 0; j < 2; ++j)
                    /* Only one NULL link should be set to new_face. */
                    if (new_edge[i].Adjface[j] == null)
                    {
                        new_edge[i].Adjface[j] = new_face;
                        break;
                    }

            return new_face;
        }

        /*---------------------------------------------------------------------
          MakeCcw puts the pointCloud in the face structure in counterclock wise 
          order.  We want to store the pointCloud in the same 
          order as in the visible face.  The third vertex is always p.
          ---------------------------------------------------------------------*/
        protected void MakeCcw(cFace f, cEdge e, cVertex p)
        {
            cFace fv;                  /* The visible face adjacent to e */
            int i;                   /* Index of e.endpoint[0] in fv. */
            cEdge s = new cEdge();     /* Temporary, for swapping */

            if (e.Adjface[0].visible)
                fv = e.Adjface[0];
            else fv = e.Adjface[1];

            /* Set vertex[0] & [1] of f to have the same orientation
               as do the corresponding pointCloud of fv. */
            for (i = 0; fv.Vertices[i] != e.Endpts[0]; ++i)
                ;
            /* Orient f the same as fv. */
            if (fv.Vertices[(i + 1) % 3] != e.Endpts[1])
            {
                f.Vertices[0] = e.Endpts[1];
                f.Vertices[1] = e.Endpts[0];
            }
            else
            {
                f.Vertices[0] = e.Endpts[0];
                f.Vertices[1] = e.Endpts[1];
                Swap(s, f.Edges[1], f.Edges[2]);
            }
            /* This swap is tricky. e is edge[0]. edge[1] is based on endpt[0],
               edge[2] on endpt[1].  So if e is oriented "forwards," we
               need to move edge[1] to follow [0], because it precedes. */

            f.Vertices[2] = p;
        }

        /*---------------------------------------------------------------------
          MakeFace creates a new face structure from three pointCloud (in ccw
          order).  It returns a pointer to the face.
          ---------------------------------------------------------------------*/
        protected cFace MakeFace(cVertex v0, cVertex v1, cVertex v2, cFace fold)
        {
            cFace f;
            cEdge e0, e1, e2;

            /* Create edges of the initial triangle. */
            if (fold == null)
            {
                e0 = Edges.MakeNullEdge();
                e1 = Edges.MakeNullEdge();
                e2 = Edges.MakeNullEdge();
            }
            else
            { /* Copy from fold, in reverse order. */
                e0 = fold.Edges[2];
                e1 = fold.Edges[1];
                e2 = fold.Edges[0];
            }
            e0.Endpts[0] = v0; e0.Endpts[1] = v1;
            e1.Endpts[0] = v1; e1.Endpts[1] = v2;
            e2.Endpts[0] = v2; e2.Endpts[1] = v0;

            /* Create face for triangle. */
            f = Faces.MakeNullFace();
            f.Edges[0] = e0; f.Edges[1] = e1; f.Edges[2] = e2;
            f.Vertices[0] = v0; f.Vertices[1] = v1; f.Vertices[2] = v2;

            /* Link edges to face. */
            e0.Adjface[0] = e1.Adjface[0] = e2.Adjface[0] = f;

            return f;
        }

        /*---------------------------------------------------------------------
          CleanUp goes through each data structure list and clears all
          flags and NULLs out some pointers.  The order of processing
          (edges, faces, pointCloud) is important.
          ---------------------------------------------------------------------*/
        protected void CleanUp()
        {
            CleanEdges();
            CleanFaces();
            CleanVertices();
        }

        /*---------------------------------------------------------------------
          CleanEdges runs through the edge list and cleans up the structure.
          If there is a newface then it will put that face in place of the 
          visible face and NULL out newface. It also deletes so marked edges.
          ---------------------------------------------------------------------*/
        protected void CleanEdges()
        {
            cEdge e;	/* Primary index into edge list. */
            cEdge t;	/* Temporary edge pointer. */

            /* Integrate the newface's into the data structure. */
            /* Check every edge. */
            e = Edges.head;
            do
            {
                if (e.newface != null)
                {
                    if (e.Adjface[0].visible)
                        e.Adjface[0] = e.newface;
                    else e.Adjface[1] = e.newface;
                    e.newface = null;
                }
                e = e.next;
            } while (e != Edges.head);

            /* Delete any edges marked for deletion. */
            while (Edges.head != null && Edges.head.delete)
            {
                e = Edges.head;
                Edges.Delete(e);
            }
            e = Edges.head.next;
            do
            {
                if (e.delete)
                {
                    t = e;
                    e = e.next;
                    Edges.Delete(t);
                }
                else e = e.next;
            } while (e != Edges.head);
        }

        /*---------------------------------------------------------------------
          CleanFaces runs through the face list and deletes any face marked visible.
          ---------------------------------------------------------------------*/
        protected void CleanFaces()
        {
            cFace f;	/* Primary pointer into face list. */
            cFace t;	/* Temporary pointer, for deleting. */


            while (Faces.head != null && Faces.head.visible)
            {
                f = Faces.head;
                Faces.Delete(f);
            }
            f = Faces.head.next;
            do
            {
                if (f.visible)
                {
                    t = f;
                    f = f.next;
                    Faces.Delete(t);
                }
                else f = f.next;
            } while (f != Faces.head);
        }

        /*---------------------------------------------------------------------
          CleanVertices runs through the vertex list and deletes the 
          pointCloud that are marked as processed but are not incident to any 
          undeleted edges. 
          ---------------------------------------------------------------------*/
        protected void CleanVertices()
        {
            cEdge e;
            cVertex v, t;

            /* Mark all pointCloud incident to some undeleted edge as on the hull. */
            e = Edges.head;
            do
            {
                e.Endpts[0].IsOnHull = e.Endpts[1].IsOnHull = ONHULL;
                e = e.next;
            } while (e != Edges.head);

            /* Delete all pointCloud that have been processed but
               are not on the hull. */
            while (Vertices.head != null && Vertices.head.IsProcessed && !Vertices.head.IsOnHull)
            {
                v = Vertices.head;
                Vertices.Delete(v);
            }
            v = Vertices.head.NextVertex;
            do
            {
                if (v.IsProcessed && !v.IsOnHull)
                {
                    t = v;
                    v = v.NextVertex;
                    Vertices.Delete(t);
                }
                else v = v.NextVertex;
            } while (v != Vertices.head);

            /* Reset flags. */
            v = Vertices.head;
            do
            {
                v.Edge = null;
                v.IsOnHull = !ONHULL;
                v = v.NextVertex;
            } while (v != Vertices.head);
        }

        /*---------------------------------------------------------------------
          Collinear checks to see if the three points given are collinear,
          by checking to see if each element of the cross product is zero.
          ---------------------------------------------------------------------*/
        protected bool Collinear(cVertex a, cVertex b, cVertex c)
        {
            return
              (c.Point.Z - a.Point.Z) * (b.Point.Y - a.Point.Y) -
              (b.Point.Z - a.Point.Z) * (c.Point.Y - a.Point.Y) == 0
              && (b.Point.Z - a.Point.Z) * (c.Point.X - a.Point.X) -
              (b.Point.X - a.Point.X) * (c.Point.Z - a.Point.Z) == 0
              && (b.Point.X - a.Point.X) * (c.Point.Y - a.Point.Y) -
              (b.Point.Y - a.Point.Y) * (c.Point.X - a.Point.X) == 0;
        }

        /*---------------------------------------------------------------------
          Computes the z-coordinate of the vector normal to face f.
          ---------------------------------------------------------------------*/
        protected float Normz(cFace f)
        {
            cVertex a, b, c;
            /*float ba0, ca1, ba1, ca0,z;*/

            a = f.Vertices[0];
            b = f.Vertices[1];
            c = f.Vertices[2];

            /*
              ba0 = ( b.v.x - a.v.x );
              ca1 = ( c.v.y - a.v.y );
              ba1 = ( b.v.y - a.v.y );
              ca0 = ( c.v.x - a.v.x );
      
              z = ba0 * ca1 - ba1 * ca0; 
              System.Diagnostics.Debug.WriteLine("Normz = %lf=%g\n", z,z);
              if      ( z > 0.0 )  return  1;
              else if ( z < 0.0 )  return -1;
              else                 return  0;
            */
            return
              (b.Point.X - a.Point.X) * (c.Point.Y - a.Point.Y) -
              (b.Point.Y - a.Point.Y) * (c.Point.X - a.Point.X);
        }

        /*---------------------------------------------------------------------
          Consistency runs through the edge list and checks that all
          adjacent faces have their endpoints in opposite order.  This verifies
          that the pointCloud are in counterclockwise order.
          ---------------------------------------------------------------------*/
        protected void Consistency()
        {
            cEdge e;
            int i, j;

            e = Edges.head;

            do
            {
                /* find index of endpoint[0] in adjacent face[0] */
                for (i = 0; e.Adjface[0].Vertices[i] != e.Endpts[0]; ++i)
                    ;

                /* find index of endpoint[0] in adjacent face[1] */
                for (j = 0; e.Adjface[1].Vertices[j] != e.Endpts[0]; ++j)
                    ;

                /* check if the endpoints occur in opposite order */
                if (!(e.Adjface[0].Vertices[(i + 1) % 3] ==
                    e.Adjface[1].Vertices[(j + 2) % 3] ||
                    e.Adjface[0].Vertices[(i + 2) % 3] ==
                    e.Adjface[1].Vertices[(j + 1) % 3]))
                    break;
                e = e.next;

            } while (e != Edges.head);

            if (e != Edges.head)
                System.Diagnostics.Debug.WriteLine("Checks: edges are NOT consistent.");
            else
                System.Diagnostics.Debug.WriteLine("Checks: edges consistent.");
        }

        /*---------------------------------------------------------------------
          Convexity checks that the volume between every face and every
          point is negative.  This shows that each point is inside every face
          and therefore the hull is convex.
          ---------------------------------------------------------------------*/
        protected void Convexity()
        {
            cFace f;
            cVertex v;
            float vol;

            f = Faces.head;

            do
            {
                v = Vertices.head;
                do
                {
                    if (v.IsProcessed)
                    {
                        vol = VolumeSign(f, v);
                        if (vol < 0)
                            break;
                    }
                    v = v.NextVertex;
                } while (v != Vertices.head);

                f = f.next;

            } while (f != Faces.head);

            if (f != Faces.head)
                System.Diagnostics.Debug.WriteLine("Checks: NOT convex.");
            
        }

        /*---------------------------------------------------------------------
          CheckEuler checks Euler's relation, as well as its implications when
          all faces are known to be triangles.  Only prints positive information
          when debug is true, but always prints negative information.
          ---------------------------------------------------------------------*/
        protected void CheckEuler(float V, float E, float F)
        {
           

            if ((V - E + F) != 2)
                System.Diagnostics.Debug.WriteLine(" Checks: V-E+F != 2\n");
          


            if (F != (2 * V - 4))
                System.Diagnostics.Debug.WriteLine(" Checks: F=" + F + " != 2V-4=" + (2 * V - 4) + "; V=" + V);

         

            if ((2 * E) != (3 * F))
                System.Diagnostics.Debug.WriteLine(" Checks: 2E=" + 2 * E + " != 3F=" + 3 * F + "; E=" + E + ", F=" + F);
          
        }

        /*-------------------------------------------------------------------*/
        protected void Checks()
        {
            cVertex v;
            cEdge e;
            cFace f;
            float V = 0, E = 0, F = 0;

            Consistency();
            Convexity();

            v = Vertices.head;
            if (v != null)
                do
                {
                    if (v.IsProcessed) V++;
                    v = v.NextVertex;
                } while (v != Vertices.head);

            e = Edges.head;
            if (e != null)
                do
                {
                    E++;
                    e = e.next;
                } while (e != Edges.head);

            f = Faces.head;
            if (f != null)
                do
                {
                    F++;
                    f = f.next;
                } while (f != Faces.head);
            CheckEuler(V, E, F);
        }


        /*===================================================================
          These functions are used whenever the debug flag is set.
          They print out the entire contents of each data structure.  
          Printing is to standard error.  To grab the output in a file in the csh, 
          use this:
          chull < i.file >&! o.file
          =====================================================================*/
        /*-------------------------------------------------------------------*/
        protected void PrintOut(cVertex v)
        {
            System.Diagnostics.Debug.WriteLine("Head vertex " + v.IndexInModel + " =  " + v + "\t:");
            PrintVertices();
            PrintEdges();
            PrintFaces();
        }

        /*-------------------------------------------------------------------*/
        protected void PrintVertices()
        {
            cVertex temp;

            temp = Vertices.head;
            System.Diagnostics.Debug.WriteLine("Vertex List");
            if (Vertices.head != null) do
                {
                    System.Diagnostics.Debug.WriteLine("  addr " + Vertices.head + "\t");
                    System.Diagnostics.Debug.WriteLine("  vnum " + Vertices.head.IndexInModel);
                    System.Diagnostics.Debug.WriteLine("   (" + Vertices.head.Point.X + ","
                           + Vertices.head.Point.Y + ","
                           + Vertices.head.Point.Z + ")");
                    System.Diagnostics.Debug.WriteLine("   active:" + Vertices.head.IsOnHull);
                    System.Diagnostics.Debug.WriteLine("   dup:" + Vertices.head.Edge);
                    System.Diagnostics.Debug.WriteLine("   mark:\n" + Vertices.head.IsProcessed);
                    Vertices.head = Vertices.head.NextVertex;
                } while (Vertices.head != temp);

        }

        /*-------------------------------------------------------------------*/
        protected void PrintEdges()
        {
            cEdge temp;
            int i;

            temp = Edges.head;
            System.Diagnostics.Debug.WriteLine("Edge List");
            if (Edges.head != null) do
                {
                    System.Diagnostics.Debug.WriteLine("  addr: " + Edges.head + "\t");
                    System.Diagnostics.Debug.WriteLine("adj: ");
                    for (i = 0; i < 2; ++i)
                        System.Diagnostics.Debug.WriteLine(Edges.head.Adjface[i]);
                    System.Diagnostics.Debug.WriteLine("  endpts:");
                    for (i = 0; i < 2; ++i)
                        System.Diagnostics.Debug.WriteLine(Edges.head.Endpts[i].IndexInModel);
                    System.Diagnostics.Debug.WriteLine("  del:" + Edges.head.delete + "\n");
                    Edges.head = Edges.head.next;
                } while (Edges.head != temp);

        }

        /*-------------------------------------------------------------------*/
        protected void PrintFaces()
        {
            int i;
            cFace temp;

            temp = Faces.head;
            System.Diagnostics.Debug.WriteLine("Face List\n");
            if (Faces.head != null) do
                {
                    System.Diagnostics.Debug.WriteLine("  addr: " + Faces.head + "\t");
                    System.Diagnostics.Debug.WriteLine("  edges:");
                    for (i = 0; i < 3; ++i)
                        System.Diagnostics.Debug.WriteLine(Faces.head.Edges[i]);
                    System.Diagnostics.Debug.WriteLine("  vert:");
                    for (i = 0; i < 3; ++i)
                        System.Diagnostics.Debug.WriteLine(Faces.head.Vertices[i].IndexInModel);
                    System.Diagnostics.Debug.WriteLine("  vis: " + Faces.head.visible + "\n");
                    Faces.head = Faces.head.next;
                } while (Faces.head != temp);
        }

        protected void Swap(cEdge t, cEdge x, cEdge y)
        {
            t = x;
            x = y;
            y = t;
        }
    }
}