// Copyright (c) 2011 Hamrouni Ghassen. All rights reserved.
//
// Portions of this library are based on Miscellaneous Utility Library by Jon Skeet
// Portions of this library are a port of IlmBase http://www.openexr.com/
//
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NLinear
{
    /// <summary>
    /// A 2-dimensional vector/point
    /// </summary>
    /// <typeparam name="T">A numeric type (float, int, ...)</typeparam>
    public struct Vector2<T> : IEquatable<Vector2<T>>
        where T : IEquatable<T>
    {
        /// <summary>
        ///  The vector components.
        /// </summary>
        Numeric<T> x, y;

        public T X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        public T Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        /// <summary>
        ///  Get component at index
        /// </summary>
        /// <param name="index"> Index between 0 and 1 </param>
        /// <returns></returns>
        public T this[int index]
        {
            get
            {
                if (index == 0) return x;

                return y;
            }
        }

        /// <summary>
        /// Constructor x = y = a
        /// </summary>
        /// <param name="a"></param>
        public Vector2(T a)
        {
            x = y = a;
        }

        /// <summary>
        /// Constructor (x y) = (a b)
        /// </summary>
        /// <param name="a"></param>
        public Vector2(T a, T b)
        {
            x = a;
            y = b;
        }

        /// <summary>
        /// The dot product.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public T Dot(Vector2<T> v)
        {
            return x * v.x + y * v.y;
        }

        /// <summary>
        /// The dot product.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static T operator ^(Vector2<T> c1, Vector2<T> c2)
        {
            return c1.Dot(c2);
        }

        /// <summary>
        /// Cross-product
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public T Cross(Vector2<T> v)
        {
            return x * v.y - y * v.x;
        }

        /// <summary>
        /// Cross-product
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static T operator %(Vector2<T> c1, Vector2<T> c2)
        {
            return c1.Cross(c2);
        }

        /// <summary>
        /// Component wise addition
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        public static Vector2<T> operator +(Vector2<T> c1, Vector2<T> c2)
        {
            Vector2<T> v = new Vector2<T>();

            v.x = c1.x + c2.x;
            v.y = c1.y + c2.y;

            return v;
        }

        /// <summary>
        /// Component wise subtraction
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        public static Vector2<T> operator -(Vector2<T> c1, Vector2<T> c2)
        {
            Vector2<T> v = new Vector2<T>();

            v.x = c1.x - c2.x;
            v.y = c1.y - c2.y;

            return v;
        }

        /// <summary>
        /// Component wise inversion -(x y) = (-x -y)
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        public static Vector2<T> operator -(Vector2<T> c1)
        {
            Vector2<T> v = new Vector2<T>();

            v.x = -c1.x;
            v.y = -c1.y;

            return v;
        }

        /// <summary>
        /// Component wise inversion -(x y) = (-x -y)
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        public Vector2<T> Negate()
        {
            x = -x;
            y = -y;

            return this;
        }

        /// <summary>
        /// Component wise multiplication
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        public static Vector2<T> operator *(Vector2<T> c1, Vector2<T> c2)
        {
            Vector2<T> v = new Vector2<T>();

            v.x = c1.x * c2.x;
            v.y = c1.y * c2.y;

            return v;
        }

        /// <summary>
        /// Component wise multiplication by a constant
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        public static Vector2<T> operator *(Vector2<T> c1, T c)
        {
            Vector2<T> v = new Vector2<T>();

            v.x = c1.x * c;
            v.y = c1.y * c;

            return v;
        }

        /// <summary>
        /// Component wise multiplication by a constant
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        public static Vector2<T> operator *(T c, Vector2<T> c1)
        {
            Vector2<T> v = new Vector2<T>();

            v.x = c1.x * c;
            v.y = c1.y * c;

            return v;
        }

        /// <summary>
        /// Component wise division
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static Vector2<T> operator /(Vector2<T> c1, Vector2<T> c2)
        {
            Vector2<T> v = new Vector2<T>();

            v.x = c1.x / c2.x;
            v.y = c1.y / c2.y;

            return v;
        }

        /// <summary>
        /// Component wise division by a constant
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static Vector2<T> operator /(Vector2<T> c1, T c)
        {
            Vector2<T> v = new Vector2<T>();

            v.x = c1.x / c;
            v.y = c1.y / c;

            return v;
        }

        /// <summary>
        /// Length of the vector
        /// </summary>
        /// <returns></returns>
        public T Length(T unit)
        {
            return GenericMath.Sqrt<T>(x * x + y * y, unit);
        }

        /// <summary>
        /// Squared Length of the vector
        /// </summary>
        /// <returns></returns>
        public T Length2()
        {
            return x * x + y * y;
        }

        /// <summary>
        /// Normalize the vector (x y) / Length( (x y) )
        /// </summary>
        /// <returns></returns>
        public Vector2<T> Normalize(T unit)
        {
            T l = Length(unit);

            Numeric<T> numL = l;

            if (!numL.Equals(Numeric<T>.Zero()))
            {
                x /= l;
                y /= l;
            }

            return this;
        }

        /// <summary>
        /// get a normalized vector (x y) / Length( (x y) )
        /// </summary>
        /// <returns></returns>
        public Vector2<T> Normalized(T unit)
        {
            T l = Length(unit);

            Numeric<T> numL = l;

            if (numL.Equals(Numeric<T>.Zero()))
                return new Vector2<T>(Numeric<T>.Zero());

            return new Vector2<T>(x / l, y / l);
        }

        #region Equality

        public override bool Equals(object obj)
        {
            if (obj != null && obj is Vector2<T>)
            {
                Vector2<T> other = (Vector2<T>)obj;
                return Equals(ref this, ref other);
            }
            return base.Equals(obj);
        }

        public bool Equals(Vector2<T> other)
        {
            return Equals(this, other);
        }

        public static bool operator ==(Vector2<T> v1, Vector2<T> v2)
        {
            return EqualityComparer<T>.Default.Equals(v1.X, v2.X)
                && EqualityComparer<T>.Default.Equals(v1.Y, v2.Y);
        }

        public static bool operator !=(Vector2<T> v1, Vector2<T> v2)
        {
            return !(EqualityComparer<T>.Default.Equals(v1.X, v2.X)
                && EqualityComparer<T>.Default.Equals(v1.Y, v2.Y));
        }

        private static bool Equals(ref Vector2<T> v1, ref Vector2<T> v2)
        {
            return EqualityComparer<T>.Default.Equals(v1.X, v2.X)
                && EqualityComparer<T>.Default.Equals(v1.Y, v2.Y);
        }

        public override int GetHashCode()
        {
            int hashCode = 67;

            hashCode = hashCode * 71 + x.GetHashCode();
            hashCode = hashCode * 71 + y.GetHashCode();

            return hashCode;
        }

        #endregion

        public override string ToString()
        {
            return String.Format("({0}, {1})", X, Y);
        }
    }
}
