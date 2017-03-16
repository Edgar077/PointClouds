using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using Microsoft.Kinect;
using System.Windows;
using OpenTKExtension;
using System.Runtime.InteropServices;
using OpenTK;

namespace PointCloudUtils
{
    public partial class MetaDataBase
    {
        public static double[,] CreatePointMatrixFromPoint3DList(List<Point3D> DepthVertex, int width, int height)
        {
            double[,] points = new double[width, height];


            for (int i = 0; i < DepthVertex.Count; i++)
            {

                Point3D p3D = DepthVertex[i];
                points[Convert.ToInt32(DepthVertex[i].X), Convert.ToInt32(DepthVertex[i].Y)] = Convert.ToSingle(DepthVertex[i].Z);

            }
            return points;


        }

        public static byte[] RotateColorInfoForDepth(byte[] points, int width, int height)
        {
            byte[] newDepthFrame = new byte[points.GetLength(0)];

            for (ushort x = 0; x < width; ++x)
            {
                for (ushort y = 0; y < height; ++y)
                {

                    int depthIndex = ((y * width) + x) * BYTES_PER_PIXEL;
                    int newDepthIndex = (((height - y - 1) * width) + x) * BYTES_PER_PIXEL;

                    byte z = points[depthIndex];

                    newDepthFrame[newDepthIndex] = points[depthIndex];
                    newDepthFrame[newDepthIndex + 1] = points[depthIndex + 1];
                    newDepthFrame[newDepthIndex + 2] = points[depthIndex + 2];
                    newDepthFrame[newDepthIndex + 3] = points[depthIndex + 3];



                }
            }
            return newDepthFrame;

        }


        public static void Test_GetCameraPoints(DepthMetaData myDepthMetaData, CoordinateMapper myCoordinateMapper)
        {

            //test
            CameraSpacePoint[] cameraMappedPoints = new CameraSpacePoint[ColorMetaData.XColorMaxKinect * ColorMetaData.YColorMaxKinect];

            myCoordinateMapper.MapColorFrameToCameraSpace(myDepthMetaData.FrameData, cameraMappedPoints);
            List<CameraSpacePoint> myMappedPoints = new List<CameraSpacePoint>();
            for (int i = 0; i < cameraMappedPoints.Length; ++i)
            {
                double colorMappedToDepthX = cameraMappedPoints[i].X;
                double colorMappedToDepthY = cameraMappedPoints[i].Y;
                if (!double.IsNegativeInfinity(colorMappedToDepthX) &&
                           !double.IsNegativeInfinity(colorMappedToDepthY))
                {
                    myMappedPoints.Add(cameraMappedPoints[i]);
                    //System.Diagnostics.Debug.WriteLine("--> Camera Points : " + cameraMappedPoints[colorIndex].X.ToString() + " : " + cameraMappedPoints[colorIndex].Y.ToString() + " : " + cameraMappedPoints[colorIndex].Z.ToString());

                }
            }
            System.Diagnostics.Debug.WriteLine("--> Number of ColorSpacePoints: " + myMappedPoints.Count.ToString());

            //System.Diagnostics.Debug.WriteLine("--> Old.Camera Points : " + cameraMappedPoints[0].X.ToString() + " : " + cameraMappedPoints[0].X.ToString() + " : " + cameraMappedPoints[0].Z.ToString() + " : ");

            DepthSpacePoint[] colorMappedToDepthPoints = new DepthSpacePoint[ColorMetaData.XColorMaxKinect * ColorMetaData.YColorMaxKinect];
            myCoordinateMapper.MapColorFrameToDepthSpace(myDepthMetaData.FrameData, colorMappedToDepthPoints);
            List<DepthSpacePoint> myMappedPointsDepth = new List<DepthSpacePoint>();
            for (int i = 0; i < colorMappedToDepthPoints.Length; ++i)
            {
                double colorMappedToDepthX = colorMappedToDepthPoints[i].X;
                double colorMappedToDepthY = colorMappedToDepthPoints[i].Y;
                if (!double.IsNegativeInfinity(colorMappedToDepthX) &&
                           !double.IsNegativeInfinity(colorMappedToDepthY))
                {
                    myMappedPointsDepth.Add(colorMappedToDepthPoints[i]);
                    //System.Diagnostics.Debug.WriteLine("--> Camera Points : " + cameraMappedPoints[colorIndex].X.ToString() + " : " + cameraMappedPoints[colorIndex].Y.ToString() + " : " + cameraMappedPoints[colorIndex].Z.ToString());

                }
            }
            System.Diagnostics.Debug.WriteLine("--> Number of ColorDepthPoints: " + myMappedPointsDepth.Count.ToString());

            //System.Diagnostics.Debug.WriteLine("--> Depth Points : " + colorMappedToDepthPoints[0].X.ToString() + " : " + colorMappedToDepthPoints[0].X.ToString() + " : " );



        }
    

