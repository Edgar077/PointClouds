using System;
using System.Collections.Generic;
using OpenTK;

namespace CharacterCreator
{
    public class Morph
    {
        public string morph_name;
        public Dictionary<uint, Vector3> morph_data;
        public List<uint> morph_modified_verts;
        public float morph_values;

        //float morph_values;
        public override string ToString()
        {

            return this.morph_name;
        }
        public static Morph GetFromListByName(List<Morph> list, string name)
        {
            foreach (Morph m in list)
            {
                if (m.morph_name == name)
                    return m;
            }
            return null;

        }
    }
    public struct SharedData
    {
        public string name;
        public float val1;
        public float val2;
    }
}
