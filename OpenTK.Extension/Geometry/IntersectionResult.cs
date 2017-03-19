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
    public struct IntersectionResult<T> : IEquatable<IntersectionResult<T>>
    {
        #region Private attributes

        readonly T location;

        readonly bool result;

        #endregion

        #region ctor

        public IntersectionResult(bool result)
        {
            this.location = default(T);
            this.result = result;
        }

        public IntersectionResult(T location, bool result)
        {
            this.location = location;
            this.result = result;
        }

        #endregion

        #region Public attributes

        public T Location
        {
            get
            {
                return location;
            }
        }

        public bool Result
        {
            get
            {
                return result;
            }
        }

        #endregion

        #region Equality

        public override bool Equals(object obj)
        {
            if (obj != null && obj is IntersectionResult<T>)
            {
                IntersectionResult<T> other = (IntersectionResult<T>)obj;
                return Equals(ref this, ref other);
            }
            return base.Equals(obj);
        }

        public bool Equals(IntersectionResult<T> other)
        {
            return Equals(this, other);
        }

        public static bool operator ==(IntersectionResult<T> v1, IntersectionResult<T> v2)
        {
            return EqualityComparer<T>.Default.Equals(v1.location, v2.location)
                && (v1.result == v2.result);
        }

        public static bool operator !=(IntersectionResult<T> v1, IntersectionResult<T> v2)
        {
            return !(EqualityComparer<T>.Default.Equals(v1.location, v2.location)
                && (v1.result == v2.result));
        }

        private static bool Equals(ref IntersectionResult<T> v1, ref IntersectionResult<T> v2)
        {
            return EqualityComparer<T>.Default.Equals(v1.location, v2.location)
                && (v1.result == v2.result);
        }

        public override int GetHashCode()
        {
            int hashCode = 67;

            hashCode = hashCode * 71 + location.GetHashCode();
            hashCode = hashCode * 71 + result.GetHashCode();

            return hashCode;
        }

        #endregion
    }
}
