


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
    

    //public class VertexKDTree : Vertex
    //{
    //    public bool TakenInTree;
    //    //public float Length;

    //    public VertexKDTree()
    //    { }

    //    public VertexKDTree(Vector3 v, int i): base(v, i)
    //    {
    //        //this.Length = v.Length;

    //    }
    //    public new VertexKDTree Clone()
    //    {
    //        VertexKDTree v = new VertexKDTree(this.Vector, this.Index);
    //        return v;

    //    }
    //    public new static VertexKDTree Zero
    //    {
    //        get
    //        {
    //            return new VertexKDTree(Vector3.Zero, -1);
    //        }
    //    }
    //    public override string ToString()
    //    {
    //        return this.Vector.ToString() + " -i: " + this.Index.ToString();// + " -l: " + this.Length.ToString("G2");

    //    }
    //}
    public class VertexKDTree //: List<float>
    {
        public bool TakenInTree;
        //public float Length;
        public float Distance;
        public int IndexNeighbour;
        public Vector3 Vector;
        public Vector3 Color;
        public int Index;

        public VertexKDTree()
        { }

        public VertexKDTree(Vector3 v, int index) 
        {
            this.Vector = v.Clone();
            this.Index = index;

        }
        public VertexKDTree Clone()
        {
            VertexKDTree v = new VertexKDTree(this.Vector, this.Index);
            return v;

        }

        public VertexKDTree(Vector3 v, Vector3 c, int index) : this(v, index)
        {
            this.Color = c;
        }

        public override string ToString()
        {
            return this.Vector.ToString() + " -i: " + this.Index.ToString();// + " -l: " + this.Length.ToString("G2");

        }
        public static VertexKDTree Zero
        {
            get
            {
                return new VertexKDTree(Vector3.Zero, -1);
            }
        }
    }

}
