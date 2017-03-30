using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using OpenTK;


namespace OpenTKExtension
{
    public class Line
    {
        public Vector3 PStart;
        public Vector3 PEnd;
        public Color Color;

        public Line(Vector3 pStart, Vector3 pEnd)
        {
            PStart = pStart;
            PEnd = pEnd;
        }
        public Line(Vector3 pStart, Vector3 pEnd, Color myColor)
        {
            PStart = pStart;
            PEnd = pEnd;
            Color = myColor;
        }
        public override string ToString()
        {

            return "Start: " + PStart.ToString() + " ; End: " + PEnd.ToString();
        }
        public Vector3 PointSymmetricByOrigin()
        {
            Vector3 p = new Vector3();
            for (int i = 0; i < 3; i++ )
            {
                float delta = PEnd[i] - PStart[i];
                p[i] = PStart[i] - delta;

            }
           return p;


        }
        /// <summary>
        /// result = - v + 2 * myCenterOfMass
        /// </summary>
        /// <param name="myCenterOfMass"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector3 PointSymmetricByCenterOfMass(Vector3 myCenterOfMass, Vector3 v)
        {
            //Line l = new Line(pStart, v);
            Vector3 p = v - myCenterOfMass;
            p = myCenterOfMass - p;

          
            return p;


        }
        public Vector3 MiddleVector3()
        {
            return new Vector3((this.PStart.X + this.PEnd.X) / 2, (this.PStart.Y + this.PEnd.Y) / 2, (this.PStart.Z + this.PEnd.Z) / 2);
        }
        public double SlopeXY
        {
            get
            {
                double d = PEnd.X - PStart.X;

                if (d == 0)
                {
                    return 1000; //very large value
                    //throw new LineIsVerticalException("The line passing through listVectors " + P1.ToString() + " and " + P2.ToString() + " is vertical.");
                }
                return (PEnd.Y - PStart.Y) / (PEnd.X - PStart.X);
            }
        }
        public static Vector3 GetYFromVector(Vector3 p, double slope, double x)
        {
            double y = (x - p.X) * slope + p.Y;

            return new Vector3(x, y, 0);
        }

        public static Vector3 GetXFromVector(Vector3 p, double slope, double y)
        {
            if (slope == 0)
                return new Vector3(y, y, 0);

            double x = (y - p.Y) / slope + p.X;

            return new Vector3(x, y, 0);
        }
    }
}
