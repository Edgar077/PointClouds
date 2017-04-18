using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Kinect;
//using System.Windows.Media.Media3D;
using OpenTK;

//from https://bitbucket.org/nguyenivan/kinect2bvh.v2/src/d19ccd4e7631?at=master

namespace PointCloudUtils
{
    class MathHelper
    {

        public static float[] VectorToDeg(Microsoft.Kinect.Vector4 vec)
        {
            float[] value = new float[3];
            value[0] =(float) Math.Atan2(2 * (vec.W * vec.X + vec.Y * vec.Z), 1 - 2 * (Math.Pow(vec.X, 2) + Math.Pow(vec.Y, 2)));
            value[1] = (float)Math.Asin(2 * (vec.W * vec.Y - vec.Z * vec.X));
            value[2] = (float)Math.Atan2(2 * (vec.W * vec.Z + vec.X * vec.Y), 1 - 2 * (Math.Pow(vec.Y, 2) + Math.Pow(vec.Z, 2)));
            value[0] = value[0] * (180 / (float)Math.PI);
            value[1] = value[1] * (180 / (float)Math.PI);
            value[2] = value[2] * (180 / (float)Math.PI);
            return value;
        }
        public static float[] quat2Deg(Quaternion vec)
        {
            float[] value = new float[3];
            value[0] = (float)Math.Atan2(2 * (vec.W * vec.X + vec.Y * vec.Z), 1 - 2 * (Math.Pow(vec.X, 2) + Math.Pow(vec.Y, 2)));
            value[1] = (float)Math.Asin(2 * (vec.W * vec.Y - vec.Z * vec.X));
            value[2] = (float)Math.Atan2(2 * (vec.W * vec.Z + vec.X * vec.Y), 1 - 2 * (Math.Pow(vec.Y, 2) + Math.Pow(vec.Z, 2)));
            value[0] = value[0] * (180 / (float)Math.PI);
            value[1] = value[1] * (180 / (float)Math.PI);
            value[2] = value[2] * (180 / (float)Math.PI);
            return value;
        }
   
        public static Vector3 QuaternionToEuler(OpenTK.Vector4 q)
        {
            Vector3 v = Vector3.Zero;
            v.X =(float) Math.Atan2(2 * q.Y * q.W - 2 * q.X * q.Z,
                                    1 - 2 * Math.Pow(q.Y, 2) - 2 * Math.Pow(q.Z, 2));

            v.Z = (float)Math.Asin(2 * q.X * q.Y + 2 * q.Z * q.W);

            v.Y = (float)Math.Atan2(2 * q.X * q.W - 2 * q.Y * q.Z,
                                      1 - 2 * Math.Pow(q.X, 2) - 2 * Math.Pow(q.Z, 2));

            if (q.X * q.Y + q.Z * q.W == 0.5)
            {
                v.X = (float)(2 * Math.Atan2(q.X, q.W));
                v.Y = 0;
            }
            else if (q.X * q.Y + q.Z * q.W == -0.5)
            {
                v.X = (float)(-2 * Math.Atan2(q.X, q.W));
                v.Y = 0;
            }

            v.X = RadianToDegree(v.X);
            v.Y = RadianToDegree(v.Y);
            v.Z = RadianToDegree(v.Z);
            return v;
        }
        private static float RadianToDegree(float angle)
        {//Return degrees (0->360) from radians
            return angle * (float)(180.0 / Math.PI) + 180;
        }

       
    

        /*
        public static Quaternion Deg2Quat(float[] deg)
        {
            Quaternion quat = new Quaternion();
            float a = deg[0] * (Math.PI / 180);
            float b = deg[1] * (Math.PI / 180);
            float c = deg[2] * (Math.PI / 180);

            quat.W = Math.Cos(a / 2) * Math.Cos(b / 2) * Math.Cos(c / 2) + Math.Sin(a / 2) * Math.Sin(b / 2) * Math.Sin(c / 2);
            quat.X = Math.Sin(a / 2) * Math.Cos(b / 2) * Math.Cos(c / 2) - Math.Cos(a / 2) * Math.Sin(b / 2) * Math.Sin(c / 2);
            quat.Y = Math.Cos(a / 2) * Math.Sin(b / 2) * Math.Cos(c / 2) + Math.Sin(a / 2) * Math.Cos(b / 2) * Math.Sin(c / 2);
            quat.Z = Math.Cos(a / 2) * Math.Cos(b / 2) * Math.Sin(c / 2) - Math.Sin(a / 2) * Math.Sin(b / 2) * Math.Cos(c / 2);

            return quat;
        }
         * */



        public static Quaternion getQuaternion(Vector3 v0, Vector3 v1)
        {
            Quaternion q = new Quaternion();
            // Copy, since cannot modify local
            v0.Normalize();
            v1.Normalize();

            float d =  Vector3.Dot(v0, v1);
            // If dot == 1, vectors are the same
            if (d >= 1.0f)
            {
                return Quaternion.Identity;
            }

            float s =(float) Math.Sqrt((1 + d) * 2);
            float invs = 1 / s;

            Vector3 c = Vector3.Cross(v0, v1);

            q.X = c.X * invs;
            q.Y = c.Y * invs;
            q.Z = c.Z * invs;
            q.W = s * 0.5f;
            q.Normalize();

            return q;
        }

        public static Quaternion Vector4ToQuat(Microsoft.Kinect.Vector4 vec)
        {
            Quaternion quat = new Quaternion();
            quat.W = vec.W;
            quat.X = vec.X;
            quat.Y = vec.Y;
            quat.Z = vec.Z;
            return quat;
        }

    

        public static float[] quat2Deg(OpenTK.Vector4 vec)
        {
            float[] value = new float[3];
            value[0] = (float)Math.Atan2(2 * (vec.W * vec.X + vec.Y * vec.Z), 1 - 2 * (Math.Pow(vec.X, 2) + Math.Pow(vec.Y, 2)));
            value[1] = (float)Math.Asin(2 * (vec.W * vec.Y - vec.Z * vec.X));
            value[2] = (float)Math.Atan2(2 * (vec.W * vec.Z + vec.X * vec.Y), 1 - 2 * (Math.Pow(vec.Y, 2) + Math.Pow(vec.Z, 2)));
            value[0] = value[0] * (180 / (float)Math.PI);
            value[1] = value[1] * (180 / (float)Math.PI);
            value[2] = value[2] * (180 / (float)Math.PI);
            return value;
        }

        public static float[] addArray(float[] array1, float[] array2)
        {
            float[] result = new float[3]
            {
                array1[0] + array2[0],
                array1[1] + array2[1],
                array1[2] + array2[2]
            };
            return result;
        }

    }


}