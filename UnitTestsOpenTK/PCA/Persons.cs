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

        //Model3D model3DTarget = new Model3D(path + "\\KinectFace_1_15000.obj");

        [Test]
        public void Faces_SVD()
        {
            Model model3DTarget = new Model(pathUnitTests + "\\P0_01.obj");
            this.pointCloudTarget = model3DTarget.PointCloud;
            Model model3DSource = new Model(pathUnitTests + "\\P0_03.obj");
            this.pointCloudSource = model3DSource.PointCloud;
           

            pca.MaxmimumIterations = 1;
            this.pointCloudResult = pca.AlignPointClouds_SVD(this.pointCloudSource, this.pointCloudTarget);

            CheckResultTargetAndShow_Cloud(this.threshold);

        }
        [Test]
        public void Faces_AlignToOriginAxes()
        {
            Model model3DTarget = new Model(pathUnitTests + "\\P0_01.obj");
            this.pointCloudTarget = model3DTarget.PointCloud;
            Model model3DSource = new Model(pathUnitTests + "\\P0_03.obj");
            this.pointCloudSource = model3DSource.PointCloud;

            pointCloudTarget = PCA.RotateToOriginAxes(pointCloudTarget);
            pointCloudSource = PCA.RotateToOriginAxes(pointCloudSource);

            pca.MaxmimumIterations = 1;
            this.pointCloudResult = pca.AlignPointClouds_SVD(this.pointCloudSource, this.pointCloudTarget);


            CheckResultTargetAndShow_Cloud(this.threshold);

        }

   
        
      

    }
}
