using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKExtension;
using OpenTK;
using UnitTestsOpenTK;

namespace Automated.PrincipalComponentAnalysis
{
    [TestFixture]
    [Category("UnitTest")]
    public class Face_14000 : PCABase
    {

        public Face_14000()
        {
            UIMode = false;
        }
     
        [Test]
        public void ShowAxes_AlignedToOriginAxes()
        {
           

            Model myModel = new Model(pathUnitTests + "\\KinectFace_1_15000.obj");
            this.pointCloudSource = myModel.PointCloud;

            
            //PointCloud.ResizeVerticesTo1(pointCloudSource);
            pca.PCA_OfPointCloud(pointCloudSource);
            pointCloudSource = PCA.RotateToOriginAxes(pointCloudSource);

            myModel.PointCloud = pointCloudSource;


            //-----------Show in Window
            if (UIMode)
            {
                this.ShowModel(myModel);
               
                
            }

            //----------------check Result
           
            expectedResultCloud.AddVector(new Vector3(1, 0, 0));
            expectedResultCloud.AddVector(new Vector3(0, 1, 0));
            expectedResultCloud.AddVector(new Vector3(0, 0, 1));

            double executionTime = Performance_Stop("Execution Time");//on i3_2121 (3.3 GHz)
            Assert.IsTrue(executionTime < 0.3);

            float meanDistance = PointCloud.MeanDistance(expectedResultCloud, pointCloudSource.PCAAxesNormalized);
            Assert.IsTrue(this.threshold > meanDistance);

        }
        [Test]
        public void AlignPCAToOriginAxes_SecondCloud()
        {
          
            Model model3DTarget = new Model(pathUnitTests + "\\KinectFace_2_15000.obj");
            
            this.pointCloudSource = model3DTarget.PointCloud;


            pointCloudResult = PCA.RotateToOriginAxes(pointCloudSource);


            //-----------Show in Window
            if (UIMode)
            {
                Show4PointCloudsInWindow(true);
                //ShowResultsInWindow_Cube(true);
            }
            
            expectedResultCloud.AddVector(new Vector3(1, 0, 0));
            expectedResultCloud.AddVector(new Vector3(0, 1, 0));
            expectedResultCloud.AddVector(new Vector3(0, 0, 1));

            //----------------check Result
            double executionTime = Performance_Stop("Execution Time");//on i3_2121 (3.3 GHz)
            //bool condition = true;
            Assert.LessOrEqual(executionTime, 0.25);

            float meanDistance = PointCloud.MeanDistance(expectedResultCloud, pointCloudSource.PCAAxesNormalized);
        
            Assert.IsTrue(this.threshold > meanDistance);

        }
        [Test]
        public void AlignPCAToOriginAxes()
        {
            Model model3DTarget = new Model(pathUnitTests + "\\KinectFace_1_15000.obj");
            this.pointCloudSource = model3DTarget.PointCloud;
            
        
            pointCloudResult = PCA.RotateToOriginAxes(pointCloudSource);


            //-----------Show in Window
            if (UIMode)
            {
                Show4PointCloudsInWindow(true);
                //ShowResultsInWindow_Cube(true);
            }
           
            expectedResultCloud.AddVector(new Vector3(1, 0, 0));
            expectedResultCloud.AddVector(new Vector3(0, 1, 0));
            expectedResultCloud.AddVector(new Vector3(0, 0, 1));

            //----------------check Result
            double executionTime = Performance_Stop("Execution Time");//on i3_2121 (3.3 GHz)
            Assert.IsTrue(executionTime < 3);
            float meanDistance = PointCloud.MeanDistance(expectedResultCloud, pointCloudSource.PCAAxesNormalized);
            Assert.IsTrue(this.threshold > meanDistance);


        }

        [Test]
        public void Translate()
        {
           
            Model myModel = new Model(pathUnitTests + "\\KinectFace_1_15000.obj");
            this.pointCloudTarget = myModel.PointCloud;

            this.pointCloudSource = PointCloud.CloneAll(pointCloudTarget);
            PointCloud.Translate(pointCloudSource, 0, -300, 0);
            //PointCloud.Translate(pointCloudSource, 10, -40, 40);


            this.pointCloudResult = pca.AlignPointClouds_SVD( this.pointCloudSource, this.pointCloudTarget);
            //-----------Show in Window

            double executionTime = Performance_Stop("Execution Time");//on i3_2121 (3.3 GHz)
            Assert.IsTrue(executionTime < 3);
            CheckResultTargetAndShow_Cloud(this.threshold);


        }
        [Test]
        public void Rotate()
        {
         
            Model model3DTarget = new Model(pathUnitTests + "\\KinectFace_1_15000.obj");
            this.pointCloudTarget = model3DTarget.PointCloud;
            
            this.pointCloudSource = PointCloud.CloneAll(pointCloudTarget);
            PointCloud.RotateDegrees(pointCloudSource, 25, 90, 25);


            this.pointCloudResult = pca.AlignPointClouds_SVD(this.pointCloudSource, this.pointCloudTarget);
            double executionTime = Performance_Stop("Execution Time");//on i3_2121 (3.3 GHz)
            Assert.IsTrue(executionTime < 3);
            CheckResultTargetAndShow_Cloud(this.threshold);

        }
        [Test]
        public void Rotate2()
        {

    
            Model m = new Model(pathUnitTests + "\\KinectFace_1_15000.obj");
            

            Model model3DTarget = new Model(pathUnitTests + "\\KinectFace_1_15000.obj");
            this.pointCloudTarget = model3DTarget.PointCloud;

            this.pointCloudSource = PointCloud.CloneAll(pointCloudTarget);
            PointCloud.RotateDegrees(pointCloudSource, 60, 60, 0);


            this.pointCloudResult = pca.AlignPointClouds_SVD(this.pointCloudSource, this.pointCloudTarget);
            double executionTime = Performance_Stop("Execution Time");//on i3_2121 (3.3 GHz)
            Assert.IsTrue(executionTime < 3);
            CheckResultTargetAndShow_Cloud(this.threshold);
        }
      
   
        [Test]
        public void Scale()
        {

         
            pointCloudTarget = PointCloud.FromObjFile(pathUnitTests + "\\KinectFace_1_15000.obj");
            
         
            this.pointCloudSource = PointCloud.CloneAll(pointCloudTarget);
            
            PointCloud.ScaleByFactor(pointCloudSource, 0.78f);

            this.pointCloudResult = pca.AlignPointClouds_SVD(this.pointCloudSource, this.pointCloudTarget);

            double executionTime = Performance_Stop("Execution Time");//on i3_2121 (3.3 GHz)
            Assert.IsTrue(executionTime < 3);
            CheckResultTargetAndShow_Cloud(this.threshold);


        }

       


        [Test]
        public void TranslateRotate()
        {

       
            Model model3DTarget = new Model(pathUnitTests + "\\KinectFace_1_15000.obj");
            this.pointCloudTarget = model3DTarget.PointCloud;


            this.pointCloudSource = PointCloud.CloneAll(pointCloudTarget);
            PointCloud.RotateDegrees(pointCloudSource, 60, 60, 90);
            PointCloud.Translate(pointCloudSource, 10, -40, 40);


            this.pointCloudResult = pca.AlignPointClouds_SVD( this.pointCloudSource, this.pointCloudTarget);

            double executionTime = Performance_Stop("Execution Time");//on i3_2121 (3.3 GHz)
            Assert.IsTrue(executionTime < 1);
            CheckResultTargetAndShow_Cloud(this.threshold);


        }
    }
}
