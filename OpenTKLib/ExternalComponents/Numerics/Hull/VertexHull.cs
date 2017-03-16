using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTKExtension;


namespace NLinear
{
    public class VertexHull
    {
        public Vector3<float> Vector;
        public int Condition = -1;
        public float Radius = 0;
        public VertexHull() { }
        public VertexHull(Vector3<float> pt, float radius)
        {
            this.Vector = pt; 
            Radius = radius;
        }
    }
  
   
}
