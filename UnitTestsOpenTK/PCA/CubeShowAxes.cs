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
    public class CubeShowAxes : PCABase
    {

        [Test]
        public void Cuboid_New()
        {
            ResetPointCloudForOpenGL();

            
            
            List<Vector3> listVectors = Example3DModels.Cuboid_Corners_CenteredAt0(1, 1, 1);
            pointCloudSource = PointCloud.FromListVector3(listVectors);
            
            this.pointCloudSource = PointCloud.Clone(pointCloudSource);

            PointCloud.RotateDegrees(pointCloudSource, 45, 45, 45);

            
            pointCloudSource = pointCloudSource;
            
            pca.PCA_OfPointCloud(pointCloudSource);
            //-----------Show in Window
            if (UIMode)
            {
                this.ShowResultsInWindow_CubeNew(true, true);
                //ShowResultsInWindow_Cube(true);
            }

        }
       

        [Test]
        public void Cuboid_RotateCenter_Axes()
        {
            
            float cubeSizeY = 2;
            int numberOfPoints = 3;

            Model myModel = Example3DModels.Cuboid("Cuboid", cubeSizeX, cubeSizeY, numberOfPoints, System.Drawing.Color.White, null);


            this.pointCloudSource = myModel.PointCloud;
            PointCloud.RotateDegrees(pointCloudSource, 45, 45, 45);
            pointCloudSource.ResizeVerticesTo1();

            
            pca.PCA_OfPointCloud(pointCloudSource);
            //-----------Show in Window
            if (UIMode)
            {
                
                ShowResultsInWindow_Cube(true);
            }

        }



        [Test]
        public void Cuboid_Rotate_Axes()
        {
            
            float cubeSizeY = 2;
            int numberOfPoints = 3;
            Model myModel = Example3DModels.Cuboid("Cuboid", cubeSizeX, cubeSizeY, numberOfPoints, System.Drawing.Color.White, null);
            


            this.pointCloudSource = myModel.PointCloud;
            PointCloud.RotateDegrees(pointCloudSource, 45, 45, 45);
           
            pca.PCA_OfPointCloud(pointCloudSource);

            //-----------Show in Window
            if (UIMode)
            {
                
                ShowPointCloud(pointCloudSource);
           
                
            }
        }
       
   
    
      
     

    }
}
