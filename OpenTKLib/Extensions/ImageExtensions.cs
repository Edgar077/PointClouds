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

    public static class ImageExtensions
    {
        private static ColorPalette grayScalePalette;
       
     
        public static ColorPalette GrayScalePalette
        {
            get
            {
                if (grayScalePalette == null)
                    grayScalePalette = GetGrayScalePalette();
                return grayScalePalette;
            }
        }

        public static ColorPalette GetGrayScalePalette()
        {
            Bitmap bmp = new Bitmap(1, 1, PixelFormat.Format8bppIndexed);

            ColorPalette monoPalette = bmp.Palette;

            Color[] entries = monoPalette.Entries;

            for (int i = 0; i < 256; i++)
            {
                entries[i] = Color.FromArgb(i, i, i);
            }

            return monoPalette;
        }
     
       
        public static Bitmap UpdateFromByteArray_Color(this Image im, byte[] data)
        {
            //Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppRgb);

            LockBitmap lockBitmap = new LockBitmap((Bitmap)im);
            lockBitmap.LockBits();
            lockBitmap.Pixels = data;


            lockBitmap.UnlockBits();

            return (Bitmap)im;


        }
   
        public static Bitmap UpdateFromPointCloud_Color(this Image im, PointCloud pc, int width, int height)
        {
            //Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppRgb);
            byte[] data = PointCloud.ToColorArrayBytes(pc, width, height);
            LockBitmap lockBitmap = new LockBitmap((Bitmap)im);
            lockBitmap.LockBits();
            lockBitmap.Pixels = data;
            
            lockBitmap.UnlockBits();
            return (Bitmap)im;


        }
       


        public static void SaveImage(this Image img, string path, string fileNameShort, bool addTimeInfoToFile)
        {
            try
            {
                if (addTimeInfoToFile)
                    fileNameShort += DateTimeString();
                string pathNew = path + "\\" + fileNameShort + ".png";
                img.SaveImage(pathNew);
            }
            catch(Exception err)
            {
                System.Windows.Forms.MessageBox.Show("Error saving image : " + err.Message);
            }

        }

        public static string DateTimeString()
        {
            DateTime dt = DateTime.Now;
            return dt.Year.ToString() + "." + dt.Month.ToString() + "." + dt.Day.ToString() + "." + dt.Hour.ToString() + "." + dt.Minute.ToString() + "." + dt.Second.ToString() + "_";


        }
        public static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];
            return null;
        }
        public static void SaveImage(this Image img, string filepath)
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
            ImageCodecInfo pngCodec = GetEncoderInfo("image/png");

            if (pngCodec == null)
            {
                throw new Exception("SW Error saving image - png codec not present on computer");

            }

            EncoderParameters eps = new EncoderParameters(1);
            EncoderParameter ep = new EncoderParameter(System.Drawing.Imaging.Encoder.ColorDepth, 32L);
            eps.Param[0] = ep;


            img.Save(filepath, pngCodec, eps);

        }



        public static byte[] ToByteArray(this System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }
        //public static byte[] ToByteArray2(this Image myImage)
        //{
        //    int width = myImage.Width;
        //    int height = myImage.Height;
        //    System.Windows.Media.PixelFormat format = System.Windows.Media.PixelFormats.Bgr32;

        //    byte[] pixels = new byte[width * height * ((format.BitsPerPixel + 7) / 8)];
        //    Bitmap bitm = new Bitmap(myImage);
        //    //bitm.LockBits();

        //    //bitm.UnlockBits();
        //    //int stride = width * format.BitsPerPixel / 8;
        //    return pixels;

        //}


     
      

        //int height = frame.FrameDescription.Height;

        //public static List<System.Drawing.Color> ToColorList_Slow(this Image image)
        //{
        //    List<int> raw = new List<int>();
        //    Bitmap img = new Bitmap(image);
        //    List<System.Drawing.Color> pixels = new List<System.Drawing.Color>();

        //    for (int y = 0; y < img.Height; y++)
        //    {
        //        for (int x = 0; x < img.Width; ++x)
        //            pixels.Add(img.GetPixel(x, y));
        //    }
        //    return pixels;

        //}
        //public static List<System.Drawing.Color> ToColorList(this Image image) //Bitmap original
        //{
        //    List<System.Drawing.Color> pixels = new List<System.Drawing.Color>();
        //    unsafe
        //    {
        //        //create an empty bitmap the same size as original
        //        Bitmap newBitmap = new Bitmap(image);

        //        //lock the original bitmap in memory
        //        System.Drawing.Imaging.BitmapData bitmapData = newBitmap.LockBits(new Rectangle(0, 0, newBitmap.Width, newBitmap.Height),
        //           System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

        //        ////lock the new bitmap in memory
        //        //System.Drawing.Imaging.BitmapData newData = newBitmap.LockBits(new Rectangle(0, 0, newBitmap.Width, newBitmap.Height),
        //        //   System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

        //        //set the number of bytes per pixel
        //        int pixelSize = 3;

        //        for (int y = 0; y < image.Height; y++)
        //        {
        //            //get the data from the original image
        //            byte* oRow = (byte*)bitmapData.Scan0 + (y * bitmapData.Stride);

        //            //get the data from the new image
        //            byte* nRow = (byte*)bitmapData.Scan0 + (y * bitmapData.Stride);

        //            for (int x = 0; x < newBitmap.Width; x++)
        //            {


        //                //create the grayscale version
        //                byte r = (byte)(oRow[x * pixelSize]);
        //                byte g = (byte)(oRow[x * pixelSize + 1]);
        //                byte b = (byte)(oRow[x * pixelSize + 2]);
        //                System.Drawing.Color color = System.Drawing.Color.FromArgb(r, g, b);
        //                pixels.Add(color);


        //            }
        //        }

        //        //unlock the bitmaps
        //        newBitmap.UnlockBits(bitmapData);

        //        return pixels;
        //    }
        //}

        public static void OverwriteImageRegion(this Image image, Image destinationImage, System.Drawing.Rectangle myRect)
        {

            System.Drawing.Image newImage = (System.Drawing.Image)image.Clone();
            System.Drawing.Graphics g1 = System.Drawing.Graphics.FromImage(newImage);
            // Create a color map.
            System.Drawing.Imaging.ColorMap colorMap = new System.Drawing.Imaging.ColorMap();

            System.Drawing.Imaging.ColorMap[] colorMapArray = new System.Drawing.Imaging.ColorMap[1];
            colorMapArray[0].OldColor = System.Drawing.Color.Red;
            colorMapArray[0].NewColor = System.Drawing.Color.Beige;
            //= new System.Drawing.Imaging.ColorMap() {colorMap};

            //Create an ImageAttributes object, and then pass the transformerobject to the SetRemapTable method.
            System.Drawing.Imaging.ImageAttributes imageAttr = new System.Drawing.Imaging.ImageAttributes();
            imageAttr.SetRemapTable(colorMapArray);

            g1.DrawImage(newImage, new System.Drawing.Rectangle(0, 0, newImage.Width, newImage.Height), 0, 0, newImage.Width, newImage.Height, System.Drawing.GraphicsUnit.Pixel, imageAttr);

        }
        public static void ExtractRectangle(this Image image, System.Drawing.Rectangle myRect)
        {
            Image destinationImage = new Bitmap(image.Width, image.Height);

            //System.Drawing.Image newImage = (System.Drawing.Image)image.Clone();
            System.Drawing.Graphics g1 = System.Drawing.Graphics.FromImage(destinationImage);
            //// Create a color map.
            //System.Drawing.Imaging.ColorMap colorMap = new System.Drawing.Imaging.ColorMap();

            //System.Drawing.Imaging.ColorMap[] colorMapArray = new System.Drawing.Imaging.ColorMap[1];
            //colorMapArray[0].OldColor = System.Drawing.Color.Red;
            //colorMapArray[0].NewColor = System.Drawing.Color.Beige;
            //= new System.Drawing.Imaging.ColorMap() {colorMap};

            //Create an ImageAttributes object, and then pass the transformerobject to the SetRemapTable method.
            //System.Drawing.Imaging.ImageAttributes imageAttr = new System.Drawing.Imaging.ImageAttributes();
            //imageAttr.SetRemapTable(colorMapArray);
            // PointF destinationPoint = new PointF(myRect.X, myRect.Y);
            g1.DrawImage(image, myRect, myRect, System.Drawing.GraphicsUnit.Pixel);
            //g1.DrawImage(image, myRect, myRect0, 0, newImage.Width, newImage.Height, System.Drawing.GraphicsUnit.Pixel, imageAttr);

        }
        public static byte[] ConvertUshortToByte(ushort[] ushortArray)
        {
            int sizeOfList = ushortArray.GetLength(0);

            byte[] pixels = new byte[sizeOfList];

            // convert depth to a visual representation
            for (int i = 0; i < sizeOfList; ++i)
            {
                ushort depth = ushortArray[i];

                pixels[i] = (byte)(depth);

            }
            return pixels;

        }
        

    }
}
