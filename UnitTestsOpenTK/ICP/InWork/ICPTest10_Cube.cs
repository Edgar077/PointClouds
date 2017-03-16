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
    public class ICPTest10_Cube : TestBaseICP
    {
        [Test]
        public void Cube_98Points_Rotate_Umeyama_Normals()
        {
           
            this.icp.Settings_Reset_GeometricObject();
            IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = false;
            this.icp.ICPSettings.ResetVector3ToOrigin = true;
            this.icp.ICPSettings.Normal_RemovePoints = true;
            this.icp.ICPSettings.Normal_SortPoints = true;
            this.icp.ICPSettings.ShuffleEffect = false;

            
            this.icp.ICPSettings.MaximumNumberOfIterations = 50;

         
            meanDistance = ICPTestData.Test10_Cube98p_Rotate(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult, cubeSizeX);
            this.ShowResultsInWindow_Cube(true);


            CheckResult_MeanDistance(1e-7f);
        }

        [Test]
        public void Cube26_TranslateRotateScaleShuffle()
        {
           
            this.icp.Settings_Reset_GeometricObject();
            IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = false;
            this.icp.ICPSettings.ResetVector3ToOrigin = true;
            this.icp.ICPSettings.Normal_RemovePoints = true;
            this.icp.ICPSettings.MaximumNumberOfIterations = 50;


            meanDistance = ICPTestData.Test10_Cube26pRotateTranslateScaleShuffle(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult, cubeSizeX);
            this.ShowResultsInWindow_Cube(true);


            CheckResult_MeanDistance(1e-7f);
        }
        [Test]
        public void Cube26_RotateShuffle()
        {
           
            this.icp.Settings_Reset_GeometricObject();
            IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = false;


            meanDistance = ICPTestData.Test10_Cube26p_RotateShuffle(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult, cubeSizeX);

            this.ShowResultsInWindow_Cube(true);


            CheckResult_MeanDistance(1e-7f);
        }
    

        [Test]
        public void Cube8_TranslateRotateScaleShuffle()
        {
           
            this.icp.Settings_Reset_GeometricObject();
            IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = false;
            this.icp.ICPSettings.ResetVector3ToOrigin = true;

         
            meanDistance = ICPTestData.Test10_Cube8pRotateTranslateScaleShuffle(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult, cubeSizeX);
            this.ShowResultsInWindow_Cube(true);


            CheckResult_MeanDistance(1e-7f);
        }


        [Test]
        public void Cube8_TranslateRotateShuffle()
        {
           
            this.icp.Settings_Reset_GeometricObject();
            IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = false;


            meanDistance = ICPTestData.Test10_Cube8pRotateTranslateShuffle(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult, cubeSizeX);
            this.ShowResultsInWindow_Cube(true);


            CheckResult_MeanDistance(1e-7f);
        }


     
    
        
     
        [Test]
        public void Cube8_RotateShuffle()
        {
           
            this.icp.Settings_Reset_GeometricObject();
            IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = false;


            meanDistance = ICPTestData.Test10_Cube8pRotateShuffle(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult, cubeSizeX);

            this.ShowResultsInWindow_Cube(true);


            CheckResult_MeanDistance(1e-7f);
        }

      
 
        
     
    }
}
