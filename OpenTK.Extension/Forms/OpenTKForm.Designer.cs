using OpenTK.Extension;

namespace OpenTK.Extension
{
    partial class OpenTKForm
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
            // OpenTKForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1312, 651);
            this.Controls.Add(this.panelOpenTK);
            this.KeyPreview = true;
            this.Name = "OpenTKForm";
            this.Text = "OpenTK Form";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OpenTKForm_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelOpenTK;

       
    }
}