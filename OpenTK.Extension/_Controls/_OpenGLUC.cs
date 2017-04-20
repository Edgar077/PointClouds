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
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;



namespace OpenTKExtension
{
    public partial class OpenGLUC : System.Windows.Forms.UserControl
    {

        Matrix4 registrationMatrix;
        PointCloud pSource;
        PointCloud pTarget;
        PointCloud pResult;

        PointCloud pointCloudFirstAfterLoad;

        public OpenGLUC()
        {
            InitializeComponent();
            if (!GLSettings.IsInitializedFromSettings)
                GLSettings.InitFromSettings();

            
            InitComboBox();
            comboRenderMode.SelectedText = GLSettings.ViewMode;
            

            this.initGLControl();
            
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
      


        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //System.Drawing.Color color = System.Drawing.Color.Red;

            //PointCloud PointCloud1 = ExamplePointClouds.Sphere("Sphere", 2, 8, color, (System.Drawing.Bitmap)null);
            //PointCloud myPointCloud = PointCloud1.ToPointCloud();
            //ShowPointCloud(myPointCloud);
            
        }

       

     
        private void toolStripLoadPointCloud_Click(object sender, EventArgs e)
        {
            string fileName = LoadFileDialog();
            PointCloud pc = PointCloud.FromObjFile(fileName);
            //PointCloud myPointCloud = new PointCloud(fileName);
            pointCloudFirstAfterLoad = pc;
            //ShowPointCloud(pointCloudFirstAfterLoad);
            ShowPointCloud_ClearAllOthers(pointCloudFirstAfterLoad);
            //ShowPointCloud(myPointCloud);
            
        }
       
        public void LoadPointCloudFromFile(string fileName, bool clearOthers)
        {
            PointCloud pc = PointCloud.FromObjFile(fileName);

            if (clearOthers)
            {
                ShowPointCloud_ClearAllOthers(pc);
            }
            else
            {
                ShowPointCloud(pc);
            }

        }



        private void comboViewMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strDisplay = Enum.GetValues(typeof(PrimitiveType)).GetValue(comboRenderMode.SelectedIndex).ToString();

            GLSettings.ViewMode = strDisplay;

            this.OGLControl.GLrender.PrimitiveTypes = PrimitiveType.Points;
            for (int i = 0; i < Enum.GetValues(typeof(PrimitiveType)).GetLength(0); i++)
            {
                string strVal = Enum.GetValues(typeof(PrimitiveType)).GetValue(i).ToString();
                if (strVal == strDisplay)
                {
                    this.OGLControl.GLrender.PrimitiveTypes = (PrimitiveType)Enum.GetValues(typeof(PrimitiveType)).GetValue(i);
                    break;
                }

            }
            this.OGLControl.Invalidate();

        }
      
