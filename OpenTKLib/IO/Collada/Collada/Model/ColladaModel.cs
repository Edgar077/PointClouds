using System.Collections.Generic;

namespace OpenTKExtension.Collada
{
    public class ColladaModel
	{
		public List<PointCloud> PointClouds { get; set; }
		public List<Material> Materials { get; set; }

		public ColladaModel()
		{
			PointClouds = new List<PointCloud>();
			Materials = new List<Material>();
		}
		
		//public void CreateVBOs()
		//{
  //          PointClouds.ForEach(g => g.CreateVBOs());
		//}

		public void LoadTextures()
		{
			Materials.ForEach(m => m.LoadTexture("models.textures"));
		}

		//public void Bind(int shaderProgram, int textureLocation, int haveTextureLocation)
		//{
		//	Geometries.ForEach(g => g.Bind(shaderProgram));
		//	Materials.ForEach(m => m.Bind(textureLocation, haveTextureLocation));
		//}

		//public void Render() 
		//{
		//	Geometries.ForEach(g => g.Render());
		//}
	}
}