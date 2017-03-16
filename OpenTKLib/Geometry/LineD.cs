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
    public class LineD
    {
        public Vector3 PStart;
        public Vector3 PEnd;
        public Color Color;

        public LineD(Vector3 pStart, Vector3 pEnd)
        {
            PStart = pStart;
            PEnd = pEnd;
        }
        public LineD(Vector3 pStart, Vector3 pEnd, Color myColor)
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
        
    }
}
