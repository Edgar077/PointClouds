namespace PointCloudScanner
{
    partial class SaveDialog
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
            this.buttonSavePointCloud = new System.Windows.Forms.Button();
            this.buttonTest = new System.Windows.Forms.Button();
            this.buttonSaveNSnapshots = new System.Windows.Forms.Button();
            this.buttonShowColorizedPointCloud = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonOpenSaved = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonSavePointCloud
            // 
            this.buttonSavePointCloud.Location = new System.Drawing.Point(76, 268);
            this.buttonSavePointCloud.Name = "buttonSavePointCloud";
            this.buttonSavePointCloud.Size = new System.Drawing.Size(105, 23);
            this.buttonSavePointCloud.TabIndex = 16;
            this.buttonSavePointCloud.Text = "Save Point Cloud";
            this.buttonSavePointCloud.UseVisualStyleBackColor = true;
            this.buttonSavePointCloud.Click += new System.EventHandler(this.buttonSavePointCloud_Click);
            // 
            // buttonTest
            // 
            this.buttonTest.Location = new System.Drawing.Point(222, 199);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(75, 23);
            this.buttonTest.TabIndex = 15;
            this.buttonTest.Text = "Test Camera";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // buttonSaveNSnapshots
            // 
            this.buttonSaveNSnapshots.Location = new System.Drawing.Point(222, 143);
            this.buttonSaveNSnapshots.Name = "buttonSaveNSnapshots";
            this.buttonSaveNSnapshots.Size = new System.Drawing.Size(123, 23);
            this.buttonSaveNSnapshots.TabIndex = 14;
            this.buttonSaveNSnapshots.Text = "Save more snapshots";
            this.buttonSaveNSnapshots.UseVisualStyleBackColor = true;
            this.buttonSaveNSnapshots.Click += new System.EventHandler(this.buttonSaveNSnapshots_Click);
            // 
            // buttonShowColorizedPointCloud
            // 
            this.buttonShowColorizedPointCloud.Location = new System.Drawing.Point(267, 30);
            this.buttonShowColorizedPointCloud.Name = "buttonShowColorizedPointCloud";
            this.buttonShowColorizedPointCloud.Size = new System.Drawing.Size(173, 36);
            this.buttonShowColorizedPointCloud.TabIndex = 13;
            this.buttonShowColorizedPointCloud.Text = "Show current scan - new Window";
            this.buttonShowColorizedPointCloud.UseVisualStyleBackColor = true;
            this.buttonShowColorizedPointCloud.Click += new System.EventHandler(this.buttonShowColorizedPointCloud_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(76, 30);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(105, 23);
            this.buttonSave.TabIndex = 11;
            this.buttonSave.Text = "&Save All (Enter)";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonOpenSaved
            // 
            this.buttonOpenSaved.Location = new System.Drawing.Point(222, 91);
            this.buttonOpenSaved.Name = "buttonOpenSaved";
            this.buttonOpenSaved.Size = new System.Drawing.Size(113, 23);
            this.buttonOpenSaved.TabIndex = 12;
            this.buttonOpenSaved.Text = "Show last saved depth";
            this.buttonOpenSaved.UseVisualStyleBackColor = true;
            this.buttonOpenSaved.Click += new System.EventHandler(this.buttonOpenSaved_Click);
            // 
            // SaveDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 347);
            this.Controls.Add(this.buttonSavePointCloud);
            this.Controls.Add(this.buttonTest);
            this.Controls.Add(this.buttonSaveNSnapshots);
            this.Controls.Add(this.buttonShowColorizedPointCloud);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonOpenSaved);
            this.Name = "SaveDialog";
            this.Text = "SaveDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonSavePointCloud;
        private System.Windows.Forms.Button buttonTest;
        private System.Windows.Forms.Button buttonSaveNSnapshots;
        private System.Windows.Forms.Button buttonShowColorizedPointCloud;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonOpenSaved;
    }
}