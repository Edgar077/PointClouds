/*--------------------------------------------------------------------------
 * Class DelaunayTri
 *
 * Computes DelaunayTriangulation of points in 2D
 * This code is described in "Computational Geometry in C",
 * It is not written to be comprehensible without the
 * explanation in that book.
 *---------------------------------------------------------------------------*/

using OpenTK;
using OpenTKExtension;
using System;
using System.Collections.Generic;



namespace OpenTKExtension
{

    public class DelaunayTri : ConvexHull3D
    {
        
        public DelaunayTri():base()
        {
           
           
        }
        private bool Delaunay(List<Vector3> myListVectors)
        {
            InitVectors(myListVectors);
            if (Hull())
            {
                LowerFaces();
                return true;
            }
            else
            {
               System.Windows.Forms.MessageBox.Show("Delaunay Failed");
                return false;
            }
            
            
        }
        public DelaunayTri(List<Vector3> myListVectors) : this()
        {
            Delaunay(myListVectors);
            

        }


        public bool Voronoi(List<Vector3> myListVectors)
        {
            if (Delaunay(myListVectors))
            {

                for (int i = 0; i < this.Faces.ListFaces.Count; i++)
                {
                    cFace face = this.Faces.ListFaces[i];
                    for (int j = 0; j < face.Edges.Length; j++)
                    {
                        cEdge edge = face.Edges[j];
                        for (int k = 0; k < edge.Adjface.Length; k++)
                        {
                            cFace adjFace = edge.Adjface[k];
                            cEdge newEdge = new cEdge();

                            //Kante m durch Verbindung der Umkreismittelpunkte von k und k+1
                        }
                    }


                }
                return true;
            }
            return false;

            //var t = DelaunayTriangulation<TCell>.Create(data);
            //var myCells = t.Cells;
            //var edges = new HashSet<TEdge>(new EdgeComparer());

            //foreach (var c in myCells)
            //{
            //    for (int i = 0; i < c.Adjacency.Length; i++)
            //    {
            //        var af = c.Adjacency[i];
            //        if (af != null)
            //            edges.Add(new TEdge { Source = c, Target = af });
            //    }
            //}

            //return new VoronoiMesh<TCell, TEdge>
            //{
            //    Cells = myCells,
            //    Edges = edges.ToList()
            //};
        }
       
        

      

        private void LowerFaces()
        {
            cFace f = Faces.head;
            /*int   z;*/
            int Flower = 0;   /* Total number of lower faces. */

            do
            {
                /*z = Normz( f );
              if ( z < 0 ) {*/
                if (Normz(f) < 0)
                {
                    Flower++;
                    f.lower = true;
                    System.Diagnostics.Debug.WriteLine("lower face indices: " + f.Vertices[0].IndexInPointCloud + ", " +
                               f.Vertices[1].IndexInPointCloud + ", " + f.Vertices[2].IndexInPointCloud);
                }
                else f.lower = false;
                f = f.next;
            } while (f != Faces.head);
            System.Diagnostics.Debug.WriteLine("A total of " + Flower + " lower faces identified.");
        }

    
    }
}