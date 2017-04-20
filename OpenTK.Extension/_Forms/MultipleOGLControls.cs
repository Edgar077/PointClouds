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
    public partial class MultipleOGLControls : Form
    {

        
        
        public OpenGLUC OpenGLControl;

        public MultipleOGLControls()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(CultureInfo.CurrentCulture.LCID);
            InitializeComponent();
           
         
            AddOpenGLControl();

            if (!GLSettings.IsInitializedFromSettings)
                GLSettings.InitFromSettings();

            this.Height = GLSettings.Height;
            this.Width = GLSettings.Width;


        }
        private void AddOpenGLControl()
        {
            this.OpenGLControl = new OpenGLUC();
            this.SuspendLayout();
            // 
            // openGLControl1
            // 
            this.OpenGLControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OpenGLControl.Location = new System.Drawing.Point(0, 0);
            this.OpenGLControl.Name = "openGLControl1";
            this.OpenGLControl.Size = new System.Drawing.Size(854, 453);
            this.OpenGLControl.TabIndex = 0;

           
          
            this.ResumeLayout(false);
        }
    
   

        protected override void OnClosed(EventArgs e)
        {
           
            GLSettings.Height = this.Height;
            GLSettings.Width = this.Width;

            GLSettings.SaveSettings();
            base.OnClosed(e);
        }
      
       

        //public void ShowListOfVertices(PointCloud myPCLList)
        //{
        //    this.OpenGLControl.ShowPointCloud("Point Cloud", myPCLList);

        //}
        public void ShowPointCloudOpenGL(PointCloud myP, bool removeOthers)
        {
            if (removeOthers)
                this.OpenGLControl.RemoveAllPointClouds();
            
          

            this.OpenGLControl.OGLControl.GLrender.AddPointCloud(myP);

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

            this.OpenGLControl.RemoveAllPointClouds();

            //target in green
            
            if (mypointCloudTarget != null)
            {

                if (changeColor)
                {
                    mypointCloudTarget.Colors = ColorExtensions.ToVector3Array(mypointCloudTarget.Vectors.Length, 0, 255, 0);
                    
                }
                ShowPointCloudOpenGL(mypointCloudTarget, false);

            }

            if (mypointCloudSource != null)
            {
                //source in white
               
                if (changeColor)
                    mypointCloudSource.Colors = ColorExtensions.ToVector3Array(mypointCloudSource.Vectors.Length, 255, 255, 255);

                ShowPointCloudOpenGL(mypointCloudSource, false);

            }

            if (mypointCloudResult != null)
            {

                //transformed in red
                if (changeColor)
                    mypointCloudResult.Colors = ColorExtensions.ToVector3Array(mypointCloudResult.Vectors.Length, 255, 0, 0);

                ShowPointCloudOpenGL(mypointCloudResult, false);

               

            }

        }
   
        /// <summary>
        /// at least source points should be non zero
        /// </summary>
        /// <param name="mypointCloudTarget"></param>
        /// <param name="mypointCloudSource"></param>
        /// <param name="mypointCloudResult"></param>
        /// <param name="changeColor"></param>
        public void Show3PointClouds(PointCloud mypointCloudSource, PointCloud mypointCloudTarget, PointCloud mypointCloudResult, bool changeColor)
        {

            this.OpenGLControl.RemoveAllPointClouds();
            mypointCloudTarget.Name = "Target";
            mypointCloudSource.Name = "Source";
            mypointCloudResult.Name = "Result";

            //target in green
            List<System.Drawing.Color> myColors;
            if (mypointCloudTarget != null)
            {

                if (changeColor)
                {
                    myColors = ColorExtensions.ToColorList(mypointCloudTarget.Count, 0, 255, 0, 255);
                    PointCloud.SetColorToList(mypointCloudTarget, myColors);
                }
                this.OpenGLControl.ShowPointCloud( mypointCloudTarget);

            }

            if (mypointCloudSource != null)
            {
                //source in white
                myColors = ColorExtensions.ToColorList(mypointCloudSource.Count, 255, 255, 255, 255);
                if (changeColor)
                    PointCloud.SetColorToList(mypointCloudSource, myColors);
                this.OpenGLControl.ShowPointCloud(mypointCloudSource);

            }

            if (mypointCloudResult != null)
            {

                //transformed in red
                myColors = ColorExtensions.ToColorList(mypointCloudResult.Count, 255, 0, 0, 255);
                if (changeColor)
                    PointCloud.SetColorToList(mypointCloudResult, myColors);
                this.OpenGLControl.ShowPointCloud(mypointCloudResult);

            }

        }
       
        public void ClearModels()
        {
            OpenGLControl.RemoveAllPointClouds();
        }
       
        public bool UpdateFirstModel(PointCloud pc)
        {
            //ClearModels();


            ShowPointCloud(pc);
            return true;
        }

        public void ShowPointCloud(PointCloud pc)
        {

            this.OpenGLControl.ShowPointCloud( pc);
            
        }
      
      
        private void TestForm_FormClosed(object sender, FormClosedEventArgs e)
        {
           
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            this.OpenGLControl.OGLControl.MouseWheelActions(e);
            base.OnMouseWheel(e);
        }

    
        public void IPCOnTwoPointClouds()
        {

            this.OpenGLControl.RemoveAllPointClouds();
            //this.OpenGLControl.OpenTwoTrialPointClouds();
            ICP_OnCurrentModels();

        }

        public bool UpdatePointCloud(PointCloud pc)
        {
            if (pc != null && pc.Count > 0)
                this.OpenGLControl.ShowPointCloud(pc);

            //if (this.OpenGLControl.GLrender.Models3D.Count == 0)
            //{
            //    ShowPointCloud(pc);
            //}
            //else
            //{
            //    this.OpenGLControl.RemoveFirstModel(true);
            //    Model3D myNewPointCloud = new PointCloud 3D();
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
        //public void ShowModel(PointCloud myModel, bool removeAllOthers)
        //{
        //    if(removeAllOthers)
        //        this.OpenGLControl.RemoveAllModels();
        //    this.OpenGLControl.ShowModel(myModel);


        //    PointCloud myPointCloud = new PointCloud ();
        //    myModel.pointCloudGL = myModel.Pointcloud.ToPointCloudOpenGL();

        //    //this.glControl1.GLrender.AddModel(myModel);
        //    this.OpenGLControl.OGLControl.GLrender.AddModel(myModel);

        //}

    }
}
