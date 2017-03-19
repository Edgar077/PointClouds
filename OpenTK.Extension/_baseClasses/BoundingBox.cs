#region License
/*
MIT License
Copyright Â© 2006 The Mono.Xna Team

All rights reserved.

Authors:
Olivier Dufour (Duff)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
#endregion License

using System;
using System.Collections.Generic;
using System.ComponentModel;
using OpenTK;
using OpenTKExtension;


namespace OpenTKExtension
{
    public enum ContainmentType
    {
        Contains,
        Disjoint,
        Intersects
    }
    public struct BoundingBox : IEquatable<BoundingBox>
    {

        #region Public Fields


        public Vector3 Min;

        public Vector3 Max;

        public const int CornerCount = 8;

        #endregion Public Fields


        #region Public Constructors

        public BoundingBox(Vector3 min, Vector3 max)
        {
            this.Min = min;
            this.Max = max;
        }
       
        //public static BoundingBox FromPoints(List<Vector3> points)
        //{
        //    if (points == null)
        //        throw new ArgumentNullException();

        //    // TODO: Just check that Count > 0
        //    bool empty = true;
        //    Vector3 vector2 = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        //    Vector3 vector1 = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        //    foreach (Vector3 vector3 in points)
        //    {
        //        vector2 = Vector3.Min(vector2, vector3);
        //        vector1 = Vector3.Max(vector1, vector3);
        //        empty = false;
        //    }
        //    if (empty)
        //        throw new ArgumentException();

        //    return new BoundingBox(vector2, vector1);
        //}
        //public static BoundingBox FromPointCloud(PointCloud pointCloud)
        //{
        //    if (pointCloud == null)
        //        throw new ArgumentNullException();

        //    // TODO: Just check that Count > 0
        //    bool empty = true;
        //    Vector3 vector2 = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        //    Vector3 vector1 = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        //    foreach (Vector3 vector3 in pointCloud.Vectors)
        //    {
        //        vector2 = Vector3.Min(vector2, vector3);
        //        vector1 = Vector3.Max(vector1, vector3);
        //        empty = false;
        //    }
        //    if (empty)
        //        throw new ArgumentException();

        //    return new BoundingBox(vector2, vector1);
        //}

        public static BoundingBox FromPointCloud(PointCloud pointCloud)
        {

            if (pointCloud.Vectors == null || pointCloud.Vectors.Length == 0)
                return new BoundingBox();
            //int nDim = pointCloud.Vectors.Length;
            //if (nDim < 1)
            //    return BoundingBox;

            Vector3 maxPoint = new Vector3();
            Vector3 minPoint = new Vector3();

            float xMax = pointCloud.Vectors[0].X;
            float yMax = pointCloud.Vectors[0].Y;
            float zMax = pointCloud.Vectors[0].Z;
            float xMin = pointCloud.Vectors[0].X;
            float yMin = pointCloud.Vectors[0].Y;
            float zMin = pointCloud.Vectors[0].Z;
            for (int i = 0; i < pointCloud.Vectors.Length; i++)
            {
                Vector3 ver = pointCloud.Vectors[i];
                if (ver.X > xMax)
                    xMax = ver.X;
                if (ver.Y > yMax)
                    yMax = ver.Y;
                if (ver.Z > zMax)
                    zMax = ver.Z;
                if (ver.X < xMin)
                    xMin = ver.X;
                if (ver.Y < yMin)
                    yMin = ver.Y;
                if (ver.Z < zMin)
                    zMin = ver.Z;
            }
            maxPoint.X = xMax;
            maxPoint.Y = yMax;
            maxPoint.Z = zMax;

            minPoint.X = xMin;
            minPoint.Y = yMin;
            minPoint.Z = zMin;

            return new BoundingBox(minPoint, maxPoint);

        }
        #endregion Public Constructors


        #region Public Methods

        public Vector3[] GetCorners()
        {
            return new Vector3[] {
                new Vector3(this.Min.X, this.Max.Y, this.Max.Z), 
                new Vector3(this.Max.X, this.Max.Y, this.Max.Z),
                new Vector3(this.Max.X, this.Min.Y, this.Max.Z), 
                new Vector3(this.Min.X, this.Min.Y, this.Max.Z), 
                new Vector3(this.Min.X, this.Max.Y, this.Min.Z),
                new Vector3(this.Max.X, this.Max.Y, this.Min.Z),
                new Vector3(this.Max.X, this.Min.Y, this.Min.Z),
                new Vector3(this.Min.X, this.Min.Y, this.Min.Z)
            };
        }

        public ContainmentType Contains(BoundingBox box)
        {
            //test if all corner is in the same side of a face by just checking min and max
            if (box.Max.X < Min.X
                || box.Min.X > Max.X
                || box.Max.Y < Min.Y
                || box.Min.Y > Max.Y
                || box.Max.Z < Min.Z
                || box.Min.Z > Max.Z)
                return ContainmentType.Disjoint;


            if (box.Min.X >= Min.X
                && box.Max.X <= Max.X
                && box.Min.Y >= Min.Y
                && box.Max.Y <= Max.Y
                && box.Min.Z >= Min.Z
                && box.Max.Z <= Max.Z)
                return ContainmentType.Contains;

            return ContainmentType.Intersects;
        }

        public void Contains(ref BoundingBox box, out ContainmentType result)
        {
            result = Contains(box);
        }

      
        public ContainmentType Contains(Vector3 point)
        {
            ContainmentType result;
            this.Contains(ref point, out result);
            return result;
        }

        public void Contains(ref Vector3 point, out ContainmentType result)
        {
            //first we get if point is out of box
            if (point.X < this.Min.X
                || point.X > this.Max.X
                || point.Y < this.Min.Y
                || point.Y > this.Max.Y
                || point.Z < this.Min.Z
                || point.Z > this.Max.Z)
            {
                result = ContainmentType.Disjoint;
            }//or if point is on box because coordonate of point is lesser or equal
            else if (point.X == this.Min.X
                || point.X == this.Max.X
                || point.Y == this.Min.Y
                || point.Y == this.Max.Y
                || point.Z == this.Min.Z
                || point.Z == this.Max.Z)
                result = ContainmentType.Intersects;
            else
                result = ContainmentType.Contains;


        }

   

   
        public static BoundingBox CreateMerged(BoundingBox original, BoundingBox additional)
        {
            return new BoundingBox(
                Vector3.Min(original.Min, additional.Min), Vector3.Max(original.Max, additional.Max));
        }

        public static void CreateMerged(ref BoundingBox original, ref BoundingBox additional, out BoundingBox result)
        {
            result = BoundingBox.CreateMerged(original, additional);
        }

        public bool Equals(BoundingBox other)
        {
            return (this.Min == other.Min) && (this.Max == other.Max);
        }

        public override bool Equals(object obj)
        {
            return (obj is BoundingBox) ? this.Equals((BoundingBox)obj) : false;
        }

      

        public void GetCorners(Vector3[] corners)
        {
            if (corners == null)
            {
                throw new ArgumentNullException("corners");
            }
            if (corners.Length < 8)
            {
                throw new ArgumentOutOfRangeException("corners", "Not Enought Corners");
            }
            corners[0].X = this.Min.X;
            corners[0].Y = this.Max.Y;
            corners[0].Z = this.Max.Z;
            corners[1].X = this.Max.X;
            corners[1].Y = this.Max.Y;
            corners[1].Z = this.Max.Z;
            corners[2].X = this.Max.X;
            corners[2].Y = this.Min.Y;
            corners[2].Z = this.Max.Z;
            corners[3].X = this.Min.X;
            corners[3].Y = this.Min.Y;
            corners[3].Z = this.Max.Z;
            corners[4].X = this.Min.X;
            corners[4].Y = this.Max.Y;
            corners[4].Z = this.Min.Z;
            corners[5].X = this.Max.X;
            corners[5].Y = this.Max.Y;
            corners[5].Z = this.Min.Z;
            corners[6].X = this.Max.X;
            corners[6].Y = this.Min.Y;
            corners[6].Z = this.Min.Z;
            corners[7].X = this.Min.X;
            corners[7].Y = this.Min.Y;
            corners[7].Z = this.Min.Z;
        }

        public override int GetHashCode()
        {
            return this.Min.GetHashCode() + this.Max.GetHashCode();
        }

        public bool Intersects(BoundingBox box)
        {
            bool result;
            Intersects(ref box, out result);
            return result;
        }

        public void Intersects(ref BoundingBox box, out bool result)
        {
            if ((this.Max.X >= box.Min.X) && (this.Min.X <= box.Max.X))
            {
                if ((this.Max.Y < box.Min.Y) || (this.Min.Y > box.Max.Y))
                {
                    result = false;
                    return;
                }

                result = (this.Max.Z >= box.Min.Z) && (this.Min.Z <= box.Max.Z);
                return;
            }

            result = false;
            return;
        }

     
        //public void Intersects(ref BoundingSphere sphere, out bool result)
        //{
        //    result = Intersects(sphere);
        //}

        //public PlaneIntersectionType Intersects(Plane plane)
        //{
        //    PlaneIntersectionType result;
        //    Intersects(ref plane, out result);
        //    return result;
        //}

        //public void Intersects(ref Plane plane, out PlaneIntersectionType result)
        //{
        //    // See http://zach.in.tu-clausthal.de/teaching/cg_literatur/lighthouse3d_view_frustum_culling/index.html

        //    Vector3 positiveVertex;
        //    Vector3 negativeVertex;

        //    if (plane.Normal.X >= 0)
        //    {
        //        positiveVertex.X = Max.X;
        //        negativeVertex.X = Min.X;
        //    }
        //    else
        //    {
        //        positiveVertex.X = Min.X;
        //        negativeVertex.X = Max.X;
        //    }

        //    if (plane.Normal.Y >= 0)
        //    {
        //        positiveVertex.Y = Max.Y;
        //        negativeVertex.Y = Min.Y;
        //    }
        //    else
        //    {
        //        positiveVertex.Y = Min.Y;
        //        negativeVertex.Y = Max.Y;
        //    }

        //    if (plane.Normal.Z >= 0)
        //    {
        //        positiveVertex.Z = Max.Z;
        //        negativeVertex.Z = Min.Z;
        //    }
        //    else
        //    {
        //        positiveVertex.Z = Min.Z;
        //        negativeVertex.Z = Max.Z;
        //    }

        //    var distance = Vector3.Dot(plane.Normal, negativeVertex) + plane.D;
        //    if (distance > 0)
        //    {
        //        result = PlaneIntersectionType.Front;
        //        return;
        //    }

        //    distance = Vector3.Dot(plane.Normal, positiveVertex) + plane.D;
        //    if (distance < 0)
        //    {
        //        result = PlaneIntersectionType.Back;
        //        return;
        //    }

        //    result = PlaneIntersectionType.Intersecting;
        //}

        //public Nullable<float> Intersects(Ray ray)
        //{
        //    return ray.Intersects(this);
        //}

        //public void Intersects(ref Ray ray, out Nullable<float> result)
        //{
        //    result = Intersects(ray);
        //}

        public static bool operator ==(BoundingBox a, BoundingBox b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(BoundingBox a, BoundingBox b)
        {
            return !a.Equals(b);
        }

        public override string ToString()
        {
            return string.Format("{{Min:{0} Max:{1}}}", this.Min.ToString(), this.Max.ToString());
        }

        #endregion Public Methods
    }
}