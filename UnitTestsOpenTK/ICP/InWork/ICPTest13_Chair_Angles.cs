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
    public class ICPTest14_Chair_Angles : TestBaseICP
    {

       
       
        [Test]
        public void Chair_Angles()
        {
           
            this.icp.Reset_RealData();

            Model model3DTarget = new Model(pathUnitTests + "\\G1.obj");
            this.pointCloudTarget = model3DTarget.PointCloud;
            pointCloudTarget = PCA.RotateToOriginAxes(pointCloudTarget);

            Model model3DSource = new Model(pathUnitTests + "\\G2.obj");
            this.pointCloudSource = model3DSource.PointCloud;
            pointCloudSource = PCA.RotateToOriginAxes(pointCloudSource);

            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Zinsser;
            IterativeClosestPointTransform.Instance.ICPSettings.MaximumNumberOfIterations = 5;
            //IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = true;

            this.pointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(pointCloudSource, pointCloudTarget);

            this.pointCloudResult.ToObjFile(pathUnitTests, "Result.obj");
            IterativeClosestPointTransform.Instance.PMerged.ToObjFile(pathUnitTests, "Result_Merged.obj");
            Show3PointCloudsInWindow(true);
            Assert.IsTrue(1e-3f > PointCloud.MeanDistance(pointCloudTarget, pointCloudResult));

       
        }
      
    }
}
