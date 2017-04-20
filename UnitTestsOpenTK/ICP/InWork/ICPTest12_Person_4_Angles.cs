using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKExtension;
using UnitTestsOpenTK;
using ICPLib;


namespace UnitTestsOpenTK.InWork
{
    [TestFixture]
    [Category("UnitTest")]
    public class ICPTest12_Person_4_Angles : TestBaseICP
    {

       
       
        [Test]
        public void Person_4_Angles()
        {
           
            this.icp.Reset_RealData();

         
            this.pointCloudTarget = new PointCloud(pathUnitTests + "\\C1.obj");
            pointCloudTarget = PCA.RotateToOriginAxes(pointCloudTarget);

          
            this.pointCloudSource = new PointCloud(pathUnitTests + "\\C2.obj");
            pointCloudSource = PCA.RotateToOriginAxes(pointCloudSource);

            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Zinsser;
            IterativeClosestPointTransform.Instance.ICPSettings.MaximumNumberOfIterations = 5;
            this.pointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(pointCloudSource, pointCloudTarget);

            Show3PointCloudsInWindow(true);
            Assert.IsTrue(1e-3f > PointCloud.MeanDistance(pointCloudTarget, pointCloudResult));

       
        }
      
    }
}
