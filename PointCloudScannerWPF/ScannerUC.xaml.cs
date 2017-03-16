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
using System.Windows.Media.Media3D;

using Microsoft.Kinect;
using PointCloudUtils;


using OpenTKExtension;

namespace ScannerWPF
{
    /// <summary>
    /// </summary>
    public partial class PointCloudUC : UserControl
    {

        private delegate void DispatcherCallback();
        private ImageSource ImageScreenshot;
        BackgroundRemoval backgroundRemovalTool;
        //readonly int BYTES_PER_PIXEL = (PixelFormats.Bgr32.BitsPerPixel + 7) / 8;


        #region public properties

       

        
        public DepthMetaData DepthMetaData { get; set; }
        public ColorMetaData ColorMetaData { get; set; }
        public BodyMetaData BodyMetaData { get; set; }

        #endregion

        public PointCloudUC()
        {
            InitializeComponent();
            
            Dispatcher.ShutdownStarted += Dispatcher_ShutdownStarted;
            this.frameListTimes = new System.Collections.ArrayList();
            pathModels = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Models\\";

        }
        private void UpdateFramesPerSecond()
        {
            frameListTimes.Add(DateTime.Now);
            if (frameListTimes.Count < 10)
                return;

            //int frameRate = frameListTimes.Count;
            TimeSpan ts = (DateTime)frameListTimes[9] - (DateTime)frameListTimes[0];
            if (ts.TotalSeconds > 0)
            {
                int frameRate = Convert.ToInt32(10 / ts.TotalSeconds);

                this.labelFramesPerSecond.Content = frameRate.ToString() + " f/s";
                this.frameListTimes = new System.Collections.ArrayList();
            }

        }
      

   
  
        private WriteableBitmap CreateBitmapColorPlayer()
        {
            
          
            if (this.DepthMetaData != null && this.ColorMetaData != null && this.BodyMetaData != null)
            {
                
                return backgroundRemovalTool.Color_Bitmap(this.ColorMetaData, this.DepthMetaData, this.BodyMetaData);

            }
            return null;
            
        }
    
        private void ShowDepthScreenshot()
        {
            this.imageDepth.Source = null;
            
            if (this.DepthMetaData == null)
                return;

            WriteableBitmap depthBitmapTest =  WriteableBitmapUtils.FromByteArray_ToGray(this.DepthMetaData.Pixels, DepthMetaData.XDepthMaxKinect, DepthMetaData.YDepthMaxKinect);
            this.ImageScreenshot = depthBitmapTest;
            this.imageDepth.Source = depthBitmapTest;

        }

        
        private void SHowInterpolatedFrame()
        {
            this.imageEntropy.Source = WriteableBitmapUtils.FromByteArray_ToGray(DepthMetaData.Pixels, DepthMetaData.XDepthMaxKinect, DepthMetaData.YDepthMaxKinect);

        }
     
        private void SaveAll()
        {
            if (this.DepthMetaData != null)
            {
                SaveDepthPoints();
                SaveDepthBitmap();
                SaveDepthPointsInterpolated();
                SaveImageInterpolated();

                SaveColorBitmap();
                SaveDepthAndColor_DataAndImage();
            }

        }
        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {

            SaveAll();
           

        }
   

        private void checkBoxBackground_Checked(object sender, RoutedEventArgs e)
        {
            if (PointCloudScannerSettings.BackgroundRemoved != checkBoxBackground.IsChecked)
                PointCloudScannerSettings.BackgroundRemoved = !PointCloudScannerSettings.BackgroundRemoved;
        }

   

      
        private void checkBoxBackground_Click(object sender, RoutedEventArgs e)
        {
            if (PointCloudScannerSettings.BackgroundRemoved != checkBoxBackground.IsChecked)
                PointCloudScannerSettings.BackgroundRemoved = !PointCloudScannerSettings.BackgroundRemoved;
        }

        private void radtioButtonDepthColor_Click(object sender, RoutedEventArgs e)
        {
            PointCloudScannerSettings.ScannerMode = PointCloudUtils.ScannerMode.Color_Depth;
            
        }

        private void radtioButtonColor_Click(object sender, RoutedEventArgs e)
        {
            PointCloudScannerSettings.ScannerMode = PointCloudUtils.ScannerMode.Color;
        }

        private void radtioButtonDepth_Click(object sender, RoutedEventArgs e)
        {
            PointCloudScannerSettings.ScannerMode = PointCloudUtils.ScannerMode.Depth;
        }

        private void checkBoxInterpolate_Click(object sender, RoutedEventArgs e)
        {
            if (PointCloudScannerSettings.InterpolateFrames != checkBoxInterpolate.IsChecked)
                PointCloudScannerSettings.InterpolateFrames = !PointCloudScannerSettings.InterpolateFrames;

            if (!PointCloudScannerSettings.InterpolateFrames)
            {
                if (PointCloudScannerSettings.EntropyImage)
                {
                    PointCloudScannerSettings.EntropyImage = false;
                    checkBoxEntropyImage.IsChecked = false;
                }
            }
        }

