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
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using OpenTK.Extension;

using OpenTK;
using OpenTK.Extension;
namespace OpenTK.Extension
{
    public partial class OpenTKForm : Form
    {

        
        
        public OpenGLUserControl OpenGLUserControl;
        
        public OpenTKForm()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(CultureInfo.CurrentCulture.LCID);
            InitializeComponent();
           
         
            AddOpenGLControl();

            if (!GLSettings.IsInitializedFromSettings)
                GLSettings.InitFromSettings();

            this.Height = GLSettings.Height;
            this.Width = GLSettings.Width;


        }
        protected override void OnLoad(EventArgs e)
        {

            
        }
      
        private void AddOpenGLControl()
        {
            this.OpenGLUserControl = new OpenGLUserControl();
            this.SuspendLayout();
            // 
            // openGLControl1
            // 
            this.OpenGLUserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OpenGLUserControl.Location = new System.Drawing.Point(0, 0);
            this.OpenGLUserControl.Name = "openGLControl1";
            this.OpenGLUserControl.Size = new System.Drawing.Size(854, 453);
            this.OpenGLUserControl.TabIndex = 0;

            panelOpenTK.Controls.Add(this.OpenGLUserControl);
          
            this.ResumeLayout(false);
        }
    
   

        protected override void OnClosed(EventArgs e)
        {
            GlobalVariables.FormFast = null;
            GLSettings.Height = this.Height;
            GLSettings.Width = this.Width;

            GLSettings.SaveSettings();
            base.OnClosed(e);
        }
        //public void AddVerticesAsModel(string name, PointCloud myPCLList)
        //{
        //    this.OpenGLControl.AddVertexListAsModel(name, myPCLList);

        //}
        public void ShowModels()
        {
            this.OpenGLUserControl.ShowModels();
        }
        //public void ShowListOfVertices(PointCloud myPCLList)
        //{
        //    this.OpenGLControl.ShowPointCloud("Point Cloud", myPCLList);

        //}
        //public void ShowModel(ModelOld myModel, bool removeAllOthers)
        //{
        //    if(removeAllOthers)
        //        this.OpenGLControl.RemoveAllModels();
        //    this.OpenGLControl.ShowModel(myModel);

        //}
    
        ///// <summary>
        ///// at least source points should be non zero
        ///// </summary>
        ///// <param name="myPCLTarget"></param>
        ///// <param name="myPCLSource"></param>
        ///// <param name="myPCLResult"></param>
        ///// <param name="changeColor"></param>
        //public void Show3PointClouds(PointCloud myPCLSource, PointCloud myPCLTarget, PointCloud myPCLResult, bool changeColor)
        //{

        //    this.OpenGLControl.RemoveAllModels();

        //    //target in green
        //    List<System.Drawing.Color> myColors;
        //    if (myPCLTarget != null)
        //    {

        //        if (changeColor)
        //        {
        //            myColors = ColorExtensions.CreateColorList(myPCLTarget.Count, 0, 255, 0, 255);
        //            PointCloud.SetColorToList(myPCLTarget, myColors);
        //        }
        //        this.OpenGLControl.ShowPointCloud("ICP Target", myPCLTarget);

        //    }

        //    if (myPCLSource != null)
        //    {
        //        //source in white
        //        myColors = ColorExtensions.CreateColorList(myPCLSource.Count, 255, 255, 255, 255);
        //        if (changeColor)
        //            PointCloud.SetColorToList(myPCLSource, myColors);
        //        this.OpenGLControl.ShowPointCloud("ICP To be matched", myPCLSource);

        //    }

        //    if (myPCLResult != null)
        //    {

        //        //transformed in red
        //        myColors = ColorExtensions.CreateColorList(myPCLResult.Count, 255, 0, 0, 255);
        //        if (changeColor)
        //            PointCloud.SetColorToList(myPCLResult, myColors);
        //        this.OpenGLControl.ShowPointCloud("ICP Solution", myPCLResult);

        //    }

        //}
       
        public void ClearModels()
        {
            OpenGLUserControl.RemoveAllModels();
        }
        //public bool UpdatePointCloud(PointCloud pc)
        //{
        //    //if (this.OpenGLControl.GLrender.Models3D.Count == 0)
        //    //{
        //    //    ShowPointCloud(pc);
        //    //}
        //    //else
        //    //{
        //    //    this.OpenGLControl.RemoveFirstModel(true);
        //    //    Model3D myNewModel = new Model3D();
        //    //    myNewModel.Pointcloud = pc;
        //    //    //this.OpenGLControl.GLrender.Models3D[0].Pointcloud = pc;
        //    //    this.OpenGLControl.GLrender.Models3D.Add(myNewModel);
        //    //    this.OpenGLControl.RedrawAllModels(false);
        //    //    //this.OpenGLControl.Refresh();
        //    //}

        //    ////ClearModels();

            
        //    return true;
        //}
        //public bool UpdateFirstModel(PointCloud pc)
        //{
        //    //ClearModels();


        //    ShowPointCloud(pc);
        //    return true;
        //}

        //public void ShowPointCloud(PointCloud pc)
        //{

        //    this.OpenGLControl.ShowPointCloud("Color Point Cloud", pc);
            
        //}

      
        private void OpenTKForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (GlobalVariables.FormFast != null)
                GlobalVariables.FormFast.Dispose();

            GlobalVariables.FormFast = null;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            this.OpenGLUserControl.OpenGLControl.MouseWheelActions(e);
            base.OnMouseWheel(e);
        }
        
        

       
    }
}
