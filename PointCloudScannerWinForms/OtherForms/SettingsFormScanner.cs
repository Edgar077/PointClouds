using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PointCloudUtils;
using OpenTKExtension;

namespace PointCloudScanner
{
    public partial class SettingsFormScanner : Form
    {
        ScannerUC scannerUC;
        public SettingsFormScanner(ScannerUC myscannerUC)
        {
            this.scannerUC = myscannerUC;
            InitializeComponent();
            InitFromSettings();
        }
        public void InitFromSettings()
        {

            this.checkBoxScanOnStart.Checked = PointCloudScannerSettings.ScanOnStartProgram;

            this.checkBoxInterpolate.Checked = PointCloudScannerSettings.InterpolateFrames;
            this.checkBoxBackground.Checked = PointCloudScannerSettings.BackgroundRemoved;
            this.checkBoxEntropyImage.Checked = PointCloudScannerSettings.EntropyImage;
            this.checkBoxSaveAndStop.Checked = PointCloudScannerSettings.SaveAndStop;
            this.checkBoxCutFrame.Checked = PointCloudScannerSettings.CutFrames;
            this.textBoxSaveIfQualityIsBetterThan.Text = PointCloudScannerSettings.SaveImageIfQualityIsBetterThan.ToString();

            this.checkBoxShowOnlyCalibrationModel.Checked = PointCloudScannerSettings.ShowOnlyCalibrationModel;

            this.checkBoxSkeleton.Checked = PointCloudScannerSettings.ShowSkeleton ;

            this.checkBoxFace.Checked = PointCloudScannerSettings.ShowFace;
            this.checkBoxFaceScan.Checked = PointCloudScannerSettings.ShowFaceScanEllipse;
            

            ScannerSettings();
            ComboBoxCameras();
        }
        private void ComboBoxCameras()
        {

            for (int i = 0; i < this.scannerUC.RealSenseBO.CameraStrings.Count; i++)
            {
                
                comboBoxCameraSelected.Items.Add(this.scannerUC.RealSenseBO.CameraStrings[i]);
              
            }
            if (this.scannerUC.RealSenseBO.ScannerID < 0)
            {
                System.Windows.Forms.MessageBox.Show("SW Error - Selected Index of Combo Box is -1");

            }
            if (comboBoxCameraSelected.Items.Count > 0)
                this.comboBoxCameraSelected.SelectedIndex = this.scannerUC.RealSenseBO.ScannerID;

        }
        private void ScannerSettings()
        {
            switch (this.scannerUC.Scanner )
            {

                case ScannerType.MicrosoftKinect://Vertex
                    {
                        this.radioButtonKinect.Checked = true;
                        break;
                    }
                case ScannerType.IntelRealsenseF200://Vertex
                    {
                        this.radioButtonIntelRealSense.Checked = true;
                        break;
                    }
                case ScannerType.KinectANDIntelRealsenseF200://Vertex
                    {
                        this.radioButtonKinectAndRealSense.Checked = true;
                        break;
                    }
             

            }

           
        }

        private void buttonDefaultSettings_Click(object sender, EventArgs e)
        {
            PointCloudScannerSettings.SetDefaultSettings();
        }

        private void textBoxSaveIfQualityIsBetterThan_TextChanged(object sender, EventArgs e)
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
    
        private void checkBoxInterpolate_CheckedChanged(object sender, EventArgs e)
        {
            if (PointCloudScannerSettings.InterpolateFrames != checkBoxInterpolate.Checked)
                PointCloudScannerSettings.InterpolateFrames = !PointCloudScannerSettings.InterpolateFrames;

            if (!PointCloudScannerSettings.InterpolateFrames)
            {
                if (PointCloudScannerSettings.EntropyImage)
                {
                    PointCloudScannerSettings.EntropyImage = false;
                    checkBoxEntropyImage.Checked = false;
                }
            }
        }

        private void checkBoxBackground_CheckedChanged(object sender, EventArgs e)
        {
           

            if (PointCloudScannerSettings.BackgroundRemoved != checkBoxBackground.Checked)
                PointCloudScannerSettings.BackgroundRemoved = !PointCloudScannerSettings.BackgroundRemoved;

        }

