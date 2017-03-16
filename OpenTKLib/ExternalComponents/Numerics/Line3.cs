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
    public struct Line3<T> : IEquatable<Line3<T>>
        where T : IEquatable<T>
    {

        private Vector3<T> from;
        private Vector3<T> to;
        Vector3<T> dir;
       

        public Line3(Vector3<T> p0, Vector3<T> p1, T unit)
        {
            from = p0;
            to = p1;
            
            dir = p1 - p0;
            dir.Normalize(unit);
        }
        public Line3(Vector3<T> p0, Vector3<T> p1)
        {
            from = p0;
            to = p1;

            dir = p1 - p0;
           
        }

        public Vector3<T> From
        {
            get
            {
                return from;
            }
            set
            {
                from = value;
            }
        }
        public Vector3<T> To
        {
            get
            {
                return to;
            }
            set
            {
                to = value;
            }
        }

        public Vector3<T> Dir
        {
            get
            {
                return dir;
            }
            set
            {
                dir = value;
            }
        }

        void Set(Vector3<T> p0, Vector3<T> p1, T unit)
        {
            from = p0; 
            dir = p1 - p0;
            dir.Normalize(unit);
        }

       

        public Vector3<T> ClosestPointTo(Vector3<T> point)
        {
            return ((point - from) ^ dir) * dir + from;
        }

        public Vector3<T> GetPoint(T parameter)
        {
            return from + dir * parameter;
        }

        public T DistanceTo(Vector3<T> point, T unit)
        {
            return (ClosestPointTo(point) - point).Length(unit);
        }
        

        public T DistanceTo(Line3<T> line)
        {
            Numeric<T> d = (dir % line.dir) ^ (line.from - from);
            return (d >= Numeric<T>.Zero()) ? d : -d;
        }

        //public Vector3<T> ClosestPointTo(Line3<T> line, Func<float, T> fromFloat = null, Func<T, float> magnitude = null)
        //{
        //    if (fromFloat == null)
        //        fromFloat = Numeric<T>.FromFloat;

        //    if (magnitude == null)
        //        magnitude = Numeric<T>.ToFloat;

        //    // Assumes the lines are normalized

        //    Vector3<T> posLpos = fromFloat - line.from;
        //    Numeric<T> c = dir ^ posLpos;
        //    Numeric<T> a = line.dir ^ dir;
        //    Numeric<T> f = line.dir ^ posLpos;
        //    Numeric<T> num = c - a * f;

        //    Numeric<T> denom = a * a - fromFloat(1);

        //    Numeric<T> absDenom = ((denom >= Numeric<T>.Zero()) ? denom : -denom);

        //    if (absDenom < fromfloat(1))
        //    {
        //        Numeric<T> absNum = ((num >= Numeric<T>.Zero()) ? num : -num);

        //        if (magnitude(absNum) >= magnitude(absDenom) * float.MaxValue)
        //            return fromFloat;
        //    }

        //    return fromFloat + dir * (num / denom);
        //}

        public void Multiply(Matrix44<T> m, T unit)
        {
            this = new Line3<T>(from * m, (from + dir) * m, unit);
        }

        #region Equality

        public override bool Equals(object obj)
        {
            if (obj != null && obj is Line3<T>)
            {
                Line3<T> other = (Line3<T>)obj;
                return Equals(ref this, ref other);
            }
            return base.Equals(obj);
        }

        public bool Equals(Line3<T> other)
        {
            return Equals(this, other);
        }

        private static bool Equals(ref Line3<T> v1, ref Line3<T> v2)
        {
            return EqualityComparer<Vector3<T>>.Default.Equals(v1.dir, v2.dir)
                && EqualityComparer<Vector3<T>>.Default.Equals(v1.from, v2.from);
        }

        public static bool operator ==(Line3<T> v1, Line3<T> v2)
        {
            return EqualityComparer<Vector3<T>>.Default.Equals(v1.dir, v2.dir)
                && EqualityComparer<Vector3<T>>.Default.Equals(v1.from, v2.from);
        }

        public static bool operator !=(Line3<T> v1, Line3<T> v2)
        {
            return !(EqualityComparer<Vector3<T>>.Default.Equals(v1.dir, v2.dir)
                && EqualityComparer<Vector3<T>>.Default.Equals(v1.from, v2.from));
        }

        public override int GetHashCode()
        {
            int hashCode = 67;

            hashCode = hashCode * 71 + dir.GetHashCode();
            hashCode = hashCode * 71 + from.GetHashCode();

            return hashCode;
        }

        #endregion

        #region new 

        public static bool intersection(Line3<T> a, Line3<T> b, ref Vector3<T> intersectionPoint)
        // http://mathworld.wolfram.com/Line-LineIntersection.html
        // in 3d; will also work in 2d if z components are 0
        {

            //Check with
            T v = a.DistanceTo(b);
            float valRet = Convert.ToSingle(v);
            //if(val > 0)
            //should give the same result as below
            

            Vector3<T> da = a.To - a.From;
            Vector3<T> db = b.To - b.From;
            Vector3<T> dc = b.From - a.From;

            T val = dc.Dot(da.Cross(db));
            
            if (Convert.ToSingle(val) != 0) // lines are not coplanar
                return false;
           // Point s = dot(cross(dc, db), cross(da, db)) / norm2(cross(da, db));
             Vector3<T> temp1 = dc.Cross(db);
            Vector3<T> temp2 = da.Cross(db);
            val = temp1.Dot(temp2);
            Numeric<T> valN = val;
            valN /= temp2.Norm2();
            float vald = Convert.ToSingle( val);
            if (vald >= 0.0 && vald <= 1)
            {
                intersectionPoint = a.From + da * val;
                return true;
            }

            return false;
        }
        #endregion

    }
}
