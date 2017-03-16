using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKExtension
{
    public class KeyValueList : List<KeyValuePair<int, float>>
    {
        public bool Contains(int index)
        {
            for(int i = 0; i < this.Count; i++)
            {
                if (this[i].Key == index)
                    return true;
            }
            return false;
        }
        public bool Contains(float value)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].Value == value)
                    return true;
            }
            return false;
        }
        public void Remove(int index)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].Key == index)
                {
                    this.RemoveAt(i);
                    return;
                }
                    
            }
            
        }
        

    }
}
