
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointCloudUtils
{
    public static class PointCloudScannerSettings
    {
        
       
        public static string FileNamePLY ;
        public static string FileNameOBJ;
        public static string FileNameDepthOBJ ;

        public static bool BackgroundRemoved ;
        public static bool InterpolateFrames ;
        public static bool EntropyImage ;
        public static bool SaveAndStop ;
        public static bool CutFrames ;
        public static int CutFrameMaxDistance;
        public static int CutFrameMinDistance;
        public static int SaveImageIfQualityIsBetterThan;
        public static bool RotatePointCloud;
        
        public static int InterpolationNumberOfFrames;
        public static int SnapshotNumberOfImages;
        public static bool ShowOpenGLWindow ;
        public static int Height;
        public static int Width;
        public static bool IsInitializedFromSettings ;
        public static bool ScanOnStartProgram;

        public static DisplayType DisplayType ;
        public static ScannerMode ScannerMode ;
        public static ScannerType ScannerTypeDefault ;

        public static int OpenGLRefreshAt;
        public static bool ShowSkeleton;
        public static bool ShowFace;
        public static bool ShowFaceScanEllipse;

        public static bool ShowOnlyCalibrationModel;
      

        public static void InitFromSettings()
        {
            IsInitializedFromSettings = true;

            SetScannerMode(PointCloudUtils.Properties.Settings.Default.ScannerMode);
            SetDisplayType(PointCloudUtils.Properties.Settings.Default.DisplayType);
            SetScannerType(PointCloudUtils.Properties.Settings.Default.ScannerType);

            BackgroundRemoved = PointCloudUtils.Properties.Settings.Default.BackgroundRemoved;
            InterpolateFrames = PointCloudUtils.Properties.Settings.Default.InterpolateFrames;
     

            FileNameOBJ = PointCloudUtils.Properties.Settings.Default.FileColorInfoWithDepth;
            EntropyImage = PointCloudUtils.Properties.Settings.Default.EntropyImage;
            SaveAndStop = PointCloudUtils.Properties.Settings.Default.SaveAndStop;
            SaveImageIfQualityIsBetterThan = PointCloudUtils.Properties.Settings.Default.SaveImageIfQualityIsBetterThan;
            CutFrames = PointCloudUtils.Properties.Settings.Default.CutFrames;
            CutFrameMaxDistance = PointCloudUtils.Properties.Settings.Default.CutFrameMaxDistance ;
            CutFrameMinDistance = PointCloudUtils.Properties.Settings.Default.CutFrameMinDistance ;
            RotatePointCloud = PointCloudUtils.Properties.Settings.Default.RotatePointCloud;
            InterpolationNumberOfFrames = PointCloudUtils.Properties.Settings.Default.InterpolationNumberOfFrames;
            SnapshotNumberOfImages = PointCloudUtils.Properties.Settings.Default.SnapshotNumberOfImages;
            ShowOpenGLWindow = PointCloudUtils.Properties.Settings.Default.ShowOpenGLWindow;
            Height = PointCloudUtils.Properties.Settings.Default.Height;
            Width = PointCloudUtils.Properties.Settings.Default.Width;
            ScanOnStartProgram = PointCloudUtils.Properties.Settings.Default.ScanOnStartProgram;
            OpenGLRefreshAt = PointCloudUtils.Properties.Settings.Default.OpenGLRefreshAt;
            ShowOnlyCalibrationModel = PointCloudUtils.Properties.Settings.Default.ShowOnlyCalibrationModel;
            ShowSkeleton = PointCloudUtils.Properties.Settings.Default.ShowSkeleton;
            ShowFace = PointCloudUtils.Properties.Settings.Default.ShowFace;
            ShowFaceScanEllipse = PointCloudUtils.Properties.Settings.Default.ShowFaceScanEllipse;
        }
  

        public static void SaveSettings()
        {
            string scannerMode = ScannerMode.ToString();
            string scannerType = ScannerTypeDefault.ToString();
            string displayType = DisplayType.ToString();


            try
            {
                PointCloudUtils.Properties.Settings.Default.ScannerMode = scannerMode;
                PointCloudUtils.Properties.Settings.Default.ScannerType = scannerType;
                PointCloudUtils.Properties.Settings.Default.DisplayType = displayType;


                PointCloudUtils.Properties.Settings.Default.BackgroundRemoved = BackgroundRemoved;
                PointCloudUtils.Properties.Settings.Default.InterpolateFrames = InterpolateFrames;
              
                PointCloudUtils.Properties.Settings.Default.FileColorInfoWithDepth = FileNameOBJ;
                PointCloudUtils.Properties.Settings.Default.EntropyImage = EntropyImage;
                PointCloudUtils.Properties.Settings.Default.SaveAndStop = SaveAndStop;
                PointCloudUtils.Properties.Settings.Default.CutFrames = CutFrames;
                PointCloudUtils.Properties.Settings.Default.CutFrameMaxDistance = CutFrameMaxDistance;
                PointCloudUtils.Properties.Settings.Default.CutFrameMinDistance = CutFrameMinDistance;
                PointCloudUtils.Properties.Settings.Default.SaveImageIfQualityIsBetterThan = SaveImageIfQualityIsBetterThan;
                PointCloudUtils.Properties.Settings.Default.RotatePointCloud = RotatePointCloud;
                PointCloudUtils.Properties.Settings.Default.InterpolationNumberOfFrames = InterpolationNumberOfFrames;
                PointCloudUtils.Properties.Settings.Default.SnapshotNumberOfImages = SnapshotNumberOfImages;
                PointCloudUtils.Properties.Settings.Default.ShowOpenGLWindow = ShowOpenGLWindow;
                PointCloudUtils.Properties.Settings.Default.Height = Height;
                PointCloudUtils.Properties.Settings.Default.Width = Width;
                PointCloudUtils.Properties.Settings.Default.ScanOnStartProgram = ScanOnStartProgram;
                PointCloudUtils.Properties.Settings.Default.OpenGLRefreshAt = OpenGLRefreshAt ;
                PointCloudUtils.Properties.Settings.Default.ShowOnlyCalibrationModel = ShowOnlyCalibrationModel;
                PointCloudUtils.Properties.Settings.Default.ShowSkeleton = ShowSkeleton ;
                PointCloudUtils.Properties.Settings.Default.ShowFace = ShowFace;
                PointCloudUtils.Properties.Settings.Default.ShowFaceScanEllipse = ShowFaceScanEllipse;

                PointCloudUtils.Properties.Settings.Default.Save();
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine(err.Message);
            }
        }
      
        public static void SetDefaultSettings()
        {
            
            FileNamePLY = "DepthAndColor.ply";
            FileNameOBJ = "DepthAndColor.obj";
            FileNameDepthOBJ = "Depth.obj";

            ScannerMode = PointCloudUtils.ScannerMode.Depth;
            ScannerTypeDefault = PointCloudUtils.ScannerType.MicrosoftKinect;
            DisplayType = PointCloudUtils.DisplayType.Depth;

           

            BackgroundRemoved = false;
            InterpolateFrames = true;
            EntropyImage = true;
            SaveAndStop = true;
            CutFrames = true;
            CutFrameMaxDistance = 7500;
            CutFrameMinDistance = 500;
            SaveImageIfQualityIsBetterThan = 50;
            RotatePointCloud = true;
            SaveSettings();
            InterpolationNumberOfFrames = 10;
            SnapshotNumberOfImages = 5;
            ShowOpenGLWindow = false;
            Height = 600;
            Width = 1000;
            ScanOnStartProgram = false;
            OpenGLRefreshAt = 5;
            ShowOnlyCalibrationModel = false;
            ShowSkeleton = false;
            ShowFace = false;
            ShowFaceScanEllipse = false;
        }
        private static void SetScannerMode(string stringMode)
        {
            for (int i = 0; i < Enum.GetValues(typeof(ScannerMode)).GetLength(0); i++)
            {
                string strVal = Enum.GetValues(typeof(ScannerMode)).GetValue(i).ToString();
                if (stringMode == strVal)
                {
                    ScannerMode = (ScannerMode)Enum.GetValues(typeof(ScannerMode)).GetValue(i);


                }
            }

        }
        private static void SetScannerType(string stringMode)
        {
            for (int i = 0; i < Enum.GetValues(typeof(ScannerType)).GetLength(0); i++)
            {
                string strVal = Enum.GetValues(typeof(ScannerType)).GetValue(i).ToString();
                if (stringMode == strVal)
                {
                    ScannerTypeDefault = (ScannerType)Enum.GetValues(typeof(ScannerType)).GetValue(i);


                }
            }

        }
        private static void SetDisplayType(string displayType)
        {
            for (int i = 0; i < Enum.GetValues(typeof(DisplayType)).GetLength(0); i++)
            {
                string strVal = Enum.GetValues(typeof(DisplayType)).GetValue(i).ToString();
                if (displayType == strVal)
                {
                    DisplayType = (DisplayType)Enum.GetValues(typeof(DisplayType)).GetValue(i);


                }
            }

        }

    }
}
