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
           

            pointCloudSource = new PointCloud(pathUnitTests + "\\KinectFace_1_15000.obj");
           
            //PointCloud.ResizeVerticesTo1(pointCloudSource);
            pca.PCA_OfPointCloud(pointCloudSource);
            this.pointCloudResult = PCA.RotateToOriginAxes(pointCloudSource);


            //-----------Show in Window
            if (UIMode)
            {
                this.ShowPointCloud(pointCloudSource);
               
                
            }

            //----------------check Result
           
            expectedResultCloud.AddVector(new Vector3(1, 0, 0));
            expectedResultCloud.AddVector(new Vector3(0, 1, 0));
            expectedResultCloud.AddVector(new Vector3(0, 0, 1));

            double executionTime = Performance_Stop("Execution Time");//on i3_2121 (3.3 GHz)
            Assert.IsTrue(executionTime < 0.3);

            Assert.IsTrue(PointCloud.CheckCloudAbs(expectedResultCloud, pointCloudResult.PCAAxesNormalized, this.threshold));

         

        }
        [Test]
        public void AlignPCAToOriginAxes_SecondCloud()
        {
          
      
            
            this.pointCloudSource = new PointCloud(pathUnitTests + "\\KinectFace_2_15000.obj");


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
            Assert.IsTrue(PointCloud.CheckCloudAbs(expectedResultCloud, pointCloudResult.PCAAxesNormalized, this.threshold));

        }
        [Test]
        public void AlignPCAToOriginAxes()
        {
    
            this.pointCloudSource = new PointCloud(pathUnitTests + "\\KinectFace_1_15000.obj");


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
            double executionTime = Performance_Stop("Execution Time");
            Assert.IsTrue(executionTime < 0.4);//on i7 6700 (3.4 GHz, 8 cores)
            Assert.IsTrue(PointCloud.CheckCloudAbs(expectedResultCloud, pointCloudResult.PCAAxesNormalized, this.threshold));


        }

        [Test]
        public void Translate()
        {
           
      
            this.pointCloudTarget = new PointCloud(pathUnitTests + "\\KinectFace_1_15000.obj");

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
      
            this.pointCloudTarget = new PointCloud(pathUnitTests + "\\KinectFace_1_15000.obj");

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

    
            PointCloud m = new PointCloud (pathUnitTests + "\\KinectFace_1_15000.obj");
            

       
            this.pointCloudTarget = new PointCloud(pathUnitTests + "\\KinectFace_1_15000.obj");

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

       
      
            this.pointCloudTarget = new PointCloud(pathUnitTests + "\\KinectFace_1_15000.obj");


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
