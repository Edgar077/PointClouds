
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using OpenTK;

namespace OpenTKExtension
{
    /// <summary>
    /// An interface for a structure with indexer
    /// </summary>
    public interface IVector : IList<float>
    {


    }

    public class Vertex : List<float>, IVector
    {
        public Vector3 Vector;
        public uint Index;
        public Vector3 Color;
        public List<int> IndexTriangles;
       // private float length;

        public Vertex()
        { }
        public Vertex(Vector3 v, uint index)
        {
            this.Vector = v;
            this.Index = index;


        }
        public Vertex(Vector3 v, Vector3 color, uint index)
        {

            Vector = v;
            this.Color = color;
            this.Index = index;

        }
       
        public Vertex(Vector3 v, Color color)
        {

            Vector = v;
            this.Color = Color.ColorVectorFromColor(color);

        }
        public Vertex(double x, double y, double z)
        {
            Vector = new Vector3(Convert.ToSingle(x), Convert.ToSingle(y), Convert.ToSingle(z));

        }
        public Vertex(Vector3 v)
        {
            Vector = new Vector3(v.X, v.Y, v.Z);

        }

        public Vertex(Vector3 v, Color color, uint index)
        {

            Vector = v;
            this.Color = Color.ColorVectorFromColor(color);
            this.Index = index;

        }

        public static Vertex Zero
        {
            get
            {
                return new Vertex(Vector3.Zero, uint.MaxValue);
            }
        }
        public override string ToString()
        {
            return this.Vector.ToString() + " -i: " + this.Index.ToString();// +" -l: " + this.Length.ToString("G2");

        }

        #region List
        public bool CompareTo(Vertex v)
        {
            //if (this.Length != v.Length)
            //    return false;
            if (this.Vector.X != v.Vector.X)
                return false;
            if (this.Vector.Y != v.Vector.Y)
                return false;
            if (this.Vector.Z != v.Vector.Z)
                return false;

            return true;


        }
        public static Vertex operator -(Vertex v1, Vertex v2)
        {
            Vector3 v = v1.Vector - v2.Vector;
            return new Vertex(v, 0);
        }
        public static Vertex operator +(Vertex v1, Vertex v2)
        {
            Vector3 v = v1.Vector + v2.Vector;
            return new Vertex(v, 0);
        }
        public static Vertex operator /(Vertex v1, Vertex v2)
        {

            Vector3 v = new Vector3(v1.Vector.X / v2.Vector.X, v1.Vector.Y / v2.Vector.Y, v1.Vector.Z / v2.Vector.Z);
            return new Vertex(v, 0);
        }
        public static Vertex operator /(Vertex v1, float f)
        {

            Vector3 v = new Vector3(v1.Vector.X / f, v1.Vector.Y / f, v1.Vector.Z / f);
            return new Vertex(v, 0);
        }
        public Vertex Clone()
        {
            Vertex v = new Vertex(this.Vector, this.Index);
            return v;

        }

        #endregion


        //public float Length
        //{
        //    get
        //    {
        //        if (length == 0 && this.Vector != Vector3.Zero)
        //        {
        //            length = this.Vector.Length;
        //        }
        //        return length;

        //    }
        //}
            
        
    }

}
