// Pogramming by
//     Douglas Andrade ( http://www.cmsoft.com.br, email: cmsoft@cmsoft.com.br)
//               Implementation of most of the functionality
//     Edgar Maass: (email: maass@logisel.de)
//               Code adaption, changed to user control
//
//Software used: 
//    OpenGL : http://www.opengl.org
//    OpenTK : http://www.opentk.com
//
// DISCLAIMER: Users rely upon this software at their own risk, and assume the responsibility for the results. Should this software or program prove defective, 
// users assume the cost of all losses, including, but not limited to, any necessary servicing, repair or correction. In no event shall the developers or any person 
// be liable for any loss, expense or damage, of any type or nature arising out of the use of, or inability to use this software or program, including, but not
// limited to, claims, suits or causes of action involving alleged infringement of copyrights, patents, trademarks, trade secrets, or unfair competition. 
//


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
  
    public class VertexSimple : List<float> , IVector
    {

        public Vector3 Vector;

        public Vector3 Color;


        //necessary for triangulation
        public uint IndexInModel;
        public bool Marked;
    
        public VertexSimple()
        {
        }
        public VertexSimple(Vector3 v)
        {
            Vector = v;
        }
        public VertexSimple(Vertex v)
        {

            Vector = new Vector3(v.Vector.X, v.Vector.Y, v.Vector.Z);

            this.IndexInModel = v.Index;
            this.Color = v.Color;
            

        }

        public VertexSimple(double x, double y, double z)
        {
            Vector = new Vector3(Convert.ToSingle(x), Convert.ToSingle(y), Convert.ToSingle(z));
           
        }
        public VertexSimple(Vector3 v, byte[] color)
        {
            
            Vector = v;
            this.Color = Color.ColorVectorFromByteArray(color); 
            
        }
        public VertexSimple(Vector3 v, Color color)
        {
           
            Vector = v;
            this.Color = Color.ColorVectorFromColor(color);
            
        }
        public VertexSimple(uint ind, Vector3 v, byte[] color)
        {
            this.IndexInModel = ind;
            Vector = v;
            this.Color = Color.ColorVectorFromByteArray(color); 
           
        }
        public VertexSimple(uint ind, Vector3 v, Color color)
        {
            this.IndexInModel = ind;
            Vector = v;
            this.Color = Color.ColorVectorFromColor(color);

        }
        public VertexSimple(uint myindexInModel, double x, double y, double z)
        {
            IndexInModel = myindexInModel;
            Vector = new Vector3(Convert.ToSingle(x), Convert.ToSingle(y), Convert.ToSingle(z));
           

        }
        public VertexSimple(uint indexInModel, Vertex v)
        {
            IndexInModel = indexInModel;
            Vector = v.Vector;
            Color = v.Color;
           
        }

        public VertexSimple(uint indexInModel, Vector3 v)
        {
            IndexInModel = indexInModel;
           
            Vector = v;

        }
        /// <summary>
        ///  only important for Delaunay 2D
        /// </summary>
        public float[] PositionArray
        {
            get
            {

            
                float[] position;
                if (this.Vector.Z == 0)
                {
                    position = new float[2];
                    position[0] = this.Vector.X;
                    position[1] = this.Vector.Y;
                }
                else
                {
                    position = new float[3];
                    position[0] = this.Vector.X;
                    position[1] = this.Vector.Y;
                    position[2] = this.Vector.Z;
                }

                return position; 

            }
            set
            {

                for (int i = 0; i < value.Length; i++)
                {
                    this.Vector[i] = value[i];
                }
            }
        }

        public new float this[int index]
        {
            get
            {
                return this.Vector[index];
            }
            set
            {
                this.Vector[index] = value;
            }
        }

    }


}
