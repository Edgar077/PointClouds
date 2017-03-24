using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK;

namespace OpenTKExtension.DelaunayVoronoi
{
    public class ConvexHull
    {
        public static List<Vector3> ComputeConvexHull(IList<Vector3> listVectors)
        {
            Vector3 startVector3 = listVectors[0];
            List<Vector3> hull = new List<Vector3>();

            foreach (Vector3 p in listVectors)
            {
                if (p.Y < startVector3.Y || (p.Y == startVector3.Y && p.X < startVector3.X))
                    startVector3 = p;
            }

            Vector3 curr = startVector3;

            for (; ; )
            {
                hull.Add(curr);

                Vector3 maybeVector3 = listVectors[(curr == listVectors[0] ? 1 : 0)];

                foreach (Vector3 p in listVectors)
                {
                    double t = TriangleVectors.SignedArea(curr, maybeVector3, p);

                    if (t < 0)
                        maybeVector3 = p;
                }

                if (maybeVector3 == startVector3)
                    break;

                curr = maybeVector3;
            }

            return hull;
        }
    }
}