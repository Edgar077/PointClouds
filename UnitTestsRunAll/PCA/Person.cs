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
    public class Person : PCABase
    {
        public Person()
        {
            UIMode = false;
        }

        [Test]
        public void Rotate()
        {
            Model model3DTarget = new Model(pathUnitTests + "\\1.obj");
            this.pointCloudTarget = model3DTarget.PointCloud;
            
            this.pointCloudSource = PointCloud.CloneAll(pointCloudTarget);
            PointCloud.RotateDegrees(pointCloudSource, 25, 10, 25);
            
            this.pointCloudResult = pca.AlignPointClouds_SVD(this.pointCloudSource, this.pointCloudTarget);

            CheckResultTargetAndShow_Cloud(this.threshold);

        }
     
      

    }
}
