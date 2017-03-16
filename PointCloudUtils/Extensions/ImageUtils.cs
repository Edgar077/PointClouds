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
using OpenTKExtension;
using OpenTK;

namespace PointCloudUtils
{

    public static class ImageUtils
    {
       
       
     
     

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
    
        public static Bitmap FromByteArray_ColorUnsafe(byte[] data, int width, int height)
        {
            GCHandle pin = GCHandle.Alloc(data, GCHandleType.Pinned);
            var bmp = new Bitmap(width, height,(width * 3 + 3) / 4 * 4,PixelFormat.Format32bppRgb,
                                 Marshal.UnsafeAddrOfPinnedArrayElement(data, 0));
            bmp = (Bitmap)bmp.Clone(); // workaround the requirement that the memory address stay valid
            // the clone step can also crop and/or change PixelFormat, if desired
            pin.Free();
            return bmp;
        }
        public static Bitmap FromByteArray_Color1(byte[] data, int width, int height)
        {
            //Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppRgb);


            //Create a BitmapData and Lock all pixels to be written 
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);

             //byte[] rowPixelsAddress = new byte[bmp.Width];
             //for (int k = rowPixelsAddress.Length - 1; k >= 0; k--)
             //    rowPixelsAddress[k] = (byte)k;

             IntPtr scan0 = bmpData.Scan0;
             Marshal.Copy(data, 0, scan0, data.Length);

             //IntPtr scan0 = bmpData.Scan0;
             //for (int h = 0; h < data.Height; h++)
             //{
             //    Marshal.Copy(rowPixels, 0, scan0, rowPixels.Length);
             //    scan0 = new IntPtr(scan0.ToInt64() + data.Stride);
             //}

           

            ////Copy the data from the byte array into BitmapData.Scan0
            //for (int y = 0; y < bmp.Height - 1; y++)
            //{
            //    Marshal.Copy(data, y * bmp.Width, bmpData.Scan0, bmpData.Stride);
            //}


            //Unlock the pixels
            bmp.UnlockBits(bmpData);

            return bmp;
        }
    
        /// <summary>
        /// byte[] pixels = new byte[width * height * ((format.BitsPerPixel + 7) / 8)];
        /// </summary>
        /// <param name="pixels"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
     
            //return WriteableBitmapUtils.ToImage(wbm);
     
        //public static void CreateImage(int width, int height)
        //{
        //    //Size size = Image.Size;
        //    //Bitmap bitmap = Image;
        //    //// myPrewrittenBuff is allocated just like myReadingBuffer below (skipped for space sake)
        //    //// But with two differences: the buff would be byte [] (not ushort[]) and the Stride == 3 * size.Width (not 6 * ...) because we build a 24bpp not 48bpp
        //    //BitmapData writerBuff = bm.LockBits(new Rectangle(0, 0, size.Width, size.Height), ImageLockMode.UserInputBuffer | ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb, myPrewrittenBuff);
        //    //// note here writerBuff and myPrewrittenBuff are the same reference
        //    //bitmap.UnlockBits(writerBuff);
        //    //// done. bitmap updated , no marshal needed to copy myPrewrittenBuff 

        //    //// Now lets read back the bitmap into another format...
        //    //Bitmap bitmap = new Bitmap(width, height);

        //    BitmapData myReadingBuffer = new BitmapData();
        //    ushort[] buff = new ushort[(3 * width) * height]; // ;Marshal.AllocHGlobal() if you want
        //    GCHandle handle = GCHandle.Alloc(buff, GCHandleType.Pinned);
        //    myReadingBuffer.Scan0 = Marshal.UnsafeAddrOfPinnedArrayElement(buff, 0);
        //    myReadingBuffer.Height = height;
        //    myReadingBuffer.Width = width;
        //    myReadingBuffer.PixelFormat = PixelFormat.Format48bppRgb;
        //    myReadingBuffer.Stride = 6 * width;
        //    // now read into that buff
        //    BitmapData result = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.UserInputBuffer | ImageLockMode.ReadOnly, PixelFormat.Format48bppRgb, myReadingBuffer);
        //    if (object.ReferenceEquals(result, myReadingBuffer))
        //    {
        //        // Note: we pass here
        //        // and buff is filled
        //    }
        //    bitmap.UnlockBits(result);
        //    handle.Free();
        //}
        public static Bitmap CreatePolygon(Point[] points)
        {

            Bitmap bm = new Bitmap(600, 600);
            Graphics g = Graphics.FromImage(bm);
            Brush b = new LinearGradientBrush(new Point(1, 1), new Point(600, 600), Color.White, Color.Red);
            //Point[] points = new Point[] { new Point(77, 500), new Point(590, 100), 
            //             new Point(250, 590), new Point(300, 410)};
            g.FillPolygon(b, points);
            //bm.Save("bm.jpg", ImageFormat.Jpeg);
            return bm;

        }
        //public static Bitmap CreatePolygon(System.Windows.Media.PointCollection p)
        //{
        //    Point[] points = new Point[p.Count];
        //    for(int i = 0; i < p.Count; i++)
        //    {
        //        points[i] = new Point( Convert.ToInt32(p[i].X), Convert.ToInt32(p[i].Y));
        //    }
        //    Bitmap bm = new Bitmap(600, 600);
        //    Graphics g = Graphics.FromImage(bm);
        //    Brush b = new LinearGradientBrush(new Point(1, 1), new Point(600, 600), Color.White, Color.Red);
        //    //Point[] points = new Point[] { new Point(77, 500), new Point(590, 100), 
        //    //             new Point(250, 590), new Point(300, 410)};
        //    g.FillPolygon(b, points);
        //    //bm.Save("bm.jpg", ImageFormat.Jpeg);
        //    return bm;

        //}
        public static Bitmap CreatePolygon(List<Vector2d> p)
        {
            Point[] points = new Point[p.Count];
            for (int i = 0; i < p.Count; i++)
            {
                points[i] = new Point(Convert.ToInt32(p[i].X), Convert.ToInt32(p[i].Y));
            }
            Bitmap bm = new Bitmap(600, 600);
            Graphics g = Graphics.FromImage(bm);
            Brush b = new LinearGradientBrush(new Point(1, 1), new Point(600, 600), Color.White, Color.Red);
            //Point[] points = new Point[] { new Point(77, 500), new Point(590, 100), 
            //             new Point(250, 590), new Point(300, 410)};
            g.FillPolygon(b, points);
            //bm.Save("bm.jpg", ImageFormat.Jpeg);
            return bm;

        }
   
     

     
        public static void ChangeColor(string path)
        {
            Bitmap bmp = (Bitmap)Image.FromFile(path);
            //Benchmark.Start();
            LockBitmap lockBitmap = new LockBitmap(bmp);
            lockBitmap.LockBits();

            Color compareClr = Color.FromArgb(255, 255, 255, 255);

            for (int y = 0; y < lockBitmap.Height; y++)
            {
                for (int x = 0; x < lockBitmap.Width; x++)
                {
                    if (lockBitmap.GetPixel(x, y) == compareClr)
                    {
                        lockBitmap.SetPixel(x, y, Color.Red);
                    }
                }
            }
            lockBitmap.UnlockBits();
            //Benchmark.End();
            //double seconds = Benchmark.GetSeconds();
            bmp.Save(path);
        }
     

   
    }
}
