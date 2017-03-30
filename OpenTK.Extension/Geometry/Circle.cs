using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTKExtension;

namespace OpenTKExtension
{
    public class Circle
    {
        public Vector3 Center { get; set; }
        public double Radius { get; set; }

        public double Area
        {
            get
            {
                return Math.PI * Math.Pow(this.Radius, 2);
            }
        }

        public Circle(Vector3 center, double radius)
        {
            this.Center = center;
            this.Radius = radius;
        }

        public Circle(TriangleVectors t)
            : this(t.A.Vector, t.B.Vector, t.C.Vector)
        {

        }

        public Circle(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            if (p1.Collinear2D(p2, p3))
            {
                if (p1 != p2 && p1 != p3 && p2 != p3)
                {
                    throw new ImpossibleCircleException("The listVectors are different and collinear.");
                }
                else
                {
                    Vector3 pa = p1;
                    Vector3 pb = (p1 == p2 ? p3 : p2);

                    this.Center = (new Line(pa, pb)).MiddleVector3();
                    Vector3 vDist = Vector3.Subtract(this.Center, pa);
                    this.Radius = vDist.Length;
                }
            }
            else
            {
                if (p2.X == p1.X)
                {
                    Vector3 temp = p3;
                    p3 = p2;
                    p2 = temp;
                }
                else
                {
                    if (p2.X == p3.X)
                    {
                        Vector3 temp = p1;
                        p1 = p2;
                        p2 = temp;
                    }
                }

                Line l1 = new Line(p1, p2);
                Line l2 = new Line(p2, p3);

                double m1 = l1.SlopeXY;
                double m2 = l2.SlopeXY;

                double x = (m1 * m2 * (p1.Y - p3.Y) + m2 * (p1.X + p2.X) - m1 * (p2.X + p3.X)) / (2 * (m2 - m1));

                Line l = m1 != 0 ? l1 : l2;

                Vector3 p = Line.GetYFromVector(l.MiddleVector3(), -1 / l.SlopeXY, x);

                this.Center = p;
                Vector3 vDist = Vector3.Subtract(p1, this.Center);

              
                this.Radius = vDist.Length;
            }
        }

        public double Distance(Vector3 p)
        {
            
            Vector3 vDist = Vector3.Subtract(p, this.Center);
            
            var d = vDist.Length - this.Radius;
            return (d > 0 ? d : 0);
        }

        public double DistanceToBorder(Vector3 p)
        {
            Vector3 vDist = Vector3.Subtract(p, this.Center);
          
            return Math.Abs(vDist.Length - this.Radius);
        }

        public bool IsOnBorder(Vector3 p)
        {
            return this.DistanceToBorder(p) == 0;
        }

        public bool Contains(Vector3 p)
        {
            Vector3 vDist = Vector3.Subtract(p, this.Center);
           

            return vDist.Length <= this.Radius;
        }
    }

    class ImpossibleCircleException : ApplicationException
    {
        public ImpossibleCircleException(string message)
            : base(message)
        {

        }
    }
}
