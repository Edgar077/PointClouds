namespace CharacterCreator
{
    partial class CharacterCreatorTest
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.buttonLoadFace = new System.Windows.Forms.Button();
            this.buttonShowSkeletonAdjustModel = new System.Windows.Forms.Button();
            this.buttonShowSkeleton = new System.Windows.Forms.Button();
            this.buttonChangeHead = new System.Windows.Forms.Button();
            this.buttonTriangulate = new System.Windows.Forms.Button();
            this.buttonCutHead = new System.Windows.Forms.Button();
            this.buttonCustom = new System.Windows.Forms.Button();
            this.comboBoxPose = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownHeight = new System.Windows.Forms.NumericUpDown();
            this.buttonReload = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.openGLUC1 = new OpenTKExtension.OpenGLUC();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHeight)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.openGLUC1);
            this.splitContainer1.Size = new System.Drawing.Size(1547, 622);
            this.splitContainer1.SplitterDistance = 308;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(308, 622);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.buttonLoadFace);
            this.tabPage1.Controls.Add(this.buttonShowSkeletonAdjustModel);
            this.tabPage1.Controls.Add(this.buttonShowSkeleton);
            this.tabPage1.Controls.Add(this.buttonChangeHead);
            this.tabPage1.Controls.Add(this.buttonTriangulate);
            this.tabPage1.Controls.Add(this.buttonCutHead);
            this.tabPage1.Controls.Add(this.buttonCustom);
            this.tabPage1.Controls.Add(this.comboBoxPose);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.numericUpDownHeight);
            this.tabPage1.Controls.Add(this.buttonReload);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage1.Size = new System.Drawing.Size(300, 593);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // buttonLoadFace
            // 
            this.buttonLoadFace.Location = new System.Drawing.Point(20, 321);
            this.buttonLoadFace.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonLoadFace.Name = "buttonLoadFace";
            this.buttonLoadFace.Size = new System.Drawing.Size(156, 28);
            this.buttonLoadFace.TabIndex = 12;
            this.buttonLoadFace.Text = "Load Face";
            this.buttonLoadFace.UseVisualStyleBackColor = true;
            this.buttonLoadFace.Click += new System.EventHandler(this.buttonLoadFace_Click);
            // 
            // buttonShowSkeletonAdjustModel
            // 
            this.buttonShowSkeletonAdjustModel.Location = new System.Drawing.Point(155, 20);
            this.buttonShowSkeletonAdjustModel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonShowSkeletonAdjustModel.Name = "buttonShowSkeletonAdjustModel";
            this.buttonShowSkeletonAdjustModel.Size = new System.Drawing.Size(135, 52);
            this.buttonShowSkeletonAdjustModel.TabIndex = 11;
            this.buttonShowSkeletonAdjustModel.Text = "Load skeleton + adjust";
            this.buttonShowSkeletonAdjustModel.UseVisualStyleBackColor = true;
            this.buttonShowSkeletonAdjustModel.Click += new System.EventHandler(this.buttonShowSkeletonAdjustModel_Click);
            // 
            // buttonShowSkeleton
            // 
            this.buttonShowSkeleton.Location = new System.Drawing.Point(20, 357);
            this.buttonShowSkeleton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonShowSkeleton.Name = "buttonShowSkeleton";
            this.buttonShowSkeleton.Size = new System.Drawing.Size(135, 28);
            this.buttonShowSkeleton.TabIndex = 10;
            this.buttonShowSkeleton.Text = "Load Skeleton";
            this.buttonShowSkeleton.UseVisualStyleBackColor = true;
            this.buttonShowSkeleton.Click += new System.EventHandler(this.buttonShowSkeleton_Click);
            // 
            // buttonChangeHead
            // 
            this.buttonChangeHead.Location = new System.Drawing.Point(20, 393);
            this.buttonChangeHead.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonChangeHead.Name = "buttonChangeHead";
            this.buttonChangeHead.Size = new System.Drawing.Size(156, 28);
            this.buttonChangeHead.TabIndex = 9;
            this.buttonChangeHead.Text = "Update Model";
            this.buttonChangeHead.UseVisualStyleBackColor = true;
            this.buttonChangeHead.Click += new System.EventHandler(this.buttonUpdateModel_Click);
            // 
            // buttonTriangulate
            // 
            this.buttonTriangulate.Location = new System.Drawing.Point(49, 551);
            this.buttonTriangulate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonTriangulate.Name = "buttonTriangulate";
            this.buttonTriangulate.Size = new System.Drawing.Size(156, 28);
            this.buttonTriangulate.TabIndex = 6;
            this.buttonTriangulate.Text = "Triangulate";
            this.buttonTriangulate.UseVisualStyleBackColor = true;
            this.buttonTriangulate.Click += new System.EventHandler(this.buttonTriangulate_Click);
            // 
            // buttonCutHead
            // 
            this.buttonCutHead.Location = new System.Drawing.Point(20, 428);
            this.buttonCutHead.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonCutHead.Name = "buttonCutHead";
            this.buttonCutHead.Size = new System.Drawing.Size(156, 28);
            this.buttonCutHead.TabIndex = 5;
            this.buttonCutHead.Text = "Cut Head";
            this.buttonCutHead.UseVisualStyleBackColor = true;
            this.buttonCutHead.Click += new System.EventHandler(this.buttonDeleteFace_Click);
            // 
            // buttonCustom
            // 
            this.buttonCustom.Location = new System.Drawing.Point(49, 500);
            this.buttonCustom.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonCustom.Name = "buttonCustom";
            this.buttonCustom.Size = new System.Drawing.Size(156, 28);
            this.buttonCustom.TabIndex = 4;
            this.buttonCustom.Text = "Load from File";
            this.buttonCustom.UseVisualStyleBackColor = true;
            // 
            // comboBoxPose
            // 
            this.comboBoxPose.FormattingEnabled = true;
            this.comboBoxPose.Location = new System.Drawing.Point(128, 203);
            this.comboBoxPose.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboBoxPose.Name = "comboBoxPose";
            this.comboBoxPose.Size = new System.Drawing.Size(160, 24);
            this.comboBoxPose.TabIndex = 3;
            this.comboBoxPose.SelectedIndexChanged += new System.EventHandler(this.comboBoxPose_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 135);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Height (cm)";
            // 
            // numericUpDownHeight
            // 
            this.numericUpDownHeight.Location = new System.Drawing.Point(205, 133);
            this.numericUpDownHeight.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numericUpDownHeight.Maximum = new decimal(new int[] {
            225,
            0,
            0,
            0});
            this.numericUpDownHeight.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDownHeight.Name = "numericUpDownHeight";
            this.numericUpDownHeight.Size = new System.Drawing.Size(84, 22);
            this.numericUpDownHeight.TabIndex = 1;
            this.numericUpDownHeight.Value = new decimal(new int[] {
            167,
            0,
            0,
            0});
            // 
            // buttonReload
            // 
            this.buttonReload.Location = new System.Drawing.Point(0, 54);
            this.buttonReload.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonReload.Name = "buttonReload";
            this.buttonReload.Size = new System.Drawing.Size(100, 28);
            this.buttonReload.TabIndex = 0;
            this.buttonReload.Text = "Reload";
            this.buttonReload.UseVisualStyleBackColor = true;
            this.buttonReload.Click += new System.EventHandler(this.buttonReload_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage2.Size = new System.Drawing.Size(300, 593);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // openGLUC1
            // 
            this.openGLUC1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.openGLUC1.Location = new System.Drawing.Point(0, 0);
            this.openGLUC1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.openGLUC1.Name = "openGLUC1";
            this.openGLUC1.Size = new System.Drawing.Size(1234, 622);
            this.openGLUC1.TabIndex = 0;
            // 
            // CharacterCreatorTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1547, 622);
            this.Controls.Add(this.splitContainer1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "CharacterCreatorTest";
            this.Text = "Character Creator Test";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHeight)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button buttonReload;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDownHeight;
        private System.Windows.Forms.ComboBox comboBoxPose;
        private System.Windows.Forms.Button buttonCustom;
        private System.Windows.Forms.Button buttonCutHead;
        private System.Windows.Forms.Button buttonTriangulate;
        private System.Windows.Forms.Button buttonChangeHead;
        private System.Windows.Forms.Button buttonShowSkeleton;
        private System.Windows.Forms.Button buttonShowSkeletonAdjustModel;
        private OpenTKExtension.OpenGLUC openGLUC1;
        private System.Windows.Forms.Button buttonLoadFace;
    }
}