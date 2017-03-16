using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKExtension;
using ICPLib;

namespace UnitTestsOpenTK.InWork
{
    [TestFixture]
    [Category("UnitTest")]
    public class ICPTestOld : TestBaseICP
    {
        
        [Test]
        public void ICP_Show_KnownTransformation()
        {

            TestForm fOTK = new TestForm();
            fOTK.OpenGL_UControl.RemoveAllModels();
            string fileNameLong = pathUnitTests + "\\KinectFace_1_15000.obj";
            fOTK.OpenGL_UControl.LoadModelFromFile(fileNameLong);

            fileNameLong = pathUnitTests + "\\transformed.obj";
            fOTK.OpenGL_UControl.LoadModelFromFile(fileNameLong);
            fOTK.ICP_OnCurrentModels();
            fOTK.ShowDialog();


        }
        [Test]
        public void Translation_Horn_Old()
        {

           
            IterativeClosestPointTransform.Instance.ICPSettings.ICPVersion = ICP_VersionUsed.Horn;

            meanDistance = ICPTestData.Test1_Translation(ref pointCloudTarget, ref pointCloudSource, ref pointCloudResult);
            Assert.IsTrue(this.threshold > meanDistance);


            TestForm fOTK = new TestForm();
            fOTK.Show3PointClouds(pointCloudSource, pointCloudTarget, pointCloudResult, true);
            fOTK.ShowDialog();

        }
        [Test]
        public void ICP_Face_Old()
        {

            TestForm fOTK = new TestForm();
            fOTK.IPCOnTwoPointClouds();
            fOTK.ShowDialog();

        }

     
    }
}
