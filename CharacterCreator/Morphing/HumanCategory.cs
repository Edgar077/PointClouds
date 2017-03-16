using System;
using System.Collections.Generic;


namespace CharacterCreator
{
    public class HumanCategory
    {
        public string name;
        public List<HumanModifier> modifiers = new List<HumanModifier>();

        public static bool Contains(List<HumanCategory> list, string name)
        {

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].name == name)
                    return true;
            }
            return false;

        }
        public static HumanCategory GetByName(List<HumanCategory> list, string name)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].name == name)
                    return list[i];
            }
            return null;
        }
        public string ToString()
        {
            return this.name;
        }
    }
   
   
}
