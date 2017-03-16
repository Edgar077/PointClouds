using OpenTKExtension;

namespace OpenTKExtension
{
    partial class TestForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestForm));
            this.panelOpenTK = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panelOpenTK
            // 
            this.panelOpenTK.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panelOpenTK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOpenTK.Location = new System.Drawing.Point(0, 0);
            this.panelOpenTK.Name = "panelOpenTK";
            this.panelOpenTK.Size = new System.Drawing.Size(1312, 651);
            this.panelOpenTK.TabIndex = 1;
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1312, 651);
            this.Controls.Add(this.panelOpenTK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "TestForm";
            this.Text = "OpenTK Form";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TestForm_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelOpenTK;

       
    }
}