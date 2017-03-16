using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKExtension;
using UnitTestsOpenTK;
using ICPLib;


namespace UnitTestsOpenTK.UI
{
    [TestFixture]
    [Category("UnitTest")]
    public class ICPTest8_RealSense : TestBaseICP
    {
        

     
        [Test]
        public void Stitch1()
        {
           
            this.icp.Settings_Reset_RealData();

            Model model3DTarget = new Model(pathUnitTests + "\\Stitch1\\0.obj");
            this.pointCloudTarget = model3DTarget.PointCloud;
            pointCloudTarget = PCA.RotateToOriginAxes(pointCloudTarget);

            Model model3DSource = new Model(pathUnitTests + "\\Stitch1\\1.obj");
            this.pointCloudSource = model3DSource.PointCloud;
            pointCloudSource = PCA.RotateToOriginAxes(pointCloudSource);

            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Zinsser;
            this.pointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(pointCloudSource, pointCloudTarget);

            this.pointCloudResult.ToObjFile(pathUnitTests + "\\Stitch1\\", "result.obj");


            Show3PointCloudsInWindow(true);
            Assert.IsTrue(1e-3f > PointCloud.MeanDistance(pointCloudTarget, pointCloudResult));


        }
     
     
    }
}
