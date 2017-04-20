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
using OpenTK;



namespace ScannerWPF
{
    /// <summary>
    /// </summary>
    public partial class PointCloudUC 
    {
        private string pathModels = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + GLSettings.PathPointClouds;
        string lastFileOpened;
        private readonly System.IO.FileSystemWatcher _watcher = new System.IO.FileSystemWatcher();
       

        private void buttonSaveDepthPoints_Click(object sender, RoutedEventArgs e)
        {
            SaveDepthPoints();

        }
        public void OpenSavedDepthData()
        {
            List<OpenTK.Vector3> listPoints = UtilsPointCloudIO.Read_XYZ_Vectors(pathModels, GLSettings.FileNamePointCloudLast1);
            this.DepthMetaData = new DepthMetaData(listPoints, false);
            

        }
        public void OpenSavedColorDataWithDepth()
        {
            if (this.DepthMetaData == null)
                this.DepthMetaData = new DepthMetaData();

            byte[] colorInfo = null;
            //DepthMetaData.ReadDepthWithColor_PLY(pathModels, FileNamePLY, ref this.DepthMetaData.FrameData, ref colorInfo);
            DepthMetaData.ReadDepthWithColor_OBJ(pathModels, PointCloudScannerSettings.FileNameOBJ, ref this.DepthMetaData.FrameData, ref colorInfo);

            if (colorInfo != null)
            {
                if (this.ColorMetaData == null)
                    this.ColorMetaData = new ColorMetaData();

                this.ColorMetaData.SetColorPixels(colorInfo);

            }
            else
            {
                MessageBox.Show("Error reading a color Info With Depth: " + PointCloudScannerSettings.FileNamePLY + " - please create one first");
            }
           
        }
        private void ShowColorWithDepthScreenshot()
        {
            this.imageColor.Source = null;
            //byte[] mydisplayPixels = DepthMetaData.RotateColorInfoForDepth(ColorMetaData.Pixels, DepthMetaData.XResDefault, DepthMetaData.YResDefault);
            this.imageColor.Source = PointCloudUtils.ImageSourceUtils.CreateImageSource(ColorMetaData.Pixels, DepthMetaData.XDepthMaxKinect, DepthMetaData.YDepthMaxKinect);
            
        }
        private void SaveDepthBitmap()
        {
            WriteableBitmap depthBitmap = WriteableBitmapUtils.FromByteArray_ToGray(DepthMetaData.Pixels, DepthMetaData.XDepthMaxKinect, DepthMetaData.YDepthMaxKinect);

            WriteableBitmapUtils.SaveImage(pathModels, "Depth", depthBitmap, true);

            
        }

        public void SaveDepthPoints()
        {
            if (this.DepthMetaData == null)
            {
                MessageBox.Show("No Depth Data to save - please capture, or open last saved depth data");
                return;

            }
            //ushort[] rotatedPoints = DepthMetaData.RotateDepthFrame(this.DepthMetaData.FrameData, DepthMetaData.XResDefault, DepthMetaData.YResDefault);
            List<Vector3> listPoints = DepthMetaData.CreateListPoints_Depth(this.DepthMetaData.FrameData, DepthMetaData.XDepthMaxKinect, DepthMetaData.YDepthMaxKinect);
            GLSettings.FileNamePointCloudLast1 = DateTime.Now.Year.ToString() + "." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + "." + DateTime.Now.Hour.ToString() + "." + DateTime.Now.Minute.ToString() + "." + DateTime.Now.Second.ToString() + "_PointCloud.xyz";
            UtilsPointCloudIO.ToXYZFile(listPoints, GLSettings.FileNamePointCloudLast1, pathModels);
        }
      
