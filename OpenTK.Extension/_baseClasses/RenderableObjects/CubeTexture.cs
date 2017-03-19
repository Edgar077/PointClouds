using System.Collections.Generic;
using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using OpenTKExtension;

namespace OpenTKExtension
{

    public class CubeTexture : RenderableObject
    {
        
        public CubeTexture() 
        {

            this.Position = Vector3.Zero;
            this.Scale = 1f;
        }
     
        public override void InitializeGL()
        {
            this.primitiveType = PrimitiveType.Points;
            //this.primitiveType = PrimitiveType.Triangles;

            initialized = true;

           

            if (this.PointCloud == null)
            {
                System.Diagnostics.Debug.Assert(false, "SW Error - please set the point cloud data first ");
                return;

            }


            if (InitShaders("cubeTexture.vert", "cubeTexture.frag", path + "Shaders\\"))
            {
                this.initBuffers();
                //at this point the data is transferred to GPU - therefore have to reset vector data here, otherwise it is useless.
                if (GLSettings.PointCloudCentered && !PointCloud.DisregardCenteredShowing)
                    this.PointCloud.ResetCentroid(true);
                if (GLSettings.BoundingBoxLeftStartsAt000 && !PointCloud.DisregardCenteredShowing)
                {
                    this.PointCloud.Translate_StartAt_Y0();
                    //this.PointCloud.Translate_StartAtBoundingBox000();
                }
                //point cloud data, indices, colors etc. are set before...
                //this.RefreshRenderableData();
            }
           
        }

        
      
        public override void Dispose()
        {

            base.Dispose();
        }

        public override string ToString()
        {
            if (PointCloud != null)
                return PointCloud.Name;

            return base.ToString();
        }
      
     
      
      
      
     
   
    }
}