using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows;
using System.Drawing;
using OpenTKExtension;

namespace PointCloudUtils
{
    public static class WriteableBitmapUtils
    {
      
        public static Image ByteArrayToImage_Gray_Alternative(byte[] pixels, int width, int height)
        {
            WriteableBitmap wbm = WriteableBitmapUtils.FromByteArray_ToGray(pixels, width, height);
            return WriteableBitmapUtils.ToImage(wbm);
        }

        public static WriteableBitmap FromByteArray_ToGray(byte[] pixels, int width, int height)
        {
            WriteableBitmap depthBitmapTest = new WriteableBitmap(width, height, 96.0, 96.0, PixelFormats.Gray8, null);


            depthBitmapTest.WritePixels(
                new Int32Rect(0, 0, width, height),
                pixels,
                width,
                0);
            return depthBitmapTest;

        }
        public static WriteableBitmap FromByteArray_ToColor(byte[] pixels, int width, int height)
        {

            PixelFormat FORMAT = PixelFormats.Bgra32;
            double DPI = 96.0f;
            WriteableBitmap colorBitmap = new WriteableBitmap(width, height, DPI, DPI, FORMAT, null);
            int bytesPerPixel = (PixelFormats.Bgr32.BitsPerPixel + 7) / 8;
            try
            {
                colorBitmap.WritePixels(
                   new Int32Rect(0, 0, width, height),
                   pixels,
                   width * bytesPerPixel,
                   0);
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine("Error " + err.Message);
                return null;
            }

            return colorBitmap;
        }
        public static WriteableBitmap FromPointCloud_ToColor(PointCloud pc, int width, int height)
        {
            byte[] pixels = PointCloud.ToColorArrayBytes(pc, width, height);
            return FromByteArray_ToColor(pixels, width, height);

            
        }

        public static Image ToImage(this WriteableBitmap wbm)
        {
            BitmapFrame bmf = BitmapFrame.Create(wbm);

            System.IO.MemoryStream mems = new System.IO.MemoryStream();

            BmpBitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(bmf);
            encoder.Save(mems);
            System.Drawing.Image im = System.Drawing.Image.FromStream(mems);

            return im;

        }
        /// <summary>
        /// alternative calculation, not used!
        /// </summary>
        /// <param name="frameDepth"></param>
        /// <returns></returns>
        public static ImageSource ToBitmapColor_Intensity(ushort[] pixelData, int width, int height, ushort minDepth, ushort maxDepth)
        {

            PixelFormat format = PixelFormats.Bgr32;
            ushort[] pixels = new ushort[width * height * (format.BitsPerPixel + 7) / 8];


            int colorIndex = 0;
            for (int depthIndex = 0; depthIndex < pixelData.Length; ++depthIndex)
            {
                ushort depth = pixelData[depthIndex];

                byte intensity = (byte)(depth >= minDepth && depth <= maxDepth ? depth : 0);

                pixels[colorIndex++] = intensity; // Blue
                pixels[colorIndex++] = intensity; // Green
                pixels[colorIndex++] = intensity; // Red

                ++colorIndex;
            }

            int stride = width * format.BitsPerPixel / 8;

            return BitmapSource.Create(width, height, 96, 96, format, null, pixels, stride);
        }
        public static void SaveImage(string path, string fileNameShort, WriteableBitmap bitmap, bool addTimeInfoToFile)
        {


            if (bitmap != null)
            {
                // create a png bitmap encoder which knows how to save a .png file
                BitmapEncoder encoder = new PngBitmapEncoder();

                // create frame from the writable bitmap and add to encoder
                encoder.Frames.Add(BitmapFrame.Create(bitmap));

                if (addTimeInfoToFile)
                    path += DateTime.Now.Year.ToString() + "." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + "." + DateTime.Now.Hour.ToString() + "." + DateTime.Now.Minute.ToString() + "." + DateTime.Now.Second.ToString() + "_";
                string pathNew = path + fileNameShort + ".png";

                // write the new file to disk
                try
                {

                    using (System.IO.FileStream fs = new System.IO.FileStream(pathNew, System.IO.FileMode.Create))
                    {
                        encoder.Save(fs);
                    }

                    //this.StatusText = string.Format(CultureInfo.CurrentCulture, Properties.Resources.SavedScreenshotStatusTextFormat, path);
                }
                catch (System.IO.IOException)
                {
                    MessageBox.Show("Error saving in : " + pathNew);
                    //this.StatusText = string.Format(CultureInfo.CurrentCulture, Properties.Resources.FailedScreenshotStatusTextFormat, path);
                }
            }
        }
    }
       
}
