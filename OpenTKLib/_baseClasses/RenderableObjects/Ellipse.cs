using System.Collections.Generic;
using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using OpenTKExtension;



namespace OpenTKExtension
{

    public class Ellipse : RenderableObject
    {
        private List<Vector3> listPoints;
        //private List<Vector3> boundingBox;

        public Ellipse(float a, float b, float z)
        {

            listPoints = new List<Vector3>();

            for (int i = 0; i < 360; i++)
            {
                float t = Convert.ToSingle(MathBase.DegreesToRadians_Float * i);
                Vector3 v = new OpenTK.Vector3(Convert.ToSingle(a * Math.Sin(t)), Convert.ToSingle(b * Math.Cos(t)), Convert.ToSingle(z));
                listPoints.Add(v);
            }

            FillPointCloud();
            FillIndexBuffer();

        }
      
     
        public override void InitializeGL()
        {
            this.primitiveType = PrimitiveType.Lines;

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
      
        public override void FillPointCloud()
        {

            this.PointCloud.Vectors = new Vector3[listPoints.Count * 2 - 1];

            int vectIndex = -1;
            for (int i = 1; i < listPoints.Count; i++)
            {
                vectIndex++;
                this.PointCloud.Vectors[vectIndex] = listPoints[i - 1];
                vectIndex++;
                this.PointCloud.Vectors[vectIndex] = listPoints[i];

            }

            
            this.PointCloud.Colors = new Vector3[this.PointCloud.Vectors.Length];
            for (int i = 0; i < this.PointCloud.Vectors.Length; i++)
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
     
        //public List<Vector3> CalculateBoundingBox(List<Vector3> vectors)
        //{

        //    if (vectors.Count == 0)
        //        return null;

        //    List<Vector3> resultList = new List<OpenTK.Vector3>();
        //    Vector3 maxPoint = new Vector3();
        //    Vector3 minPoint = new Vector3();

        //    float xMax = vectors[0].X;
        //    float yMax = vectors[0].Y;
        //    float zMax = vectors[0].Z;
        //    float xMin = vectors[0].X;
        //    float yMin = vectors[0].Y;
        //    float zMin = vectors[0].Z;
        //    for (int i = 0; i < vectors.Count; i++)
        //    {
        //        Vector3 ver = vectors[i];
        //        if (ver.X > xMax)
        //            xMax = ver.X;
        //        if (ver.Y > yMax)
        //            yMax = ver.Y;
        //        if (ver.Z > zMax)
        //            zMax = ver.Z;
        //        if (ver.X < xMin)
        //            xMin = ver.X;
        //        if (ver.Y < yMin)
        //            yMin = ver.Y;
        //        if (ver.Z < zMin)
        //            zMin = ver.Z;
        //    }
        //    maxPoint.X = xMax;
        //    maxPoint.Y = yMax;
        //    maxPoint.Z = zMax;

        //    minPoint.X = xMin;
        //    minPoint.Y = yMin;
        //    minPoint.Z = zMin;

        //    resultList.Add(minPoint);
        //    resultList.Add(maxPoint);

        //    return resultList;

        //}

      
        
     
   
    }
}