using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using OpenTKExtension;
using OpenTK;

namespace UnitTestsOpenTK
{
    
    public class TestBase
    {
        protected static bool UIMode = true;
        //protected static string path;
        protected string pathUnitTests;
        protected string pathModels;
        protected string pathBin;

        protected PointCloud pointCloudAddition1 = null;
        protected PointCloud pointCloudAddition2 = null;
        protected PointCloud expectedResultCloud = new PointCloud();

        protected PointCloud pointCloudTarget = null;
        protected PointCloud pointCloudSource = null;
        protected PointCloud pointCloudResult = null;
        protected float cubeSizeX = 2f;

        
        
        
        protected float threshold = Convert.ToSingle(1E-4);

        DateTime CurrentTime;

                 
        protected bool checkMultipleNormals = false;

        [SetUp]
        protected virtual void SetupTest()
        {
            GlobalVariables.ResetTime();
            Performance_Start();
            expectedResultCloud = new PointCloud();
            pointCloudTarget = null;
            pointCloudSource = null;
            pointCloudResult = null;
            pointCloudAddition1 = null;
            pointCloudAddition2 = null;


        }
        public TestBase()
        {
            pathBin = AppDomain.CurrentDomain.BaseDirectory;
            pathModels = pathBin + "Models";
            pathUnitTests = pathBin + "Models\\UnitTests";
            
           
        }
      
        protected void Show3PointCloudsInWindow(bool changeColor)
        {
            TestForm fOTK = new TestForm();
            fOTK.Show3PointClouds(pointCloudSource, pointCloudTarget, pointCloudResult, changeColor);
            fOTK.ShowDialog();


        }
        protected void ShowVerticesInWindow(byte[] colorModel1, byte[] colorModel2)
        {
            TestForm fOTK = new TestForm();
            PointCloud.SetColorToList(pointCloudSource, colorModel1);
            fOTK.AddVerticesAsModel("Point Cloud", pointCloudSource);



            if (pointCloudResult != null && this.pointCloudResult.Count > 0)
            {


                PointCloud.SetColorToList(pointCloudResult, colorModel2);
                fOTK.AddVerticesAsModel("Transformed", pointCloudResult);

            }

            //fOTK.ShowModels();

            fOTK.ShowDialog();

          
        }
        protected void ShowModel(Model myModel)
        {
            TestForm fOTK = new TestForm();

            fOTK.ShowModel(myModel);
          
            GlobalVariables.ShowLastTimeSpan("Show Model");
            fOTK.ShowDialog();

        }
        protected void ShowPointCloud(PointCloud pc)
        {
            TestForm fOTK = new TestForm();

            fOTK.ShowPointCloud(pc);

            GlobalVariables.ShowLastTimeSpan("Show PC");
            fOTK.ShowDialog();

        }

        protected void ShowVector3DInWindow(List<Vector3> listResult)
        {

            pointCloudSource = PointCloud.FromListVector3(listResult);

            Model myModel = new Model();
            myModel.PointCloud = pointCloudSource;

            ShowModel(myModel);

        }
     
        protected void ShowModel_Cube(Model myModel)
        {
            TestForm fOTK = new TestForm();
            
            
            fOTK.ShowModel(myModel);


            GlobalVariables.ShowLastTimeSpan("Show Model");
            fOTK.ShowDialog();

        }
        protected void ShowModel_Normals(Model myModel)
        {
            TestForm fOTK = new TestForm();

            
            fOTK.ShowModel(myModel);


            GlobalVariables.ShowLastTimeSpan("Show Model");
            fOTK.ShowDialog();


        }
    
     


        //private void UpateModel_Faces(Model myModel, List<cFace> listFaces)
        //{
        //    List<Triangle> listTriangle = new List<Triangle>();

        //    System.Diagnostics.Debug.WriteLine("Number of faces " + listFaces.Count.ToString());
        //    for (int i = 0; i < listFaces.Count; i++)
        //    {
        //        cFace face = listFaces[i];
        //        Triangle a = new Triangle();

        //        for (int j = 0; j < face.Vertices.Length; j++)
        //        {
        //            a.IndVertices.Add(face.Vertices[j].IndexInModel);

        //        }

        //        listTriangle.Add(a);
        //    }


        //    Part p = new Part();
        //    //myModel.Triangles = listTriangle;
        //    //myModel.Parts.Add(p);

