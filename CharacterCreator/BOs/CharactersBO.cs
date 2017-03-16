using System;
using System.Collections.Generic;

using OpenTK;
using OpenTKExtension;

using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;


namespace CharacterCreator
{
    public class CharactersBO
    {
        public List<string> Names;
        public CharactersBO()
        {
            ReadAllCharacters();
        }
        public void ReadAllCharacters()
        {
            Names = new List<string>();
            //string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetCallingAssembly().Location) + GLSettings.PathCharacters;
            string path = AppDomain.CurrentDomain.BaseDirectory + GLSettings.PathCharacters;

            //string path = GLSettings.Path + GLSettings.PathCharacters;
            string[] dirs = System.IO.Directory.GetDirectories(path);
            for(int i = 0; i < dirs.Length; i++)
            {
                Names.Add( IOUtils.ExtractDirectoryLast(dirs[i]));
                
            }

        }
    }
}
