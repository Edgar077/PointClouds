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
using OpenTKExtension.FastGLControl;

namespace PointCloudScanner
{
    public partial class SaveDialog : Form
    {
        private ScannerUC scannerUC;
        public SaveDialog(ScannerUC myscannerUC)
        {
            this.scannerUC = myscannerUC;
            InitializeComponent();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            scannerUC.SaveAll();

        }

        private void buttonShowColorizedPointCloud_Click(object sender, EventArgs e)
        {
            
            switch (this.scannerUC.Scanner)
            {
                case ScannerType.MicrosoftKinect:
                    {

                        scannerUC.KinectBO.ShowPointCloud();
                        break;
                    }
                case ScannerType.IntelRealsenseF200:
                    {
                        //this.realSenseBO.StopScanner();
                        break;
                    }
            }


        
        }

        private void buttonOpenSaved_Click(object sender, EventArgs e)
        {

            PointCloud pc = PointCloud.FromObjFile(ScannerBase.PathModels, PointCloudScannerSettings.FileNameOBJ);
            scannerUC.KinectBO.ShowDepthColorImage(pc);

            scannerUC.OglControl.GLrender.ReplaceRenderableObject(pc.ToPointCloudRenderable(), false);

          
            
        }

        private void buttonSaveNSnapshots_Click(object sender, EventArgs e)
        {
            scannerUC.TimerSnapshots.Interval = 5000;
            scannerUC.TimerSnapshots.Start();


        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            scannerUC.RealSenseBO.DisplayProperties(this.Handle);
            
        }

        private void buttonSavePointCloud_Click(object sender, EventArgs e)
        {
            PointCloudScannerSettings.FileNameOBJ = DateTime.Now.Year.ToString() + "." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + "." + DateTime.Now.Hour.ToString() + "." + DateTime.Now.Minute.ToString() + "." + DateTime.Now.Second.ToString() + "_PointCloud.obj";

            scannerUC.SavePointCloud(PointCloudScannerSettings.FileNameOBJ);
            
        }
    }
}
