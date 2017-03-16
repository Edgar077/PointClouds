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
           
            this.icp.Settings_Reset_RealData();

            Model model3DTarget = new Model(pathUnitTests + "\\2.obj");
            this.pointCloudTarget = model3DTarget.PointCloud;
            pointCloudTarget = PCA.RotateToOriginAxes(pointCloudTarget);

            Model model3DSource = new Model(pathUnitTests + "\\1.obj");
            this.pointCloudSource = model3DSource.PointCloud;
            pointCloudSource = PCA.RotateToOriginAxes(pointCloudSource);

            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Zinsser;
            this.pointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(pointCloudSource, pointCloudTarget);

            Show3PointCloudsInWindow(true);
            Assert.IsTrue(1e-3f > PointCloud.MeanDistance(pointCloudTarget, pointCloudResult));

       
        }
        [Test]
        public void Person_TwoClouds_ICP()
        {
           
            this.icp.Settings_Reset_RealData();

            Model model3DTarget = new Model(pathUnitTests + "\\2.obj");
            this.pointCloudTarget = model3DTarget.PointCloud;
            //pointCloudTarget = PCA.RotateToOriginAxes(pointCloudTarget);

            Model model3DSource = new Model(pathUnitTests + "\\1.obj");
            this.pointCloudSource = model3DSource.PointCloud;


            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Zinsser;
            this.pointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(pointCloudSource, pointCloudTarget);

            Show3PointCloudsInWindow(true);
            Assert.IsTrue(1e-3f > PointCloud.MeanDistance(pointCloudTarget, pointCloudResult));


        }
        [Test]
        public void Person_ICP()
        {
           
            this.icp.Settings_Reset_RealData();

            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Zinsser;

            meanDistance = ICPTestData.Test11_Person(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);

            Show3PointCloudsInWindow(true);
            Assert.IsTrue(1e-3f > PointCloud.MeanDistance(pointCloudTarget, pointCloudResult));

        }
        [Test]
        public void Person_PCA_ICP()
        {
           
            this.icp.Settings_Reset_RealData();

            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Zinsser;

            meanDistance = ICPTestData.Test11_Person(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);

            Show3PointCloudsInWindow(true);
            Assert.IsTrue(1e-3f > PointCloud.MeanDistance(pointCloudTarget, pointCloudResult));

        }
        [Test]
        public void Person_PCA()
        {
            Model model3DTarget = new Model(pathUnitTests + "\\2.obj");
            this.pointCloudTarget = model3DTarget.PointCloud;
            
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
            Model model3DTarget = new Model(pathUnitTests + "\\2.obj");
            this.pointCloudTarget = model3DTarget.PointCloud;
           
            Model model3DSource = new Model(pathUnitTests + "\\1.obj");
            this.pointCloudSource = model3DSource.PointCloud;
           
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
            Model model3DTarget = new Model(pathUnitTests + "\\2.obj");
            this.pointCloudTarget = model3DTarget.PointCloud;
            pointCloudTarget = PCA.RotateToOriginAxes(pointCloudTarget);

            Model model3DSource = new Model(pathUnitTests + "\\1.obj");
            this.pointCloudSource = model3DSource.PointCloud;
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
