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
    public class Cuboid : TestBaseICP
    {
             
      
        [Test]
        public void Cuboid_Rotate()
        {
           
            this.icp.Settings_Reset_GeometricObject();
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Du;

            meanDistance = ICPTestData.Test5_CuboidIdentity(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);

            Assert.IsTrue(this.threshold > meanDistance);
        }
        [Test]
        public void Cuboid_Identity()
        {
           
            this.icp.Settings_Reset_GeometricObject();
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Du;

            meanDistance = ICPTestData.Test5_CuboidIdentity(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);

            Assert.IsTrue(this.threshold > meanDistance);
        }
        [Test]
        public void Cube_RotateTranslate_ScaleUniform_Du()
        {
           
            this.icp.Settings_Reset_GeometricObject();
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Du;

            meanDistance = ICPTestData.Test5_CubeRotateTranslate_ScaleUniform(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);

            Assert.IsTrue(this.threshold > meanDistance);
        }
  
        [Test]
        public void Cube_Shuffle()
        {
           
            this.icp.Settings_Reset_GeometricObject();
            IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = false;
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Umeyama;
            IterativeClosestPointTransform.Instance.ICPSettings.MaximumNumberOfIterations = 50;

            meanDistance = ICPTestData.Test5_Cube8Shuffle(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult, cubeSizeX);

            

            CheckResult_MeanDistance(this.threshold);
        }
    }
}