        /// <summary>
        /// creates color info for all DEPTH pixels (to later e.g. write ply file from it)
        /// </summary>
        /// <param name="myColorMetaData"></param>
        /// <param name="myDepthMetaData"></param>
        /// <param name="myCoordinateMapper"></param>
        /// <returns></returns>
        public static byte[] ColorInfoForDepth(ColorMetaData myColorMetaData, DepthMetaData myDepthMetaData, CoordinateMapper myCoordinateMapper)
        {
            //Test_GetCameraPoints(myDepthMetaData, myCoordinateMapper);

            byte[] mydisplayPixels = new byte[DepthMetaData.XDepthMaxKinect * DepthMetaData.YDepthMaxKinect * MetaDataBase.BYTES_PER_PIXEL];
            //mapped data
            int numberOfPoints = 0;
            int notMappedPixels = -1;
            if (myColorMetaData != null)
            {

                ColorSpacePoint[] mycolorPointsInDepthSpace = new ColorSpacePoint[DepthMetaData.XDepthMaxKinect * DepthMetaData.YDepthMaxKinect];

                myCoordinateMapper.MapDepthFrameToColorSpace(myDepthMetaData.FrameData, mycolorPointsInDepthSpace);


                for (int x = 0; x < DepthMetaData.XDepthMaxKinect; ++x)
                {
                    for (int y = 0; y < DepthMetaData.YDepthMaxKinect; ++y)
                    {
                        int depthIndex = (y * DepthMetaData.XDepthMaxKinect) + x;

                        if (myDepthMetaData.FrameData[depthIndex] != 0)
                        {
                            ColorSpacePoint colorPoint = mycolorPointsInDepthSpace[depthIndex];


                            int xColor = Convert.ToInt32(colorPoint.X);
                            int yColor = Convert.ToInt32(colorPoint.Y);
                            int depthIndexColor = depthIndex * MetaDataBase.BYTES_PER_PIXEL;

                            if ((xColor >= 0) && (xColor < ColorMetaData.XColorMaxKinect) && (yColor >= 0) && (yColor < ColorMetaData.YColorMaxKinect))
                            {
                                int colorIndex = ((yColor * ColorMetaData.XColorMaxKinect) + xColor) * MetaDataBase.BYTES_PER_PIXEL;

                                mydisplayPixels[depthIndexColor + 0] = myColorMetaData.Pixels[colorIndex];
                                mydisplayPixels[depthIndexColor + 1] = myColorMetaData.Pixels[colorIndex + 1];
                                mydisplayPixels[depthIndexColor + 2] = myColorMetaData.Pixels[colorIndex + 2];
                                mydisplayPixels[depthIndexColor + 3] = 0xff;
                                numberOfPoints++;
                            }
                            else
                            {
                                notMappedPixels++;
                                mydisplayPixels[depthIndexColor + 0] = 255;
                                mydisplayPixels[depthIndexColor + 1] = 255;
                                mydisplayPixels[depthIndexColor + 2] = 255;
                                mydisplayPixels[depthIndexColor + 3] = 0xff;

                            }
                        }
                    }
                }
            }
            //System.Diagnostics.Debug.WriteLine("---------> Number of Depth points: " + numberOfPoints.ToString());


            return mydisplayPixels;

        }



