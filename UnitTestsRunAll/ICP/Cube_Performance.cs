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
    public class Cube_Performance : TestBaseICP
    {
             
      
        [Test]
        public void Cube_Shuffle_60000p()
        {
           
           
            this.icp.Settings_Reset_GeometricObject();
            IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = false;
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Umeyama;
            IterativeClosestPointTransform.Instance.ICPSettings.MaximumNumberOfIterations = 50;

            meanDistance = ICPTestData.Test5_Cube8Shuffle_60000(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult, cubeSizeX);



            CheckResult_MeanDistance(this.threshold);
            
            double executionTime = Performance_Stop("ICP_Cube_Shuffle_60000");
            //1.4 seconds on i3_2121 (3.3 GHz)
            //0.5 s on i7_6700 - 3.4 GHz
            System.Diagnostics.Debug.WriteLine("Execution time is: " + executionTime.ToString());
            Assert.IsTrue(executionTime < 0.5);


        }
        [Test]
        public void Cube_Shuffle_1MilionPoints()
        {
           
            this.icp.Settings_Reset_GeometricObject();
            IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = false;
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Umeyama;
            IterativeClosestPointTransform.Instance.ICPSettings.MaximumNumberOfIterations = 3;
            IterativeClosestPointTransform.Instance.ICPSettings.ThresholdConvergence = Convert.ToSingle(1E-2);


            meanDistance = ICPTestData.Test5_Cube8Shuffle_1Milion(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult, cubeSizeX);

            
            CheckResult_MeanDistance(Convert.ToSingle(1e-2));
            double executionTime = Performance_Stop("ICP_Cube_Shuffle_1Million");
            //66 - 71 seconds on i3_2121 (3.3 GHz)
            //7 s on i7_6700 - 3.4 GHz
            System.Diagnostics.Debug.WriteLine("Execution time is: " + executionTime.ToString());
            Assert.IsTrue(executionTime < 8);

         
        }
     
    }
}
