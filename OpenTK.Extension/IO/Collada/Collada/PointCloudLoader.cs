using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System;
using System.Globalization;

using OpenTK;
using OpenTKExtension;

namespace OpenTKExtension.Collada
{
	public class PointCloudLoader
	{
		private static XNamespace ns = "{http://www.collada.org/2005/11/COLLADASchema}";

		private List<VertexForCollada> Vertices;
		private List<Vector3> Normals;
		private List<Vector2> Textures;		
		private List<Vector3> Colors;	
		private List<uint> PolyList;

		private XElement mesh;


		public PointCloudLoader(XElement mesh)
		{
			Vertices = new List<VertexForCollada>();
			PolyList = new List<uint>();

			this.mesh = mesh;
		}

		public PointCloud Load()
		{
			// Vertices
			var positionId = mesh
				.Element($"{ns}vertices")
				.Element($"{ns}input")
				.Attribute("source").Value.TrimStart(new[]{ '#' });

			var polylist = readVecArray<Vector3>(mesh, positionId);
			foreach(var poly in polylist)
				Vertices.Add(new VertexForCollada(System.Convert.ToUInt32(Vertices.Count), poly));

			// Normals
			var normals = mesh
				.Element($"{ns}polylist")
				.Elements($"{ns}input").FirstOrDefault(x => x.Attribute("semantic").Value == "NORMAL");
			if (normals != null) {
				var normalId = normals.Attribute("source").Value.TrimStart(new[]{ '#' });

				Normals = new List<Vector3>();
				Normals = readVecArray<Vector3>(mesh, normalId);
			}

			// Textures
			var texCoords = mesh
				.Element($"{ns}polylist")
				.Elements($"{ns}input").FirstOrDefault(x => x.Attribute("semantic").Value == "TEXCOORD");
			if (texCoords != null)
            {
				var texCoordId = texCoords.Attribute("source").Value.TrimStart(new[]{ '#' });

				Textures = new List<Vector2>();
               	Textures = readVecArray<Vector2>(mesh, texCoordId);
			}

			// Colors
			var colors = mesh
				.Element($"{ns}polylist")
				.Elements($"{ns}input").FirstOrDefault(x => x.Attribute("semantic").Value == "COLOR");
			if (colors != null) {
				var colorId = colors.Attribute("source").Value.TrimStart(new[]{ '#' });

				Colors = new List<Vector3>();
				Colors = readVecArray<Vector3>(mesh, colorId);
			}

			assembleVertices(mesh);
			removeUnusedVertices();

			return convertDataToArrays();
		}

		private List<T> readVecArray<T>(XElement mesh, string id)
		{
			var data = mesh
				.Elements($"{ns}source").FirstOrDefault(x => x.Attribute("id").Value == id)
				.Element($"{ns}float_array");

			var count = int.Parse(data.Attribute("count").Value);
			var array = parseFloats(data.Value);
			var result = new List<T>();

			if(typeof(T) == typeof(Vector3))
				for (var i = 0; i < count / 3; i++) 
					result.Add((T)(object)new Vector3(
						array[i * 3] ,
						array[i * 3 + 2],
						array[i * 3 + 1]
					));
			else if(typeof(T) == typeof(Vector2))
				for (var i = 0; i < count / 2; i++) 
					result.Add((T)(object)new Vector2(
						array[i * 2],
						array[i * 2 + 1]
					));
			
			return result;
		}

        private void assembleVertices(XElement mesh)
        {
            var poly = mesh.Element($"{ns}polylist");
            var typeCount = poly.Elements($"{ns}input").Count();
            //EDGAR TODO
            ////get infos from the elements:
            // < input semantic = "VERTEX" source = "#Cube-mesh-vertices" offset = "0" />
            //   < input semantic = "NORMAL" source = "#Cube-mesh-normals" offset = "1" />
            //   < input semantic = "COLOR" source = "#Cube-mesh-colors-Col" offset = "2" set = "0" />

               List <uint> id = parseUInts(poly.Element($"{ns}p").Value);

            for (int i = 0; i < id.Count / typeCount; i++)
            {
                uint textureIndex = uint.MaxValue;
                uint colorIndex = uint.MaxValue;

                var index = 0;

                var posIndex = id[i * typeCount + index];
                index++;


                
                var normalIndex = id[i * typeCount + index];
                index++;


                //if (Textures != null)
                //    textureIndex = id[i * typeCount + index];
                //index++;


                if (Colors != null)
                    colorIndex = id[i * typeCount + index];
                index++;


                processVertex(System.Convert.ToUInt32(posIndex), System.Convert.ToUInt32(normalIndex), System.Convert.ToUInt32(textureIndex), System.Convert.ToUInt32(colorIndex));
            }
        }

