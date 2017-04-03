using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;


namespace OpenTKExtension.FastGLControl
{
    public partial class TestFormAlternative : Form
    {

        public TestFormAlternative()
        {
           
            InitializeComponent();
            InitComboBox();

            if (!GLSettings.IsInitializedFromSettings)
                GLSettings.InitFromSettings();

            this.Height = GLSettings.Height;
            this.Width = GLSettings.Width;



        }
        public void ReplaceRenderableObject(RenderableObject pc)
        {
           
            this.glControl.GLrender.ReplaceRenderableObject(pc, true);


        }
        public void AddRenderableObject(RenderableObject pc)
        {

            this.glControl.GLrender.AddRenderableObject(pc);


        }
        public void AddModel(Model myModel)
        {

            this.glControl.GLrender.AddModel(myModel);


        }
        private void AddMenus()
        {
            


        }
        private void InitComboBox()
        {
           
            int iPrev = -1;
            for (int i = 0; i < Enum.GetValues(typeof(PrimitiveType)).GetLength(0); i++)
            {
                string strVal = Enum.GetValues(typeof(PrimitiveType)).GetValue(i).ToString();
                int iVal = (int)Enum.GetValues(typeof(PrimitiveType)).GetValue(i);
                if(iVal != iPrev) 
                    this.comboRenderMode.Items.Add(strVal);
                iPrev = iVal;
            }

            for (int i = 0; i < Enum.GetValues(typeof(PolygonMode)).GetLength(0); i++)
            {
                string strVal = Enum.GetValues(typeof(PolygonMode)).GetValue(i).ToString();
                int iVal = (int)Enum.GetValues(typeof(PolygonMode)).GetValue(i);
                if(iVal != iPrev) 
                    this.comboFill.Items.Add(strVal);
                iPrev = iVal;
            }
        
        }

       

        protected override void OnClosed(EventArgs e)
        {
            
            GLSettings.Height = this.Height;
            GLSettings.Width = this.Width;
            GLSettings.SaveSettings();

            base.OnClosed(e);
        }
    
     
        private static void ShowGLVersionAndString()
        {
          
            System.Diagnostics.Debug.WriteLine("Version, renderer: " + GL.GetString(StringName.Version) + " : " + GL.GetString(StringName.Renderer));
        }

        private void showTriangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CubeLines render = new CubeLines();
            render.InitializeGL();
            //CubePerspective tr = new CubePerspective();

            //Triangle tr = new Triangle();
            ReplaceRenderableObject(render);

        }

        private void animateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.glControl.StartAnimationTimer();
        }

        private void showCubeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CubeLines render = new CubeLines();
            render.InitializeGL();
           
            ReplaceRenderableObject(render);

        }

      

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "TestData";
            Model myModel = new Model(path + "\\Bunny.obj");

            PointCloud pgl = myModel.PointCloud;
            PointCloudRenderable pcr = new PointCloudRenderable();
            pcr.PointCloud = pgl;

            ReplaceRenderableObject(pcr);
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            string strDisplay = Enum.GetValues(typeof(PrimitiveType)).GetValue(comboRenderMode.SelectedIndex).ToString();

            GLSettings.ViewMode = strDisplay;

            this.glControl.GLrender.PrimitiveTypes = PrimitiveType.Points;
            for (int i = 0; i < Enum.GetValues(typeof(PrimitiveType)).GetLength(0); i++)
            {
                string strVal = Enum.GetValues(typeof(PrimitiveType)).GetValue(i).ToString();
                if (strVal == strDisplay)
                {
                    this.glControl.GLrender.PrimitiveTypes = (PrimitiveType)Enum.GetValues(typeof(PrimitiveType)).GetValue(i);
                    break;
                }

            }
            this.glControl.Invalidate();

        }


        private void comboFill_SelectedIndexChanged(object sender, EventArgs e)
        {

            string strDisplay = Enum.GetValues(typeof(PolygonMode)).GetValue(comboFill.SelectedIndex).ToString();

            GLSettings.ViewMode = strDisplay;

            this.glControl.GLrender.PoygonModes = PolygonMode.Point;
            for (int i = 0; i < Enum.GetValues(typeof(PolygonMode)).GetLength(0); i++)
            {
                string strVal = Enum.GetValues(typeof(PolygonMode)).GetValue(i).ToString();
                if (strVal == strDisplay)
                {
                    this.glControl.GLrender.PoygonModes = (PolygonMode)Enum.GetValues(typeof(PolygonMode)).GetValue(i);
                    break;
                }

            }
            this.glControl.Invalidate();

        }

       
        private void openToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            SettingsForm sf = new SettingsForm(this.glControl);
            sf.ShowDialog();
            this.glControl.GLrender.ResetPointLineSizes();

        }

        private void openToolStripMenuItem5_Click(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem6_Click(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem7_Click(object sender, EventArgs e)
        {

        }
    }
}
