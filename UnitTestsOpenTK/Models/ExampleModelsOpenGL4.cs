using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using OpenTKExtension;
using OpenTK;
using System.Threading;
using OpenTKExtension.FastGLControl;

namespace UnitTestsOpenTK.Models
{
    [TestFixture]
    [Category("UnitTest")]
    public class Example3DModelsOpenGL4 : TestBase
    {
        
        System.Timers.Timer formOpenTKTimer = new System.Timers.Timer();
        private delegate void UpdateOpenGLDelegeate();
 

         [Test]
        public void BunnyNew()
        {

            
            TestFormAlternative fOTK = new TestFormAlternative();

            Model myModel = new Model(pathUnitTests + "\\Bunny.obj");
            fOTK.AddModel(myModel);
            fOTK.ShowDialog();
        }
         [Test]
         public void BunnyFace()
         {

             TestFormAlternative fOTK = new TestFormAlternative();

         
             fOTK.AddModel(new Model(pathUnitTests + "\\Bunny.obj"));
           
             fOTK.AddModel(new Model(pathUnitTests + "\\KinectFace_1_15000.obj"));
             fOTK.ShowDialog();
         }
        
       
        [Test]
        public void BunnyModel3D_Old()
        {
            
            Model myModel = new Model(pathUnitTests + "\\Bunny.obj");
            this.pointCloudSource = myModel.PointCloud;


            PointCloud pgl = this.pointCloudSource;
            PointCloudRenderable pcr = new PointCloudRenderable();
            pcr.PointCloud = pgl;


            TestFormAlternative fOTK = new TestFormAlternative();

            fOTK.ReplaceRenderableObject(pcr);
            fOTK.ShowDialog();
        }
        [Test]
        public void Cube()
        {
            Model myModel = Example3DModels.Cuboid("Cuboid", 20f, 40f, 100, System.Drawing.Color.White, null);
            //Model3D myModel = Example3DModels.Cuboid("Cuboid", 1f, 2f, 100, System.Drawing.Color.White, null);
            pointCloudSource = myModel.PointCloud;
            PointCloud pgl = this.pointCloudSource;
            PointCloudRenderable pcr = new PointCloudRenderable();
            pcr.PointCloud = pgl;

            TestFormAlternative fOTK = new TestFormAlternative();

            //UnitCube uc = new UnitCube();
            fOTK.ReplaceRenderableObject(pcr);
            fOTK.ShowDialog();


        }
        [Test]
        public void Cube1()
        {

            TestFormAlternative fOTK = new TestFormAlternative();
            CubeLines uc = new CubeLines();
            fOTK.ReplaceRenderableObject(uc);

            fOTK.ShowDialog();

        }

        [Test]
        public void EmptyWindow()
        {
            
            TestFormAlternative fOTK = new TestFormAlternative();
          
            fOTK.ShowDialog();

        }

         [Test]
            public void Face()
         {
           
             TestFormAlternative fOTK = new TestFormAlternative();

         
             fOTK.AddModel(new Model(pathUnitTests + "\\KinectFace_1_15000.obj"));
             fOTK.ShowDialog();

             fOTK.ShowDialog();

         }
      
        

         [Test]
         public void FaceOld()
         {
             Model myModel = new Model(pathUnitTests + "\\KinectFace_1_15000.obj");
             this.pointCloudSource = myModel.PointCloud;
            

             PointCloud pgl = this.pointCloudSource;
             PointCloudRenderable pcr = new PointCloudRenderable();
             pcr.PointCloud = pgl;


             TestFormAlternative fOTK = new TestFormAlternative();
             
             fOTK.ReplaceRenderableObject(pcr);
             fOTK.ShowDialog();

         }
     
      
    
    
      
     
    }
}
