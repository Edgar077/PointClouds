using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKExtension;
using OpenTK;
using UnitTestsOpenTK;

namespace ToDo.ICP
{
    [TestFixture]
    [Category("UnitTest")]
    public class Cuboid : TestBaseICP
    {



        [Test]
        public void Cuboid_8_FindItself()
        {
            CreateCuboid(2, 4, 1);
            this.pointCloudResult = icp.PerformICP(this.pointCloudSource, this.pointCloudTarget);

            Assert.IsTrue(icp.MeanDistance < 1e-6f);

            Show3PointCloudsInWindow(true);
            //ShowPointCloudInWindow(this.pointCloudTarget);
        }

      
        [Test]
        public void Cuboid_8_Rotate_45minus()
        {
            CreateCuboid(2, 4, 1);
            this.pointCloudSource.RotateDegrees(0, -45, 0);

            icp.ICPSettings.MaximumNumberOfIterations = 5;
            this.pointCloudResult = icp.PerformICP(this.pointCloudSource, this.pointCloudTarget);
            PointCloud.SetIndicesForCubeCorners(this.pointCloudResult);

            Assert.IsTrue(icp.MeanDistance < 1e-6f);



        }
        [Test]
        public void Cuboid_8_Rotate_45()
        {
            CreateCuboid(2, 4, 1);
            this.pointCloudSource.RotateDegrees(0, 45, 0);

            icp.ICPSettings.MaximumNumberOfIterations = 5;
            this.pointCloudResult = icp.PerformICP(this.pointCloudSource, this.pointCloudTarget);
            PointCloud.SetIndicesForCubeCorners(this.pointCloudResult);

            Assert.IsTrue(icp.MeanDistance < 1e-6f);



        }
        [Test]
        public void Cuboid_8_Rotate_90()
        {
            CreateCuboid(2, 4, 1);
            this.pointCloudSource.RotateDegrees(0, 90, 0);


            icp.ICPSettings.MaximumNumberOfIterations = 4;
            this.pointCloudResult = icp.PerformICP(this.pointCloudSource, this.pointCloudTarget);
            PointCloud.SetIndicesForCubeCorners(this.pointCloudResult);

            Show3PointCloudsInWindow(true);



        }
        [Test]
        public void Cuboid_8_Rotate_90_TakenAlgorithm()
        {
            CreateCuboid(2, 4, 1);
            this.pointCloudSource.RotateDegrees(0, 90, 0);


            icp.ICPSettings.MaximumNumberOfIterations = 4;
            icp.TakenAlgorithm = true;
            this.pointCloudResult = icp.PerformICP(this.pointCloudSource, this.pointCloudTarget);
            PointCloud.SetIndicesForCubeCorners(this.pointCloudResult);

            Show3PointCloudsInWindow(true);



        }
        [Test]
        public void Cuboid_8_Rotate_90_AddPoints()
        {
            //CreateCuboid_AddPoints(2, 4, 1);
            this.pointCloudSource.RotateDegrees(0, 90, 0);

            icp.TakenAlgorithm = true;
            icp.ICPSettings.MaximumNumberOfIterations = 4;
            this.pointCloudResult = icp.PerformICP(this.pointCloudSource, this.pointCloudTarget);
            PointCloud.SetIndicesForCubeCorners(this.pointCloudResult);

            Show3PointCloudsInWindow(true);



        }
          [Test]
        public void Cuboid_26_Rotate90()
        {

            this.pointCloudTarget = ExamplePointClouds.CuboidEmpty(2, 4, 1, 2, 2, 2);

            this.pointCloudSource = pointCloudTarget.Clone();
            //CreateCuboid_AddPoints(2, 4, 1);
            this.pointCloudSource.RotateDegrees(0, 90, 0);

            icp.TakenAlgorithm = true;
            icp.ICPSettings.MaximumNumberOfIterations = 10;
            this.pointCloudResult = icp.PerformICP(this.pointCloudSource, this.pointCloudTarget);

            Assert.IsTrue(icp.MeanDistance < 1e-6f);
            //Show3PointCloudsInWindow(true);



        }
   
