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
    public class PCA2D : PCABase
    {
        
      
        [Test]
        public void PCA_2D_InWork()
        {
            List<Vector3> pointsSource = new List<Vector3>();

            Vector3 v = new Vector3(1, 2, 0);
            pointsSource.Add(v);
            v = new Vector3(2, 3, 0);
            pointsSource.Add(v);
            v = new Vector3(3, 2, 0);
            pointsSource.Add(v);
            v = new Vector3(4, 4, 0);
            pointsSource.Add(v);
            v = new Vector3(5, 4, 0);
            pointsSource.Add(v);
            v = new Vector3(6, 7, 0);
            pointsSource.Add(v);
            v = new Vector3(7, 6, 0);
            pointsSource.Add(v);
            v = new Vector3(9, 7, 0);
            pointsSource.Add(v);


            PCA pca = new PCA();

         
            ////expected result
            //List<Vector3> listExpectedResult = new List<Vector3>();
            //Vector3 v1 = new Vector3(2.371258964, 2.51870600832217, 0);
            //listExpectedResult.Add(v1);
            //v1 = new Vector3(0.605025583745627, 0.603160886338143, 0);
            //listExpectedResult.Add(v1);
            //v1 = new Vector3(2.48258428755, 2.63944241997847, 0);
            //listExpectedResult.Add(v1);
            //v1 = new Vector3(1.99587994658902, 2.11159364495307, 0);
            //listExpectedResult.Add(v1);
            //v1 = new Vector3(2.94598120291464, 3.1420134339185, 0);
            //listExpectedResult.Add(v1);
            //v1 = new Vector3(2.42886391124136, 2.58118069424077, 0);
            //listExpectedResult.Add(v1);
            //v1 = new Vector3(1.74281634877673, 1.83713685698813, 0);
            //listExpectedResult.Add(v1);
            //v1 = new Vector3(1.03412497746524, 1.06853497544495, 0);
            //listExpectedResult.Add(v1);
            //v1 = new Vector3(1.51306017656077, 1.58795783010856, 0);
            //listExpectedResult.Add(v1);
            //v1 = new Vector3(0.980404601156606, 1.01027324970724, 0);
            //listExpectedResult.Add(v1);

            //for (int i = 0; i < listExpectedResult.Count; i++)
            //{
            //    Assert.IsTrue(PointCloud.CheckCloud(listExpectedResult[i].X, listResult[i].X, this.threshold));
            //    Assert.IsTrue(PointCloud.CheckCloud(listExpectedResult[i].Y, listResult[i].Y, this.threshold));


            //}


            // ShowVector3DInWindow(listResult);
            this.pointCloudSource = PointCloud.FromListVector3(pointsSource);
            this.pointCloudTarget = pca.CalculatePCA(PointCloud.FromListVector3(pointsSource), 0);


            this.pointCloudResult = pca.CalculatePCA(PointCloud.FromListVector3(pointsSource), 1);

            if (UIMode)
                Show4PointCloudsInWindow(true);


        }
        
      

    }
}
