using System.Linq;
using System.Xml.Linq;


namespace OpenTKExtension.Collada
{
	public class MaterialLoader
	{
		private static XNamespace ns = "{http://www.collada.org/2005/11/COLLADASchema}";

		private XDocument root;
		private XElement material;
		private XElement effect;

		private string texturePath;

		public MaterialLoader(XDocument root, XElement material)
		{
			this.root = root;
			this.material = material;
		}

		public Material Load()
		{
			var effectId = material
				.Descendants($"{ns}instance_effect").First()
				.Attribute("url").Value.TrimStart(new[] { '#' });
			
			effect = root.Descendants($"{ns}effect")
				.First(x => x.Attribute("id").Value == effectId);

			getTexture();

			return new Material(texturePath);
		}

		private void getTexture()
		{
			var imageId = effect.Descendants($"{ns}init_from").FirstOrDefault();
			if (imageId == null)
				return; // No textures
			
			texturePath = root.Descendants($"{ns}library_images")
                .Elements($"{ns}image")
				.First(x => x.Attribute("id").Value == imageId.Value).Value;
		}

	}
}