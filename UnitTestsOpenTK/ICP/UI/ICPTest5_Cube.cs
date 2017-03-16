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
    public class ICPTest5_Cube : TestBaseICP
    {
             
        [Test]
        public void Cube_Translate()
        {
           
            this.icp.Settings_Reset_GeometricObject();

            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Horn;
            

            meanDistance = ICPTestData.Test5_CubeTranslation(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult, 50);
          
            this.ShowResultsInWindow_CubeLines(false);

            Assert.IsTrue(this.threshold > meanDistance);
        }
        [Test]
        public void Cube_Translate_Horn_TreeRednaxela_OK()
        {

           
            this.icp.Settings_Reset_GeometricObject();
            this.icp.ICPSettings.FixedTestPoints = false;
            
            this.icp.ICPSettings.ResetVector3ToOrigin = true;
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Horn;


            meanDistance = ICPTestData.Test5_CubeTranslation2(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult, cubeSizeX);

            this.ShowResultsInWindow_Cube(true);

            CheckResult_MeanDistance(this.threshold);
        }
        [Test]
        public void Cube_Rotate()
        {
           
            this.icp.Settings_Reset_GeometricObject();

            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Horn;
            

            meanDistance = ICPTestData.Test5_CubeRotate(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);

            this.ShowResultsInWindow_CubeLines(false);

            Assert.IsTrue(this.threshold > meanDistance);
        }
        [Test]
        public void Cube_Scale_Uniform()
        {
           
            this.icp.Settings_Reset_GeometricObject();

            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Horn;
           

            meanDistance = ICPTestData.Test5_CubeScale_Uniform(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);


            this.ShowResultsInWindow_CubeLines(false);

            Assert.IsTrue(this.threshold > meanDistance);
        }
        [Test]
        public void Cube_ScaleInhomogenous_Du()
        {
           
            this.icp.Settings_Reset_GeometricObject();

            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Du;
            
            meanDistance = ICPTestData.Test5_CubeScale_Inhomogenous(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);
           

            this.ShowResultsInWindow_CubeLines(false);
            //
            Assert.IsTrue(this.threshold > meanDistance);
        }

        [Test]
        public void Cube_RotateTranslate_ScaleUniform_Umeyama()
        {
           
            this.icp.Settings_Reset_GeometricObject();

            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Umeyama;
            
            meanDistance = ICPTestData.Test5_CubeRotateTranslate_ScaleUniform(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);


            this.ShowResultsInWindow_CubeLines(false);
            //
            Assert.IsTrue(this.threshold > meanDistance);
        }
        [Test]
        public void Cube_RotateTranslate_ScaleUniform_Du()
        {
           
            this.icp.Settings_Reset_GeometricObject();
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Du;
            
            meanDistance = ICPTestData.Test5_CubeRotateTranslate_ScaleUniform(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);


            this.ShowResultsInWindow_CubeLines(false);
            //
            Assert.IsTrue(this.threshold > meanDistance);
        }
        [Test]
        public void Cube_RotateTranslate_ScaleInhomegenous_Du()
        {
           
            this.icp.Settings_Reset_GeometricObject();

            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Du;
           
            meanDistance = ICPTestData.Test5_CubeRotateTranslate_ScaleInhomogenous(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);


            this.ShowResultsInWindow_CubeLines(false);
            //
            Assert.IsTrue(this.threshold > meanDistance);
        }
        [Test]
        public void Cube_Shuffle()
        {
           
            this.icp.Settings_Reset_GeometricObject();
            IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = false;
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Umeyama;
            IterativeClosestPointTransform.Instance.ICPSettings.MaximumNumberOfIterations = 50;

            meanDistance = ICPTestData.Test5_Cube8Shuffle_60000(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult, cubeSizeX);

            this.ShowResultsInWindow_Cube(true);

            CheckResult_MeanDistance(this.threshold);
        }
        [Test]
        public void Cube_RotateShuffle_Horn()
        {
           
            this.icp.Settings_Reset_GeometricObject();
            IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = false;
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Horn;


            meanDistance = ICPTestData.Test5_Cube8RotateShuffle(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult, cubeSizeX);

            this.ShowResultsInWindow_Cube(true);

            CheckResult_MeanDistance(this.threshold);
        }
        [Test]
        public void Cube_RotateShuffle_Umeyama()
        {
           
            this.icp.Settings_Reset_GeometricObject();
            IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = false;
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Umeyama;


            meanDistance = ICPTestData.Test5_Cube8RotateShuffle(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult, cubeSizeX);

            this.ShowResultsInWindow_Cube(true);

            CheckResult_MeanDistance(this.threshold);
        }
 
     
    }
}
