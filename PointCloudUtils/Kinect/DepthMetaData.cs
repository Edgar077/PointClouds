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
    public partial class DepthMetaData : MetaDataBase
    {
       
        DepthFrame frameDepth;
        
        private WriteableBitmap depthBitmap;


        public DepthMetaData()
        {
        }

        public DepthMetaData(DepthFrame myframeDepth, bool createBitmap)
        {
            this.frameDepth = myframeDepth;

            this.FrameData = AssignDepthFrameData(myframeDepth);

            if (createBitmap)
            {
                this.pixels = ImageExtensions.ConvertUshortToByte(this.FrameData);
                //depthBitmap = WriteableBitmapUtils.FromByteArray_ToGray(pixels, DepthMetaData.XDepthMaxKinect, DepthMetaData.YDepthMaxKinect);

            }

        }
        public DepthMetaData(List<OpenTK.Vector3> vectors, bool createBitmap)
        {

            this.FrameData = FromVector3List(vectors);
            if (createBitmap)
            {
                this.pixels = ImageExtensions.ConvertUshortToByte(this.FrameData);
                depthBitmap = WriteableBitmapUtils.FromByteArray_ToGray(pixels, DepthMetaData.XDepthMaxKinect, DepthMetaData.YDepthMaxKinect);

            }

        }
        public DepthMetaData(PointCloud  pc, bool createBitmap)
        {

            this.FrameData = FromPointCloud(pc);
            if (createBitmap)
            {
                this.pixels = ImageExtensions.ConvertUshortToByte(this.FrameData);
                //depthBitmap = WriteableBitmapUtils.FromByteArray_ToGray(pixels, DepthMetaData.XDepthMaxKinect, DepthMetaData.YDepthMaxKinect);

            }

        }
      
        public static ushort[] FromVector3List(List<OpenTK.Vector3> vectors)
        {
            ushort[] newData = new ushort[XDepthMaxKinect * YDepthMaxKinect];
            for (int i = 0; i < vectors.Count; i++)
            {
                OpenTK.Vector3 p3D = vectors[i];
                ushort x = Convert.ToUInt16(p3D.X);
                ushort y = Convert.ToUInt16(p3D.Y);
                ushort z = Convert.ToUInt16(p3D.Z);

                int depthIndex = (y * XDepthMaxKinect) + x;

                newData[depthIndex] = z;

            }


            return newData;


        }
        //public static ushort[] FromVector3List(List<OpenTK.Vector3> vectors)
        //{
        //    ushort[] newData = new ushort[XDepthMaxKinect * YDepthMaxKinect];
        //    for (int i = 0; i < vectors.Count; i++)
        //    {
        //        OpenTK.Vector3 p3D = vectors[i];
        //        ushort x = Convert.ToUInt32(p3D.X);
        //        ushort y = Convert.ToUInt32(p3D.Y);
        //        ushort z = Convert.ToUInt32(p3D.Z);

        //        int depthIndex = (y * XDepthMaxKinect) + x;

        //        newData[depthIndex] = z;

        //    }


        //    return newData;


        //}
   
        public static ushort[] CutDepth(ushort[] depthFrame, int maxDepth, int minDepth, ref int numberOfCutPoints)
        {
            int pixelsCutAbove = 0;
            int pixelsCutBelow = 0;

            
            ushort[] newDepthData = new ushort[depthFrame.GetLength(0)];
            int nCount = depthFrame.GetLength(0);
            for (int i = 0; i < nCount; i++)
            {
                if (depthFrame[i] >= maxDepth)
                {
                    pixelsCutAbove++;
                    newDepthData[i] = 0;
                }
                else if (depthFrame[i] <= minDepth)
                {
                    pixelsCutBelow++;
                    newDepthData[i] = 0;
                }
                else
                {
                    newDepthData[i] = depthFrame[i];
                }
            }

            numberOfCutPoints = pixelsCutAbove + pixelsCutBelow;
            
          
            return newDepthData;

        }
   
      
        public static WriteableBitmap FromUShort(ushort[] myDepthFrame)
        {
            byte[] pixels = ImageExtensions.ConvertUshortToByte(myDepthFrame);
            WriteableBitmap depthBitmap = WriteableBitmapUtils.FromByteArray_ToGray(pixels, DepthMetaData.XDepthMaxKinect, DepthMetaData.YDepthMaxKinect);
            return depthBitmap;

        }
    

        public static WriteableBitmap ToWriteableBitmap(List<Vector3> listOfPoints)
        {
            ushort[] myDepthFrame = FromVector3List(listOfPoints);

            byte[] pixels = ImageExtensions.ConvertUshortToByte(myDepthFrame);
            WriteableBitmap depthBitmap = WriteableBitmapUtils.FromByteArray_ToGray(pixels, DepthMetaData.XDepthMaxKinect, DepthMetaData.YDepthMaxKinect);
            
            return depthBitmap;

        }
        public static System.Drawing.Image ToImage(List<Vector3> listOfPoints)
        {
            ushort[] myDepthFrame = FromVector3List(listOfPoints);

            byte[] pixels = ImageExtensions.ConvertUshortToByte(myDepthFrame);


            System.Drawing.Image depthImage = BitmapExtensions.FromByteArray_Gray(pixels, DepthMetaData.XDepthMaxKinect, DepthMetaData.YDepthMaxKinect);
            return depthImage;


        }
        
      
        private static ushort[] AssignDepthFrameData(DepthFrame frameDepth)
        {
            ushort[] pixelData = new ushort[XDepthMaxKinect * YDepthMaxKinect];
          
            frameDepth.CopyFrameDataToArray(pixelData);
            
            return pixelData;

        }


        public static ushort[] FromPointCloud(PointCloud pc)
        {
            PointCloud pcNew = PointCloud_ToImageCloud(pc);

            int xdim = XDepthMaxKinect;
            int dim = XDepthMaxKinect * YDepthMaxKinect;
            ushort[] newData = new ushort[dim];
            for (int i = 0; i < pcNew.Count; i++)
            {
                Vector3 p3D = pcNew.Vectors[i];
                ushort x = Convert.ToUInt16(p3D.X);
                ushort y = Convert.ToUInt16(p3D.Y);
                ushort z = Convert.ToUInt16(p3D.Z);

                int depthIndex = (y * xdim) + x;
                if (depthIndex < dim)
                    newData[depthIndex] = z;
                else
                {
                    //System.Diagnostics.Debug.WriteLine("Pixel ignored ");
                }

            }

            return newData;

        }
        public static byte[] ToEntropyImage_ByteArray(int width, int height, ushort[] histogramData, int stride)
        {
            
            int ymin = 0;
            int colorMax = 32;
            byte[,] cmapArray = new byte[colorMax, 4];
            ColorMap cmap = new ColorMap();
            cmapArray = cmap.YellowRedBlack();

            int dy = height / (colorMax - ymin);
            int m = 64;

            for (int i = 0; i < colorMax; i++)
            {
                int colorIndex = (int)((i - ymin) * m / (colorMax - ymin));
                System.Drawing.SolidBrush aBrush = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(
                    cmapArray[colorIndex, 0], cmapArray[colorIndex, 1],
                    cmapArray[colorIndex, 2], cmapArray[colorIndex, 3]));
                //g.FillRectangle(aBrush, xPosition, yPosition + i * dy, width, dy);
            }

            var pixelFormat = PixelFormats.Bgra32;
            

            byte[] pixelData = new byte[height * stride];

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    var index = (y * stride) + (x * 4);

                    int depthIndex = (y * width) + x;
                    ushort zVal = histogramData[depthIndex];
                    if (zVal >= colorMax)
                        zVal = (ushort)(colorMax - 1);

                    //needed B - G - R - A
                    // Array: A-R-G-B
                    if (zVal == 0)
                    {
                        pixelData[index] = 255;
                        pixelData[index + 1] = 255;
                        pixelData[index + 2] = 255;
                        pixelData[index + 3] = 255; // 
                    }
                    else
                    {
                        pixelData[index] = cmapArray[zVal, 3];
                        pixelData[index + 1] = cmapArray[zVal, 2];
                        pixelData[index + 2] = cmapArray[zVal, 1];
                        pixelData[index + 3] = cmapArray[zVal, 0]; // 

                    }
                    
                }
            }

            return pixelData;

        }
        public static BitmapSource ToEntropyImage_BitmapSource(int width, int height, ushort[] histogramData)
        {
            int stride = width * 4; // bytes per row
            byte[] pixelData = ToEntropyImage_ByteArray(width, height, histogramData, stride);
            BitmapSource bitmap = BitmapSource.Create(width, height, 96, 96, PixelFormats.Bgra32, null, pixelData, stride);
           
            return bitmap;

        }
        public static System.Drawing.Bitmap ToEntropyImage_Bitmap(System.Drawing.Bitmap bitmap, int width, int height, ushort[] entropyData)
        {
            int stride = width * 4; // bytes per row
            byte[] pixelData = ToEntropyImage_ByteArray(width, height, entropyData, stride);
            System.Drawing.Bitmap bm = bitmap.Update_Color(pixelData);

            //BitmapSource bitmap = BitmapSource.Create(width, height, 96, 96, PixelFormats.Bgra32, null, pixelData, stride);

            return bm;

        }

     
    

    }

  


}
