using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKExtension;
using UnitTestsOpenTK;
using ICPLib;


namespace UnitTestsOpenTK.InWork
{
    [TestFixture]
    [Category("UnitTest")]
    public class ICPTest5_Cube : TestBaseICP
    {

        
        [Test]
        public void Cube_RotateShuffle_Du()
        {
           
            this.icp.Settings_Reset_GeometricObject();
            IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = false;
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Du;
            IterativeClosestPointTransform.Instance.ICPSettings.MaximumNumberOfIterations = 50;

            meanDistance = ICPTestData.Test5_Cube8RotateShuffle(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult, cubeSizeX);

            this.ShowResultsInWindow_Cube(true);

            CheckResult_MeanDistance(1e-7f);
        }
        [Test]
        public void Cube_RotateShuffle_Umeyama()
        {
           
            this.icp.Settings_Reset_GeometricObject();
            IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = false;
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Umeyama;
            IterativeClosestPointTransform.Instance.ICPSettings.MaximumNumberOfIterations = 50;

            meanDistance = ICPTestData.Test5_Cube8RotateShuffle(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult, cubeSizeX);

            this.ShowResultsInWindow_Cube(true);

            CheckResult_MeanDistance(1e-7f);
        }
         [Test]
        public void Cube_RotateShuffle_Normals_Umeyama()
        {
           
            this.icp.Settings_Reset_GeometricObject();
            IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = false;
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Umeyama;
            IterativeClosestPointTransform.Instance.ICPSettings.MaximumNumberOfIterations = 50;
            IterativeClosestPointTransform.Instance.ICPSettings.Normal_RemovePoints = true;

            meanDistance = ICPTestData.Test5_Cube8RotateShuffle(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult, cubeSizeX);

            this.ShowResultsInWindow_Cube(true);

            CheckResult_MeanDistance(1e-7f);
        }
        [Test]
        public void Cube_Translate_TreeRednaxela()
        {
            //gives NAN
           
            this.icp.Settings_Reset_GeometricObject();
            IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = false;
            IterativeClosestPointTransform.Instance.ICPSettings.MaximumNumberOfIterations = 50;
            
            this.icp.ICPSettings.ResetVector3ToOrigin = true;

            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Horn;


            meanDistance = ICPTestData.Test5_CubeTranslation2(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult, cubeSizeX);

            this.ShowResultsInWindow_Cube(true);

            CheckResult_MeanDistance(1e-7f);
        }
    
      
       
    }
}
