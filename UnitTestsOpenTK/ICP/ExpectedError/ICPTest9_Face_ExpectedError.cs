using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKExtension;
using UnitTestsOpenTK;
using ICPLib;


namespace UnitTestsOpenTK.ExpectedError
{
    [TestFixture]
    [Category("UnitTest")]
    public class ICPTest9_Face_ExpectedError : TestBaseICP
    {
        

        [Test]
        public void Horn()
        {
           
            this.icp.Reset_RealData();
            //as it does not converge anyway, keep the iteration number low
            IterativeClosestPointTransform.Instance.ICPSettings.MaximumNumberOfIterations = 10;

            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Horn;
            
           
            meanDistance = ICPTestData.Test9_Face_Stitch(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);

            Show3PointCloudsInWindow(true);
            Assert.IsTrue(1e-3f > PointCloud.MeanDistance(pointCloudTarget, pointCloudResult));
           
        }
        [Test]
        public void Umeyama()
        {
           
            this.icp.Reset_RealData();
            
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Umeyama;
           
            
            meanDistance = ICPTestData.Test9_Face_Stitch(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);

            Show3PointCloudsInWindow(true);
            Assert.IsTrue(1e-3f > PointCloud.MeanDistance(pointCloudTarget, pointCloudResult));

        }
      
          
     
    }
}
