/*-----------------------------------------------------------------------------
 * Class cEdge is used to represent an edge of a polygon or polyhydra
 *-----------------------------------------------------------------------------*/



using OpenTK;
using OpenTKExtension;
using System;


namespace OpenTKExtension
{

    public class cEdge
    {

        public cFace[] Adjface;          /* adjacent face; 2 */
        public cVertex[] Endpts;           /* end points of the edge */
        public cFace newface;            /* pointer to incident cone face. */
        public bool delete;	       /* T iff edge should be delete. */
        public cEdge next, prev;         /* pointers to neighbours in cEdgeList */

        public cEdge()
        {
            Adjface = new cFace[2];
            Adjface[0] = Adjface[1] = null;
            Endpts = new cVertex[2];
            Endpts[0] = Endpts[1] = null;
            newface = null;
            delete = false;
            next = prev = null;
        }

        public void PrintEdge(int n)
        {
            if (this != null)
            {
                System.Diagnostics.Debug.WriteLine("Edge" + n + ": ");
                Endpts[0].PrintVertex();
                System.Diagnostics.Debug.WriteLine(" ");
                Endpts[1].PrintVertex();
                System.Diagnostics.Debug.WriteLine("; ");
                System.Diagnostics.Debug.WriteLine("");
            }
            else
                System.Diagnostics.Debug.WriteLine("no edge");
        }
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < this.Endpts.Length; i++)
            {
                sb.Append(i.ToString() + " : " + this.Endpts[i].ToString() + ";");

            }
            return sb.ToString();
        }
    }

}