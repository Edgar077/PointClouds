using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PointCloudUtils;
using OpenTKExtension;
using OpenTKExtension.FastGLControl;

namespace PointCloudScanner
{

    public partial class ScannerUC : UserControl
    {
        public OpenTKExtension.OGLControl OglControl;

      
        private bool isScanning;

        PerformanceCheck performanceCheck = new PerformanceCheck();
        public RealsenseBO RealSenseBO;
        public KinectBO KinectBO;
        //public KinectFaceBO KinectFaceBO;

        List<MainForm> otherScanners;
        public System.Timers.Timer TimerSnapshots = new System.Timers.Timer();
        int imagesSaved = 0;

        public ScannerType Scanner;

        public ScannerUC()
        {
            
            
            InitializeComponent();

 //           AddOpenGLUserControl();
            OglControl = this.openGLUC.OGLControl;

            if (!PointCloudScannerSettings.IsInitializedFromSettings)
                PointCloudScannerSettings.InitFromSettings();

            InitFromSettings();


            TimerSnapshots.Elapsed += new System.Timers.ElapsedEventHandler(TimerSnapshots_Tick);
            InitKinectScanner();
            InitRealSenseScanner();
           
           
            this.isScanning = false;
            this.captureToolStripMenuItem.Text = "Scan";

            SwitchTabs();
           
        }
        //private void AddOpenGLUserControl()
        //{
        //    this.SuspendLayout();

        //    this.openGLUC = new OpenGLUC();

        //    this.tabPage3D.Controls.Add(this.openGLUC);
        //    this.openGLUC.Dock = System.Windows.Forms.DockStyle.Fill;
        //    this.openGLUC.Location = new System.Drawing.Point(3, 3);
        //    this.openGLUC.Name = "openGLUC";
        //    this.openGLUC.Size = new System.Drawing.Size(1202, 415);
        //    this.openGLUC.TabIndex = 0;

        //    this.ResumeLayout(false);
        //}

        public void InitKinectScanner()
        {
            KinectBO = new KinectBO(this, this.pictureBoxColor, this.pictureBoxDepth, this.pictureBoxIR, this.OglControl, this.pictureBoxEntropy, this.pictureBoxPolygon,
               this.labelFramesPerSecond, this.labelRefreshRateOpenGL, labelDepth1);
            KinectBO.OpenGLRefreshAt = PointCloudScannerSettings.OpenGLRefreshAt;

        }
        private void InitRealSenseWindows()
        {
            //second windows
            //otherScanners
            for (int i = 0; i < this.RealSenseBO.NumberOfDevices; i++)
            {
                otherScanners = new List<MainForm>();
                MainForm newForm = new MainForm();
                newForm.Text = "Camera: " + (i +1).ToString();
                
                newForm.ScannerUC.RealSenseBO.ScannerID = i;
                newForm.ScannerUC.Scanner = ScannerType.IntelRealsenseF200;
                this.otherScanners.Add(newForm);
                newForm.Show();
                newForm.ScannerUC.StartScanning();

            }

        }
        public void InitRealSenseScanner()
        {
            RealSenseBO = new RealsenseBO(this, this.pictureBoxColor, this.pictureBoxDepth, this.pictureBoxIR, this.OglControl, this.labelFramesPerSecond);
            this.RealSenseBO.DisplayType = PointCloudScannerSettings.DisplayType;
            RealSenseBO.OpenGLRefreshAt = PointCloudScannerSettings.OpenGLRefreshAt;


        }
    

