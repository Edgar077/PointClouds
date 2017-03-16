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
    /// Represents a 3x3 matrix.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct Matrix33<T> : IEquatable<Matrix33<T>>
        where T : IEquatable<T>
    {
        /// <summary>
        ///  The matrix components.
        /// </summary>
        Numeric<T>[][] x;

        /// <summary>
        /// Get component at index i, j
        /// </summary>
        /// <param name="index"></param>
        /// <returns>M[i, j]</returns>
        public Numeric<T>[] this[int index]
        {
            get
            {
                return x[index];
            }
        }

        /// <summary>
        /// Constructor M[i][j] = a
        /// </summary>
        /// <param name="a"></param>
        public Matrix33(T a)
        {
            x = new Numeric<T>[3][];

            for (int i = 0; i < 3; i++)
                x[i] = new Numeric<T>[3];

            x[0][0] = a;
            x[0][1] = a;
            x[0][2] = a;
            x[1][0] = a;
            x[1][1] = a;
            x[1][2] = a;
            x[2][0] = a;
            x[2][1] = a;
            x[2][2] = a;
        }

        /// <summary>
        /// Constructor M = a
        /// </summary>
        /// <param name="a"></param>
        public Matrix33(T[][] a)
        {
            x = new Numeric<T>[3][];

            for (int i = 0; i < 3; i++)
                x[i] = new Numeric<T>[3];

            if (a.Length != 3)
                return;

            if (a[0].Length != 3) return;
            if (a[1].Length != 3) return;
            if (a[2].Length != 3) return;

            x[0][0] = a[0][0];
            x[0][1] = a[0][1];
            x[0][2] = a[0][2];
            x[1][0] = a[1][0];
            x[1][1] = a[1][1];
            x[1][2] = a[1][2];
            x[2][0] = a[2][0];
            x[2][1] = a[2][1];
            x[2][2] = a[2][2];
        }

        public Matrix33(T a, T b, T c, T d, T e, T f, T g, T h, T i)
        {
            x = new Numeric<T>[3][];

            for (int k = 0; k < 3; k++)
                x[k] = new Numeric<T>[3];

            x[0][0] = a;
            x[0][1] = b;
            x[0][2] = c;
            x[1][0] = d;
            x[1][1] = e;
            x[1][2] = f;
            x[2][0] = g;
            x[2][1] = h;
            x[2][2] = i;
        }

        Matrix33(Numeric<T> a, Numeric<T> b, Numeric<T> c, Numeric<T> d, Numeric<T> e, Numeric<T> f, Numeric<T> g, Numeric<T> h, Numeric<T> i)
        {
            x = new Numeric<T>[3][];

            for (int k = 0; k < 3; k++)
                x[k] = new Numeric<T>[3];

            x[0][0] = a;
            x[0][1] = b;
            x[0][2] = c;
            x[1][0] = d;
            x[1][1] = e;
            x[1][2] = f;
            x[2][0] = g;
            x[2][1] = h;
            x[2][2] = i;
        }

        public Matrix33(Matrix33<T> v)
        {
            x = new Numeric<T>[3][];

            for (int k = 0; k < 3; k++)
                x[k] = new Numeric<T>[3];

            x[0][0] = v.x[0][0];
            x[0][1] = v.x[0][1];
            x[0][2] = v.x[0][2];
            x[1][0] = v.x[1][0];
            x[1][1] = v.x[1][1];
            x[1][2] = v.x[1][2];
            x[2][0] = v.x[2][0];
            x[2][1] = v.x[2][1];
            x[2][2] = v.x[2][2];
        }

        public Matrix33<T> SetTheMatrix(Matrix33<T> v)
        {
            x[0][0] = v.x[0][0];
            x[0][1] = v.x[0][1];
            x[0][2] = v.x[0][2];
            x[1][0] = v.x[1][0];
            x[1][1] = v.x[1][1];
            x[1][2] = v.x[1][2];
            x[2][0] = v.x[2][0];
            x[2][1] = v.x[2][1];
            x[2][2] = v.x[2][2];
            return this;
        }

        public void MakeIdentity(T unit)
        {
            x[0][0] = unit;
            x[0][1] = Numeric<T>.Zero();
            x[0][2] = Numeric<T>.Zero();
            x[1][0] = Numeric<T>.Zero();
            x[1][1] = unit;
            x[1][2] = Numeric<T>.Zero();
            x[2][0] = Numeric<T>.Zero();
            x[2][1] = Numeric<T>.Zero();
            x[2][2] = unit;
        }

        public static Matrix33<T> Identity(T unit)
        {
            Matrix33<T> m = new Matrix33<T>(Numeric<T>.Zero());
            m.MakeIdentity(unit);

            return m;
        }

        /// <summary>
        /// Component-wise addition
        /// </summary>
        /// <param name="v"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Matrix33<T> operator +(Matrix33<T> v, Matrix33<T> v2)
        {
            return new Matrix33<T>(v2[0][0] + v.x[0][0],
                                     v2[0][1] + v.x[0][1],
                                     v2[0][2] + v.x[0][2],
                                     v2[1][0] + v.x[1][0],
                                     v2[1][1] + v.x[1][1],
                                     v2[1][2] + v.x[1][2],
                                     v2[2][0] + v.x[2][0],
                                     v2[2][1] + v.x[2][1],
                                     v2[2][2] + v.x[2][2]);
        }

        /// <summary>
        /// Component-wise subtraction
        /// </summary>
        /// <param name="v2"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Matrix33<T> operator -(Matrix33<T> v2, Matrix33<T> v)
        {
            return new Matrix33<T>(v2[0][0] - v.x[0][0],
                                     v2[0][1] - v.x[0][1],
                                     v2[0][2] - v.x[0][2],
                                     v2[1][0] - v.x[1][0],
                                     v2[1][1] - v.x[1][1],
                                     v2[1][2] - v.x[1][2],
                                     v2[2][0] - v.x[2][0],
                                     v2[2][1] - v.x[2][1],
                                     v2[2][2] - v.x[2][2]);
        }

        /// <summary>
        /// Component-wise inversion
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Matrix33<T> operator -(Matrix33<T> v)
        {
            return new Matrix33<T>(-v.x[0][0],
                                     -v.x[0][1],
                                     -v.x[0][2],
                                     -v.x[1][0],
                                     -v.x[1][1],
                                     -v.x[1][2],
                                     -v.x[2][0],
                                     -v.x[2][1],
                                     -v.x[2][2]);
        }

        /// <summary>
        /// Component-wise inversion
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public Matrix33<T> Negate()
        {
            x[0][0] = -x[0][0];
            x[0][1] = -x[0][1];
            x[0][2] = -x[0][2];
            x[1][0] = -x[1][0];
            x[1][1] = -x[1][1];
            x[1][2] = -x[1][2];
            x[2][0] = -x[2][0];
            x[2][1] = -x[2][1];
            x[2][2] = -x[2][2];

            return this;
        }

        /// <summary>
        /// Component-wise multiplication
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Matrix33<T> operator *(Matrix33<T> v2, T c)
        {
            return new Matrix33<T>(v2[0][0] * c,
                                 v2[0][1] * c,
                                 v2[0][2] * c,
                                 v2[1][0] * c,
                                 v2[1][1] * c,
                                 v2[1][2] * c,
                                 v2[2][0] * c,
                                 v2[2][1] * c,
                                 v2[2][2] * c);
        }

        /// <summary>
        /// Component-wise multiplication
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Matrix33<T> operator *(T c, Matrix33<T> v2)
        {
            return new Matrix33<T>(v2[0][0] * c,
                                 v2[0][1] * c,
                                 v2[0][2] * c,
                                 v2[1][0] * c,
                                 v2[1][1] * c,
                                 v2[1][2] * c,
                                 v2[2][0] * c,
                                 v2[2][1] * c,
                                 v2[2][2] * c);
        }

        /// <summary>
        /// Matrix multiplication
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Matrix33<T> operator *(Matrix33<T> v2, Matrix33<T> v)
        {
            Matrix33<T> tmp = new Matrix33<T>(Numeric<T>.Zero());

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    for (int k = 0; k < 3; k++)
                        tmp.x[i][j] += v2.x[i][k] * v.x[k][j];

            return tmp;
        }

        /// <summary>
        /// Vector-matrix multiplication
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public void MultVecMatrix(Vector2<T> src, ref Vector2<T> dst)
        {
            Numeric<T> a, b, w;

            a = src[0] * x[0][0] + src[1] * x[1][0] + x[2][0];
            b = src[0] * x[0][1] + src[1] * x[1][1] + x[2][1];
            w = src[0] * x[0][2] + src[1] * x[1][2] + x[2][2];

            dst.X = a / w;
            dst.Y = b / w;
        }

        /// <summary>
        /// Vector-matrix multiplication
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public void MultDirMatrix(Vector2<T> src, ref Vector2<T> dst)
        {
            Numeric<T> a, b;

            a = src[0] * x[0][0] + src[1] * x[1][0];
            b = src[0] * x[0][1] + src[1] * x[1][1];

            dst.X = a;
            dst.Y = b;
        }

        /// <summary>
        /// Component-wise division
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Matrix33<T> operator /(Matrix33<T> v2, Matrix33<T> v)
        {
            return new Matrix33<T>(v2[0][0] / v.x[0][0],
                                 v2[0][1] / v.x[0][1],
                                 v2[0][2] / v.x[0][2],
                                 v2[1][0] / v.x[1][0],
                                 v2[1][1] / v.x[1][1],
                                 v2[1][2] / v.x[1][2],
                                 v2[2][0] / v.x[2][0],
                                 v2[2][1] / v.x[2][1],
                                 v2[2][2] / v.x[2][2]);
        }

        /// <summary>
        /// Component-wise division
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Matrix33<T> operator /(Matrix33<T> v2, T c)
        {
            return new Matrix33<T>(v2[0][0] / c,
                                 v2[0][1] / c,
                                 v2[0][2] / c,
                                 v2[1][0] / c,
                                 v2[1][1] / c,
                                 v2[1][2] / c,
                                 v2[2][0] / c,
                                 v2[2][1] / c,
                                 v2[2][2] / c);
        }

        /// <summary>
        /// Multiply vector-matrix
        /// </summary>
        /// <param name="v"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static Vector2<T> operator *(Vector2<T> v, Matrix33<T> m)
        {
            Numeric<T> x = (v.X * m[0][0] + v.Y * m[1][0] + m[2][0]);
            Numeric<T> y = (v.X * m[0][1] + v.Y * m[1][1] + m[2][1]);
            Numeric<T> w = (v.X * m[0][2] + v.Y * m[1][2] + m[2][2]);

            return new Vector2<T>(x / w, y / w);
        }

        /// <summary>
        /// Multiply vector-matrix
        /// </summary>
        /// <param name="v"></param>
        /// <param name="m"></param>
        public static Vector3<T> operator *(Vector3<T> v, Matrix33<T> m)
        {
            Numeric<T> x = (v.X * m[0][0] + v.Y * m[1][0] + v.Z * m[2][0]);
            Numeric<T> y = (v.X * m[0][1] + v.Y * m[1][1] + v.Z * m[2][1]);
            Numeric<T> z = (v.X * m[0][2] + v.Y * m[1][2] + v.Z * m[2][2]);

            return new Vector3<T>(x, y, z);
        }

        /// <summary>
        /// Transpose the matrix
        /// </summary>
        /// <returns></returns>
        public Matrix33<T>
        Transpose()
        {
            Matrix33<T> tmp = new Matrix33<T>(x[0][0],
                                           x[1][0],
                                           x[2][0],
                                           x[0][1],
                                           x[1][1],
                                           x[2][1],
                                           x[0][2],
                                           x[1][2],
                                           x[2][2]);

            this = tmp;
            return this;
        }

        /// <summary>
        /// Get the transposed matrix
        /// </summary>
        /// <returns></returns>
        public Matrix33<T>
        Transposed()
        {
            return new Matrix33<T>(x[0][0],
                                 x[1][0],
                                 x[2][0],
                                 x[0][1],
                                 x[1][1],
                                 x[2][1],
                                 x[0][2],
                                 x[1][2],
                                 x[2][2]);
        }

        /// <summary>
        /// Gauss–Jordan elimination
        /// </summary>
        /// <exception cref="MatrixNotInvertibleException">MatrixNotInvertibleException</exception>
        /// <returns></returns>
        public Matrix33<T>
        Invert(T unit)
        {
            this = Inverse(unit);
            return this;
        }

        /// <summary>
        /// Gauss–Jordan elimination
        /// </summary>
        /// <exception cref="MatrixNotInvertibleException">MatrixNotInvertibleException</exception>
        /// <returns></returns>
        public Matrix33<T>
        Inverse(T unit)
        {
            int i, j, k;
            Matrix33<T> s = Matrix33<T>.Identity(unit);
            Matrix33<T> t = new Matrix33<T>(this);

            // Forward elimination

            for (i = 0; i < 2; i++)
            {
                int pivot = i;

                Numeric<T> pivotsize = (t[i][i]);

                if (pivotsize < Numeric<T>.Zero())
                    pivotsize = -pivotsize;

                for (j = i + 1; j < 3; j++)
                {
                    Numeric<T> tmp = (t[j][i]);

                    if (tmp < Numeric<T>.Zero())
                        tmp = -tmp;

                    if (tmp > pivotsize)
                    {
                        pivot = j;
                        pivotsize = tmp;
                    }
                }

                if (pivotsize.Equals(Numeric<T>.Zero()))
                {
                    throw new Exception("Cannot invert singular matrix.");
                }

                if (pivot != i)
                {
                    for (j = 0; j < 3; j++)
                    {
                        T tmp;

                        tmp = t[i][j];
                        t[i][j] = t[pivot][j];
                        t[pivot][j] = tmp;

                        tmp = s[i][j];
                        s[i][j] = s[pivot][j];
                        s[pivot][j] = tmp;
                    }
                }

                for (j = i + 1; j < 3; j++)
                {
                    T f = t[j][i] / t[i][i];

                    for (k = 0; k < 3; k++)
                    {
                        t[j][k] -= f * t[i][k];
                        s[j][k] -= f * s[i][k];
                    }
                }
            }

            // Backward substitution

            for (i = 2; i >= 0; --i)
            {
                Numeric<T> f;

                if ((f = t[i][i]).Equals(Numeric<T>.Zero()))
                {
                    throw new Exception("Cannot invert singular matrix.");
                }

                for (j = 0; j < 3; j++)
                {
                    t[i][j] /= f;
                    s[i][j] /= f;
                }

                for (j = 0; j < i; j++)
                {
                    f = t[j][i];

                    for (k = 0; k < 3; k++)
                    {
                        t[j][k] -= f * t[i][k];
                        s[j][k] -= f * s[i][k];
                    }
                }
            }

            return s;
        }

        /// <summary>
        /// Set matrix to rotation by angle
        /// </summary>
        /// <param name="angle">angle of rotation in radians</param>
        /// <returns></returns>
        public Matrix33<T> SetRotation(T angle, T unit)
        {
            Numeric<T> cos_r, sin_r;

            cos_r = GenericMath.Cos(angle, unit);
            sin_r = GenericMath.Sin(angle, unit);

            x[0][0] = cos_r;
            x[0][1] = sin_r;
            x[0][2] = Numeric<T>.Zero();

            x[1][0] = -sin_r;
            x[1][1] = cos_r;
            x[1][2] = Numeric<T>.Zero();

            x[2][0] = Numeric<T>.Zero();
            x[2][1] = Numeric<T>.Zero();
            x[2][2] = unit;

            return this;
        }

        /// <summary>
        /// Set matrix to rotation by angle
        /// </summary>
        /// <param name="angle">angle of rotation in radians</param>
        /// <returns></returns>
        public Matrix33<T> Rotate(T angle, T unit)
        {
            this = this * Matrix33<T>.Identity(unit).SetRotation(angle, unit);
            return this;
        }

        /// <summary>
        /// Set matrix to scale by s
        /// </summary>
        /// <param name="s">The uniform factor</param>
        /// <returns></returns>
        public Matrix33<T>
        SetScale(T s, T unit)
        {
            x[0][0] = s;
            x[0][1] = Numeric<T>.Zero();
            x[0][2] = Numeric<T>.Zero();

            x[1][0] = Numeric<T>.Zero();
            x[1][1] = s;
            x[1][2] = Numeric<T>.Zero();

            x[2][0] = Numeric<T>.Zero();
            x[2][1] = Numeric<T>.Zero();
            x[2][2] = unit;

            return this;
        }

        /// <summary>
        /// Set matrix to scale by s
        /// </summary>
        public Matrix33<T> SetScale(Vector2<T> s, T unit)
        {
            x[0][0] = s[0];
            x[0][1] = Numeric<T>.Zero();
            x[0][2] = Numeric<T>.Zero();

            x[1][0] = Numeric<T>.Zero();
            x[1][1] = s[1];
            x[1][2] = Numeric<T>.Zero();

            x[2][0] = Numeric<T>.Zero();
            x[2][1] = Numeric<T>.Zero();
            x[2][2] = unit;

            return this;
        }

        /// <summary>
        /// Set matrix to scale by s
        /// </summary>
        public Matrix33<T> Scale(Vector2<T> s)
        {
            x[0][0] *= s[0];
            x[0][1] *= s[0];
            x[0][2] *= s[0];

            x[1][0] *= s[1];
            x[1][1] *= s[1];
            x[1][2] *= s[1];

            return this;
        }

        /// <summary>
        /// Set matrix to translation by t
        /// </summary>
        public Matrix33<T> SetTranslation(Vector2<T> t, T unit)
        {
            x[0][0] = unit;
            x[0][1] = Numeric<T>.Zero();
            x[0][2] = Numeric<T>.Zero();

            x[1][0] = Numeric<T>.Zero();
            x[1][1] = unit;
            x[1][2] = Numeric<T>.Zero();

            x[2][0] = t[0];
            x[2][1] = t[1];
            x[2][2] = unit;

            return this;
        }

        /// <summary>
        /// Get matrix translation components
        /// </summary>
        public Vector2<T>
        Translation()
        {
            return new Vector2<T>(x[2][0], x[2][1]);
        }

        /// <summary>
        /// translate by t
        /// </summary>
        public Matrix33<T> Translate(Vector2<T> t)
        {
            x[2][0] += t[0] * x[0][0] + t[1] * x[1][0];
            x[2][1] += t[0] * x[0][1] + t[1] * x[1][1];
            x[2][2] += t[0] * x[0][2] + t[1] * x[1][2];

            return this;
        }

        public Matrix33<T> SetShear(T xy, T unit)
        {
            x[0][0] = unit;
            x[0][1] = Numeric<T>.Zero();
            x[0][2] = Numeric<T>.Zero();

            x[1][0] = xy;
            x[1][1] = unit;
            x[1][2] = Numeric<T>.Zero();

            x[2][0] = Numeric<T>.Zero();
            x[2][1] = Numeric<T>.Zero();
            x[2][2] = unit;

            return this;
        }

        public Matrix33<T> SetShear(Vector2<T> h, T unit)
        {
            x[0][0] = unit;
            x[0][1] = h[1];
            x[0][2] = Numeric<T>.Zero();

            x[1][0] = h[0];
            x[1][1] = unit;
            x[1][2] = Numeric<T>.Zero();

            x[2][0] = Numeric<T>.Zero();
            x[2][1] = Numeric<T>.Zero();
            x[2][2] = unit;

            return this;
        }

        public Matrix33<T> Shear(T xy)
        {
            x[1][0] += xy * x[0][0];
            x[1][1] += xy * x[0][1];
            x[1][2] += xy * x[0][2];

            return this;
        }

        public Matrix33<T> Shear(Vector2<T> h)
        {
            Matrix33<T> P = new Matrix33<T>(this);

            x[0][0] = P[0][0] + h[1] * P[1][0];
            x[0][1] = P[0][1] + h[1] * P[1][1];
            x[0][2] = P[0][2] + h[1] * P[1][2];

            x[1][0] = P[1][0] + h[0] * P[0][0];
            x[1][1] = P[1][1] + h[0] * P[0][1];
            x[1][2] = P[1][2] + h[0] * P[0][2];

            return this;
        }

        #region Equality

        public override bool Equals(object obj)
        {
            if (obj != null && obj is Matrix33<T>)
            {
                Matrix33<T> other = (Matrix33<T>)obj;
                return Equals(ref this, ref other);
            }
            return base.Equals(obj);
        }

        public bool Equals(Matrix33<T> other)
        {
            return Equals(this, other);
        }

        public static bool operator ==(Matrix33<T> v1, Matrix33<T> v2)
        {
            return Equals(ref v1, ref v2);
        }

        public static bool operator !=(Matrix33<T> v1, Matrix33<T> v2)
        {
            return !Equals(ref v1, ref v2);
        }

        private static bool Equals(ref Matrix33<T> v1, ref Matrix33<T> v2)
        {
            return (EqualityComparer<T>.Default.Equals(v1.x[0][0], v2.x[0][0]) &&
                    EqualityComparer<T>.Default.Equals(v1.x[0][1], v2.x[0][1]) &&
                    EqualityComparer<T>.Default.Equals(v1.x[0][2], v2.x[0][2]) &&
                    EqualityComparer<T>.Default.Equals(v1.x[1][0], v2.x[1][0]) &&
                    EqualityComparer<T>.Default.Equals(v1.x[1][1], v2.x[1][1]) &&
                    EqualityComparer<T>.Default.Equals(v1.x[1][2], v2.x[1][2]) &&
                    EqualityComparer<T>.Default.Equals(v1.x[2][0], v2.x[2][0]) &&
                    EqualityComparer<T>.Default.Equals(v1.x[2][1], v2.x[2][1]) &&
                    EqualityComparer<T>.Default.Equals(v1.x[2][2], v2.x[2][2]));
        }

        public override int GetHashCode()
        {
            int hashCode = 67;

            hashCode = hashCode * 71 + x[0][0].GetHashCode();
            hashCode = hashCode * 71 + x[0][1].GetHashCode();
            hashCode = hashCode * 71 + x[0][2].GetHashCode();
            hashCode = hashCode * 71 + x[1][0].GetHashCode();
            hashCode = hashCode * 71 + x[1][1].GetHashCode();
            hashCode = hashCode * 71 + x[1][2].GetHashCode();
            hashCode = hashCode * 71 + x[2][0].GetHashCode();
            hashCode = hashCode * 71 + x[2][1].GetHashCode();
            hashCode = hashCode * 71 + x[2][2].GetHashCode();

            return hashCode;
        }

        #endregion

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder();

            strBuilder.Append("{");

            for (int k = 0; k < 3; k++)
            {
                strBuilder.Append(" {");

                for (int l = 0; l < 3; l++)
                {
                    if (l != 0)
                        strBuilder.Append(", ");
                    strBuilder.Append(x[k][l]);
                }

                strBuilder.Append("} ");
            }

            strBuilder.Append("}");

            return strBuilder.ToString();
        }
    }
}
