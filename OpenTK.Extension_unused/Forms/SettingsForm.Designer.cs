namespace OpenTK.Extension
{
    partial class SettingsForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.checkBoxShowXYZAxes = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.checkBoxShowAxesLabels = new System.Windows.Forms.CheckBox();
            this.checkBoxShowModelAxes = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.checkBoxPointCloudCentered = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonBackColor = new System.Windows.Forms.Button();
            this.buttonColorBackground = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonColorModels = new System.Windows.Forms.Button();
            this.textBoxPointSizeAxis = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxPointSize = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.checkBoxCull = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.checkBoxLighting = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(655, 486);
            this.tabControl1.TabIndex = 47;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.checkBoxShowXYZAxes);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Controls.Add(this.checkBoxShowAxesLabels);
            this.tabPage1.Controls.Add(this.checkBoxShowModelAxes);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.checkBoxPointCloudCentered);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.buttonBackColor);
            this.tabPage1.Controls.Add(this.buttonColorBackground);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.buttonColorModels);
            this.tabPage1.Controls.Add(this.textBoxPointSizeAxis);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.textBoxPointSize);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.buttonOK);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(647, 460);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "View";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // checkBoxShowXYZAxes
            // 
            this.checkBoxShowXYZAxes.AutoSize = true;
            this.checkBoxShowXYZAxes.Checked = true;
            this.checkBoxShowXYZAxes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowXYZAxes.Location = new System.Drawing.Point(188, 293);
            this.checkBoxShowXYZAxes.Name = "checkBoxShowXYZAxes";
            this.checkBoxShowXYZAxes.Size = new System.Drawing.Size(15, 14);
            this.checkBoxShowXYZAxes.TabIndex = 65;
            this.checkBoxShowXYZAxes.UseVisualStyleBackColor = true;
            this.checkBoxShowXYZAxes.CheckedChanged += new System.EventHandler(this.checkBoxShowXYZAxes_CheckedChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(37, 293);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(81, 13);
            this.label9.TabIndex = 64;
            this.label9.Text = "Show xyz Axes ";
            // 
            // checkBoxShowAxesLabels
            // 
            this.checkBoxShowAxesLabels.AutoSize = true;
            this.checkBoxShowAxesLabels.Checked = true;
            this.checkBoxShowAxesLabels.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowAxesLabels.Location = new System.Drawing.Point(188, 242);
            this.checkBoxShowAxesLabels.Name = "checkBoxShowAxesLabels";
            this.checkBoxShowAxesLabels.Size = new System.Drawing.Size(15, 14);
            this.checkBoxShowAxesLabels.TabIndex = 63;
            this.checkBoxShowAxesLabels.UseVisualStyleBackColor = true;
            this.checkBoxShowAxesLabels.CheckedChanged += new System.EventHandler(this.checkBoxShowAxesLabels_CheckedChanged);
            // 
            // checkBoxShowModelAxes
            // 
            this.checkBoxShowModelAxes.AutoSize = true;
            this.checkBoxShowModelAxes.Checked = true;
            this.checkBoxShowModelAxes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowModelAxes.Location = new System.Drawing.Point(188, 218);
            this.checkBoxShowModelAxes.Name = "checkBoxShowModelAxes";
            this.checkBoxShowModelAxes.Size = new System.Drawing.Size(15, 14);
            this.checkBoxShowModelAxes.TabIndex = 62;
            this.checkBoxShowModelAxes.UseVisualStyleBackColor = true;
            this.checkBoxShowModelAxes.CheckedChanged += new System.EventHandler(this.checkBoxShowModelAxes_CheckedChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(38, 242);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(94, 13);
            this.label8.TabIndex = 61;
            this.label8.Text = "Show Axes Labels";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(37, 218);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(92, 13);
            this.label7.TabIndex = 60;
            this.label7.Text = "Show Model Axes";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(37, 193);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(119, 13);
            this.label6.TabIndex = 59;
            this.label6.Text = "Point Cloud is centered ";
            // 
            // checkBoxPointCloudCentered
            // 
            this.checkBoxPointCloudCentered.AutoSize = true;
            this.checkBoxPointCloudCentered.Checked = true;
            this.checkBoxPointCloudCentered.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxPointCloudCentered.Location = new System.Drawing.Point(188, 189);
            this.checkBoxPointCloudCentered.Name = "checkBoxPointCloudCentered";
            this.checkBoxPointCloudCentered.Size = new System.Drawing.Size(15, 14);
            this.checkBoxPointCloudCentered.TabIndex = 58;
            this.checkBoxPointCloudCentered.UseVisualStyleBackColor = true;
            this.checkBoxPointCloudCentered.CheckedChanged += new System.EventHandler(this.checkBoxPointCloudCentered_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(37, 148);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 13);
            this.label5.TabIndex = 57;
            this.label5.Text = "Color of background";
            // 
            // buttonBackColor
            // 
            this.buttonBackColor.Location = new System.Drawing.Point(188, 143);
            this.buttonBackColor.Name = "buttonBackColor";
            this.buttonBackColor.Size = new System.Drawing.Size(123, 23);
            this.buttonBackColor.TabIndex = 56;
            this.buttonBackColor.Text = "Change";
            this.buttonBackColor.UseVisualStyleBackColor = true;
            this.buttonBackColor.Click += new System.EventHandler(this.buttonBackColor_Click);
            // 
            // buttonColorBackground
            // 
            this.buttonColorBackground.Location = new System.Drawing.Point(188, 114);
            this.buttonColorBackground.Name = "buttonColorBackground";
            this.buttonColorBackground.Size = new System.Drawing.Size(123, 23);
            this.buttonColorBackground.TabIndex = 55;
            this.buttonColorBackground.Text = "Change";
            this.buttonColorBackground.UseVisualStyleBackColor = true;
            this.buttonColorBackground.Click += new System.EventHandler(this.buttonColorBackground_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(37, 114);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(110, 13);
            this.label4.TabIndex = 54;
            this.label4.Text = "Color of current model";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 13);
            this.label3.TabIndex = 53;
            this.label3.Text = "Color of all Models";
            // 
            // buttonColorModels
            // 
            this.buttonColorModels.Location = new System.Drawing.Point(188, 81);
            this.buttonColorModels.Name = "buttonColorModels";
            this.buttonColorModels.Size = new System.Drawing.Size(123, 23);
            this.buttonColorModels.TabIndex = 52;
            this.buttonColorModels.Text = "Change";
            this.buttonColorModels.UseVisualStyleBackColor = true;
            this.buttonColorModels.Click += new System.EventHandler(this.buttonColorModels_Click);
            // 
            // textBoxPointSizeAxis
            // 
            this.textBoxPointSizeAxis.Location = new System.Drawing.Point(188, 53);
            this.textBoxPointSizeAxis.Name = "textBoxPointSizeAxis";
            this.textBoxPointSizeAxis.Size = new System.Drawing.Size(100, 20);
            this.textBoxPointSizeAxis.TabIndex = 51;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 50;
            this.label2.Text = "Point Size for axis";
            // 
            // textBoxPointSize
            // 
            this.textBoxPointSize.Location = new System.Drawing.Point(188, 20);
            this.textBoxPointSize.Name = "textBoxPointSize";
            this.textBoxPointSize.Size = new System.Drawing.Size(100, 20);
            this.textBoxPointSize.TabIndex = 49;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 13);
            this.label1.TabIndex = 48;
            this.label1.Text = "Point Size for point cloud";
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(177, 324);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(176, 23);
            this.buttonOK.TabIndex = 47;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label11);
            this.tabPage2.Controls.Add(this.checkBoxLighting);
            this.tabPage2.Controls.Add(this.button1);
            this.tabPage2.Controls.Add(this.label10);
            this.tabPage2.Controls.Add(this.checkBoxCull);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(647, 460);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "OpenGL Settings";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(234, 397);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(176, 23);
            this.button1.TabIndex = 62;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(35, 41);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(38, 13);
            this.label10.TabIndex = 61;
            this.label10.Text = "Culling";
            // 
            // checkBoxCull
            // 
            this.checkBoxCull.AutoSize = true;
            this.checkBoxCull.Checked = true;
            this.checkBoxCull.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCull.Location = new System.Drawing.Point(186, 37);
            this.checkBoxCull.Name = "checkBoxCull";
            this.checkBoxCull.Size = new System.Drawing.Size(15, 14);
            this.checkBoxCull.TabIndex = 60;
            this.checkBoxCull.UseVisualStyleBackColor = true;
            this.checkBoxCull.CheckedChanged += new System.EventHandler(this.checkBoxCull_CheckedChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(35, 68);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(44, 13);
            this.label11.TabIndex = 64;
            this.label11.Text = "Lighting";
            // 
            // checkBoxLighting
            // 
            this.checkBoxLighting.AutoSize = true;
            this.checkBoxLighting.Checked = true;
            this.checkBoxLighting.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxLighting.Location = new System.Drawing.Point(186, 64);
            this.checkBoxLighting.Name = "checkBoxLighting";
            this.checkBoxLighting.Size = new System.Drawing.Size(15, 14);
            this.checkBoxLighting.TabIndex = 63;
            this.checkBoxLighting.UseVisualStyleBackColor = true;
            this.checkBoxLighting.CheckedChanged += new System.EventHandler(this.checkBoxLighting_CheckedChanged);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(655, 486);
            this.Controls.Add(this.tabControl1);
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.CheckBox checkBoxShowXYZAxes;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox checkBoxShowAxesLabels;
        private System.Windows.Forms.CheckBox checkBoxShowModelAxes;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox checkBoxPointCloudCentered;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonBackColor;
        private System.Windows.Forms.Button buttonColorBackground;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonColorModels;
        private System.Windows.Forms.TextBox textBoxPointSizeAxis;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxPointSize;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox checkBoxCull;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox checkBoxLighting;


    }
}