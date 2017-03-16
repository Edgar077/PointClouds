using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKExtension;


using ICPLib;

namespace UnitTestsOpenTK
{
    [TestFixture]
    [Category("UnitTest")]
    public class KDTreeTest_ICP : TestBaseICP
    {

        public KDTreeTest_ICP()
        {
            pathUnitTests = AppDomain.CurrentDomain.BaseDirectory + "\\Models\\UnitTests";
           
        }


        [Test]
        public void Cube_RotateScaleTranslate_KDTree_Stark()
        {
           
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Du;
            IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = true;
            this.icp.ICPSettings.ResetVector3ToOrigin = true;
            meanDistance = ICPTestData.Test5_CubeRotateTranslate_ScaleUniform(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);


            this.ShowResultsInWindow_CubeLines(false);
            
            Assert.IsTrue(this.threshold > meanDistance);
        }
        [Test]
        public void Cube_RotateScaleTranslate_KDTreeBruteForce()
        {
           
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Du;
            this.icp.ICPSettings.ResetVector3ToOrigin = true;
            meanDistance = ICPTestData.Test5_CubeRotateTranslate_ScaleUniform(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);


            this.ShowResultsInWindow_CubeLines(false);
            //
            Assert.IsTrue(this.threshold > meanDistance);
        }
   
   
    }
}
