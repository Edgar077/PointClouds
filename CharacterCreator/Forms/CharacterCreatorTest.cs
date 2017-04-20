using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTKExtension;
using OpenTK;
using PointCloudUtils;


namespace CharacterCreator
{
    public partial class CharacterCreatorTest : Form
    {
         
         string pathModels;
         //float conversionFactor = 100;

         FaceMatcher faceMatcher;
        
        public CharacterCreatorTest()
        {
            pathModels = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + GLSettings.PathPointClouds;
            InitializeComponent();

            
            faceMatcher = new FaceMatcher();
            faceMatcher.LoadHumanoid(4);

            showHumanoid(false);
            populatePoseComboBox();
            
        }
       
        private void buttonReload_Click(object sender, EventArgs e)
        {
            this.faceMatcher.LoadHumanoid(4);


        }
        private void UpdateUI()
        {
            

            //f = humanoid.m_engine.measures["body_height_Z"];
            float f = faceMatcher.Humanoid.GetHeight();
            int i = Convert.ToInt32(f * 100)  ;
            this.numericUpDownHeight.Value = i;
        }
        private void populatePoseComboBox()
        {
            string[] files = System.IO.Directory.GetFiles(faceMatcher.Humanoid.PathCharacters + "\\shared_poses\\" + faceMatcher.Humanoid.NameGeneral);
            foreach(string file in files)
            {
                string fileShort = IOUtils.ExtractFileNameShortWithoutExtension(file);
                comboBoxPose.Items.Add(fileShort);
            }

        }
        //private void SetHeight()
        //{
        //    float f = humanoid.m_engine.measures["body_height_Z"];

        //    //humanoid.measures_data[]
        //    //humanoid.measures_database_exist
        //    //for measure in sorted(the_humanoid.measures.keys()):
        //    //                if measure != "body_height_Z":

        //}
        private void comboBoxPose_SelectedIndexChanged(object sender, EventArgs e)
        {

            string[] files = System.IO.Directory.GetFiles(faceMatcher.Humanoid.PathCharacters + "\\shared_poses\\" + faceMatcher.Humanoid.NameGeneral);
            string fileName = files[comboBoxPose.SelectedIndex];

            //A:\Archiv\3D\_PointCloudScanner\bin\Characters\shared_poses\humanoid_humanf

        }

      
        private void buttonDeleteFace_Click(object sender, EventArgs e)
        {
            faceMatcher.CutFace();
            showHumanoid(true);

            
        }
        private void triangulate()
        {
            this.openGLUC1.RemoveAllPointClouds();

            PointCloud pc = faceMatcher.Humanoid.ToPointCloud();
            //pc.TriangulateVertices_Rednaxela(0.1f);

            this.openGLUC1.ShowPointCloud(pc);


            UtilsPointCloudIO.ToObjFile_ColorInVertex(pc, pathModels, "modelTriangulated.obj");
        }
        private void buttonTriangulate_Click(object sender, EventArgs e)
        {
            triangulate();

        }
       
        private void buttonUpdateModel_Click(object sender, EventArgs e)
        {

            this.faceMatcher.UpdateModel_Joints();
            showAll();


        }

   
        protected override void OnClosed(EventArgs e)
        {
         
            GLSettings.Height = this.Height;
            GLSettings.Width = this.Width;

            GLSettings.SaveSettings();
            base.OnClosed(e);
        }

     
        private void buttonShowSkeleton_Click(object sender, EventArgs e)
        {
            this.faceMatcher.LoadSkeleton(this.pathModels + "\\Skeleton", "joints.json");
            showSkeleton();

        }

    
        private void showFace()
        {

            this.openGLUC1.ShowPointCloud(this.faceMatcher.FaceNew);
            UpdateUI();   
        }
        private void showSkeleton()
        {
            this.openGLUC1.ShowRenderableObject(this.faceMatcher.Skeleton);
            UpdateUI();   
        }
     
        private void buttonLoadFace_Click(object sender, EventArgs e)
        {
            showFace();


        }
        private void showHumanoid(bool clearOthers)
        {
            PointCloud pc = faceMatcher.Humanoid.ToPointCloud();
            //CheckMeasures();
            if (clearOthers)
                this.openGLUC1.ShowPointCloud_ClearAllOthers(pc);
            else
                this.openGLUC1.ShowPointCloud(pc);

            UpdateUI();
        }
        private void showAll()
        {
            showHumanoid(true);
            showSkeleton();
            showFace();

        }

        private void buttonShowSkeletonAdjustModel_Click(object sender, EventArgs e)
        {
            this.openGLUC1.ClearAll();

            //this.faceMatcher.LoadHumanoid();
            this.faceMatcher.LoadFace(this.pathModels + "\\Face", "Ed1.obj");
            this.faceMatcher.Rotate_AdjustFaceDepth();

            this.faceMatcher.LoadSkeleton(this.pathModels + "\\Skeleton", "joints.json");
            this.faceMatcher.UpdateModel_Joints();
            this.faceMatcher.CutFace();
            this.faceMatcher.AdjustFaceToHumanoid();

            PointCloud result = this.faceMatcher.MergeResultModelAndSave(this.pathModels + "\\Face", "result.obj");
           

            this.openGLUC1.ShowPointCloud(result);
            showSkeleton();
            UpdateUI();   
            //showHumanoid(false);
            //showFace();
            



        }
     
       

    }
}
