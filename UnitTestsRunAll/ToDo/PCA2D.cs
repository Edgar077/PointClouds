using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKExtension;
using OpenTK;
using UnitTestsOpenTK;

namespace ToDo.PrincipalComponentAnalysis
{
    [TestFixture]
    [Category("UnitTest")]
    public class PCA2D : PCABase
    {

   

        [Test]
        public void Sample2D_Old()
        {

            UIMode = false;
            Create2DSamples();
            

            PCA pca = new PCA();

            
            pca.PCA_OfPointCloud(pointCloudSource);

            //List<Vector3> resultList = pca.ProjectPointsOnPCAAxes();

            // pointCloudTarget - is the result list of the first vector
            // pointCloudResult - is the result list of the second vector
            pointCloudTarget = PointCloud.FromListVector3(pca.PointsResult0);
            pointCloudResult = PointCloud.FromListVector3(pca.PointsResult1);

          
            if (UIMode)
            {
                //ShowResultsInWindow_Cube(true);
                Show4PointCloudsInWindow(true);
            }

           

            if (!GLSettings.PointCloudCentered)
            {
              
                Assert.IsTrue(1e-3f > PointCloud.MeanDistance(expectedResultCloud, pointCloudTarget));
               
            }
            else
            {
                expectedResultCloud = new PointCloud();
                expectedResultCloud.AddVector(new Vector3(-1.20497441625437f, -1.30683911366186f, 0));
                expectedResultCloud.AddVector(new Vector3(-0.282584287549998f, 0.260557580021532f, 0));
                expectedResultCloud.AddVector(new Vector3(0, 0, 0));
                Assert.IsTrue(this.threshold > PointCloud.MeanDistance(pointCloudSource.PCAAxes, expectedResultCloud));
               
                
            }

           

        }
        [Test]
        public void AlignPCAToOriginAxes()
        {

            Create2DSamples();
            pointCloudTarget = null;

            pointCloudResult = PCA.RotateToOriginAxes(pointCloudSource);


            //-----------Show in Window
            if (UIMode)
            {
                Show4PointCloudsInWindow(true);
                //ShowResultsInWindow_Cube(true);
            }
            expectedResultCloud = new PointCloud();
            expectedResultCloud.AddVector(new Vector3(1.77758032528043f, 0, 0));
            expectedResultCloud.AddVector(new Vector3(0, 0.384374988880412f, 0));
            expectedResultCloud.AddVector(new Vector3(0, 0, 0));
            //----------------check Result

            Assert.IsTrue(this.threshold > PointCloud.MeanDistance(expectedResultCloud, pointCloudResult.PCAAxes));
               
            
        }
        private void Create2DSamples()
        {
           
            pointCloudSource = new PointCloud();

            Vector3 v = new Vector3(2.5f, 2.4f, 0);
            pointCloudSource.AddVector(v);
            v = new Vector3(0.5f, 0.7f, 0);
            pointCloudSource.AddVector( v);
            v = new Vector3(2.2f, 2.9f, 0);
            pointCloudSource.AddVector( v);
            v = new Vector3(1.9f, 2.2f, 0);
            pointCloudSource.AddVector( v);
            v = new Vector3(3.1f, 3.0f, 0);
            pointCloudSource.AddVector( v);
            v = new Vector3(2.3f, 2.7f, 0);
            pointCloudSource.AddVector( v);
            v = new Vector3(2, 1.6f, 0);
            pointCloudSource.AddVector( v);
            v = new Vector3(1, 1.1f, 0);
            pointCloudSource.AddVector( v);
            v = new Vector3(1.5f, 1.6f, 0);
            pointCloudSource.AddVector( v);
            v = new Vector3(1.1f, 0.9f, 0);
            pointCloudSource.AddVector( v);

            //--------------------

            //expected result
            expectedResultCloud = new PointCloud();
            Vector3 v1 = new Vector3(2.371258964f, 2.51870600832217f, 0);
            expectedResultCloud.AddVector(new Vector3(v1));
            v1 = new Vector3(0.605025583745627f, 0.603160886338143f, 0);
            expectedResultCloud.AddVector(new Vector3(v1));
            v1 = new Vector3(2.48258428755f, 2.63944241997847f, 0);
            expectedResultCloud.AddVector(new Vector3(v1));
            v1 = new Vector3(1.99587994658902f, 2.11159364495307f, 0);
            expectedResultCloud.AddVector(new Vector3(v1));
            v1 = new Vector3(2.94598120291464f, 3.1420134339185f, 0);
            expectedResultCloud.AddVector(new Vector3(v1));
            v1 = new Vector3(2.42886391124136f, 2.58118069424077f, 0);
            expectedResultCloud.AddVector(new Vector3(v1));
            v1 = new Vector3(1.74281634877673f, 1.83713685698813f, 0);
            expectedResultCloud.AddVector(new Vector3(v1));
            v1 = new Vector3(1.03412497746524f, 1.06853497544495f, 0);
            expectedResultCloud.AddVector(new Vector3(v1));
            v1 = new Vector3(1.51306017656077f, 1.58795783010856f, 0);
            expectedResultCloud.AddVector(new Vector3(v1));
            v1 = new Vector3(0.980404601156606f, 1.01027324970724f, 0);
            expectedResultCloud.AddVector(new Vector3(v1));

        }
      

    }
}
