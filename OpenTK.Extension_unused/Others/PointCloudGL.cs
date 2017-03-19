using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace OpenTK.Extension
{

    public class PointCloudGL
    {
        public Vector3[] Vectors;
        public Vector3[] Colors;
        public Vector3[] Normals;
        public Vector3[] Tangents;
        public uint[] Triangles;

        public Vector2[] UVS;
        //triangle part
        public uint[] IndicesNormals;
        public uint[] IndicesTexture;

        private Vector3 centroid;
        Vector3 boundingBoxMax;
        Vector3 boundingBoxMin;

        bool centroidAndBoundingBoxCalculated;
        private Vector3 centroidOld;
        public PointCloudGL()
        {
        }
        public PointCloudGL(int dim)
        {
            Vectors = new Vector3[dim];
            Colors = new Vector3[dim];
            Triangles = new uint[dim];

        }
        public PointCloudGL(List<Vector3> vectors, List<Vector3> colors, List<Vector3> normals, List<uint> triangles, List<uint> indicesNormals, List<uint> indicesTexture)
        {

            this.Vectors = vectors.ToArray();
            if(colors != null && colors.Count > 0)
                this.Colors = colors.ToArray();
            if (normals != null && normals.Count > 0)
                this.Normals = normals.ToArray();
            if (triangles != null && triangles.Count > 0)
                this.Triangles = triangles.ToArray();
            if (indicesNormals != null && indicesNormals.Count > 0)
                this.IndicesNormals = indicesNormals.ToArray();
            if (indicesTexture != null && indicesTexture.Count > 0)
                this.IndicesTexture = indicesTexture.ToArray();


        }
        public Vector3 CentroidVector
        {
            get
            {
                if (!centroidAndBoundingBoxCalculated)
                    CalculateCentroidBoundingBox();
                return centroid;

            }

        }
        private void CalculateCentroidBoundingBox()
        {
            this.CalculateCentroid();
            PointCloudGL.BoundingBox(this, ref boundingBoxMax, ref boundingBoxMin);
            centroidAndBoundingBoxCalculated = true;
        }
        private Vector3 CalculateCentroid()
        {
            centroid = new Vector3();

            int nCount = Vectors.Length;
            for (int i = 0; i < nCount; i++)
                centroid += Vectors[i];
            centroid /= nCount;
            return centroid;
        }
        public Vector3 BoundingBoxMax
        {
            get
            {
                if (!centroidAndBoundingBoxCalculated)
                    CalculateCentroidBoundingBox();
                return boundingBoxMax;
            }

        }
        public float BoundingBoxMaxFloat
        {
            get
            {
                if (!centroidAndBoundingBoxCalculated)
                    CalculateCentroidBoundingBox();
                float f = float.MinValue;
                for (int i = 0; i < 2; i++)
                {
                    f = System.Math.Max(f, System.Math.Abs(this.boundingBoxMin[i]));
                }
                for (int i = 0; i < 2; i++)
                {
                    f = System.Math.Max(f, System.Math.Abs(boundingBoxMax[i]));
                }

                return f;
            }

        }
        public Vector3 BoundingBoxMin
        {
            get
            {
                if (!centroidAndBoundingBoxCalculated)
                    CalculateCentroidBoundingBox();
                return boundingBoxMin;
            }

        }
        private static void BoundingBox(PointCloudGL pointCloud, ref Vector3 maxPoint, ref Vector3 minPoint)
        {

            int nDim = pointCloud.Vectors.Length;
            if (nDim < 1)
                return;

            maxPoint = new Vector3();
            minPoint = new Vector3();

            float xMax = pointCloud.Vectors[0].X;
            float yMax = pointCloud.Vectors[0].Y;
            float zMax = pointCloud.Vectors[0].Z;
            float xMin = pointCloud.Vectors[0].X;
            float yMin = pointCloud.Vectors[0].Y;
            float zMin = pointCloud.Vectors[0].Z;
            for (int i = 0; i < nDim; i++)
            {
                Vector3 ver = pointCloud.Vectors[i];
                if (ver.X > xMax)
                    xMax = ver.X;
                if (ver.Y > yMax)
                    yMax = ver.Y;
                if (ver.Z > zMax)
                    zMax = ver.Z;
                if (ver.X < xMin)
                    xMin = ver.X;
                if (ver.Y < yMin)
                    yMin = ver.Y;
                if (ver.Z < zMin)
                    zMin = ver.Z;
            }
            maxPoint.X = xMax;
            maxPoint.Y = yMax;
            maxPoint.Z = zMax;

            minPoint.X = xMin;
            minPoint.Y = yMin;
            minPoint.Z = zMin;

        }
        public Vector3 ResetCentroid(bool centered)
        {
            if (centered)
            {
                //center point cloud to centroid

                this.centroidOld = this.CentroidVector;
                SubtractVector(this.CentroidVector);
                //centroid - is now origin
                this.centroid = new Vector3(0, 0, 0);
                //if (pointCloud.ShowLinesConnectingPoints)
                //{
                //    if (pointCloud.LinesGeometricalModels != null)
                //        pointCloud.LinesGeometricalModels_Reset();
                //}
                //pointCloud.CalculateLinesPCA();



            }
            else
            {
                //reset to old center
                if (this.centroidOld != Vector3.Zero)
                {
                    AddVector(this.centroidOld);
                    this.CalculateCentroid();//recalcs centrooid

                    //if (pointCloud.ShowLinesConnectingPoints)
                    //    pointCloud.LinesGeometricalModels_Reset();

                    this.centroidOld = Vector3.Zero;
                    //pointCloud.CalculateLinesPCA();

                }

            }
            return this.centroidOld;


        }
        public void AddVector(Vector3 centroid)
        {

            for (int i = 0; i < this.Vectors.Length; i++)
            {

                Vector3 v = this.Vectors[i];
                Vector3 translatedV = Vector3.Add(v, centroid);
                v = translatedV;
                this.Vectors[i] = v;
            }

        }
        public void SubtractVector(Vector3 centroid)
        {

            for (int i = 0; i < this.Vectors.Length; i++)
            {

                Vector3 v = this.Vectors[i];
                Vector3 translatedV = Vector3.Subtract(v, centroid);
                v = translatedV;
                this.Vectors[i] = v;
            }

        }
        public void ResizeVerticesTo1()
        {
            if (boundingBoxMax == null) 
                CalculateBoundingBox();
            this.SubtractVector(this.boundingBoxMin);

            Vector3 vectorAdjust = boundingBoxMax - boundingBoxMin;

            float d = Math.Max(vectorAdjust.X, vectorAdjust.Y);
            d = Math.Max(d, vectorAdjust.Z);
            if (d > 0)
            {
                //centroid.X /= d;
                //centroid.Y /= d;
                //centroid.Z /= d;
                for (int i = 0; i < this.Vectors.GetLength(0); i++)
                {
                    this.Vectors[i].X /= d;
                    this.Vectors[i].Y /= d;
                    this.Vectors[i].Z /= d;
                }
            }
            this.CalculateCentroid();
            this.ResetCentroid(true);
            this.CalculateCentroidBoundingBox();


        }
        public void CalculateBoundingBox()
        {

            if (this.Vectors.Length < 1)
                return;

            boundingBoxMax = new Vector3();
            boundingBoxMin = new Vector3();

            float xMax = this.Vectors[0].X;
            float yMax = this.Vectors[0].Y;
            float zMax = this.Vectors[0].Z;
            float xMin = this.Vectors[0].X;
            float yMin = this.Vectors[0].Y;
            float zMin = this.Vectors[0].Z;
            for (int i = 0; i < Vectors.Length; i++)
            {

                Vector3 v = Vectors[i];
                if (v.X > xMax)
                    xMax = v.X;
                if (v.Y > yMax)
                    yMax = v.Y;
                if (v.Z > zMax)
                    zMax = v.Z;
                if (v.X < xMin)
                    xMin = v.X;
                if (v.Y < yMin)
                    yMin = v.Y;
                if (v.Z < zMin)
                    zMin = v.Z;
            }
            boundingBoxMax.X = xMax;
            boundingBoxMax.Y = yMax;
            boundingBoxMax.Z = zMax;

            boundingBoxMin.X = xMin;
            boundingBoxMin.Y = yMin;
            boundingBoxMin.Z = zMin;

        }
    }
}
