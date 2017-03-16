using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKExtension;
using ICPLib;

namespace UnitTestsOpenTK.InWork
{
    [TestFixture]
    [Category("UnitTest")]
    public class ICPTest5_CubeInhomogenous : TestBaseICP
    {
         
      
        [Test]
        public void Prespective_Horn()
        {
           
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Horn;
            IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = true;
            meanDistance = ICPTestData.Test9_Inhomogenous(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);

            this.ShowResultsInWindow_CubeLines(false);
            CheckResult_MeanDistance(1e-3f);

            
        }
        [Test]
        public void Prespective_Du()
        {
           
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Du;
            IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = true;
            meanDistance = ICPTestData.Test9_Inhomogenous(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);

            this.ShowResultsInWindow_CubeLines(false);
            CheckResult_MeanDistance(1e-3f);
        }
        [Test]
        public void Prespective_Umeyama()
        {
           
            
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Umeyama;
            IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = true;
            meanDistance = ICPTestData.Test9_Inhomogenous(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);

            this.ShowResultsInWindow_CubeLines(false);
            CheckResult_MeanDistance(1e-7f);
            
        }
        [Test]
        public void Prespective_Zinsser()
        {
           
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Zinsser;
            IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = true;
            meanDistance = ICPTestData.Test9_Inhomogenous(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);

            this.ShowResultsInWindow_CubeLines(false);
            CheckResult_MeanDistance(1e-3f);
        }
       
    }
}
