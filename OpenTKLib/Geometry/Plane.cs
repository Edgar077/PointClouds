using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace OpenTKExtension
{
    public class Plane
    {

        Vector3 normal;
        double w;

        public Plane(Vector3 normal, double w)
        {
            this.normal = normal;
            this.w = w;
        }
        public static Plane PlaneFromPoints(Vector3 a, Vector3 b, Vector3 c)
        {
            Vector3 n = b - a;
            Vector3 temp = c - a;
            n = Vector3.Cross(n, temp);
            n /= n.Length;
            double d = Vector3.Dot(n, a);

            //var n = b.Minus(a).cross(c.Minus(a)).unit();
            return new Plane(n, d);
        }

   
        public void flip() 
        {
            this.normal = this.normal.Negate();
            this.w = -this.w;
        }
     
   
    }
}
