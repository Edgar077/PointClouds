using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKExtension;
using ICPLib;


namespace UnitTestsOpenTK.UI
{
    [TestFixture]
    [Category("UnitTest")]
    public class ICPTest6_Bunny : TestBaseICP
    {
        
        
        [Test]
        public void Horn()
        {
           
            this.icp.Settings_Reset_RealData();

            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Horn;
            
            meanDistance = ICPTestData.Test6_Bunny(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);

            Show3PointCloudsInWindow(true);

            CheckResult_MeanDistance(1e-3f);
           
        }
        [Test]
        public void Umeyama()
        {

           
            this.icp.Settings_Reset_RealData();
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Umeyama;
            
            meanDistance = ICPTestData.Test6_Bunny(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);
            Show3PointCloudsInWindow(true);

            CheckResult_MeanDistance(1e-3f);


        }
        [Test]
        public void Du()
        {

           
            this.icp.Settings_Reset_RealData();
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Du;
            
            meanDistance = ICPTestData.Test6_Bunny(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);

            Show3PointCloudsInWindow(true);

            CheckResult_MeanDistance(1e-3f);

        }
        [Test]
        public void Zinsser()
        {

           
            this.icp.Settings_Reset_RealData();
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Zinsser;
            

            meanDistance = ICPTestData.Test6_Bunny(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);

            Show3PointCloudsInWindow(true);
            CheckResult_MeanDistance(1e-3f);

        }
     
     
    }
}
