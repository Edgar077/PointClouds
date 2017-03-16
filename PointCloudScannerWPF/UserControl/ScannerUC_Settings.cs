using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PointCloudUtils;

namespace ScannerWPF
{
    public partial class PointCloudUC
    {

       
        public void InitFromSettings()
        {
            SetScannerMode(PointCloudScannerSettings.ScannerMode);


            this.checkBoxInterpolate.IsChecked = PointCloudScannerSettings.InterpolateFrames;
            this.checkBoxBackground.IsChecked = PointCloudScannerSettings.BackgroundRemoved;
            this.checkBoxEntropyImage.IsChecked = PointCloudScannerSettings.EntropyImage;
            this.checkBoxSaveAndStop.IsChecked = PointCloudScannerSettings.SaveAndStop;
            this.checkBoxCutFrame.IsChecked = PointCloudScannerSettings.CutFrames;
            this.textBoxSaveIfQualityIsBetterThan.Text = PointCloudScannerSettings.SaveImageIfQualityIsBetterThan.ToString();
            textBoxCutFrameMaxDistance.Text = (PointCloudScannerSettings.CutFrameMaxDistance ).ToString();



        }

        private void SetScannerMode(PointCloudUtils.ScannerMode scannerMode)
        {
            switch (scannerMode)
            {
                case PointCloudUtils.ScannerMode.Color:
                    {
                        this.radtioButtonColor.IsChecked = true;
                        PointCloudScannerSettings.ScannerMode = PointCloudUtils.ScannerMode.Color;
                        break;
                    }
                case PointCloudUtils.ScannerMode.Depth:
                    {
                        this.radtioButtonDepth.IsChecked = true;
                        PointCloudScannerSettings.ScannerMode = PointCloudUtils.ScannerMode.Depth;
                        break;
                    }
                case PointCloudUtils.ScannerMode.Color_Depth:
                    {
                        this.radtioButtonDepthColor.IsChecked = true;
                        PointCloudScannerSettings.ScannerMode = PointCloudUtils.ScannerMode.Color_Depth;
                        break;
                    }
                case PointCloudUtils.ScannerMode.Color_Depth_3DDisplay:
                    {
                        this.radtioButtonDepthColor.IsChecked = true;
                        PointCloudScannerSettings.ScannerMode = PointCloudUtils.ScannerMode.Color_Depth;
                        break;
                    }

            }

        }
    }
}
