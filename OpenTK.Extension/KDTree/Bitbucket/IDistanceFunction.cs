using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKLib
{

    public interface IDistanceFunction
    {
        double distance(double[] p1, double[] p2);
        double distanceToRect(double[] point, double[] min, double[] max);
    }
}
