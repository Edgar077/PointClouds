using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterCreator
{
    public class Utils
    {
        public static KeyValuePair<string, float[]> GetValueFromList(List<KeyValuePair<string, float[]>> list, string name)
        {

            foreach(KeyValuePair<string, float[]> kvp in list)
            {
                if (kvp.Key == name)
                    return kvp;
            }
            return new KeyValuePair<string,float[]>();

        }
        public static float linear_interpolation_y(float xa,float xb,float ya,float yb,float y)
        {
            return (((xa - xb) * y) + (xb * ya) - (xa * yb)) / (ya - yb);
        }
    }
}
