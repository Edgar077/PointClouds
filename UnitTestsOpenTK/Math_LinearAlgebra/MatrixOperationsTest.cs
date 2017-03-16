using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using OpenTKExtension;
using OpenTK;

namespace UnitTestsOpenTK.LinearAlgebra
{
    [TestFixture]
    [Category("UnitTest")]
    public class MatrixOperationsTest : TestBase
    {
      
      
        [Test]
        public void TranslateCuboid()
        {
            this.pointCloudSource = PointCloud.CreateCuboid(5, 8, 60);
            pointCloudResult = PointCloud.CloneAll(pointCloudSource);
            PointCloud.Translate(pointCloudResult, 30, -20, 12);
            ShowVerticesInWindow(new byte[4] { 255, 255, 255, 255 }, new byte[4] { 255, 0, 0, 255 });
                      
        }
        [Test]
        public void RotateCuboid()
        {
            this.pointCloudSource = PointCloud.CreateCuboid(5, 8, 60);
            pointCloudResult = PointCloud.CloneAll(pointCloudSource);

            Matrix3 R = new Matrix3();
            R = R.RotationXYZDegrees(90, 124, -274);

            
            PointCloud.Rotate(pointCloudResult, R);

            ShowVerticesInWindow(new byte[4] { 255, 255, 255, 255 }, new byte[4] { 255, 0, 0, 255 });
        }
        [Test]
        public void ScaleCuboid()
        {
            this.pointCloudSource = PointCloud.CreateCuboid(5, 8, 60);
            pointCloudResult = PointCloud.CloneAll(pointCloudSource);

            PointCloud.ScaleByVector(pointCloudResult, new Vector3(1, 2, 3));
            ShowVerticesInWindow(new byte[4] { 255, 255, 255, 255 }, new byte[4] { 255, 0, 0, 255 });
        }

        [Test]
        public void RotateScaleTranslate()
        {
            this.pointCloudSource = PointCloud.CreateCuboid(5, 8, 60);

            pointCloudResult = PointCloud.CloneAll(pointCloudSource);
            Matrix3 R = new Matrix3();
            R = R.RotationXYZDegrees(90, 124, -274);

            PointCloud.Rotate(pointCloudResult, R);
            PointCloud.Translate(pointCloudResult, 30, -20, 12);
            PointCloud.ScaleByVector(pointCloudResult, new Vector3(1, 2, 3));
            ShowVerticesInWindow(new byte[4] { 255, 255, 255, 255 }, new byte[4] { 255, 0, 0, 255 });
        }
     
    }
}
