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
    public class Face_VMode : PCABase
    {

      

        [Test]
        public void V_Rotate_NotWorking()
        {
            
     
            this.pointCloudTarget = new PointCloud(pathUnitTests + "\\KinectFace_1_15000.obj");
            //PointCloud.ResizeVerticesTo1(pointCloudTarget);

            this.pointCloudSource = PointCloud.CloneAll(pointCloudTarget);
            //Vertices.TranslateVertices(pointCloudTarget, 0, -300, 0);
            PointCloud.RotateDegrees(pointCloudSource, 25, 0, 0);


            this.pointCloudResult = pca.AlignPointClouds_OneVector(this.pointCloudSource, this.pointCloudTarget, 0, 0);
            this.pointCloudSource = pointCloudResult;
            this.pointCloudResult = pca.AlignPointClouds_OneVector(this.pointCloudSource, this.pointCloudTarget, 1, 1);
            this.pointCloudSource = pointCloudResult;
            this.pointCloudResult = pca.AlignPointClouds_OneVector(this.pointCloudSource, this.pointCloudTarget, 2, 2);
            this.pointCloudSource = pointCloudResult;
            this.pointCloudResult = pca.AlignPointClouds_OneVector(this.pointCloudSource, this.pointCloudTarget, 0, 0);


            CheckResultTargetAndShow_Cloud(1e-5f);
        }
        [Test]
        public void V_RotateNotWorking()
        {
           
       
            this.pointCloudTarget = new PointCloud(pathUnitTests + "\\KinectFace_1_15000.obj");
            //PointCloud.ResizeVerticesTo1(pointCloudTarget);

            this.pointCloudSource = PointCloud.CloneAll(pointCloudTarget);
            PointCloud.RotateDegrees(pointCloudSource, 60, 60, 90);


            this.pointCloudResult = pca.AlignPointClouds_OneVector(this.pointCloudSource, this.pointCloudTarget, 0, 0);
            

            ShowPointCloudsInWindow_PCAVectors(true);

            Assert.IsTrue(this.threshold > pca.MeanDistance);

        }
       
    

    }
}
