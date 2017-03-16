using System.Collections.Generic;
using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
namespace OpenTKExtension
{

    public class Axes : RenderableObject
    {
        float axisLength = 1f ;
       
        public Axes() : this(1f)
        {
            
            this.Position = Vector3.Zero;
            this.Scale = 1f;

        }
        public Axes(float myaxisLength)
        {
            axisLength = myaxisLength;
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
      
      
        public override void Dispose()
        {

            base.Dispose();
        }

      
      
        public override void FillPointCloud()
        {


            this.PointCloud.Vectors = new Vector3[]
             {            
                new Vector3(0, 0.0f, 0.0f),
                new Vector3(axisLength, 0.0f, 0.0f),
                new Vector3(0, 0.0f, 0.0f),
                new Vector3(0.0f, axisLength, 0.0f),
                new Vector3(0, 0.0f, 0.0f),
                new Vector3(0.0f, 0.0f, axisLength)
             };


            this.PointCloud.Colors = new Vector3[this.PointCloud.Vectors.Length];
            for (int i = 0; i < this.PointCloud.Vectors.Length; i++)
            {
                this.PointCloud.Colors[i] = new Vector3(1.0f, 0.0f, 0.0f);
               
            }

          


        }
        public override void FillIndexBuffer()
        {
            this.PointCloud.Indices = new uint[this.PointCloud.Vectors.Length];

            for (int i = 0; i < this.PointCloud.Vectors.Length; i++)
            {
                this.PointCloud.Indices[i] = Convert.ToUInt32(i);

            }

       
          
        }
      
     
   
    }
}