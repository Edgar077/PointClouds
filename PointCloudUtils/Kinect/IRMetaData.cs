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
    public class IRMetaData : MetaDataBase
    {
       
        InfraredFrame frameInfrared;
        
        private WriteableBitmap irBitmap;
        /// <summary>
        /// Maximum value (as a float) that can be returned by the InfraredFrame
        /// </summary>
        private const float InfraredSourceValueMaximum = (float)ushort.MaxValue;
        /// <summary>
        /// The value by which the infrared source data will be scaled
        /// </summary>
        private const float InfraredSourceScale = 0.75f;
        /// <summary>
        /// Smallest value to display when the infrared data is normalized
        /// </summary>
        private const float InfraredOutputValueMinimum = 0.01f;
        /// <summary>
        /// Largest value to display when the infrared data is normalized
        /// </summary>
        private const float InfraredOutputValueMaximum = 1.0f;


        public IRMetaData()
        {
        }

        public IRMetaData(InfraredFrame myframeIR, bool createBitmap)
        {
            this.frameInfrared = myframeIR;

            this.FrameData = AssignIRFrameData(myframeIR);


            if (createBitmap)
            {
                this.pixels = ImageExtensions.ConvertUshortToByte(this.FrameData);
                irBitmap = WriteableBitmapUtils.FromByteArray_ToGray(pixels, MetaDataBase.XDepthMaxKinect, MetaDataBase.YDepthMaxKinect);

            }

        }
        /// <summary>
        /// TODO - not correct yet
        /// </summary>
        /// <param name="myframeIR"></param>
        /// <returns></returns>
           private static ushort[] AssignIRFrameData(InfraredFrame myframeIR)
        {
            ushort[] frameData = new ushort[XDepthMaxKinect * YDepthMaxKinect];

            ushort[] pixelData = new ushort[XDepthMaxKinect * YDepthMaxKinect];

            myframeIR.CopyFrameDataToArray(frameData);

            int index = 0;
            for (int infraredIndex = 0; infraredIndex < frameData.Length; ++infraredIndex)
            {
                ushort ir = frameData[infraredIndex];
                byte intensity = (byte)(ir >> 8);

                pixelData[index] = intensity; 
               

                ++index;
            }

          
           
            return pixelData;

        }
   
        public WriteableBitmap DepthWriteableBitmap
        {
            get
            {
                if (irBitmap == null)
                {
                    this.pixels = ImageExtensions.ConvertUshortToByte(this.FrameData);
                    irBitmap = WriteableBitmapUtils.FromByteArray_ToGray(pixels, MetaDataBase.XDepthMaxKinect, MetaDataBase.YDepthMaxKinect);
                    

                }
                return irBitmap;
            }
        }
        public System.Drawing.Bitmap UpdateDepthImage(System.Drawing.Bitmap bm)
        {
            this.pixels = ImageExtensions.ConvertUshortToByte(this.FrameData);
            bm = bm.Update_Gray(pixels);
            return bm;

        }
 
        public static WriteableBitmap FromUShort(ushort[] myDepthFrame)
        {
            byte[] pixels = ImageExtensions.ConvertUshortToByte(myDepthFrame);
            WriteableBitmap depthBitmap = WriteableBitmapUtils.FromByteArray_ToGray(pixels, DepthMetaData.XDepthMaxKinect, DepthMetaData.YDepthMaxKinect);
            return depthBitmap;

        }
    

    
     

    }

  


}
