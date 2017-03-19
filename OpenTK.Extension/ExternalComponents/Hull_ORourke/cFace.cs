/*--------------------------------------------------------------------------
 * Class cFace -- face of polyhydra or planar graph;
 *                in this case it is a triangular face
 *
 * cFace() - constructor
 * PrintFace(int k) - prints face k to the console
 *
 *-------------------------------------------------------------------------*/



using OpenTK;
using OpenTKExtension;
using System;


namespace OpenTKExtension
{

    public class cFace
    {

        public cEdge[] Edges;             /* edges which compose the face */
        public cVertex[] Vertices;           /* pointCloud which bound the face */
        public bool visible;	        /* T iff face visible from new point. */
        public bool lower;              /* T iff on the lower hull */
        public cFace next, prev;

        public cFace()
        {
            Edges = new cEdge[3];
            Edges[0] = Edges[1] = Edges[2] = null;
            Vertices = new cVertex[3];
            Vertices[0] = Vertices[1] = Vertices[2] = null;
            visible = lower = false;
            next = prev = null;
        }

        public void PrintFace(int k)
        {
            System.Diagnostics.Debug.WriteLine("Face" + k + ":: edges...");
            Edges[0].PrintEdge(0);
            Edges[1].PrintEdge(1);
            Edges[2].PrintEdge(2);
        }
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for(int i = 0; i < this.Vertices.Length; i++)
            {
                sb.Append(i.ToString() + " : " + this.Vertices[i].ToString() + " || ");

            }
            return sb.ToString();
        }
    }

}