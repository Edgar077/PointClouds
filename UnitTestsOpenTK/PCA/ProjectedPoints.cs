using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKExtension;
using OpenTK;

namespace UnitTestsOpenTK.PrincipalComponentAnalysis
{
    [TestFixture]
    [Category("UnitTest")]
    public class ProjectedPoints : PCABase
    {

      
      
        [Test]
        public void CuboidRotated_Projected()
        {
            
            float cubeSizeY = 2;
            int numberOfPoints = 3;

            Model myModel = Example3DModels.Cuboid("Cuboid", cubeSizeX, cubeSizeY, numberOfPoints, System.Drawing.Color.White, null);


            this.pointCloudSource = myModel.PointCloud;
            PointCloud.RotateDegrees(pointCloudSource, 45, 45, 45);

            this.pointCloudAddition1 = PointCloud.CloneAll(pointCloudSource);

            pca.PCA_OfPointCloud(pointCloudSource);

            this.pointCloudSource = PointCloud.FromListVector3(pca.PointsResult0);
            this.pointCloudTarget = PointCloud.FromListVector3(pca.PointsResult1);
            this.pointCloudResult = PointCloud.FromListVector3(pca.PointsResult2);

            //Show4PointCloudsInWindow(true);
            this.ShowResultsInWindow_Cube_ProjectedPoints(true);

        }
        [Test]
        public void Cuboid_RotateCenter_Projected()
        {
            
            float cubeSizeY = 2;
            int numberOfPoints = 3;

            Model myModel = Example3DModels.Cuboid("Cuboid", cubeSizeX, cubeSizeY, numberOfPoints, System.Drawing.Color.White, null);


            this.pointCloudSource = myModel.PointCloud;
            pointCloudSource.ResizeVerticesTo1();
            PointCloud.RotateDegrees(pointCloudSource, 45, 45, 45);

            this.pointCloudAddition1 = PointCloud.CloneAll(pointCloudSource);

            pca.PCA_OfPointCloud(pointCloudSource);

            this.pointCloudSource = PointCloud.FromListVector3(pca.PointsResult0);
            this.pointCloudTarget = PointCloud.FromListVector3(pca.PointsResult1);
            this.pointCloudResult = PointCloud.FromListVector3(pca.PointsResult2);

            //Show4PointCloudsInWindow(true);
            
            this.ShowResultsInWindow_Cube_ProjectedPoints(true);

        }
       
     
      
        [Test]
        public void Cube_Projected()
        {
           
            this.pointCloudSource = PointCloud.CreateCube_RegularGrid_Empty(cubeSizeX, 1);
            this.pointCloudAddition1 = pointCloudSource;

            pca.PCA_OfPointCloud(pointCloudSource);

            this.pointCloudSource = PointCloud.FromListVector3(pca.PointsResult0);
            this.pointCloudTarget = PointCloud.FromListVector3(pca.PointsResult1);
            this.pointCloudResult = PointCloud.FromListVector3(pca.PointsResult2);

            

            //Show4PointCloudsInWindow(true);
            
            this.ShowResultsInWindow_Cube_ProjectedPoints(true);

            //Show4PointCloudsInWindow(true);

        }
      
     
      

        [Test]
        public void CubeRotated()
        {
            

            this.pointCloudSource = PointCloud.CreateCube_RegularGrid_Empty(cubeSizeX, 1);
            PointCloud.RotateDegrees(pointCloudSource, 45, 45, 45);

            this.pointCloudAddition1 = PointCloud.CloneAll(pointCloudSource);

            pca.PCA_OfPointCloud(pointCloudSource);
            

            this.pointCloudSource = PointCloud.FromListVector3(pca.PointsResult0);
            this.pointCloudTarget = PointCloud.FromListVector3(pca.PointsResult1);
            this.pointCloudResult = PointCloud.FromListVector3(pca.PointsResult2);

            //Show4PointCloudsInWindow(true);
            this.ShowResultsInWindow_Cube_ProjectedPoints(true);

        }

        [Test]
        public void CubeRotated_X()
        {
            this.pointCloudSource = PointCloud.CreateCube_RegularGrid_Empty(cubeSizeX, 1);
            PointCloud.RotateDegrees(pointCloudSource, 45, 0, 0);

            this.pointCloudAddition1 = PointCloud.CloneAll(pointCloudSource);

            pca.PCA_OfPointCloud(pointCloudSource);
          

            this.pointCloudSource = PointCloud.FromListVector3(pca.PointsResult0);
            this.pointCloudTarget = PointCloud.FromListVector3(pca.PointsResult1);
            this.pointCloudResult = PointCloud.FromListVector3(pca.PointsResult2);

            //Show4PointCloudsInWindow(true);
            this.ShowResultsInWindow_Cube_ProjectedPoints(true);

        }
    
       
        [Test]
        public void CuboidRotated_X()
        {
            
            float cubeSizeY = 2;
            int numberOfPoints = 3;

            Model myModel = Example3DModels.Cuboid("Cuboid", cubeSizeX, cubeSizeY, numberOfPoints, System.Drawing.Color.White, null);

            
            this.pointCloudSource = myModel.PointCloud;
            PointCloud.RotateDegrees(pointCloudSource, 45, 0, 0);


            pca.PCA_OfPointCloud(pointCloudSource);
            

            this.pointCloudSource = PointCloud.FromListVector3(pca.PointsResult0);
            this.pointCloudTarget = PointCloud.FromListVector3(pca.PointsResult1);
            this.pointCloudResult = PointCloud.FromListVector3(pca.PointsResult2);

            //Show4PointCloudsInWindow(true);
            this.ShowResultsInWindow_Cube_ProjectedPoints(true);

        }
     

