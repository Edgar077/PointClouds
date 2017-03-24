using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace OpenTKExtension.DelaunayVoronoi
{
    public class TriangleVectors
    {
        public TriangleVectors(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            this.P1 = p1;
            this.P2 = p2;
            this.P3 = p3;
        }

        public Vector3 P1, P2, P3;
      

        public Vector3[] Vertices 
        {
            get
            {
                return new Vector3[] { P1, P2, P3 };

            }

        }

      

      

        public static double SignedArea(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            return ((p1.X - p3.X) * (p2.Y - p1.Y) - (p1.X - p2.X) * (p3.Y - p1.Y)) / 2;
        }
    }
}
