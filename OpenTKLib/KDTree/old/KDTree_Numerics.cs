using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Collections;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using NLinear;

namespace OpenTKExtension
{
    public class KDTree3D_Numerics<T> //: IEquatable<BoundingBox<T>> 
        where T : IEquatable<T>
    {
        public bool die = false;
        //1 means X 2 means Y 3 meansZ
       
        public List<Vector3<T>> points;
        public BoundingBox<T> Counter;

        public KDTree3D_Numerics(List<NLinear.Vector3<T>> Points, BoundingBox<T> box)
        {
            points = Points; Counter = box;
        }
        public KDTree3D_Numerics(List<Vector3<T>> Points)
        {
            this.points = new List<Vector3<T>>();
            T x1, x2, y1, y2, z1, z2;
            Points.Sort(CompareVectors_X);
            
            x1 = Points[0].X;
            x2 = Points[Points.Count - 1].X;
            Points.Sort(CompareVectors_Y);
            y1 = Points[0].Y; 
            y2 = Points[Points.Count - 1].Y;
            Points.Sort(CompareVectors_Z);
            z1 = Points[0].Z; 
            z2 = Points[Points.Count - 1].Z;

            for (int i = 0; i < Points.Count; i++)
            {
                Vector3<T> p = Points[i];
                if (p.x != x1 && p.x != x2 && p.y != y1 && p.y != y2 && p.z != z1 && p.z != z2)
                {
                    this.points.Add(p);
                }
            }
            this.Counter = new BoundingBox<T>(x1, y1, z1, x2, y2, z2);
        }
        public List<KDTree3D_Numerics<T>> Split()
        {
            if (this.points.Count < 1) { return null; }
            List<KDTree3D_Numerics<T>> trees = new List<KDTree3D_Numerics<T>>();
            this.die = true;
            BoundingBox<T> box1 = new BoundingBox<T>();
            BoundingBox<T> box2 = new BoundingBox<T>();
            List<Vector3<T>> ps1 = new List<Vector3<T>>();
            List<Vector3<T>> ps2 = new List<Vector3<T>>();

            Numeric<T> x1 = Counter.Min.X;
            Numeric<T> y1 = Counter.Min.Y;
            Numeric<T> z1 = Counter.Min.Z;
            Numeric<T> x2 = Counter.Max.X;
            Numeric<T> y2 = Counter.Max.Y;
            Numeric<T> z2 = Counter.Max.Z;
            Numeric<T> t1 = x2 - x1;
            Numeric<T> t2 = y2 - y1;
            Numeric<T> t3 = z2 - z1;

            if (t1 >= t2 && t1 >= t3)
            {
                this.points.Sort(CompareVectors_X);
                int count = Cut(this.points.Count);
                T t4 = this.points[count].X;
                box1 = new BoundingBox<T>(x1, y1, z1, t4, y2, z2);
                box2 = new BoundingBox<T>(t4, y1, z1, x2, y2, z2);
            }
            else if (t2 >= t1 && t2 >= t3)
            {
                this.points.Sort(CompareVectors_Y);
                int count = Cut(this.points.Count);
                T t4 = this.points[count].Y;
                box1 = new BoundingBox<T>(x1, y1, z1, x2, t4, z2);
                box2 = new BoundingBox<T>(x1, t4, z1, x2, y2, z2);
            }
            else if (t3 >= t2 && t3 >= t1)
            {
                this.points.Sort(CompareVectors_Z);
                int count = Cut(this.points.Count);
                T t4 = this.points[count].Z;
                box1 = new BoundingBox<T>(x1, y1, z1, x2, y2, t4);
                box2 = new BoundingBox<T>(x1, y1, t4, x2, y2, z2);
            }

            for (int i = 0; i < this.points.Count; i++)
            {
                Vector3<T> p = this.points[i];
                if (IsPointin(p, box1)) ps1.Add(p);
                if (IsPointin(p, box2)) ps2.Add(p);
            }
            trees.Add(new KDTree3D_Numerics<T>(ps1, box1));
            trees.Add(new KDTree3D_Numerics<T>(ps2, box2));
            return trees;
        }
        private static int CompareVectors_X(Vector3<T> first, Vector3<T> second)
        {
            if (first == default(Vector3<T>)) { if (second == default(Vector3<T>)) { return 0; } else { return -1; } }
            else
            {
                if (second == default(Vector3<T>)) { return 1; }
                else
                {
                    if (first.x > second.x) return 1;
                    if (first.X == second.x) return 0;
                    if (first.X < second.x) return -1;
                    else return 0;
                }
            }
        }
      
        private static int CompareVectors_Y(Vector3<T> first, Vector3<T> second)
        {
            if (first == default(Vector3<T>) ) { if (second == default(Vector3<T>)) { return 0; } else { return -1; } }
            else
            {
                if (second == default(Vector3<T>)) { return 1; }
                else
                {
                    if (first.y > second.y) return 1;
                    if (first.y == second.y) return 0;
                    if (first.y < second.y) return -1;
                    else return 0;
                }
            }
        }
        private static int CompareVectors_Z(Vector3<T> first, Vector3<T> second)
        {
            if (first == default(Vector3<T>)) { if (second == default(Vector3<T>)) { return 0; } else { return -1; } }
            else
            {
                if (second == default(Vector3<T>)) { return 1; }
                else
                {
                    if (first.z > second.z) return 1;
                    if (first.z == second.z) return 0;
                    if (first.z < second.z) return -1;
                    else return 0;
                }
            }
        }
        public int Cut(int i)
        {
            if (i < 1) { return -1; }
            return (int) Math.Floor(System.Convert.ToSingle(i / 2));
        }
        public bool IsPointin(Vector3<T> p, BoundingBox<T> pl)
        {
            Numeric<T> x1 = pl.Min.X;
            Numeric<T> y1 = pl.Min.Y;
            Numeric<T> z1 = pl.Min.Z;
            Numeric<T> x2 = pl.Max.X;
            Numeric<T> y2 = pl.Max.Y;
            Numeric<T> z2 = pl.Max.Z;
            if (p.X > x1 && p.X < x2 &&
              p.Y > y1 && p.Y < y2 &&
              p.Z > z1 && p.Z < z2
            ) { return true; }
            else { return false; }
        }
        public static bool IsPointinBox(Vector3<T> p, BoundingBox<T> pl)
        {
            Numeric<T> x1 = pl.Min.X;
            Numeric<T> y1 = pl.Min.Y;
            Numeric<T> z1 = pl.Min.Z;
            Numeric<T> x2 = pl.Max.X;
            Numeric<T> y2 = pl.Max.Y;
            Numeric<T> z2 = pl.Max.Z;
            if (p.X > x1 && p.X < x2 &&
              p.Y > y1 && p.Y < y2 &&
              p.Z > z1 && p.Z < z2
            ) { return true; }
            else { return false; }
        }
        public static List<BoundingBox<T>> SolveKdTree3D(List<Vector3<T>> points, BoundingBox<T> box)
        {

            List<BoundingBox<T>> PL = new List<BoundingBox<T>>();
            if (points.Count < 1) return PL;
            List<KDTree3D_Numerics<T>> trees = new List<KDTree3D_Numerics<T>>();

            List<Vector3<T>> pts = new List<Vector3<T>>();
            points.ForEach(delegate(Vector3<T> p)
            {
                if (IsPointinBox(p, box)) pts.Add(p);
            });
            trees.Add(new KDTree3D_Numerics<T>(pts, box));

            bool toggle = true;
            for (int i = 0; i < points.Count; i++)
            {
                if (toggle == false) break;
                toggle = false;
                for (int j = 0; j < trees.Count; j++)
                {
                    if (trees[j].die == false && trees[j].points.Count > 0)
                    {
                        trees.AddRange(trees[j].Split()); toggle = true;
                    }
                }
            }
            for (int i = 0; i < trees.Count; i++)
            { if (trees[i].die == false)PL.Add(trees[i].Counter); }
            return PL;
        }
        public static List<BoundingBox<T>> SolveKdTree3D(List<Vector3<T>> points)
        {
            List<BoundingBox<T>> PL = new List<BoundingBox<T>>();
            if (points.Count < 1) return PL;
            List<KDTree3D_Numerics<T>> trees = new List<KDTree3D_Numerics<T>>();
            trees.Add(new KDTree3D_Numerics<T>(points));
            bool toggle = true;
            for (int i = 0; i < points.Count; i++)
            {
                if (toggle == false) break;
                toggle = false;
                for (int j = 0; j < trees.Count; j++)
                {
                    if (trees[j].die == false && trees[j].points.Count > 0)
                    {
                        trees.AddRange(trees[j].Split()); toggle = true;
                    }
                }
            }
            for (int i = 0; i < trees.Count; i++)
            { if (trees[i].die == false)PL.Add(trees[i].Counter); }
            return PL;
        }
        public void Dispose()
        {
            this.points = default(List<Vector3<T>>);
            this.Counter = default(BoundingBox<T>);
        }
    }


}

