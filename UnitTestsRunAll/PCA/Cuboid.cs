using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKExtension;
using OpenTK;
using UnitTestsOpenTK;

namespace Automated.PrincipalComponentAnalysis
{
    [TestFixture]
    [Category("UnitTest")]
    public class Cuboid : PCABase
    {
        public Cuboid()
        {
            UIMode = false;
        }
        [Test]
        public void PCA_Axes()
        {
       
            

            //this.pointCloudSource = Example3DModels.CreateCube_RegularGrid_Empty(1);
            this.pointCloudSource = PointCloud.CreateCube_Corners_CenteredAt0(cubeSizeX);

            pointCloudSource.ResizeVerticesTo1();
            pca.PCA_OfPointCloud(pointCloudSource);

            //-----------Show in Window
            if (UIMode)
            {
                //
                //ShowResultsInWindow_Cube(true);
                //-----------Show in Window
               
                this.ShowResultsInWindow_CubeNew(true, true);
                
            }

            expectedResultCloud.AddVector(new Vector3(1, 0, 0));
            expectedResultCloud.AddVector(new Vector3(0, 1, 0));
            expectedResultCloud.AddVector(new Vector3(0, 0, 1));


           
            Assert.IsTrue(1e-3f > PointCloud.MeanDistance(expectedResultCloud, pointCloudSource.PCAAxesNormalized));
        }
        [Test]
        public void PCA_Axes_Translated()
        {
          
         
            

            this.pointCloudSource = PointCloud.CreateCube_RegularGrid_Empty(1f, 1);
            PointCloud.Translate(pointCloudSource, 2, 4, 3);
            pca.PCA_OfPointCloud(pointCloudSource);

            //----------------check Result
            //expectedResultCloud.Add(new Vector3(-0.5f, 0, 0));
            //expectedResultCloud.Add(new Vector3(0, -0.5f, 0));
            //expectedResultCloud.Add(new Vector3(0, 0, -0.5f));

            expectedResultCloud.AddVector(new Vector3(0.5f, 0, 0));
            expectedResultCloud.AddVector(new Vector3(0, 0.5f, 0));
            expectedResultCloud.AddVector(new Vector3(0, 0, 0.5f));

            //-----------Show in Window
            if (UIMode)
            {
                
                ShowResultsInWindow_Cube(true);
            }

            Assert.IsTrue(1e-3f > PointCloud.MeanDistance(expectedResultCloud, pointCloudSource.PCAAxes));
               
            
        }
     
        [Test]
        public void Translate()
        {

            CreateCube();
            PointCloud.Translate(pointCloudSource, 3, 2, 5);
            

            this.pointCloudResult = pca.AlignPointClouds_SVD(pointCloudSource, pointCloudTarget);
           
            CheckResultTargetAndShow_Cube();
        }

    
        [Test]
        public void Scale()
        {
            CreateCube();
            PointCloud.ScaleByFactor(pointCloudSource, 0.4f);
           
            this.pointCloudResult = pca.AlignPointClouds_SVD(pointCloudSource, pointCloudTarget);
           
            CheckResultTargetAndShow_Cube();
        }
        [Test]
        public void Rotate()
        {
            CreateCube();
            //pointCloudTarget.ResizeVerticesTo1();
            this.pointCloudSource = PointCloud.CloneAll(pointCloudTarget);
            
            PointCloud.RotateDegrees(pointCloudSource, 45, 0, 0);
           
            this.pointCloudResult = pca.AlignPointClouds_SVD(pointCloudSource, pointCloudTarget);
            
            CheckResultTargetAndShow_Cube();

        }
        [Test]
        public void TranslateRotate()
        {
            CreateCube();
            PointCloud.RotateDegrees(pointCloudSource, 45, 0, 0);
            PointCloud.Translate(pointCloudSource, 3, 2, 5);
            this.pointCloudResult = pca.AlignPointClouds_SVD(pointCloudSource, pointCloudTarget);
            
            
            CheckResultTargetAndShow_Cube();



        }
       
     
        [Test]
        public void AlignToItself()
        {
            CreateCube();

            this.pointCloudResult = pca.AlignPointClouds_SVD(pointCloudSource, pointCloudTarget);
            
            CheckResultTargetAndShow_Cube();

        }
       
     
      
      

    }
}
