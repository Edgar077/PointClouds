using System;
using System.Runtime.InteropServices;
using OpenTK;

namespace OpenTK.Extension
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct QuaternionNew : IEquatable<QuaternionNew>
    {
        public float X, Y, Z, W;

        #region Static Constructors
        public static QuaternionNew Zero
        {
            get { return new QuaternionNew(0, 0, 0, 0); }
        }

        public static QuaternionNew Identity
        {
            get { return new QuaternionNew(1, 0, 0, 0); }
        }
        #endregion

        #region Constructor
        public QuaternionNew(float x, float y, float z, float w)
        {
            this.X = x; this.Y = y; this.Z = z; this.W = w;
        }

        public QuaternionNew(Vector4 vec)
        {
            this.X = vec.X; this.Y = vec.Y; this.Z = vec.Z; this.W = vec.W;
        }
        #endregion

        #region Operators
        public static QuaternionNew operator +(QuaternionNew q1, QuaternionNew q2)
        {
            return new QuaternionNew(q1.X + q2.X, q1.Y + q2.Y, q1.Z + q2.Z, q1.W + q2.W);
        }

        public static QuaternionNew operator -(QuaternionNew q1, QuaternionNew q2)
        {
            return new QuaternionNew(q1.X - q2.X, q1.Y - q2.Y, q1.Z - q2.Z, q1.W - q2.W);
        }

        public static QuaternionNew operator -(QuaternionNew q)
        {
            return new QuaternionNew(-q.X, -q.Y, -q.Z, -q.W);
        }

        public static QuaternionNew operator *(QuaternionNew q, float s)
        {
            return new QuaternionNew(s * q.X, s * q.Y, s * q.Z, s * q.W);
        }

        public static QuaternionNew operator *(float s, QuaternionNew q)
        {
            return new QuaternionNew(s * q.X, s * q.Y, s * q.Z, s * q.W);
        }

        public static Vector3 operator *(QuaternionNew q, Vector3 v)
        {   // From nVidia SDK
            Vector3 t_uv, t_uuv;
            Vector3 t_qvec = new Vector3(q.X, q.Y, q.Z);
            t_uv = Vector3.Cross(t_qvec, v);
            t_uuv = Vector3.Cross(t_qvec, t_uv);
            t_uv *= 2.0f * q.W;
            t_uuv *= 2.0f;
            return v + t_uv + t_uuv;
        }

        public static QuaternionNew operator *(QuaternionNew q1, QuaternionNew q2)
        {
            return new QuaternionNew(
               q1.X * q2.W + q1.Y * q2.Z - q1.Z * q2.Y + q1.W * q2.X,
              -q1.X * q2.Z + q1.Y * q2.W + q1.Z * q2.X + q1.W * q2.Y,
               q1.X * q2.Y - q1.Y * q2.X + q1.Z * q2.W + q1.W * q2.Z,
              -q1.X * q2.X - q1.Y * q2.Y - q1.Z * q2.Z + q1.W * q2.W);
        }

        public static QuaternionNew operator /(QuaternionNew q, float scalar)
        {
            float invScalar = 1.0f / scalar;
            return new QuaternionNew(q.X * invScalar, q.Y + invScalar, q.Z * invScalar, q.W * invScalar);
        }

        public static QuaternionNew operator /(QuaternionNew q1, QuaternionNew q2)
        {
            return q1 * q2.Inverse();
        }

        public static bool operator ==(QuaternionNew q1, QuaternionNew q2)
        {
            return (q1.W == q2.W && q1.X == q2.X && q1.Y == q2.Y && q1.Z == q2.Z);
        }

        public static bool operator !=(QuaternionNew q1, QuaternionNew q2)
        {
            return !(q1.W == q2.W && q1.X == q2.X && q1.Y == q2.Y && q1.Z == q2.Z);
        }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return "{" + X + ", " + Y + ", " + Z + ", " + W + "}";
        }

        /// <summary>
        /// Parses a JSON stream and produces a QuaternionNew struct.
        /// </summary>
        public static QuaternionNew Parse(string text)
        {
            string[] split = text.Trim(new char[] { '{', '}' }).Split(',');
            if (split.Length != 4) return QuaternionNew.Identity;

            return new QuaternionNew(float.Parse(split[0]), float.Parse(split[1]), float.Parse(split[2]), float.Parse(split[3]));
        }

        public override bool Equals(object obj)
        {
            if (!(obj is QuaternionNew)) return false;

            return this.Equals((QuaternionNew)obj);
        }

        public bool Equals(QuaternionNew other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion

        #region Properties
        public Matrix4 Matrix4
        {
            get
            {
                return new Matrix4(
                    new Vector4(1.0f - 2.0f * (Y * Y + Z * Z), 2.0f * (X * Y - W * Z), 2.0f * (X * Z + W * Y), 0.0f),
                    new Vector4(2.0f * (X * Y + W * Z), 1.0f - 2.0f * (X * X + Z * Z), 2.0f * (Y * Z - W * X), 0.0f),
                    new Vector4(2.0f * (X * Z - W * Y), 2.0f * (Y * Z + W * X), 1.0f - 2.0f * (X * X + Y * Y), 0.0f),
                    new Vector4(0.0f, 0.0f, 0.0f, 1.0f));
            }
        }

        public float this[int a]
        {
            get { return (a == 0) ? X : (a == 1) ? Y : (a == 3) ? Z : W; }
            set { if (a == 0) X = value; else if (a == 1) Y = value; else if (a == 2) Z = value; else W = value; }
        }

        /// <summary>
        /// Gets the length of the current QuaternionNew.  sqrt(x^2 + y^2 + z^2 + w^2)
        /// </summary>
        public float Length
        {
            get { return (float)Math.Sqrt(X * X + Y * Y + Z * Z + W * W); }
        }

        /// <summary>
        /// Gets the squared length of the current QuaternionNew.  x^2 + y^2 + z^2 + w^2
        /// </summary>
        public float SquaredLength
        {
            get { return X * X + Y * Y + Z * Z + W * W; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the dot product of the current QuaternionNew with another.  Dot = x*q.x + y * q.y + z * q.z + w * q.w
        /// </summary>
        public float Dot(QuaternionNew q)
        {
            return (X * q.X) + (Y * q.Y) + (Z * q.Z) + (W * q.W);
        }

        /// <summary>
        /// Calculates the complex conjugate of this QuaternionNew.
        /// </summary>
        public QuaternionNew Conjugate()
        {
            return new QuaternionNew(-X, -Y, -Z, W);
        }

        /// <summary>
        /// Calculates the norm of this QuaternionNew.  Norm = x^2 + y^2 + z^2 + w^2
        /// </summary>
        public float Norm()
        {
            return X * X + Y * Y + Z * Z + W * W;
        }

        /// <summary>
        /// Calculates the inverse of this QuaternionNew.  Inverse = Conjugate(q) / Norm(q)
        /// </summary>
        public QuaternionNew Inverse()
        {
            return Conjugate() / Norm();
        }

        /// <summary>
        /// Normalizes this QuaternionNew.  Normalize = q / q.Length()
        /// </summary>
        private static QuaternionNew Normalize(QuaternionNew q)
        {
            return q / q.Length;
        }

        /// <summary>
        /// Normalizes this QuaternionNew.  Normalize = q / q.Length()
        /// </summary>
        public QuaternionNew Normalize()
        {
            return this / Length;
        }

        /// <summary>
        /// Takes the log of the QuaternionNew.
        /// </summary>
        public static QuaternionNew Log(QuaternionNew q)
        {
            float a = (float)Math.Acos(q.W);
            float sina = (float)Math.Sin(a);

            if (sina > 0)
                return new QuaternionNew(a * q.X / sina, a * q.Y / sina, a * q.Z / sina, 0.0f);
            return new QuaternionNew(q.X, q.Y, q.Z, 0.0f);
        }

        /// <summary>
        /// Takes the exp of the QuaternionNew.
        /// </summary>
        public static QuaternionNew Exp(QuaternionNew q)
        {
            float a = (float)Math.Sqrt(q.X * q.X + q.Y * q.Y + q.Z * q.Z);
            float sina = (float)Math.Sin(a);
            float cosa = (float)Math.Cos(a);

            if (a > 0)
                return new QuaternionNew(sina * q.X / a, sina * q.Y / a, sina * q.Z / a, cosa);
            return new QuaternionNew(q.X, q.Y, q.Z, cosa);
        }

        /// <summary>
        /// Calculates the linear interpolation between q1 and q2 at time t.
        /// </summary>
        /// <param name="q1">Source QuaternionNew</param>
        /// <param name="q2">Destination QuaternionNew</param>
        /// <param name="t">Time between 0 and 1</param>
        public static QuaternionNew Lerp(QuaternionNew q1, QuaternionNew q2, float t)
        {
            return Normalize(q1 + t * (q1 - q2));
        }

        /// <summary>
        /// Calculates the spherical linear interpolation between q1 and q2 at time t.
        /// </summary>
        /// <param name="q1">Source QuaternionNew</param>
        /// <param name="q2">Destination QuaternionNew</param>
        /// <param name="t">Time between 0 and 1</param>
        public static QuaternionNew Slerp(QuaternionNew q1, QuaternionNew q2, float t)
        {
            float c0, c1;
            float cos = q1.Dot(q2);
            float sign = 1;
            if (cos < 0.0f)
            {
                cos = -cos;
                sign = -1.0f;
            }

            if (cos < 1.0f - 1e-3f)
            {
                float angle = (float)Math.Acos(cos);
                //float invSin = 1.0f/Math.Trigonometry.Sin(angle);
                float invSin = 1.0f / (float)(Math.Sqrt(1.0f - cos * cos));
                c0 = (float)Math.Sin((1.0f - t) * angle) * invSin;
                c1 = (float)Math.Sin(t * angle) * invSin;
            }
            else
            {
                // If a is nearly the same as b we just linearly interpolate
                c0 = 1.0f - t;
                c1 = t;
            }

            QuaternionNew q = c0 * q1 + (sign * c1) * q2;
            return Normalize(q);
        }

        /// <summary>
        /// TODO: Document
        /// </summary>
        public static QuaternionNew Squad(QuaternionNew q1, QuaternionNew q2, QuaternionNew ta, QuaternionNew tb, float t)
        {
            float slerpT = 2.0f * t * (1.0f - t);
            QuaternionNew p = Slerp(q1, q2, t);
            QuaternionNew q = Slerp(ta, tb, t);
            return Slerp(p, q, slerpT);
        }

        /// <summary>
        /// TODO: Document
        /// </summary>
        public static QuaternionNew SimpleSquad(QuaternionNew prev, QuaternionNew q1,
          QuaternionNew q2, QuaternionNew post, float t)
        {

            if (prev.Dot(q1) < 0)
                q1 = -q1;
            if (q1.Dot(q2) < 0)
                q2 = -q2;
            if (q2.Dot(post) < 0)
                post = -post;

            QuaternionNew ta = Spline(prev, q1, q2);
            QuaternionNew tb = Spline(q1, q2, post);

            return Squad(q1, q2, ta, tb, t);
        }

        /// <summary>
        /// TODO: Document
        /// </summary>
        public static QuaternionNew Spline(QuaternionNew pre, QuaternionNew q, QuaternionNew post)
        {
            QuaternionNew cj = q.Conjugate();
            QuaternionNew e = q * Exp((Log(cj * pre) + Log(cj * post)) * -0.25f);
            return e;
        }

        /// <summary>
        /// Creates an orientation QuaternionNew using an angle and arbitrary axis.
        /// </summary>
        public static QuaternionNew FromAngleAxis(float Angle, Vector3 Axis)
        {
            if (Axis.LengthSquared == 0.0f)
                return Identity;

            return new QuaternionNew(new Vector4(Axis.Normalize() * (float)Math.Sin(Angle * 0.5f), (float)Math.Cos(Angle * 0.5f)));
        }

        ///// <summary>
        ///// Converts this quaternion to an axis representation.
        ///// </summary>
        ///// <returns>Three Vector3 structs in an array, representing this quaternion.</returns>
        //public Vector3[] ToAxis()
        //{
        //    Matrix4 rotationMatrix = this.Matrix4;
        //    return new Vector3[] { new Vector3(rotationMatrix[0].X, rotationMatrix[1].X, rotationMatrix[2].X),
        //        new Vector3(rotationMatrix[0].Y, rotationMatrix[1].Y, rotationMatrix[2].Y),
        //        new Vector3(rotationMatrix[0].Z, rotationMatrix[1].Z, rotationMatrix[2].Z) };
        //}

        /// <summary>
        /// Convert this instance to an axis-angle representation.
        /// </summary>
        /// <returns>A Vector4 that is the axis-angle representation of this quaternion.</returns>
        public Vector4 ToAxisAngle()
        {
            QuaternionNew q = this;
            if (q.W > 1.0f)
                q.Normalize();

            Vector4 result = new Vector4();

            result.W = 2.0f * (float)System.Math.Acos(q.W); // angle
            float den = (float)System.Math.Sqrt(1.0 - q.W * q.W);
            if (den > 0.0001f)
            {
                result.Xyz = new Vector3(q.X, q.Y, q.Z) / den;
            }
            else
            {
                // This occurs when the angle is zero. 
                // Not a problem: just set an arbitrary normalized axis.
                result.Xyz = Vector3.UnitX;
            }

            return result;
        }

        /// <summary>
        /// Creates an orientation QuaternionNew given the 3 axis.
        /// </summary>
        /// <param name="Axis">An array of 3 axis</param>
        public static QuaternionNew FromAxis(Vector3 xvec, Vector3 yvec, Vector3 zvec)
        {
            Matrix4 Rotation = new Matrix4(
                new Vector4(xvec.X, yvec.X, zvec.X, 0),
                new Vector4(xvec.Y, yvec.Y, zvec.Y, 0),
                new Vector4(xvec.Z, yvec.Z, zvec.Z, 0),
                Vector4.Zero);
            return FromRotationMatrix(Rotation);
        }

        private static readonly int[] rotationLookup = new int[] { 1, 2, 0 };

        /// <summary>
        /// Creates an orientation QuaternionNew from a target Matrix4 rotational matrix.
        /// </summary>
        public static QuaternionNew FromRotationMatrix(Matrix4 Rotation)
        {
            // Algorithm from Ken Shoemake's article in 1987 SIGGRAPH course notes
            // "QuaternionNew Calculus and Fast Animation"

            float t_trace = Rotation[0, 0] + Rotation[1, 1] + Rotation[2, 2];
            float t_root = 0.0f;

            if (t_trace > 0.0)
            {   // |w| > 1/2
                QuaternionNew t_return = QuaternionNew.Zero;
                t_root = (float)Math.Sqrt(t_trace + 1.0);
                t_return.W = 0.5f * t_root;
                t_root = 0.5f / t_root;
                t_return.X = (Rotation[2, 1] - Rotation[1, 2]) * t_root;
                t_return.Y = (Rotation[0, 2] - Rotation[2, 0]) * t_root;
                t_return.Z = (Rotation[1, 0] - Rotation[0, 1]) * t_root;
                return t_return;
            }
            else
            {   // |w| <= 1/2
                QuaternionNew t_return = QuaternionNew.Zero;

                int i = 0;
                if (Rotation[1, 1] > Rotation[0, 0]) i = 1;
                if (Rotation[2, 2] > Rotation[i, i]) i = 2;
                int j = rotationLookup[i];
                int k = rotationLookup[j];

                t_root = (float)Math.Sqrt(Rotation[i, i] - Rotation[j, j] - Rotation[k, k] + 1.0f);
                t_return[i] = 0.5f * t_root;
                t_root = 0.5f / t_root;
                t_return.W = (Rotation[k, j] - Rotation[j, k]) * t_root;
                t_return[j] = (Rotation[j, i] + Rotation[i, j]) * t_root;
                t_return[k] = (Rotation[k, i] + Rotation[i, k]) * t_root;
                return t_return;
            }
        }
        #endregion
    }
}
