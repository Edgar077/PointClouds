
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace OpenTKExtension
{
    //Extensios attached to the object which folloes the "this" 
    public static class Vector3Extension
    {

        /// <summary>Clone the vector
        /// </summary>
        /// <param name="vec">The vector to transform</param>
        /// <returns>The cloned vector</returns>
        public static Vector3 Clone(this Vector3 vector)
        {
            Vector3 vNew = new Vector3(vector.X, vector.Y, vector.Z);
            

            return vNew;

        }
        public static Vector3 Negate(this Vector3 vector)
        {
            Vector3 vNew = new Vector3(-vector.X, -vector.Y, -vector.Z);
            return vNew;

        }
        public static Vector3 LinearInterpolate(this Vector3 vector, Vector3 otherVector, float d)
        {
            Vector3 temp = vector + (otherVector - vector) * d;
            return temp;
        }

        public static float Norm(this Vector3 vector)
        {
            return Convert.ToSingle(Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z));
        }
        public static Vector3 NormalizeV(this Vector3 vector)
        {
            float den = vector.Norm();
            if (den != 0)
            {
                vector.X /= den;
                vector.Y /= den;
                vector.Z /= den;

            }
            return vector;

        }
        //public static Vector3 Normalize(this Vector3 vector)
        //{
        //    float den = vector.Norm();
        //    if (den != 0)
        //    {
        //        vector.X /= den;
        //        vector.Y /= den;
        //        vector.Z /= den;

        //    }
        //    return vector;

        //}
       
     
     // converts cartesion to polar coordinates
     // result:
     // [0] = length
     // [1] = angle with z-axis
     // [2] = angle of projection into x,y, plane with x-axis
     //
        public static Vector3 CartesianToPolar(this Vector3 v)
        {
            Vector3 polar = new Vector3();

            polar.X = v.Length;

            if (v[2] > 0.0f)
            {
                polar.Y = Convert.ToSingle(Math.Atan(Math.Sqrt(v[0] * v[0] + v[1] * v[1]) / v[2]));
            }
            else if (v[2] < 0.0f)
            {
                polar[1] = Convert.ToSingle(Math.Atan(Math.Sqrt(v[0] * v[0] + v[1] * v[1]) / v[2]) + Math.PI);
            }
            else
            {
                polar[1] = Convert.ToSingle(Math.PI * 0.5f);
            }


            if (v[0] > 0.0f)
            {
                polar[2] = (float) Convert.ToSingle(Math.Atan(v[1] / v[0]));
            }
            else if (v[0] < 0.0f)
            {
                polar[2] = (float) Convert.ToSingle(Math.Atan(v[1] / v[0]) + Math.PI);
            }
            else if (v[1] > 0)
            {
                polar[2] = Convert.ToSingle(Math.PI * 0.5f);
            }
            else
            {
                polar[2] = -Convert.ToSingle(Math.PI * 0.5);
            }
            return polar;
        }



        ///
        //  converts polar to cartesion coordinates
        //  input:
        //  [0] = length
        //  [1] = angle with z-axis
        //  [2] = angle of projection into x,y, plane with x-axis
        // 
        public static Vector3 PolarToCartesian(this Vector3 v)
        {
            Vector3 cart = new Vector3();
            cart[0] = Convert.ToSingle(v[0] * Math.Sin(v[1]) * (float)Math.Cos(v[2]));
            cart[1] = Convert.ToSingle(v[0] * Math.Sin(v[1]) * (float)Math.Sin(v[2]));
            cart[2] = Convert.ToSingle(v[0] * Math.Cos(v[1]));
            return cart;
        }


        /////////////////////////////////////////////////////////////////
        /// <summary>
        /// projects Vector v1 on v2 , return value is projection
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        static Vector3 ProjectOntoVector(this Vector3 v1, Vector3 v2)
        {
            return v2 * Vector3.Dot(v1, v2);
        }


        public static Vector3 ProjectVectorIntoPlane(this Vector3 v1, Vector3 normalOfPlane)
        {
            return v1 - ProjectOntoVector(v1, normalOfPlane);
        }


        public static Vector3 ProjectPointOntoPlane(this Vector3 point, Vector3 anchor, Vector3 normal)
        {
            Vector3 temp = point - anchor;
            return point - ProjectOntoVector(temp, normal);
        }
        public static float AngleInRadians(this Vector3 a, Vector3 b)
        {
            if (a.Equals(Vector3.Zero) || b.Equals(Vector3.Zero))
                return 360;

            Vector3 v = Vector3.Cross(a, b);
            float d1 = v.Norm();

            float d2 = Vector3.Dot(a, b);
            float angle =  Convert.ToSingle(Math.Atan2(d1, d2));
            

            ////-----------------------------------
            ////alternative
            //float dot = Vector3.Dot(a, b);
            //// Divide the dot by the product of the magnitudes of the vectors
            //dot = dot / (a.Norm() * b.Norm());
            ////Get the arc cosin of the angle, you now have your angle in radians 
            //float acos = Math.Acos(dot);
            ////Multiply by 180/Mathf.PI to convert to degrees
            //float angleCheck = acos * 180 / Math.PI;
            ////-----------------------------

            //if (angle - angleCheck > 0.1)
            //    System.Windows.Forms.MessageBox.Show("SW Check Angle");

            return angle;




        }
        public static float AngleInDegrees(this Vector3 a, Vector3 b)
        {
            if(a.Equals(Vector3.Zero) || b.Equals(Vector3.Zero))
                return 360;

            Vector3 v = Vector3.Cross(a, b);
            float d1 = v.Norm();

            float d2 = Vector3.Dot(a, b);
            float angle =  Convert.ToSingle(Math.Atan2(d1, d2));
            angle =Convert.ToSingle( angle * 180 / Math.PI);

            ////-----------------------------------
            ////alternative
            //float dot = Vector3.Dot(a, b);
            //// Divide the dot by the product of the magnitudes of the vectors
            //dot = dot / (a.Norm() * b.Norm());
            ////Get the arc cosin of the angle, you now have your angle in radians 
            //float acos = Math.Acos(dot);
            ////Multiply by 180/Mathf.PI to convert to degrees
            //float angleCheck = acos * 180 / Math.PI;
            ////-----------------------------

            //if (angle - angleCheck > 0.1)
            //    System.Windows.Forms.MessageBox.Show("SW Check Angle");

            return angle;


        }
        public static float DistanceSlower(this Vector3 vector, Vector3 vOther)
        {
            float fSum = 0;
            for (int i = 0; i < 3; i++)
            {
                float fDifference = (vector[i] - vOther[i]);
                fSum += fDifference * fDifference;
            }
            return Convert.ToSingle(System.Math.Sqrt(fSum));
        }
        public static float DistanceSquared(this Vector3 vector, Vector3 vOther)
        {
            float fSum = 0;
            for (int i = 0; i < 3; i++)
            {
                float fDifference = (vector[i] - vOther[i]);
                fSum += fDifference * fDifference;
            }
            return fSum;
        }
        public static float Distance(this Vector3 vector, Vector3 vOther)
        {
            float fSum = 0f;
            float f = vector.X - vOther.X;
            fSum += f * f;
            f = vector.Y - vOther.Y;
            fSum += f * f;
            f = vector.Z - vOther.Z;
            fSum += f * f;
            

            return Convert.ToSingle(System.Math.Sqrt(fSum));
        }
     
        public static Vector3 FromFloatArray(this Vector3 v, float[] arr)
        {
         
            
            for (int i = 0; i < arr.Length; i++)
            {
                v[i] = arr[i];
            }
            return v;


        }
        public static Vector3 ColorVectorFromByteArray(this Vector3 v, byte[] arr)
        {


            for (int i = 0; i < arr.Length; i++)
            {
                v[i] = Convert.ToSingle(arr[i]/255f);
            }
            return v;


        }
        public static Vector3 ColorVectorFromColor(this Vector3 v, System.Drawing.Color c)
        {


            v[0] = Convert.ToSingle(c.R/ 255f);
            v[1] = Convert.ToSingle(c.G / 255f);
            v[2] = Convert.ToSingle(c.B / 255f);

            return v;


        }
        public static Vector3 FromDoubleArray(this Vector3 v, double[] arr)
        {


            for (int i = 0; i < arr.Length; i++)
            {
                v[i] = Convert.ToSingle(arr[i]);
            }
            return v;


        }
        public static Vector3 Abs(this Vector3 v)
        {

            Vector3 vAbs = new Vector3();
            for (int i = 0; i < 3; i++)
            {
                vAbs[i] = Math.Abs(v[i]);
            }
            return vAbs;


        }
        public static bool IsZero(this Vector3 v)
        {

            if(v.X == 0 && v.Y == 0 && v.Z == 0)
                return true;

            return false;


        }
        public static void Print(this Vector3 v, string name)
        {


            System.Diagnostics.Debug.WriteLine(name + " : " + v[0].ToString("0.00") + " " + v[1].ToString("0.00") + " " + v[2].ToString("0.00"));

         
        }


        public static Vector3 CrossProduct(this Vector3 v1, Vector3 v2)
        {
            return new Vector3()
            {
                X = v1.Y * v2.Z - v2.Y * v1.Z,
                Y = -v1.X * v2.Z + v2.X * v1.Z,
                Z = v1.X * v2.Y - v2.X * v1.Y
            };
        }

        public static float NormSquared(this Vector3 v)
        {

            float val = v.X * v.X + v.Y * v.Y + v.Z * v.Z;

            return val;
        }
        public static float MaxCoordinate(this Vector3 v)
        {
            float maxC = v.X;
            if (v.Y > maxC)
                maxC = v.Y;
            if (v.Z > maxC)
                maxC = v.Z;


            return maxC;
        }

        public static Vector3 Up (this Vector3 v)
        {
            return new Vector3(0.0f, 1.0f, 0.0f); 
        }

        public static Vector3 Down(this Vector3 v)
        {
            return new Vector3(0.0f, -1.0f, 0.0f); 
        }

        public static Vector3 Forward(this Vector3 v)
        {
            return new Vector3(0.0f, 0.0f, -1.0f); 
        }

        public static Vector3 Backward(this Vector3 v)
        {
            return new Vector3(0.0f, 0.0f, 1.0f); 
        }

        public static Vector3 Left(this Vector3 v)
        {
            return new Vector3(-1.0f, 0.0f, 0.0f); 
        }

        public static Vector3 Right(this Vector3 v)
        {
            return new Vector3(1.0f, 0.0f, 0.0f); 
        }

        public static float distance(this Vector3 v, Vector3 v1, Vector3 v2, string axis)
        {


            if (axis == "X")
            {
                float f = v1[0] - v2[0];
                return Math.Abs(f);
            }
            if (axis == "Y")
            {
                float f = v1[1] - v2[1];
                return Math.Abs(f);
            }
            if (axis == "Z")
            {
                float f = v1[2] - v2[2];
                return Math.Abs(f);
            }

            Vector3 vAll = v1 - v2;
            return vAll.Length;
        }


        /// <summary>
        /// Delta distance squared between this and other vertex t
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static float DeltaSquaredXY(this Vector3 v, Vector3 t)
        {
            float dx = (v.X - t.X);
            float dy = (v.Y - t.Y);
            return (dx * dx) + (dy * dy);
        }

        /// <summary>
        /// Delta distance squared between this and other vertex t
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static float DeltaSquared(this Vector3 v, Vector3 t)
        {
            float dx = (v.X - t.X);
            float dy = (v.Y - t.Y);
            float dz = (v.Z - t.Z);
            return (dx * dx) + (dy * dy) + (dz * dz);
        }

        /// <summary>
        /// The distance between this and t.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static float DistanceXY(this Vector3 v, Vector3 t)
        {
            
            return (float)System.Math.Sqrt(DeltaSquaredXY(v, t));
        }

        /// <summary>
        /// Find the shortest distance from a point to an axis aligned rectangle in n-dimensional space.
        /// </summary>
        /// <param name="point">The point of interest.</param>
        /// <param name="Rmin">The minimum coordinate of the rectangle.</param>
        /// <param name="Rmax">The maximum coorindate of the rectangle.</param>
        /// <returns>The shortest squared n-dimensional squared distance between the point and rectangle.</returns>
        public static float DistanceToRectangleSquared(this Vector3 point, Vector3 Rmin, Vector3 Rmax)
        {

            float fSum = 0;
            float fDifference = 0;
            for (int i = 0; i < point.Length; ++i)
            {
                fDifference = 0;
                if (point[i] > Rmax[i])
                    fDifference = (point[i] - Rmax[i]);
                else if (point[i] < Rmin[i])
                    fDifference = (point[i] - Rmin[i]);
                fSum += fDifference * fDifference;
            }
            return fSum;
        }
        /// <summary>
        /// Find the shortest distance from a point to an axis aligned rectangle in n-dimensional space.
        /// </summary>
        /// <param name="point">The point of interest.</param>
        /// <param name="Rmin">The minimum coordinate of the rectangle.</param>
        /// <param name="Rmax">The maximum coorindate of the rectangle.</param>
        /// <returns>The shortest squared n-dimensional squared distance between the point and rectangle.</returns>
        public static float DistanceToRectangle(this Vector3 point, Vector3 Rmin, Vector3 Rmax)
        {

            return Convert.ToSingle(Math.Sqrt(DistanceToRectangleSquared(point, Rmin, Rmax)));
        }
        ///// <summary>
        ///// The distance between this and t.
        ///// </summary>
        ///// <param name="t"></param>
        ///// <returns></returns>
        //public static float Distance(this Vector3 v, Vector3 t)
        //{
        //    return (float)System.Math.Sqrt(DeltaSquared(t));
        //}

        /// <summary>
        /// Is this rectangle within the region.
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        public static bool InsideXY(this Vector3 v, System.Drawing.RectangleF region)
        {
            if (v.X < region.Left) return false;
            if (v.X > region.Right) return false;
            if (v.Y < region.Top) return false;
            if (v.Y > region.Bottom) return false;
            return true;
        }

        ///// <summary>
        ///// Get the largest axis 
        ///// </summary>
        ///// <returns>Largest enumerated axis</returns>
        //public static Axis LargestAxis(this Vector3 v)
        //{
        //    if (v.X > v.Y && v.Y > v.Z)
        //        return Axis.X;
        //    else if (v.Y > v.Z)
        //        return Axis.Y;
        //    else
        //        return Axis.Z;

        //}
        /// <summary>
        /// Get the largest axis 
        /// </summary>
        /// <returns>Largest enumerated axis</returns>
        public static Axis LargestAxis(this Vector3 v)
        {
            float x = Math.Abs(v.X);
            float y = Math.Abs(v.Y);
            float z = Math.Abs(v.Z);

            if (x > y && y > z)
                return Axis.X;
            else if (y > z)
                return Axis.Y;
            else
                return Axis.Z;

        }

        /// <summary>
        /// Get the largest axis and the value of that axis
        /// </summary>
        /// <param name="value">Value of the largest axis</param>
        /// <returns>Largest enumerated axis</returns>
        public static Axis LargestAxis(this Vector3 v, out float value)
        {
            Axis return_axis = v.LargestAxis();
            value = v[(int)return_axis];
            return return_axis;
        }

        /// <summary>
        /// Get the largest axis and the value of that axis
        /// </summary>
        /// <param name="value">Value of the largest axis</param>
        /// <param name="axis"> Largest enumerated axis</param>
        public static void LargestAxis(this Vector3 v, out Axis axis, out float value)
        {
            axis = v.LargestAxis();
            value = v[(int)axis];
        }
        /// <summary>
        /// Find a series of metrics for the give Vector3 list.  
        /// </summary>
        /// <returns>TRUE if metrics were calculated, FALSE if not.</returns>
        public static bool Metrics(List<Vector3> vertices, out Vector3 min, out Vector3 max, out Vector3 center, out Vector3 centroid, out Vector3 range)
        {
            {
                min = new Vector3();
                max = new Vector3();
                center = new Vector3();
                centroid = new Vector3();
                range = new Vector3();
            }

            if (vertices.Count <= 0) return false;

            bool first_time = true;
            foreach (var vertex in vertices)
            {
                if (first_time) { min = max = vertex; first_time = false; }

                min = Vector3.Min(min, vertex);
                max = Vector3.Max(max, vertex);
                centroid += vertex;
            }
            center = (max + min) / 2.0f;
            centroid /= vertices.Count;
            range = max - min;

            return true;
        }

    }

    public enum Axis { X, Y, Z };
}