        private static PointCloud PointCloudWithColorParallel_AllPoints(ColorSpacePoint[] mycolorPointsInDepthSpace, CameraSpacePoint[] myRealWorldPoints, ColorMetaData myColorMetaData, DepthMetaData myDepthMetaData)
        {

            VectorColor[,] arrVectors = Helper_CreateArray_AllPoints(mycolorPointsInDepthSpace, myRealWorldPoints, myColorMetaData, myDepthMetaData);
            PointCloud pc = new PointCloud();
            pc.Vectors = new Vector3[arrVectors.Length];
            pc.Colors = new Vector3[arrVectors.Length];

            int indV = -1;
            for (int x = 0; x < DepthMetaData.XDepthMaxKinect; x++)
            {
                for (int y = 0; y < DepthMetaData.YDepthMaxKinect; y++)
                {
                    indV++;
                    pc.Vectors[indV] = arrVectors[x, y].Vector;
                    pc.Colors[indV] = arrVectors[x, y].Color;
                    
                }
            }


            return pc;
        }
        private static VectorColor[,] Helper_CreateArray_AllPoints(ColorSpacePoint[] mycolorPointsInDepthSpace, CameraSpacePoint[] myRealWorldPoints, ColorMetaData myColorMetaData, DepthMetaData myDepthMetaData)
        {

            VectorColor[,] arrVectors = new VectorColor[DepthMetaData.XDepthMaxKinect, DepthMetaData.YDepthMaxKinect];

            try
            {

                //for (int x = 0; x < DepthMetaData.XResDefault; x++)
                System.Threading.Tasks.Parallel.For(0, DepthMetaData.XDepthMaxKinect, x =>
                {
                    for (int y = 0; y < DepthMetaData.YDepthMaxKinect; y++)
                    {
                        int depthIndex = (y * DepthMetaData.XDepthMaxKinect) + x;

                        if (myDepthMetaData.FrameData[depthIndex] != 0)
                        {


                            ColorSpacePoint colorPoint = mycolorPointsInDepthSpace[depthIndex];


                            int xColor = Convert.ToInt32(colorPoint.X);
                            int yColor = Convert.ToInt32(colorPoint.Y);
                            int depthIndexColor = depthIndex * MetaDataBase.BYTES_PER_PIXEL;

                            if ((xColor >= 0) && (xColor < ColorMetaData.XColorMaxKinect) && (yColor >= 0) && (yColor < ColorMetaData.YColorMaxKinect))
                            {
                                int colorIndex = ((yColor * ColorMetaData.XColorMaxKinect) + xColor) * MetaDataBase.BYTES_PER_PIXEL;
                                Vector3 vect = new Vector3(myRealWorldPoints[depthIndex].X, myRealWorldPoints[depthIndex].Y, myRealWorldPoints[depthIndex].Z);
                                Vector3 col = new Vector3(myColorMetaData.Pixels[colorIndex] / 255f, myColorMetaData.Pixels[colorIndex + 1] / 255f, myColorMetaData.Pixels[colorIndex + 2] / 255f);
                                arrVectors[x, y].Vector = vect;

                                arrVectors[x, y].Color = col;

                            }
                            else
                            {

                                Vector3 vect = new Vector3(myRealWorldPoints[depthIndex].X, myRealWorldPoints[depthIndex].Y, myRealWorldPoints[depthIndex].Z);
                                Vector3 col = new Vector3(1f, 0, 0);

                                arrVectors[x, y].Vector = vect;
                                arrVectors[x, y].Color = col;

                            }

                        }
                        else
                        {
                            // no depth measured - set point to 10 m apart
                            //get x and y from the conversion table - 
                            float zTest = 10f;
                            float xTest = ConversionTable[depthIndex].X * zTest;
                            float yTest = ConversionTable[depthIndex].Y * zTest;

                            Vector3 vect = new Vector3(xTest, yTest, zTest);
                            arrVectors[x, y].Vector = vect;
                            //arrVectors[x, y].Color = col;


                        }
                    }
                });
                //};
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine(" error in PointCloudWithColor " + err.Message);
            }

            return arrVectors;

        }
        private static int FindNonNull(PointCloud pc, int startIndex, int stopIndex)
        {
            int indexFound = -1;
            for (int i = startIndex; i < stopIndex; i++)
            {
                if (pc.Vectors[i] != null)
                {
                    indexFound = i;
                    return indexFound;
                }

            }
            return -1;
            //throw new Exception("SW Implementation Error in FindIndex Non Null - MetadDataBase");

        }

