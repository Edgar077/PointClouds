using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.IO;
using System.Globalization;
using System.Drawing;

namespace OpenTKExtension
{

    public partial class PointCloud 
    {
        public static PointCloud CreateSomePoints()
        {


            float[] origin = { 0.0f, 0.0f, 0.0f };
            List<Vector3> list = new List<Vector3>();
            list.Add(new Vector3(100, 0, 0));
            list.Add(new Vector3(0, 100, 0));
            list.Add(new Vector3(0, 0, 100));

            return PointCloud.FromListVector3(list);

        }
        public static PointCloud CreateCube_RegularGrid_Empty(float cubeSize, int numberOfPointsPerPlane)
        {
            List<Vector3> listVectors = ExamplePointClouds.Cube_RegularGrid_Empty_List(cubeSize, numberOfPointsPerPlane);
            return FromListVector3(listVectors);


        }
        public static PointCloud CreateCube_Corners_CenteredAt0(float cubeSizeX)
        {
            List<Vector3> listVectors = ExamplePointClouds.Cuboid_Corners_CenteredAt0(cubeSizeX, cubeSizeX, cubeSizeX);

            return FromListVector3(listVectors);

        }
        public static PointCloud CreateCube_Corners_StartAt0(float cubeSizeX)
        {
            List<Vector3> listVectors = ExamplePointClouds.Cuboid_Corners_CenteredAt0(cubeSizeX, cubeSizeX, cubeSizeX);
            PointCloud pc = FromListVector3(listVectors);
            float shiftVectors = cubeSizeX / 2;
            pc.Translate(shiftVectors, shiftVectors, shiftVectors);
            return pc;

        }
     
        public static PointCloud CreateCube_RandomPointsOnPlanes(float cubeSize, int numberOfRandomPoints)
        {

            List<Vector3> points = ExamplePointClouds.Cuboid_Corners_CenteredAt0(cubeSize, cubeSize, cubeSize);

            var r = new Random();


            for (var i = 0; i < numberOfRandomPoints; i++)
            {

                var vi = new Vector3(cubeSize / 2, cubeSize * Convert.ToSingle(r.NextDouble()) - cubeSize / 2, cubeSize * Convert.ToSingle(r.NextDouble()) - cubeSize / 2);
                points.Add(vi);

            }
            for (var i = 0; i < numberOfRandomPoints; i++)
            {

                var vi = new Vector3(-cubeSize / 2, cubeSize * Convert.ToSingle(r.NextDouble()) - cubeSize / 2, cubeSize * Convert.ToSingle(r.NextDouble()) - cubeSize / 2);
                points.Add(vi);

            }


            for (var i = 0; i < numberOfRandomPoints; i++)
            {

                var vi = new Vector3(cubeSize * Convert.ToSingle(r.NextDouble()) - cubeSize / 2, cubeSize / 2, cubeSize * Convert.ToSingle(r.NextDouble()) - cubeSize / 2);
                points.Add(vi);

            }
            for (var i = 0; i < numberOfRandomPoints; i++)
            {

                var vi = new Vector3(
                    cubeSize * Convert.ToSingle(r.NextDouble()) - cubeSize / 2,
                    -cubeSize / 2,
                    cubeSize * Convert.ToSingle(r.NextDouble()) - cubeSize / 2);
                points.Add(vi);

            }


            for (var i = 0; i < numberOfRandomPoints; i++)
            {

                var vi = new Vector3(cubeSize * Convert.ToSingle(r.NextDouble()) - cubeSize / 2, cubeSize * Convert.ToSingle(r.NextDouble()) - cubeSize / 2, -cubeSize / 2);
                points.Add(vi);

            }
            for (var i = 0; i < numberOfRandomPoints; i++)
            {

                var vi = new Vector3(cubeSize * Convert.ToSingle(r.NextDouble()) - cubeSize / 2, cubeSize * Convert.ToSingle(r.NextDouble()) - cubeSize / 2, cubeSize / 2);
                points.Add(vi);

            }
            PointCloud pointCloud = new PointCloud();
            pointCloud.Vectors = points.ToArray();
            return pointCloud;
        }

        public static PointCloud CreateCuboid(float u, float v, int numberOfPoints)
        {
            List<Vector3> points = new List<Vector3>();
            float v0 = 0f;
           
            for (int i = 0; i < numberOfPoints; i++)
            {
                
                points.Add(new Vector3(0, v0, 0));
               
                points.Add(new Vector3(0, v0, u));
                
                points.Add(new Vector3(u, v0, u));
               
                points.Add(new Vector3(u, v0, 0));

                v0 += v / numberOfPoints;

            }

            return PointCloud.FromListVector3(points);

        }

        public static PointCloud CreateSphere_RandomPoints(float cubeSize, int numberOfRandomPoints)
        {

            var r = new Random();
            /****** Random Vertices ******/
            List<Vector3> points = new List<Vector3>();
            for (var i = 0; i < numberOfRandomPoints; i++)
            {
                var radius = cubeSize * r.NextDouble();
                // if (i < NumberOfVertices / 2) radius /= 2;
                float theta = Convert.ToSingle(2 * Math.PI * r.NextDouble());
                float azimuth = Convert.ToSingle(Math.PI * r.NextDouble());
                float x = Convert.ToSingle(radius * Math.Cos(theta) * Math.Sin(azimuth));
                float y = Convert.ToSingle(radius * Math.Sin(theta) * Math.Sin(azimuth));
                float z = Convert.ToSingle(radius * Math.Cos(azimuth));
                Vector3 vi = new Vector3(x, y, z);
                points.Add(vi);
            }
            PointCloud pointCloud = new PointCloud();
            pointCloud.Vectors = points.ToArray();
            return pointCloud;

        }
        #region model helpers

        public static void SetIndicesForCubeCorners(PointCloud pcl)
        {
            pcl.Indices = new uint[24];


            pcl.Indices[0] = 0;
            pcl.Indices[1] = 1;
            pcl.Indices[2] = 1;
            pcl.Indices[3] = 5;
            pcl.Indices[4] = 5;
            pcl.Indices[5] = 4;
            pcl.Indices[6] = 4;
            pcl.Indices[7] = 0;

            pcl.Indices[8] = 1;
            pcl.Indices[9] = 2;
            pcl.Indices[10] = 2;
            pcl.Indices[11] = 3;
            pcl.Indices[12] = 3;
            pcl.Indices[13] = 0;
            pcl.Indices[14] = 3;
            pcl.Indices[15] = 7;
            pcl.Indices[16] = 7;
            pcl.Indices[17] = 6;
            pcl.Indices[18] = 2;
            pcl.Indices[19] = 6;
            pcl.Indices[20] = 4;
            pcl.Indices[21] = 7;
            pcl.Indices[22] = 5;
            pcl.Indices[23] = 6;


        }
        #endregion

    }
}
