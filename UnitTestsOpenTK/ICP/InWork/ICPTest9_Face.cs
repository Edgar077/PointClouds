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
    public class ICPTest9_Face : TestBaseICP
    {
        
        [Test]
        public void Du()
        {
           
            this.icp.Reset_RealData();
            IterativeClosestPointTransform.Instance.ICPSettings.MaximumNumberOfIterations = 100;
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Du;
           

            meanDistance = ICPTestData.Test9_Face_Stitch(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);

            Show3PointCloudsInWindow(true);
            Assert.IsTrue(1e-3f > PointCloud.MeanDistance(pointCloudTarget, pointCloudResult));

        }
          [Test]
        public void Umeyama_SA()
        {
           
            this.icp.Reset_RealData();

            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Umeyama;
            IterativeClosestPointTransform.Instance.ICPSettings.SimulatedAnnealing = true;


            meanDistance = ICPTestData.Test9_Face_Stitch(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);

            Show3PointCloudsInWindow(true);
            Assert.IsTrue(1e-3f > PointCloud.MeanDistance(pointCloudTarget, pointCloudResult));

        }
          
     
    }
}
