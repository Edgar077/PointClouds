/*----------------------------------------------------------------------------
 * Class cPointd  -- point with float coordinates
 *
 * PrintPoint() -- prints point to the console;
 *
 *---------------------------------------------------------------------------*/


using OpenTK;
using OpenTKExtension;
using System;

namespace OpenTKExtension
{

    public class cPointd
    {
        public float x;
        public float y;

        public cPointd()
        {
            x = y = 0;
        }

        public cPointd(int myx, int myy)
        {
            x = myx;
            y = myy;
        }

        public void PrintPoint()
        {
            System.Diagnostics.Debug.WriteLine(" (" + x + "," + y + ")");
        }
    }
}