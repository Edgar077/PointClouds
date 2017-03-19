using System.Collections.Generic;
using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
namespace OpenTKExtension.FastGLControl
{

    public class SampleCloud : RenderableObject
    {
        public SampleCloud()
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

            this.PointCloud.Vectors = new Vector3[8];
            this.PointCloud.Vectors[0] = new Vector3(-0.5f, -0.5f, -0.5f);
            this.PointCloud.Vectors[1] = new Vector3(0.5f, -0.5f, -0.5f);
            this.PointCloud.Vectors[2] = new Vector3(0.5f, 0.5f, -0.5f);
            this.PointCloud.Vectors[3] = new Vector3(-0.5f, 0.5f, -0.5f);
            this.PointCloud.Vectors[4] = new Vector3(-0.5f, -0.5f, 0.5f);

            this.PointCloud.Vectors[5] = new Vector3(0.5f, -0.5f, 0.5f);

            this.PointCloud.Vectors[6] = new Vector3(0.5f, 0.5f, 0.5f);
            this.PointCloud.Vectors[7] = new Vector3(-0.5f, 0.5f, 0.5f);

            this.PointCloud.Colors = new Vector3[8];
            for (int i = 0; i < 8; i++)
            {
                this.PointCloud.Colors[i] = new Vector3(0, 1, 0);
            }
           
            
        }
        public override void FillIndexBuffer()
        {

            this.PointCloud.Indices = new uint[36];

            //bottom face
            this.PointCloud.Indices[0] = 0;
            this.PointCloud.Indices[1] = 5;
            this.PointCloud.Indices[2] = 4;
            this.PointCloud.Indices[3] = 5;
            this.PointCloud.Indices[4] = 0;
            this.PointCloud.Indices[5] = 1;


            //top face
            this.PointCloud.Indices[6] = 3;
            this.PointCloud.Indices[7] = 7;
            this.PointCloud.Indices[8] = 6;
            this.PointCloud.Indices[9] = 3;
            this.PointCloud.Indices[10] = 6;
            this.PointCloud.Indices[11] = 2;



            //front face
            this.PointCloud.Indices[12] = 7;
            this.PointCloud.Indices[13] = 4;
            this.PointCloud.Indices[14] = 6;
            this.PointCloud.Indices[15] = 6;
            this.PointCloud.Indices[16] = 4;
            this.PointCloud.Indices[17] = 5;


            //back face
            this.PointCloud.Indices[18] = 2;
            this.PointCloud.Indices[19] = 1;
            this.PointCloud.Indices[20] = 3;
            this.PointCloud.Indices[21] = 3;
            this.PointCloud.Indices[22] = 1;
            this.PointCloud.Indices[23] = 0;


            //left face
            this.PointCloud.Indices[24] = 3;
            this.PointCloud.Indices[25] = 0;
            this.PointCloud.Indices[26] = 7;
            this.PointCloud.Indices[27] = 7;
            this.PointCloud.Indices[28] = 0;
            this.PointCloud.Indices[29] = 4;


            //right face
            this.PointCloud.Indices[30] = 6;
            this.PointCloud.Indices[31] = 5;
            this.PointCloud.Indices[32] = 2;
            this.PointCloud.Indices[33] = 2;
            this.PointCloud.Indices[34] = 5;
            this.PointCloud.Indices[35] = 1;
          
        }

        
    }
}