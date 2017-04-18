using OpenTK;

namespace OpenTKExtension
{
    partial class OpenGLUC
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
                this.glControl1.Dispose();
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStripTest = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTest1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTest2 = new System.Windows.Forms.ToolStripMenuItem();

            this.toolStripTriangulation = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripOutliers = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripOutliersBatch = new System.Windows.Forms.ToolStripMenuItem();
            



            this.toolStripLoadTwoPCL = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripPCAAxes = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripICP = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripPCA = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripLoadPointCloud = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSavePointCloud = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSavePointCloudAs = new System.Windows.Forms.ToolStripMenuItem();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showAxesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showCameraFOVMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTools = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripRemoveAllModels = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripRemoveSelectedModel = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripChangeColor = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripRegistration = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripShowRegistrationMatrix = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripCalculateRegistration1_2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripCalculateRegistrationLoaded_Moved = new System.Windows.Forms.ToolStripMenuItem();

            this.toolStripAlignUsingRegistrationMatrix = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripViewMode = new System.Windows.Forms.ToolStripLabel();
            this.comboRenderMode = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripFill = new System.Windows.Forms.ToolStripLabel();
            this.comboBoxFill = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripModels = new System.Windows.Forms.ToolStripLabel();
            this.comboModels = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripCameraModel = new System.Windows.Forms.ToolStripLabel();
            this.comboCameraModel = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripTransparency = new System.Windows.Forms.ToolStripLabel();
            this.comboTransparency = new System.Windows.Forms.ToolStripComboBox();
            this.glControl1 = new OpenTKExtension.OGLControl();
            this.menuStripMain.SuspendLayout();
            this.toolStripMain.SuspendLayout();
            this.SuspendLayout();

