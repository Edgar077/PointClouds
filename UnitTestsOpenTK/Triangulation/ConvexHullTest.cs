using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using OpenTKExtension;
using OpenTK;


using System.Linq;
namespace UnitTestsOpenTK.Triangulation
{
    [TestFixture]
    [Category("UnitTest")]
    public class ConvexHullTest : TestBase
    {


        [Test]
        public void Cube_ConvexHull()
        {

            pointCloudSource = PointCloud.CreateCube_Corners_CenteredAt0(0.1f);
            PointCloud.SetColorOfListTo(pointCloudSource, Color.Red);
            List<Vector3> myListVectors = pointCloudSource.ListVectors;

            ConvexHull3D convHull = new ConvexHull3D(myListVectors);

            Model myModel = CreateModel("Convex Hull", pointCloudSource, convHull.Faces.ListFaces);


            ShowModel(myModel);
            System.Diagnostics.Debug.WriteLine("Number of faces: " + convHull.Faces.ListFaces.Count.ToString());

        }
        [Test]
        public void Cube_ConvexHull_RandomPoints()
        {

            pointCloudSource = PointCloud.CreateCube_RandomPointsOnPlanes(1, 10);

            PointCloud.SetColorOfListTo(pointCloudSource, Color.Red);
            List<Vector3> myListVectors = pointCloudSource.ListVectors;

            ConvexHull3D convHull = new ConvexHull3D(myListVectors);

            Model myModel = CreateModel("Convex Hull", pointCloudSource, convHull.Faces.ListFaces);

            ShowModel(myModel);

            System.Diagnostics.Debug.WriteLine("Number of faces: " + convHull.Faces.ListFaces.Count.ToString());

        }
     
        [Test]
        public void Bunny_Hull()
        {
            string fileNameLong = pathUnitTests + "\\bunny.xyz";
            pointCloudSource = IOUtils.ReadXYZFile_ToVertices(fileNameLong, false);
            PointCloud.SetColorOfListTo(pointCloudSource, System.Drawing.Color.Red);

            List<Vector3> myListVectors = pointCloudSource.ListVectors;

            ConvexHull3D cHull = new ConvexHull3D(myListVectors);

            Model myModel = CreateModel("Bunny Hull", pointCloudSource, cHull.Faces.ListFaces);

            ShowModel(myModel);

        }
      
     
    }
}
