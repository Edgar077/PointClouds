/*
 * Created by SharpDevelop.
 * User: Burhan
 * Date: 11/05/2014
 * Time: 01:02 ص
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace VoronoiFortune
{
	partial class FortuneVoronoiUI
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.button1 = new System.Windows.Forms.Button();
			this.pb = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			((System.ComponentModel.ISupportInitialize)(this.pb)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(419, 559);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(105, 27);
			this.button1.TabIndex = 0;
			this.button1.Text = "button1";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.Button1Click);
			// 
			// pb
			// 
			this.pb.Location = new System.Drawing.Point(12, 12);
			this.pb.Name = "pb";
			this.pb.Size = new System.Drawing.Size(512, 512);
			this.pb.TabIndex = 1;
			this.pb.TabStop = false;
			this.pb.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PbMouseMove);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(14, 536);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(122, 17);
			this.label1.TabIndex = 2;
			this.label1.Text = "label1";
			// 
			// richTextBox1
			// 
			this.richTextBox1.Location = new System.Drawing.Point(530, 12);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.Size = new System.Drawing.Size(309, 574);
			this.richTextBox1.TabIndex = 4;
			this.richTextBox1.Text = "";
			this.richTextBox1.WordWrap = false;
			// 
			// numericUpDown1
			// 
			this.numericUpDown1.Location = new System.Drawing.Point(273, 566);
			this.numericUpDown1.Maximum = new decimal(new int[] {
									10000,
									0,
									0,
									0});
			this.numericUpDown1.Minimum = new decimal(new int[] {
									1,
									0,
									0,
									0});
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new System.Drawing.Size(123, 20);
			this.numericUpDown1.TabIndex = 5;
			this.numericUpDown1.Value = new decimal(new int[] {
									30,
									0,
									0,
									0});
			this.numericUpDown1.ValueChanged += new System.EventHandler(this.NumericUpDown1ValueChanged);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(851, 598);
			this.Controls.Add(this.numericUpDown1);
			this.Controls.Add(this.richTextBox1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.pb);
			this.Controls.Add(this.button1);
			this.Name = "MainForm";
			this.Text = "Voronoi";
			((System.ComponentModel.ISupportInitialize)(this.pb)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.NumericUpDown numericUpDown1;
		private System.Windows.Forms.RichTextBox richTextBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.PictureBox pb;
		private System.Windows.Forms.Button button1;
	}
}
