// Pogramming by
//     Douglas Andrade ( http://www.cmsoft.com.br, email: cmsoft@cmsoft.com.br)
//               Implementation of most of the functionality
//     Edgar Maass: (email: maass@logisel.de)
//               Code adaption, changed to user control
//
//Software used: 
//    OpenGL : http://www.opengl.org
//    OpenTK : http://www.opentk.com
//
// DISCLAIMER: Users rely upon this software at their own risk, and assume the responsibility for the results. Should this software or program prove defective, 
// users assume the cost of all losses, including, but not limited to, any necessary servicing, repair or correction. In no event shall the developers or any person 
// be liable for any loss, expense or damage, of any type or nature arising out of the use of, or inability to use this software or program, including, but not
// limited to, claims, suits or causes of action involving alleged infringement of copyrights, patents, trademarks, trade secrets, or unfair competition. 
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;



namespace OpenTK.Extension
{
    public partial class OpenGLUserControl : System.Windows.Forms.UserControl
    {
       
        public OpenGLUserControl()
        {
            InitializeComponent();
            if (!GLSettings.IsInitializedFromSettings)
                GLSettings.InitFromSettings();

            
            InitComboBox();
            comboRenderMode.SelectedText = GLSettings.ViewMode;
            

            this.initGLControl();

            //to make change e.g. back color
            this.glControl1.Invalidate();
        }
        private void InitComboBox()
        {
            comboBoxFill.Items.Add("Points/Lines");
            comboBoxFill.Items.Add("Fill");
            if (GLSettings.Fill)
                comboBoxFill.SelectedIndex = 1;
            else
            {
                comboBoxFill.SelectedIndex = 0;
            }


            int iPrev = -1;
            for (int i = 0; i < Enum.GetValues(typeof(PrimitiveType)).GetLength(0); i++)
            {
                string strVal = Enum.GetValues(typeof(PrimitiveType)).GetValue(i).ToString();
                int iVal = (int)Enum.GetValues(typeof(PrimitiveType)).GetValue(i);
                if (iVal != iPrev)
                    this.comboRenderMode.Items.Add(strVal);
                iPrev = iVal;
            }
        }


        private void TestCube()
        {
            Cube rc = new Cube();
            rc.FillPointCloud();

            Model myModel = new Model(rc);

            this.OpenGLControl.openGLContext.AddModel(myModel);

        }

     
        private void loadModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadFileDialog();
        }

     

        

        private void comboViewMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strDisplay = Enum.GetValues(typeof(PrimitiveType)).GetValue(comboRenderMode.SelectedIndex).ToString();

            GLSettings.ViewMode = strDisplay;

