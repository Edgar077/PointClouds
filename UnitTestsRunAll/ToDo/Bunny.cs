using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKExtension;
using UnitTestsOpenTK;
using ICPLib;

using OpenTK;
namespace ToDo.ICP
{
    [TestFixture]
    [Category("UnitTest")]
    public class Bunny : TestBaseICP
    {
        
        
        //private bool LoadObjFiles_ResizeAndSort(string fileName1, string fileName2)
        //{
        //    string fileNameLong = Path + "\\Bunny\\" + fileName1;
        //    pointCloudSource = PointCloud.FromObjFile(fileNameLong);
            
        //    fileNameLong = Path + "\\Bunny\\" + fileName2;
        //    pointCloudTarget = PointCloud.FromObjFile(fileNameLong);

        //    if (pointCloudSource == null || pointCloudTarget == null)
        //        return false;

        //    pointCloudSource = PointCloud.ResizeAndSort_Distance(pointCloudSource);
        //    pointCloudTarget = PointCloud.ResizeAndSort_Distance(pointCloudTarget);

        //    pointCloudSource.SetColor(new Vector3(0, 0.5f, 0.5f));
        //    pointCloudTarget.SetColor(new Vector3(0, 1f, 0f));


        //    icp.ICPSettings.MaximumNumberOfIterations = 50;

        //    return true;
        //}
      
        [Test]
        public void Zinsser_PCA_ICP()
        {
            
            if (!LoadObjFiles_ResizeAndSort("Bunny\\bun000.obj", "Bunny\\bun045.obj", true))
                return;

           

            pointCloudResult = PCA.AlignPointClouds_Simple(pointCloudSource, pointCloudTarget);


            this.pointCloudSource = this.pointCloudResult;

            this.pointCloudResult = icp.PerformICP(this.pointCloudSource, this.pointCloudTarget);
            float m = icp.MeanDistance;


           
            double executionTime = Performance_Stop("Zinsser_15000");//7 miliseconds on i3_2121 (3.3 GHz)
            
            UIMode = true;
            CheckResultTargetAndShow_Cloud(this.threshold, false);

           

            CheckResult_MeanDistance(this.threshold);
            Assert.IsTrue(this.icp.NumberOfIterations <= 43);

            Assert.IsTrue(executionTime < 7);

        }
        [Test]
        public void Bunny00_to_45()
        {

            if (!LoadObjFiles_ResizeAndSort("Bunny\\bun000.obj", "Bunny\\bun045.obj", true))
                return;
       
            this.pointCloudResult = icp.PerformICP(this.pointCloudSource, this.pointCloudTarget);
         
            double executionTime = Performance_Stop("Zinsser_15000");//7 miliseconds on i3_2121 (3.3 GHz)

            UIMode = true;
            string fileNameLong = pathUnitTests + "\\Bunny\\bun00_45.obj";
            pointCloudResult.ToObjFile(fileNameLong);


            CheckResultTargetAndShow_Cloud(this.threshold, false);
           
            CheckResult_MeanDistance(this.threshold);
            
            Assert.IsTrue(executionTime < 7);

        }
     
        [Test]
        public void PCA_00_to_90()
        {
            if (!LoadObjFiles_ResizeAndSort("Bunny\\bun090.obj", "Bunny\\bun00_45.obj", true))
                return;

            //this.pointCloudResult = icp.PerformICP(this.pointCloudSource, this.pointCloudTarget);
            //float m = icp.MeanDistance;
            pointCloudResult = PCA.AlignPointClouds_Simple(pointCloudSource, pointCloudTarget);




            double executionTime = Performance_Stop("ICP-Bunny");//7 miliseconds on i3_2121 (3.3 GHz)

            UIMode = true;

            //save
            string fileNameLong = pathUnitTests + "\\Bunny\\bun00_90.obj";
            pointCloudResult.ToObjFile(fileNameLong);


            CheckResultTargetAndShow_Cloud(this.threshold, false);


            CheckResult_MeanDistance(this.threshold);

            Assert.IsTrue(executionTime < 7);



        }
        [Test]
        public void Show00_to_90()
        {

            if (!LoadObjFiles_ResizeAndSort("Bunny\\bun090.obj", "Bunny\\bun00_45.obj", true))
                return;
            icp.ICPSettings.MaximumNumberOfIterations = 5;
            icp.TakenAlgorithm = true;
            this.pointCloudResult = this.pointCloudTarget;

            

            CheckResultTargetAndShow_Cloud(this.threshold, false);


            CheckResult_MeanDistance(this.threshold);

           
        }
       
     
      