          [Test]
          public void Cuboid_56_Rotate90_Taken()
          {

              this.pointCloudTarget = ExamplePointClouds.CuboidEmpty(2, 4, 1, 3, 3, 3);
              this.pointCloudTarget.SetColor(new Vector3(0, 1, 0));

              this.pointCloudSource = pointCloudTarget.Clone();
              this.pointCloudSource.SetColor(new Vector3(1, 1, 1));

              //CreateCuboid_AddPoints(2, 4, 1);
              this.pointCloudSource.RotateDegrees(0, 90, 0);

              icp.TakenAlgorithm = true;
              icp.ICPSettings.MaximumNumberOfIterations = 100;
              this.pointCloudResult = icp.PerformICP(this.pointCloudSource, this.pointCloudTarget);

              Assert.IsTrue(icp.MeanDistance < 1e-6f);
              Show3PointCloudsInWindow(false);
                     
          }
        

          [Test]
          public void Cuboid_98_Rotate90_Taken()
          {

              this.pointCloudTarget = ExamplePointClouds.CuboidEmpty(2, 4, 1, 4, 4, 4);
              this.pointCloudTarget.SetColor(new Vector3(0, 1, 0));

              //ExamplePointClouds.CuboidEmpty(2, 4, 1, 3, 3, 3);

              this.pointCloudSource = pointCloudTarget.Clone();
              this.pointCloudSource.SetColor(new Vector3(1, 1, 1));

              //CreateCuboid_AddPoints(2, 4, 1);
              this.pointCloudSource.RotateDegrees(0, 90, 0);

              icp.TakenAlgorithm = true;
              icp.ICPSettings.MaximumNumberOfIterations = 100;
              this.pointCloudResult = icp.PerformICP(this.pointCloudSource, this.pointCloudTarget);

              Assert.IsTrue(icp.MeanDistance < 1e-6f);

              Show3PointCloudsInWindow(false);



          }
          [Test]
          public void Cuboid_600_Rotate90_Taken_NoScaling()
          {

              this.pointCloudTarget = ExamplePointClouds.CuboidEmpty(2, 4, 1, 10, 10, 10);
              this.pointCloudTarget.SetColor(new Vector3(0, 1, 0));

              this.pointCloudSource = pointCloudTarget.Clone();
              this.pointCloudSource.SetColor(new Vector3(1, 1, 1));

             
              this.pointCloudSource.RotateDegrees(0, 90, 0);

              icp.ICPSettings.ICPVersion = ICP_VersionUsed.NoScaling;
              icp.TakenAlgorithm = true;
              icp.ICPSettings.MaximumNumberOfIterations = 500;
              this.pointCloudResult = icp.PerformICP(this.pointCloudSource, this.pointCloudTarget);

              Assert.IsTrue(icp.MeanDistance < 1e-6f);
              //Show3PointCloudsInWindow(false);

          }
          [Test]
          public void Cuboid_2400_Rotate90_Taken_NoScaling()
          {

              this.pointCloudTarget = ExamplePointClouds.CuboidEmpty(2, 4, 1, 20, 20, 20);
              this.pointCloudTarget.SetColor(new Vector3(0, 1, 0));

              this.pointCloudSource = pointCloudTarget.Clone();
              this.pointCloudSource.SetColor(new Vector3(1, 1, 1));


              this.pointCloudSource.RotateDegrees(0, 90, 0);

              icp.ICPSettings.ICPVersion = ICP_VersionUsed.NoScaling;
              icp.TakenAlgorithm = true;
              icp.ICPSettings.MaximumNumberOfIterations = 500;
              this.pointCloudResult = icp.PerformICP(this.pointCloudSource, this.pointCloudTarget);

              //Assert.IsTrue(icp.MeanDistance < 1e-6f);
              Show3PointCloudsInWindow(false);

          }

    }
}