        public bool StartScanning()
        {

            this.isScanning = true;
            captureToolStripMenuItem.Text = "Stop Scan";

            try
            {
                switch (this.Scanner)
                {
                    case ScannerType.MicrosoftKinect:
                        {
                            if (this.KinectBO.StartScanner())
                            {
                                
                                break; 

                            }
                            else
                            {
                                StopScannerUI();
                                return false;
                            }
                            
                        }
                    case ScannerType.IntelRealsenseF200:
                        {
                            if (this.RealSenseBO.StartScanner())
                            {
                                SwitchTabsRealsense();
                                break;
                            }
                            else
                            {
                                StopScannerUI();
                                return false;
                            }
                            
                        }
                   
                }
                if (otherScanners == null)
                {
                    return false;
                }

                for (int i = 0; i < this.otherScanners.Count; i++)
                {
                    MainForm mf = this.otherScanners[i];
                    mf.ScannerUC.StartScanning();
                }
               
                return true;

            }
            catch(Exception err)
            {
                System.Windows.Forms.MessageBox.Show("Cannot start scanning : " + err.Message);
                System.Diagnostics.Debug.WriteLine("Error connecting scanner " + err.Message);
                return false;
            }

        }

       
     

    

        private void buttonStopCapture_Click(object sender, EventArgs e)
        {
            this.StopScanner();
        }

        private void buttonScannerStart_Click(object sender, EventArgs e)
        {
            try
            {
                StartScanning();

            }
            catch(Exception err)
            {
                System.Windows.Forms.MessageBox.Show("Error connecting Scanner: " + err.Message);
            }
        }

      

        private void trackBarCutoffNear_ValueChanged(object sender, EventArgs e)
        {
            PointCloudScannerSettings.CutFrameMinDistance = Convert.ToInt32(trackBarCutoffNear.Value);

        }

        private void trackBarCutoffFar_ValueChanged(object sender, EventArgs e)
        {
            int newValue = 0;

            try
            {
                newValue = Convert.ToInt32(trackBarCutoffFar.Value);

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

        private void trackBarSnapshotNumber_ValueChanged(object sender, EventArgs e)
        {
            PointCloudScannerSettings.SnapshotNumberOfImages = Convert.ToInt32(trackBarSnapshotNumber.Value);
        }

        private void settingsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
            SettingsFormScanner sf = new SettingsFormScanner(this);
            sf.ShowDialog();

        }



   
        private void trackBarInterpolationNumber_ValueChanged(object sender, EventArgs e)
        {
            PointCloudScannerSettings.InterpolationNumberOfFrames = Convert.ToInt32(trackBarInterpolationNumber.Value);

        }

      
    
     

        /// <summary>
        /// at first load, the mouse wheel are not triggered on the control - probably error of MS control - trigger them explicitely
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("UserControl - MouseWheel");
            this.OglControl.MouseWheelActions(e);
            base.OnMouseWheel(e);
        }

       
        
        private void TimerSnapshots_Tick(object sender, EventArgs e)
        {

            if (imagesSaved == trackBarSnapshotNumber.Value)
            {
                TimerSnapshots.Stop();
                imagesSaved = 0;
            }
            else
            {
                imagesSaved++;
                System.Diagnostics.Debug.WriteLine("Save at: " + DateTime.Now.ToLongTimeString());
                SaveAll();
            }
        }

        private void openGLSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControlImages.SelectTab(0);

            SettingsForm openGLSettings = new SettingsForm(this.OglControl);
            openGLSettings.ShowDialog();
            this.OglControl.GLrender.ResetPointLineSizes();

        }
      
