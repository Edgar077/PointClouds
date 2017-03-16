using System;
using System.Collections;
using System.Collections.Generic;


using OpenTK;
namespace OpenTKExtension
{

    /// <summary>
    /// A data item which is stored in each kd node.
    /// </summary>
    public class EllipseWrapper
    {
        //public bool Filled;
        public double X;
        public double Y;
        public double Z;
        public Vertex Vertex;
        public EllipseWrapper(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Vertex = new Vertex(x, y, z);
            //this.Filled = false;
        }
        public EllipseWrapper(Vertex myVertex)
        {
            this.X = myVertex.Vector.X;
            this.Y = myVertex.Vector.Y;
            this.Z = myVertex.Vector.Z;
            this.Vertex = myVertex;
            //this.Filled = false;
        }
        public EllipseWrapper(Vector3 myVector)
        {
            this.X = myVector.X;
            this.Y = myVector.Y;
            this.Z = myVector.Z;
            this.Vertex = new Vertex(myVector);
            //this.Filled = false;
        }
        public override string ToString()
        {
            return this.X.ToString("0.00") + " : "  + this.Y.ToString("0.00") + " : " + this.Z.ToString("0.00") + " : ";
        }
    }
    //public enum KDTreeMode
    //{
    //    Rednaxela,
    //    Rednaxela_ExcludePoints,
    //    Stark,
    //    Numerics,
    //    BruteForce
    //}
}