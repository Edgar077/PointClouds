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
    public struct Shear6<T> : IEquatable<Shear6<T>>
        where T : IEquatable<T>
    {
        Numeric<T> xy, xz, yz, yx, zx, zy;

        public T XY
        {
            get
            {
                return xy;
            }
            set
            {
                xy = value;
            }
        }

        public T XZ
        {
            get
            {
                return xz;
            }
            set
            {
                xz = value;
            }
        }

        public T YZ
        {
            get
            {
                return yz;
            }
            set
            {
                yz = value;
            }
        }

        public T YX
        {
            get
            {
                return yx;
            }
            set
            {
                yx = value;
            }
        }

        public T ZX
        {
            get
            {
                return zx;
            }
            set
            {
                zx = value;
            }
        }

        public T ZY
        {
            get
            {
                return zy;
            }
            set
            {
                zy = value;
            }
        }

        // Indexer declaration
        public T this[int index]
        {
            get
            {
                if (index == 0) return xy;
                if (index == 1) return xz;
                if (index == 2) return yz;
                if (index == 3) return yx;
                if (index == 4) return zx;
               
                return zy;
            }
        }

        public Shear6(T XY, T XZ, T YZ)
        {
            xy = XY;
            xz = XZ;
            yz = YZ;
            yx = Numeric<T>.Zero();
            zx = Numeric<T>.Zero();
            zy = Numeric<T>.Zero();
        }

        public Shear6(Vector3<T> v)
        {
            xy = v.X;
            xz = v.Y;
            yz = v.Z;
            yx = Numeric<T>.Zero();
            zx = Numeric<T>.Zero();
            zy = Numeric<T>.Zero();
        }

        public Shear6(T XY, T XZ, T YZ, T YX, T ZX, T ZY)
        {
            xy = XY;
            xz = XZ;
            yz = YZ;
            yx = YX;
            zx = ZX;
            zy = ZY;
        }

        public Shear6(Shear6<T> h)
        {
            xy = h.xy;
            xz = h.xz;
            yz = h.yz;
            yx = h.yx;
            zx = h.zx;
            zy = h.zy;
        }

        public static Shear6<T> operator +(Shear6<T> h, Shear6<T> h2)
        {
            return new Shear6<T>(h2.xy + h.xy, h2.xz + h.xz, h2.yz + h.yz,
                               h2.yx + h.yx, h2.zx + h.zx, h2.zy + h.zy);
        }

        public static Shear6<T> operator -(Shear6<T> h2, Shear6<T> h)
        {
            return new Shear6<T>(h2.xy - h.xy, h2.xz - h.xz, h2.yz - h.yz,
                               h2.yx - h.yx, h2.zx - h.zx, h2.zy - h.zy);
        }

        public static Shear6<T> operator -(Shear6<T> c1)
        {
            return new Shear6<T>(-c1.xy, -c1.xz, -c1.yz, -c1.yx, -c1.zx, -c1.zy);
        }

        public Shear6<T> Negate()
        {
            xy = -xy;
            xz = -xz;
            yz = -yz;
            yx = -yx;
            zx = -zx;
            zy = -zy;

            return this;
        }

        public static Shear6<T> operator *(Shear6<T> h2, Shear6<T> h)
        {
            return new Shear6<T>(h2.xy * h.xy, h2.xz * h.xz, h2.yz * h.yz,
                               h2.yx * h.yx, h2.zx * h.zx, h2.zy * h.zy);
        }

        public static Shear6<T> operator *(Shear6<T> h2, T c)
        {
            return new Shear6<T>(h2.xy * c, h2.xz * c, h2.yz * c,
                               h2.yx * c, h2.zx * c, h2.zy * c);
        }

        public static Shear6<T> operator *(T c, Shear6<T> h2)
        {
            return new Shear6<T>(h2.xy * c, h2.xz * c, h2.yz * c,
                               h2.yx * c, h2.zx * c, h2.zy * c);
        }

        public static Shear6<T> operator /(Shear6<T> h2, T c)
        {
            return new Shear6<T>(h2.xy / c, h2.xz / c, h2.yz / c,
                               h2.yx / c, h2.zx / c, h2.zy / c);
        }

        public static Shear6<T> operator /(Shear6<T> h2, Shear6<T> h)
        {
            return new Shear6<T>(h2.xy / h.xy, h2.xz / h.xz, h2.yz / h.yz,
                               h2.yx / h.yx, h2.zx / h.zx, h2.zy / h.zy);
        }

        #region Equality

        public override bool Equals(object obj)
        {
            if (obj != null && obj is Shear6<T>)
            {
                Shear6<T> other = (Shear6<T>)obj;
                return Equals(ref this, ref other);
            }
            return base.Equals(obj);
        }

        public bool Equals(Shear6<T> other)
        {
            return Equals(this, other);
        }

        public static bool operator ==(Shear6<T> v1, Shear6<T> v2)
        {
            return Equals(ref v1, ref v2);
        }

        public static bool operator !=(Shear6<T> v1, Shear6<T> v2)
        {
            return !Equals(ref v1, ref v2);
        }

        private static bool Equals(ref Shear6<T> v1, ref Shear6<T> v2)
        {
            return (EqualityComparer<T>.Default.Equals(v1.xy, v2.xy) &&
                    EqualityComparer<T>.Default.Equals(v1.xz, v2.xz) &&
                    EqualityComparer<T>.Default.Equals(v1.yz, v2.yz) &&
                    EqualityComparer<T>.Default.Equals(v1.yx, v2.yx) &&
                    EqualityComparer<T>.Default.Equals(v1.zx, v2.zx) &&
                    EqualityComparer<T>.Default.Equals(v1.zy, v2.zy));
        }

        public override int GetHashCode()
        {
            int hashCode = 67;

            hashCode = hashCode * 71 + xy.GetHashCode();
            hashCode = hashCode * 71 + xz.GetHashCode();
            hashCode = hashCode * 71 + yz.GetHashCode();
            hashCode = hashCode * 71 + yx.GetHashCode();
            hashCode = hashCode * 71 + zx.GetHashCode();
            hashCode = hashCode * 71 + zy.GetHashCode();

            return hashCode;
        }

        #endregion
    }
}
