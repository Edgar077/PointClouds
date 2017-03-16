using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterCreator
{
    public class MeasureData
    {
        public string Name;
        public List<int> Indices;
        public override string ToString()
        {

            return this.Name;
        }
        public static MeasureData GetFromListByName(List<MeasureData> list, string name)
        {
            foreach(MeasureData m in list)
            {
                if(m.Name == name)
                    return m;
            }
            return null;

        }
    }
}
