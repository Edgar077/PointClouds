
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Reflection;
using OpenTK;

namespace OpenTKExtension
{
    public static class ExamplePointClouds
    {
        private static float rCyl = 1f;
        //private static float rCon = 1f;
        //private static float rSph = 1f;

    

        private static float[] CylFunction(float u, float v)
        {
            float[] listOfPoints = new float[6] { 0.0f, 0.0f, 0.0f, (float)Math.Cos((float)u), (float)Math.Sin((float)u), 0.0f };
            listOfPoints[0] = ExamplePointClouds.rCyl * listOfPoints[3];
            listOfPoints[1] = ExamplePointClouds.rCyl * listOfPoints[4];
            listOfPoints[2] = v;
            return listOfPoints;
        }
        /// <summary>Generates a 3D PointCloud for a cone.</summary>
     

        /// <summary>
        /// Generates a 3D PointCloud for a cuboid
        /// </summary>
        /// <param name="Name">PointCloud name</param>
        /// <param name="u">Length of the lower part</param>
        /// <param name="v">Length of the high part</param>
        /// <param name="numberOfPoints">Number of points to use in circumference</param>
        /// <param name="Color">Color vector</param>
        /// <param name="TextureBitmap">Texture bitmap. Null uses no texture</param>
        /// <returns></returns>
        public static PointCloud Cuboid(string Name, float u, float v, int numberOfPoints, System.Drawing.Color color, System.Drawing.Bitmap TextureBitmap)
        {

            PointCloud points = PointCloud.CreateCuboid(u, v, numberOfPoints);
            PointCloud.SetColorOfListTo(points, color);

          

            return points;

        }
        public static Vector3[] Cuboid(int numberOfPoints, float u, float v)
        {

            Vector3[] arr = new Vector3[numberOfPoints * 4];

            float v0 = 0f;
            int indexInPointCloud = 0;
            for (int i = 0; i < numberOfPoints; i++)
            {

                arr[indexInPointCloud] = new Vector3(0, v0, 0);
                indexInPointCloud++;
                arr[indexInPointCloud] = new Vector3(0, v0, u);
                indexInPointCloud++;
                arr[indexInPointCloud] = new Vector3(u, v0, u);
                indexInPointCloud++;
                arr[indexInPointCloud] = new Vector3(u, v0, 0);

                v0 += v / numberOfPoints;

            }

            return arr;

        }
        /// <summary>
        /// Generates a 3D PointCloud for a cuboid
        /// </summary>
        /// <param name="Name">PointCloud name</param>
        /// <param name="u">Length of the lower part</param>
        /// <param name="v">Length of the high part</param>
        /// <param name="numberOfPoints">Number of points to use in circumference</param>
        /// <param name="Color">Color vector</param>
        /// <returns></returns>
        public static PointCloud Cuboid(float u, float v, int numberOfPoints, System.Drawing.Color color)
        {
            PointCloud pcl = new PointCloud();
            pcl.Vectors = ExamplePointClouds.Cuboid(numberOfPoints, u, v);
            pcl.SetColor(new Vector3(color.R, color.G, color.B));


            return pcl;


        }

        public static List<Vector3> Square(float cubeSizeX, float cubeSizeY, float z)
        {


            List<Vector3> listVectors = new List<Vector3>();

            listVectors.Add(new Vector3(-cubeSizeX / 2, -cubeSizeY / 2, z));
            listVectors.Add(new Vector3(cubeSizeX / 2, -cubeSizeY / 2, z));
            listVectors.Add(new Vector3(cubeSizeX / 2, cubeSizeY / 2, z));
            listVectors.Add(new Vector3(-cubeSizeX / 2, cubeSizeY / 2, z));

         


            return listVectors;

        }