        private void buttonScannerStart_Click(object sender, RoutedEventArgs e)
        {
            ScannerConnect();
        }

   
        public void SetDepthBitmap()
        {
            //for image viewing - rotate by 180 degrees
            //this.DepthMetaData.FrameData = DepthMetaData.RotateDepthFrame(this.DepthMetaData.FrameData, DepthMetaData.XResDefault, DepthMetaData.YResDefault);

            this.imageDepth.Source = WriteableBitmapUtils.FromByteArray_ToGray(this.DepthMetaData.Pixels, DepthMetaData.XDepthMaxKinect, DepthMetaData.YDepthMaxKinect);

        }

        private void checkBoxEntropyImage_Click(object sender, RoutedEventArgs e)
        {
            if (PointCloudScannerSettings.EntropyImage != this.checkBoxEntropyImage.IsChecked)
                PointCloudScannerSettings.EntropyImage = !PointCloudScannerSettings.EntropyImage;

            if (PointCloudScannerSettings.EntropyImage)
            {
                if (!PointCloudScannerSettings.InterpolateFrames)
                {
                    PointCloudScannerSettings.InterpolateFrames = true;
                    checkBoxInterpolate.IsChecked = true;
                }
            }
            
        }

        private void checkBoxSaveAndStop_Click(object sender, RoutedEventArgs e)
        {
            if (PointCloudScannerSettings.SaveAndStop != checkBoxSaveAndStop.IsChecked)
                PointCloudScannerSettings.SaveAndStop = !PointCloudScannerSettings.SaveAndStop;

            

        }
        
        
      
        private void checkBoxCutFrame_Click(object sender, RoutedEventArgs e)
        {
            if (PointCloudScannerSettings.CutFrames != checkBoxCutFrame.IsChecked)
                PointCloudScannerSettings.CutFrames = !PointCloudScannerSettings.CutFrames;

            
        }

        private void buttonStopCapture_Click(object sender, RoutedEventArgs e)
        {
            this.ScannerClose();
        }

        private void textBoxCutFrameMaxDistance_TextChanged(object sender, TextChangedEventArgs e)
        {
            int newValue = 0;
            
            try
            {
                newValue = Convert.ToInt32(textBoxCutFrameMaxDistance.Text);
                
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine("Error " + err.Message);
            }
            if (newValue > 0)
            {
                PointCloudScannerSettings.CutFrameMaxDistance = newValue;
            }
           
            
            
        }

        private void textBoxSaveIfQualityIsBetterThan_TextChanged(object sender, TextChangedEventArgs e)
        {
            int newValue = 0;
            try
            {
                newValue = Convert.ToInt32(textBoxSaveIfQualityIsBetterThan.Text);

            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine("Error " + err.Message);
            }
            if (newValue > 0)
            {
                PointCloudScannerSettings.SaveImageIfQualityIsBetterThan = newValue;
            }
            
        }
        private void buttonOpenSaved_Click(object sender, RoutedEventArgs e)
        {
            this.OpenSavedDepthData();
            //for image viewing - rotate by 180 degrees
            //this.DepthMetaData.FrameData = DepthMetaData.RotateDepthFrame(this.DepthMetaData.FrameData, DepthMetaData.XResDefault, DepthMetaData.YResDefault);

            ShowDepthScreenshot();
           
            this.OpenSavedColorDataWithDepth();
            ShowColorWithDepthScreenshot();

        }
        private void buttonShowPointCloud_Click(object sender, RoutedEventArgs e)
        {
            SaveAll();
            ScannerClose();

            this.OpenSavedDepthData();
            //for image viewing - rotate by 180 degrees
            //this.DepthMetaData.FrameData = DepthMetaData.RotateDepthFrame(this.DepthMetaData.FrameData, DepthMetaData.XResDefault, DepthMetaData.YResDefault);

            ShowDepthScreenshot();
            PointCloud pc = MetaDataBase.ToPointCloud(this.ColorMetaData, this.DepthMetaData, this.BodyMetaData, this.coordinateMapper);
           
            TestForm fOTK = new TestForm();
            fOTK.Show();
            fOTK.ShowPointCloud(pc);
            
            
        }

        private void buttonShowColorizedPointCloud_Click(object sender, RoutedEventArgs e)
        {
            SaveAll();
            ScannerClose();

            OpenSavedColorDataWithDepth();
            ShowDepthScreenshot();
            ShowColorWithDepthScreenshot();

            TestForm fOTK = new TestForm();
            fOTK.Show();


            PointCloud pc = MetaDataBase.ToPointCloud(this.ColorMetaData, this.DepthMetaData, this.BodyMetaData, this.coordinateMapper);
            fOTK.ShowPointCloud(pc);


        }
      
      
    }
  
}
