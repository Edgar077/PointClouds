using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using Microsoft.Kinect;
using System.Windows.Media.Media3D;


using PointCloudUtils;

namespace ScannerWPF
{
    public partial class PointCloudUC
    {
        private System.Collections.ArrayList frameListTimes;

       

        KinectSensor _sensor;
        CoordinateMapper coordinateMapper = null;
        MultiSourceFrameReader _reader;
        bool bStopAfterFrameInterpolation = false;
       
   
        private int iFrameInterpolation = 0;
        private int numberOfCutPoints = 0;
        private List<ushort[]> pixelsList;

       

        public void ScannerConnect()
        {
            _sensor = KinectSensor.GetDefault();

            if (_sensor != null)
            {
                _sensor.Open();

                _reader = _sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Depth | FrameSourceTypes.BodyIndex);
                //_reader = _sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Depth);
                _reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;
                this.ImageScreenshot = new WriteableBitmap(this._sensor.DepthFrameSource.FrameDescription.Width, this._sensor.DepthFrameSource.FrameDescription.Height, 96.0, 96.0, PixelFormats.Gray8, null);
                this.coordinateMapper = this._sensor.CoordinateMapper;
                backgroundRemovalTool = new PointCloudUtils.BackgroundRemoval(_sensor.CoordinateMapper);
                
            }
            bStopAfterFrameInterpolation = false;


        }
        public void ScannerClose()
        {
            if (_reader != null)
            {
                _reader.Dispose();
            }


            if (_sensor != null)
            {
                _sensor.Close();
            }


        }
        private void ProcessColorDepthIR(MultiSourceFrame multiSourceFrame)
        {
            switch (PointCloudScannerSettings.ScannerMode)
            {
                case PointCloudUtils.ScannerMode.Color:
                    {
                        if (ProcessColorFrame(multiSourceFrame))
                            UpdateFramesPerSecond();

                        break;
                    }
                case PointCloudUtils.ScannerMode.Color_Depth:
                    {
                        if (ProcessColorFrame(multiSourceFrame) && ProcessDepthFrame(multiSourceFrame))
                            UpdateFramesPerSecond();
                        break;
                    }
                case PointCloudUtils.ScannerMode.Depth:
                    {
                        if (ProcessDepthFrame(multiSourceFrame))
                            UpdateFramesPerSecond();
                        break;
                    }
            }
        }
        void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            MultiSourceFrame multiSourceFrame = e.FrameReference.AcquireFrame();


            if (PointCloudScannerSettings.BackgroundRemoved)
            {
                ProcessBodyFrame(multiSourceFrame);
            }
            else
                ProcessColorDepthIR(multiSourceFrame);

        }

        private bool ProcessDepthFrame(MultiSourceFrame multiSourceFrame)
        {
            // Depth
            using (var frame = multiSourceFrame.DepthFrameReference.AcquireFrame())
            {

                if (frame != null)
                {
                    DepthFrame frameDepth = frame;
                    if (PointCloudScannerSettings.ScannerMode == ScannerMode.Depth || PointCloudScannerSettings.ScannerMode == ScannerMode.Color_Depth || PointCloudScannerSettings.ScannerMode == ScannerMode.Color_Depth_3DDisplay)
                    {
                        this.DepthMetaData = new DepthMetaData(frameDepth, false);


                        if (PointCloudScannerSettings.BackgroundRemoved)
                        {
                            backgroundRemovalTool.DepthFrameData_RemoveBackground(this.DepthMetaData, this.BodyMetaData);
                            if (PointCloudScannerSettings.CutFrames)
                                this.DepthMetaData.FrameData = DepthMetaData.CutDepth(this.DepthMetaData.FrameData, PointCloudScannerSettings.CutFrameMaxDistance, PointCloudScannerSettings.CutFrameMinDistance, ref numberOfCutPoints);
                            if(PointCloudScannerSettings.ScannerMode != ScannerMode.Color_Depth_3DDisplay)
                                this.imageDepth.Source = WriteableBitmapUtils.FromByteArray_ToGray(this.DepthMetaData.Pixels, DepthMetaData.XDepthMaxKinect, DepthMetaData.YDepthMaxKinect);


                        }
                        else
                        {
                            if (PointCloudScannerSettings.CutFrames)
                                this.DepthMetaData.FrameData = DepthMetaData.CutDepth(this.DepthMetaData.FrameData, PointCloudScannerSettings.CutFrameMaxDistance, PointCloudScannerSettings.CutFrameMinDistance, ref numberOfCutPoints);
                            if (PointCloudScannerSettings.ScannerMode != ScannerMode.Color_Depth_3DDisplay)
                                this.imageDepth.Source = DepthMetaData.FromUShort(DepthMetaData.FrameData);                            
                        }
                        if (PointCloudScannerSettings.InterpolateFrames)
                        {
                            CalculateInterpolatedPixels();
                        }
                     
                    }

                    return true;
                }

            }
            return false;
        }
    
     
        private bool ProcessColorFrame(MultiSourceFrame multiSourceFrame)
        {
            // Color
            using (var frame = multiSourceFrame.ColorFrameReference.AcquireFrame())
            {
                ColorFrame frameColor = frame;
                if (frame != null)
                {
                    this.ColorMetaData = new ColorMetaData(frameColor);

                    if (PointCloudScannerSettings.ScannerMode == ScannerMode.Color || PointCloudScannerSettings.ScannerMode == ScannerMode.Color_Depth)
                    {
                        //if (PointCloudScannerSettings.BackgroundRemoved)
                        //{
                        //    WriteableBitmap myBitmap = backgroundRemovalTool.Color_Bitmap(this.ColorMetaData, this.DepthMetaData, this.BodyMetaData);
                        //    this.imageColor.Source = myBitmap;

                        //}
                        //else
                        //{
                            if (PointCloudScannerSettings.ScannerMode != ScannerMode.Color_Depth_3DDisplay)
                                this.imageColor.Source = ImageSourceUtils.CreateImageSource(ColorMetaData.Pixels, ColorMetaData.XColorMaxKinect, ColorMetaData.YColorMaxKinect);
                        //}
                        
                    }

                    return true;
                }
            }
            return false;
        }
        private bool ProcessBodyFrame(MultiSourceFrame multiSourceFrame)
        {
            // Body
            using (var bodyIndexFrame = multiSourceFrame.BodyIndexFrameReference.AcquireFrame())
            {
                BodyIndexFrame frameBodyIndex = bodyIndexFrame;
                if (bodyIndexFrame != null && this.DepthMetaData != null)
                {
                    
                    this.BodyMetaData = new BodyMetaData(frameBodyIndex);
                    ProcessColorDepthIR(multiSourceFrame);
                    return true;
                }
            }
            return false;
        }
 



        void Dispatcher_ShutdownStarted(object sender, EventArgs e)
        {
            ScannerClose();
        }
    }
}