        [Test]
        public void Bunny00_to_90()
        {

            if (!LoadObjFiles_ResizeAndSort("Bunny\\bun090.obj", "Bunny\\bun00_45.obj", true))
                return;

            icp.ICPSettings.MaximumNumberOfIterations = 10;
            icp.TakenAlgorithm = true;
            this.pointCloudResult = icp.PerformICP(this.pointCloudSource, this.pointCloudTarget);

            double executionTime = Performance_Stop("ICP-Bunny");//7 miliseconds on i3_2121 (3.3 GHz)

            UIMode = true;

            //save
            string fileNameLong = pathUnitTests + "\\Bunny\\bun00_90.obj";
            pointCloudResult.ToObjFile(fileNameLong);


            CheckResultTargetAndShow_Cloud(this.threshold, false);


            CheckResult_MeanDistance(this.threshold);

            Assert.IsTrue(executionTime < 7);

        }
        [Test]
        public void Show_90_to_180()
        {

            if (!LoadObjFiles_ResizeAndSort("Bunny\\bun090.obj", "Bunny\\bun180.obj", true))
                return;

            
            icp.TakenAlgorithm = true;
            this.pointCloudSource.RotateDegrees(0, 90, 0);
            this.pointCloudResult = this.pointCloudTarget;


            Show3PointCloudsInWindow(false);

            

        }
        [Test]
        public void Bunny_90_to_180()
        {

            if (!LoadObjFiles_ResizeAndSort("Bunny\\bun090.obj", "Bunny\\bun180.obj", true))
                return;

            
            //icp.ICPSettings.ICPVersion = ICP_VersionUsed.Zinsser;
            icp.ICPSettings.MaximumNumberOfIterations = 100;
            icp.ICPSettings.ICPVersion = ICP_VersionUsed.NoScaling;

            //icp.TakenAlgorithm = true;
            //this.pointCloudSource.RotateDegrees(0, 90, 0);
            this.pointCloudResult = icp.PerformICP(this.pointCloudSource, this.pointCloudTarget);

            //this.pointCloudResult = pointCloudSource;

            double executionTime = Performance_Stop("ICP-Bunny");//7 miliseconds on i3_2121 (3.3 GHz)

            UIMode = true;

            //save
            string fileNameLong = pathUnitTests + "\\Bunny\\bun90_180.obj";
            pointCloudResult.ToObjFile(fileNameLong);

            CheckResultTargetAndShow_Cloud(this.threshold, false);


            CheckResult_MeanDistance(this.threshold);

            Assert.IsTrue(executionTime < 7);

        }
        [Test]
        public void Bunny_90_to_180_Taken()
        {

            if (!LoadObjFiles_ResizeAndSort("Bunny\\bun090.obj", "Bunny\\bun180.obj", true))
                return;
            //this.pointCloudSource.RotateDegrees(0, 90, 0);


            icp.ICPSettings.MaximumNumberOfIterations = 100;
            icp.ICPSettings.ICPVersion = ICP_VersionUsed.NoScaling;
            icp.TakenAlgorithm = true;
            
            this.pointCloudResult = icp.PerformICP(this.pointCloudSource, this.pointCloudTarget);

            
            double executionTime = Performance_Stop("ICP-Bunny");//7 miliseconds on i3_2121 (3.3 GHz)

            UIMode = true;

            //save
            string fileNameLong = pathUnitTests + "\\Bunny\\bun90_180.obj";
            pointCloudResult.ToObjFile(fileNameLong);

            CheckResultTargetAndShow_Cloud(this.threshold, false);


            CheckResult_MeanDistance(this.threshold);

            Assert.IsTrue(executionTime < 7);

        }
    }
}
