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
    public class AlignTwoDifferentScans : PCABase
    {

        //Model3D model3DTarget = new PointCloud 3D(path + "\\KinectFace_1_15000.obj");

        [Test]
        public void Faces_SVD()
        {
         
            this.pointCloudTarget = new PointCloud(pathUnitTests + "\\KinectFace_2_15000.obj");
            //PointCloud.ResizeVerticesTo1(pointCloudTarget);
            
            this.pointCloudSource = new PointCloud(pathUnitTests + "\\KinectFace_1_15000.obj");
            //PointCloud.ResizeVerticesTo1(pointCloudSource);

            pca.MaxmimumIterations = 5;
            this.pointCloudResult = pca.AlignPointClouds_SVD(this.pointCloudSource, this.pointCloudTarget);

            CheckResultTargetAndShow_Cloud(this.threshold);

        }
        [Test]
        public void Faces_AlignToOriginAxes()
        {
           
            this.pointCloudTarget = new PointCloud(pathUnitTests + "\\KinectFace_2_15000.obj");
            pointCloudTarget = PCA.RotateToOriginAxes(pointCloudTarget);
            //PointCloud.ResizeVerticesTo1(pointCloudTarget);
          
            this.pointCloudSource = new PointCloud(pathUnitTests + "\\KinectFace_1_15000.obj");
            pointCloudSource = PCA.RotateToOriginAxes(pointCloudSource);
            PointCloud.RotateDegrees(pointCloudSource, 90, 0, 0);

            //PointCloud.ResizeVerticesTo1(pointCloudSource);
            pointCloudResult = null;
            pca.MaxmimumIterations = 5;
            //this.pointCloudResult = pca.AlignPointClouds_SVD(this.pointCloudSource, this.pointCloudTarget);

            CheckResultTargetAndShow_Cloud(this.threshold);

        }

        [Test]
        public void Persons_SVD()
        {
            
            this.pointCloudTarget = new PointCloud(pathUnitTests + "\\2.obj");

        
            this.pointCloudSource = new PointCloud(pathUnitTests + "\\1.obj");

            this.pointCloudResult = pca.AlignPointClouds_SVD(this.pointCloudSource, this.pointCloudTarget);

            CheckResultTargetAndShow_Cloud(this.threshold);

        }
        [Test]
        public void Persons_SVD_XYZ()
        {
         
            this.pointCloudTarget = new PointCloud(pathUnitTests + "\\2.obj");
            pointCloudTarget = PCA.RotateToOriginAxes(pointCloudTarget);

        
            this.pointCloudSource = new PointCloud(pathUnitTests + "\\1.obj");
            pointCloudSource = PCA.RotateToOriginAxes(pointCloudSource);
            pca.MaxmimumIterations = 1;
            this.pointCloudResult = pca.AlignPointClouds_SVD(this.pointCloudSource, this.pointCloudTarget);

            CheckResultTargetAndShow_Cloud(this.threshold);

        }
        [Test]
        public void Persons_V()
        {
       
            this.pointCloudTarget = new PointCloud(pathUnitTests + "\\2.obj");

         
            this.pointCloudSource = new PointCloud(pathUnitTests + "\\1.obj");

            this.pointCloudResult = pca.AlignPointClouds_OneVector(this.pointCloudSource, this.pointCloudTarget, 0, 0);





            Assert.IsTrue(this.threshold > pca.MeanDistance);
            if(UIMode)
                ShowPointCloudsInWindow_PCAVectors(true);
        }
      
        [Test]
        public void Persons_V_4()
        {
         
            this.pointCloudTarget = new PointCloud(pathUnitTests + "\\2.obj");

            this.pointCloudSource = new PointCloud(pathUnitTests + "\\1.obj");


            this.pointCloudResult = pca.AlignPointClouds_OneVector(this.pointCloudSource, this.pointCloudTarget, 0, 0);
            this.pointCloudSource = PointCloud.CloneAll(pointCloudResult);
            this.pointCloudResult = pca.AlignPointClouds_OneVector(this.pointCloudSource, this.pointCloudTarget, 1, 1);
            this.pointCloudSource = PointCloud.CloneAll(pointCloudResult);
            this.pointCloudResult = pca.AlignPointClouds_OneVector(this.pointCloudSource, this.pointCloudTarget, 2, 2);
            this.pointCloudSource = PointCloud.CloneAll(pointCloudResult);
            this.pointCloudResult = pca.AlignPointClouds_OneVector(this.pointCloudSource, this.pointCloudTarget, 1, 1);
            this.pointCloudSource = PointCloud.CloneAll(pointCloudResult);

            this.pointCloudResult = pca.AlignPointClouds_SVD(this.pointCloudSource, this.pointCloudTarget);
            

            
            ShowPointCloudsInWindow_PCAVectors(true);

            Assert.IsTrue(this.threshold > pca.MeanDistance);

        }

     
      
       
     
        
      

    }
}
