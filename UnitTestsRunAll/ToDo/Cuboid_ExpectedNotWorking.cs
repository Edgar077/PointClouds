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
    public class Cuboid_ExpectedNotWorking : TestBaseICP
    {


        /// <summary>
        /// works by setting TakenAlgorithm
        /// </summary>
        [Test]
        public void Cuboid_56_Rotate90()
        {

            this.pointCloudTarget = ExamplePointClouds.CuboidEmpty(2, 4, 1, 3, 3, 3);
            this.pointCloudTarget.SetColor(new Vector3(0, 1, 0));

            //ExamplePointClouds.CuboidEmpty(2, 4, 1, 3, 3, 3);

            this.pointCloudSource = pointCloudTarget.Clone();
            this.pointCloudSource.SetColor(new Vector3(1, 1, 1));

            //CreateCuboid_AddPoints(2, 4, 1);
            this.pointCloudSource.RotateDegrees(0, 90, 0);


            icp.ICPSettings.MaximumNumberOfIterations = 50;
            this.pointCloudResult = icp.PerformICP(this.pointCloudSource, this.pointCloudTarget);


            Show3PointCloudsInWindow(false);



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
        
        /// <summary>
        /// //works only if the TakenAlgorithm is used
        /// </summary>
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
     
        /// <summary>
        /// only by activating this line it is working:
        //  icp.ICPSettings.ICPVersion = ICP_VersionUsed.NoScaling;
        /// </summary>
          [Test]
          public void Cuboid_10098_Rotate90_Taken()
          {

              this.pointCloudTarget = ExamplePointClouds.CuboidEmpty(2, 4, 1, 10, 10, 10);
              this.pointCloudTarget.SetColor(new Vector3(0, 1, 0));

              
              this.pointCloudSource = pointCloudTarget.Clone();
              this.pointCloudSource.SetColor(new Vector3(1, 1, 1));

             
              this.pointCloudSource.RotateDegrees(0, 90, 0);

              
              icp.TakenAlgorithm = true;
              icp.ICPSettings.MaximumNumberOfIterations = 500;
              this.pointCloudResult = icp.PerformICP(this.pointCloudSource, this.pointCloudTarget);


              Show3PointCloudsInWindow(false);



          }

    }
}