        public static ushort[] RotateDepthData(ushort[] points, int width, int height)
        {
            ushort[] rotatedPoints = new ushort[points.GetLength(0)];

            for (ushort x = 0; x < width; ++x)
            {
                for (ushort y = 0; y < height; ++y)
                {

                    int depthIndex = (y * width) + x;
                    int newDepthIndex = ((height - y - 1) * width) + x;

                    ushort z = points[depthIndex];
                    rotatedPoints[newDepthIndex] = z;

                }
            }
            return rotatedPoints;

        }

        public static List<Vector3> RotatePointCloud180(List<Vector3> oldList, int Width, int Height)
        {
            //int Widht and Height has to be set on class level - is set for Scanner v2!

            List<Vector3> listOfVectors = new List<Vector3>();


            for (int i = 0; i < oldList.Count; i++)
            {
                Vector3 v = oldList[i];

                double newX = Width - v.X;
                double newY = Height - v.Y;

                listOfVectors.Add(new Vector3(Convert.ToSingle(newX), Convert.ToSingle(newY), Convert.ToSingle(v.Z)));

            }

            return listOfVectors;

        }
        ///// <summary>
        ///// creates color info for all DEPTH pixels (to later e.g. write ply file from it)
        ///// </summary>
        ///// <param name="myColorMetaData"></param>
        ///// <param name="myDepthMetaData"></param>
        ///// <param name="myCoordinateMapper"></param>
        ///// <returns></returns>
        //public static PointCloud ToPointCloud(ColorMetaData myColorMetaData, DepthMetaData myDepthMetaData, BodyMetaData myBodyMetaData, CoordinateMapper myCoordinateMapper)
        //{

        //    ColorSpacePoint[] mycolorPointsInDepthSpace = new ColorSpacePoint[DepthMetaData.XDepthMaxKinect * DepthMetaData.YDepthMaxKinect];
        //    CameraSpacePoint[] myRealWorldPoints = new CameraSpacePoint[DepthMetaData.XDepthMaxKinect * DepthMetaData.YDepthMaxKinect];
        //    myCoordinateMapper.MapDepthFrameToCameraSpace(myDepthMetaData.FrameData, myRealWorldPoints);
        //    myCoordinateMapper.MapDepthFrameToColorSpace(myDepthMetaData.FrameData, mycolorPointsInDepthSpace);


        //    CameraIntrinsics ci = myCoordinateMapper.GetDepthCameraIntrinsics();
        //    PointF[] myConversionTable = myCoordinateMapper.GetDepthFrameToCameraSpaceTable();


        //    ConversionTable = new Vector2[myConversionTable.Length];
        //    for (int i = 0; i < myConversionTable.Length; i++)
        //    {
        //        ConversionTable[i] = new Vector2(myConversionTable[i].X, myConversionTable[i].Y);

        //    }


        //    if (myColorMetaData != null)
        //    {
        //        return PointCloudWithColorParallel(mycolorPointsInDepthSpace, myRealWorldPoints, myColorMetaData, myDepthMetaData, myBodyMetaData);

        //    }
        //    else
        //    {
        //        return PointCloudBW(myRealWorldPoints, myDepthMetaData);

        //    }


        //}
        private static PointCloud PointCloudBW(CameraSpacePoint[] myRealWorldPoints, DepthMetaData myDepthMetaData)
        {
            int x = 0;
            int y = 0;

            List<Vector3> vecs = new List<Vector3>();
            List<Vector3> cols = new List<Vector3>();

            PointCloud pc = new PointCloud();
            try
            {

                int i = -1;
                for (x = 0; x < DepthMetaData.XDepthMaxKinect; x++)
                {
                    for (y = 0; y < DepthMetaData.YDepthMaxKinect; y++)
                    {
                        int depthIndex = (y * DepthMetaData.XDepthMaxKinect) + x;
                        //int depthIndex = ((DepthMetaData.YResDefault - y - 1) * DepthMetaData.XResDefault) + x;

                        if (myDepthMetaData.FrameData[depthIndex] != 0)
                        {

                            i++;
                            int depthIndexColor = depthIndex * MetaDataBase.BYTES_PER_PIXEL;

                            Vector3 vect = new Vector3(myRealWorldPoints[depthIndex].X, myRealWorldPoints[depthIndex].Y, -myRealWorldPoints[depthIndex].Z);

                            vecs.Add(vect);



                        }
                    }
                }
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("..Debug : " + x.ToString() + " : " + y.ToString() + " : ");
            }
            pc.Vectors = vecs.ToArray();

            return pc;
        }

