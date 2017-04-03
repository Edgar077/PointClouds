using OpenTKExtension;

namespace OpenTKExtension
{
    partial class MultipleOGLControls
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
            this.openGLUC1 = new OpenTKExtension.OpenGLUC();
            this.SuspendLayout();
            // 
            // openGLUC1
            // 
            this.openGLUC1.Location = new System.Drawing.Point(12, 12);
            this.openGLUC1.Name = "openGLUC1";
            this.openGLUC1.Size = new System.Drawing.Size(703, 589);
            this.openGLUC1.TabIndex = 0;
            // 
            // MultipleOGLControls
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1312, 651);
            this.Controls.Add(this.openGLUC1);
            this.KeyPreview = true;
            this.Name = "MultipleOGLControls";
            this.Text = "OpenTK Form";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TestForm_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private OpenGLUC openGLUC1;





    }
}