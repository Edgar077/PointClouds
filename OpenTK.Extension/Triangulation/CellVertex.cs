/*************************************************************************
 *     This file & class is part of the MIConvexHull Library Project. 
 *     Copyright 2010 Matthew Ira Campbell, PhD.
 *
 *     MIConvexHull is free software: you can redistribute it and/or modify
 *     it under the terms of the GNU General Public License as published by
 *     the Free Software Foundation, either version 3 of the License, or
 *     (at your option) any later version.
 *  
 *     MIConvexHull is distributed in the hope that it will be useful,
 *     but WITHOUT ANY WARRANTY; without even the implied warranty of
 *     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *     GNU General Public License for more details.
 *  
 *     You should have received a copy of the GNU General Public License
 *     along with MIConvexHull.  If not, see <http://www.gnu.org/licenses/>.
 *     
 *     Please find further details and contact information on GraphSynth
 *     at http://miconvexhull.codeplex.com
 *************************************************************************/
using System;
using System.Linq;
using System.Windows;
using MIConvexHull;
using OpenTK;

namespace OpenTKExtension
{


    /// <summary>
    /// A vertex is a simple class that stores the postion of a point, node or vertex.
    /// </summary>
    public class CellVertex : TriangulationCell<Vertex, CellVertex>
    {
        static Random rnd = new Random();
        OpenTK.Vector3 circumCenter;
        OpenTK.Vector3 centroid;

        
        float Det(float[,] m)
        {
            return m[0, 0] * ((m[1, 1] * m[2, 2]) - (m[2, 1] * m[1, 2])) - m[0, 1] * (m[1, 0] * m[2, 2] - m[2, 0] * m[1, 2]) + m[0, 2] * (m[1, 0] * m[2, 1] - m[2, 0] * m[1, 1]);
        }

        float LengthSquared(float[] v)
        {
            float norm = 0;
            for (int i = 0; i < v.Length; i++)
            {
                var t = v[i];
                norm += t * t;
            }
            return norm;
        }

        OpenTK.Vector3 GetCircumcenter()
        {
            // From MathWorld: http://mathworld.wolfram.com/Circumcircle.html

            var points = Vertices;

            float[,] m = new float[3, 3];

            // x, y, 1
            for (int i = 0; i < 3; i++)
            {
                m[i, 0] = points[i][0];
                m[i, 1] = points[i][1];
                m[i, 2] = 1;
            }
            var a = Det(m);

            // size, y, 1
            for (int i = 0; i < 3; i++)
            {
                //m[i, 0] = LengthSquared(points[i].PositionArray);
                m[i, 0] = LengthSquared(points[i].ToArray());
            }
            var dx = -Det(m);

            // size, x, 1
            for (int i = 0; i < 3; i++)
            {
                m[i, 1] = points[i][0];
            }
            var dy = Det(m);

            // size, x, y
            for (int i = 0; i < 3; i++)
            {
                m[i, 2] = points[i][1];
            }
            var c = -Det(m);

            var s = -1/ (2* a);
            var r = System.Math.Abs(s) * System.Math.Sqrt(dx * dx + dy * dy - 4 * a * c);
            return new OpenTK.Vector3(s * dx, s * dy, 0);
        }

        OpenTK.Vector3 GetCentroid()
        {
            return new OpenTK.Vector3(Vertices.Select(v => v[0]).Average(), Vertices.Select(v => v[1]).Average(), 0);
        }


        public OpenTK.Vector3 Circumcenter
        {
            get
            {
                if (circumCenter == default(OpenTK.Vector3))
                    circumCenter = GetCircumcenter();
                return circumCenter;
            }
        }

        public OpenTK.Vector3 Centroid
        {
            get
            {
                if (centroid == default(OpenTK.Vector3))
                    centroid = GetCentroid();
                 
                return centroid;
            }
        }

       
    }
}
