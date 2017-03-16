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
using System.Runtime.InteropServices;

using OpenTK;

namespace PointCloudUtils
{
    public class BackgroundRemoval
    {

               
        /// <summary>
        /// Bytes per pixel.
        /// </summary>
        readonly int BYTES_PER_PIXEL = (PixelFormats.Bgr32.BitsPerPixel + 7) / 8;

        /// <summary>
        /// The DPI.
        /// </summary>
        readonly double DPI = 96.0f;

        /// <summary>
        /// Default format.
        /// </summary>
        readonly PixelFormat FORMAT = PixelFormats.Bgra32;

       
        /// <summary>
        /// The coordinate mapper for the background removal (green-screen) effect.
        /// </summary>
        CoordinateMapper coordinateMapper = null;

        /// <summary>
        /// Storage for the depth info of the color Array
        /// </summary>
        private DepthSpacePoint[] colorArrayWithDepthInfo = null;

        public BackgroundRemoval(CoordinateMapper myCoordinateMapper)
        {
            coordinateMapper = myCoordinateMapper;

        }


        /// <summary>
        /// updates myDepthMetaData.FrameData
        /// </summary>
        /// <param name="myDepthMetaData"></param>
        /// <param name="myBodyMetaData"></param>
        public int DepthFrameData_RemoveBackground(DepthMetaData myDepthMetaData, BodyMetaData myBodyMetaData)
        {
            if (myBodyMetaData == null || myDepthMetaData == null)
                return -1;
            int numberOfPixelsRemoved = 0;

            int nCount = myDepthMetaData.FrameData.GetLength(0);
            ushort[] myPixels = new ushort[nCount];

            for (int i = 0; i < nCount; i++)
            {
                myPixels[i] = myDepthMetaData.FrameData[i];

            }


            for (int x = 0; x < DepthMetaData.XDepthMaxKinect; ++x)
            {
                for (int y = 0; y < DepthMetaData.YDepthMaxKinect; ++y)
                {
                    int depthIndex = (y * DepthMetaData.XDepthMaxKinect) + x;

                    byte player = myBodyMetaData.Pixels[depthIndex];

                    if (player != 0xff)
                    {

                    }
                    else
                    {
                        myDepthMetaData.FrameData[depthIndex] = 0;
                        numberOfPixelsRemoved++;
                    }
                }
            }
            return numberOfPixelsRemoved;

        }