        private void tabControlImages_SelectedIndexChanged(object sender, EventArgs e)
        {


            SwitchTabs();

        }
        private void SwitchTabs()
        {

            switch (this.Scanner)
            {
                case ScannerType.MicrosoftKinect:
                    {
                        SwitchTabKinect();
                        break;
                    }
                case ScannerType.IntelRealsenseF200:
                    {
                        SwitchTabsRealsense();
                        break;
                    }
            }


        }
        public void SwitchTabKinect()
        {
            //at first start
            if (this.KinectBO == null)
                return;
            switch (this.tabControlImages.SelectedTab.Text)
            {

                case "3D":
                    {
                        if (!this.KinectBO.OpenGLPart.ShowingIn3DControl)
                        {
                            this.KinectBO.OpenGLPart.Start3DShow();

                        }
                        PointCloudScannerSettings.ScannerMode = ScannerMode.Color_Depth_3DDisplay;
                        break;
                    }
                case "Depth":
                    {
                        this.KinectBO.OpenGLPart.Stop3dShow();
                        PointCloudScannerSettings.ScannerMode = ScannerMode.Depth;
                        break;
                    }
                case "Color":
                    {
                        if (this.KinectBO.OpenGLPart.ShowingIn3DControl)
                        {
                            this.KinectBO.OpenGLPart.Stop3dShow();
                            //tabControlImages.SelectTab(0);
                            
                        }
                        PointCloudScannerSettings.ScannerMode = ScannerMode.Color_Depth;
                        break;
                    }
                case "Infrared":
                    {
                        if (this.KinectBO.OpenGLPart.ShowingIn3DControl)
                        {
                            this.KinectBO.OpenGLPart.Stop3dShow();
                            //tabControlImages.SelectTab(0);

                        }
                        
                        PointCloudScannerSettings.ScannerMode = ScannerMode.IR;
                        //this.realSenseBO.DisplayType = DisplayType.IR;
                        break;
                    }
                



            }


        }
        private void SwitchTabsRealsense()
        {
            switch (this.tabControlImages.SelectedTab.Text)
            {

                case "3D":
                    {

                        this.RealSenseBO.DisplayType = DisplayType.OpenGL;
                        if (!this.RealSenseBO.OpenGLPart.ShowingIn3DControl)
                        {
                            this.RealSenseBO.OpenGLPart.Start3DShow();
                           
                            
                        }
                        PointCloudScannerSettings.ScannerMode = ScannerMode.Color_Depth_3DDisplay;
                        break;
                    }
                case "Color":
                    {
                        this.RealSenseBO.OpenGLPart.Stop3dShow();
                        this.RealSenseBO.DisplayType = DisplayType.Color;
                        break;
                    }
                case "Infrared":
                    {
                        this.RealSenseBO.OpenGLPart.Stop3dShow();
                        this.RealSenseBO.DisplayType = DisplayType.IR;
                        break;
                    }
                case "Depth":
                    {
                        this.RealSenseBO.OpenGLPart.Stop3dShow();
                        this.RealSenseBO.DisplayType = DisplayType.Depth;
                        break;
                    }



            }
        }


        public void SwitchTabKinectFace()
        {
            //at first start
            if (this.KinectBO == null)
                return;
            switch (this.tabControlImages.SelectedTab.Text)
            {

                case "3D":
                    {
                        
                        PointCloudScannerSettings.ScannerMode = ScannerMode.Color_Depth_3DDisplay;
                        break;
                    }
                case "Depth":
                    {
                        
                        PointCloudScannerSettings.ScannerMode = ScannerMode.Depth;
                        break;
                    }
                case "Color":
                    {
                        
                        PointCloudScannerSettings.ScannerMode = ScannerMode.Color_Depth;
                        break;
                    }
                case "Infrared":
                    {
                      
                        PointCloudScannerSettings.ScannerMode = ScannerMode.IR;
                        //this.realSenseBO.DisplayType = DisplayType.IR;
                        break;
                    }




            }


        }

