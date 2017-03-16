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
        public static int XDepthMaxKinect = 512;
        public static int YDepthMaxKinect = 424;

        public static int XDepthMaxRealSense = 640;
        public static int YDepthMaxRealSense = 480;

        public static int XColorMaxKinect = 1920;
        public static int YColorMaxKinect = 1080;


        public static int DepthMinDefault = 500;
        public static int DepthMaxDefault = 7500;

        public static float FOV_X_Kinect = 70.6f;
        public static float FOV_Y_Kinect = 60f;

        protected byte[] pixels;
        public ushort[] FrameData;

        public static int BYTES_PER_PIXEL = (PixelFormats.Bgr32.BitsPerPixel + 7) / 8;
        public List<Vector3> Vectors;
        public static Vector2[] ConversionTable;//= myCoordinateMapper.GetDepthFrameToCameraSpaceTable();

        public byte[] Pixels
        {
            get
            {
                if (pixels == null || pixels.GetLength(0) == 0)
                {
                    //only for DepthFrame !!
                    this.pixels = ImageExtensions.ConvertUshortToByte(this.FrameData);
                }

                return pixels;

            }
        }

    

        private static PointCloud PointCloudBW(CameraSpacePoint[] myRealWorldPoints, DepthMetaData myDepthMetaData, BodyMetaData myBodyMetaData)
        {
            int x = 0;
            int y = 0;
           // int indexVertex = 0;

            List<Vector3> list = new List<Vector3>();
            
            try
            {

                for (x = 0; x < DepthMetaData.XDepthMaxKinect; x++)
                {
                    for (y = 0; y < DepthMetaData.YDepthMaxKinect; y++)
                    {
                        int depthIndex = (y * DepthMetaData.XDepthMaxKinect) + x;
                        //int depthIndex = ((DepthMetaData.YResDefault - y - 1) * DepthMetaData.XResDefault) + x;

                        if (myDepthMetaData.FrameData[depthIndex] != 0)
                        {


                            int depthIndexColor = depthIndex * MetaDataBase.BYTES_PER_PIXEL;

                            Vector3 vect = new Vector3(myRealWorldPoints[depthIndex].X, myRealWorldPoints[depthIndex].Y, -myRealWorldPoints[depthIndex].Z);
                            list.Add(vect);



                        }
                    }
                }
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("..Debug : " + x.ToString() + " : " + y.ToString() + " : ");
            }
            return PointCloud.FromListVector3(list);
        }
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="colorInfoR"></param>
        /// <param name="colorInfoG"></param>
        /// <param name="colorInfoB"></param>
        /// <param name="colorInfoA"></param>
        /// <returns></returns>
        private static byte[] CreateColorPixelArray(byte[,] colorInfoR, byte[,] colorInfoG, byte[,] colorInfoB, byte[,] colorInfoA)
        {
            byte[] mydisplayPixels = new byte[DepthMetaData.XDepthMaxKinect * DepthMetaData.YDepthMaxKinect * MetaDataBase.BYTES_PER_PIXEL];
            int numberOfColorPointsNotZero = -1;
            for (int x = 0; x < DepthMetaData.XDepthMaxKinect; ++x)
            {
                for (int y = 0; y < DepthMetaData.YDepthMaxKinect; ++y)
                {
                    int depthIndex = (y * DepthMetaData.XDepthMaxKinect) + x;
                    int displayIndex = depthIndex * BYTES_PER_PIXEL;
                    mydisplayPixels[displayIndex + 0] = colorInfoR[x, y];
                    mydisplayPixels[displayIndex + 1] = colorInfoG[x, y];
                    mydisplayPixels[displayIndex + 2] = colorInfoB[x, y];
                    mydisplayPixels[displayIndex + 3] = colorInfoA[x, y];
                    if (mydisplayPixels[displayIndex + 0] != 0 && mydisplayPixels[displayIndex + 1] != 0 && mydisplayPixels[displayIndex + 2] != 0)
                    {
                        numberOfColorPointsNotZero++;
                    }
                }
            }
            return mydisplayPixels;

        }

        public static List<Vector3> CreateListPoints_Depth(ushort[] points, int width, int height)
        {

            List<Vector3> DepthVertex = new List<Vector3>();

            for (ushort x = 0; x < width; ++x)
            {
                for (ushort y = 0; y < height; ++y)
                {

                    int depthIndex = (y * width) + x;
                    ushort z = points[depthIndex];

                    if (z != 0)
                        DepthVertex.Add(new Vector3(x, y, z));
                }
            }
            return DepthVertex;

        }
   
   
        protected static PointCloud PointCloud_ToImageCloud(PointCloud pc)
        {
            PointCloud pcNew = PointCloud.CloneAll(pc);
            Vector3 v = new Vector3(-pcNew.BoundingBoxMin.X, -pcNew.BoundingBoxMin.Y, -pcNew.BoundingBoxMin.Z);
            PointCloud.AddVectorToAll(pcNew, v);
            pcNew.CalculateBoundingBox();


            float scaleFactorX = Convert.ToSingle(XDepthMaxKinect) / pcNew.BoundingBoxMax.X;
            float scaleFactorY = Convert.ToSingle(YDepthMaxKinect) / pcNew.BoundingBoxMax.Y;

            Vector3 vScale = new Vector3(scaleFactorX, scaleFactorY, 1000f);
            PointCloud.ScaleByVector(pcNew, vScale);


            pcNew.CalculateBoundingBox();
            

            return pcNew;


        }
        #region mapping

      
        /// <summary>
        /// creates color info for all DEPTH pixels (to later e.g. write ply file from it)
        /// </summary>
        /// <param name="myColorMetaData"></param>
        /// <param name="myDepthMetaData"></param>
        /// <param name="myCoordinateMapper"></param>
        /// <returns></returns>
        public static PointCloud ToPointCloud(ColorMetaData myColorMetaData, DepthMetaData myDepthMetaData, BodyMetaData myBodyMetaData,  CoordinateMapper myCoordinateMapper)
        {

            if (myColorMetaData == null && myDepthMetaData == null)
                return null;

            ColorSpacePoint[] mycolorPointsInDepthSpace = new ColorSpacePoint[DepthMetaData.XDepthMaxKinect * DepthMetaData.YDepthMaxKinect];
            CameraSpacePoint[] myRealWorldPoints = new CameraSpacePoint[DepthMetaData.XDepthMaxKinect * DepthMetaData.YDepthMaxKinect];
            myCoordinateMapper.MapDepthFrameToCameraSpace(myDepthMetaData.FrameData, myRealWorldPoints);
            myCoordinateMapper.MapDepthFrameToColorSpace(myDepthMetaData.FrameData, mycolorPointsInDepthSpace);


            if (myColorMetaData != null)
            {
                return PointCloudWithColorParallel(mycolorPointsInDepthSpace, myRealWorldPoints, myColorMetaData, myDepthMetaData, myBodyMetaData);

            }
            else
            {
                return PointCloudBW(myRealWorldPoints, myDepthMetaData, myBodyMetaData);

            }


        }
        private static PointCloud PointCloudWithColorParallel(ColorSpacePoint[] mycolorPointsInDepthSpace, CameraSpacePoint[] myRealWorldPoints, ColorMetaData myColorMetaData, DepthMetaData myDepthMetaData, BodyMetaData myBodyMetaData)
        {

            Vector3[,] arrV= new Vector3[DepthMetaData.XDepthMaxKinect, DepthMetaData.YDepthMaxKinect];
            Vector3[,] arrC = new Vector3[DepthMetaData.XDepthMaxKinect, DepthMetaData.YDepthMaxKinect];

           
            try
            {

                //for (int x = 0; x < DepthMetaData.XDepthMaxKinect; x++)
                System.Threading.Tasks.Parallel.For(0, DepthMetaData.XDepthMaxKinect, x =>
                {
                    for (int y = 0; y < DepthMetaData.YDepthMaxKinect; y++)
                    {
                        int depthIndex = (y * DepthMetaData.XDepthMaxKinect) + x;

                        if (myDepthMetaData.FrameData[depthIndex] != 0)
                        {
                            if (PointCloudScannerSettings.BackgroundRemoved && myBodyMetaData != null)
                            {
                                byte player = myBodyMetaData.Pixels[depthIndex];
                                if (player != 0xff)
                                {
                                    SetPoint(depthIndex, x, y, arrV, arrC, mycolorPointsInDepthSpace, myRealWorldPoints, myColorMetaData);
                                }

                            }
                            else
                                SetPoint(depthIndex, x, y, arrV, arrC, mycolorPointsInDepthSpace, myRealWorldPoints, myColorMetaData);
                            
        

                        }
                    }
                });
                //};
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine(" error in PointCloudWithColor " + err.Message);
            }

            List<Vector3> listV = new List<Vector3>();
            List<Vector3> listC = new List<Vector3>();


            for (int x = 0; x < DepthMetaData.XDepthMaxKinect; x++)
                for (int y = 0; y < DepthMetaData.YDepthMaxKinect; y++)
                {
                    if (arrV[x, y] != Vector3.Zero)
                    {
                        listV.Add(arrV[x, y]);
                        listC.Add(arrC[x, y]);

                    }
                }

            PointCloud pc = new PointCloud(listV, listC, null, null, null, null); 
            if(GLSettings.ShowPointCloudAsTexture)
            {
                pc.TextureCreateFromColors(DepthMetaData.XDepthMaxKinect, DepthMetaData.YDepthMaxKinect);

            }
            return pc;

        }
        private static void SetPoint(int depthIndex, int x, int y, Vector3[,] arrV, Vector3[,] arrC, ColorSpacePoint[] mycolorPointsInDepthSpace, CameraSpacePoint[] myRealWorldPoints, ColorMetaData myColorMetaData)
        {
            ColorSpacePoint colorPoint = mycolorPointsInDepthSpace[depthIndex];


            int xColor = Convert.ToInt32(colorPoint.X);
            int yColor = Convert.ToInt32(colorPoint.Y);
            int depthIndexColor = depthIndex * MetaDataBase.BYTES_PER_PIXEL;

            if ((xColor >= 0) && (xColor < ColorMetaData.XColorMaxKinect) && (yColor >= 0) && (yColor < ColorMetaData.YColorMaxKinect))
            {
                int colorIndex = ((yColor * ColorMetaData.XColorMaxKinect) + xColor) * MetaDataBase.BYTES_PER_PIXEL;
                Vector3 vect = new Vector3(myRealWorldPoints[depthIndex].X, myRealWorldPoints[depthIndex].Y, myRealWorldPoints[depthIndex].Z);
                Vector3 col = new Vector3( myColorMetaData.Pixels[colorIndex + 2] / 255f, myColorMetaData.Pixels[colorIndex + 1] / 255f, myColorMetaData.Pixels[colorIndex] / 255f);
                
                //System.Drawing.Color c = System.Drawing.Color.FromArgb(myColorMetaData.Pixels[colorIndex + 3], myColorMetaData.Pixels[colorIndex + 2], myColorMetaData.Pixels[colorIndex + 1], myColorMetaData.Pixels[colorIndex]);

                //Vertex v = new Vertex(0, vect, c);
                arrV[x, y] = vect;

                arrC[x, y] = col;

            }

        }

        #endregion

      

    }
    public struct VectorColor
    {
        public Vector3 Vector;
        public Vector3 Color;
    }
  


}
