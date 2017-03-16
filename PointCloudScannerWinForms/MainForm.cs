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
    public partial class MainForm : Form
    {
        public ScannerUC ScannerUC;

        public MainForm()
        {
            try
            {
                //OpenTK.Toolkit.Init();
                InitializeComponent();
                AddUserControl();
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error loading - OpenGL problems? - " + ex.Message);
            }
            if (!PointCloudScannerSettings.IsInitializedFromSettings)
                PointCloudScannerSettings.InitFromSettings();
            if (!GLSettings.IsInitializedFromSettings)
                GLSettings.InitFromSettings();

            this.Height = PointCloudScannerSettings.Height;
            this.Width = PointCloudScannerSettings.Width;
            
            //ScannerUC.ScannerTypeDisplayed = PointCloudScannerSettings.ScannerTypeDefault;

        }
        private void AddUserControl()
        {
            this.SuspendLayout();
          

            ScannerUC = this.scannerUC;

            // 
            // scannerUC
            // 
            this.scannerUC = new PointCloudScanner.ScannerUC();
            this.scannerUC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scannerUC.Location = new System.Drawing.Point(0, 24);
            this.scannerUC.Name = "scannerUC";
            this.scannerUC.Size = new System.Drawing.Size(1403, 651);
            this.scannerUC.TabIndex = 1;
            this.scannerUC.Load += new System.EventHandler(this.PointCloudUC1_Load);
            this.Controls.Add(this.scannerUC);


            this.ResumeLayout(false);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (PointCloudScannerSettings.ScanOnStartProgram)
            {
                this.scannerUC.StartScanning();
            }
        }
        protected override void OnClosed(EventArgs e)
        {
            PointCloudScannerSettings.Height = this.Height;
            PointCloudScannerSettings.Width = this.Width;

            PointCloudScannerSettings.SaveSettings();
            GLSettings.SaveSettings();
            if(scannerUC != null)
                scannerUC.StopScanner();

            base.OnClosed(e);
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Window Key Down: ");

            //bool flag = false;
            if (e.KeyCode == Keys.S || e.KeyCode == Keys.Return)
            {
                this.scannerUC.SaveAll();
            }
            base.OnKeyDown(e);
        }

        private void PointCloudUC1_Load(object sender, EventArgs e)
        {

        }
    }
}
