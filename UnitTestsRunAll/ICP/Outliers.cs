using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKExtension;
using UnitTestsOpenTK;
using ICPLib;


namespace Automated.ICP
{
    [TestFixture]
    [Category("UnitTest")]
    public class Outliers : TestBaseICP
    {

        [Test]
        public void Outliers_CubeTranslate_FixedPoints()
        {
           

            
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Horn;
            IterativeClosestPointTransform.Instance.ICPSettings.FixedTestPoints = true;
            //KDTreeVertex.KDTreeMode = KDTreeMode.Rednaxela;
            IterativeClosestPointTransform.Instance.ICPSettings.MaximumNumberOfIterations = 10;
          //  this.icp.ICPSettings.SimulatedAnnealing = true;

            meanDistance = ICPTestData.Test8_CubeOutliers_Translate(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);
           // this.ShowResultsInWindowIncludingLines(false);

            
            Assert.IsTrue(1e-3f > PointCloud.MeanDistance(pointCloudTarget, pointCloudResult));

        }
    
     
     
    }
}
