using System.Collections.Generic;
using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
namespace OpenTKExtension.FastGLControl
{

    public class CubePerspective : RenderableObject
    {
        

        public CubePerspective() : this(new Vector3(1, 1, 1))
        {
            this.primitiveType = PrimitiveType.Triangles;
        }
     

            
        public CubePerspective(Vector3 col)
        {
            this.primitiveType = PrimitiveType.Triangles;
            color = col;


            if (InitShaders("cubePerspective.vert", "cubePerspective.frag", path + "Shaders\\"))
            {
                this.initBuffers();
                this.FillPointCloud();
                this.FillIndexBuffer();
                //this.RefreshRenderableData();
            }

        }
      
        public override void Dispose()
        {

            base.Dispose();
        }

       
        public override void FillPointCloud()
        {


            this.PointCloud.Vectors = new Vector3[]{
            new Vector3(-1.0f, -1.0f,  1.0f),
            new Vector3( 1.0f, -1.0f,  1.0f),
            new Vector3( 1.0f,  1.0f,  1.0f),
            new Vector3(-1.0f,  1.0f,  1.0f),
            new Vector3(-1.0f, -1.0f, -1.0f),
            new Vector3( 1.0f, -1.0f, -1.0f), 
            new Vector3( 1.0f,  1.0f, -1.0f),
            new Vector3(-1.0f,  1.0f, -1.0f) };



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
                1, 5, 6, 6, 2, 1 };


        }

        
    }
}