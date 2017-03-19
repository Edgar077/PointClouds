using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKLib
{
    public class SquareEuclideanDistanceFunction : IDistanceFunction 
    {

        public double distance(double[] p1, double[] p2)
        {
            double d = 0;

            for (int i = 0; i < p1.Length; i++)
            {
                double diff = (p1[i] - p2[i]);
                d += diff * diff;
            }

            return d;
        }


        public double distanceToRect(double[] point, double[] min, double[] max)
        {
            double d = 0;

            for (int i = 0; i < point.Length; i++)
            {
                double diff = 0;
                if (point[i] > max[i])
                {
                    diff = (point[i] - max[i]);
                }
                else if (point[i] < min[i])
                {
                    diff = (point[i] - min[i]);
                }
                d += diff * diff;
            }

            return d;
        }

    }
}
