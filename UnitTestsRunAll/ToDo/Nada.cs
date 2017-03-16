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
    public class Nada : TestBaseICP
    {
        public Nada()
        {
            UIMode = true;
        }

        [Test]
        public void Nada_00_to_30()
        {
            if (!LoadObjFiles("Nada\\PointCloudLast1.obj", "Nada\\PointCloudLast2.obj", false))
                return;

            //if (!LoadObjFiles_ResizeAndSort("Armadillo\\ArmadilloBack_0.obj", "Armadillo\\ArmadilloBack_30.obj"))
            //    return;
            
            icp.ICPSettings.MaximumNumberOfIterations = 100;
            icp.ICPSettings.ICPVersion = ICP_VersionUsed.Zinsser;
            //icp.TakenAlgorithm = true;

            this.pointCloudResult = icp.PerformICP(this.pointCloudSource, this.pointCloudTarget);

            UIMode = true;

            //save
            string fileNameLong = pathUnitTests + "\\Armadillo\\Result_Back_00_30.obj";
            pointCloudResult.ToObjFile(fileNameLong);

            CheckResultTargetAndShow_Cloud(this.threshold, false);
            CheckResult_MeanDistance(this.threshold);

       

        }
        [Test]
        public void Arm00_to_30_Show()
        {

            if (!LoadObjFiles("Armadillo\\ArmadilloBack_0.obj", "Armadillo\\ArmadilloBack_30.obj", true))
                return;


            this.pointCloudSource.RotateDegrees(0, 0, 30);
            this.pointCloudResult = this.pointCloudSource;


          

            UIMode = true;

            //save
            string fileNameLong = pathUnitTests + "\\Armadillo\\Result_Back_00_30.obj";
            pointCloudResult.ToObjFile(fileNameLong);

            CheckResultTargetAndShow_Cloud(this.threshold, false);
            CheckResult_MeanDistance(this.threshold);



        }
        
    }
}