        private void comboModels_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.OGLControl.GLrender.SelectedModelIndex = comboModels.SelectedIndex - 1;
        
        }

        private void comboCameraModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboCameraModel.Text == "Camera")
            {
                this.OGLControl.ModelViewMode = ModelViewMode.Camera;
            }
            else
            {
                this.OGLControl.ModelViewMode = ModelViewMode.ModelMove;
            }
            
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

         private void toolStripSavePointCloud_Click(object sender, EventArgs e)
        {
            this.OGLControl.SaveSelectedModel();
        }

         private void toolStripSavePointCloudAs_Click(object sender, EventArgs e)
        {
            this.OGLControl.SaveSelectedModelAs();
        }

         private void toolStripChangeColor_Click(object sender, EventArgs e)
        {
            if (comboModels.SelectedIndex >= 0)
            {
                ColorDialog colDiag = new ColorDialog();
                // Sets the initial color select to the current text color.

                // Update the text box color if the user clicks OK 
                if (colDiag.ShowDialog() == DialogResult.OK)
                {
                    RenderableObject pcr = this.OGLControl.GLrender.RenderableObjects[this.OGLControl.GLrender.SelectedModelIndex];
                    PointCloud pc = pcr.PointCloud;
                    pc.SetColor(new Vector3(colDiag.Color.R / 255f, colDiag.Color.G / 255f, colDiag.Color.B / 255f));
                    this.OGLControl.Refresh();
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please load a 3D object first");

            }

            
        }



      
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsForm sf = new SettingsForm(this);
            if (sf.ShowDialog() == DialogResult.OK)
            {
                this.OGLControl.GLrender.Draw();
                //RefreshView(true);
                
            }
        }
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            this.glControl1.MouseWheelActions(e);
            base.OnMouseWheel(e);
        }
     
        public OGLControl OGLControl
        {
            get
            {
                return this.glControl1;
            }
        }

        private void comboBoxFill_SelectedIndexChanged(object sender, EventArgs e)
        {
           // this.OGLControl.GLrender.FillMode = 
            if (comboBoxFill.Text == "Fill")
            {
                this.OGLControl.GLrender.PoygonModes = PolygonMode.Fill;
                GLSettings.Fill = true;
               
            }
            else
            {
                this.OGLControl.GLrender.PoygonModes = PolygonMode.Line;
                GLSettings.Fill = false;
            }
            this.glControl1.Refresh();
        }

        private void toolStripRemoveAllModelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveAllPointClouds();
            this.glControl1.GLrender.ClearAllObjects();

            this.Refresh();


        }
        private void removeSelectedModelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.glControl1.GLrender.ClearSelectedModel();
            refreshComboModels();
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
        private void showCameraFOV_Click(object sender, EventArgs e)
        {
            if (showCameraFOVMenuItem.Text == "Show Camera FOV")
            {
                GLSettings.ShowCameraFOV = true;
                showCameraFOVMenuItem.Text = "Hide Camera FOV";
                glControl1.Refresh();
            }
            else if (showCameraFOVMenuItem.Text == "Hide Camera FOV")
            {
                GLSettings.ShowCameraFOV = false;
                showCameraFOVMenuItem.Text = "Show Camera FOV";
                glControl1.Refresh();
            }
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

        //private void checkBoxFill_CheckedChanged(object sender, EventArgs e)
        //{



        //}
        private void convexHullToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //int indexPointCloud = comboModels.SelectedIndex;
            //Model3D myPointCloud = GLrender.Models3D[comboModels.SelectedIndex];



            //List<Vector3> myListVectors = myModel.Pointcloud);
            //ConvexHull3D convHull = new ConvexHull3D(myListVectors);

        }

        private void triangulateNearestNeighbourToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveNormalsOfCurrentModelToolStripMenuItem_Click(object sender, EventArgs e)
        {


            //Model3D myPointCloud = GLrender.Models3D[comboModels.SelectedIndex];
            //CheckNormals(myModel);
            //string path = AppDomain.CurrentDomain.BaseDirectory + "Models";


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
            //        Model3D myPointCloud = GLrender.Models3D[i];
            //        myModel.Pointcloud.LinesNormals = null;
            //    }

            //    RefreshView(true);

            //}



        }

        private void toolStripRegistration_Click(object sender, EventArgs e)
        {
            
           
        }
        private void toolStripShowRegistrationMatrix_Click(object sender, EventArgs e)
        {


        }
       
        
        private void toolStripCalculateRegistration1_2_Click(object sender, EventArgs e)
        {
            registrationMatrix = this.OGLControl.CalculateRegistrationMatrix1_2();
            registrationMatrix.Save(GLSettings.Path + GLSettings.PathPointClouds, "registrationMatrix.txt");


        }
        private void toolStripCalculateRegistrationLoaded_Moved_Click(object sender, EventArgs e)
        {
            if (pointCloudFirstAfterLoad != null)
            {
                registrationMatrix = this.OGLControl.CalculateRegistrationMatrix(this.pointCloudFirstAfterLoad);
                registrationMatrix.Save(GLSettings.Path + GLSettings.PathPointClouds, "registrationMatrix.txt");
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("No Point cloud available");
            }
        }

        private void toolStripAlignUsingRegistrationMatrix_Click(object sender, EventArgs e)
        {

            registrationMatrix.FromFile(GLSettings.Path + GLSettings.PathPointClouds, "registrationMatrix.txt");
            this.OGLControl.AlignFirstModelFromRegistratioMatrix(registrationMatrix);
            this.OGLControl.Refresh();

            //registrationMatrix = this.OGLControl.CalculateRegistrationMatrix();

        }
      
        private void testClouds_FirstIteration()
        {
            this.Cursor = Cursors.WaitCursor;

            ICPLib.IterativeClosestPointTransform icp = new ICPLib.IterativeClosestPointTransform();
            icp.ICPSettings.Prealign_PCA = true;
          
            pTarget = icp.AlignCloudsFromDirectory_StartFirst(GLSettings.Path + GLSettings.PathPointClouds + "\\Nick", 100);

            SaveResultCloudAndShow(pTarget);

            this.Cursor = Cursors.Default;


            //pTarget = icp.AlignCloudsFromDirectory(GLSettings.Path + GLSettings.PathModels + "\\Nick", 30);
            //pTarget = icp.AlignCloudsFromDirectory_StartLast(GLSettings.Path + GLSettings.PathModels + "\\Nick", 10);

        }
        private void testClouds_SecondIteration()
        {

            this.Cursor = Cursors.WaitCursor;

            ICPLib.IterativeClosestPointTransform icp = new ICPLib.IterativeClosestPointTransform();
            pTarget = icp.AlignCloudsFromDirectory_StartFirst(GLSettings.Path + GLSettings.PathPointClouds + "\\Nick\\Result", 10);
            SaveResultCloudAndShow(pTarget);

            this.Cursor = Cursors.Default;
        }


     

        private void toolStripPCA_Axes_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            //TestLoadTwoClouds();

            Clouds_AlignPCAAxes();
            this.Cursor = Cursors.Default;
        }

        private void toolStripICP_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            //TestLoadTwoClouds();
            Clouds_ICP();
            this.Cursor = Cursors.Default;
        }

        private void toolStripPCA_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            //TestLoadTwoClouds();
            Clouds_PCA();
            this.Cursor = Cursors.Default;
        }
        private void Clouds_AlignPCAAxes()
        {
            if (!GetFirstTwoCloudsFromOpenGLControl())
                return;




            PointCloud pointCloudTarget = this.pSource;
            pointCloudTarget = PCA.RotateToOriginAxes(pointCloudTarget);


            PointCloud pointCloudSource = this.pTarget;

            PointCloud pointCloudResult = PCA.RotateToOriginAxes(pointCloudSource);
            pResult = pointCloudResult;
            //pResult.SetColor(new OpenTK.Vector3(1, 0, 0));

            pointCloudResult.AddPointCloud(this.pTarget);
            SaveResultCloudAndShow(pointCloudResult);
            //DisplayResultPointCloud();
            //DisplayObjects();


        }


        private void LoadTwoClouds()
        {

            string fileName1 = GLSettings.FileNamePointCloudLast1;
            string fileName2 = GLSettings.FileNamePointCloudLast2;


            //tabControlImages.SelectTab(0);


            pSource = new PointCloud(GLSettings.Path + GLSettings.PathPointClouds, fileName1);

           
            pointCloudFirstAfterLoad = pSource.Clone();
            //pSource.SetColor(new OpenTK.Vector3(0, 1, 0));

            pSource.Name = GLSettings.FileNamePointCloudLast1;
            pSource.Path = GLSettings.Path + GLSettings.PathPointClouds;
            pSource.FileNameLong = pSource.Path + "\\" + pSource.Name;



            pTarget = new PointCloud(GLSettings.Path + GLSettings.PathPointClouds, fileName2);

          
            //pTarget.SetColor(new OpenTK.Vector3(1, 1, 1));
            pTarget.Name = GLSettings.FileNamePointCloudLast2;
            pTarget.Path = GLSettings.Path + GLSettings.PathPointClouds;
            pTarget.FileNameLong = pTarget.Path + "\\" + pTarget.Name;



        }
        private void Clouds_PCA()
        {
            if (!GetFirstTwoCloudsFromOpenGLControl())
                return;


            PCA pca = new PCA();
            pca.MaxmimumIterations = 1;
            PointCloud pointCloudResult = pca.AlignPointClouds_SVD(this.pSource, this.pTarget);

            pointCloudResult.AddPointCloud(this.pTarget);

            SaveResultCloudAndShow(pointCloudResult);
            this.registrationMatrix = pca.Matrix;
            registrationMatrix.Save(GLSettings.Path + GLSettings.PathPointClouds, "registrationMatrix.txt");

        }
        private void SaveResultCloudAndShow(PointCloud pointCloudResult)
        {
            //-------------------------------------
            //save
            if (pointCloudResult != null)
            {

                string path, fileNameShort;
                
                IOUtils.ExtractDirectoryAndNameFromFileName(pointCloudResult.FileNameLong, out fileNameShort, out path);
                pointCloudResult.ToObjFile(pointCloudResult.Path, "Result.obj");


                this.pResult = pointCloudResult;
                //pResult.SetColor(new OpenTK.Vector3(1, 0, 0));

                DisplayResultPointCloud();


            }

        }
        private void DisplaySourceAndTargetClouds(bool removeAllOther)
        {
            if(removeAllOther)
                this.OGLControl.GLrender.ClearAllObjects();

            this.ShowPointCloud(this.pSource);
            this.ShowPointCloud(this.pTarget);
           // DisplayResultPointCloud();

        }
        private void DisplayResultPointCloud()
        {
            if (this.pResult != null)
            {
                ShowPointCloud(pResult);

                //RenderableObject pcr = new PointCloudRenderable();
                //pcr.PointCloud = this.pResult;
                //this.OGLControl.GLrender.AddRenderableObject(pcr);
            }

        }
        private void Clouds_ICP()
        {

            if (!GetFirstTwoCloudsFromOpenGLControl())
                return;



            //------------------
            //ICP
            ICPLib.IterativeClosestPointTransform icp = new ICPLib.IterativeClosestPointTransform();
            icp.Reset_RealData();

            icp.ICPSettings.ICPVersion = ICP_VersionUsed.Zinsser;
            icp.ICPSettings.MaximumNumberOfIterations = 50;
          
           
            PointCloud pointCloudResult = icp.PerformICP(this.pSource, this.pTarget);

            pointCloudResult.AddPointCloud(this.pTarget);
            SaveResultCloudAndShow(pointCloudResult);
            this.registrationMatrix = icp.Matrix;
            registrationMatrix.Save(GLSettings.Path + GLSettings.PathPointClouds, "registrationMatrix.txt");

        }

        private bool GetFirstTwoCloudsFromOpenGLControl()
        {
            if (this.OGLControl.GLrender.RenderableObjects.Count < 2)
                return false;

            this.pSource = this.OGLControl.GLrender.RenderableObjects[0].PointCloud;
            this.pTarget = this.OGLControl.GLrender.RenderableObjects[1].PointCloud;

            return true;
        }
        private void testClouds()
        {


            ICPLib.IterativeClosestPointTransform icp = new ICPLib.IterativeClosestPointTransform();

            pTarget = null;

            for (int i = 0; i < 10; i++)
            {

                //first iteration
                if (pTarget == null)
                {
                    pTarget = PointCloud.FromObjFile(GLSettings.Path + GLSettings.PathPointClouds, "Nick\\PointCloudSequence#" + (i).ToString() + ".obj");

                }



                pSource = PointCloud.FromObjFile(GLSettings.Path + GLSettings.PathPointClouds, "Nick\\PointCloudSequence#" + (i + 1).ToString() + ".obj");

                //------------------
                //ICP

                icp.Reset_RealData();
                //icp.ICPSettings.ThresholdMergedPoints = 0f;
                icp.ICPSettings.MaximumNumberOfIterations = 15;


                pTarget = icp.PerformICP(pSource, this.pTarget);
                System.Diagnostics.Debug.WriteLine("###### ICP for point cloud: " + pTarget.Name + " - points added: " + icp.PointsAdded.ToString());


                //   this.registrationMatrix = icp.Matrix;
                //   registrationMatrix.Save(GLSettings.Path + GLSettings.PathPointClouds, "registrationMatrix.txt");


            }
            GlobalVariables.ShowLastTimeSpan("--> Time for ICP ");

            SaveResultCloudAndShow(pTarget);

        }

      
        private void toolStripTest2_Click(object sender, EventArgs e)
        {
            testClouds_SecondIteration();
        }
        private void toolStripLoadTwoPCL_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            testClouds();

            //LoadTwoClouds();
            //DisplaySourceAndTargetClouds(true);

            this.Cursor = Cursors.Default;
        }
        private void toolStripTriangulate_Delaunay_Click(object sender, EventArgs e)
        {

            if (this.glControl1.GLrender.RenderableObjects.Count > 0)
            {
                PointCloud pc = this.glControl1.GLrender.RenderableObjects[0].PointCloud;
                OpenTKExtension.Triangulation.Mesh m = OpenTKExtension.Triangulation.Mesh.Triangulate(pc, 6);
                pc.CreateIndicesFromTriangles(m.Triangles);

                RemoveAllPointClouds();
                this.glControl1.GLrender.ClearAllObjects();
                ShowPointCloud(pc);


            }
       

            return;
        }
        /// <summary>
        /// KDTree triangulation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripTriangulate_Click(object sender, EventArgs e)
        {

            if (this.glControl1.GLrender.RenderableObjects.Count > 0)
            {
                PointCloud pc = this.glControl1.GLrender.RenderableObjects[0].PointCloud;
                //pc.Triangulate_KDTree(10);
                pc.Triangulate25D(0.01f);
                
                RemoveAllPointClouds();
                this.glControl1.GLrender.ClearAllObjects();
                ShowPointCloud(pc);


            }


            return;
        }
       
        private void toolStripOutliers_Click(object sender, EventArgs e)
        {

            if(this.glControl1.GLrender.RenderableObjects.Count > 0)
            {
                PointCloud pc = this.glControl1.GLrender.RenderableObjects[0].PointCloud;
                int thresholdNeighboursCount = 20;
                float thresholdDistance = 4e-4f;

                PointCloud pcWithOutliersMarkedRed;
                PointCloud pointCloudClean = OpenTKExtension.Outliers.ByStandardDeviation(pc, thresholdNeighboursCount, thresholdDistance, out pcWithOutliersMarkedRed);


                RemoveAllPointClouds();
                this.glControl1.GLrender.ClearAllObjects();
                ShowPointCloud(pcWithOutliersMarkedRed);
              

            }

          

            // tweaks

            //PointCloud pointCloudClean = tree.RemoveOutliersNeighbour(PointCloudDirty, thresholdDistance, thresholdNeighboursCount);

            //pointCloudClean.ToObjFile(pathResult + "\\CleanPointCloudSequence#" + i.ToString() + ".obj");

        }
        private void toolStripOutliersBatch_Click(object sender, EventArgs e)
        {
            string directory = GLSettings.Path + GLSettings.PathPointClouds + "\\Nick";
            string pathResult = directory + "\\clean";

            string[] files = IOUtils.FileNamesSorted(directory, "*.obj");
            
            if (!System.IO.Directory.Exists(pathResult))
                System.IO.Directory.CreateDirectory(pathResult);

            for (int i = 0; i < files.Length; i++)
            {
                PointCloud pointCloudDirty = PointCloud.FromObjFile(files[i]);

                int thresholdNeighboursCount = 10;
                float thresholdDistance = 4e-4f;

                PointCloud pcOutliers;
                PointCloud pointCloudClean = OpenTKExtension.Outliers.ByStandardDeviation(pointCloudDirty, thresholdNeighboursCount, thresholdDistance, out pcOutliers);

                // tweaks

                //PointCloud pointCloudClean = tree.RemoveOutliersNeighbour(PointCloudDirty, thresholdDistance, thresholdNeighboursCount);

                pointCloudClean.ToObjFile(pathResult + "\\CleanPointCloudSequence#" + i.ToString() + ".obj");
            }

            //testClouds_FirstIteration();

            return;
        }
        private void toolStripTest1_Click(object sender, EventArgs e)
        {
       
            testClouds_FirstIteration();
          
            return;
        }
        private void toolStripTest1_Click_LoadCloud(object sender, EventArgs e)
        {
            string directory = GLSettings.Path + GLSettings.PathPointClouds + "\\Nick";
          
            string[] files = IOUtils.FileNamesSorted(directory, "*.obj");


            for (int i = 0; i < files.Length; i++)
            {
                LoadPointCloudFromFile(files[i], true);
                //LoadPointCloudFromFile(files[i]);
            }
            //for (int i = 0; i < 100; i++)
            //{
            //    LoadPointCloudFromFile(files[i], true);
            //    //LoadPointCloudFromFile(files[i]);
            //}

          

            return;
        }
    }
}
