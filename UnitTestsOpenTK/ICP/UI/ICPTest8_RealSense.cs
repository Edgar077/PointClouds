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
           
            this.icp.Reset_RealData();

          
            this.pointCloudTarget = new PointCloud(pathUnitTests + "\\Stitch1\\0.obj");
            pointCloudTarget = PCA.RotateToOriginAxes(pointCloudTarget);

      
            this.pointCloudSource = new PointCloud(pathUnitTests + "\\Stitch1\\1.obj");
            pointCloudSource = PCA.RotateToOriginAxes(pointCloudSource);

            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Zinsser;
            this.pointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(pointCloudSource, pointCloudTarget);

            this.pointCloudResult.ToObjFile(pathUnitTests + "\\Stitch1\\", "result.obj");


            Show3PointCloudsInWindow(true);
            Assert.IsTrue(1e-3f > PointCloud.MeanDistance(pointCloudTarget, pointCloudResult));


        }
     
     
    }
}