        public static List<Vector3> Cuboid_Corners_CenteredAt0(float cubeSizeX, float cubeSizeY, float cubeSizeZ)
        {

            List<Vector3> listVectors = new List<Vector3>();

            listVectors.Add(new Vector3(-cubeSizeX / 2, -cubeSizeY / 2, cubeSizeZ / 2));
            listVectors.Add(new Vector3(cubeSizeX / 2, -cubeSizeY / 2, cubeSizeZ / 2));
            listVectors.Add(new Vector3(cubeSizeX / 2, cubeSizeY / 2, cubeSizeZ / 2));
            listVectors.Add(new Vector3(-cubeSizeX / 2, cubeSizeY / 2, cubeSizeZ / 2));

            listVectors.Add(new Vector3(-cubeSizeX / 2, -cubeSizeY / 2, -cubeSizeZ / 2));
            listVectors.Add(new Vector3(cubeSizeX / 2, -cubeSizeY / 2, -cubeSizeZ / 2));
            listVectors.Add(new Vector3(cubeSizeX / 2, cubeSizeY / 2, -cubeSizeZ / 2));
            listVectors.Add(new Vector3(-cubeSizeX / 2, cubeSizeY / 2, -cubeSizeZ / 2));

            return listVectors;

        }
        public static PointCloud CreateCube24()
        {
            Vector3[] vArray = new Vector3[] {
                new Vector3(1, 1, -1), new Vector3(-1, 1, -1), new Vector3(-1, 1, 1), new Vector3(1, 1, 1),
                new Vector3(1, -1, 1), new Vector3(-1, -1, 1), new Vector3(-1, -1, -1), new Vector3(1, -1, -1),
                new Vector3(1, 1, 1), new Vector3(-1, 1, 1), new Vector3(-1, -1, 1), new Vector3(1, -1, 1),
                new Vector3(1, -1, -1), new Vector3(-1, -1, -1), new Vector3(-1, 1, -1), new Vector3(1, 1, -1),
                new Vector3(-1, 1, 1), new Vector3(-1, 1, -1), new Vector3(-1, -1, -1), new Vector3(-1, -1, 1),
                new Vector3(1, 1, -1), new Vector3(1, 1, 1), new Vector3(1, -1, 1), new Vector3(1, -1, -1) };

            PointCloud pc = new PointCloud(vArray, null, null, null, null, null);
            pc.SetDefaultIndices();
            return pc;
        }

       
        public static List<Vector3> Cuboid_RegularGrid_Filled(float cubeSize, int numberOfPointsPerPlane)
        {
            List<Vector3> listVectors = new List<Vector3>();

            float startP = cubeSize;


            for (int i = 0; i <= numberOfPointsPerPlane; i++)
            {
                for (int j = 0; j <= numberOfPointsPerPlane; j++)
                {

                    for (int k = 0; k <= numberOfPointsPerPlane; k++)
                    {
                        Vector3 v = new Vector3(startP * (-.5f + i / Convert.ToSingle(numberOfPointsPerPlane)), startP * (-0.5f + j / Convert.ToSingle(numberOfPointsPerPlane)), startP * (-0.5f + k / Convert.ToSingle(numberOfPointsPerPlane)));
                        listVectors.Add(v);

                    }

                }
            }
            return listVectors;
        }
        public static PointCloud Cube_RegularGrid_Empty(float cubeSize, int numberOfPointsPerPlane)
        {
            return PointCloud.FromListVector3(Cube_RegularGrid_Empty_List(cubeSize, numberOfPointsPerPlane));

            
        }
        public static List<Vector3> Cube_RegularGrid_Empty_List(float cubeSize, int numberOfPointsPerPlane)
        {
            List<Vector3> points = new List<Vector3>();

            float startP = cubeSize;

            List<Vector3> pointsList = new List<Vector3>();

            
            for (int i = 0; i <= numberOfPointsPerPlane; i++)
            {
                for (int j = 0; j <= numberOfPointsPerPlane; j++)
                {
                    if ((i == 0 || i == numberOfPointsPerPlane) || (j == 0 || j == numberOfPointsPerPlane))
                    {
                        for (int k = 0; k <= numberOfPointsPerPlane; k++)
                        {
                            
                            Vector3 v = new Vector3(startP * (-.5f + i / Convert.ToSingle(numberOfPointsPerPlane)), startP * (-0.5f + j / Convert.ToSingle(numberOfPointsPerPlane)), startP * (-0.5f + k / Convert.ToSingle(numberOfPointsPerPlane)));
                            pointsList.Add(v);

                        }
                    }

                    else
                    {
                        
                        Vector3 v = new Vector3(startP * (-.5f + i / Convert.ToSingle(numberOfPointsPerPlane)), startP * (-0.5f + j / Convert.ToSingle(numberOfPointsPerPlane)), startP * (-0.5f));
                        pointsList.Add(v);

                        
                        v = new Vector3(startP * (-.5f + i / Convert.ToSingle(numberOfPointsPerPlane)), startP * (-0.5f + j / Convert.ToSingle(numberOfPointsPerPlane)), startP * (0.5f));
                        pointsList.Add(v);
                    }
                }
            }
            points = pointsList;
            return points;
        }
        /// <summary>
        /// Generates a 3D PointCloud for a cuboid, by setting all lines with points
        /// </summary>
        /// <param name="Name">PointCloud name</param>
        /// <param name="u">Length of the lower part</param>
        /// <param name="v">Length of the high part</param>
        /// <param name="numberOfPoints">Number of points to use in circumference</param>
        /// <param name="Color">Color vector</param>
        /// <returns></returns>
        public static PointCloud Cuboid_AllLines(string Name, float u, float v, int numberOfPoints, System.Drawing.Color color)
        {

            PointCloud points = new PointCloud();

            float u0 = 0f;
            float v0 = 0f;
            List<Vector3> pointsList = new List<Vector3>();
            
            for (int i = 0; i < numberOfPoints; i++)
            {
                pointsList.Add(new Vector3(u0, 0, 0));
                pointsList.Add(new Vector3(0, 0, u0));
                pointsList.Add(new Vector3(u0, 0, u));
                pointsList.Add(new Vector3(u, 0, u0));
                pointsList.Add(new Vector3(0, v0, 0));
                pointsList.Add(new Vector3(0, v0, u));
                pointsList.Add(new Vector3(u, v0, u));
                pointsList.Add(new Vector3(u, v0, 0));
                pointsList.Add(new Vector3(u0, v, 0));
                pointsList.Add(new Vector3(0, v, u0));
                pointsList.Add(new Vector3(u0, v, u));
                pointsList.Add(new Vector3(u, v, u0));

                u0 += u / 100;
                v0 += v / 100;


            }

            
            PointCloud myPointCloud = new PointCloud();
            myPointCloud.Vectors = pointsList.ToArray();
            myPointCloud.CreateIndicesDefault();
            return myPointCloud;

        }
        /// <summary>Generates a 3D PointCloud for a cylinder.</summary>
        /// <param name="Name">PointCloud name.</param>
        /// <param name="Radius">Cylinder radius.</param>
        /// <param name="Height">Cylinder height.</param>
        /// <param name="numPoints">Number of points for circular section.</param>
        /// <param name="Color">Color vector.</param>
        public static PointCloud Cuboid(float xMax, float yMax, float zMax, int pointsMaxX, int pointsMaxY, int pointsMaxZ)
        {
            float stepX = xMax / pointsMaxX;
            float stepY = yMax / pointsMaxY;
            float stepZ = zMax / pointsMaxZ;

            PointCloud pCloud = new PointCloud();
            int indexInPointCloud = -1;
            List<Vector3> pointsList = new List<Vector3>();
            for (int i = 0; i <= pointsMaxX; i++)
            {
                for (int j = 0; j <= pointsMaxY; j++)
                {
                    for (int k = 0; k <= pointsMaxZ; k++)
                    {
                        indexInPointCloud++;
                        Vector3 v = new Vector3(i * stepX, j * stepY, k * stepZ);
                        pointsList.Add(v);
                    }
                }

            }
            pCloud.Vectors = pointsList.ToArray();
            pCloud.CreateIndicesDefault();
            return pCloud;
        }
        /// <summary>Generates a 3D PointCloud for a cuboid.</summary>
        /// <param name="Name">PointCloud name.</param>
        /// <param name="Radius">Cylinder radius.</param>
        /// <param name="Height">Cylinder height.</param>
        /// <param name="numPoints">Number of points for circular section.</param>
        /// <param name="Color">Color vector.</param>
        public static PointCloud CuboidEmpty(float xMax, float yMax, float zMax, int pointsMaxX, int pointsMaxY, int pointsMaxZ)
        {
            float stepX = xMax / pointsMaxX;
            float stepY = yMax / pointsMaxY;
            float stepZ = zMax / pointsMaxZ;

            PointCloud pCloud = new PointCloud();
            List<Vector3> pointsList = new List<Vector3>();
           
            List<uint> listIndices = new List<uint>();
            uint ind = 0;
            for (int i = 0; i <= pointsMaxX; i++)
            {
                for (int j = 0; j <= pointsMaxY; j++)
                {

                    if ((i == 0 || i == pointsMaxX) || (j == 0 || j == pointsMaxY))
                    {
                        for (int k = 0; k <= pointsMaxZ; k++)
                        {
                            
                            Vector3 v = new Vector3(i * stepX, j * stepY, k * stepZ);
                            pointsList.Add(v);
                           
                            if (pointsList.Count > 3)
                            {
                                ind++;
                                listIndices.Add(ind - 1);
                                listIndices.Add(ind);

                                listIndices.Add(ind - 2);
                                listIndices.Add(ind);

                                listIndices.Add(ind - 3);
                                listIndices.Add(ind);

                                listIndices.Add(ind - 4);
                                listIndices.Add(ind);

                            }
                            if (pointsList.Count > 2)
                            {
                                ind++;
                                listIndices.Add(ind - 1);
                                listIndices.Add(ind);

                                listIndices.Add(ind - 2);
                                listIndices.Add(ind);

                                listIndices.Add(ind - 3);
                                listIndices.Add(ind);

                            }
                            if (pointsList.Count > 1)
                            {
                                ind++;
                                listIndices.Add(ind - 1);
                                listIndices.Add(ind);

                                listIndices.Add(ind - 2);
                                listIndices.Add(ind);

                            }
                            if (pointsList.Count > 0)
                            {
                                listIndices.Add(ind);
                                listIndices.Add(ind++);
                            }
                           
                           
                           

                        }
                    }

                    else
                    {

                        Vector3 v = new Vector3(i * stepX, j * stepY, 0);
                        pointsList.Add(v);


                        v = new Vector3(i * stepX, j * stepY, zMax);
                        pointsList.Add(v);


                    }



                }

            }
            pCloud.Vectors = pointsList.ToArray();
            pCloud.Indices = listIndices.ToArray();
            //pCloud.CreateIndicesDefault();
            return pCloud;
        }

        /// <summary>Generates a 3D PointCloud for a cylinder.</summary>
        /// <param name="Name">PointCloud name.</param>
        /// <param name="Radius">Cylinder radius.</param>
        /// <param name="Height">Cylinder height.</param>
        /// <param name="numPoints">Number of points for circular section.</param>
        /// <param name="Color">Color vector.</param>
        public static PointCloud Rectangle(float xMax, float yMax, int pointsMaxX, int pointsMaxY)
        {
            float stepX = xMax / pointsMaxX;
            float stepY = yMax / pointsMaxY;

            PointCloud pCloud = new PointCloud();
            List<Vector3> pointsList = new List<Vector3>();
            int indexInPointCloud = -1;
            for (int i = 0; i < pointsMaxX; i++)
            {
                for (int j = 0; j < pointsMaxY; j++)
                {
                    indexInPointCloud++;
                    Vector3 v = new Vector3(i * stepX, j * stepY, 0);
                    pointsList.Add(v);

                }

            }

            pCloud.Vectors = pointsList.ToArray();
            pCloud.CreateIndicesDefault();
            return pCloud;
        }
    }
}
