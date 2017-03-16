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

namespace PointCloudUtils
{
    public class BodyMetaData : MetaDataBase
    {
        /// <summary>
        /// The body index values.
        /// </summary>
        //public byte[] Pixels = null;
        int bodyIndexWidth;
        int bodyIndexHeight;

         public BodyMetaData(BodyIndexFrame myframeBodyIndex)
        {

            pixels = new byte[DepthMetaData.XDepthMaxKinect * DepthMetaData.YDepthMaxKinect];
            bodyIndexWidth = myframeBodyIndex.FrameDescription.Width;
            bodyIndexHeight = myframeBodyIndex.FrameDescription.Height;

            myframeBodyIndex.CopyFrameDataToArray(Pixels);

        }
         

    }
}