        private void SaveDepthPointsInterpolated()
        {
            if (listPointsInterpolated == null)
                return;
            //-----------------------------------------------
            //now interpolate last 10 frames to one frame and save 

            PointCloudScannerSettings.InterpolateFrames = true;
            GLSettings.FileNamePointCloudLast1 = DateTime.Now.Year.ToString() + "." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + "." + DateTime.Now.Hour.ToString() + "." + DateTime.Now.Minute.ToString() + "." + DateTime.Now.Second.ToString() + "_PointCloudInterpolated.xyz";
            UtilsPointCloudIO.ToXYZFile(listPointsInterpolated, GLSettings.FileNamePointCloudLast1, pathModels);

        }
        private void SaveImageInterpolated()
        {
            if (listPointsInterpolated == null)
                return;
            //-----------------------------------------------
            //now interpolate last 10 frames to one frame and save 


            //ushort[] rotatedPoints = DepthMetaData.RotateDepthFrame(this.DepthMetaData.DepthFrameData, DepthMetaData.XResDefault, DepthMetaData.YResDefault);

            WriteableBitmap depthInterpolated = DepthMetaData.ToWriteableBitmap(listPointsInterpolated);
            WriteableBitmapUtils.SaveImage(pathModels, "DepthInterpolated", depthInterpolated, true);
            
        }
        private void SaveColorBitmap()
        {
            //write color Bitmap
            if (this.ColorMetaData != null)
            {
                //ImageSource imSource = ImageSourceUtils.CreateImageSource(ColorMetaData.Pixels, ColorMetaData.XResDefault, ColorMetaData.YResDefault);
                WriteableBitmap bitmap = WriteableBitmapUtils.FromByteArray_ToColor(ColorMetaData.Pixels, ColorMetaData.XColorMaxKinect, ColorMetaData.YColorMaxKinect);
                WriteableBitmapUtils.SaveImage(pathModels, "Color_", bitmap, true);
                
            }


        }

        private void SavePointCloudColor(PointCloud pc)
        {

            PointCloudScannerSettings.FileNameOBJ = DateTime.Now.Year.ToString() + "." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + "." + DateTime.Now.Hour.ToString() + "." + DateTime.Now.Minute.ToString() + "." + DateTime.Now.Second.ToString() + "_DepthAndColor.obj";
           // PointCloud pc = PointCloud.FromDepthColors(myColorPixels, this.DepthMetaData.FrameData, DepthMetaData.XResDefault, DepthMetaData.YResDefault);

            UtilsPointCloudIO.ToObjFile_ColorInVertex(pc, pathModels, PointCloudScannerSettings.FileNameOBJ);


            
        }
        private void SaveDepthAndColor_DataAndImage()
        {
            try
            {
                //byte[] myColorPixels = MetaDataBase.CreateColorInfoForDepth(this.ColorMetaData, this.DepthMetaData, this.coordinateMapper);
                 PointCloud pc = MetaDataBase.ToPointCloud(this.ColorMetaData, this.DepthMetaData, this.BodyMetaData, this.coordinateMapper);

                //List<Point3D> newList = DepthMetaData.CreateListPoint3D_Depth(this.DepthMetaData.FrameData, DepthMetaData.XResDefault, DepthMetaData.YResDefault);
                SavePointCloudColor(pc);

                WriteableBitmap bitmapCustom = WriteableBitmapUtils.FromPointCloud_ToColor(pc, DepthMetaData.XDepthMaxKinect, DepthMetaData.YDepthMaxKinect);
                WriteableBitmapUtils.SaveImage(pathModels, "ColorDepthSpace_", bitmapCustom, true);
            }
            catch(Exception err)
            {
                System.Diagnostics.Debug.WriteLine("Error " + err.Message);

            }
            
        }

        private void OnOpen(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Xaml files (*.xaml)|*.xaml";

            if (openFileDialog.ShowDialog().Value)
            {
                Load(openFileDialog.FileName);
            }
        }
     
        
        internal void Load(string pathFile)
        {
            if (pathFile != null && System.IO.File.Exists(pathFile))
            {
                LoadCore(pathFile);

                _watcher.BeginInit();
                _watcher.Path = System.IO.Path.GetDirectoryName(pathFile);
                _watcher.Filter = System.IO.Path.GetFileName(pathFile);
                _watcher.Changed += FileChanged;
                _watcher.EnableRaisingEvents = true;
                _watcher.EndInit();
            }
        }
        private void FileChanged(object sender, System.IO.FileSystemEventArgs e)
        {

            if (e.ChangeType == System.IO.WatcherChangeTypes.Changed)
            {
                this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new DispatcherCallback(delegate() { this.LoadCore(e.FullPath); }));
            }
        }
        private void LoadCore(string pathFile)
        {


            if (pathFile != null)
            {
                try
                {
                    using (System.IO.FileStream file = System.IO.File.OpenRead(pathFile))
                    {
                        lastFileOpened = pathFile;
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show(
                        String.Format("Unable to parse file:\r\n\r\n{0}",
                        err.Message), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void OnDefaultSettings(object sender, RoutedEventArgs e)
        {
            PointCloudScannerSettings.SetDefaultSettings();
        }

    }

}
