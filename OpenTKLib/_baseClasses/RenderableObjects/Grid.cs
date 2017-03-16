using System.Collections.Generic;
using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
namespace OpenTKExtension
{

    public class Grid : RenderableObject
    {
        int width;
        int depth;
       
        public Grid() : this(20, 20)
        {
            
            this.Position = Vector3.Zero;
            this.Scale = 1f;

        }
        public Grid(int myWidth, int mydepth) 
        {

            this.width = myWidth;
            this.depth = mydepth;

        }

        public override void InitializeGL()
        {
            this.primitiveType = PrimitiveType.Lines;

            if (initialized)
                this.Dispose();

            initialized = true;


            if (InitShaders("PointCloud.vert", "PointCloud.frag", path + "Shaders\\"))
            {
                this.initBuffers();
                this.FillPointCloud();
                FillIndexBuffer();
                //this.RefreshRenderableData();
            }

        }
      
      
        public override void Dispose()
        {

            base.Dispose();
        }

      
      
        public override void FillPointCloud()
        {

            this.PointCloud.Vectors = new Vector3[width * depth];

            int count = 0;
            int width_2 = width / 2;
            int depth_2 = depth / 2;
            
            for (int i = -width_2; i <= width_2; i++)
            {
                this.PointCloud.Vectors[count++] = new Vector3(i, 0, -depth_2);
                this.PointCloud.Vectors[count++] = new Vector3(i, 0, depth_2);
                this.PointCloud.Vectors[count++] = new Vector3(-width_2, 0, i);
                this.PointCloud.Vectors[count++] = new Vector3(width_2, 0, i);

            }

    
            this.PointCloud.Colors = new Vector3[this.PointCloud.Vectors.Length];
            for (int i = 0; i < this.PointCloud.Colors.Length; i++ )
            {
                this.PointCloud.Colors[i] = new Vector3(0.5f, 0.5f, 0.5f);
               
            }

          


        }
        public override void FillIndexBuffer()
        {
            this.PointCloud.Indices = new uint[this.PointCloud.Vectors.Length];
            for (uint i = 0; i < this.PointCloud.Colors.Length; i++)
            {
                this.PointCloud.Indices[0] = i;

            }
          

          
        }
      
     
   
    }
}