        //    //myModel.Helper_AdaptNormalsForEachVertex();
        //    myModel.PointCloud.CalculateCentroidBoundingBox();
        //}
        protected Model CreateModel(string modelName, PointCloud myList, List<cFace> listFaces)
        {
            Model myModel = new Model();
            myModel.PointCloud = myList;

            return myModel;
        }


        protected bool CheckResult(float d1, float d2, float threshold)
        {
            float diff = Math.Abs(d1 - d2);
            if (diff > threshold)
                return false;
            return true;

        }
        protected void ShowCubeVerticesNormals(float cubeSize, List<Vector3> normals)
        {

            PointCloud.SetColorOfListTo(pointCloudSource, Color.Red);
            Model myModel = new Model();

            myModel.PointCloud = pointCloudSource;
          
            ShowModel_Cube(myModel);

        }
        protected void ShowVerticesNormals(List<Vector3> normals)
        {

            PointCloud.SetColorOfListTo(pointCloudSource, Color.Red);
            Model myModel = new Model();

            myModel.PointCloud = pointCloudSource;
            myModel.Normals = normals;


          
            ShowModel(myModel);
            //ShowModel_CubeLines(myModel, 1, true);



        }
        protected void ShowCubeLinesAndNormals(float cubeSize, List<Vector3> normals)
        {

            //PointCloud.SetColorOfListTo(pointCloudSource, Color.Red);
            Model myModel = new Model();

            myModel.PointCloud = pointCloudSource;
            myModel.Normals = normals;


          
            ShowModel_Cube(myModel);

        }
        //protected void ShowCubeNormals(float cubeSize, List<Vector3> normals)
        //{

        //    PointCloud.SetColorOfListTo(pointCloudSource, Color.Red);
        //    Model myModel = new Model("Cube");

        //    myModel.PointCloud = pointCloudSource;
        //    myModel.Normals = normals;

          

        //    ShowModel_Normals(myModel);

        //}
     
     
      
        protected void Show4PointCloudsInWindow(bool changeColor)
        {
            TestForm fOTK = new TestForm();


            fOTK.Show3PointClouds(pointCloudSource, pointCloudTarget, pointCloudResult, changeColor);
            if (pointCloudAddition1 != null)
                fOTK.AddVerticesAsModel("Original Data", this.pointCloudAddition1);
            fOTK.ShowDialog();


        }
        protected void ShowPointCloudsInWindow_PCAVectors(bool changeColor)
        {
            TestForm fOTK = new TestForm();

            fOTK.Show3PointClouds(pointCloudSource, pointCloudTarget, pointCloudResult, changeColor);
                       
            fOTK.ShowDialog();


        }
       
        protected void CreateCube()
        {

            CreateCube(2);
            

        }
        protected void CreateCube(int numberOfPoints)
        {
            
            
          
            
            List<Vector3> listVectors = Example3DModels.Cuboid_Corners_CenteredAt0(1, 2, 1);
           
            this.pointCloudTarget = PointCloud.FromListVector3(listVectors);


            this.pointCloudSource = PointCloud.CloneAll(pointCloudTarget);

           

        }
        protected void CreateCuboid(float sizeX, float sizeY, float sizeZ)
        {
            List<Vector3> listVectors = Example3DModels.Cuboid_Corners_CenteredAt0(sizeX, sizeY, sizeZ);
            this.pointCloudTarget = PointCloud.FromListVector3(listVectors);
            
            PointCloud.ResetToOriginAxis(pointCloudTarget);
            PointCloud.SetIndicesForCubeCorners(this.pointCloudTarget);

            this.pointCloudSource = pointCloudTarget.Clone();
            PointCloud.SetIndicesForCubeCorners(pointCloudSource);


        }
       
        protected void CreateCuboid(int numberOfPoints)
        {
           
            pointCloudTarget = Example3DModels.Cuboid(4, 2, 1, numberOfPoints, numberOfPoints, numberOfPoints);
            pointCloudSource = PointCloud.CloneAll(pointCloudTarget);
            
            pointCloudSource = PointCloud.ResizeAndSort_Distance(pointCloudSource);
            pointCloudTarget = PointCloud.ResizeAndSort_Distance(pointCloudTarget);

        }
        protected void CreateTileEmpty(int numberOfPoints)
        {
           
            pointCloudTarget = Example3DModels.CuboidEmpty(4, 2, 1, numberOfPoints, numberOfPoints, numberOfPoints);
           
            this.pointCloudSource = PointCloud.CloneAll(pointCloudTarget);

        }
        protected void CreateRectangle()
        {
           
           
            pointCloudTarget = Example3DModels.Rectangle(4, 2, 2, 2);
          
            this.pointCloudSource = PointCloud.CloneAll(pointCloudTarget);

        }
        protected void CreateRectangleTranslated()
        {
           
            

            pointCloudTarget = Example3DModels.Rectangle(4, 2, 2, 2);
            PointCloud.Translate(pointCloudTarget, 3, 3, -1);
            this.pointCloudSource = PointCloud.CloneAll(pointCloudTarget);

        }
        protected void ResetPointCloudForOpenGL()
        {
            this.pointCloudSource = null;
            this.pointCloudTarget = null;
            this.pointCloudResult = null;

        }

