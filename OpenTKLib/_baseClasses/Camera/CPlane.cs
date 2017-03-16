using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenTKExtension.FastGLControl
{
    public class CPlane
    {

        public Vector3 N = new Vector3();
        public float d;

        public enum Where
        {
            COPLANAR,
            FRONT,
            BACK
        }

        public static class GlobalMembersPlane
        {
            public const float EPSILON = 0.0001f;
        }


        public CPlane()
        {
            N = new Vector3(0, 1, 0);
            d = 0F;
        }
        public CPlane(Vector3 normal, Vector3 p)
        {
            N = normal;
            d = - Vector3.Dot(N, p);
        }
        public void Dispose()
        {
        }

        public static CPlane FromPoints(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            CPlane temp = new CPlane();
            Vector3 e1 = v2 - v1;
            Vector3 e2 = v3 - v1;
            Vector3 v = 
            temp.N = Vector3.Normalize(Vector3.Cross(e1, e2));
            temp.d = - Vector3.Dot(temp.N , v1);
            return temp;
        }
        public CPlane.Where Classify(Vector3 p)
        {
            float res = GetDistance(p);
            if (res > GlobalMembersPlane.EPSILON)
            {
                return Where.FRONT;
            }
            else if (res < GlobalMembersPlane.EPSILON)
            {
                return Where.BACK;
            }
            else
            {
                return Where.COPLANAR;
            }
        }
        public float GetDistance(Vector3 p)
        {
            return Vector3.Dot(N, p) + d;
        }


    }

}