        public WriteableBitmap Color_Bitmap(ColorMetaData myColorMetaData, DepthMetaData myDepthMetaData, BodyMetaData myBodyMetaData)
        {
            if (myDepthMetaData == null || myBodyMetaData == null || myColorMetaData == null)
                return null;

           
            /// <summary>
            /// The RGB pixel values used for the background removal (green-screen) effect.
            /// </summary>
            byte[] myPixelsColorPlayer = null;
            ColorSpacePoint[] _colorPointsPlayer = null;
            _colorPointsPlayer = new ColorSpacePoint[DepthMetaData.XDepthMaxKinect * DepthMetaData.YDepthMaxKinect];

            myPixelsColorPlayer = new byte[DepthMetaData.XDepthMaxKinect * DepthMetaData .YDepthMaxKinect * BYTES_PER_PIXEL];

            coordinateMapper.MapDepthFrameToColorSpace(myDepthMetaData.FrameData, _colorPointsPlayer);
            
            for (int x = 0; x < DepthMetaData.XDepthMaxKinect; ++x)
            {
                for (int y = 0; y < DepthMetaData.YDepthMaxKinect; ++y)
                {
                    int depthIndex = (y * DepthMetaData.XDepthMaxKinect) + x;
                    byte player = myBodyMetaData.Pixels[depthIndex];
                    int displayIndex = depthIndex * BYTES_PER_PIXEL;

                    if (player != 0xff)
                    {
                        ColorSpacePoint colorPoint = _colorPointsPlayer[depthIndex];

                        //int colorX = (int)Math.Floor(colorPoint.X + 0.5);
                        //int colorY = (int)Math.Floor(colorPoint.Y + 0.5);
                        
                        //EDGAR TO DO _ player
                       
                            
                            if ((colorPoint.X >= 0) && (colorPoint.X < ColorMetaData.XColorMaxKinect) && (colorPoint.Y >= 0) && (colorPoint.Y < ColorMetaData.YColorMaxKinect))
                        {
                            int colorIndex = ((Convert.ToInt32(colorPoint.Y) * ColorMetaData.XColorMaxKinect) + Convert.ToInt32(colorPoint.X)) * BYTES_PER_PIXEL;


                            myPixelsColorPlayer[displayIndex + 0] = myColorMetaData.Pixels[colorIndex];
                            myPixelsColorPlayer[displayIndex + 1] = myColorMetaData.Pixels[colorIndex + 1];
                            myPixelsColorPlayer[displayIndex + 2] = myColorMetaData.Pixels[colorIndex + 2];
                            myPixelsColorPlayer[displayIndex + 3] = 0xff;
                        }
                        else
                        {
                        }
                    }
                    else
                    {
                        myPixelsColorPlayer[displayIndex + 3] = 0;//identify later....
                    }
                }
            }

            WriteableBitmap myBitmapColorPlayer = null;
            myBitmapColorPlayer = new WriteableBitmap(DepthMetaData.XDepthMaxKinect, DepthMetaData.XDepthMaxKinect, DPI, DPI, FORMAT, null);


            myBitmapColorPlayer.Lock();

            Marshal.Copy(myPixelsColorPlayer, 0, myBitmapColorPlayer.BackBuffer, myPixelsColorPlayer.Length);
            myBitmapColorPlayer.AddDirtyRect(new Int32Rect(0, 0, DepthMetaData.XDepthMaxKinect, DepthMetaData.YDepthMaxKinect));

            myBitmapColorPlayer.Unlock();

            return myBitmapColorPlayer;
        }
        public System.Drawing.Image Color_Image(ColorMetaData myColorMetaData, DepthMetaData myDepthMetaData, BodyMetaData myBodyMetaData)
        {

            WriteableBitmap myBitmapColorPlayer = Color_Bitmap(myColorMetaData, myDepthMetaData, myBodyMetaData);
            if (myBitmapColorPlayer != null)
                return myBitmapColorPlayer.ToImage();
            else
                return null;
        }
        public WriteableBitmap ColorBitmap2(ColorMetaData myColorMetaData, DepthMetaData myDepthMetaData, BodyMetaData myBodyMetaData)
        {
            if (myDepthMetaData == null || myBodyMetaData == null || myColorMetaData == null)
                return null;
            this.colorArrayWithDepthInfo = new DepthSpacePoint[ColorMetaData.XColorMaxKinect * ColorMetaData.YColorMaxKinect];
            coordinateMapper.MapColorFrameToDepthSpace(myDepthMetaData.FrameData, this.colorArrayWithDepthInfo);

            if (colorArrayWithDepthInfo == null)
                return null;

            this.colorArrayWithDepthInfo = new DepthSpacePoint[ColorMetaData.XColorMaxKinect * ColorMetaData.YColorMaxKinect];

            // Loop over each row and column of the color image
            // Zero out any pixels that don't correspond to a body index
            

            myColorMetaData.WriteableBitmapColor.Lock();
            
            unsafe
            {
                
                int colorMappedToDepthPointCount = this.colorArrayWithDepthInfo.Length;

                
                fixed (DepthSpacePoint* colorMappedToDepthPointsPointer = this.colorArrayWithDepthInfo)
                {
                    for (int colorIndex = 0; colorIndex < colorMappedToDepthPointCount; ++colorIndex)
                    {
                        double colorMappedToDepthX = colorMappedToDepthPointsPointer[colorIndex].X;
                        double colorMappedToDepthY = colorMappedToDepthPointsPointer[colorIndex].Y;

                     
                        if (!double.IsNegativeInfinity(colorMappedToDepthX) &&
                            !double.IsNegativeInfinity(colorMappedToDepthY))
                        {
                            // Make sure the depth pixel maps to a valid point in color space
                            int depthX = (int)(colorMappedToDepthX + 0.5f);
                            int depthY = (int)(colorMappedToDepthY + 0.5f);

                            // If the point is not valid, there is no body index there.
                            if ((depthX >= 0) && (depthX < DepthMetaData.XDepthMaxKinect) && (depthY >= 0) && (depthY < DepthMetaData.YDepthMaxKinect))
                            {
                                int depthIndex = (depthY * DepthMetaData.XDepthMaxKinect) + depthX;

                                byte player = myBodyMetaData.Pixels[depthIndex];
                                if (player != 0xff)
                                {
                                    continue;
                                }
                                
                            }
                        }
                        
                        int displayIndex = colorIndex * BYTES_PER_PIXEL;

                        myColorMetaData.Pixels[colorIndex + 1] = 0;
                        myColorMetaData.Pixels[colorIndex + 2] = 0;
                        myColorMetaData.Pixels[colorIndex + 3] = 0xff;

                    }


                    myColorMetaData.WriteableBitmapColor.AddDirtyRect(new Int32Rect(0, 0, myColorMetaData.WriteableBitmapColor.PixelWidth, myColorMetaData.WriteableBitmapColor.PixelHeight));
                }
            }

            myColorMetaData.WriteableBitmapColor.Unlock();
            return myColorMetaData.WriteableBitmapColor;

            //}
        }

    
    

    }
}
