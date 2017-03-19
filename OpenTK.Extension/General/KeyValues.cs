using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKExtension
{

    [Serializable]
    public struct Neighbours
    {

        public int IndexSource;
        public int IndexTarget;

        public float Angle;
        public float Distance;

        public Neighbours(int keySource, int keyTarget, float angle, float distance)
        {
            IndexSource = keySource;
            IndexTarget = keyTarget;
            Angle = angle;
            Distance = distance;
        }


        public override string ToString()
        {
            return "Source: " + this.IndexSource.ToString() + " : Target: " + this.IndexTarget.ToString() + " : Distance - angle: " +  this.Distance.ToString("0.000") + " : " + this.Angle.ToString("0.00");


        }
    }

}
