using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKExtension;
using UnitTestsOpenTK;
using ICPLib;


namespace Automated.ICP
{
    [TestFixture]
    [Category("UnitTest")]
    public class Scaling : TestBaseICP
    {
         
       
        [Test]
        public void Scale_Horn()
        {
           
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Horn;
            IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = true;
            meanDistance = ICPTestData.Test3_Scale(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);

            //this.ShowResultsInWindowIncludingLines(false);
            Assert.IsTrue(this.threshold > meanDistance);
            Performance_Stop("Scale_Horn");
        }
        [Test]
        public void Scale_Umeyama()
        {
           
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Umeyama;
            IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = true;
            meanDistance = ICPTestData.Test3_Scale(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);

            Performance_Stop("Scale_Umeyama");//7 miliseconds on i3_2121 (3.3 GHz)
            Assert.IsTrue(this.threshold > meanDistance);

        }
        [Test]
        public void Scale_Zinsser()
        {
           
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Zinsser;
            IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = true;
            meanDistance = ICPTestData.Test3_Scale(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);

            Assert.IsTrue(this.threshold > meanDistance);
        }
        [Test]
        public void Scale_Du()
        {
           
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Du;
            IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = true;
            meanDistance = ICPTestData.Test3_Scale(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);

            Assert.IsTrue(this.threshold > meanDistance);
        }
        [Test]
        public void Scale_AllAxes_Du()
        {
           

           
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Du;
            IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = true;
            meanDistance = ICPTestData.Test3_Scale(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);

            Assert.IsTrue(this.threshold > meanDistance);
        }
        
      
    }
}