        //private static PointCloud PointCloudWithColorParallel(ColorSpacePoint[] mycolorPointsInDepthSpace, CameraSpacePoint[] myRealWorldPoints, ColorMetaData myColorMetaData, DepthMetaData myDepthMetaData, BodyMetaData myBodyMetaData)
        //{

        //    VectorColor[,] arrVectors = Helper_CreateArray(mycolorPointsInDepthSpace, myRealWorldPoints, myColorMetaData, myDepthMetaData);
        //    PointCloud pc = new PointCloud();
        //    List<Vector3> vecList = new List<Vector3>();
        //    List<Vector3> colList = new List<Vector3>();


        //    int indV = -1;
        //    for (int x = 0; x < DepthMetaData.XDepthMaxKinect; x++)
        //    {
        //        for (int y = 0; y < DepthMetaData.YDepthMaxKinect; y++)
        //        {
        //            indV++;
        //            if (arrVectors[x, y].Vector != Vector3.Zero)
        //            {
        //                vecList.Add(arrVectors[x, y].Vector);
        //                colList.Add(arrVectors[x, y].Color);


        //            }


        //        }
        //    }

        //    pc.Vectors = vecList.ToArray();
        //    pc.Colors = colList.ToArray();



        //    return pc;
        //}
        private static VectorColor[,] Helper_CreateArray(ColorSpacePoint[] mycolorPointsInDepthSpace, CameraSpacePoint[] myRealWorldPoints, ColorMetaData myColorMetaData, DepthMetaData myDepthMetaData)
        {

            VectorColor[,] arrVectors = new VectorColor[DepthMetaData.XDepthMaxKinect, DepthMetaData.YDepthMaxKinect];

            try
            {

                //for (int x = 0; x < DepthMetaData.XResDefault; x++)
                System.Threading.Tasks.Parallel.For(0, DepthMetaData.XDepthMaxKinect, x =>
                {
                    for (int y = 0; y < DepthMetaData.YDepthMaxKinect; y++)
                    {
                        int depthIndex = (y * DepthMetaData.XDepthMaxKinect) + x;

                        if (myDepthMetaData.FrameData[depthIndex] != 0)
                        {


                            ColorSpacePoint colorPoint = mycolorPointsInDepthSpace[depthIndex];


                            int xColor = Convert.ToInt32(colorPoint.X);
                            int yColor = Convert.ToInt32(colorPoint.Y);
                            int depthIndexColor = depthIndex * MetaDataBase.BYTES_PER_PIXEL;

                            if ((xColor >= 0) && (xColor < ColorMetaData.XColorMaxKinect) && (yColor >= 0) && (yColor < ColorMetaData.YColorMaxKinect))
                            {
                                int colorIndex = ((yColor * ColorMetaData.XColorMaxKinect) + xColor) * MetaDataBase.BYTES_PER_PIXEL;
                                Vector3 vect = new Vector3(myRealWorldPoints[depthIndex].X, myRealWorldPoints[depthIndex].Y, myRealWorldPoints[depthIndex].Z);
                                Vector3 col = new Vector3(myColorMetaData.Pixels[colorIndex] / 255f, myColorMetaData.Pixels[colorIndex + 1] / 255f, myColorMetaData.Pixels[colorIndex + 2] / 255f);
                                arrVectors[x, y].Vector = vect;

                                arrVectors[x, y].Color = col;

                            }

                        }

                    }
                });
                //};
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine(" error in PointCloudWithColor " + err.Message);
            }

            return arrVectors;

        }

