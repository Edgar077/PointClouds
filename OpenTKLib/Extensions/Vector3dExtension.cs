
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace OpenTKExtension
{
    //Extensios attached to the object which folloes the "this" 
    public static class Vector3dExtension
    {

        /// <summary>Transform a direction vector by the given Matrix
        /// Assumes the matrix has a bottom row of (0,0,0,1), that is the translation part is ignored.
        /// </summary>
        /// <param name="vec">The vector to transform</param>
        /// <param name="mat">The desired transformation</param>
        /// <returns>The transformed vector</returns>
        public static Vector3d Clone(this Vector3d vector)
        {
            Vector3d vNew = new Vector3d(vector.X, vector.Y, vector.Z);
            

            return vNew;

        }
        public static Vector3d FromVector3(this Vector3d vector, Vector3 v3)
        {
            Vector3d vNew = new Vector3d(v3.X, v3.Y, v3.Z);


            return vNew;

        }
        public static List<Vector3d> ArrayVector3ToList(this Vector3d vector, Vector3[] v3Array)
        {
            List<Vector3d> vList = new List<Vector3d>();
            for (int i = 0; i < v3Array.Length; i++)
            {
                Vector3d v = new Vector3d();
                v = v.FromVector3(v3Array[i]);

                vList.Add(v);
            }


            return vList;

        }
        public static Vector3d Negate(this Vector3d vector)
        {
            Vector3d vNew = new Vector3d(-vector.X, -vector.Y, -vector.Z);
            return vNew;

        }
        public static Vector3d LinearInterpolate(this Vector3d vector, Vector3d otherVector, double d)
        {
            Vector3d temp = vector + (otherVector - vector) * d;
            return temp;
        }

        public static double Norm(this Vector3d vector)
        {
            return Convert.ToSingle(Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z));
        }
        public static Vector3d NormalizeV(this Vector3d vector)
        {
            double den = vector.Norm();
            if (den != 0)
            {
                vector.X /= den;
                vector.Y /= den;
                vector.Z /= den;

            }
            return vector;

        }
        //public static Vector3d Normalize(this Vector3d vector)
        //{
        //    double den = vector.Norm();
        //    if (den != 0)
        //    {
        //        vector.X /= den;
        //        vector.Y /= den;
        //        vector.Z /= den;

        //    }
        //    return vector;

        //}
        public static void Normalize(this Vertex vertex)
        {
            float den = vertex.Vector.Norm();
            if (den != 0)
            {
                vertex.Vector.X /= den;
                vertex.Vector.Y /= den;
                vertex.Vector.Z /= den;

            }

        }

     
     // converts cartesion to polar coordinates
     // result:
     // [0] = length
     // [1] = angle with z-axis
     // [2] = angle of projection into x,y, plane with x-axis
     //
        public static Vector3d CartesianToPolar(this Vector3d v)
        {
            Vector3d polar = new Vector3d();

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
                polar[2] = (double) Convert.ToSingle(Math.Atan(v[1] / v[0]));
            }
            else if (v[0] < 0.0f)
            {
                polar[2] = (double) Convert.ToSingle(Math.Atan(v[1] / v[0]) + Math.PI);
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
        public static Vector3d PolarToCartesian(this Vector3d v)
        {
            Vector3d cart = new Vector3d();
            cart[0] = Convert.ToSingle(v[0] * Math.Sin(v[1]) * (double)Math.Cos(v[2]));
            cart[1] = Convert.ToSingle(v[0] * Math.Sin(v[1]) * (double)Math.Sin(v[2]));
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
        static Vector3d ProjectOntoVector(this Vector3d v1, Vector3d v2)
        {
            return v2 * Vector3d.Dot(v1, v2);
        }


        public static Vector3d ProjectVectorIntoPlane(this Vector3d v1, Vector3d normalOfPlane)
        {
            return v1 - ProjectOntoVector(v1, normalOfPlane);
        }


        public static Vector3d ProjectPointOntoPlane(this Vector3d point, Vector3d anchor, Vector3d normal)
        {
            Vector3d temp = point - anchor;
            return point - ProjectOntoVector(temp, normal);
        }
        public static double AngleInRadians(this Vector3d a, Vector3d b)
        {
            if (a.Equals(Vector3d.Zero) || b.Equals(Vector3d.Zero))
                return 360;

            Vector3d v = Vector3d.Cross(a, b);
            double d1 = v.Norm();

            double d2 = Vector3d.Dot(a, b);
            double angle =  Convert.ToSingle(Math.Atan2(d1, d2));
            

            ////-----------------------------------
            ////alternative
            //double dot = Vector3d.Dot(a, b);
            //// Divide the dot by the product of the magnitudes of the vectors
            //dot = dot / (a.Norm() * b.Norm());
            ////Get the arc cosin of the angle, you now have your angle in radians 
            //double acos = Math.Acos(dot);
            ////Multiply by 180/Mathf.PI to convert to degrees
            //double angleCheck = acos * 180 / Math.PI;
            ////-----------------------------

            //if (angle - angleCheck > 0.1)
            //    System.Windows.Forms.MessageBox.Show("SW Check Angle");

            return angle;




        }
        public static float AngleInDegrees(this Vector3d a, Vector3d b)
        {
            if(a.Equals(Vector3d.Zero) || b.Equals(Vector3d.Zero))
                return 360;

            Vector3d v = Vector3d.Cross(a, b);
            double d1 = v.Norm();

            double d2 = Vector3d.Dot(a, b);
            float angle =  Convert.ToSingle(Math.Atan2(d1, d2));
            angle =Convert.ToSingle( angle * 180 / Math.PI);

            ////-----------------------------------
            ////alternative
            //double dot = Vector3d.Dot(a, b);
            //// Divide the dot by the product of the magnitudes of the vectors
            //dot = dot / (a.Norm() * b.Norm());
            ////Get the arc cosin of the angle, you now have your angle in radians 
            //double acos = Math.Acos(dot);
            ////Multiply by 180/Mathf.PI to convert to degrees
            //double angleCheck = acos * 180 / Math.PI;
            ////-----------------------------

            //if (angle - angleCheck > 0.1)
            //    System.Windows.Forms.MessageBox.Show("SW Check Angle");

            return angle;


        }
        public static double Distance(this Vector3d vector, Vector3d vOther)
        {
            double fSum = 0;
            for (int i = 0; i < 3; i++)
            {
                double fDifference = (vector[i] - vOther[i]);
                fSum += fDifference * fDifference;
            }
            return Convert.ToSingle(System.Math.Sqrt(fSum));
        }
     
        public static Vector3d FromFloatArray(this Vector3d v, double[] arr)
        {
         
            
            for (int i = 0; i < arr.Length; i++)
            {
                v[i] = arr[i];
            }
            return v;


        }
        public static Vector3d FromDoubleArray(this Vector3d v, double[] arr)
        {


            for (int i = 0; i < arr.Length; i++)
            {
                v[i] = Convert.ToSingle(arr[i]);
            }
            return v;


        }
        public static Vector3d Abs(this Vector3d v)
        {

            Vector3d vAbs = new Vector3d();
            for (int i = 0; i < 3; i++)
            {
                vAbs[i] = Math.Abs(v[i]);
            }
            return vAbs;


        }
        public static bool IsZero(this Vector3d v)
        {

            if(v.X == 0 && v.Y == 0 && v.Z == 0)
                return true;

            return false;


        }
        public static void Print(this Vector3d v, string name)
        {

            System.Diagnostics.Debug.WriteLine(name + " : " + v[0].ToString("0.00") + " " + v[1].ToString("0.00") + " " + v[2].ToString("0.00"));

         
        }


        public static Vector3d CrossProduct(this Vector3d v1, Vector3d v2)
        {
            return new Vector3d()
            {
                X = v1.Y * v2.Z - v2.Y * v1.Z,
                Y = -v1.X * v2.Z + v2.X * v1.Z,
                Z = v1.X * v2.Y - v2.X * v1.Y
            };
        }

        public static double NormSquared(this Vector3d v)
        {

            double val = v.X * v.X + v.Y * v.Y + v.Z * v.Z;

            return val;
        }
        public static Vector3d Up (this Vector3d v)
        {
            return new Vector3d(0.0f, 1.0f, 0.0f); 
        }

        public static Vector3d Down(this Vector3d v)
        {
            return new Vector3d(0.0f, -1.0f, 0.0f); 
        }
        public static Vector3 ToVector(this Vector3d v)
        {
            return new Vector3((float)v.X, (float)v.Y, (float)v.Z);


        }
        public static Vector3d Forward(this Vector3d v)
        {
            return new Vector3d(0.0f, 0.0f, -1.0f); 
        }

        public static Vector3d Backward(this Vector3d v)
        {
            return new Vector3d(0.0f, 0.0f, 1.0f); 
        }

        public static Vector3d Left(this Vector3d v)
        {
            return new Vector3d(-1.0f, 0.0f, 0.0f); 
        }

        public static Vector3d Right(this Vector3d v)
        {
            return new Vector3d(1.0f, 0.0f, 0.0f); 
        }
    }
}
