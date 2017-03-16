using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTKExtension;
using UnitTestsOpenTK;
using ICPLib;

using OpenTK;

namespace UnitTestsOpenTK.InWork
{
    [TestFixture]
    [Category("UnitTest")]
    public class ICPTest_2D : TestBaseICP
    {
        [Test]
        public void Simple3Vectors()
        {
           
        
           
            this.pointCloudTarget = new PointCloud();

            pointCloudTarget.AddVector(new Vector3(1, 0, 0)); 
            pointCloudTarget.AddVector(new Vector3(0, 1, 0)); 
            pointCloudTarget.AddVector(new Vector3(1, 1, 0));


            this.pointCloudSource = PointCloud.CloneAll(pointCloudTarget); 
            PointCloud.Translate(pointCloudSource, 1, 4, 5);

           
            this.pointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(pointCloudSource, pointCloudTarget);
            float f = IterativeClosestPointTransform.Instance.MeanDistance;

            Assert.IsTrue(f < this.threshold);

            //if (pointCloudResult != null)
            //{
            //    for (int i = 0; i < 3; i++)
            //    {

            //        System.Diagnostics.Debug.WriteLine("target: " + pointCloudTarget[i].ToString() + " : result: " + pointCloudResult[i].ToString());

            //    }
            //}
        }
        [Test]
        public void SomePoints()
        {
           

            this.pointCloudTarget = PointCloud.CreateSomePoints();
            this.pointCloudSource = pointCloudTarget.Clone();
            PointCloud.Translate(pointCloudSource, 1, 2, 0);


            this.pointCloudResult = IterativeClosestPointTransform.Instance.PerformICP(pointCloudSource, this.pointCloudTarget);
            float f = IterativeClosestPointTransform.Instance.MeanDistance;
            Assert.IsTrue(f < this.threshold);
        }
     
    }
}
