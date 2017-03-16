using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKExtension;
using UnitTestsOpenTK;
using ICPLib;

using OpenTK;
namespace ToDo.ICP
{
    [TestFixture]
    [Category("UnitTest")]
    public class HeadTest : TestBaseICP
    {
        
        
       
        [Test]
        public void Load()
        {

            if (!LoadObjFiles_ResizeAndSort("Head\\HeadFront.obj", "Head\\Head.obj", true))
                return;

            icp.ICPSettings.ChangeColorOfMergedPoints = false;
            pointCloudSource.RotateDegrees(0, 180, 0);

            icp.TakenAlgorithm = true;
            this.pointCloudResult = icp.PerformICP(this.pointCloudSource, this.pointCloudTarget);
            
            //this.pointCloudResult = pointCloudSource.Clone();
            //pointCloudResult.RotateDegrees(0, 180, 0);

        
            UIMode = true;
            string fileNameLong = pathUnitTests + "\\Head\\Merged.obj";
            pointCloudResult.ToObjFile(fileNameLong);


            CheckResultTargetAndShow_Cloud(this.threshold, false);
           
            CheckResult_MeanDistance(this.threshold);
            
        }
     
      
    }
}