        private void checkBoxSaveAndStop_CheckedChanged(object sender, EventArgs e)
        {
            if (PointCloudScannerSettings.SaveAndStop != checkBoxSaveAndStop.Checked)
                PointCloudScannerSettings.SaveAndStop = !PointCloudScannerSettings.SaveAndStop;


        }

        private void checkBoxCutFrame_CheckedChanged(object sender, EventArgs e)
        {
            if (PointCloudScannerSettings.CutFrames != checkBoxCutFrame.Checked)
                PointCloudScannerSettings.CutFrames = !PointCloudScannerSettings.CutFrames;


        }

        private void checkBoxShowEntropyImage_CheckedChanged(object sender, EventArgs e)
        {
            if (PointCloudScannerSettings.EntropyImage != this.checkBoxEntropyImage.Checked)
                PointCloudScannerSettings.EntropyImage = !PointCloudScannerSettings.EntropyImage;

            if (PointCloudScannerSettings.EntropyImage)
            {
                if (!PointCloudScannerSettings.InterpolateFrames)
                {
                    PointCloudScannerSettings.InterpolateFrames = true;
                    checkBoxInterpolate.Checked = true;
                }
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            PointCloudScannerSettings.SaveSettings();
        }

        private void checkBoxScanOnStart_CheckedChanged(object sender, EventArgs e)
        {
            if (PointCloudScannerSettings.ScanOnStartProgram != this.checkBoxScanOnStart.Checked)
                PointCloudScannerSettings.ScanOnStartProgram = !PointCloudScannerSettings.ScanOnStartProgram;

           
        }

      

        private void buttonRealSenseProperties_Click(object sender, EventArgs e)
        {
            if (this.scannerUC.RealSenseBO != null)
            {
                this.scannerUC.RealSenseBO.ConfigurationDialog();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Only for Intel Realsense cameras");
            }
        }
        private void radioButtonKinect_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonKinect.Checked == true)
            {
                PointCloudScannerSettings.ScannerTypeDefault = ScannerType.MicrosoftKinect;
                this.scannerUC.Scanner = ScannerType.MicrosoftKinect;
                this.scannerUC.InitKinectScanner();
                this.scannerUC.SwitchTabKinect();
            }
            //else
            //{
            //    PointCloudScannerSettings.ScannerType = ScannerType.IntelRealsenseF200;
            //    this.scannerUC.InitRealSenseScanner();
            //}
        }

        private void radioButtonIntelRealSense_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonIntelRealSense.Checked == true)
            {
                PointCloudScannerSettings.ScannerTypeDefault = ScannerType.IntelRealsenseF200;
                this.scannerUC.InitRealSenseScanner();
            }
         
        }
    
        private void radioButtonKinectAndRealSense_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonKinectAndRealSense.Checked == true)
            {
                PointCloudScannerSettings.ScannerTypeDefault = ScannerType.KinectANDIntelRealsenseF200;
                this.scannerUC.Scanner = ScannerType.KinectANDIntelRealsenseF200;
                //this.scannerUC.InitRealSenseScanner();
            }

        }

        private void checkBoxShowOnlyCalibrationModel_CheckedChanged(object sender, EventArgs e)
        {
            if (PointCloudScannerSettings.ShowOnlyCalibrationModel != this.checkBoxShowOnlyCalibrationModel.Checked)
                PointCloudScannerSettings.ShowOnlyCalibrationModel = !PointCloudScannerSettings.ShowOnlyCalibrationModel;


            

        }

        private void checkBoxSkeleton_CheckedChanged(object sender, EventArgs e)
        {
            if (PointCloudScannerSettings.ShowSkeleton != this.checkBoxSkeleton.Checked)
                PointCloudScannerSettings.ShowSkeleton = !PointCloudScannerSettings.ShowSkeleton;

        }

        private void checkBoxFace_CheckedChanged(object sender, EventArgs e)
        {
            if (PointCloudScannerSettings.ShowFace != this.checkBoxFace.Checked)
                PointCloudScannerSettings.ShowFace = !PointCloudScannerSettings.ShowFace;
        }

        private void checkBoxFaceScan_CheckedChanged(object sender, EventArgs e)
        {
            if (PointCloudScannerSettings.ShowFaceScanEllipse != this.checkBoxFaceScan.Checked)
                PointCloudScannerSettings.ShowFaceScanEllipse = !PointCloudScannerSettings.ShowFaceScanEllipse;
        }

   

       
    }
}
