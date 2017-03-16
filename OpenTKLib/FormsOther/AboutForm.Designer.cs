namespace OpenTKExtension
{
    partial class AboutForm
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "OpenTK",
            "www.opentk.com",
            "C# port of OpenGL and additional graphics implementations"}, -1);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "Alglib",
            "www.alglib.net/",
            "Math library for geometry and numerical mathematic calculations"}, -1);
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] {
            "MIConvexHull",
            "miconvexhull.codeplex.com",
            "KDTree, Convex hull, geometric objects, etc. "}, -1);
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new string[] {
            "KDTree code",
            "code.google.com/p/kd-sharp",
            "Fast KD Tree implementation in C#"}, -1);
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem(new string[] {
            "KDTree Original",
            "bitbucket.org/rednaxela/knn-benchmark/src",
            "The original KDTree in Java by Rednaxela"}, -1);
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem(new string[] {
            "KDTree Alternative",
            "github.com/rredford/LdrawToObj/blob/master/LdrawData/KDTree.cs",
            "Alternative KD Tree, used for comparison"}, -1);
            System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem(new string[] {
            "Geometry Library",
            "cs.smith.edu/~orourke/code.html",
            "Geometry routines like convex hull "}, -1);
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem(new string[] {
            "NLinear",
            "nlinear.codeplex.com",
            "Linear mathematics routines"}, -1);
            System.Windows.Forms.ListViewItem listViewItem9 = new System.Windows.Forms.ListViewItem(new string[] {
            "Openexr",
            "www.openexr.com",
            "Math and Matrix utilities   "}, -1);
            System.Windows.Forms.ListViewItem listViewItem10 = new System.Windows.Forms.ListViewItem(new string[] {
            "Track Bar",
            "http://www.codeproject.com/Articles/35104/gTrackBar-A-Custom-TrackBar-UserControl" +
                "-VB-NET",
            "Track bar in VB.NET"}, -1);
            System.Windows.Forms.ListViewItem listViewItem11 = new System.Windows.Forms.ListViewItem(new string[] {
            "Lab3D",
            "cmsoft.com.br",
            "Douglas Andrade, email: cmsoft@cmsoft.com.br, Extended OpenGL Control "}, -1);
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.listBox3 = new System.Windows.Forms.ListBox();
            this.listBox4 = new System.Windows.Forms.ListBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Items.AddRange(new object[] {
            "Pogramming",
            "",
            "     A lot of code is used from freely available source files published by others" +
                ". ",
            "     I have adapted these parts and made them work together.",
            "",
            "     Tried to mention all code sources - in case I\'ve missed something, please no" +
                "tify.",
            "     ",
            "     Edgar Maass, email: 1154-114@onlinehome.de"});
            this.listBox1.Location = new System.Drawing.Point(42, 12);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(485, 95);
            this.listBox1.TabIndex = 25;
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Items.AddRange(new object[] {
            "DISCLAIMER: Users rely upon this software at their own risk, ",
            "and assume the responsibility for the results. Should this ",
            "software or program prove defective, users assume the cost ",
            "of all losses, including, but not limited to, any necessary ",
            "servicing, repair or correction. In no event shall the developers ",
            "or any person be liable for any loss, expense or damage, of",
            " any type or nature arising out of the use of, or inability to ",
            "use this software or program, including, but not limited to, ",
            "claims, suits or causes of action involving alleged infringement ",
            "of copyrights, patents, trademarks, trade secrets, or unfair ",
            "competition. "});
            this.listBox2.Location = new System.Drawing.Point(570, 12);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(358, 173);
            this.listBox2.TabIndex = 26;
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(319, 603);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(176, 23);
            this.buttonOK.TabIndex = 27;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // listBox3
            // 
            this.listBox3.FormattingEnabled = true;
            this.listBox3.Items.AddRange(new object[] {
            "LICENSE:",
            "For the main implementation, see Disclaimer ",
            "",
            "For the other components, like Alglib, etc., different license models apply, ",
            "see the links in  \"Software used\" for more details"});
            this.listBox3.Location = new System.Drawing.Point(42, 113);
            this.listBox3.Name = "listBox3";
            this.listBox3.Size = new System.Drawing.Size(485, 108);
            this.listBox3.TabIndex = 28;
            // 
            // listBox4
            // 
            this.listBox4.FormattingEnabled = true;
            this.listBox4.Items.AddRange(new object[] {
            "OpenTKExtension, Version: 0.9.0.10"});
            this.listBox4.Location = new System.Drawing.Point(319, 553);
            this.listBox4.Name = "listBox4";
            this.listBox4.Size = new System.Drawing.Size(159, 30);
            this.listBox4.TabIndex = 29;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader3,
            this.columnHeader2});
            this.listView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4,
            listViewItem5,
            listViewItem6,
            listViewItem7,
            listViewItem8,
            listViewItem9,
            listViewItem10,
            listViewItem11});
            this.listView1.Location = new System.Drawing.Point(42, 227);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(886, 296);
            this.listView1.TabIndex = 32;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Software used";
            this.columnHeader1.Width = 110;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Link";
            this.columnHeader3.Width = 401;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Description";
            this.columnHeader2.Width = 526;
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(940, 759);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.listBox4);
            this.Controls.Add(this.listBox3);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.listBox1);
            this.Name = "AboutForm";
            this.Text = "About";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.ListBox listBox3;
        private System.Windows.Forms.ListBox listBox4;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;

    }
}