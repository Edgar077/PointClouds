using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKExtension;
using ICPLib;
using UnitTestsOpenTK;
using OpenTK;


namespace UnitTestsOpenTK
{
  

    public class TestBaseICP : TestBase
    {
        protected IterativeClosestPointTransform icp ; 
        protected float meanDistance;
       
        
        public TestBaseICP():base()
         {
             icp = new IterativeClosestPointTransform();
             

             pathUnitTests = AppDomain.CurrentDomain.BaseDirectory + "\\Models\\UnitTests";

           

         }
        [SetUp]
        protected override void SetupTest()
        {
            base.SetupTest();
            ResetICP();
        }
      
        protected void ShowResultsInWindow_CubeLines(bool changeColor)
        {

            //color code: 
            //Target is green
            //source : white
            //result : red

            //so - if there is nothing red on the OpenTK control, the result overlaps the target
            PointCloud.SetColorOfListTo(pointCloudTarget, Color.Green);
            PointCloud.SetColorOfListTo(pointCloudSource, Color.White);
            if (pointCloudResult != null)
            {
                PointCloud.SetColorOfListTo(pointCloudResult, Color.Red);
                
            }
           

            TestForm fOTK = new TestForm();
            fOTK.Show3PointClouds(pointCloudSource, pointCloudTarget, pointCloudResult, changeColor);
            fOTK.ShowDialog();


        }
        
         protected void ResetICP()
         {
             
             icp = new IterativeClosestPointTransform();
             this.icp.Reset();
             icp.ICPSettings.ThresholdConvergence = this.threshold;

         }
      
        protected void CheckResult_MeanDistance(float threshold)
         {
             Assert.IsTrue(icp.MeanDistance < threshold);

         }
     
        protected void CheckResult_Vectors()
        {
            Assert.IsTrue(1e-3f > PointCloud.MeanDistance(pointCloudTarget, pointCloudResult));
        }
        protected void CheckResult_VectorsUltra()
        {
            Assert.IsTrue(this.threshold > meanDistance);
        }
        //protected void SettingsRealData()
        //{
        //    icp.ICPSettings.MaximumNumberOfIterations = 10;
        //    IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = false;
        //    this.icp.ICPSettings.Normal_RemovePoints = false;
        //    this.icp.ICPSettings.Normal_SortPoints = false;

        //    this.icp.ICPSettings.ResetVector3ToOrigin = true;
        //    
        //    IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Scaling_Umeyama;

            

        //}
        //protected void SettingsGeometricObjects()
        //{
        //    IterativeClosestPointTransform.Instance.ICPSettings.MaximumNumberOfIterations = 5;
        //    IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = true;
        //    this.icp.ICPSettings.ResetVector3ToOrigin = false;
        //    
        //    this.icp.ICPSettings.Normal_RemovePoints = false;
        //}
        protected void CheckResultTargetAndShow_Cloud(float threshold, bool changeColor)
        {
            //-----------Show in Window
            if (UIMode)
            {
                Show3PointCloudsInWindow(changeColor);
                //ShowPointCloudsInWindow_PCAVectors(true);
            }
            //----------------check Result
            Assert.IsTrue(CheckResult(icp.MeanDistance, 0f, threshold));

        }
        protected bool LoadObjFiles(string fileName1, string fileName2, bool changeColor)
        {
            string fileNameLong = pathUnitTests + "\\" + fileName1;
            pointCloudSource = PointCloud.FromObjFile(fileNameLong);

            fileNameLong = pathUnitTests + "\\" + fileName2;
            pointCloudTarget = PointCloud.FromObjFile(fileNameLong);

            if (pointCloudSource == null || pointCloudTarget == null || pointCloudSource.Count == 0 || pointCloudTarget.Count == 0)
                return false;

            if (changeColor)
            {
                pointCloudSource.SetColor(new Vector3(1, 1, 1));
                pointCloudTarget.SetColor(new Vector3(0, 1f, 0f));
            }

            return true;
        }
        protected bool LoadObjFiles_ResizeAndSort(string fileName1, string fileName2, bool changeColor)
        {

            if (!LoadObjFiles(fileName1, fileName2, changeColor))
                return false;

            
            pointCloudSource = PointCloud.ResizeAndSort_Distance(pointCloudSource);
            pointCloudTarget = PointCloud.ResizeAndSort_Distance(pointCloudTarget);

          



            return true;
        }

    }
}
