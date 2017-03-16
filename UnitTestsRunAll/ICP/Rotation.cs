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
    public class Rotation : TestBaseICP
    {

        [Test]
        public void RotationIdentity()
        {
           
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Zinsser;
            IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = true;
            meanDistance = ICPTestData.Test2_Identity(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);


            Assert.IsTrue(this.threshold > meanDistance);
        }
        
        [Test]
        public void RotationX_Horn()
        {
           
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Horn;
            IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = true;
            meanDistance = ICPTestData.Test2_RotationX30Degrees(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);


            Assert.IsTrue(this.threshold > meanDistance);
        }
        
        [Test]
        public void RotationX_Umeyama()
        {
           
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Umeyama;
            IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = true;
            meanDistance = ICPTestData.Test2_RotationX30Degrees(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);

            Assert.IsTrue(this.threshold > meanDistance);
        }
        [Test]
        public void RotationX_Zinsser()
        {
           
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Zinsser;
            IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = true;
            meanDistance = ICPTestData.Test2_RotationX30Degrees(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);

            Assert.IsTrue(this.threshold > meanDistance);
        }
        [Test]
        public void RotationX_Du()
        {
           
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Du;
            IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = true;
            meanDistance = ICPTestData.Test2_RotationX30Degrees(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);

            Assert.IsTrue(this.threshold > meanDistance);
        }
        [Test]
        public void RotationXYZ_Horn()
        {
           
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Horn;
            IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = true;
            meanDistance = ICPTestData.Test2_RotationXYZ(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);

            //this.ShowResultsInWindowIncludingLines(false);
            Assert.IsTrue(this.threshold > meanDistance);
        }
        [Test]
        public void RotationXYZ_Umeyama()
        {
           
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Umeyama;
            IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = true;
            meanDistance = ICPTestData.Test2_RotationXYZ(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);

            Assert.IsTrue(this.threshold > meanDistance);
        }
        [Test]
        public void RotationXYZ_Zinsser()
        {
           
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Zinsser;
            IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = true;
            meanDistance = ICPTestData.Test2_RotationXYZ(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);

            Assert.IsTrue(this.threshold > meanDistance);
        }
        [Test]
        public void RotationXYZ_Du()
        {
           
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Du;
            IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = true;
            meanDistance = ICPTestData.Test2_RotationXYZ(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);

            Assert.IsTrue(this.threshold > meanDistance);
        }
     
    }
}