        /// <summary>
        /// reads the PLY file ONLY with the special format used also in the write_PLY method
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <param name="depthData"></param>
        /// <returns></returns>
        public static void ReadDepthWithColor_PLY(string path, string fileName, ref ushort[] depthData, ref byte[] colorPixelArray)
        {

            ushort[,] depth = null;
            byte[,] colorInfoR = null;
            byte[,] colorInfoG = null;
            byte[,] colorInfoB = null;
            byte[,] colorInfoA = null;


            UtilsPointCloudIO.FromPLYFile(path, fileName, DepthMetaData.XDepthMaxKinect, DepthMetaData.YDepthMaxKinect, ref depth, ref colorInfoR, ref colorInfoG, ref colorInfoB, ref colorInfoA);

            depthData = UtilsPointCloudIO.CreatePointArrayOneDim(depth, DepthMetaData.XDepthMaxKinect, DepthMetaData.YDepthMaxKinect);
            colorPixelArray = CreateColorPixelArray(colorInfoR, colorInfoG, colorInfoB, colorInfoA);

        }
        /// <summary>
        /// reads the PLY file ONLY with the special format used also in the write_PLY method
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <param name="depthData"></param>
        /// <returns></returns>
        public static void ReadDepthWithColor_OBJ(string path, string fileName, ref ushort[] depthData, ref byte[] colorPixelArray)
        {

            ushort[,] depth = null;
            byte[,] colorInfoR = null;
            byte[,] colorInfoG = null;
            byte[,] colorInfoB = null;
            byte[,] colorInfoA = null;

            UtilsPointCloudIO.ObjFileToArray(path, fileName, DepthMetaData.XDepthMaxKinect, DepthMetaData.YDepthMaxKinect, ref depth, ref colorInfoR, ref colorInfoG, ref colorInfoB, ref colorInfoA);

            depthData = UtilsPointCloudIO.CreatePointArrayOneDim(depth, DepthMetaData.XDepthMaxKinect, DepthMetaData.YDepthMaxKinect);
            colorPixelArray = CreateColorPixelArray(colorInfoR, colorInfoG, colorInfoB, colorInfoA);

        }
        //TODO: Test a new process frame method using pointers - perhaps it is faster
        //#region mapping - fast
        ////added declaration and initialization
        //private ColorSpacePoint[] m_pColorSpacePoints;

        ////in awake method


        ////accesor to the colorspacepoints
        //public ColorSpacePoint[] GetColorSpacePointBuffer()
        //{
        //    return m_pColorSpacePoints;
        //}

        //void ProcessFrame(ColorMetaData myColorMetaData, DepthMetaData myDepthMetaData, CoordinateMapper myCoordinateMapper)
        //{
        //    //m_pColorSpacePoints = new ColorSpacePoint[myDepthMetaData.FrameData.Length];

        //    //var pDepthData = GCHandle.Alloc(myDepthMetaData.FrameData, GCHandleType.Pinned);
        //    //var pDepthCoordinatesData = GCHandle.Alloc(myDepthMetaData.Pixels, GCHandleType.Pinned);
        //    //var pColorData = GCHandle.Alloc(m_pColorSpacePoints, GCHandleType.Pinned);

        //    //myCoordinateMapper.MapColorFrameToDepthSpaceUsingIntPtr(pDepthData.AddrOfPinnedObject(),
        //    //  (uint)pDepthBuffer.Length * sizeof(ushort),
        //    //  pDepthCoordinatesData.AddrOfPinnedObject(),
        //    //  (uint)m_pDepthCoordinates.Length);


        //    //m_pCoordinateMapper.MapDepthFrameToColorSpaceUsingIntPtr(
        //    //  pDepthData.AddrOfPinnedObject(),
        //    //  pDepthBuffer.Length * sizeof(ushort),
        //    //  pColorData.AddrOfPinnedObject(),
        //    //  (uint)m_pColorSpacePoints.Length);

        //    //pColorData.Free();
        //    //pDepthCoordinatesData.Free();
        //    //pDepthData.Free();

        //    //m_pColorRGBX.LoadRawTextureData(pColorBuffer);
        //    //m_pColorRGBX.Apply();
        //}

    }
 

}
