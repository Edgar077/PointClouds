using System.Collections.Generic;
using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using OpenTK.Extension;

namespace OpenTK.Extension
{

    public class Grid : RenderableObject
    {
        int width;
        int depth;

        public Grid()
            : this(20, 20)
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

          
            InitShader();
            CreateVAO();

            shaderProgram["colorOutput"].SetValue(true);
        }
     

        public override void FillPointCloud()
        {

            this.PointCloudGL.Vectors = new Vector3[width * depth];

            int count = 0;
            int width_2 = width / 2;
            int depth_2 = depth / 2;

            for (int i = -width_2; i <= width_2; i++)
            {
                this.PointCloudGL.Vectors[count++] = new Vector3(i, 0, -depth_2);
                this.PointCloudGL.Vectors[count++] = new Vector3(i, 0, depth_2);
                this.PointCloudGL.Vectors[count++] = new Vector3(-width_2, 0, i);
                this.PointCloudGL.Vectors[count++] = new Vector3(width_2, 0, i);

            }


            this.PointCloudGL.Colors = new Vector3[this.PointCloudGL.Vectors.Length];
            for (int i = 0; i < this.PointCloudGL.Colors.Length; i++)
            {
                this.PointCloudGL.Colors[i] = new Vector3(0.2f, 0.2f, 0.5f);

            }
            

            this.PointCloudGL.Triangles = new uint[this.PointCloudGL.Vectors.Length];
            for (uint i = 0; i < this.PointCloudGL.Triangles.Length; i++)
            {
                this.PointCloudGL.Triangles[0] = i;

            }
            

        }
        public override void CreateVBOs()
        {
            vboVectors = new VBO<Vector3>("Axes vertices", this.PointCloudGL.Vectors);
            vboColors = new VBO<Vector3>("Axes colors", this.PointCloudGL.Colors);
            vboTriangles = new VBO<uint>("Axes triangles", this.PointCloudGL.Triangles, OpenTK.Extension.BufferTarget.ElementArrayBuffer);
        }


    }
}