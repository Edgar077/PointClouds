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
    public class CuboidNew : PCABase
    {

       
        [Test]
        public void Axes()
        {
            
            CreateCuboid(1);
            pointCloudTarget = null;
            pca.PCA_OfPointCloud(pointCloudSource);

            //-----------Show in Window
            if (UIMode)
            {

                ShowResultsInWindow_Cube(true);
            }
            //----------------check Result
            expectedResultCloud.AddVector(new Vector3(0.5f, 0, 0));
            expectedResultCloud.AddVector(new Vector3(0, 0.25f, 0));
            expectedResultCloud.AddVector(new Vector3(0, 0, 0.125f));

            float meanDistance = PointCloud.MeanDistance(expectedResultCloud, pointCloudSource.PCAAxes);
            Assert.IsTrue(this.threshold > meanDistance) ;

           
        }

   
        [Test]
        public void AlignToItself()
        {
            CreateCuboid(1);
            pointCloudSource.ResizeVerticesTo1();
            pointCloudTarget.ResizeVerticesTo1();

            this.pointCloudResult = pca.AlignPointClouds_SVD( pointCloudSource, pointCloudTarget);

            CheckResultTargetAndShow_Cube();

        }
       
        [Test]
        public void Translate()
        {

            CreateCuboid(1);
            PointCloud.Translate(pointCloudSource, 3, 2, 5);

            this.pointCloudResult = pca.AlignPointClouds_SVD( pointCloudSource, pointCloudTarget);

            CheckResultTargetAndShow_Cube();
        }

        [Test]
        public void Rotate()
        {

            CreateCuboid(1);
            PointCloud.RotateDegrees(pointCloudSource, 45, 124, 297);

            this.pointCloudResult = pca.AlignPointClouds_SVD(pointCloudSource, pointCloudTarget);

            CheckResultTargetAndShow_Cube();
        }
        [Test]
        public void Scale()
        {

            CreateCuboid(1);
            PointCloud.ScaleByFactor(pointCloudSource, 0.8f);

            this.pointCloudResult = pca.AlignPointClouds_SVD(pointCloudSource, pointCloudTarget);

            CheckResultTargetAndShow_Cube();
        }
        [Test]
        public void TranslateRotateScale()
        {

            CreateCuboid(1);
            PointCloud.Translate(pointCloudSource, 3, 2, 5);
            PointCloud.RotateDegrees(pointCloudSource, 45, 124, 297);
            PointCloud.ScaleByFactor(pointCloudSource, 0.8f);

            this.pointCloudResult = pca.AlignPointClouds_SVD(pointCloudSource, pointCloudTarget);

            CheckResultTargetAndShow_Cube();
        }
     
      
      

    }
}
