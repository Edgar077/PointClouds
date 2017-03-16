using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;

namespace OpenTKExtension.MathNew
{
    public class Math
    {

        public static float SqrtBabylon(float n)
        {
            /*We are using n itself as initial approximation
             This can definitely be improved */
            float x = n;
            float y = 1;
            float e = 0.1f; /* e decides the accuracy level*/
            while (x - y > e)
            {
                x = (x + y) / 2;
                y = n / x;
            }
            return x;
        }
     
        public static float  Sqrt_9(float fg)
        {
            float n = fg / 2f;
            float lstX = 0f;
            while (n != lstX)
            {
                lstX = n;
                n = (n + fg / n) / 2f;
            }
            return n;
        }
        public static float Sqrt_Approx(float z)
        {
            if (z == 0) return 0;
            FloatIntUnion u;
            u.tmp = 0;
            u.f = z;
            u.tmp -= 1 << 23; /* Subtract 2^m. */
            u.tmp >>= 1; /* Divide by 2. */
            u.tmp += 1 << 29; /* Add ((b + 1) / 2) * 2^m. */
            return u.f;
        }

    

        //public unsafe static float Sqrt_Float(float number)
        //{
        //    long i;
        //    float x, y;
        //    const float f = 1.5F;

        //    x = number * 0.5F;
        //    y = number;
        //    i = *(long*)&y;
        //    i = 0x5f3759df - (i >> 1);
        //    y = *(float*)&i;
        //    y = y * (f - (x * y * y));
        //    y = y * (f - (x * y * y));
        //    return number * y;
        //}
        public static float Sqrt_Approx2(float z)
        {
            if (z == 0) return 0;
            FloatIntUnion u;
            u.tmp = 0;
            float xhalf = 0.5f * z;
            u.f = z;
            u.tmp = 0x5f375a86 - (u.tmp >> 1);
            u.f = u.f * (1.5f - xhalf * u.f * u.f);
            return u.f * z;
        }
        [StructLayout(LayoutKind.Explicit)]
        private struct FloatIntUnion
        {
            [FieldOffset(0)]
            public float f;

            [FieldOffset(0)]
            public int tmp;
        }
    }
}
