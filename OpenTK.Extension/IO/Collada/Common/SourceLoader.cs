using System.IO;
using System.Reflection;

namespace OpenTKExtension.Collada
{
	public static class SourceLoader
	{
        public static Stream GetStream(string filePath)
        {
            var reader = new StreamReader(filePath);
            return reader.BaseStream;
        }
        public static string Read(string filePath)
        {
            using (var reader = new StreamReader(filePath))
                return reader.ReadToEnd();
        }
	}
}
