

using OpenTK;

namespace OpenTKExtension.Collada
{
	public class VertexForCollada 
	{
        private const uint NO_INDEX = uint.MaxValue;
		
		public Vector3 Position { get; set; }
		public uint TextureIndex { get; set; }
		public uint NormalIndex { get; set; }
		public uint ColorIndex { get; set; }
		public uint Index { get; private set; }
		public VertexForCollada DuplicateVertex { get; set; }

        public bool IsSet = false;//=> NormalIndex != NO_INDEX;
		
		public VertexForCollada(uint index, Vector3 position)
		{
			Index = index;
			NormalIndex = NO_INDEX;
			TextureIndex = index;
			ColorIndex = NO_INDEX;
			Position = position;
		}
		
		public bool HasSameInformation(uint normalIndexOther, uint textureIndexOther, uint colorIndexOther)
		{
			return 
				textureIndexOther == TextureIndex && 
				normalIndexOther == NormalIndex &&
				colorIndexOther == ColorIndex;
		}
	}
}