            this.OpenGLControl.openGLContext.RenderMode = PrimitiveType.Points;
            for (int i = 0; i < Enum.GetValues(typeof(PrimitiveType)).GetLength(0); i++)
            {
                string strVal = Enum.GetValues(typeof(PrimitiveType)).GetValue(i).ToString();
                if (strVal == strDisplay)
                {
                    this.OpenGLControl.openGLContext.RenderMode = (PrimitiveType)Enum.GetValues(typeof(PrimitiveType)).GetValue(i);
                    break;
                }

            }
            this.OpenGLControl.Invalidate();

        }
      
        private void toolStripButtonColor_Click(object sender, EventArgs e)
        {
            ChangeModelColor();

   
        }
        private void SetColorOfSelectedModel(byte R, byte G, byte B, byte A)
        {
          

            //int indexModel = comboModels.SelectedIndex;
            //Model3D myModel = GLrender.Models3D[indexModel];
         
            //Color newColor = System.Drawing.Color.FromArgb(A, R, G, B);
            
                
            //SetModelColor(myModel, newColor);

        }
    

     
       
      
        private void RefreshView_MakeCurrent()
        {
            //this.glControl1.MakeCurrent();

            //this.GLrender.Draw();
            //this.glControl1.Refresh();


        }
        private void RefreshView(bool forceRedraw)
        {
            //for (int i = 0; i < GLrender.Models3D.Count; i++)
            //{
            //    GLrender.Models3D[i].ForceRedraw = true;
                
            //}
            //this.glControl1.Invalidate();
            
           
        }
        private void comboTransparency_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            //if (comboModels.SelectedIndex >= 0)
            //{
            //    float transp;
            //    float.TryParse(comboTransparency.Text, out transp);
            //    transp *= 0.01f;
            //    float alpha = 1 - transp;
            //    for (int i = 0; i < GLrender.Models3D[comboModels.SelectedIndex].Parts.Count; i++)
            //    {
            //        PointCloud.ChangeTransparency(GLrender.Models3D[comboModels.SelectedIndex].Pointcloud, alpha);
                   
            //    }
            //}

            //RefreshView(true);

        }

       
        public void SaveSettings()
        {
            GLSettings.SaveSettings();

        }

    
       

        private void showAxesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (showAxesToolStripMenuItem.Text == "Show Axis")
            {
                GLSettings.ShowAxes = true;
                showAxesToolStripMenuItem.Text = "Hide Axis";
                glControl1.Refresh();
            }
            else if (showAxesToolStripMenuItem.Text == "Hide Axis")
            {
                GLSettings.ShowAxes = false;
                showAxesToolStripMenuItem.Text = "Show Axis";
                glControl1.Refresh();
            }
        }

    
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm sf = new AboutForm();
            sf.ShowDialog();

        }

     


        private void convexHullToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //int indexModel = comboModels.SelectedIndex;
            //Model3D myModel = GLrender.Models3D[comboModels.SelectedIndex];



            //List<Vector3> myListVectors = PointCloud.ToVectors(myModel.Pointcloud);
            //ConvexHull3D convHull = new ConvexHull3D(myListVectors);

        }

        private void triangulateNearestNeighbourToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveNormalsOfCurrentModelToolStripMenuItem_Click(object sender, EventArgs e)
        {


            //Model3D myModel = GLrender.Models3D[comboModels.SelectedIndex];
            //CheckNormals(myModel);
            //string path = AppDomain.CurrentDomain.BaseDirectory + "TestData";

                      
            //Model3D.Save_OBJ(myModel, path, myModel.Name + ".obj");

        }
   
        private void showNormalsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (showNormalsToolStripMenuItem.Text == "Show Normals")
            //{
            //    GLSettings.ShowNormals = true;
            //    showNormalsToolStripMenuItem.Text = "Hide Normals";

            //    UpdataNormals();

            //    RefreshView(true);

            //}
            //else if (showNormalsToolStripMenuItem.Text == "Hide Normals")
            //{
            //    GLSettings.ShowNormals = false;
            //    showNormalsToolStripMenuItem.Text = "Show Normals";
            //    for (int i = 0; i < GLrender.Models3D.Count; i++ )
            //    {
            //        Model3D myModel = GLrender.Models3D[i];
            //        myModel.Pointcloud.LinesNormals = null;
            //    }
                
            //    RefreshView(true);
                
            //}

            

        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsForm sf = new SettingsForm(this);
            if (sf.ShowDialog() == DialogResult.OK)
            {
                this.OpenGLControl.openGLContext.Draw();
                //RefreshView(true);
                
            }
        }
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            this.glControl1.MouseWheelActions(e);
            base.OnMouseWheel(e);
        }
     
        public OpenGLControl OpenGLControl
        {
            get
            {
                return this.glControl1;
            }
        }

        private void comboBoxFill_SelectedIndexChanged(object sender, EventArgs e)
        {
           // this.OpenGLControl.GLrender.FillMode = 
            if (comboBoxFill.Text == "Fill")
            {
                this.OpenGLControl.openGLContext.FillMode = OpenTK.Graphics.OpenGL.PolygonMode.Fill;
                GLSettings.Fill = true;
               
            }
            else
            {
                this.OpenGLControl.openGLContext.FillMode = OpenTK.Graphics.OpenGL.PolygonMode.Line;
                GLSettings.Fill = false;
            }
            this.glControl1.Refresh();
        }

        private void removeSelectedModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.glControl1.openGLContext.ClearAllObjects();
            this.Refresh();
        }

        private void showGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (showGridToolStripMenuItem.Text == "Show Grid")
            {
                GLSettings.ShowGrid = true;
                showGridToolStripMenuItem.Text = "Hide Grid";
                glControl1.Refresh();
            }
            else if (showGridToolStripMenuItem.Text == "Hide Grid")
            {
                GLSettings.ShowGrid = false;
                showGridToolStripMenuItem.Text = "Show Grid";
                glControl1.Refresh();
            }
        }

        private void cubeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.glControl1.openGLContext.ClearAllObjects();
            TestCube();
            this.Refresh();
        }

        private void pointCloudToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadModelFromFile("Models\\1.obj");
        }

        private void bunnyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadModelFromFile("TestData\\bunny.obj");
        }

        //private void checkBoxFill_CheckedChanged(object sender, EventArgs e)
        //{



        //}
    }
}
