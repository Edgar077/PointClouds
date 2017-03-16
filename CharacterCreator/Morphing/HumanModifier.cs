using System;
using System.Collections.Generic;


namespace CharacterCreator
{
 
    public class HumanModifier
    {
        public string name;
        public List<string> properties = new List<string>();

        public static HumanModifier GetByName(List<HumanModifier> list, string name)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].name == name)
                    return list[i];
            }
            return null;
        }
     
        public bool is_changed(List<KeyValuePair<string, float>> char_data)
        {
        //      """
        //If a prop is changed, the whole modifier is considered changed
        //""";
            return false;
        }
        public override string ToString()
        {
            return this.name;
        }


//  {
      
//        obj = this.get_object();
//        for( prop in this.properties)
//        {
//            current_val = getattr(obj, prop, 0.5);
//            if( char_data[prop] != current_val)
//            {
//                return True;
//            }
//        }
//        return False;
//  }
//}


//        obj = this.get_object();
//        for( prop in this.properties)
//            if( hasattr(obj, prop))
//                current_val = getattr(obj, prop, 0.5);
//                char_data[prop] = current_val;


    }
}
