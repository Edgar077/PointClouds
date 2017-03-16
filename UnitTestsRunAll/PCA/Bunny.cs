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
    public class Bunny : PCABase
    {

        public Bunny()
        {
            UIMode = true;
        }
     
      
        [Test]
        public void Stitch_00_45()
        {
           
            string fileNameLong = pathUnitTests + "\\Bunny\\bun000.obj";
            pointCloudTarget = PointCloud.FromObjFile(fileNameLong);

            fileNameLong = pathUnitTests + "\\Bunny\\bun045.obj";
            pointCloudSource = PointCloud.FromObjFile(fileNameLong);



            pca.MaxmimumIterations = 1;
            pca.ThresholdConvergence = 1e-3f;
            this.pointCloudResult = pca.AlignPointClouds_SVD(this.pointCloudSource, this.pointCloudTarget);
            double executionTime = Performance_Stop("Execution Time");//on i3_2121 (3.3 GHz)
            GlobalVariables.ShowLastTimeSpan("testexecution");


          

        }

        [Test]
        public void Rotate_65()
        {


            Model model3DTarget = new Model(pathUnitTests + "\\bunny.obj");
            this.pointCloudTarget = model3DTarget.PointCloud;

            this.pointCloudSource = PointCloud.CloneAll(pointCloudTarget);

            PointCloud.RotateDegrees(pointCloudSource, 60, 60, 65);
            //R = R.RotationXYZRadiants(65, 65, 65);


            this.pointCloudResult = pca.AlignPointClouds_SVD(this.pointCloudSource, this.pointCloudTarget);

            CheckResultTargetAndShow_Cloud(this.threshold);

            double executionTime = Performance_Stop("PCA_Bunny_Rotate");//3 seconds on i3_2121 (3.3 GHz)
            Assert.IsTrue(executionTime < 3);
        }
        [Test]
        public void Rotate_Custom()
        {

            Model model3DTarget = new Model(pathUnitTests + "\\bunny.obj");
            this.pointCloudTarget = model3DTarget.PointCloud;

            this.pointCloudSource = PointCloud.CloneAll(pointCloudTarget);

            PointCloud.RotateDegrees(pointCloudSource, 124, 124, 124);


            this.pointCloudResult = pca.AlignPointClouds_SVD(this.pointCloudSource, this.pointCloudTarget);

            CheckResultTargetAndShow_Cloud(this.threshold);
            double executionTime = Performance_Stop("PCA_Bunny_Rotate");//5 seconds on i3_2121 (3.3 GHz)
            Assert.IsTrue(executionTime < 5);

        }
    }
}
