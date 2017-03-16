/*----------------------------------------------------------------------
 * class cDiagonalList
 *
 * This class has no C counterpart.  It is only used to store the diagonals
 * for later graphics (repainting, etc.)
 * The methods here parallel those for VertexList, and will not be commented 
 * upon here.
 * 
 *---------------------------------------------------------------------*/


using OpenTK;
using OpenTKExtension;
using System;


namespace OpenTKExtension
{

    public class cDiagonalList
    {
        public int n;
        public cDiagonal head;

        public cDiagonalList()
        {
            head = null;
            n = 0;
        }

        public void ClearDiagonalList()
        {
            if (head != null)
                head = null;
            n = 0;
        }

        private void InitHead(cDiagonal diag)
        {
            head = new cDiagonal(diag.v1, diag.v2);
            head.next = head.prev = head;
            n = 1;
        }


        public void InsertBeforeHead(cDiagonal diag)
        {
            if (head == null)
                InitHead(diag);
            else
            {
                InsertBefore(diag, head);
            }
        }

        private void InsertBefore(cDiagonal newD, cDiagonal old)
        {
            if (head == null)
                InitHead(newD);
            else
            {
                old.prev.next = newD;
                newD.prev = old.prev;
                newD.next = old;
                old.prev = newD;
                n++;
            }
        }

        public void PrintDiagonals()
        {

            cDiagonal temp = head;
            int i = 0;

            do
            {
                temp.PrintDiagonal(i);
                temp = temp.next;
                i++;
            } while (temp != head);

        }

        public void DrawDiagonals(System.Drawing.Graphics g, System.Drawing.Color inColor)
        {
            System.Diagnostics.Debug.WriteLine("Drawing diagonals");
            cDiagonal dtemp = head;
            //g.setColor(inColor);
            //do
            //{
            //    g.drawLine(dtemp.v1.v.x, dtemp.v1.v.y, dtemp.v2.v.x, dtemp.v2.v.y);
            //    dtemp = dtemp.next;
            //} while (dtemp != head);
        }
    }
}