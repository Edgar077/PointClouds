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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using OpenTKExtension;
using System.Windows.Media.Media3D;
using OpenTK;

namespace OpenTKExtension
{
    public partial class TestForm : Form
    {

        public OpenGLUC OpenGL_UControl;

        public TestForm()
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
            base.OnLoad(e);
        }
        private void AddOpenGLControl()
        {
            
            this.SuspendLayout();
            this.OpenGL_UControl = new OpenGLUC();

            // 
            // openGLControl1
            // 
            this.OpenGL_UControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OpenGL_UControl.Location = new System.Drawing.Point(0, 0);
            this.OpenGL_UControl.Name = "openGLControl1";
            this.OpenGL_UControl.Size = new System.Drawing.Size(854, 453);
            this.OpenGL_UControl.TabIndex = 0;

            panelOpenTK.Controls.Add(this.OpenGL_UControl);
          
            this.ResumeLayout(false);
        }
    
   

        protected override void OnClosed(EventArgs e)
        {
           
            GLSettings.Height = this.Height;
            GLSettings.Width = this.Width;

            GLSettings.SaveSettings();
            base.OnClosed(e);
        }
        public void AddVerticesAsModel(string name, PointCloud myPCLList)
        {
            this.OpenGL_UControl.AddVertexListAsModel(name, myPCLList);

           
        }
        //public void ShowModel(Model myModel)
        //{
        //    this.OpenGL_UControl.ShowModel(myModel);


        //}

        //public void ShowListOfVertices(PointCloud myPCLList)
        //{
        //    this.OpenGLControl.ShowPointCloud("Point Cloud", myPCLList);

        //}
        public void ShowPointCloud(PointCloud myP, bool removeOthers)
        {
            if (removeOthers)
                this.OpenGL_UControl.RemoveAllModels();
            
            Model myModel = new Model();
            myModel.PointCloud = myP;

            this.OpenGL_UControl.OGLControl.GLrender.AddModel(myModel);

        }
        /// <summary>
        /// at least source points should be non zero
        /// </summary>
        /// <param name="mypointCloudTarget"></param>
        /// <param name="mypointCloudSource"></param>
        /// <param name="mypointCloudResult"></param>
        /// <param name="changeColor"></param>
        public void Show3PointCloudOpenGL(PointCloud mypointCloudSource, PointCloud mypointCloudTarget, PointCloud mypointCloudResult, bool changeColor)
        {

            this.OpenGL_UControl.RemoveAllModels();

            //target in green
            
            if (mypointCloudTarget != null)
            {

                if (changeColor)
                {
                    mypointCloudTarget.Colors = ColorExtensions.ToVector3Array(mypointCloudTarget.Vectors.Length, 0, 255, 0);
                    
                }
                ShowPointCloud(mypointCloudTarget, false);

            }

            if (mypointCloudSource != null)
            {
                //source in white
               
                if (changeColor)
                    mypointCloudSource.Colors = ColorExtensions.ToVector3Array(mypointCloudSource.Vectors.Length, 255, 255, 255);

                ShowPointCloud(mypointCloudSource, false);

            }

            if (mypointCloudResult != null)
            {

                //transformed in red
                if (changeColor)
                    mypointCloudResult.Colors = ColorExtensions.ToVector3Array(mypointCloudResult.Vectors.Length, 255, 0, 0);

                ShowPointCloud(mypointCloudResult, false);

               

            }

        }

        public void ShowPointClouds(List<PointCloud> pcs)
        {
            for(int i = 0; i < pcs.Count; i++)
            {
                ShowPointCloud(pcs[i], false);
            }
           

        }
        /// <summary>
        /// at least source points should be non zero
        /// </summary>
        /// <param name="mypointCloudTarget"></param>
        /// <param name="mypointCloudSource"></param>
        /// <param name="mypointCloudResult"></param>
        /// <param name="changeColor"></param>
        //public void Show3PointClouds(PointCloud mypointCloudSource, PointCloud mypointCloudTarget, PointCloud mypointCloudResult, bool changeColor)
        //{

        //    this.OpenGL_UControl.RemoveAllModels();

        //    //target in green
        //    List<System.Drawing.Color> myColors;
        //    if (mypointCloudTarget != null)
        //    {

        //        if (changeColor)
        //        {
        //            myColors = ColorExtensions.ToColorList(mypointCloudTarget.Count, 0, 255, 0, 255);
        //            PointCloud.SetColorToList(mypointCloudTarget, myColors);
        //        }
        //        this.OpenGL_UControl.ShowPointCloud("ICP Target", mypointCloudTarget);

        //    }

        //    if (mypointCloudSource != null)
        //    {
        //        //source in white
        //        myColors = ColorExtensions.ToColorList(mypointCloudSource.Count, 255, 255, 255, 255);
        //        if (changeColor)
        //            PointCloud.SetColorToList(mypointCloudSource, myColors);
        //        this.OpenGL_UControl.ShowPointCloud("ICP To be matched", mypointCloudSource);

        //    }

        //    if (mypointCloudResult != null)
        //    {

        //        //transformed in red
        //        myColors = ColorExtensions.ToColorList(mypointCloudResult.Count, 255, 0, 0, 255);
        //        if (changeColor)
        //            PointCloud.SetColorToList(mypointCloudResult, myColors);
        //        this.OpenGL_UControl.ShowPointCloud("ICP Solution", mypointCloudResult);