        // On form closing
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            switch (this.Scanner)
            {
                case ScannerType.MicrosoftKinect:
                    {
                       // SwitchTabKinect();
                        break;
                    }
                case ScannerType.IntelRealsenseF200:
                    {
                        this.RealSenseBO.StopScanner();
                        break;
                    }
            }

            
        }

  
        private void StopScannerUI()
        {
            this.isScanning = false;
            captureToolStripMenuItem.Text = "Scan";



        }
        public void StopScanner()
        {
            StopScannerUI();

            switch (this.Scanner)
            {
                case ScannerType.MicrosoftKinect:
                    {
                        this.KinectBO.StopScanner();
                        break;
                    }
                case ScannerType.IntelRealsenseF200:
                    {
                        this.RealSenseBO.StopScanner();
                        break;
                    }
             
            }
            
        }
      

        private void cameraConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (RealSenseBO != null)
            {
                this.RealSenseBO.ConfigurationDialog();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Only for Intel Realsense cameras");
            }
        }

       

      

        public void SaveAll()
        {
            StopScannerUI();

            switch (this.Scanner)
            {
                case ScannerType.MicrosoftKinect:
                    {
                        KinectBO.SaveAll();

                     
                        break;
                    }
                case ScannerType.IntelRealsenseF200:
                    {
                        
                        this.RealSenseBO.SaveAll();
                        

                        break;
                    }
                
            }

            if (otherScanners == null)
                return;

            for (int i = 0; i < this.otherScanners.Count; i++)
            {
                MainForm mf = this.otherScanners[i];
                mf.ScannerUC.SaveAll();
            }

        }
        public void SavePointCloud(string fileName)
        {
            if(!isScanning)
            {
                System.Windows.Forms.MessageBox.Show("Please start scanning first");
                return;
            }
            try
            {
                StopScanner();
                StopScannerUI();

                switch (this.Scanner)
                {
                    case ScannerType.MicrosoftKinect:
                        {

                            KinectBO.SavePointCloud(fileName);


                            break;
                        }
                    case ScannerType.IntelRealsenseF200:
                        {

                            this.RealSenseBO.SavePointCloud(fileName);


                            break;
                        }
               
                }

                if (otherScanners == null)
                    return;

                for (int i = 0; i < this.otherScanners.Count; i++)
                {
                    MainForm mf = this.otherScanners[i];
                    mf.ScannerUC.SaveAll();
                }

            }
            catch(Exception err)
            {
                System.Windows.Forms.MessageBox.Show("Error saving PC: " + err.Message);

            }
        }
        private void gTrackBarOpenGLAt_ValueChanged(object sender, EventArgs e)
        {
            PointCloudScannerSettings.OpenGLRefreshAt = TrackBarOpenGLAt.Value;

            if(this.RealSenseBO != null)
            {
                this.RealSenseBO.OpenGLRefreshAt = PointCloudScannerSettings.OpenGLRefreshAt;
            }
            if (this.KinectBO != null)
            {
                this.KinectBO.OpenGLRefreshAt = PointCloudScannerSettings.OpenGLRefreshAt;
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {

            InitRealSenseWindows();
        }

     
    
        //private void TestAlign()
        //{

        //    string fileName1 = @"A:\Archiv\3D\_Models\_Kinect Nico Nonne\Person4Angles\C1.obj";
        //    string fileName2 = @"A:\Archiv\3D\_Models\_Kinect Nico Nonne\Person4Angles\C2.obj";


        //    tabControlImages.SelectTab(3);
        //    //Model myModel = new Model(fileName1);
        //    Model myModel = new Model(fileName1);
        //    PCloud pcr = new PCloud();
        //    pcr.PointCloudOpenGL = myModel.pointCloudGL;
        //    pcr.PointCloudOpenGL.RotateDegrees(0f, 45f, 0f);
        //    pcr.PointCloudOpenGL.Translate(1.7f, 0f, -2f);

        //    IOUtils.ExtractDirectoryAndNameFromFileName(fileName1, ref pcr.PointCloudOpenGL.FileName, ref pcr.PointCloudOpenGL.Path);
        //    this.OglControl1.GLrender.ReplaceRenderableObject(pcr, true);

        //    Model myModel1 = new Model(fileName2);
        //    pcr = new PCloud();
        //    pcr.PointCloudOpenGL = myModel1.pointCloudGL;
        //    IOUtils.ExtractDirectoryAndNameFromFileName(fileName2, ref pcr.PointCloudOpenGL.FileName, ref pcr.PointCloudOpenGL.Path);

        //    pcr.PointCloudOpenGL.RotateDegrees(0f, -45f, 0f);
        //    pcr.PointCloudOpenGL.Translate(-1.91f, -0.1f, -1.55f);
        //    //pcr.PointCloudOpenGL.Translate(1.7f, 0f, -2f);

        //    this.OglControl1.GLrender.AddRenderableObject(pcr);



        //}


        private void OpenFilesDialog_MultiplePC()
        {
            OpenFileDialog openModel = new OpenFileDialog();
            openModel.Multiselect = true;

            if (openModel.ShowDialog() != DialogResult.OK)
                return;

            if (openModel.FileNames != null && openModel.FileNames.Length > 1)
            {
                tabControlImages.SelectTab(0);
                Model myModel = new Model(openModel.FileNames[0]);
                PointCloudRenderable pcr = new PointCloudRenderable();
                pcr.PointCloud = myModel.PointCloud;
                //pcr.PointCloudOpenGL.RotateDegrees(0f, -45f, 0f);
                IOUtils.ExtractDirectoryAndNameFromFileName(openModel.FileNames[0], out  pcr.PointCloud.FileNameLong, out pcr.PointCloud.Path);
                //this.OglControl1.GLrender.ReplaceRenderableObject(pcr, true);
                this.OglControl.GLrender.AddRenderableObject(pcr);

                Model myModel1 = new Model(openModel.FileNames[1]);
                pcr = new PointCloudRenderable();
                pcr.PointCloud = myModel1.PointCloud;
                IOUtils.ExtractDirectoryAndNameFromFileName(openModel.FileNames[1], out  pcr.PointCloud.FileNameLong, out pcr.PointCloud.Path);

                // pcr.PointCloudOpenGL.RotateDegrees(0f, -45f, 0f);
                //pcr.PointCloudOpenGL.Translate(-4f, 0f, 2f);

                this.OglControl.GLrender.AddRenderableObject(pcr);


            }
            else
                OpenFilesDialog_SinglePC();

        }


     
        private void savePointCloudToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (this.OglControl.GLrender.RenderableObjects.Count > 0)
            {
                for (int i = 0; i < this.OglControl.GLrender.RenderableObjects.Count; i++)
                {
                    RenderableObject o = null;
                    o = this.OglControl.GLrender.RenderableObjects[i];

                    if (o != null)
                    {
                        PointCloud pc = o.PointCloud;
                        pc.ToObjFile(pc.Path, IOUtils.ExtractFileNameWithoutExtension(pc.FileNameLong) + "_new.obj");

                    }
                }
            }
        }

        private void testAlignToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string fileName1 = @"A:\Archiv\3D\_Models\Chair_DifferentAngles\G1.obj";
            string fileName2 = @"A:\Archiv\3D\_Models\Chair_DifferentAngles\G2.obj";


            tabControlImages.SelectTab(0);
           
            Model myModel = new Model(fileName1);
            PointCloudRenderable pcr = new PointCloudRenderable();
            pcr.PointCloud = myModel.PointCloud;

            IOUtils.ExtractDirectoryAndNameFromFileName(fileName1, out pcr.PointCloud.FileNameLong, out  pcr.PointCloud.Path);
            this.OglControl.GLrender.AddRenderableObject(pcr);

            //this.OglControl1.GLrender.ReplaceRenderableObject(pcr, true);

            Model myModel1 = new Model(fileName2);
            pcr = new PointCloudRenderable();
            pcr.PointCloud = myModel1.PointCloud;
            IOUtils.ExtractDirectoryAndNameFromFileName(fileName2, out  pcr.PointCloud.FileNameLong, out pcr.PointCloud.Path);

         

            this.OglControl.GLrender.AddRenderableObject(pcr);


        }

        private void deleteAllPointCloudsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.openGLUC.RemoveAllModels();

            //this.OglControl.GLrender.RemoveAllModels();
            //this.OglControl.Refresh();

        }
       
        //public void UpdateOpenGLFramesPerSecond()
        //{
        //    this.framesOpenGL.Add(DateTime.Now);
        //    if (framesOpenGL.Count < 10)
        //        return;

        //    //int frameRate = frameListTimes.Count;
        //    TimeSpan ts = (DateTime)framesOpenGL[9] - (DateTime)framesOpenGL[0];
        //    if (ts.TotalSeconds > 0)
        //    {
        //        int frameRate = Convert.ToInt32(10 / ts.TotalSeconds);

        //        this.labelRefreshRateOpenGL.Text = frameRate.ToString() + " f/s";
        //        this.framesOpenGL = new System.Collections.ArrayList();
        //    }

        //}

        //private void openPointCloudToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    OpenFilesDialog_MultiplePC();

        //}
        //private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        //{

        //    if (this.OglControl1.GLrender.RenderableObjects.Count > 0)
        //    {
        //        for (int i = 0; i < this.OglControl1.GLrender.RenderableObjects.Count; i++)
        //        {
        //            RenderableObject o = null;
        //            o = this.OglControl1.GLrender.RenderableObjects[i];

        //            if (o != null)
        //            {
        //                PointCloud pc = o.PointCloudOpenGL;
        //                pc.ToObjFile(pc.Path, IOUtils.ExtractFileNameWithoutExtension(pc.FileName) + "_new." + IOUtils.ExtractExtension(pc.FileName));

        //            }
        //        }
        //    }


        //}

        private void saveAsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
            this.OglControl.SaveSelectedModelAs();
            
        }
       
       


        private void captureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (isScanning)
                {
                    this.StopScanner();
                    
                }
                else
                {
                    StartScanning();
                }
                
            }
            catch (Exception err)
            {
                System.Windows.Forms.MessageBox.Show("Error connecting Scanner: " + err.Message);
            }
        }

   

        private void buttonSavePC_Click(object sender, EventArgs e)
        {
            PointCloudScannerSettings.FileNameOBJ = DateTime.Now.Year.ToString() + "." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + "." + DateTime.Now.Hour.ToString() + "." + DateTime.Now.Minute.ToString() + "." + DateTime.Now.Second.ToString() + ".obj";
            this.SavePointCloud(PointCloudScannerSettings.FileNameOBJ);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (isScanning)
            {
                string strFileName = GLSettings.FileNamePointCloudLast1;
                strFileName = strFileName.Remove(strFileName.Length - 5, 5);

                strFileName += this.numericUpDownSave.Value.ToString();
                strFileName += ".obj";
                this.SavePointCloud(strFileName);

                if (this.numericUpDownSave.Value == this.numericUpDownSave.Maximum)
                    this.numericUpDownSave.Value = 1;
                else
                {
                    this.numericUpDownSave.Value = this.numericUpDownSave.Value + 1;
                    StartScanning();
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please start scanning first");
                
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenTKExtension.AboutForm fAbout = new AboutForm();
            fAbout.ShowDialog();
        }

        private void toolStripLineExtract_Click(object sender, EventArgs e)
        {
            //FeatureDetection.Lines((Bitmap)pictureBoxColor.Image);

            StopScanner();

            pictureBoxColor.Image = FeatureDetection.DetectLines((Bitmap)pictureBoxColor.Image);


            //FeatureDetection.CheckLines((Bitmap)pictureBoxColor.Image, Color.Red);

          


        }

        private void buttonSaveAll_Click(object sender, EventArgs e)
        {

            SaveAll();

            
        }

      

        private void recordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.Scanner == ScannerType.MicrosoftKinect && this.KinectBO != null && this.KinectBO.IsScanning)
            {
                this.KinectBO.RecordBVH();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("SW - Error - No BVH recording possible");
            }
            // BVH_IO bvhIO = new BVH_IO();

        }
        private void openPointCloudToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFilesDialog_SinglePC();
            //OpenFilesDialog_MultiplePC();
        }
        private void OpenFilesDialog_SinglePC()
        {
            OpenFileDialog openModel = new OpenFileDialog();
            if (openModel.ShowDialog() != DialogResult.OK)
                return;

            tabControlImages.SelectTab(0);
            PointCloud pc = PointCloud.FromObjFile(openModel.FileName);

           
            PointCloudRenderable pcr = new PointCloudRenderable();
            pcr.PointCloud = pc;
            IOUtils.ExtractDirectoryAndNameFromFileName(openModel.FileName, out pc.FileNameLong, out pc.Path);
            
            this.openGLUC.ShowPointCloud_ClearAllOthers(pc);
            this.Refresh();

            //this.OglControl.GLrender.ReplaceRenderableObject(pcr, true);
        }




    }
}
