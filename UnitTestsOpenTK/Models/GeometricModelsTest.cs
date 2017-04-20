using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using OpenTKExtension;
using OpenTK;

namespace UnitTestsOpenTK.Models
{
    [TestFixture]
    [Category("UnitTest")]
    public class GeometricModelsTest : TestBase
    {

        [Test]
        public void Cuboid_OldControl()
        {
            PointCloud pc = ExamplePointClouds.Cuboid("Cuboid", 20f, 40f, 100, System.Drawing.Color.White, null);
            pc.ResizeVerticesTo1();
            ShowPointCloud(pc);
        }

        [Test]
        public void Cuboid_100()
        {

            this.pointCloudSource = PointCloud.CreateCuboid(20f, 40f, 100);

//            myModel.Pointcloud.ResizeVerticesTo1();
            this.ShowResultsInWindow_Cube(true);
        }

       

        [Test]
        public void Cube_Corners()
        {
            
            List<Vector3> listVectors = ExamplePointClouds.Cuboid_Corners_CenteredAt0(1, 1, 1);
            PointCloud pcl = PointCloud.FromListVector3(listVectors);
            PointCloud.SetIndicesForCubeCorners(pcl);


            ShowPointCloudForOpenGL(pcl);

            
        }
        [Test]
        public void Cube_56()
        {

            
            PointCloud pcl = ExamplePointClouds.Cube_RegularGrid_Empty(1, 3);
            ShowPointCloudForOpenGL(pcl);


        }
        [Test]
        public void Cube_16()
        {
            CreateCube(4);
            this.ShowResultsInWindow_Cube(true);

        }
        [Test]
        public void Cube_32()
        {
            CreateCube(8);
            this.ShowResultsInWindow_Cube(true);

        }

    
       

     
     
    }
}
