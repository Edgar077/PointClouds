using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using OpenTKExtension;
using OpenTK;

namespace UnitTestsOpenTK.Models
{
    [TestFixture]
    [Category("UnitTest")]
    public class Example3DModelsFromDisk : TestBase
    {
        [Test]
        public void Bunny_obj_Triangulated()
        {
            string fileNameLong = pathUnitTests + "\\Bunny.obj";
            TestForm fOTK = new TestForm();
            fOTK.OpenGL_UControl.LoadPointCloudFromFile(fileNameLong, false);
            fOTK.ShowDialog();


        }
        [Test]
        public void Bunny_Face()
        {
            string fileNameLong = pathUnitTests + "\\Bunny.obj";
            TestForm fOTK = new TestForm();
            fOTK.OpenGL_UControl.LoadPointCloudFromFile(fileNameLong, false);

            fileNameLong = pathUnitTests + "\\KinectFace_1_15000.obj";
            fOTK.OpenGL_UControl.LoadPointCloudFromFile(fileNameLong, false);

            fOTK.ShowDialog();


        }
        [Test]
        public void KinectFace_ObjFile()
        {
            string fileNameLong = pathUnitTests + "\\KinectFace_1_15000.obj";
            TestForm fOTK = new TestForm();
            fOTK.OpenGL_UControl.LoadPointCloudFromFile(fileNameLong, false);
            fOTK.ShowDialog();


        }

        [Test]
        public void EmptyWindow()
        {
            TestForm fOTK = new TestForm();
            fOTK.ShowDialog();


        }
   
       
        public void ShowDialog()
        {
            TestForm fOTK = new TestForm();
            fOTK.ShowDialog();

        }

       
        [Test]
        public void SaveAsTextureObj()
        {
            //string fileNameLong = pathUnitTests + "\\KinectFace_1_15000.obj";
            string fileNameLong = pathUnitTests + "\\Face1.obj";

            this.pointCloudSource = PointCloud.FromObjFile(fileNameLong);
            GLSettings.ShowPointCloudAsTexture = true;
            this.pointCloudSource.Triangulate25D(0.01f);

            this.pointCloudSource.ToObjFile(pathUnitTests + "\\fileWithTexture.obj");

            

            TestForm fOTK = new TestForm();
            fOTK.OpenGL_UControl.LoadPointCloudFromFile(pathUnitTests + "\\Face1.obj", false);
            fOTK.OpenGL_UControl.LoadPointCloudFromFile(pathUnitTests + "\\fileWithTexture.obj", false);

            fOTK.ShowDialog();


        }
        [Test]
        public void Bunny_obj_Loop()
        {
            string fileNameLong = pathUnitTests + "\\Bunny.obj";
            TestForm fOTK = new TestForm();
            for(int i = 0; i < 1000; i++)
                fOTK.OpenGL_UControl.LoadPointCloudFromFile(fileNameLong, false);
            

            fOTK.ShowDialog();
            
          



        }
    }
}
