using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTKExtension;
using System.Collections.Specialized;

namespace OpenTKExtension.DelaunayVoronoi
{
    public class Tester
    {
        IList<Vector3> v = new List<Vector3>();
        public void TestDelaunay()
        {
            Delaunay.DelaunayTriangulation(v);



        }
        public void VoronoiClosest()
        {
            Voronoi v1 = new ClosestVoronoi(v, 5000);
            Voronoi v2 = new FurthestVoronoi(ConvexHull.ComputeConvexHull(v), 5000);
            ConvexHull.ComputeConvexHull(v);

        }
        
    }

   
   
}