            // 
            // toolStrip1
            // 
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.toolStripViewMode,
            this.comboRenderMode,
            this.toolStripFill,
            this.comboBoxFill,
            this.toolStripModels,
            this.comboModels,
            this.toolStripCameraModel,
            this.comboCameraModel,
            this.toolStripTransparency,
            this.comboTransparency});

            // menuStrip1
            // 
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.toolStripTools,
            this.toolStripTest,
            this.aboutToolStripMenuItem});
          

            // 
            // toolStripLoadTwoPCL
            // 
            this.toolStripLoadTwoPCL.Name = "toolStripLoadTwoPCL";
            this.toolStripLoadTwoPCL.Size = new System.Drawing.Size(340, 22);
            this.toolStripLoadTwoPCL.Text = "Load two PCL";
            this.toolStripLoadTwoPCL.Click += new System.EventHandler(this.toolStripLoadTwoPCL_Click);
            // 
            // toolStripPCAAxes
            // 
            this.toolStripPCAAxes.Name = "toolStripPCAAxes";
            this.toolStripPCAAxes.Size = new System.Drawing.Size(340, 22);
            this.toolStripPCAAxes.Text = "Align PCL using PCA axes";
            this.toolStripPCAAxes.Click += new System.EventHandler(this.toolStripPCA_Axes_Click);
            // 
            // toolStripICP
            // 
            this.toolStripICP.Name = "toolStripICP";
            this.toolStripICP.Size = new System.Drawing.Size(340, 22);
            this.toolStripICP.Text = "Align PCL using ICP";
            this.toolStripICP.Click += new System.EventHandler(this.toolStripICP_Click);
            // 
            // toolStripPCA
            // 
            this.toolStripPCA.Name = "toolStripPCA";
            this.toolStripPCA.Size = new System.Drawing.Size(340, 22);
            this.toolStripPCA.Text = "Align PCL using PCA";
            this.toolStripPCA.Click += new System.EventHandler(this.toolStripPCA_Click);
            // 
            // menuStrip1
            // 
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStrip1";
            this.menuStripMain.Size = new System.Drawing.Size(1233, 24);
            this.menuStripMain.TabIndex = 0;
            this.menuStripMain.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLoadPointCloud,
            this.toolStripSavePointCloud,
            this.toolStripSavePointCloudAs,
            this.testToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // toolStripLoadModel
            // 
            this.toolStripLoadPointCloud.Name = "toolStripLoadModel";
            this.toolStripLoadPointCloud.Size = new System.Drawing.Size(178, 22);
            this.toolStripLoadPointCloud.Text = "Load Point Cloud";
            this.toolStripLoadPointCloud.Click += new System.EventHandler(this.toolStripLoadPointCloud_Click);
            // 
            // toolStripSavePointCloud
            // 
            this.toolStripSavePointCloud.Name = "toolStripSavePointCloud";
            this.toolStripSavePointCloud.Size = new System.Drawing.Size(178, 22);
            this.toolStripSavePointCloud.Text = "Save point cloud";
            this.toolStripSavePointCloud.Click += new System.EventHandler(this.toolStripSavePointCloud_Click);
            // 
            // toolStripSavePointCloudAs
            // 
            this.toolStripSavePointCloudAs.Name = "toolStripSavePointCloudAs";
            this.toolStripSavePointCloudAs.Size = new System.Drawing.Size(178, 22);
            this.toolStripSavePointCloudAs.Text = "Save point cloud As";
            this.toolStripSavePointCloudAs.Click += new System.EventHandler(this.toolStripSavePointCloudAs_Click);
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.testToolStripMenuItem.Text = "Add sphere";
            this.testToolStripMenuItem.Click += new System.EventHandler(this.testToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showAxesToolStripMenuItem,
            this.showGridToolStripMenuItem,
            this.showCameraFOVMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // showAxesToolStripMenuItem
            // 
            this.showAxesToolStripMenuItem.Name = "showAxesToolStripMenuItem";
            this.showAxesToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.showAxesToolStripMenuItem.Text = "Show Axis";
            this.showAxesToolStripMenuItem.Click += new System.EventHandler(this.showAxesToolStripMenuItem_Click);
            // 
            // showGridToolStripMenuItem
            // 
            this.showGridToolStripMenuItem.Name = "showGridToolStripMenuItem";
            this.showGridToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.showGridToolStripMenuItem.Text = "Show Grid";
            this.showGridToolStripMenuItem.Click += new System.EventHandler(this.showGridToolStripMenuItem_Click);
            // 
            // showCameraFOVMenuItem
            // 
            this.showCameraFOVMenuItem.Name = "showCameraFOVMenuItem";
            this.showCameraFOVMenuItem.Size = new System.Drawing.Size(172, 22);
            this.showCameraFOVMenuItem.Text = "Show Camera FOV";
            this.showCameraFOVMenuItem.Click += new System.EventHandler(this.showCameraFOV_Click);
            // 
            // toolStripTools
            // 
            this.toolStripTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripRemoveAllModels,
            this.toolStripRemoveSelectedModel,
            this.toolStripChangeColor,
            this.toolStripRegistration,
            this.settingsToolStripMenuItem});
            this.toolStripTools.Name = "toolStripTools";
            this.toolStripTools.Size = new System.Drawing.Size(47, 20);
            this.toolStripTools.Text = "Tools";
            // 
            // toolStripRemoveAllModels
            // 
            this.toolStripRemoveAllModels.Name = "toolStripRemoveAllModels";
            this.toolStripRemoveAllModels.Size = new System.Drawing.Size(244, 22);
            this.toolStripRemoveAllModels.Text = "Remove all models";
            this.toolStripRemoveAllModels.Click += new System.EventHandler(this.toolStripRemoveAllModelsToolStripMenuItem_Click);
            // 
            // toolStripRemoveSelectedModel
            // 
            this.toolStripRemoveSelectedModel.Name = "toolStripRemoveSelectedModel";
            this.toolStripRemoveSelectedModel.Size = new System.Drawing.Size(244, 22);
            this.toolStripRemoveSelectedModel.Text = "Remove selected model";
            this.toolStripRemoveSelectedModel.Click += new System.EventHandler(this.removeSelectedModelsToolStripMenuItem_Click);
            // 
            // toolStripChangeColor
            // 
            this.toolStripChangeColor.Name = "toolStripChangeColor";
            this.toolStripChangeColor.Size = new System.Drawing.Size(244, 22);
            this.toolStripChangeColor.Text = "Change Color of selected model";
            this.toolStripChangeColor.Click += new System.EventHandler(this.toolStripChangeColor_Click);
            // 
            // toolStripRegistration
            // 
            this.toolStripRegistration.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLoadTwoPCL,
            this.toolStripPCAAxes,
            this.toolStripICP,
            this.toolStripPCA,
            this.toolStripShowRegistrationMatrix,
            this.toolStripCalculateRegistration1_2,
            toolStripCalculateRegistrationLoaded_Moved,
            this.toolStripAlignUsingRegistrationMatrix});
            this.toolStripRegistration.Name = "toolStripRegistration";
            this.toolStripRegistration.Size = new System.Drawing.Size(244, 22);
            this.toolStripRegistration.Text = "Point Cloud Registration";
            this.toolStripRegistration.Click += new System.EventHandler(this.toolStripRegistration_Click);
            // 
            // toolStripShowRegistrationMatrix
            // 
            this.toolStripShowRegistrationMatrix.Name = "toolStripShowRegistrationMatrix";
            this.toolStripShowRegistrationMatrix.Size = new System.Drawing.Size(340, 22);
            this.toolStripShowRegistrationMatrix.Text = "Show Registration Matrix";
            this.toolStripShowRegistrationMatrix.Click += new System.EventHandler(this.toolStripShowRegistrationMatrix_Click);
            // 
            // toolStriptest
            // 
            this.toolStripTest.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTest1,
            this.toolStripTest2, toolStripOutliersBatch, toolStripTriangulation, toolStripOutliers});
            this.toolStripTest.Name = "Test";
            this.toolStripTest.Size = new System.Drawing.Size(37, 20);
            this.toolStripTest.Text = "Test";

            // 
            // 
            // 
            this.toolStripTest1.Name = "Test1";
            this.toolStripTest1.Size = new System.Drawing.Size(340, 22);
            this.toolStripTest1.Text = "Test1";
            this.toolStripTest1.Click += new System.EventHandler(this.toolStripTest1_Click);

            // 
            // 
            // 
            this.toolStripTriangulation.Name = "Triangulate";
            this.toolStripTriangulation.Size = new System.Drawing.Size(340, 22);
            this.toolStripTriangulation.Text = "Triangulate";
            this.toolStripTriangulation.Click += new System.EventHandler(this.toolStripTriangulate_Click);

            // 
            // 
            // 
            this.toolStripOutliers.Name = "toolStripOutliers";
            this.toolStripOutliers.Size = new System.Drawing.Size(340, 22);
            this.toolStripOutliers.Text = "Outliers";
            this.toolStripOutliers.Click += new System.EventHandler(this.toolStripOutliers_Click);


            // 
            // 
            // 
            this.toolStripOutliersBatch.Name = "toolStripOutliersBatch";
            this.toolStripOutliersBatch.Size = new System.Drawing.Size(340, 22);
            this.toolStripOutliersBatch.Text = "Outliers Batch";
            this.toolStripOutliersBatch.Click += new System.EventHandler(this.toolStripOutliersBatch_Click);

            // 
            // 
            // 
            this.toolStripTest2.Name = "Test2";
            this.toolStripTest2.Size = new System.Drawing.Size(340, 22);
            this.toolStripTest2.Text = "Test2";
            this.toolStripTest2.Click += new System.EventHandler(this.toolStripTest2_Click);




            // 
            // 
            // 
            this.toolStripCalculateRegistrationLoaded_Moved.Name = "toolStripCalculateRegistrationLoaded_Moved";
            this.toolStripCalculateRegistrationLoaded_Moved.Size = new System.Drawing.Size(340, 22);
            this.toolStripCalculateRegistrationLoaded_Moved.Text = "Calculate registration Loaded - Moved model 1";
            this.toolStripCalculateRegistrationLoaded_Moved.Click += new System.EventHandler(this.toolStripCalculateRegistrationLoaded_Moved_Click);
            // 
            // toolStripCalculateRegistrationMatrix
            // 
            this.toolStripCalculateRegistration1_2.Name = "toolStripCalculateRegistrationMatrix";
            this.toolStripCalculateRegistration1_2.Size = new System.Drawing.Size(340, 22);
            this.toolStripCalculateRegistration1_2.Text = "Calculate registration 1-2";
            this.toolStripCalculateRegistration1_2.Click += new System.EventHandler(this.toolStripCalculateRegistration1_2_Click);
            // 
            // toolStripAlignUsingRegistrationMatrix
            // 
            this.toolStripAlignUsingRegistrationMatrix.Name = "toolStripAlignUsingRegistrationMatrix";
            this.toolStripAlignUsingRegistrationMatrix.Size = new System.Drawing.Size(340, 22);
            this.toolStripAlignUsingRegistrationMatrix.Text = "Align first cloud using saved registration matrix";
            this.toolStripAlignUsingRegistrationMatrix.Click += new System.EventHandler(this.toolStripAlignUsingRegistrationMatrix_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(244, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStripMain.Location = new System.Drawing.Point(0, 24);
            this.toolStripMain.Name = "toolStripAll";
            this.toolStripMain.Size = new System.Drawing.Size(1233, 25);
            this.toolStripMain.TabIndex = 11;
            this.toolStripMain.Text = "toolStrip1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripViewMode
            // 
            this.toolStripViewMode.Name = "toolStripViewMode";
            this.toolStripViewMode.Size = new System.Drawing.Size(66, 22);
            this.toolStripViewMode.Text = "View Mode";
            // 
            // comboRenderMode
            // 
            this.comboRenderMode.Name = "comboRenderMode";
            this.comboRenderMode.Size = new System.Drawing.Size(121, 25);
            this.comboRenderMode.SelectedIndexChanged += new System.EventHandler(this.comboViewMode_SelectedIndexChanged);
            // 
            // toolStripFill
            // 
            this.toolStripFill.Name = "toolStripFill";
            this.toolStripFill.Size = new System.Drawing.Size(58, 22);
            this.toolStripFill.Text = "Fill-Mode";
            // 
            // comboBoxFill
            // 
            this.comboBoxFill.Name = "comboBoxFill";
            this.comboBoxFill.Size = new System.Drawing.Size(121, 25);
            this.comboBoxFill.SelectedIndexChanged += new System.EventHandler(this.comboBoxFill_SelectedIndexChanged);
            // 
            // toolStripModels
            // 
            this.toolStripModels.Name = "toolStripModels";
            this.toolStripModels.Size = new System.Drawing.Size(88, 22);
            this.toolStripModels.Text = "Selected Model";
            this.toolStripModels.Visible = true;

            // 
            // comboModels
            // 
            this.comboModels.Name = "comboModels";
            this.comboModels.Size = new System.Drawing.Size(121, 25);
            this.comboModels.SelectedIndexChanged += new System.EventHandler(this.comboModels_SelectedIndexChanged);
            // 
            // toolStripCameraModel
            // 
            this.toolStripCameraModel.Name = "toolStripCameraModel";
            this.toolStripCameraModel.Size = new System.Drawing.Size(127, 22);
            this.toolStripCameraModel.Text = "Mode: Camera/Model:";
            // 
            // comboCameraModel
            // 
            this.comboCameraModel.Name = "comboCameraModel";
            this.comboCameraModel.Size = new System.Drawing.Size(121, 25);
            this.comboCameraModel.SelectedIndexChanged += new System.EventHandler(this.comboCameraModel_SelectedIndexChanged);
            // 
            // transparency
            // 
            this.toolStripTransparency.Name = "toolStripTransparency";
            this.toolStripTransparency.Size = new System.Drawing.Size(77, 22);
            this.toolStripTransparency.Text = "Transparency";
            this.toolStripTransparency.Visible = false;
            // 
            // comboTransparency
            // 
            this.comboTransparency.Name = "comboTransparency";
            this.comboTransparency.Size = new System.Drawing.Size(121, 25);
            this.comboTransparency.Visible = false;
            this.comboTransparency.SelectedIndexChanged += new System.EventHandler(this.comboTransparency_SelectedIndexChanged);
            // 
            // glControl1
            // 
            this.glControl1.BackColor = System.Drawing.Color.Black;
            this.glControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glControl1.Location = new System.Drawing.Point(0, 49);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(1233, 533);
            this.glControl1.TabIndex = 12;
            this.glControl1.VSync = false;
            // 
            // OpenGLUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.glControl1);
            this.Controls.Add(this.toolStripMain);
            this.Controls.Add(this.menuStripMain);
            this.Name = "OpenGLUC";
            this.Size = new System.Drawing.Size(1233, 582);
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem toolStripLoadTwoPCL;
        private System.Windows.Forms.ToolStripMenuItem toolStripPCAAxes;
        private System.Windows.Forms.ToolStripMenuItem toolStripICP;
        private System.Windows.Forms.ToolStripMenuItem toolStripPCA;

        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripLoadPointCloud;
        private System.Windows.Forms.ToolStrip toolStripMain;
        private OGLControl glControl1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripViewMode;
        private System.Windows.Forms.ToolStripComboBox comboRenderMode;
        private System.Windows.Forms.ToolStripLabel toolStripModels;
        private System.Windows.Forms.ToolStripLabel toolStripCameraModel;
        private System.Windows.Forms.ToolStripComboBox comboModels;
        private System.Windows.Forms.ToolStripComboBox comboCameraModel;
        private System.Windows.Forms.ToolStripLabel toolStripTransparency;
        private System.Windows.Forms.ToolStripComboBox comboTransparency;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showAxesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripTools;
        private System.Windows.Forms.ToolStripMenuItem toolStripRegistration;
        private System.Windows.Forms.ToolStripMenuItem toolStripShowRegistrationMatrix;
        private System.Windows.Forms.ToolStripMenuItem toolStripCalculateRegistration1_2;
        private System.Windows.Forms.ToolStripMenuItem toolStripCalculateRegistrationLoaded_Moved;
        private System.Windows.Forms.ToolStripMenuItem toolStripAlignUsingRegistrationMatrix;
        private System.Windows.Forms.ToolStripMenuItem toolStripRemoveAllModels;
        private System.Windows.Forms.ToolStripMenuItem toolStripRemoveSelectedModel;

        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripLabel toolStripFill;
        private System.Windows.Forms.ToolStripComboBox comboBoxFill;
        private System.Windows.Forms.ToolStripMenuItem showGridToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showCameraFOVMenuItem;

        private System.Windows.Forms.ToolStripMenuItem toolStripSavePointCloud;
        private System.Windows.Forms.ToolStripMenuItem toolStripSavePointCloudAs;
        private System.Windows.Forms.ToolStripMenuItem toolStripChangeColor;
        private System.Windows.Forms.ToolStripMenuItem toolStripTest;
        private System.Windows.Forms.ToolStripMenuItem toolStripTest1;
        private System.Windows.Forms.ToolStripMenuItem toolStripTest2;
        private System.Windows.Forms.ToolStripMenuItem toolStripOutliersBatch;
        
        private System.Windows.Forms.ToolStripMenuItem toolStripTriangulation;
        private System.Windows.Forms.ToolStripMenuItem toolStripOutliers;



    }
}