        protected void ShowResultsInWindow_CubeNew(bool changeColor, bool showCornerLines)
        {

          
            //color code: 
            //Target is green
            //source : white
            //result : red

            //so - if there is nothing red on the OpenTK control, the result overlaps the target

            //if (pointCloudSource != null)
            //{
                
            //    if(showCornerLines)
            //        PointCloud.SetIndicesForCubeCorners(pointCloudSource);

            //}


            //if (pointCloudTarget != null)
            //{
            //    this.pointCloudSource = pointCloudTarget;
            //    if (showCornerLines)
            //        PointCloud.SetIndicesForCubeCorners(pointCloudSource);
            //}


            //if (pointCloudResult != null)
            //{
            //    this.pointCloudResult = pointCloudResult;
            //    if (showCornerLines)
            //        PointCloud.SetIndicesForCubeCorners(pointCloudResult);
            //}


            TestForm fOTK = new TestForm();
            fOTK.Show3PointCloudOpenGL(this.pointCloudSource, this.pointCloudTarget, this.pointCloudResult , true);
            fOTK.ShowDialog();


        }
        protected void ShowResultsInWindow_Cube(bool changeColor)
        {

          
            //color code: 
            //Target is green
            //source : white
            //result : red

            //so - if there is nothing red on the OpenTK control, the result overlaps the target
            if (changeColor)
            {
                if (pointCloudTarget != null)
                    PointCloud.SetColorOfListTo(pointCloudTarget, Color.Green);
                PointCloud.SetColorOfListTo(pointCloudSource, Color.White);
            }


            if (pointCloudResult != null)
            {
                if (changeColor)
                    PointCloud.SetColorOfListTo(pointCloudResult, Color.White);

            }


            TestForm fOTK = new TestForm();
            fOTK.Show3PointClouds(pointCloudSource, pointCloudTarget, pointCloudResult, false);
            fOTK.ShowDialog();


        }
     
        protected void ShowResultsInWindow_Cube_ProjectedPoints(bool changeColor)
        {


            if (pointCloudAddition1 != null)
            {
                PointCloud.SetColorOfListTo(pointCloudAddition1, Color.Black);

            }


            //color code: green, red, violet axes
            //cube is white


            //so - if there is nothing red on the OpenTK control, the result overlaps the target
            PointCloud.SetColorOfListTo(pointCloudSource, Color.Black);

            if (pointCloudTarget != null)
            {
                PointCloud.SetColorOfListTo(pointCloudTarget, Color.Red);
            }


            if (pointCloudResult != null)
            {
                PointCloud.SetColorOfListTo(pointCloudResult, Color.Violet);

            }



            TestForm fOTK = new TestForm();
            fOTK.Show3PointClouds(pointCloudSource, pointCloudTarget, pointCloudResult, false);
            if (this.pointCloudAddition1 != null)
            {
                fOTK.AddVerticesAsModel("Cube", this.pointCloudAddition1);
            }
            if (pointCloudAddition2 != null)
            {

                fOTK.AddVerticesAsModel("Cube Transformed", this.pointCloudAddition2);
            }
            fOTK.ShowDialog();


        }
        protected void ShowPointCloudForOpenGL(PointCloud pcl)
        {

            TestForm fOTK = new TestForm();
            fOTK.ShowPointCloudOpenGL(pcl, true);
            fOTK.ShowDialog();

        }
        protected void Performance_Start()
        {
            CurrentTime = DateTime.Now;
           
        
        }
        protected float Performance_Stop(string nameOfTestcase)
        {
            DateTime now = DateTime.Now;
            TimeSpan ts = now - CurrentTime;

            System.Diagnostics.Debug.WriteLine("--Duration for " + nameOfTestcase + " : " + ts.TotalMilliseconds.ToString() + " - miliseconds");
            return Convert.ToSingle(Convert.ToSingle(ts.TotalMilliseconds/1000));

        }
    }
}
