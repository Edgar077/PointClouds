using System.Collections.Generic;
using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using OpenTK.Extension;

namespace OpenTK.Extension
{

    public class Axes : RenderableObject
    {
        float axisLength = 1f;

        public Axes() : this(1f)
        {

            this.Position = Vector3.Zero;
            this.Scale = 1f;

        }
        public Axes(float myaxisLength)
        {
            colorOutput = true;
            axisLength = myaxisLength;
        }
        public override void InitializeGL()
        {
            
            
            InitFont();
            InitShader();
            //CreateVBOs();
            CreateVAO();
            shaderProgram["colorOutput"].SetValue(true);
        }
    
        public float AxesLength
        {
            get
            {
                return this.axisLength;
            }
            set
            {
                this.axisLength = value;
            }
        }


        public override void FillPointCloud()
        {


            this.PointCloudGL.Vectors = new Vector3[]
             {

          
            new Vector3(0, 0.0f, 0.0f),
            new Vector3(axisLength, 0.0f, 0.0f),
            new Vector3(0, 0.0f, 0.0f),
            new Vector3(0.0f, axisLength, 0.0f),
            new Vector3(0, 0.0f, 0.0f),
            new Vector3(0.0f, 0.0f, axisLength),
             };

            

            this.PointCloudGL.Colors = new Vector3[6];
            for (int i = 0; i < 6; i++)
            {
                this.PointCloudGL.Colors[i] = new Vector3(1.0f, 0.0f, 0.0f);

            }
            
            
            this.PointCloudGL.Triangles = new uint[6];
            this.PointCloudGL.Triangles[0] = 0;
            this.PointCloudGL.Triangles[1] = 1;
            this.PointCloudGL.Triangles[2] = 0;
            this.PointCloudGL.Triangles[3] = 2;
            this.PointCloudGL.Triangles[4] = 0;
            this.PointCloudGL.Triangles[5] = 3;
           

        }

        public override void CreateVBOs()
        {
            vboVectors = new VBO<Vector3>("Axes vertices", this.PointCloudGL.Vectors);
            vboColors = new VBO<Vector3>("Axes colors", this.PointCloudGL.Colors);
            vboTriangles = new VBO<uint>("Axes triangles", this.PointCloudGL.Triangles, OpenTK.Extension.BufferTarget.ElementArrayBuffer);
        }



    }
}