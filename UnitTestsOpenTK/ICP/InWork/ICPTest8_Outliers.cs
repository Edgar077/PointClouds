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
    public class ICPTest8_Outliers : TestBaseICP
    {

        [Test]
        public void Outliers_CubeTranslate_FixedPoints()
        {
           
            
            
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Horn;
            IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = true;
         
            IterativeClosestPointTransform.Instance.ICPSettings.MaximumNumberOfIterations = 10;


            meanDistance = ICPTestData.Test8_CubeOutliers_Translate(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);
            this.ShowResultsInWindow_CubeLines(false);

            
            Assert.IsTrue(1e-3f > PointCloud.MeanDistance(pointCloudTarget, pointCloudResult));

        }
        [Test]
        public void Outliers_CubeTranslate_NotGood()
        {
           
           

            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Horn;
            IterativeClosestPointTransform.Instance.ICPSettings.MaximumNumberOfIterations = 10;

            meanDistance = ICPTestData.Test8_CubeOutliers_Translate(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);

            this.ShowResultsInWindow_CubeLines(false);
            Assert.IsTrue(1e-3f > PointCloud.MeanDistance(pointCloudTarget, pointCloudResult));

        }
        [Test]
        public void Outliers_CubeTranslate_DistanceOptimization()
        {
           


            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Horn;
            IterativeClosestPointTransform.Instance.ICPSettings.MaximumNumberOfIterations = 10;
            this.icp.ICPSettings.DistanceOptimization = true;

            meanDistance = ICPTestData.Test8_CubeOutliers_Translate(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);

            this.ShowResultsInWindow_CubeLines(false);
            Assert.IsTrue(1e-3f > PointCloud.MeanDistance(pointCloudTarget, pointCloudResult));

        }
        [Test]
        public void Outliers_CubeTranslate_NormalsCheck()
        {
           
            

            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Horn;
            IterativeClosestPointTransform.Instance.ICPSettings.MaximumNumberOfIterations = 10;
            this.icp.ICPSettings.Normal_RemovePoints = true;

            meanDistance = ICPTestData.Test8_CubeOutliers_Translate(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);

            this.ShowResultsInWindow_CubeLines(false);
            Assert.IsTrue(1e-3f > PointCloud.MeanDistance(pointCloudTarget, pointCloudResult));
            
        }
        [Test]
        public void Face_NormalsCheck()
        {
           

            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Horn;
            
            IterativeClosestPointTransform.Instance.ICPSettings.MaximumNumberOfIterations = 10;
            this.icp.ICPSettings.Normal_RemovePoints = true;

            meanDistance = ICPTestData.Test9_Face_Stitch(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);

            Show3PointCloudsInWindow(false);
            Assert.IsTrue(1e-3f > PointCloud.MeanDistance(pointCloudTarget, pointCloudResult));

        }
        [Test]
        public void Outliers_CubeRotate()
        {
           
      
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Horn;
            IterativeClosestPointTransform.Instance.ICPSettings.MaximumNumberOfIterations = 10;
            this.icp.ICPSettings.SimulatedAnnealing = true;



            meanDistance = ICPTestData.Test8_CubeOutliers_Rotate(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);

            Show3PointCloudsInWindow(true);
            Assert.IsTrue(2e-1f < PointCloud.MeanDistance(pointCloudTarget, pointCloudResult));
           
        }
       
     
     
    }
}
