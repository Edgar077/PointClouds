using System;
using System.Runtime.InteropServices;
using OpenTK;

namespace OpenTKExtension
{

    public static class QuaternionExtension
    {
        public static Vector3 QuaternionToYawPitchRoll(this OpenTK.Vector4 q)
        {
            const float Epsilon = 0.0009765625f;
            const float Threshold = 0.5f - Epsilon;

            float yaw;
            float pitch;
            float roll;

            float XY = q.X * q.Y;
            float ZW = q.Z * q.W;

            float TEST = XY + ZW;

            if (TEST < -Threshold || TEST > Threshold)
            {

                int sign = Math.Sign(TEST);

                yaw = sign * 2 * (float)Math.Atan2(q.X, q.W);

                pitch = sign * (float)(Math.PI / 2.0f);

                roll = 0;

            }
            else
            {

                float XX = q.X * q.X;
                float XZ = q.X * q.Z;
                float XW = q.X * q.W;

                float YY = q.Y * q.Y;
                float YW = q.Y * q.W;
                float YZ = q.Y * q.Z;

                float ZZ = q.Z * q.Z;

                yaw = (float)Math.Atan2(2 * YW - 2 * XZ, 1 - 2 * YY - 2 * ZZ);

                pitch = (float)Math.Atan2(2 * XW - 2 * YZ, 1 - 2 * XX - 2 * ZZ);

                roll = (float)Math.Asin(2 * TEST);

            }//if 

            return new Vector3() { X = yaw, Y = pitch, Z = roll };

        }//method 
        /// <summary>Clone the vector
        /// </summary>
        /// <param name="vec">The vector to transform</param>
        /// <returns>The cloned vector</returns>
        public static Vector3 Clone(this Quaternion vector)
        {
            Vector3 vNew = new Vector3(vector.X, vector.Y, vector.Z);


            return vNew;

        }
    }
}