		private void processVertex(uint posIndex, uint normalIndex, uint textureIndex, uint colorIndex)
        {
			var currentVertex = Vertices[System.Convert.ToInt32(posIndex)];
			
			if (!currentVertex.IsSet)
            {
				currentVertex.NormalIndex = normalIndex;
				currentVertex.TextureIndex = textureIndex;
				currentVertex.ColorIndex = colorIndex;
				PolyList.Add(posIndex);
			}
            else
            {
				handleAlreadyProcessedVertex(currentVertex, System.Convert.ToUInt32(normalIndex), System.Convert.ToUInt32(textureIndex), System.Convert.ToUInt32(colorIndex));
			}
		}

		private void handleAlreadyProcessedVertex(VertexForCollada previousVertex, uint newNormalIndex, uint newTextureIndex, uint newColorIndex) 
		{
			if (previousVertex.HasSameInformation(newNormalIndex, newTextureIndex, newColorIndex)) {
				PolyList.Add(previousVertex.Index);
				return;
			} 

			if (previousVertex.DuplicateVertex != null) {
				handleAlreadyProcessedVertex(previousVertex.DuplicateVertex, newNormalIndex, newTextureIndex, newColorIndex);
				return;
			} 

			var duplicateVertex = new VertexForCollada(System.Convert.ToUInt32(Vertices.Count), previousVertex.Position);

			duplicateVertex.NormalIndex = newNormalIndex;
			duplicateVertex.TextureIndex = newTextureIndex;
			duplicateVertex.ColorIndex = newColorIndex;
			previousVertex.DuplicateVertex = duplicateVertex;

			Vertices.Add(duplicateVertex);
			PolyList.Add(duplicateVertex.Index);
		}

		private void removeUnusedVertices() 
		{
			foreach (var vertex in Vertices) {
				if (!vertex.IsSet) {
					vertex.NormalIndex = 0;
					vertex.TextureIndex = 0;
					vertex.ColorIndex = 0;
				}
			}
		}

		private PointCloud convertDataToArrays() 
		{
			var vectors= new Vector3[Vertices.Count];
			var normals = new Vector3[Vertices.Count];

			Vector2[] textures = null;
			Vector3[] colors = null;

			if(Textures != null)
            	textures = new Vector2[Vertices.Count];

			if(Colors != null)
				colors = new Vector3[Vertices.Count];

			for (int i = 0; i < Vertices.Count; i++)
            {
				VertexForCollada currentVertex = Vertices[i];

                vectors[i] = currentVertex.Position;
				normals[i] = Normals[Convert.ToInt32(currentVertex.NormalIndex)];

				if(textures != null)
                    textures[i] = Textures[Convert.ToInt32(currentVertex.TextureIndex)];
				if(colors != null)
                    colors[i] = Colors[Convert.ToInt32(currentVertex.ColorIndex)];
			}
            
			return new PointCloud(vectors, colors, normals, PolyList.ToArray(), null, textures);
            //return new PointCloud(verticesArray, normalsArray, texturesArray, colorsArray, PolyList.ToArray());

        }

		private static List<float> parseFloats(string input) 
		{
            
            string[] arr = input.Split(' ');
            List<float> result = new List<float>();
            float f;
            for (int i = 0; i < arr.Length; i++)
            {

                float.TryParse(arr[i], NumberStyles.Float | NumberStyles.AllowThousands, GlobalVariables.CurrentCulture, out f);
                result.Add(f);

            }
            return result;

		}

		private static List<int> parseInts(string input) 
		{
			return input.Split(' ' ).Select(x => int.Parse(x)).ToList();
		}
        private static List<uint> parseUInts(string input)
        {
            return input.Split(' ').Select(x => uint.Parse(x)).ToList();
        }
    }
}