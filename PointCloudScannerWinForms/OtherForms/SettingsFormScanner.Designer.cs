namespace PointCloudScanner
{
    partial class SettingsFormScanner
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.checkBoxEntropyImage = new System.Windows.Forms.CheckBox();
            this.buttonDefaultSettings = new System.Windows.Forms.Button();
            this.textBoxSaveIfQualityIsBetterThan = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxInterpolate = new System.Windows.Forms.CheckBox();
            this.checkBoxCutFrame = new System.Windows.Forms.CheckBox();
            this.checkBoxSaveAndStop = new System.Windows.Forms.CheckBox();
            this.checkBoxBackground = new System.Windows.Forms.CheckBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.checkBoxScanOnStart = new System.Windows.Forms.CheckBox();
            this.groupBoxScanner = new System.Windows.Forms.GroupBox();
            this.radioButtonKinectAndRealSense = new System.Windows.Forms.RadioButton();
            this.radioButtonIntelRealSense = new System.Windows.Forms.RadioButton();
            this.radioButtonKinect = new System.Windows.Forms.RadioButton();
            this.buttonRealSenseProperties = new System.Windows.Forms.Button();
            this.comboBoxCameraSelected = new System.Windows.Forms.ComboBox();
            this.checkBoxShowOnlyCalibrationModel = new System.Windows.Forms.CheckBox();
            this.checkBoxSkeleton = new System.Windows.Forms.CheckBox();
            this.checkBoxFace = new System.Windows.Forms.CheckBox();
            this.checkBoxFaceScan = new System.Windows.Forms.CheckBox();
            
            this.groupBoxScanner.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBoxEntropyImage
            // 
            this.checkBoxEntropyImage.AutoSize = true;
            this.checkBoxEntropyImage.Checked = true;
            this.checkBoxEntropyImage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxEntropyImage.Location = new System.Drawing.Point(367, 23);
            this.checkBoxEntropyImage.Name = "checkBoxEntropyImage";
            this.checkBoxEntropyImage.Size = new System.Drawing.Size(124, 17);
            this.checkBoxEntropyImage.TabIndex = 0;
            this.checkBoxEntropyImage.Text = "Show Entropy Image";
            this.checkBoxEntropyImage.UseVisualStyleBackColor = true;
            this.checkBoxEntropyImage.CheckedChanged += new System.EventHandler(this.checkBoxShowEntropyImage_CheckedChanged);
            // 
            // buttonDefaultSettings
            // 
            this.buttonDefaultSettings.Location = new System.Drawing.Point(217, 316);
            this.buttonDefaultSettings.Name = "buttonDefaultSettings";
            this.buttonDefaultSettings.Size = new System.Drawing.Size(163, 23);
            this.buttonDefaultSettings.TabIndex = 1;
            this.buttonDefaultSettings.Text = "Set Settings to default";
            this.buttonDefaultSettings.UseVisualStyleBackColor = true;
            this.buttonDefaultSettings.Click += new System.EventHandler(this.buttonDefaultSettings_Click);
            // 
            // textBoxSaveIfQualityIsBetterThan
            // 
            this.textBoxSaveIfQualityIsBetterThan.Location = new System.Drawing.Point(535, 62);
            this.textBoxSaveIfQualityIsBetterThan.Name = "textBoxSaveIfQualityIsBetterThan";
            this.textBoxSaveIfQualityIsBetterThan.Size = new System.Drawing.Size(112, 20);
            this.textBoxSaveIfQualityIsBetterThan.TabIndex = 2;
            this.textBoxSaveIfQualityIsBetterThan.TextChanged += new System.EventHandler(this.textBoxSaveIfQualityIsBetterThan_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(364, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Save if quality is better than";
            // 
            // checkBoxInterpolate
            // 
            this.checkBoxInterpolate.AutoSize = true;
            this.checkBoxInterpolate.Checked = true;
            this.checkBoxInterpolate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxInterpolate.Location = new System.Drawing.Point(367, 91);
            this.checkBoxInterpolate.Name = "checkBoxInterpolate";
            this.checkBoxInterpolate.Size = new System.Drawing.Size(113, 17);
            this.checkBoxInterpolate.TabIndex = 4;
            this.checkBoxInterpolate.Text = "Interpolate Images";
            this.checkBoxInterpolate.UseVisualStyleBackColor = true;
            this.checkBoxInterpolate.CheckedChanged += new System.EventHandler(this.checkBoxInterpolate_CheckedChanged);
            // 
            // checkBoxCutFrame
            // 
            this.checkBoxCutFrame.AutoSize = true;
            this.checkBoxCutFrame.Location = new System.Drawing.Point(367, 160);
            this.checkBoxCutFrame.Name = "checkBoxCutFrame";
            this.checkBoxCutFrame.Size = new System.Drawing.Size(82, 17);
            this.checkBoxCutFrame.TabIndex = 5;
            this.checkBoxCutFrame.Text = "Cut Frames ";
            this.checkBoxCutFrame.UseVisualStyleBackColor = true;
            this.checkBoxCutFrame.CheckedChanged += new System.EventHandler(this.checkBoxCutFrame_CheckedChanged);
            // 
            // checkBoxSaveAndStop
            // 
            this.checkBoxSaveAndStop.AutoSize = true;
            this.checkBoxSaveAndStop.Checked = true;
            this.checkBoxSaveAndStop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSaveAndStop.Location = new System.Drawing.Point(367, 137);
            this.checkBoxSaveAndStop.Name = "checkBoxSaveAndStop";
            this.checkBoxSaveAndStop.Size = new System.Drawing.Size(97, 17);
            this.checkBoxSaveAndStop.TabIndex = 6;
            this.checkBoxSaveAndStop.Text = "Save and Stop";
            this.checkBoxSaveAndStop.UseVisualStyleBackColor = true;
            this.checkBoxSaveAndStop.CheckedChanged += new System.EventHandler(this.checkBoxSaveAndStop_CheckedChanged);
            // 
            // checkBoxBackground
            // 
            this.checkBoxBackground.AutoSize = true;
            this.checkBoxBackground.Location = new System.Drawing.Point(367, 114);
            this.checkBoxBackground.Name = "checkBoxBackground";
            this.checkBoxBackground.Size = new System.Drawing.Size(127, 17);
            this.checkBoxBackground.TabIndex = 7;
            this.checkBoxBackground.Text = "Remove Background";
            this.checkBoxBackground.UseVisualStyleBackColor = true;
            this.checkBoxBackground.CheckedChanged += new System.EventHandler(this.checkBoxBackground_CheckedChanged);
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(268, 345);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 8;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // checkBoxScanOnStart
            // 
            this.checkBoxScanOnStart.AutoSize = true;
            this.checkBoxScanOnStart.Checked = true;
            this.checkBoxScanOnStart.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxScanOnStart.Location = new System.Drawing.Point(12, 12);
            this.checkBoxScanOnStart.Name = "checkBoxScanOnStart";
            this.checkBoxScanOnStart.Size = new System.Drawing.Size(130, 17);
            this.checkBoxScanOnStart.TabIndex = 9;
            this.checkBoxScanOnStart.Text = "Scan on start program";
            this.checkBoxScanOnStart.UseVisualStyleBackColor = true;
            this.checkBoxScanOnStart.CheckedChanged += new System.EventHandler(this.checkBoxScanOnStart_CheckedChanged);
            // 
            // groupBoxScanner
            // 
            this.groupBoxScanner.BackColor = System.Drawing.SystemColors.ActiveBorder;
           
            this.groupBoxScanner.Controls.Add(this.radioButtonKinectAndRealSense);
            this.groupBoxScanner.Controls.Add(this.radioButtonIntelRealSense);
            this.groupBoxScanner.Controls.Add(this.radioButtonKinect);
            this.groupBoxScanner.Location = new System.Drawing.Point(12, 51);
            this.groupBoxScanner.Name = "groupBoxScanner";
            this.groupBoxScanner.Size = new System.Drawing.Size(201, 116);
            this.groupBoxScanner.TabIndex = 10;
            this.groupBoxScanner.TabStop = false;
            this.groupBoxScanner.Text = "Start Window configured for:";
            // 
            // radioButtonKinectAndRealSense
            // 
            this.radioButtonKinectAndRealSense.AutoSize = true;
            this.radioButtonKinectAndRealSense.Location = new System.Drawing.Point(21, 66);
            this.radioButtonKinectAndRealSense.Name = "radioButtonKinectAndRealSense";
            this.radioButtonKinectAndRealSense.Size = new System.Drawing.Size(174, 17);
            this.radioButtonKinectAndRealSense.TabIndex = 2;
            this.radioButtonKinectAndRealSense.Text = "All: Kinect and Intel Real Sense";
            this.radioButtonKinectAndRealSense.UseVisualStyleBackColor = true;
            this.radioButtonKinectAndRealSense.CheckedChanged += new System.EventHandler(this.radioButtonKinectAndRealSense_CheckedChanged);
            // 
            // radioButtonIntelRealSense
            // 
            this.radioButtonIntelRealSense.AutoSize = true;
            this.radioButtonIntelRealSense.Location = new System.Drawing.Point(21, 43);
            this.radioButtonIntelRealSense.Name = "radioButtonIntelRealSense";
            this.radioButtonIntelRealSense.Size = new System.Drawing.Size(103, 17);
            this.radioButtonIntelRealSense.TabIndex = 1;
            this.radioButtonIntelRealSense.Text = "Intel Real Sense";
            this.radioButtonIntelRealSense.UseVisualStyleBackColor = true;
            this.radioButtonIntelRealSense.CheckedChanged += new System.EventHandler(this.radioButtonIntelRealSense_CheckedChanged);
            // 
            // radioButtonKinect
            // 
            this.radioButtonKinect.AutoSize = true;
            this.radioButtonKinect.Checked = true;
            this.radioButtonKinect.Location = new System.Drawing.Point(21, 20);
            this.radioButtonKinect.Name = "radioButtonKinect";
            this.radioButtonKinect.Size = new System.Drawing.Size(55, 17);
            this.radioButtonKinect.TabIndex = 0;
            this.radioButtonKinect.TabStop = true;
            this.radioButtonKinect.Text = "Kinect";
            this.radioButtonKinect.UseVisualStyleBackColor = true;
            this.radioButtonKinect.CheckedChanged += new System.EventHandler(this.radioButtonKinect_CheckedChanged);
            // 
            // buttonRealSenseProperties
            // 
            this.buttonRealSenseProperties.Location = new System.Drawing.Point(326, 246);
            this.buttonRealSenseProperties.Name = "buttonRealSenseProperties";
            this.buttonRealSenseProperties.Size = new System.Drawing.Size(159, 23);
            this.buttonRealSenseProperties.TabIndex = 11;
            this.buttonRealSenseProperties.Text = "Intel Real Sense Properties";
            this.buttonRealSenseProperties.UseVisualStyleBackColor = true;
            this.buttonRealSenseProperties.Click += new System.EventHandler(this.buttonRealSenseProperties_Click);
            // 
            // comboBoxCameraSelected
            // 
            this.comboBoxCameraSelected.FormattingEnabled = true;
            this.comboBoxCameraSelected.Location = new System.Drawing.Point(326, 275);
            this.comboBoxCameraSelected.Name = "comboBoxCameraSelected";
            this.comboBoxCameraSelected.Size = new System.Drawing.Size(321, 21);
            this.comboBoxCameraSelected.TabIndex = 13;
            // 
            // checkBoxShowOnlyCalibrationModel
            // 
            this.checkBoxShowOnlyCalibrationModel.AutoSize = true;
            this.checkBoxShowOnlyCalibrationModel.Location = new System.Drawing.Point(367, 183);
            this.checkBoxShowOnlyCalibrationModel.Name = "checkBoxShowOnlyCalibrationModel";
            this.checkBoxShowOnlyCalibrationModel.Size = new System.Drawing.Size(157, 17);
            this.checkBoxShowOnlyCalibrationModel.TabIndex = 14;
            this.checkBoxShowOnlyCalibrationModel.Text = "Show only calibration model";
            this.checkBoxShowOnlyCalibrationModel.UseVisualStyleBackColor = true;
            this.checkBoxShowOnlyCalibrationModel.CheckedChanged += new System.EventHandler(this.checkBoxShowOnlyCalibrationModel_CheckedChanged);
            // 
            // checkBoxSkeleton
            // 
            this.checkBoxSkeleton.AutoSize = true;
            this.checkBoxSkeleton.Location = new System.Drawing.Point(12, 198);
            this.checkBoxSkeleton.Name = "checkBoxSkeleton";
            this.checkBoxSkeleton.Size = new System.Drawing.Size(98, 17);
            this.checkBoxSkeleton.TabIndex = 15;
            this.checkBoxSkeleton.Text = "Show Skeleton";
            this.checkBoxSkeleton.UseVisualStyleBackColor = true;
            this.checkBoxSkeleton.CheckedChanged += new System.EventHandler(this.checkBoxSkeleton_CheckedChanged);
            // 
            // checkBoxFace
            // 
            this.checkBoxFace.AutoSize = true;
            this.checkBoxFace.Location = new System.Drawing.Point(12, 221);
            this.checkBoxFace.Name = "checkBoxFace";
            this.checkBoxFace.Size = new System.Drawing.Size(80, 17);
            this.checkBoxFace.TabIndex = 16;
            this.checkBoxFace.Text = "Show Face";
            this.checkBoxFace.UseVisualStyleBackColor = true;
            this.checkBoxFace.CheckedChanged += new System.EventHandler(this.checkBoxFace_CheckedChanged);
            // 
            // checkBoxFaceScan
            // 
            this.checkBoxFaceScan.AutoSize = true;
            this.checkBoxFaceScan.Location = new System.Drawing.Point(12, 244);
            this.checkBoxFaceScan.Name = "checkBoxFaceScan";
            this.checkBoxFaceScan.Size = new System.Drawing.Size(141, 17);
            this.checkBoxFaceScan.TabIndex = 17;
            this.checkBoxFaceScan.Text = "Show Face Scan Ellipse";
            this.checkBoxFaceScan.UseVisualStyleBackColor = true;
            this.checkBoxFaceScan.CheckedChanged += new System.EventHandler(this.checkBoxFaceScan_CheckedChanged);
          
            // 
            // SettingsFormScanner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 380);
            this.Controls.Add(this.checkBoxFaceScan);
            this.Controls.Add(this.checkBoxFace);
            this.Controls.Add(this.checkBoxSkeleton);
            this.Controls.Add(this.checkBoxShowOnlyCalibrationModel);
            this.Controls.Add(this.comboBoxCameraSelected);
            this.Controls.Add(this.buttonRealSenseProperties);
            this.Controls.Add(this.groupBoxScanner);
            this.Controls.Add(this.checkBoxScanOnStart);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.checkBoxBackground);
            this.Controls.Add(this.checkBoxSaveAndStop);
            this.Controls.Add(this.checkBoxCutFrame);
            this.Controls.Add(this.checkBoxInterpolate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxSaveIfQualityIsBetterThan);
            this.Controls.Add(this.buttonDefaultSettings);
            this.Controls.Add(this.checkBoxEntropyImage);
            this.Name = "SettingsFormScanner";
            this.Text = "SettingsForm";
            this.groupBoxScanner.ResumeLayout(false);
            this.groupBoxScanner.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxEntropyImage;
        private System.Windows.Forms.Button buttonDefaultSettings;
        private System.Windows.Forms.TextBox textBoxSaveIfQualityIsBetterThan;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxInterpolate;
        private System.Windows.Forms.CheckBox checkBoxCutFrame;
        private System.Windows.Forms.CheckBox checkBoxSaveAndStop;
        private System.Windows.Forms.CheckBox checkBoxBackground;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.CheckBox checkBoxScanOnStart;
        private System.Windows.Forms.GroupBox groupBoxScanner;
        private System.Windows.Forms.RadioButton radioButtonKinectAndRealSense;
        private System.Windows.Forms.RadioButton radioButtonIntelRealSense;
        private System.Windows.Forms.RadioButton radioButtonKinect;
        private System.Windows.Forms.Button buttonRealSenseProperties;
        private System.Windows.Forms.ComboBox comboBoxCameraSelected;
        private System.Windows.Forms.CheckBox checkBoxShowOnlyCalibrationModel;
        private System.Windows.Forms.CheckBox checkBoxSkeleton;
        private System.Windows.Forms.CheckBox checkBoxFace;
        private System.Windows.Forms.CheckBox checkBoxFaceScan;
       
    }
}