using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKExtension;
using OpenTK;

using UnitTestsOpenTK;

namespace UnitTestsOpenTK.PrincipalComponentAnalysis
{
    [TestFixture]
    [Category("UnitTest")]
    public class Cube : PCABase
    {
      

        [Test]
        public void Cuboid_VectorInWork()
        {

            CreateCube();

            PointCloud.RotateDegrees(pointCloudSource, 45, 98, 124);
            

            this.pointCloudResult = pca.AlignPointClouds_OneVector(pointCloudSource, pointCloudTarget, 0, 0);
            this.pointCloudResult = pca.AlignPointClouds_OneVector(pointCloudResult, pointCloudTarget, 1, 1);
            this.pointCloudResult = pca.AlignPointClouds_OneVector(pointCloudResult, pointCloudTarget, 2, 2);

            CheckResultTargetAndShow_Cube();
            

        }
      
    

       

        [Test]
        public void Cube_NotWorking()
        {
            
           
            this.pointCloudTarget = PointCloud.CreateCube_RegularGrid_Empty(cubeSizeX, 1);
            this.pointCloudSource = PointCloud.CloneAll(pointCloudTarget);

            PointCloud.RotateDegrees(pointCloudSource, 45, 45, 45);


            this.pointCloudResult = pca.AlignPointClouds_SVD(this.pointCloudSource, this.pointCloudTarget);
                      

            this.ShowResultsInWindow_Cube(true);
        

        }
        [Test]
        public void Cuboid_TranslateScale_SVD_NotWorking()
        {

            CreateCube();
            PointCloud.ScaleByFactor(pointCloudSource, 0.4f);

            PointCloud.Translate(pointCloudSource, 3, 2, 5);
            

            this.pointCloudResult = pca.AlignPointClouds_SVD(pointCloudSource, pointCloudTarget);
            //this.pointCloudSource = this.pointCloudResult;
            //this.pointCloudResult = pca.AlignPointClouds_SVD(pointCloudSource, pointCloudTarget);

            

            this.ShowResultsInWindow_Cube(true);

        }


        [Test]
        public void Cuboid_Rotate_SVD_NotWorking()
        {

            CreateCube();
            PointCloud.RotateDegrees(pointCloudSource, 45, 123, -321);
           

            this.pointCloudResult = pca.AlignPointClouds_SVD(pointCloudSource, pointCloudTarget);
            //this.pointCloudResult = pca.AlignPointClouds_SVD(pointCloudResult, pointCloudTarget);

            

            this.ShowResultsInWindow_Cube(true);

        }
        [Test]
        public void Cuboid_TranslateRotateScale_SVD_NotWorking()
        {

            CreateCube();

            PointCloud.RotateDegrees(pointCloudSource, 45, 127, 287);
            PointCloud.Translate(pointCloudSource, 3, 2, 5);
            PointCloud.ScaleByFactor(pointCloudSource, 0.4f);

            this.pointCloudResult = pca.AlignPointClouds_SVD(pointCloudSource, pointCloudTarget);

            CheckResultTargetAndShow_Cube();



        }

     
        
      

    }
}
