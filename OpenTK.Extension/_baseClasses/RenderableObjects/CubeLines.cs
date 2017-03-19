using System.Collections.Generic;
using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
namespace OpenTKExtension.FastGLControl
{

    public class CubeLines : RenderableObject
    {
        

        public CubeLines() : this(new Vector3(1, 1, 1))
        {

            this.Position = Vector3.Zero;
            this.Scale = 1f;

        }
     
        public override void InitializeGL()
        {
            this.primitiveType = PrimitiveType.Lines;
            
            initialized = true;


            if (InitShaders("cube.vert", "cube.frag", path + "Shaders\\"))
            {
                this.initBuffers();
                this.FillPointCloud();
                FillIndexBuffer();
                this.RefreshRenderableData();
            }

        }

        public CubeLines(Vector3 mycolor)
        {
            color = mycolor;
        }
      
        public override void Dispose()
        {

            base.Dispose();
        }

      
      
        public override void FillPointCloud()
        {


            this.PointCloud.Vectors = new Vector3[]
            {
            new Vector3(-1.0f, -1.0f,  1.0f),
            new Vector3( 1.0f, -1.0f,  1.0f),
            new Vector3( 1.0f,  1.0f,  1.0f),
            new Vector3(-1.0f,  1.0f,  1.0f),
            new Vector3(-1.0f, -1.0f, -1.0f),
            new Vector3( 1.0f, -1.0f, -1.0f), 
            new Vector3( 1.0f,  1.0f, -1.0f),
            new Vector3(-1.0f,  1.0f, -1.0f)
            };



            this.PointCloud.Colors = new Vector3[8];
            this.PointCloud.Colors[0] = new Vector3(1f, 0f, 0f);
            this.PointCloud.Colors[1] = new Vector3(0f, 0f, 1f);
            this.PointCloud.Colors[2] = new Vector3(0f, 1f, 0f);
            this.PointCloud.Colors[3] = new Vector3(1f, 0f, 0f);
            this.PointCloud.Colors[4] = new Vector3(0f, 0f, 1f);
            this.PointCloud.Colors[5] = new Vector3(0f, 1f, 0f);
            this.PointCloud.Colors[6] = new Vector3(1f, 0f, 0f);
            this.PointCloud.Colors[7] = new Vector3(0f, 0f, 1f);



        }
        public override void FillIndexBuffer()
        {
            this.PointCloud.Indices = new uint[]
            {
             // front face
                0, 1, 2, 2, 3, 0,
                // top face
                3, 2, 6, 6, 7, 3,
                // back face
                7, 6, 5, 5, 4, 7,
                // left face
                4, 0, 3, 3, 7, 4,
                // bottom face
                0, 1, 5, 5, 4, 0,
                // right face
                1, 5, 6, 6, 2, 1, 
            };

          
        }
        private void DrawCube(float size)
        {
            float[,] n = new float[,]
            {
            {-1.0f, 0.0f, 0.0f},
            {0.0f, 1.0f, 0.0f},
            {1.0f, 0.0f, 0.0f},
            {0.0f, -1.0f, 0.0f},
            {0.0f, 0.0f, 1.0f},
            {0.0f, 0.0f, -1.0f}
            };

            int[,] faces = new int[,]{
            {0, 1, 2, 3},
            {3, 2, 6, 7},
            {7, 6, 5, 4},
            {4, 5, 1, 0},
            {5, 6, 2, 1},
            {7, 4, 0, 3}
        };
            float[,] v = new float[8, 3];
            int i;

            v[0, 0] = v[1, 0] = v[2, 0] = v[3, 0] = -size / 2;
            v[4, 0] = v[5, 0] = v[6, 0] = v[7, 0] = size / 2;
            v[0, 1] = v[1, 1] = v[4, 1] = v[5, 1] = -size / 2;
            v[2, 1] = v[3, 1] = v[6, 1] = v[7, 1] = size / 2;
            v[0, 2] = v[3, 2] = v[4, 2] = v[7, 2] = -size / 2;
            v[1, 2] = v[2, 2] = v[5, 2] = v[6, 2] = size / 2;


            GL.Begin(PrimitiveType.Quads);
            for (i = 5; i >= 0; i--)
            {
                GL.Normal3(ref n[i, 0]);
                GL.Vertex3(ref v[faces[i, 0], 0]);
                GL.Vertex3(ref v[faces[i, 1], 0]);
                GL.Vertex3(ref v[faces[i, 2], 0]);
                GL.Vertex3(ref v[faces[i, 3], 0]);
            }
            GL.End();
        } 
     
   
    }
}