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
  
    public class ColorMetaData : MetaDataBase
    {
       
        //could get them also with:
        //XRes = myframeColor.FrameDescription.Width;
        //YRes = myframeColor.FrameDescription.Height;

        public int DataSize;

        /// <summary>
        /// Bitmap to display
        /// </summary>
        public WriteableBitmap WriteableBitmapColor = null;

        #region privates
        /// <summary>
        /// The DPI.
        /// </summary>
        readonly double DPI = 96.0f;

        /// <summary>
        /// Default format.
        /// </summary>
        readonly PixelFormat FORMAT = PixelFormats.Bgra32;

       


        /// <summary>
        /// The size in bytes of the bitmap back buffer
        /// </summary>
        private uint bitmapColorBackBufferSize = 0;

        private readonly int bytesPerPixel = (PixelFormats.Bgr32.BitsPerPixel + 7) / 8;

        #endregion

        public ColorMetaData()
        {
        }
      
        public ColorMetaData(ColorFrame myframeColor)
        {

            CreateByteArray(myframeColor);

        }
        public void SetDataForColorRemoveBackground_Unused(ColorFrame myframeColor)
        {
            this.WriteableBitmapColor = new WriteableBitmap(XColorMaxKinect, YColorMaxKinect, DPI, DPI, PixelFormats.Bgra32, null);
            // Calculate the WriteableBitmap back buffer size
            this.bitmapColorBackBufferSize = (uint)((this.WriteableBitmapColor.BackBufferStride * (this.WriteableBitmapColor.PixelHeight - 1)) + (this.WriteableBitmapColor.PixelWidth * this.bytesPerPixel));

            //second method - used for BodyIndex - check performance!!
            this.WriteableBitmapColor.Lock();
            myframeColor.CopyConvertedFrameDataToIntPtr(this.WriteableBitmapColor.BackBuffer, this.bitmapColorBackBufferSize, ColorImageFormat.Bgra);
            this.WriteableBitmapColor.Unlock();
        }
        public void SetColorPixels(byte[] myColorPixels)
        {
            this.pixels = myColorPixels;
        }
      
      

        private byte[] CreateByteArray(ColorFrame myframeColor)
        {
           
            pixels = new byte[ColorMetaData.XColorMaxKinect * ColorMetaData.YColorMaxKinect * BYTES_PER_PIXEL];
            if (myframeColor.RawColorImageFormat == ColorImageFormat.Bgra)
            {
                myframeColor.CopyRawFrameDataToArray(this.pixels);
            }
            else
            {
                myframeColor.CopyConvertedFrameDataToArray(pixels, ColorImageFormat.Bgra);
            }
            return pixels;

        }

    
        ///// <summary>
        ///// not the full HD color image, but the Depth Image with Color !!!
        ///// </summary>
        ///// <param name="pc"></param>
        //public System.Drawing.Bitmap FromPointCloud_ToBitmap(PointCloud pc)
        //{
        //    // not the full HD color image, but the Depth Image with Color !!!
        //    pixels = new byte[ColorMetaData.XDepthMaxKinect * ColorMetaData.YDepthMaxKinect * BYTES_PER_PIXEL];

        //    System.Drawing.Bitmap bitmapColor = new System.Drawing.Bitmap(ColorMetaData.XDepthMaxKinect, ColorMetaData.YDepthMaxKinect, System.Drawing.Imaging.PixelFormat.Format32bppRgb);

        //    PointCloud pcNew = PointCloud_ToImageCloud(pc);

           
        //    int xdim = XDepthMaxKinect;
           
        //    //ushort[] newData = new ushort[dim];

        //    float x = 0;
        //    float y = 0;
        //    // ushort y = 0;
        //   // ushort x = 0;
        //   // ushort y = 0;
        //    try
        //    {
        //        for (int i = 0; i < pcNew.Count; i++)
        //        {

        //            Vector3 p3D = pcNew.Vectors[i];
        //            x = p3D.X;
        //            y = p3D.Y;
        //            if (x < XDepthMaxKinect && y < YDepthMaxKinect)
        //            {
        //                //ushort z = Convert.ToUInt32(p3D.Vector.Z);

        //                int depthIndex = ((y * xdim) + x) * BYTES_PER_PIXEL;
        //                this.pixels[depthIndex++] = p3D.Color.R;
        //                this.pixels[depthIndex++] = p3D.Color.G;
        //                this.pixels[depthIndex++] = p3D.Color.B;
        //            }

        //        }
        //    }
        //    catch
        //    {
        //        System.Windows.Forms.MessageBox.Show("Error in loop FromPointCloud_ToBitmap: " + x.ToString() + " : " + y.ToString());
        //    }
        //    //WriteableBitmap depthBitmap = WriteableBitmapUtils.FromByteArray_ToGray(pixels, DepthMetaData.XDepthMaxKinect, DepthMetaData.YDepthMaxKinect);
        //    bitmapColor.Update_Color(this.Pixels);

        //    return bitmapColor;

        //}
       

     
    }

}
