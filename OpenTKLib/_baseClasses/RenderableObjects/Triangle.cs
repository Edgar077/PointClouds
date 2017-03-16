using System.Collections.Generic;
using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
namespace OpenTKExtension.FastGLControl
{

    public class Triangle : RenderableObject
    {
        public Triangle()
        {
            this.primitiveType = PrimitiveType.Triangles;
            if (InitShaders("triangle.vert", "triangle.frag", path + "Shaders\\"))
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
           this.PointCloud.Vectors = new Vector3[3];

           this.PointCloud.Vectors[0] = new Vector3(-.8f, - .8f,0);
            this.PointCloud.Vectors[1] = new Vector3(0.8f , -.8f,0);
            this.PointCloud.Vectors[2] = new Vector3( 0, 0.8f, 0 );
          
        
            this.PointCloud.Colors = new Vector3[3];

            this.PointCloud.Colors[0] = new Vector3(1,0,0);
            this.PointCloud.Colors[1] = new Vector3(0,1,0);
            this.PointCloud.Colors[2] = new Vector3(0,0,1);
           
            
        }
        public override void FillIndexBuffer()
        {

            this.PointCloud.Indices = new uint[3];

            this.PointCloud.Indices[0] = 0;             
            this.PointCloud.Indices[1] = 1;
            this.PointCloud.Indices[2] = 2;
          
        }

        
    }
}