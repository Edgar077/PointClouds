using System.Collections.Generic;
using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
namespace OpenTKExtension
{

    public class CameraFOV : RenderableObject
    {
        float zMin;
        float zMax;
        float fovX;
        float fovY;

        //calculated
        float xMin;
        float xMax;
        float yMin;
        float yMax;

        public CameraFOV(float myZMin, float myZMax, float myFOVX, float myFOVY) 
        {
            this.zMax = myZMax;
            this.zMin = myZMin;
            this.fovX = myFOVX;
            this.fovY = myFOVY;

            //cos (fovX/2) = xMin/zMin;

            this.xMin = Convert.ToSingle(Math.Cos(MathBase.DegreesToRadians_Float * fovX / 2)  * zMin);
            this.yMin = Convert.ToSingle(Math.Cos(MathBase.DegreesToRadians_Float * fovY / 2) * zMin);

            this.xMax = Convert.ToSingle(Math.Cos(MathBase.DegreesToRadians_Float * fovX / 2) * zMax);
            this.yMax = Convert.ToSingle(Math.Cos(MathBase.DegreesToRadians_Float * fovY / 2) * zMax);

            this.Position = Vector3.Zero;
            this.Scale = 1f;

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

            float intermediateRectangle = (zMin + zMax)/2;
            this.PointCloud.Vectors = new Vector3[]
             {   
                 //diagonals
                 new Vector3(0, 0, 0),
                 new Vector3(-xMax, -yMax, zMax),
                 new Vector3(0, 0, 0),
                 new Vector3(xMax, -yMax, zMax),
                 new Vector3(0, 0, 0),
                 new Vector3(xMax, yMax, zMax),
                 new Vector3(0, 0, 0),
                 new Vector3(-xMax, yMax, zMax),

                 //zmin
                new Vector3(-xMin, -yMin, zMin),
                new Vector3(xMin, -yMin, zMin),

                new Vector3(xMin, -yMin, zMin),
                new Vector3(xMin, yMin, zMin),

                new Vector3(xMin, yMin, zMin),
                new Vector3(-xMin, yMin, zMin),

                new Vector3(-xMin, yMin, zMin),
                new Vector3(-xMin, -yMin, zMin),

                 //intermediate

                new Vector3(-xMax, -yMax, intermediateRectangle),
                new Vector3(xMax, -yMax, intermediateRectangle),

                new Vector3(xMax, -yMax, intermediateRectangle),
                new Vector3(xMax, yMax, intermediateRectangle),
                
                new Vector3(xMax, yMax, intermediateRectangle),
                new Vector3(-xMax, yMax, intermediateRectangle),

                new Vector3(-xMax, yMax, intermediateRectangle),
                new Vector3(-xMax, -yMax, intermediateRectangle),

                //zMax
                new Vector3(-xMax, -yMax, zMax),
                new Vector3(xMax, -yMax, zMax),

                new Vector3(xMax, -yMax, zMax),
                new Vector3(xMax, yMax, zMax),
                
                new Vector3(xMax, yMax, zMax),
                new Vector3(-xMax, yMax, zMax),

                new Vector3(-xMax, yMax, zMax),
                new Vector3(-xMax, -yMax, zMax)
             };


            this.PointCloud.Colors = new Vector3[this.PointCloud.Vectors.Length];
            for (int i = 0; i < this.PointCloud.Vectors.Length; i++)
            {
                this.PointCloud.Colors[i] = new Vector3(.60f, 0.6f, 0.70f);

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