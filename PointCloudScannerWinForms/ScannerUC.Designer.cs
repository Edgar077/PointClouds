using VBControls;

namespace PointCloudScanner
{
    partial class ScannerUC
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            VBControls.ColorPack colorPack76 = new VBControls.ColorPack();
            VBControls.ColorPack colorPack77 = new VBControls.ColorPack();
            VBControls.ColorPack colorPack78 = new VBControls.ColorPack();
            VBControls.ColorPack colorPack79 = new VBControls.ColorPack();
            VBControls.ColorPack colorPack80 = new VBControls.ColorPack();
            VBControls.ColorPack colorPack81 = new VBControls.ColorPack();
            VBControls.ColorPack colorPack82 = new VBControls.ColorPack();
            VBControls.ColorPack colorPack83 = new VBControls.ColorPack();
            VBControls.ColorPack colorPack84 = new VBControls.ColorPack();
            VBControls.ColorPack colorPack85 = new VBControls.ColorPack();
            VBControls.ColorPack colorPack86 = new VBControls.ColorPack();
            VBControls.ColorPack colorPack87 = new VBControls.ColorPack();
            VBControls.ColorPack colorPack88 = new VBControls.ColorPack();
            VBControls.ColorPack colorPack89 = new VBControls.ColorPack();
            VBControls.ColorPack colorPack90 = new VBControls.ColorPack();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.splitContainerUC = new System.Windows.Forms.SplitContainer();
            this.splitContainerLeft = new System.Windows.Forms.SplitContainer();
            this.buttonSaveAll = new System.Windows.Forms.Button();
            this.numericUpDownSave = new System.Windows.Forms.NumericUpDown();
            this.button2 = new System.Windows.Forms.Button();
            this.buttonSavePC = new System.Windows.Forms.Button();
            this.labelRefreshRateOpenGL = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.labelFramesPerSecond = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.captureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cameraConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openGLSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveDialogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openPointCloudToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.savePointCloudToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripLineExtract = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.splitContainerRight = new System.Windows.Forms.SplitContainer();
            this.tabControlImages = new System.Windows.Forms.TabControl();
            this.tabPage3D = new System.Windows.Forms.TabPage();
            this.openGLUC = new OpenTKExtension.OpenGLUC();
            this.tabPageDepth = new System.Windows.Forms.TabPage();
            this.pictureBoxDepth = new System.Windows.Forms.PictureBox();
            this.tabPageRGB2D = new System.Windows.Forms.TabPage();
            this.pictureBoxColor = new System.Windows.Forms.PictureBox();
            this.tabPageIR = new System.Windows.Forms.TabPage();
            this.pictureBoxIR = new System.Windows.Forms.PictureBox();
            this.tabPageStatistics = new System.Windows.Forms.TabPage();
            this.label9 = new System.Windows.Forms.Label();
            this.pictureBoxEntropy = new System.Windows.Forms.PictureBox();
            this.label8 = new System.Windows.Forms.Label();
            this.pictureBoxPolygon = new System.Windows.Forms.PictureBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.labelDepth1 = new System.Windows.Forms.Label();
            this.labelDepth0 = new System.Windows.Forms.Label();
            this.labelDepth2 = new System.Windows.Forms.Label();
            this.labelDepth4 = new System.Windows.Forms.Label();
            this.labelDepth3 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.TrackBarOpenGLAt = new VBControls.gTrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.trackBarSnapshotNumber = new VBControls.gTrackBar();
            this.trackBarInterpolationNumber = new VBControls.gTrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.trackBarCutoffFar = new VBControls.gTrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.trackBarCutoffNear = new VBControls.gTrackBar();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.recordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerUC)).BeginInit();
            this.splitContainerUC.Panel1.SuspendLayout();
            this.splitContainerUC.Panel2.SuspendLayout();
            this.splitContainerUC.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLeft)).BeginInit();
            this.splitContainerLeft.Panel1.SuspendLayout();
            this.splitContainerLeft.Panel2.SuspendLayout();
            this.splitContainerLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSave)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRight)).BeginInit();
            this.splitContainerRight.Panel1.SuspendLayout();
            this.splitContainerRight.Panel2.SuspendLayout();
            this.splitContainerRight.SuspendLayout();
            this.tabControlImages.SuspendLayout();
            this.tabPage3D.SuspendLayout();
            this.tabPageDepth.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDepth)).BeginInit();
            this.tabPageRGB2D.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxColor)).BeginInit();
            this.tabPageIR.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIR)).BeginInit();
            this.tabPageStatistics.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEntropy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPolygon)).BeginInit();
            this.SuspendLayout();
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(4, 928);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // splitContainerUC
            // 
            this.splitContainerUC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerUC.Location = new System.Drawing.Point(4, 0);
            this.splitContainerUC.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitContainerUC.Name = "splitContainerUC";
            // 
            // splitContainerUC.Panel1
            // 
            this.splitContainerUC.Panel1.Controls.Add(this.splitContainerLeft);
            // 
            // splitContainerUC.Panel2
            // 
            this.splitContainerUC.Panel2.Controls.Add(this.splitContainerRight);
            this.splitContainerUC.Size = new System.Drawing.Size(2066, 928);
            this.splitContainerUC.SplitterDistance = 288;
            this.splitContainerUC.SplitterWidth = 5;
            this.splitContainerUC.TabIndex = 2;
            // 
            // splitContainerLeft
            // 
            this.splitContainerLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerLeft.Location = new System.Drawing.Point(0, 0);
            this.splitContainerLeft.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitContainerLeft.Name = "splitContainerLeft";
            this.splitContainerLeft.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerLeft.Panel1
            // 
            this.splitContainerLeft.Panel1.Controls.Add(this.buttonSaveAll);
            this.splitContainerLeft.Panel1.Controls.Add(this.numericUpDownSave);
            this.splitContainerLeft.Panel1.Controls.Add(this.button2);
            this.splitContainerLeft.Panel1.Controls.Add(this.buttonSavePC);
            this.splitContainerLeft.Panel1.Controls.Add(this.labelRefreshRateOpenGL);
            this.splitContainerLeft.Panel1.Controls.Add(this.label11);
            this.splitContainerLeft.Panel1.Controls.Add(this.label10);
            this.splitContainerLeft.Panel1.Controls.Add(this.labelFramesPerSecond);
            this.splitContainerLeft.Panel1.Controls.Add(this.menuStrip1);
            // 
            // splitContainerLeft.Panel2
            // 
            this.splitContainerLeft.Panel2.Controls.Add(this.button1);
            this.splitContainerLeft.Size = new System.Drawing.Size(288, 928);
            this.splitContainerLeft.SplitterDistance = 450;
            this.splitContainerLeft.SplitterWidth = 5;
            this.splitContainerLeft.TabIndex = 0;
            // 
            // buttonSaveAll
            // 
            this.buttonSaveAll.Location = new System.Drawing.Point(112, 290);
            this.buttonSaveAll.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonSaveAll.Name = "buttonSaveAll";
            this.buttonSaveAll.Size = new System.Drawing.Size(100, 28);
            this.buttonSaveAll.TabIndex = 15;
            this.buttonSaveAll.Text = "Save All";
            this.buttonSaveAll.UseVisualStyleBackColor = true;
            this.buttonSaveAll.Click += new System.EventHandler(this.buttonSaveAll_Click);
            // 
            // numericUpDownSave
            // 
            this.numericUpDownSave.Location = new System.Drawing.Point(199, 258);
            this.numericUpDownSave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numericUpDownSave.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numericUpDownSave.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownSave.Name = "numericUpDownSave";
            this.numericUpDownSave.Size = new System.Drawing.Size(45, 22);
            this.numericUpDownSave.TabIndex = 14;
            this.numericUpDownSave.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(28, 254);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(164, 28);
            this.button2.TabIndex = 13;
            this.button2.Text = "For alignment: Save #";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // buttonSavePC
            // 
            this.buttonSavePC.Location = new System.Drawing.Point(28, 218);
            this.buttonSavePC.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonSavePC.Name = "buttonSavePC";
            this.buttonSavePC.Size = new System.Drawing.Size(164, 28);
            this.buttonSavePC.TabIndex = 12;
            this.buttonSavePC.Text = "Save Point Cloud 1";
            this.buttonSavePC.UseVisualStyleBackColor = true;
            this.buttonSavePC.Click += new System.EventHandler(this.buttonSavePC_Click);
            // 
            // labelRefreshRateOpenGL
            // 
            this.labelRefreshRateOpenGL.AutoSize = true;
            this.labelRefreshRateOpenGL.Location = new System.Drawing.Point(24, 175);
            this.labelRefreshRateOpenGL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelRefreshRateOpenGL.Name = "labelRefreshRateOpenGL";
            this.labelRefreshRateOpenGL.Size = new System.Drawing.Size(62, 17);
            this.labelRefreshRateOpenGL.TabIndex = 11;
            this.labelRefreshRateOpenGL.Text = "frames/s";
            this.labelRefreshRateOpenGL.Visible = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(24, 145);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(108, 17);
            this.label11.TabIndex = 10;
            this.label11.Text = "3D refresh rate ";
            this.label11.Visible = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(16, 68);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(124, 17);
            this.label10.TabIndex = 9;
            this.label10.Text = "Scanner scan rate";
            // 
            // labelFramesPerSecond
            // 
            this.labelFramesPerSecond.AutoSize = true;
            this.labelFramesPerSecond.Location = new System.Drawing.Point(24, 102);
            this.labelFramesPerSecond.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelFramesPerSecond.Name = "labelFramesPerSecond";
            this.labelFramesPerSecond.Size = new System.Drawing.Size(62, 17);
            this.labelFramesPerSecond.TabIndex = 8;
            this.labelFramesPerSecond.Text = "frames/s";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.captureToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.dToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(288, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // captureToolStripMenuItem
            // 
            this.captureToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.captureToolStripMenuItem.ForeColor = System.Drawing.Color.DarkRed;
            this.captureToolStripMenuItem.Name = "captureToolStripMenuItem";
            this.captureToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.captureToolStripMenuItem.Text = "Scan";
            this.captureToolStripMenuItem.Click += new System.EventHandler(this.captureToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cameraConfigToolStripMenuItem,
            this.openGLSettingsToolStripMenuItem,
            this.settingsToolStripMenuItem1,
            this.saveDialogToolStripMenuItem,
            this.recordToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(56, 24);
            this.settingsToolStripMenuItem.Text = "Tools";
            // 
            // cameraConfigToolStripMenuItem
            // 
            this.cameraConfigToolStripMenuItem.Name = "cameraConfigToolStripMenuItem";
            this.cameraConfigToolStripMenuItem.Size = new System.Drawing.Size(258, 26);
            this.cameraConfigToolStripMenuItem.Text = "Real Sense Camera Config";
            this.cameraConfigToolStripMenuItem.Click += new System.EventHandler(this.cameraConfigToolStripMenuItem_Click);
            // 
            // openGLSettingsToolStripMenuItem
            // 
            this.openGLSettingsToolStripMenuItem.Name = "openGLSettingsToolStripMenuItem";
            this.openGLSettingsToolStripMenuItem.Size = new System.Drawing.Size(258, 26);
            this.openGLSettingsToolStripMenuItem.Text = "OpenGL Settings";
            this.openGLSettingsToolStripMenuItem.Click += new System.EventHandler(this.openGLSettingsToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem1
            // 
            this.settingsToolStripMenuItem1.Name = "settingsToolStripMenuItem1";
            this.settingsToolStripMenuItem1.Size = new System.Drawing.Size(258, 26);
            this.settingsToolStripMenuItem1.Text = "Settings";
            this.settingsToolStripMenuItem1.Click += new System.EventHandler(this.settingsToolStripMenuItem1_Click);
            // 
            // saveDialogToolStripMenuItem
            // 
            this.saveDialogToolStripMenuItem.Name = "saveDialogToolStripMenuItem";
            this.saveDialogToolStripMenuItem.Size = new System.Drawing.Size(258, 26);
            this.saveDialogToolStripMenuItem.Text = "Save Dialog";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(258, 26);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // dToolStripMenuItem
            // 
            this.dToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openPointCloudToolStripMenuItem1,
            this.savePointCloudToolStripMenuItem,
            this.saveAsToolStripMenuItem1,
            this.toolStripLineExtract});
            this.dToolStripMenuItem.Name = "dToolStripMenuItem";
            this.dToolStripMenuItem.Size = new System.Drawing.Size(80, 24);
            this.dToolStripMenuItem.Text = "3D Test";
            // 
            // openPointCloudToolStripMenuItem1
            // 
            this.openPointCloudToolStripMenuItem1.Name = "openPointCloudToolStripMenuItem1";
            this.openPointCloudToolStripMenuItem1.Size = new System.Drawing.Size(263, 26);
            this.openPointCloudToolStripMenuItem1.Text = "Open Point Cloud";
            this.openPointCloudToolStripMenuItem1.Click += new System.EventHandler(this.openPointCloudToolStripMenuItem1_Click);
            // 
            // savePointCloudToolStripMenuItem
            // 
            this.savePointCloudToolStripMenuItem.Name = "savePointCloudToolStripMenuItem";
            this.savePointCloudToolStripMenuItem.Size = new System.Drawing.Size(263, 26);
            this.savePointCloudToolStripMenuItem.Text = "Save Point Cloud";
            this.savePointCloudToolStripMenuItem.Click += new System.EventHandler(this.savePointCloudToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem1
            // 
            this.saveAsToolStripMenuItem1.Name = "saveAsToolStripMenuItem1";
            this.saveAsToolStripMenuItem1.Size = new System.Drawing.Size(263, 26);
            this.saveAsToolStripMenuItem1.Text = "Save As";
            this.saveAsToolStripMenuItem1.Click += new System.EventHandler(this.saveAsToolStripMenuItem1_Click);
            // 
            // toolStripLineExtract
            // 
            this.toolStripLineExtract.Name = "toolStripLineExtract";
            this.toolStripLineExtract.Size = new System.Drawing.Size(263, 26);
            this.toolStripLineExtract.Text = "Line extraction from image";
            this.toolStripLineExtract.Click += new System.EventHandler(this.toolStripLineExtract_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(8, 28);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(91, 171);
            this.button1.TabIndex = 14;
            this.button1.Text = "Start All Real Sense Cameras";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // splitContainerRight
            // 
            this.splitContainerRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerRight.Location = new System.Drawing.Point(0, 0);
            this.splitContainerRight.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitContainerRight.Name = "splitContainerRight";
            this.splitContainerRight.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerRight.Panel1
            // 
            this.splitContainerRight.Panel1.Controls.Add(this.tabControlImages);
            // 
            // splitContainerRight.Panel2
            // 
            this.splitContainerRight.Panel2.Controls.Add(this.label12);
            this.splitContainerRight.Panel2.Controls.Add(this.TrackBarOpenGLAt);
            this.splitContainerRight.Panel2.Controls.Add(this.label4);
            this.splitContainerRight.Panel2.Controls.Add(this.trackBarSnapshotNumber);
            this.splitContainerRight.Panel2.Controls.Add(this.trackBarInterpolationNumber);
            this.splitContainerRight.Panel2.Controls.Add(this.label3);
            this.splitContainerRight.Panel2.Controls.Add(this.label2);
            this.splitContainerRight.Panel2.Controls.Add(this.trackBarCutoffFar);
            this.splitContainerRight.Panel2.Controls.Add(this.label1);
            this.splitContainerRight.Panel2.Controls.Add(this.trackBarCutoffNear);
            this.splitContainerRight.Size = new System.Drawing.Size(1773, 928);
            this.splitContainerRight.SplitterDistance = 773;
            this.splitContainerRight.SplitterWidth = 5;
            this.splitContainerRight.TabIndex = 0;
            // 
            // tabControlImages
            // 
            this.tabControlImages.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControlImages.Controls.Add(this.tabPage3D);
            this.tabControlImages.Controls.Add(this.tabPageDepth);
            this.tabControlImages.Controls.Add(this.tabPageRGB2D);
            this.tabControlImages.Controls.Add(this.tabPageIR);
            this.tabControlImages.Controls.Add(this.tabPageStatistics);
            this.tabControlImages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlImages.ItemSize = new System.Drawing.Size(71, 15);
            this.tabControlImages.Location = new System.Drawing.Point(0, 0);
            this.tabControlImages.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabControlImages.Name = "tabControlImages";
            this.tabControlImages.SelectedIndex = 0;
            this.tabControlImages.Size = new System.Drawing.Size(1773, 773);
            this.tabControlImages.TabIndex = 2;
            this.tabControlImages.SelectedIndexChanged += new System.EventHandler(this.tabControlImages_SelectedIndexChanged);
            // 
            // tabPage3D
            // 
            this.tabPage3D.Controls.Add(this.openGLUC);
            this.tabPage3D.Location = new System.Drawing.Point(4, 19);
            this.tabPage3D.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage3D.Name = "tabPage3D";
            this.tabPage3D.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage3D.Size = new System.Drawing.Size(1765, 750);
            this.tabPage3D.TabIndex = 1;
            this.tabPage3D.Text = "3D";
            this.tabPage3D.UseVisualStyleBackColor = true;
            // 
            // openGLUC
            // 
            this.openGLUC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.openGLUC.Location = new System.Drawing.Point(4, 4);
            this.openGLUC.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.openGLUC.Name = "openGLUC";
            this.openGLUC.Size = new System.Drawing.Size(1757, 742);
            this.openGLUC.TabIndex = 0;
            // 
            // tabPageDepth
            // 
            this.tabPageDepth.Controls.Add(this.pictureBoxDepth);
            this.tabPageDepth.Location = new System.Drawing.Point(4, 19);
            this.tabPageDepth.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPageDepth.Name = "tabPageDepth";
            this.tabPageDepth.Size = new System.Drawing.Size(1765, 750);
            this.tabPageDepth.TabIndex = 3;
            this.tabPageDepth.Text = "Depth";
            this.tabPageDepth.UseVisualStyleBackColor = true;
            // 
            // pictureBoxDepth
            // 
            this.pictureBoxDepth.BackColor = System.Drawing.Color.Olive;
            this.pictureBoxDepth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxDepth.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxDepth.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBoxDepth.Name = "pictureBoxDepth";
            this.pictureBoxDepth.Size = new System.Drawing.Size(1765, 750);
            this.pictureBoxDepth.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxDepth.TabIndex = 1;
            this.pictureBoxDepth.TabStop = false;
            // 
            // tabPageRGB2D
            // 
            this.tabPageRGB2D.Controls.Add(this.pictureBoxColor);
            this.tabPageRGB2D.Location = new System.Drawing.Point(4, 19);
            this.tabPageRGB2D.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPageRGB2D.Name = "tabPageRGB2D";
            this.tabPageRGB2D.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPageRGB2D.Size = new System.Drawing.Size(1765, 750);
            this.tabPageRGB2D.TabIndex = 0;
            this.tabPageRGB2D.Text = "Color";
            this.tabPageRGB2D.UseVisualStyleBackColor = true;
            // 
            // pictureBoxColor
            // 
            this.pictureBoxColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.pictureBoxColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxColor.Location = new System.Drawing.Point(4, 4);
            this.pictureBoxColor.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBoxColor.Name = "pictureBoxColor";
            this.pictureBoxColor.Size = new System.Drawing.Size(1757, 742);
            this.pictureBoxColor.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxColor.TabIndex = 1;
            this.pictureBoxColor.TabStop = false;
            // 
            // tabPageIR
            // 
            this.tabPageIR.Controls.Add(this.pictureBoxIR);
            this.tabPageIR.Location = new System.Drawing.Point(4, 19);
            this.tabPageIR.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPageIR.Name = "tabPageIR";
            this.tabPageIR.Size = new System.Drawing.Size(1613, 523);
            this.tabPageIR.TabIndex = 4;
            this.tabPageIR.Text = "Infrared";
            this.tabPageIR.UseVisualStyleBackColor = true;
            // 
            // pictureBoxIR
            // 
            this.pictureBoxIR.BackColor = System.Drawing.Color.Olive;
            this.pictureBoxIR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxIR.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxIR.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBoxIR.Name = "pictureBoxIR";
            this.pictureBoxIR.Size = new System.Drawing.Size(1613, 523);
            this.pictureBoxIR.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxIR.TabIndex = 2;
            this.pictureBoxIR.TabStop = false;
            // 
            // tabPageStatistics
            // 
            this.tabPageStatistics.Controls.Add(this.label9);
            this.tabPageStatistics.Controls.Add(this.pictureBoxEntropy);
            this.tabPageStatistics.Controls.Add(this.label8);
            this.tabPageStatistics.Controls.Add(this.pictureBoxPolygon);
            this.tabPageStatistics.Controls.Add(this.label7);
            this.tabPageStatistics.Controls.Add(this.label5);
            this.tabPageStatistics.Controls.Add(this.label6);
            this.tabPageStatistics.Controls.Add(this.labelDepth1);
            this.tabPageStatistics.Controls.Add(this.labelDepth0);
            this.tabPageStatistics.Controls.Add(this.labelDepth2);
            this.tabPageStatistics.Controls.Add(this.labelDepth4);
            this.tabPageStatistics.Controls.Add(this.labelDepth3);
            this.tabPageStatistics.Location = new System.Drawing.Point(4, 19);
            this.tabPageStatistics.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPageStatistics.Name = "tabPageStatistics";
            this.tabPageStatistics.Size = new System.Drawing.Size(1613, 523);
            this.tabPageStatistics.TabIndex = 2;
            this.tabPageStatistics.Text = "Statistics";
            this.tabPageStatistics.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(203, 290);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(104, 17);
            this.label9.TabIndex = 11;
            this.label9.Text = "2 <= Error <= 2";
            // 
            // pictureBoxEntropy
            // 
            this.pictureBoxEntropy.BackColor = System.Drawing.Color.Yellow;
            this.pictureBoxEntropy.Location = new System.Drawing.Point(713, 34);
            this.pictureBoxEntropy.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBoxEntropy.Name = "pictureBoxEntropy";
            this.pictureBoxEntropy.Size = new System.Drawing.Size(493, 411);
            this.pictureBoxEntropy.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxEntropy.TabIndex = 2;
            this.pictureBoxEntropy.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(203, 310);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(108, 17);
            this.label8.TabIndex = 10;
            this.label8.Text = "3 <= Error <= 6 ";
            // 
            // pictureBoxPolygon
            // 
            this.pictureBoxPolygon.BackColor = System.Drawing.Color.Yellow;
            this.pictureBoxPolygon.Location = new System.Drawing.Point(47, 34);
            this.pictureBoxPolygon.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBoxPolygon.Name = "pictureBoxPolygon";
            this.pictureBoxPolygon.Size = new System.Drawing.Size(267, 176);
            this.pictureBoxPolygon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxPolygon.TabIndex = 1;
            this.pictureBoxPolygon.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(201, 346);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(112, 17);
            this.label7.TabIndex = 9;
            this.label7.Text = "Unknown Depth ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(136, 272);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 17);
            this.label5.TabIndex = 7;
            this.label5.Text = "Error <= 1 mm";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(201, 329);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 17);
            this.label6.TabIndex = 8;
            this.label6.Text = "Error > 7 ";
            // 
            // labelDepth1
            // 
            this.labelDepth1.AutoSize = true;
            this.labelDepth1.Location = new System.Drawing.Point(36, 272);
            this.labelDepth1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDepth1.Name = "labelDepth1";
            this.labelDepth1.Size = new System.Drawing.Size(13, 17);
            this.labelDepth1.TabIndex = 2;
            this.labelDepth1.Text = "-";
            // 
            // labelDepth0
            // 
            this.labelDepth0.AutoSize = true;
            this.labelDepth0.Location = new System.Drawing.Point(160, 346);
            this.labelDepth0.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDepth0.Name = "labelDepth0";
            this.labelDepth0.Size = new System.Drawing.Size(13, 17);
            this.labelDepth0.TabIndex = 3;
            this.labelDepth0.Text = "-";
            // 
            // labelDepth2
            // 
            this.labelDepth2.AutoSize = true;
            this.labelDepth2.Location = new System.Drawing.Point(160, 290);
            this.labelDepth2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDepth2.Name = "labelDepth2";
            this.labelDepth2.Size = new System.Drawing.Size(13, 17);
            this.labelDepth2.TabIndex = 6;
            this.labelDepth2.Text = "-";
            // 
            // labelDepth4
            // 
            this.labelDepth4.AutoSize = true;
            this.labelDepth4.Location = new System.Drawing.Point(160, 329);
            this.labelDepth4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDepth4.Name = "labelDepth4";
            this.labelDepth4.Size = new System.Drawing.Size(13, 17);
            this.labelDepth4.TabIndex = 4;
            this.labelDepth4.Text = "-";
            // 
            // labelDepth3
            // 
            this.labelDepth3.AutoSize = true;
            this.labelDepth3.Location = new System.Drawing.Point(160, 310);
            this.labelDepth3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDepth3.Name = "labelDepth3";
            this.labelDepth3.Size = new System.Drawing.Size(13, 17);
            this.labelDepth3.TabIndex = 5;
            this.labelDepth3.Text = "-";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(1044, 20);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(165, 18);
            this.label12.TabIndex = 9;
            this.label12.Text = "3D Refresh at ... frames";
            // 
            // TrackBarOpenGLAt
            // 
            colorPack76.Border = System.Drawing.Color.DarkRed;
            colorPack76.Face = System.Drawing.Color.IndianRed;
            colorPack76.Highlight = System.Drawing.Color.MistyRose;
            this.TrackBarOpenGLAt.AButColor = colorPack76;
            this.TrackBarOpenGLAt.BackColor = System.Drawing.SystemColors.Control;
            this.TrackBarOpenGLAt.BorderColor = System.Drawing.Color.DarkRed;
            colorPack77.Border = System.Drawing.Color.Black;
            colorPack77.Face = System.Drawing.Color.RoyalBlue;
            colorPack77.Highlight = System.Drawing.Color.White;
            this.TrackBarOpenGLAt.ColorHover = colorPack77;
            colorPack78.Border = System.Drawing.Color.Firebrick;
            colorPack78.Face = System.Drawing.Color.Firebrick;
            colorPack78.Highlight = System.Drawing.Color.White;
            this.TrackBarOpenGLAt.ColorUp = colorPack78;
            this.TrackBarOpenGLAt.FloatValueFontColor = System.Drawing.Color.Red;
            this.TrackBarOpenGLAt.Label = null;
            this.TrackBarOpenGLAt.LabelAlighnment = System.Drawing.StringAlignment.Center;
            this.TrackBarOpenGLAt.Location = new System.Drawing.Point(1109, 42);
            this.TrackBarOpenGLAt.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TrackBarOpenGLAt.MaxValue = 30;
            this.TrackBarOpenGLAt.MinValue = 1;
            this.TrackBarOpenGLAt.Name = "TrackBarOpenGLAt";
            this.TrackBarOpenGLAt.Size = new System.Drawing.Size(293, 48);
            this.TrackBarOpenGLAt.SliderWidthHigh = 1F;
            this.TrackBarOpenGLAt.SliderWidthLow = 1F;
            this.TrackBarOpenGLAt.TabIndex = 8;
            this.TrackBarOpenGLAt.TickColor = System.Drawing.Color.DarkRed;
            this.TrackBarOpenGLAt.TickInterval = 50;
            this.TrackBarOpenGLAt.TickThickness = 1F;
            this.TrackBarOpenGLAt.TickType = VBControls.gTrackBar.eTickType.Middle;
            this.TrackBarOpenGLAt.Value = 5;
            this.TrackBarOpenGLAt.ValueAdjusted = 5F;
            this.TrackBarOpenGLAt.ValueBox = VBControls.gTrackBar.eValueBox.Left;
            this.TrackBarOpenGLAt.ValueBoxBackColor = System.Drawing.Color.IndianRed;
            this.TrackBarOpenGLAt.ValueBoxBorder = System.Drawing.Color.Red;
            this.TrackBarOpenGLAt.ValueBoxSize = new System.Drawing.Size(35, 20);
            this.TrackBarOpenGLAt.ValueDivisor = VBControls.gTrackBar.eValueDivisor.e1;
            this.TrackBarOpenGLAt.ValueStrFormat = null;
            this.TrackBarOpenGLAt.ValueChanged += new VBControls.gTrackBar.ValueChangedEventHandler(this.gTrackBarOpenGLAt_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(545, 65);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(172, 18);
            this.label4.TabIndex = 7;
            this.label4.Text = "Snapshot: No of pictures";
            // 
            // trackBarSnapshotNumber
            // 
            colorPack79.Border = System.Drawing.Color.DarkRed;
            colorPack79.Face = System.Drawing.Color.IndianRed;
            colorPack79.Highlight = System.Drawing.Color.MistyRose;
            this.trackBarSnapshotNumber.AButColor = colorPack79;
            this.trackBarSnapshotNumber.BackColor = System.Drawing.SystemColors.Control;
            this.trackBarSnapshotNumber.BorderColor = System.Drawing.Color.DarkRed;
            colorPack80.Border = System.Drawing.Color.Black;
            colorPack80.Face = System.Drawing.Color.RoyalBlue;
            colorPack80.Highlight = System.Drawing.Color.White;
            this.trackBarSnapshotNumber.ColorHover = colorPack80;
            colorPack81.Border = System.Drawing.Color.Firebrick;
            colorPack81.Face = System.Drawing.Color.Firebrick;
            colorPack81.Highlight = System.Drawing.Color.White;
            this.trackBarSnapshotNumber.ColorUp = colorPack81;
            this.trackBarSnapshotNumber.FloatValueFontColor = System.Drawing.Color.Red;
            this.trackBarSnapshotNumber.Label = null;
            this.trackBarSnapshotNumber.LabelAlighnment = System.Drawing.StringAlignment.Center;
            this.trackBarSnapshotNumber.Location = new System.Drawing.Point(769, 52);
            this.trackBarSnapshotNumber.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.trackBarSnapshotNumber.MaxValue = 20;
            this.trackBarSnapshotNumber.MinValue = 1;
            this.trackBarSnapshotNumber.Name = "trackBarSnapshotNumber";
            this.trackBarSnapshotNumber.Size = new System.Drawing.Size(255, 48);
            this.trackBarSnapshotNumber.SliderWidthHigh = 1F;
            this.trackBarSnapshotNumber.SliderWidthLow = 1F;
            this.trackBarSnapshotNumber.TabIndex = 6;
            this.trackBarSnapshotNumber.TickColor = System.Drawing.Color.DarkRed;
            this.trackBarSnapshotNumber.TickInterval = 1;
            this.trackBarSnapshotNumber.TickThickness = 1F;
            this.trackBarSnapshotNumber.TickType = VBControls.gTrackBar.eTickType.Middle;
            this.trackBarSnapshotNumber.Value = 5;
            this.trackBarSnapshotNumber.ValueAdjusted = 5F;
            this.trackBarSnapshotNumber.ValueBox = VBControls.gTrackBar.eValueBox.Left;
            this.trackBarSnapshotNumber.ValueBoxBackColor = System.Drawing.Color.IndianRed;
            this.trackBarSnapshotNumber.ValueBoxBorder = System.Drawing.Color.Red;
            this.trackBarSnapshotNumber.ValueBoxSize = new System.Drawing.Size(35, 20);
            this.trackBarSnapshotNumber.ValueDivisor = VBControls.gTrackBar.eValueDivisor.e1;
            this.trackBarSnapshotNumber.ValueStrFormat = null;
            this.trackBarSnapshotNumber.ValueChanged += new VBControls.gTrackBar.ValueChangedEventHandler(this.trackBarSnapshotNumber_ValueChanged);
            // 
            // trackBarInterpolationNumber
            // 
            colorPack82.Border = System.Drawing.Color.DarkRed;
            colorPack82.Face = System.Drawing.Color.IndianRed;
            colorPack82.Highlight = System.Drawing.Color.MistyRose;
            this.trackBarInterpolationNumber.AButColor = colorPack82;
            this.trackBarInterpolationNumber.BackColor = System.Drawing.SystemColors.Control;
            this.trackBarInterpolationNumber.BorderColor = System.Drawing.Color.DarkRed;
            colorPack83.Border = System.Drawing.Color.Black;
            colorPack83.Face = System.Drawing.Color.RoyalBlue;
            colorPack83.Highlight = System.Drawing.Color.White;
            this.trackBarInterpolationNumber.ColorHover = colorPack83;
            colorPack84.Border = System.Drawing.Color.Firebrick;
            colorPack84.Face = System.Drawing.Color.Firebrick;
            colorPack84.Highlight = System.Drawing.Color.White;
            this.trackBarInterpolationNumber.ColorUp = colorPack84;
            this.trackBarInterpolationNumber.FloatValueFontColor = System.Drawing.Color.Red;
            this.trackBarInterpolationNumber.Label = null;
            this.trackBarInterpolationNumber.LabelAlighnment = System.Drawing.StringAlignment.Center;
            this.trackBarInterpolationNumber.Location = new System.Drawing.Point(769, 7);
            this.trackBarInterpolationNumber.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.trackBarInterpolationNumber.MaxValue = 30;
            this.trackBarInterpolationNumber.MinValue = 1;
            this.trackBarInterpolationNumber.Name = "trackBarInterpolationNumber";
            this.trackBarInterpolationNumber.Size = new System.Drawing.Size(255, 48);
            this.trackBarInterpolationNumber.SliderWidthHigh = 1F;
            this.trackBarInterpolationNumber.SliderWidthLow = 1F;
            this.trackBarInterpolationNumber.TabIndex = 5;
            this.trackBarInterpolationNumber.TickColor = System.Drawing.Color.DarkRed;
            this.trackBarInterpolationNumber.TickInterval = 2;
            this.trackBarInterpolationNumber.TickOffset = 1;
            this.trackBarInterpolationNumber.TickThickness = 1F;
            this.trackBarInterpolationNumber.TickType = VBControls.gTrackBar.eTickType.Middle;
            this.trackBarInterpolationNumber.Value = 10;
            this.trackBarInterpolationNumber.ValueAdjusted = 10F;
            this.trackBarInterpolationNumber.ValueBox = VBControls.gTrackBar.eValueBox.Left;
            this.trackBarInterpolationNumber.ValueBoxBackColor = System.Drawing.Color.IndianRed;
            this.trackBarInterpolationNumber.ValueBoxBorder = System.Drawing.Color.Red;
            this.trackBarInterpolationNumber.ValueBoxSize = new System.Drawing.Size(35, 20);
            this.trackBarInterpolationNumber.ValueDivisor = VBControls.gTrackBar.eValueDivisor.e1;
            this.trackBarInterpolationNumber.ValueStrFormat = null;
            this.trackBarInterpolationNumber.ValueChanged += new VBControls.gTrackBar.ValueChangedEventHandler(this.trackBarInterpolationNumber_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(545, 20);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(185, 18);
            this.label3.TabIndex = 4;
            this.label3.Text = "Interpolation: No of images";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(4, 65);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "Far Cutoff (mm)";
            // 
            // trackBarCutoffFar
            // 
            colorPack85.Border = System.Drawing.Color.DarkRed;
            colorPack85.Face = System.Drawing.Color.IndianRed;
            colorPack85.Highlight = System.Drawing.Color.MistyRose;
            this.trackBarCutoffFar.AButColor = colorPack85;
            this.trackBarCutoffFar.BackColor = System.Drawing.SystemColors.Control;
            this.trackBarCutoffFar.BorderColor = System.Drawing.Color.DarkRed;
            colorPack86.Border = System.Drawing.Color.Black;
            colorPack86.Face = System.Drawing.Color.RoyalBlue;
            colorPack86.Highlight = System.Drawing.Color.White;
            this.trackBarCutoffFar.ColorHover = colorPack86;
            colorPack87.Border = System.Drawing.Color.Firebrick;
            colorPack87.Face = System.Drawing.Color.Firebrick;
            colorPack87.Highlight = System.Drawing.Color.White;
            this.trackBarCutoffFar.ColorUp = colorPack87;
            this.trackBarCutoffFar.FloatValueFontColor = System.Drawing.Color.Red;
            this.trackBarCutoffFar.Label = null;
            this.trackBarCutoffFar.LabelAlighnment = System.Drawing.StringAlignment.Center;
            this.trackBarCutoffFar.Location = new System.Drawing.Point(164, 52);
            this.trackBarCutoffFar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.trackBarCutoffFar.MaxValue = 7500;
            this.trackBarCutoffFar.MinValue = 500;
            this.trackBarCutoffFar.Name = "trackBarCutoffFar";
            this.trackBarCutoffFar.Size = new System.Drawing.Size(351, 48);
            this.trackBarCutoffFar.SliderWidthHigh = 1F;
            this.trackBarCutoffFar.SliderWidthLow = 1F;
            this.trackBarCutoffFar.TabIndex = 2;
            this.trackBarCutoffFar.TickColor = System.Drawing.Color.DarkRed;
            this.trackBarCutoffFar.TickInterval = 350;
            this.trackBarCutoffFar.TickThickness = 1F;
            this.trackBarCutoffFar.TickType = VBControls.gTrackBar.eTickType.Middle;
            this.trackBarCutoffFar.Value = 1000;
            this.trackBarCutoffFar.ValueAdjusted = 1000F;
            this.trackBarCutoffFar.ValueBox = VBControls.gTrackBar.eValueBox.Left;
            this.trackBarCutoffFar.ValueBoxBackColor = System.Drawing.Color.IndianRed;
            this.trackBarCutoffFar.ValueBoxBorder = System.Drawing.Color.Red;
            this.trackBarCutoffFar.ValueBoxSize = new System.Drawing.Size(35, 20);
            this.trackBarCutoffFar.ValueDivisor = VBControls.gTrackBar.eValueDivisor.e1;
            this.trackBarCutoffFar.ValueStrFormat = null;
            this.trackBarCutoffFar.ValueChanged += new VBControls.gTrackBar.ValueChangedEventHandler(this.trackBarCutoffFar_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "Near Cutoff (mm)";
            // 
            // trackBarCutoffNear
            // 
            colorPack88.Border = System.Drawing.Color.DarkRed;
            colorPack88.Face = System.Drawing.Color.IndianRed;
            colorPack88.Highlight = System.Drawing.Color.MistyRose;
            this.trackBarCutoffNear.AButColor = colorPack88;
            this.trackBarCutoffNear.BackColor = System.Drawing.SystemColors.Control;
            this.trackBarCutoffNear.BorderColor = System.Drawing.Color.DarkRed;
            colorPack89.Border = System.Drawing.Color.Black;
            colorPack89.Face = System.Drawing.Color.RoyalBlue;
            colorPack89.Highlight = System.Drawing.Color.White;
            this.trackBarCutoffNear.ColorHover = colorPack89;
            colorPack90.Border = System.Drawing.Color.Firebrick;
            colorPack90.Face = System.Drawing.Color.Firebrick;
            colorPack90.Highlight = System.Drawing.Color.White;
            this.trackBarCutoffNear.ColorUp = colorPack90;
            this.trackBarCutoffNear.FloatValueFontColor = System.Drawing.Color.Red;
            this.trackBarCutoffNear.Label = null;
            this.trackBarCutoffNear.LabelAlighnment = System.Drawing.StringAlignment.Center;
            this.trackBarCutoffNear.Location = new System.Drawing.Point(164, 2);
            this.trackBarCutoffNear.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.trackBarCutoffNear.MaxValue = 1500;
            this.trackBarCutoffNear.MinValue = 500;
            this.trackBarCutoffNear.Name = "trackBarCutoffNear";
            this.trackBarCutoffNear.Size = new System.Drawing.Size(351, 48);
            this.trackBarCutoffNear.SliderWidthHigh = 1F;
            this.trackBarCutoffNear.SliderWidthLow = 1F;
            this.trackBarCutoffNear.TabIndex = 0;
            this.trackBarCutoffNear.TickColor = System.Drawing.Color.DarkRed;
            this.trackBarCutoffNear.TickInterval = 50;
            this.trackBarCutoffNear.TickThickness = 1F;
            this.trackBarCutoffNear.TickType = VBControls.gTrackBar.eTickType.Middle;
            this.trackBarCutoffNear.Value = 500;
            this.trackBarCutoffNear.ValueAdjusted = 500F;
            this.trackBarCutoffNear.ValueBox = VBControls.gTrackBar.eValueBox.Left;
            this.trackBarCutoffNear.ValueBoxBackColor = System.Drawing.Color.IndianRed;
            this.trackBarCutoffNear.ValueBoxBorder = System.Drawing.Color.Red;
            this.trackBarCutoffNear.ValueBoxSize = new System.Drawing.Size(35, 20);
            this.trackBarCutoffNear.ValueDivisor = VBControls.gTrackBar.eValueDivisor.e1;
            this.trackBarCutoffNear.ValueStrFormat = null;
            this.trackBarCutoffNear.ValueChanged += new VBControls.gTrackBar.ValueChangedEventHandler(this.trackBarCutoffNear_ValueChanged);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(61, 4);
            // 
            // recordToolStripMenuItem
            // 
            this.recordToolStripMenuItem.Name = "recordToolStripMenuItem";
            this.recordToolStripMenuItem.Size = new System.Drawing.Size(288, 26);
            this.recordToolStripMenuItem.Text = "Record skeleton motion as bvh";
            this.recordToolStripMenuItem.Click += new System.EventHandler(this.recordToolStripMenuItem_Click);
            // 
            // ScannerUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerUC);
            this.Controls.Add(this.splitter1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ScannerUC";
            this.Size = new System.Drawing.Size(2070, 928);
            this.splitContainerUC.Panel1.ResumeLayout(false);
            this.splitContainerUC.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerUC)).EndInit();
            this.splitContainerUC.ResumeLayout(false);
            this.splitContainerLeft.Panel1.ResumeLayout(false);
            this.splitContainerLeft.Panel1.PerformLayout();
            this.splitContainerLeft.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLeft)).EndInit();
            this.splitContainerLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSave)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainerRight.Panel1.ResumeLayout(false);
            this.splitContainerRight.Panel2.ResumeLayout(false);
            this.splitContainerRight.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRight)).EndInit();
            this.splitContainerRight.ResumeLayout(false);
            this.tabControlImages.ResumeLayout(false);
            this.tabPage3D.ResumeLayout(false);
            this.tabPageDepth.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDepth)).EndInit();
            this.tabPageRGB2D.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxColor)).EndInit();
            this.tabPageIR.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIR)).EndInit();
            this.tabPageStatistics.ResumeLayout(false);
            this.tabPageStatistics.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEntropy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPolygon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.SplitContainer splitContainerUC;
        private System.Windows.Forms.SplitContainer splitContainerLeft;
        private System.Windows.Forms.SplitContainer splitContainerRight;
        private System.Windows.Forms.Label label2;
        private global::VBControls.gTrackBar trackBarCutoffFar;
        private System.Windows.Forms.Label label1;
        private global::VBControls.gTrackBar trackBarCutoffNear;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private VBControls.gTrackBar trackBarSnapshotNumber;
        private VBControls.gTrackBar trackBarInterpolationNumber;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem1;
        private System.Windows.Forms.Label labelFramesPerSecond;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label labelRefreshRateOpenGL;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TabControl tabControlImages;
        private System.Windows.Forms.TabPage tabPage3D;
        private System.Windows.Forms.TabPage tabPageStatistics;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.PictureBox pictureBoxEntropy;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.PictureBox pictureBoxPolygon;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelDepth1;
        private System.Windows.Forms.Label labelDepth0;
        private System.Windows.Forms.Label labelDepth2;
        private System.Windows.Forms.Label labelDepth4;
        private System.Windows.Forms.Label labelDepth3;
        private System.Windows.Forms.ToolStripMenuItem openGLSettingsToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.TabPage tabPageRGB2D;
        private System.Windows.Forms.PictureBox pictureBoxColor;
        private System.Windows.Forms.TabPage tabPageDepth;

        private System.Windows.Forms.PictureBox pictureBoxDepth;
        private System.Windows.Forms.PictureBox pictureBoxIR;

        private System.Windows.Forms.TabPage tabPageIR;
        private System.Windows.Forms.ToolStripMenuItem cameraConfigToolStripMenuItem;
        private System.Windows.Forms.Label label12;
        private gTrackBar TrackBarOpenGLAt;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolStripMenuItem dToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openPointCloudToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem savePointCloudToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripLineExtract;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem captureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveDialogToolStripMenuItem;
       
        private System.Windows.Forms.Button buttonSavePC;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.NumericUpDown numericUpDownSave;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private OpenTKExtension.OpenGLUC openGLUC;
        private System.Windows.Forms.Button buttonSaveAll;
        private System.Windows.Forms.ToolStripMenuItem recordToolStripMenuItem;
    }
}
