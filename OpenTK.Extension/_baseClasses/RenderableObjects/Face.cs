using System.Collections.Generic;
using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using OpenTKExtension;

namespace OpenTKExtension
{

    public class Face : RenderableObject
    {
        private PointCloud pointCloud;

        public Face(List<Vector3> points)
        {

            this.pointCloud = new OpenTKExtension.PointCloud(points, null, null, null, null, null);

        }
      
        public void Update(List<Vector3> points)
        {
            this.pointCloud = new OpenTKExtension.PointCloud(points, null, null, null, null, null);
            FillPointCloud();
            FillIndexBuffer();
        }
     
        public override void InitializeGL()
        {
            this.primitiveType = PrimitiveType.Points;
            initialized = true;


            if (this.PointCloud == null)
            {
                System.Diagnostics.Debug.Assert(false, "SW Error - please set the point cloud data f5irst ");
                return;

            }


            if (InitShaders("PointCloud.vert", "PointCloud.frag", path + "Shaders\\"))
            {
                this.initBuffers();
                FillPointCloud();
                FillIndexBuffer();

             
                //this.RefreshRenderableData();
            }

        }

        
      
        public override void Dispose()
        {

            base.Dispose();
        }
        public new PointCloud PointCloud
        {
            get
            {
                return pointCloud;
            }
            set
            {
                pointCloud = value;
                for(int i = 0; i < pointCloud.Colors.Length; i++)
                {
                    pointCloud.Colors[i] = new Vector3(0, 1, 0);
                }

            }
        }

        public override void FillPointCloud()
        {
           
            
            this.PointCloud.Colors = new Vector3[this.PointCloud.Vectors.Length];
            for (int i = 0; i < pointCloud.Vectors.Length; i++)
            {
               this.PointCloud.Colors[i] = new Vector3(0f,1f,0f);
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