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
    public class ICPTest5_Cube_ExpectedError : TestBaseICP
    {


        
        [Test]
        public void Cube_Translate_Horn_TreeRednaxela_Error()
        {
            //gives NAN
            //possible to the current implementaiton: 
            //Problem is that the (possible Horn Method has problems with some translations
            //the problem is solved, if one first resets the vectors to the origin by:
            //this.icp.ICPSettings.ResetVector3ToOrigin = true;


           
            this.icp.Settings_Reset_GeometricObject();
            
            IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = false;
            

            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Horn;


            meanDistance = ICPTestData.Test5_CubeTranslation2(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult, cubeSizeX);

            this.ShowResultsInWindow_Cube(true);

            CheckResult_MeanDistance(this.threshold);
        }
      
      
        [Test]
        public void Cube_ScaleInhomogenous_Horn()
        {
           
            this.icp.Settings_Reset_GeometricObject();
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Horn;
            meanDistance = ICPTestData.Test5_CubeScale_Inhomogenous(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);
           

            this.ShowResultsInWindow_CubeLines(true);

            CheckResult_MeanDistance(this.threshold);
        }

        [Test]
        public void Cube_Shuffle()
        {
            //this testcase works if ShuffleEffect = true
            //the UI is OK, but only the meanDistance is bad
           
            this.icp.Settings_Reset_GeometricObject();
            IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = false;
            IterativeClosestPointTransform.Instance.ICPSettings.ShuffleEffect = false;

            meanDistance = ICPTestData.Test5_Cube8Shuffle_60000(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult, cubeSizeX);

            this.ShowResultsInWindow_Cube(true);

            CheckResult_MeanDistance(this.threshold);
        }

        [Test]
        public void Cube_RotateShuffle()
        {
            //this testcase works if ShuffleEffect = true
            
           
            this.icp.Settings_Reset_GeometricObject();
            IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = false;
            IterativeClosestPointTransform.Instance.ICPSettings.ShuffleEffect = false;


            meanDistance = ICPTestData.Test5_Cube8RotateShuffle(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult, cubeSizeX);

            this.ShowResultsInWindow_Cube(true);

            CheckResult_MeanDistance(this.threshold);
        }
        [Test]
        public void Cube_Rotate45Shuffle_Horn()
        {
           
            this.icp.Settings_Reset_GeometricObject();
            IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = false;
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Horn;
            IterativeClosestPointTransform.Instance.ICPSettings.MaximumNumberOfIterations = 50;


            meanDistance = ICPTestData.Test5_CubeRotate45(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult, cubeSizeX);


            this.ShowResultsInWindow_Cube(true );

            CheckResult_MeanDistance(this.threshold);
        }
        [Test]
        public void Cube_TranslateRotateShuffle_TreeRednaxela()
        {
            //gives NAN
           
            this.icp.Settings_Reset_GeometricObject();
            IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = false;
            
            
            IterativeClosestPointTransform.Instance.ICPSettings.MaximumNumberOfIterations = 50;


            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Umeyama;


            meanDistance = ICPTestData.Test5_Cube8TranslateRotateShuffleNew(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult, cubeSizeX);

            this.ShowResultsInWindow_Cube(true);

            CheckResult_MeanDistance(this.threshold);
        }
        [Test]
        public void Cube_TranslateRotateShuffle_TreeStark()
        {
           
            this.icp.Settings_Reset_GeometricObject();
            IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = false;
            
            IterativeClosestPointTransform.Instance.ICPSettings.MaximumNumberOfIterations = 50;


            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Horn;


            meanDistance = ICPTestData.Test5_Cube8TranslateRotateShuffleNew(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult, cubeSizeX);

            this.ShowResultsInWindow_Cube(true);

            CheckResult_MeanDistance(this.threshold);
        }
     
    }
}
