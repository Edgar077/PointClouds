using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKExtension;
using OpenTK;

namespace UnitTestsOpenTK.PrincipalComponentAnalysis
{
    [TestFixture]
    [Category("UnitTest")]
    public class Persons : PCABase
    {

        //Model3D model3DTarget = new PointCloud 3D(path + "\\KinectFace_1_15000.obj");

        [Test]
        public void Faces_SVD()
        {
          
            this.pointCloudTarget = new PointCloud(pathUnitTests + "\\P0_01.obj");
          
            this.pointCloudSource = new PointCloud(pathUnitTests + "\\P0_03.obj");


            pca.MaxmimumIterations = 1;
            this.pointCloudResult = pca.AlignPointClouds_SVD(this.pointCloudSource, this.pointCloudTarget);

            CheckResultTargetAndShow_Cloud(this.threshold);

        }
        [Test]
        public void Faces_AlignToOriginAxes()
        {
         
            this.pointCloudTarget = new PointCloud(pathUnitTests + "\\P0_01.obj");
         
            this.pointCloudSource = new PointCloud(pathUnitTests + "\\P0_03.obj");

            pointCloudTarget = PCA.RotateToOriginAxes(pointCloudTarget);
            pointCloudSource = PCA.RotateToOriginAxes(pointCloudSource);

            pca.MaxmimumIterations = 1;
            this.pointCloudResult = pca.AlignPointClouds_SVD(this.pointCloudSource, this.pointCloudTarget);


            CheckResultTargetAndShow_Cloud(this.threshold);

        }

   
        
      

    }
}
