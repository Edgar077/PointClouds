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
    public class TriangulationORourke : TestBase
    {

     
        [Test]
        public void Bunny_Delaunay()
        {
            string fileNameLong = pathUnitTests + "\\bunny.xyz";
            pointCloudSource = IOUtils.ReadXYZFile_ToVertices(fileNameLong, false);
            PointCloud.SetColorOfListTo(pointCloudSource, System.Drawing.Color.Red);

            List<Vector3> myListVectors = pointCloudSource.ListVectors;
           
            //DelaunayTri delaunay = new DelaunayTri(myListVectors);
            DelaunayTri delaunay = new DelaunayTri(myListVectors);

          

            ShowPointCloud(pointCloudSource);
           

        }
        [Test]
        public void Bunny_DelaunayOLD()
        {
            string fileNameLong = pathUnitTests + "\\bunny.xyz";
            pointCloudSource = IOUtils.ReadXYZFile_ToVertices(fileNameLong, false);
            PointCloud.SetColorOfListTo(pointCloudSource, System.Drawing.Color.Red);

            List<Vector3> myListVectors = pointCloudSource.ListVectors;

            //DelaunayTri delaunay = new DelaunayTri(myListVectors);
            DelaunayTri_Old delaunay = new DelaunayTri_Old(myListVectors);


            ShowPointCloud(pointCloudSource);

        }
       
     
    }
}
