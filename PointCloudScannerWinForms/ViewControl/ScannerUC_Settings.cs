using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PointCloudUtils;
using System.Windows.Forms;
using System.Drawing;

namespace PointCloudScanner
{
    public partial class ScannerUC
    {

       
        public void InitFromSettings()
        {
            SetScannerMode(PointCloudScannerSettings.ScannerMode);

            SetScanner(PointCloudScannerSettings.ScannerTypeDefault);
            this.trackBarCutoffFar.Value = Convert.ToInt32(PointCloudScannerSettings.CutFrameMaxDistance);
            this.trackBarCutoffNear.Value = Convert.ToInt32(PointCloudScannerSettings.CutFrameMinDistance);
            this.trackBarSnapshotNumber.Value = Convert.ToInt32(PointCloudScannerSettings.SnapshotNumberOfImages);
            this.trackBarInterpolationNumber.Value = Convert.ToInt32(PointCloudScannerSettings.InterpolationNumberOfFrames);
            this.TrackBarOpenGLAt.Value = Convert.ToInt32(PointCloudScannerSettings.OpenGLRefreshAt);



        }

        private void SetScanner(PointCloudUtils.ScannerType scannerType)
        {
            switch (scannerType)
            {
                case PointCloudUtils.ScannerType.MicrosoftKinect:
                    {

                        this.Scanner = PointCloudUtils.ScannerType.MicrosoftKinect;
                        break;
                    }
                case PointCloudUtils.ScannerType.IntelRealsenseF200:
                    {

                        this.Scanner = PointCloudUtils.ScannerType.IntelRealsenseF200;
                        break;
                    }
                case PointCloudUtils.ScannerType.KinectANDIntelRealsenseF200:
                    {

                        this.Scanner = PointCloudUtils.ScannerType.KinectANDIntelRealsenseF200;
                        break;
                    }
               

            }
        }
        private void SetScannerMode(PointCloudUtils.ScannerMode scannerMode)
        {
            switch (scannerMode)
            {
                case PointCloudUtils.ScannerMode.Color:
                    {
                       
                        
                        PointCloudScannerSettings.ScannerMode = PointCloudUtils.ScannerMode.Color;
                        break;
                    }
                case PointCloudUtils.ScannerMode.Depth:
                    {
                        
                        PointCloudScannerSettings.ScannerMode = PointCloudUtils.ScannerMode.Depth;
                        break;
                    }
                case PointCloudUtils.ScannerMode.Color_Depth:
                    {
                       
                        PointCloudScannerSettings.ScannerMode = PointCloudUtils.ScannerMode.Color_Depth;
                        break;
                    }
                case PointCloudUtils.ScannerMode.Color_Depth_3DDisplay:
                    {
                        
                        PointCloudScannerSettings.ScannerMode = PointCloudUtils.ScannerMode.Color_Depth_3DDisplay;
                        break;
                    }

            }


        }
    }
}
