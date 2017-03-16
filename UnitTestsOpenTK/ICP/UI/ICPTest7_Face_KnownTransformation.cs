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
    public class ICPTest7_Face_KnownTransformation : TestBaseICP
    {
        

        [Test]
        public void Horn()
        {
           
            this.icp.Settings_Reset_RealData();

            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Horn;
            
            IterativeClosestPointTransform.Instance.ICPSettings.MaximumNumberOfIterations = 100;
            meanDistance = ICPTestData.Test7_Face_KnownTransformation_15000(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);

            Show3PointCloudsInWindow(false);
            CheckResult_MeanDistance(1e-3f);
           
        }
    
        [Test]
        public void Du()
        {

           
            this.icp.Settings_Reset_RealData();
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Du;
           
            meanDistance = ICPTestData.Test7_Face_KnownTransformation_15000(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);
            
            Show3PointCloudsInWindow(false);
            CheckResult_MeanDistance(1e-3f);

        }
        [Test]
        public void Zinsser()
        {
           
            this.icp.Settings_Reset_RealData();
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Zinsser;
           
            meanDistance = ICPTestData.Test7_Face_KnownTransformation_15000(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);
            Show3PointCloudsInWindow(false);
            CheckResult_MeanDistance(1e-7f);

        }
        [Test]
        public void Umeyama()
        {
           
            this.icp.Settings_Reset_RealData();

            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Umeyama;
            meanDistance = ICPTestData.Test7_Face_KnownTransformation_15000(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);

            Show3PointCloudsInWindow(false);
            CheckResult_MeanDistance(1e-7f);

        }
     
    }
}
