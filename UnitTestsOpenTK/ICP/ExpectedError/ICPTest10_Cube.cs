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
    public class ICPTest10_Cube : TestBaseICP
    {

        [Test]
        public void Cube_125Points_Rotate_Umeyama()
        {
           
            this.icp.Settings_Reset_GeometricObject();
            
            this.icp.ICPSettings.FixedTestPoints = false;
            
            this.icp.ICPSettings.ResetVector3ToOrigin = true;
            IterativeClosestPointTransform.Instance.ICPSettings.MaximumNumberOfIterations = 50;


            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Umeyama;

         
            meanDistance = ICPTestData.Test10_Cube98p_Rotate(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult, cubeSizeX);

            this.ShowResultsInWindow_Cube(false);

            CheckResult_MeanDistance(this.threshold);
        }

     
     
        
     
    }
}
