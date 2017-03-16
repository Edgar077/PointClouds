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
using OpenTK;


namespace PointCloudUtils
{
    public partial class DepthMetaData 
    {

        /// <summary>
        /// Converts a 16-bit grayscale depth frame which includes player indexes into a 3D point array: depthFramePoints
        /// </summary>
        /// <param name="depthFrame">The depth frame.</param>
        /// <param name="depthStream">The depth stream.</param>
        private static Point3D[] ConvertPixelArrayTo3D(ushort[] depthPixelData)
        {

            int[] rawDepth = new int[depthPixelData.Length];

            Point3D[] depthFramePoints = new Point3D[depthPixelData.Length];


            int tooNearDepth = 100;
            int tooFarDepth = 8000;
            int unknownDepth = 0;


            int cx = DepthMetaData.XDepthMaxKinect / 2;
            int cy = DepthMetaData.YDepthMaxKinect / 2;

            double fxinv = 1 / 476;
            double fyinv = 1 / 476;

            double scale = 0.001f;

            Parallel.For(
                0,
                DepthMetaData.YDepthMaxKinect,
                iy =>
                {
                    for (int ix = 0; ix < DepthMetaData.XDepthMaxKinect; ix++)
                    {
                        int i = (iy * DepthMetaData.XDepthMaxKinect) + ix;
                        //this.rawDepth[i] = depthFrame[(iy * width) + ix] >> DepthImageFrame.PlayerIndexBitmaskWidth;
                        rawDepth[i] = depthPixelData[(iy * DepthMetaData.XDepthMaxKinect) + ix];


                        if (rawDepth[i] == unknownDepth || rawDepth[i] < tooNearDepth || rawDepth[i] > tooFarDepth)
                        {
                            rawDepth[i] = -1;
                            depthFramePoints[i] = new Point3D();
                        }
                        else
                        {
                            double zz = rawDepth[i] * scale;
                            double x = (cx - ix) * zz * fxinv;
                            double y = zz;
                            double z = (cy - iy) * zz * fyinv;
                            depthFramePoints[i] = new Point3D(x, y, z);
                        }
                    }
                });
            return depthFramePoints;

        }
        public WriteableBitmap ToDepthWriteableBitmap
        {
            get
            {
                if (depthBitmap == null)
                {
                    this.pixels = ImageExtensions.ConvertUshortToByte(this.FrameData);
                    depthBitmap = WriteableBitmapUtils.FromByteArray_ToGray(pixels, DepthMetaData.XDepthMaxKinect, DepthMetaData.YDepthMaxKinect);


                }
                return depthBitmap;
            }
        }
        public System.Drawing.Bitmap UpdateDepthImage(System.Drawing.Bitmap bm)
        {
            this.pixels = ImageExtensions.ConvertUshortToByte(this.FrameData);
            bm = bm.Update_Gray(pixels);
            return bm;

        }
 

    }

  


}
