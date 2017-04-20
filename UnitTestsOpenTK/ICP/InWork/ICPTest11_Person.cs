using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKExtension;
using UnitTestsOpenTK;
using ICPLib;


namespace UnitTestsOpenTK.InWork
{
    [TestFixture]
    [Category("UnitTest")]
    public class ICPTest11_Person : TestBaseICP
    {

       
       
        [Test]
        public void Person_TwoClouds_PCA_ICP()
        {
           
            this.icp.Reset_RealData();

           
            this.pointCloudTarget = new PointCloud(pathUnitTests + "\\2.obj");
            pointCloudTarget = PCA.RotateToOriginAxes(pointCloudTarget);

         
            this.pointCloudSource = new PointCloud(pathUnitTests + "\\1.obj");
            pointCloudSource = PCA.RotateToOriginAxes(pointCloudSource);

            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Zinsser;
            this.pointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(pointCloudSource, pointCloudTarget);

            Show3PointCloudsInWindow(true);
            Assert.IsTrue(1e-3f > PointCloud.MeanDistance(pointCloudTarget, pointCloudResult));

       
        }
        [Test]
        public void Person_TwoClouds_ICP()
        {
           
            this.icp.Reset_RealData();

           
            this.pointCloudTarget = new PointCloud(pathUnitTests + "\\2.obj");
         
            this.pointCloudSource = new PointCloud(pathUnitTests + "\\1.obj");


            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Zinsser;
            this.pointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(pointCloudSource, pointCloudTarget);

            Show3PointCloudsInWindow(true);
            Assert.IsTrue(1e-3f > PointCloud.MeanDistance(pointCloudTarget, pointCloudResult));


        }
        [Test]
        public void Person_ICP()
        {
           
            this.icp.Reset_RealData();

            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Zinsser;

            meanDistance = ICPTestData.Test11_Person(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);

            Show3PointCloudsInWindow(true);
            Assert.IsTrue(1e-3f > PointCloud.MeanDistance(pointCloudTarget, pointCloudResult));

        }
        [Test]
        public void Person_PCA_ICP()
        {
           
            this.icp.Reset_RealData();

            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Zinsser;

            meanDistance = ICPTestData.Test11_Person(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);

            Show3PointCloudsInWindow(true);
            Assert.IsTrue(1e-3f > PointCloud.MeanDistance(pointCloudTarget, pointCloudResult));

        }
        [Test]
        public void Person_PCA()
        {
          
            this.pointCloudTarget = new PointCloud(pathUnitTests + "\\2.obj");

            this.pointCloudSource = PointCloud.CloneAll(pointCloudTarget);
            PointCloud.RotateDegrees(pointCloudSource, 25, 10, 25);

            
            PCA pca = new PCA();
            this.pointCloudResult = pca.AlignPointClouds_OneVector(this.pointCloudSource, this.pointCloudTarget, 0, 0);
            this.pointCloudResult = pca.AlignPointClouds_OneVector(this.pointCloudResult, this.pointCloudTarget, 1, 1);
            this.pointCloudResult = pca.AlignPointClouds_OneVector(this.pointCloudResult, this.pointCloudTarget, 2, 2);
            this.pointCloudResult = pca.AlignPointClouds_OneVector(this.pointCloudResult, this.pointCloudTarget, 0, 0);

           
            Show3PointCloudsInWindow(true);
            //ShowPointCloudsInWindow_PCAVectors(true);

            Assert.IsTrue(4e-3f < PointCloud.MeanDistance(pointCloudTarget, pointCloudResult));

        }
        [Test]
        public void Person_PCA_V_TwoClouds()
        {
         
            this.pointCloudTarget = new PointCloud(pathUnitTests + "\\2.obj");
                      
            this.pointCloudSource = new PointCloud(pathUnitTests + "\\1.obj");

            PCA pca = new PCA();
            this.pointCloudResult = pca.AlignPointClouds_OneVector(this.pointCloudSource, this.pointCloudTarget, 0, 0);
            this.pointCloudResult = pca.AlignPointClouds_OneVector(this.pointCloudResult, this.pointCloudTarget, 1, 1);
            this.pointCloudResult = pca.AlignPointClouds_OneVector(this.pointCloudResult, this.pointCloudTarget, 2, 2);
            this.pointCloudResult = pca.AlignPointClouds_OneVector(this.pointCloudResult, this.pointCloudTarget, 0, 0);

            
            Show3PointCloudsInWindow(true);
            //ShowPointCloudsInWindow_PCAVectors(true);

            Assert.IsTrue(this.threshold > meanDistance);

            pointCloudSource = PCA.RotateToOriginAxes(pointCloudSource);

        }
        [Test]
        public void Person_PCA_V_TwoClouds_XYZ()
        {
         
            this.pointCloudTarget = new PointCloud(pathUnitTests + "\\2.obj");
            pointCloudTarget = PCA.RotateToOriginAxes(pointCloudTarget);

       
            this.pointCloudSource = new PointCloud(pathUnitTests + "\\1.obj");
            pointCloudSource = PCA.RotateToOriginAxes(pointCloudSource);

            PCA pca = new PCA();
            this.pointCloudResult = pca.AlignPointClouds_OneVector(this.pointCloudSource, this.pointCloudTarget, 0, 0);
            this.pointCloudResult = pca.AlignPointClouds_OneVector(this.pointCloudResult, this.pointCloudTarget, 1, 1);
            this.pointCloudResult = pca.AlignPointClouds_OneVector(this.pointCloudResult, this.pointCloudTarget, 2, 2);
            this.pointCloudResult = pca.AlignPointClouds_OneVector(this.pointCloudResult, this.pointCloudTarget, 0, 0);


            Show3PointCloudsInWindow(true);
            //ShowPointCloudsInWindow_PCAVectors(true);

            Assert.IsTrue(this.threshold > meanDistance);

            

        }
     
    }
}
