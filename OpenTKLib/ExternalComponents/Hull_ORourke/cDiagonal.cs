/*----------------------------------------------------------------------
 * class cDiagonalList
 *
 * This class has no C counterpart.  It is needed for storing the diagonals 
 * in order to repaint them.
 *
 *---------------------------------------------------------------------*/


using OpenTK;
using OpenTKExtension;
using System;

namespace OpenTKExtension
{
    public class cDiagonal
    {
        public cVertex v1, v2;
        public cDiagonal next, prev;

        public cDiagonal()
        {
            next = prev = null;
            v1 = v2 = new cVertex();
        }

        public cDiagonal(cVertex v1, cVertex v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }

        public void PrintDiagonal(int index)
        {
            System.Diagnostics.Debug.WriteLine("D" + index + " = ");
            v1.PrintVertex();
            v2.PrintVertex();
        }
    }

}