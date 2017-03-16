using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using System.Drawing;


//using GeoAPI.Geometries;


using OpenTKExtension;
using OpenTK;

namespace UnitTestsOpenTK.TextureTest
{
    [TestFixture]
    [Category("UnitTest")]
    public class Cuboid : TestBase
    {
      
      
      
        [Test]
        public void ShowCuboid()
        {

            CreateCuboid(1, 1, 1);

            string path = AppDomain.CurrentDomain.BaseDirectory ;

           // this.pointCloudSource.Texture = new Texture(path+ "Textures\\crate.jpg");
           
            pointCloudSource.ResizeTo1();
            pointCloudSource.Translate_StartAt_000();

            this.pointCloudSource.Texture = new Texture(path + "Models\\ModelWithTexture\\cubeCrate\\crate.png");
            //this.pointCloudSource.Texture = new Texture(path + "Textures\\crate.jpg");
            this.pointCloudSource.InitCubeUVs();

            
            //this.pointCloudSource = Example3DModels.CuboidEmpty(1, 1, 1, 1, 1, 1);

            ShowPointCloud(pointCloudSource);
        

        }
        [Test]
        public void ShowCuboid_New()
        {
            this.pointCloudSource = Example3DModels.CreateCube24();


            string path = AppDomain.CurrentDomain.BaseDirectory;

            this.pointCloudSource.Texture = new Texture(path + "Models\\ModelWithTexture\\cubeBrick\\AlternatingBrick-ColorMap.png");
            this.pointCloudSource.InitCubeUVs();


            //this.pointCloudSource = Example3DModels.CuboidEmpty(1, 1, 1, 1, 1, 1);

            ShowPointCloud(pointCloudSource);


        }

        [Test]
        public void LoadObjFileWithTexture()
        {
            this.pointCloudSource = PointCloud.FromObjFile(this.pathModels + "\\ModelWithTexture", "capsule.obj");


            string path = AppDomain.CurrentDomain.BaseDirectory;

            this.pointCloudSource.Texture = new Texture(path + "Textures\\AlternatingBrick-ColorMap.png");
            this.pointCloudSource.InitCubeUVs();


            //this.pointCloudSource = Example3DModels.CuboidEmpty(1, 1, 1, 1, 1, 1);

            ShowPointCloud(pointCloudSource);


        }
      
        
    }
}
