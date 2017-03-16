using System.Collections.Generic;
using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using OpenTKExtension;

namespace OpenTKExtension
{

    public class PointCloudRenderable : RenderableObject
    {

        public PointCloudRenderable(Vector3 mycolor)
        {
            color = mycolor;
        }
        public PointCloudRenderable() : this(new Vector3(1, 1, 1))
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
                System.Diagnostics.Debug.Assert(false, "SW Error - please set the point cloud data f5irst ");
                return;

            }

            bool shadersInitialized = false;
            if (this.PointCloud.Texture == null)
            {
                shadersInitialized = InitShaders("PointCloud.vert", "PointCloud.frag", path + "Shaders\\");
            }
            else
                shadersInitialized = InitShaders("cubeTexture.vert", "cubeTexture.frag", path + "Shaders\\");

            if (shadersInitialized)
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
                if (PointCloud.Texture != null)
                    PointCloud.Texture.InitGL(true);

                //point cloud data, indices, colors etc. are set before...
                //this.RefreshRenderableData();
            }

        }




       
        public override string ToString()
        {
            if (PointCloud != null)
                return PointCloud.Name;

            return base.ToString();
        }

        #region IDisposable

        public override void Dispose()
        {

            base.Dispose();
        }
        #endregion


        public static string VertexShader = @"
#version 130

layout(location = 0) in vec3 vVertex;  //object space vertex position
layout(location = 1) in vec2 vUV;

out vec2 uv;
uniform mat4 MVP;

void main(void)
{
    uv = vUV;
    gl_Position = MVP*vec4(vVertex,1);
}
";

        public static string FragmentShader = @"
#version 130

uniform sampler2D texture;
in vec2 uv;
out vec4 fragment;

void main(void)
{
    fragment = texture2D(texture, uv);
}
";
      
     
   
    }
}