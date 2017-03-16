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
    public class AlignToCenter : PCABase
    {

        [Test]
        public void ScannerMan_AlignCenter()
        {

            Model myModel = new Model(pathUnitTests + "\\1.obj");
            this.pointCloudTarget = myModel.PointCloud;
            this.pointCloudSource = this.pointCloudTarget.Clone();

            this.pointCloudResult = pca.AlignToCenter(pointCloudSource);


          
            ShowPointCloudsInWindow_PCAVectors(true);


        }

        [Test]
        public void Cuboid_AlignCenter()
        {
            
            float cubeSizeY = 2;
            int numberOfPoints = 3;

            Model myModel = Example3DModels.Cuboid("Cuboid", cubeSizeX, cubeSizeY, numberOfPoints, System.Drawing.Color.White, null);

            this.pointCloudTarget = myModel.PointCloud;
            this.pointCloudSource = this.pointCloudTarget.Clone();

            
            this.pointCloudResult = pca.AlignToCenter(pointCloudSource);


            this.ShowResultsInWindow_Cube(true);

          

        }
        [Test]
        public void Cuboid_Rotate()
        {
            
            float cubeSizeY = 2;
            int numberOfPoints = 3;

            Model myModel = Example3DModels.Cuboid("Cuboid", cubeSizeX, cubeSizeY, numberOfPoints, System.Drawing.Color.White, null);

            this.pointCloudTarget = myModel.PointCloud;
            this.pointCloudSource = this.pointCloudTarget.Clone();

            PointCloud.RotateDegrees(pointCloudSource, 45, 45, 128);



            this.pointCloudResult = pca.AlignToCenter(pointCloudSource);


           

            this.ShowResultsInWindow_Cube(true);

            

        }

     
        
      

    }
}