        //    }

        //}
        /// <summary>
        /// at least source points should be non zero
        /// </summary>
        /// <param name="mypointCloudTarget"></param>
        /// <param name="mypointCloudSource"></param>
        /// <param name="mypointCloudResult"></param>
        /// <param name="changeColor"></param>
        public void Show3PointClouds(PointCloud mypointCloudSource, PointCloud mypointCloudTarget, PointCloud mypointCloudResult, bool changeColor)
        {
            this.OpenGL_UControl.RemoveAllModels();
           
        
            List<System.Drawing.Color> myColors;


            if (mypointCloudSource != null)
            {
                mypointCloudSource.Name = "Source";
                //source in white
                myColors = ColorExtensions.ToColorList(mypointCloudSource.Count, 255, 255, 255, 255);
                if (changeColor)
                    PointCloud.SetColorToList(mypointCloudSource, myColors);
                this.OpenGL_UControl.ShowPointCloud(mypointCloudSource);

            }

            if (mypointCloudResult != null)
            {

                mypointCloudResult.Name = "Result";
                            
                //transformed in red
                myColors = ColorExtensions.ToColorList(mypointCloudResult.Count, 255, 0, 0, 255);
                if (changeColor)
                    PointCloud.SetColorToList(mypointCloudResult, myColors);
                this.OpenGL_UControl.ShowPointCloud(mypointCloudResult);

            }
            if (mypointCloudTarget != null)
            {
                mypointCloudTarget.Name = "Target";
                //target in green
                if (changeColor)
                {
                    myColors = ColorExtensions.ToColorList(mypointCloudTarget.Count, 0, 255, 0, 255);
                    PointCloud.SetColorToList(mypointCloudTarget, myColors);
                }
                this.OpenGL_UControl.ShowPointCloud(mypointCloudTarget);

            }

        }
       
        public void ClearModels()
        {
            OpenGL_UControl.RemoveAllModels();
        }
       
        public bool UpdateFirstModel(PointCloud pc)
        {
            //ClearModels();


            ShowPointCloud(pc);
            return true;
        }

        public void ShowPointCloud(PointCloud pc)
        {

            this.OpenGL_UControl.ShowPointCloud(pc);
            
        }
       

        private void TestForm_FormClosed(object sender, FormClosedEventArgs e)
        {

            GLSettings.Height = this.Height;
            GLSettings.Width = this.Width;

            GLSettings.SaveSettings();
            base.OnClosed(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            this.OpenGL_UControl.OGLControl.MouseWheelActions(e);
            base.OnMouseWheel(e);
        }

    
        public void IPCOnTwoPointClouds()
        {

            this.OpenGL_UControl.RemoveAllModels();
            //this.OpenGLControl.OpenTwoTrialPointClouds();
            ICP_OnCurrentModels();

        }

        public bool UpdatePointCloud(PointCloud pc)
        {
            if (pc != null && pc.Count > 0)
                this.OpenGL_UControl.ShowPointCloud( pc);

            //if (this.OpenGLControl.GLrender.Models3D.Count == 0)
            //{
            //    ShowPointCloud(pc);
            //}
            //else
            //{
            //    this.OpenGLControl.RemoveFirstModel(true);
            //    Model3D myNewModel = new Model3D();
            //    myNewModel.Pointcloud = pc;
            //    //this.OpenGLControl.GLrender.Models3D[0].Pointcloud = pc;
            //    this.OpenGLControl.GLrender.Models3D.Add(myNewModel);
            //    this.OpenGLControl.RedrawAllModels(false);
            //    //this.OpenGLControl.Refresh();
            //}

            ////ClearModels();


            return true;
        }
        public void ICP_OnCurrentModels()
        {

            ////convert Points
            //if (this.OpenGLControl.GLrender.Models3D.Count > 1)
            //{
            //    PointCloud mypointCloudTarget = this.OpenGLControl.GLrender.Models3D[0].Pointcloud;
            //    PointCloud mypointCloudSource = this.OpenGLControl.GLrender.Models3D[1].Pointcloud;


            //    ResetModelsToOrigin();

            //    IterativeClosestPointTransform icpSharp = new IterativeClosestPointTransform();
            //    PointCloud myVertexTransformed = icpSharp.PerformICP(mypointCloudSource, mypointCloudTarget);

            //    if (myVertexTransformed != null)
            //    {
            //        //show result
            //        PointCloud.SetColorOfListTo(myVertexTransformed, Color.Red);
            //        this.OpenGLControl.ShowPointCloud("IPC Solution", myVertexTransformed);
            //    }

            //}


        }
        //public void ShowModel(Model myModel, bool removeAllOthers)
        //{
        //    if(removeAllOthers)
        //        this.OpenGLControl.RemoveAllModels();
        //    this.OpenGLControl.ShowModel(myModel);


        //    Model myModel = new Model();
        //    myModel.pointCloudGL = myModel.Pointcloud.ToPointCloudOpenGL();

        //    //this.glControl1.GLrender.AddModel(myModel);
        //    this.OpenGLControl.OGLControl.GLrender.AddModel(myModel);

        //}

    }
}