        [Test]
        public void Cube_ProjectedPoints()
        {
           
            this.pointCloudSource = PointCloud.CreateCube_RegularGrid_Empty(cubeSizeX, 1);
            this.pointCloudAddition1 = PointCloud.CloneAll(pointCloudSource);

            pca.PCA_OfPointCloud(pointCloudSource);
           
            this.pointCloudSource = PointCloud.FromListVector3(pca.PointsResult0);
            this.pointCloudTarget = PointCloud.FromListVector3(pca.PointsResult1);
            this.pointCloudResult = PointCloud.FromListVector3(pca.PointsResult2);

            this.ShowResultsInWindow_Cube_ProjectedPoints(true);

            //Show4PointCloudsInWindow(true);

        }
       

   
     
        [Test]
        public void Face_TranslateRotateScale2()
        {


            Model model3DTarget = new Model(pathUnitTests + "\\KinectFace_1_15000.obj");
            this.pointCloudSource = model3DTarget.PointCloud;

            PointCloud.RotateDegrees(pointCloudSource, 60, 60, 90);
            PointCloud.ScaleByFactor(pointCloudSource, 0.9f);
            PointCloud.Translate(pointCloudSource, 0.3f, 0.5f, -0.4f);

            this.pointCloudAddition1 = pointCloudSource;

            pca.PCA_OfPointCloud(pointCloudSource);
            

            this.pointCloudSource = PointCloud.FromListVector3(pca.PointsResult0);
            this.pointCloudTarget = PointCloud.FromListVector3(pca.PointsResult1);
            this.pointCloudResult = PointCloud.FromListVector3(pca.PointsResult2);

            Show4PointCloudsInWindow(true);

        }


        [Test]
        public void Face()
        {


            Model model3DTarget = new Model(pathUnitTests + "\\KinectFace_1_15000.obj");
            this.pointCloudSource = model3DTarget.PointCloud;
            this.pointCloudAddition1 = pointCloudSource;

            pca.PCA_OfPointCloud(pointCloudSource);
            



            this.pointCloudSource = PointCloud.FromListVector3(pca.PointsResult0);
            this.pointCloudTarget = PointCloud.FromListVector3(pca.PointsResult1);
            this.pointCloudResult = PointCloud.FromListVector3(pca.PointsResult2);

            Show4PointCloudsInWindow(true);

        }

        [Test]
        public void Face_TranslateRotateScale()
        {


            Model model3DTarget = new Model(pathUnitTests + "\\KinectFace_1_15000.obj");
            this.pointCloudSource = model3DTarget.PointCloud;
            PointCloud.RotateDegrees(pointCloudSource, 45, 45, 45);
            //Vertices.RotateVertices(pointCloudSource, 60, 60, 90);
            PointCloud.ScaleByFactor(pointCloudSource, 0.9f);
            PointCloud.Translate(pointCloudSource, 0.3f, 0.5f, -0.4f);

            this.pointCloudAddition1 = pointCloudSource;

            pca.PCA_OfPointCloud(pointCloudSource);


            this.pointCloudSource = PointCloud.FromListVector3(pca.PointsResult0);
            this.pointCloudTarget = PointCloud.FromListVector3(pca.PointsResult1);
            this.pointCloudResult = PointCloud.FromListVector3(pca.PointsResult2);

            Show4PointCloudsInWindow(true);

        }

        [Test]
        public void Person()
        {


            Model model3DTarget = new Model(pathUnitTests + "\\1.obj");
            this.pointCloudSource = model3DTarget.PointCloud;


            this.pointCloudAddition1 = pointCloudSource;

            pca.PCA_OfPointCloud(pointCloudSource);

            this.pointCloudSource = PointCloud.FromListVector3(pca.PointsResult0);
            this.pointCloudTarget = PointCloud.FromListVector3(pca.PointsResult1);
            this.pointCloudResult = PointCloud.FromListVector3(pca.PointsResult2);

            Show4PointCloudsInWindow(true);

        }
        [Test]
        public void Person_Rotate_Projected()
        {


            Model model3DTarget = new Model(pathUnitTests + "\\1.obj");
            this.pointCloudSource = model3DTarget.PointCloud;
            PointCloud.RotateDegrees(pointCloudSource, 25, 90, 25);


            this.pointCloudAddition1 = pointCloudSource;

            pca.PCA_OfPointCloud(pointCloudSource);

            this.pointCloudSource = PointCloud.FromListVector3(pca.PointsResult0);
            this.pointCloudTarget = PointCloud.FromListVector3(pca.PointsResult1);
            this.pointCloudResult = PointCloud.FromListVector3(pca.PointsResult2);

            Show4PointCloudsInWindow(true);

        }
     
    

    }
}
