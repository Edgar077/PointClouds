using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;


//using GeoAPI.Geometries;


using OpenTKExtension;
using OpenTK;

namespace UnitTestsOpenTK.Models
{
    [TestFixture]
    [Category("UnitTest")]
    public class GeometricObjectsTest : TestBase
    {
        private static readonly Random RND = new Random(998715632);
        private const float SideLen = 10.0f;

      
      
        [Test]
        public void ShowCuboid()
        {
            this.pointCloudSource = PointCloud.CreateCuboid(5, 8, 60);
            this.pointCloudSource.ResizeVerticesTo1();

            pointCloudResult = PointCloud.CloneAll(pointCloudSource);
           // PointCloud.Translate(pointCloudResult, 30, -20, 12);
            ShowVerticesInWindow(new byte[4] { 255, 255, 255, 255 }, new byte[4] { 255, 0, 0, 255 });


        

        }
      
     
        
    }
}
