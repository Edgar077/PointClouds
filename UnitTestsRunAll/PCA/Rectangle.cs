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
    public class Rectangle : PCABase
    {
        public Rectangle()
        {
            UIMode = false;
        }

    

        [Test]
        public void ShowAxes()
        {

            CreateRectangle();
            pointCloudTarget = null;
            pca.PCA_OfPointCloud(pointCloudSource);

            //-----------Show in Window
            if (UIMode)
            {

                ShowResultsInWindow_Cube(true);
            }
            //----------------check Result
            expectedResultCloud.AddVector(new Vector3(1, 0, 0));
            expectedResultCloud.AddVector(new Vector3(0, 0.5f, 0));
            expectedResultCloud.AddVector(new Vector3(0, 0, 0));

            float meanDistance = PointCloud.MeanDistance(expectedResultCloud, pointCloudSource.PCAAxes);
            Assert.IsTrue(this.threshold > meanDistance);

           
        }
        [Test]
        public void ShowAxes_Rotated()
        {

            CreateRectangle();
            pointCloudTarget = null;
            PointCloud.RotateDegrees(pointCloudSource, 0, 0, -45);

            pca.PCA_OfPointCloud(pointCloudSource);

            //-----------Show in Window
            if (UIMode)
            {

                ShowResultsInWindow_Cube(true);
            }
            System.Diagnostics.Debug.Write(pointCloudSource.PCAAxes.PrintVectors());

            //----------------check Result
            expectedResultCloud.AddVector(new Vector3(-0.707106781186548f, -0.707106781186548f, 0));
            expectedResultCloud.AddVector(new Vector3(-0.353553390593274f, 0.353553390593274f, 0));
            expectedResultCloud.AddVector(new Vector3(0, 0, 0));

            float meanDistance = PointCloud.MeanDistance(expectedResultCloud, pointCloudSource.PCAAxes);
            Assert.IsTrue(this.threshold > meanDistance);
        }
    
        [Test]
        public void ShowAxes_RotateToOriginAxes()
        {

            CreateRectangle();
            pointCloudTarget = null;
            pca.PCA_OfPointCloud(pointCloudSource);

            
            //-----------Show in Window
            if (UIMode)
            {

                ShowResultsInWindow_Cube(true);
            }
            //----------------check Result
            expectedResultCloud.AddVector(new Vector3(1, 0, 0));
            expectedResultCloud.AddVector(new Vector3(0, 0.5f, 0));
            expectedResultCloud.AddVector(new Vector3(0, 0, 0));

            float meanDistance = PointCloud.MeanDistance(expectedResultCloud, pointCloudSource.PCAAxes);

            Assert.IsTrue(this.threshold > meanDistance);


        }
        [Test]
        public void AlignToItself()
        {
            CreateRectangle();
            pointCloudSource.ResizeVerticesTo1();
            pointCloudTarget.ResizeVerticesTo1();

            this.pointCloudResult = pca.AlignPointClouds_SVD(pointCloudSource, pointCloudTarget);

            CheckResultTargetAndShow_Cube();

        }
        [Test]
        public void AlignPCAToOriginAxes()
        {

            CreateRectangle();
            PointCloud.RotateDegrees(pointCloudSource, 0, 0, -45);
            
            pointCloudTarget = null;
            
            pointCloudResult = PCA.RotateToOriginAxes(pointCloudSource);



            //-----------Show in Window
            if (UIMode)
            {
                //Show4PointCloudsInWindow(true);
                ShowResultsInWindow_Cube(true);
            }
            expectedResultCloud.AddVector(new Vector3(1, 0, 0));
            expectedResultCloud.AddVector(new Vector3(0, 1, 0));
            expectedResultCloud.AddVector(new Vector3(0, 0, 0));

            //----------------check Result
            float meanDistance = PointCloud.MeanDistance(expectedResultCloud, pointCloudSource.PCAAxesNormalized);
            Assert.IsTrue(this.threshold > meanDistance);


            //----------------check Result
           
        }
        [Test]
        public void Translate()
        {

            CreateRectangle();
            PointCloud.Translate(pointCloudTarget, -2, 3, -1);
            PointCloud.Translate(pointCloudSource, 3, 2, 5);

            this.pointCloudResult = pca.AlignPointClouds_SVD(pointCloudSource, pointCloudTarget);

            CheckResultTargetAndShow_Cube();
        }

   
        [Test]
        public void Scale()
        {

            CreateRectangle();
            PointCloud.ScaleByFactor(pointCloudSource, 0.8f);

            this.pointCloudResult = pca.AlignPointClouds_SVD(pointCloudSource, pointCloudTarget);

            CheckResultTargetAndShow_Cube();
        }
        [Test]
        public void TranslateRotateScale()
        {

            CreateRectangle();
            PointCloud.Translate(pointCloudSource, 3, 2, 5);
            PointCloud.RotateDegrees(pointCloudSource, 45, 124, 297);
            PointCloud.ScaleByFactor(pointCloudSource, 0.8f);

            this.pointCloudResult = pca.AlignPointClouds_SVD(pointCloudSource, pointCloudTarget);

            CheckResultTargetAndShow_Cube();
        }
     
      
      

    }
}
