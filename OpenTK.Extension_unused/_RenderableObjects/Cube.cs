using System.Collections.Generic;

using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Extension;

namespace OpenTK.Extension
{
    public class Cube : RenderableObject
    {
        public override void FillPointCloud()
        {

            this.PointCloudGL.Vectors = new Vector3[]
            {
            new Vector3(1, 1, -1), new Vector3(-1, 1, -1), new Vector3(-1, 1, 1), new Vector3(1, 1, 1),         // top
                new Vector3(1, -1, 1), new Vector3(-1, -1, 1), new Vector3(-1, -1, -1), new Vector3(1, -1, -1),     // bottom
                new Vector3(1, 1, 1), new Vector3(-1, 1, 1), new Vector3(-1, -1, 1), new Vector3(1, -1, 1),         // front face
                new Vector3(1, -1, -1), new Vector3(-1, -1, -1), new Vector3(-1, 1, -1), new Vector3(1, 1, -1),     // back face
                new Vector3(-1, 1, 1), new Vector3(-1, 1, -1), new Vector3(-1, -1, -1), new Vector3(-1, -1, 1),     // left
                new Vector3(1, 1, -1), new Vector3(1, 1, 1), new Vector3(1, -1, 1), new Vector3(1, -1, -1) 
            };

         

            this.PointCloudGL.Colors = new Vector3[8];
            this.PointCloudGL.Colors[0] = new Vector3(1f, 0f, 0f);
            this.PointCloudGL.Colors[1] = new Vector3(0f, 0f, 1f);
            this.PointCloudGL.Colors[2] = new Vector3(0f, 1f, 0f);
            this.PointCloudGL.Colors[3] = new Vector3(1f, 0f, 0f);
            this.PointCloudGL.Colors[4] = new Vector3(0f, 0f, 1f);
            this.PointCloudGL.Colors[5] = new Vector3(0f, 1f, 0f);
            this.PointCloudGL.Colors[6] = new Vector3(1f, 0f, 0f);
            this.PointCloudGL.Colors[7] = new Vector3(0f, 0f, 1f);

         
            
            this.PointCloudGL.UVS = new Vector2[] {
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1),
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1),
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1),
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1),
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1),
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1) };
           
            List<uint> triangles = new List<uint>();
            for (uint i = 0; i < 6; i++)
            {
                triangles.Add(i * 4);
                triangles.Add(i * 4 + 1);
                triangles.Add(i * 4 + 2);
                triangles.Add(i * 4);
                triangles.Add(i * 4 + 2);
                triangles.Add(i * 4 + 3);
            }
            this.PointCloudGL.Triangles = triangles.ToArray();
          

            this.PointCloudGL.Normals = Geometry.CalculateNormals(this.PointCloudGL.Vectors, triangles.ToArray());
          
            this.PointCloudGL.Tangents = Geometry.CalculateTangents(this.PointCloudGL.Vectors, this.PointCloudGL.Normals, triangles.ToArray(), this.PointCloudGL.UVS);
           
            brickDiffuse = new Texture("Textures\\AlternatingBrick-ColorMap.png");
            brickNormals = new Texture("Textures\\AlternatingBrick-NormalMap.png");

        }
        public override void Reset()
        {
            this.Dispose();

            brickDiffuse = new Texture("Textures\\AlternatingBrick-ColorMap.png");
            brickNormals = new Texture("Textures\\AlternatingBrick-NormalMap.png");
            
            InitializeGL();
        }
   
    }
}
