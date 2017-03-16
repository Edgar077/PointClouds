using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;


namespace OpenTKExtension
{

    public static class BitmapExtensions
    {

        public static Bitmap Update_Color(this Bitmap bm, byte[] data)
        {
            try
            {
                LockBitmap lockBitmap = new LockBitmap(bm);
                lockBitmap.LockBits();
                lockBitmap.Pixels = data;

                lockBitmap.UnlockBits();

                return bm;
            }
            catch(Exception err)
            {
                System.Windows.Forms.MessageBox.Show("Error in updating bitmap : " + err.Message);
            }
            return null;

        }
        public static Bitmap Update_Gray(this Bitmap bm, byte[] data)
        {
            
            LockBitmap lockBitmap = new LockBitmap(bm);
            lockBitmap.LockBits();
            lockBitmap.Pixels = data;

            lockBitmap.UnlockBits();


            return bm;
        }
      
        public static System.Drawing.Bitmap Update_Gray(this Bitmap bm, ushort[] myDepthFrame)
        {
            if (myDepthFrame != null && myDepthFrame.Length > 0)
            {
                byte[] pixels = ImageExtensions.ConvertUshortToByte(myDepthFrame);

                bm = bm.Update_Gray(pixels);

                return bm;
            }
            else
                return null;

        }
        public static Bitmap FromByteArray_Gray(byte[] data, int width, int height)
        {
            System.Drawing.Bitmap bitmap8bpp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

            bitmap8bpp.Palette = ImageExtensions.GrayScalePalette;


            LockBitmap lockBitmap = new LockBitmap(bitmap8bpp);
            lockBitmap.LockBits();
            lockBitmap.Pixels = data;

            lockBitmap.UnlockBits();


            return bitmap8bpp;
        }
      
    
        //public static Bitmap FromByteArray_Color(byte[] data, int width, int height)
        //{
        //    if(bitmap32bpp == null)
        //        bitmap32bpp = new Bitmap(width, height, PixelFormat.Format32bppRgb);

        //    LockBitmap lockBitmap = new LockBitmap(bitmap32bpp);
        //    lockBitmap.LockBits();
        //    lockBitmap.Pixels = data;


        //    lockBitmap.UnlockBits();

        //    return bitmap32bpp;


        //}

        /// <summary>
        /// not working
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Bitmap ConvertToGrayScale(this Bitmap bitmap)
        {
            Bitmap bm ;
            bitmap.Palette = ImageExtensions.GrayScalePalette;

            //this line has to be reviewed
            bm = ConvertPixelFormat(bitmap, System.Drawing.Imaging.PixelFormat.Format16bppGrayScale);
            return bm;

        }
        public static void ChangeToGrayScale(this Bitmap bitmap)
        {
            bitmap.Palette = ImageExtensions.GrayScalePalette;
        }
    
     
        /// <summary>
        /// not working
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static Bitmap ConvertPixelFormat(this Bitmap b1, System.Drawing.Imaging.PixelFormat format)
        {
            //eg System.Drawing.Imaging.PixelFormat.Format24bppRgb
            Bitmap b2 = new Bitmap(b1.Size.Width, b1.Size.Height, format);

            Graphics g = Graphics.FromImage(b2);
            g.DrawImage(b1, new Point(0, 0));
            g.Dispose();

            //System.Console.WriteLine("b1.PixelFormat=" + b1.PixelFormat.ToString());
            //System.Console.WriteLine("b2.PixelFormat=" + b2.PixelFormat.ToString());


            return b2;

        }
        public static void SaveImageJPG(this Bitmap bmp, string filepath, long quality)
        {
            // Encoder parameter for image quality
            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

            // Jpeg image codec
            ImageCodecInfo jpegCodec = ImageExtensions.GetEncoderInfo("image/jpeg");

            if (jpegCodec == null)
                return;

            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;

            bmp.Save(filepath, jpegCodec, encoderParams);
        }
        public static void SaveImage(string path, Bitmap img)
        {

            //ImageCodecInfo iciPng = null;
            //foreach (ImageCodecInfo ici in ImageCodecInfo.GetImageDecoders())
            //{
            //    if (ici.FilenameExtension.ToLower().Contains("png"))
            //    {
            //        iciPng = ici;
            //        break;
            //    }
            //}

            // Jpeg image codec
            ImageCodecInfo pngCodec = ImageExtensions.GetEncoderInfo("image/png");

            if (pngCodec == null)
            {
                throw new Exception("SW Error saving image - png codec not present on computer");

            }

            EncoderParameters eps = new EncoderParameters(1);
            EncoderParameter ep = new EncoderParameter(System.Drawing.Imaging.Encoder.ColorDepth, 32L);
            eps.Param[0] = ep;


            img.Save(path, pngCodec, eps);

        }


        public static Bitmap ToGrayscale(this Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            System.Drawing.Imaging.ColorMatrix colorMatrix = new System.Drawing.Imaging.ColorMatrix(
               new float[][] {new float[] {.3f, .3f, .3f, 0, 0},new float[] {.59f, .59f, .59f, 0, 0},
               new float[] {.11f, .11f, .11f, 0, 0},new float[] {0, 0, 0, 1, 0},new float[] {0, 0, 0, 0, 1}});

            //create some image attributes
            System.Drawing.Imaging.ImageAttributes attributes = new System.Drawing.Imaging.ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }
        public static void ChangeColor_Slow(this Bitmap im, bool R, bool G, bool B)
        {

            // Loop through the images pixels to reset color. 
            for (int i = 0; i < im.Width; i++)
            {
                for (int j = 0; j < im.Height; j++)
                {
                    Color pixelColor = im.GetPixel(i, j);
                    Color newColor = pixelColor;
                    if (R)
                        newColor = Color.FromArgb(pixelColor.R, 0, 0);
                    if (G)
                        newColor = Color.FromArgb(0, pixelColor.G, 0);
                    if (B)
                        newColor = Color.FromArgb(0, 0, pixelColor.B);

                    im.SetPixel(i, j, newColor);
                }
            }

        }
       
    

    }
}
