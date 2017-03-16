using System;
using System.Collections.Generic;

using System.Text;

using System.IO;
using System.Collections;

using OpenTK;
using OpenTKExtension;
using Newtonsoft.Json;
using System.Linq;

namespace CharacterCreator
{
    public partial class Humanoid
    {
        public morph_engine m_engine;
        string character_ID = "0001";
        public string PathCharacters;
        public string Name;
        public string NameGeneral;
        string no_categories = "BasisAsymTest";

        List<HumanCategory> categories;
        Dictionary<string, float> character_data = new Dictionary<string, float>();

        //Dictionary<string, float> categories;
        List<string> generator_bool_props = new List<string>(){"preserve_mass", "preserve_height", "preserve_tone","preserve_face", "preserve_phenotype",
                "set_tone_and_mass"};


        List<string> generator_float_props = new List<string>() { "body_mass", "body_tone" };

        List<List<string>> generator_levels = new List<List<string>>() { new List<string>() {"LI", "Light", "Little variations from the standard"},
             new List<string>(){"RE", "Realistic", "Realistic characters"},
            new List<string>(){"NO", "Noticeable", "Very characterized people"},
            new List<string>(){"CA", "Caricature", "Engine for caricatures"},
            new List<string>(){"EX", "Extreme", "Extreme characters"}};

        Dictionary<string, float> character_metaproperties = new Dictionary<string, float>() { {"last_character_age",0}, {"character_age",0f},
                                             {"last_character_mass",0f},
                                             {"character_mass",0f},
                                             {"last_character_tone",0f},
                                             {"character_tone",0f}};


        bool bodydata_activated;

        //List<string> ;

        //public Vector3[] Vectors;
        public List<Vector3> Vectors;
        
        public List<BoundingBox> bbox_data;

        public List<MeasureData> measures_data;
        public List<string[]> measures_relat_data;
        public List<string> body_height_Z_parts;
       
        Dictionary<string, List<uint>> joints;
        List< KeyValuePair<string, float[]>> delta_measures = new List< KeyValuePair<string, float[]>>();

        
        Dictionary<string, List<object>> transformations_data;
        public List<Morph> morph_data;
       // public Dictionary<string, float> morph_values = new Dictionary<string, float>();

        //ArrayList base_form;
        //ArrayList final_form;
        //ArrayList cache_form;

        string vertices_path;//= os.path.join(data_path,self.obj_name,"vertices.json")
        string morph_data_path;//= os.path.join(data_path,self.obj_name,"morphs.json")
        string morph_forma_path;//= os.path.join(data_path,self.obj_name,"forma.json")
        string expressions_path;//= os.path.join(data_path,self.obj_name,"expressions.json")

        string test_path;//= os.path.join(data_path,self.obj_name,"expressions.json")

        string shared_measures_filename;// = obj.name[:len(obj.name)-2]+"_measures.json"
        string shared_morphs_filename; //obj.name[:len(obj.name)-2]+".json"
       
    
        string shared_bbox_filename;//= obj.name[:len(obj.name)-2]+"_bbox.json"



        string measures_data_path;//= os.path.join(data_path,"shared_measures",self.shared_measures_filename)
        string relations_data_path;
        string score_weights_data_path;
        string body_height_Z_parts_data_path;



        
        public bool measures_database_exist;// = false
        string shared_morph_data_path;//= os.path.join(data_path,"shared_morphs",self.shared_morphs_filename)


        string bounding_box_path;//= os.path.join(data_path,"shared_bboxes",self.shared_bbox_filename)

        //ArrayList verts_to_update;//= set()

        //ArrayList morph_data_cache;// = {}
        //ArrayList forma_data;//= None

        //ArrayList boundary_verts;//= None
        //ArrayList measures_data ;//= {}
        //ArrayList measures_relat_data ;//= []
        //ArrayList measures_score_weights ;//= {}
        //ArrayList body_height_Z_parts ;//= {}

     //   ArrayList proportions;//= {}


        public Dictionary<string, float> char_data;

      
        string category_name;
        private bool update_directly_verts ;
            private bool update_geometry_all ;
            private bool update_geometry_selective ;
            private bool update_armature ;
            private bool update_normals ;
            private bool update_proxy ;
            private bool update_measures ;
            private bool sync_morphdata ;
            private bool sync_GUI ;
            private bool sync_GUI_metadata ;
        public Humanoid(string myName)
        {
            initFileNames(myName);

            wished_measures = new Dictionary<string, float>();

        }

        private void initFileNames(string myName)
        {
            Name = myName;
           
            NameGeneral = Name.Substring(0, Name.Length - 2);

            m_engine = new morph_engine(Name);
            morph_data = new List<Morph>();
            bbox_data = new List<BoundingBox>();


            //Path = GLSettings.Path + GLSettings.PathCharacters;
            PathCharacters = AppDomain.CurrentDomain.BaseDirectory + GLSettings.PathCharacters;

            morph_data_path = PathCharacters + "\\" + Name + "\\morphs.json"; //" + shared_measures_filename;//= os.path.join(data_path,self.obj_name,"morphs.json")
            morph_forma_path = PathCharacters + "\\" + Name + "\\forma.json"; //= os.path.join(data_path,self.obj_name,"forma.json")
            vertices_path = PathCharacters + "\\" + Name + "\\vertices.json";//= os.path.join(data_path,self.obj_name,"vertices.json")
            expressions_path = PathCharacters + "\\" + Name + "\\expressions.json"; //= os.path.join(data_path,self.obj_name,"expressions.json")

            test_path = PathCharacters + "\\" + Name + "\\test.json"; //= os.path.join(data_path,self.obj_name,"expressions.json")

            shared_morphs_filename = NameGeneral + ".json"; //obj.name[:len(obj.name)-2]+".json"
            //shared_measures_filename = nameGeneral + "_measures.json"; // = obj.name[:len(obj.name)-2]+"_measures.json"
            shared_measures_filename = NameGeneral + "_measuresNew.json"; // = obj.name[:len(obj.name)-2]+"_measures.json"

            shared_bbox_filename = NameGeneral + "_bbox.json"; //= obj.name[:len(obj.name)-2]+"_bbox.json"


            //measures_data_path = path + "\\shared_measures\\" + shared_measures_filename;//= os.path.join(data_path,"shared_measures",self.shared_measures_filename)
            measures_data_path = PathCharacters + "\\shared_measures\\" + shared_measures_filename;//= os.path.join(data_path,"shared_measures",self.shared_measures_filename)

            if(System.IO.File.Exists(measures_data_path))
                measures_database_exist = true;
            shared_morph_data_path = PathCharacters + "\\shared_morphs\\" + shared_morphs_filename;//= os.path.join(data_path,"shared_morphs",self.shared_morphs_filename)
            bounding_box_path = PathCharacters + "\\shared_bboxes\\" + shared_bbox_filename;//= os.path.join(data_path,"shared_bboxes",self.shared_bbox_filename)

            relations_data_path = PathCharacters + "\\shared_measures\\" + NameGeneral + "_relations.json";
            score_weights_data_path = PathCharacters + "\\shared_measures\\" + NameGeneral + "_score_weights.json";
            body_height_Z_parts_data_path = PathCharacters + "\\shared_measures\\" + NameGeneral + "_body_height_Z_parts.json";




        }
    
        public void init_database()
        {

            //vectors
            DeserializeVectors();

            m_engine.init_final_form(this);
            //morphs
            this.morph_data = load_morphs_database(this.morph_data_path);
                //printMorphs(this.morph_data);
            List<Morph> listNew = load_morphs_database(this.shared_morph_data_path);
            morph_data.AddRange(listNew);
                //printMorphs(listNew);
            listNew = load_morphs_database(this.expressions_path);
                morph_data.AddRange(listNew);
                //printMorphs(listNew);
            //bounding boxes
            this.bbox_data = DeserializeBoundingBoxes(this.bounding_box_path);

            //measures etc
            measures_data = DeserializeMeasures(this.measures_data_path);
            this.m_engine.measures_score_weights = JsonConvert.DeserializeObject<Dictionary<string, float>>(File.ReadAllText(this.score_weights_data_path));
            this.body_height_Z_parts = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(this.body_height_Z_parts_data_path));
            this.measures_relat_data = JsonConvert.DeserializeObject<List<string[]>>(File.ReadAllText(this.relations_data_path));

            m_engine.measures = this.calculate_measures(this.Vectors);

            //mat_engine...

            //....................
           
            

            //joints
            string shared_joints_path = PathCharacters + "\\shared_joints\\" + NameGeneral + "_joints.json"; //obj.name[:len(obj.name)-2]+"_bodies"
            joints = JsonConvert.DeserializeObject<Dictionary<string, List<uint>>>(File.ReadAllText(shared_joints_path));

            //printJoints();
            init_character_data();
                //printCategories();
            init_delta_measures();
            load_transformation_database();


           

        }
   
        private float search_best_value(string m_name, float wished_measure, HumanModifier human_modifier, string prop)
        {

            this.character_data[prop] = 0.5f;

            this.combine_morphings(human_modifier, false, true);

            //float measure2 = this.calculate_measures(null, m_name);
            float measure2 = this.calc_measure_float(m_name, null);

            string delta_name = human_modifier.name + prop;

            KeyValuePair<string, float[]> kvp = Utils.GetValueFromList(this.delta_measures, delta_name);

            float delta1 = kvp.Value[0];
            float delta3 = kvp.Value[1];

            float measure1 = measure2 + delta1;
            float measure3 = measure2 + delta3;

            float xa;
            float xb;
            float ya;
            float yb;
            float value;
            if (wished_measure < measure2)
            {
                xa = 0f;
                xb = 0.5f;
                ya = measure1;
                yb = measure2;
            }
            else
            {
                xa = 0.5f;
                xb = 1f;
                ya = measure2;
                yb = measure3;
            }

            if (ya - yb != 0)
            {
                value = Utils.linear_interpolation_y(xa, xb, ya, yb, wished_measure);

                if (value < 0)
                {
                    value = 0;
                }
                if (value > 1)
                {
                    value = 1;
                }
            }
            else
            {
                value = 0.5f;
            }
            return value;



        }
        private void measure_fitting(Dictionary<string, float> wished_measures , bool mix )
        {
            if (this.measures_database_exist)
            {
                foreach (string[] relation in this.measures_relat_data)
                {
                    string measure_name = relation[0];
                    string modifier_name = relation[1];
                    if (wished_measures.ContainsKey(measure_name) )
                    {
                        float wish_measure = wished_measures[measure_name];

                        foreach (HumanCategory category in this.categories)
                        {
                            foreach (HumanModifier modifier in category.modifiers)
                            {
                                if (modifier.name == modifier_name)
                                {
                                    foreach (string prop in modifier.properties)
                                    {

                                        if (mix)
                                        {
                                            float best_val = this.search_best_value(measure_name, wish_measure, modifier, prop);
                                            float value = (this.character_data[prop] + best_val) / 2;
                                            this.character_data[prop] = value;
                                        }
                                        else
                                        {
                                            this.character_data[prop] = this.search_best_value(measure_name, wish_measure, modifier, prop);
                                        }
                                    }
                                    this.combine_morphings(modifier, false, true);
                                }
                            }
                        }
                    }
                }


            }
 
        }
        private Dictionary<string, float> SetWishedMeasures()
        {
            Dictionary<string, float> wished_measures = new Dictionary<string, float>();

            return wished_measures;

        }
        public void UpdateModel_WishedMeasures(bool mix)
        {
            int n_samples = 3;

            //m_engine.calculate_proportions(char_data);
            m_engine.calculate_proportions(wished_measures);

            List<KeyValuePair<string, float>> similar_characters_data = m_engine.compare_data_proportions();


            float v = similar_characters_data[0].Value;

            KeyValuePair<string, float> best_character = similar_characters_data[0];

            string filepath = best_character.Key;

            load_character(filepath, "nothing", true, false, UpdateMode.update_all);


            for (int i = 1; i <= n_samples; i++)
            {
                KeyValuePair<string, float> char_data = similar_characters_data[i];

                filepath = char_data.Key;
                this.load_character(filepath, "nothing", true, true, UpdateMode.update_all);


            }

            this.measure_fitting(wished_measures, mix);
            this.update_character(UpdateMode.update_directly_verts);

        }
    
        public void automodelling(bool use_measures_from_GUI, Dictionary<string, float> use_measures_from_dict, bool use_measures_from_current_obj, bool mix)
        {
            
            Dictionary<string, float> wished_measures = new Dictionary<string, float>();

           
            if (use_measures_from_GUI)
            {
                float conversion_factor = 100f;


                
                foreach (string measure_name in this.m_engine.measures.Keys)
                {
                    if (measure_name != "body_height_Z")
                    {
                        wished_measures.Add(measure_name, this.m_engine.measures[measure_name] / conversion_factor);
                    }
                }

                float total_height_Z = 0f;
                foreach (string measure_name in this.body_height_Z_parts)
                {
                    total_height_Z += wished_measures[measure_name];
                }

                wished_measures["body_height_Z"] = total_height_Z;
            }



            if (use_measures_from_current_obj)
            {
                List<Vector3> current_shape_verts = new List<Vector3>();
                foreach (Vector3 vert in this.Vectors)
                    current_shape_verts.Add(vert.Clone());
                wished_measures = this.calculate_measures(current_shape_verts);
            }
            if (use_measures_from_dict != null)
                wished_measures = use_measures_from_dict;

            UpdateModel_WishedMeasures(mix);

        

        }

        private void load_character(string fileName, string reset_string, bool reset_unassigned, bool mix, UpdateMode update_mode)
        {
            //char_data = JsonConvert.DeserializeObject<Dictionary<string, float>>(File.ReadAllText(newMeasures));

            Dictionary<string, Dictionary<string, float>> charac_data = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, float>>>(File.ReadAllText(fileName));
            Dictionary<string, float> char_data;
            Dictionary<string, float> material_data;
            Dictionary<string, float> meta_data;



            if (charac_data.ContainsKey("structural"))
                char_data = charac_data["structural"];
            else
                char_data = null;

            if (charac_data.ContainsKey("materialproperties"))
                material_data = charac_data["materialproperties"];
            else
                material_data = null;

            if (charac_data.ContainsKey("metaproperties"))
                meta_data = charac_data["metaproperties"];
            else
                meta_data = null;


            if (char_data != null)
            {

                List<string> keysList = new List<string>(character_data.Keys);
                foreach (string myKeyName in keysList)                                  
                {
                   
                    if (myKeyName.Contains(reset_string))
                    {
                        character_data[myKeyName] = 0.5f;
                    }
                    if (char_data.ContainsKey(myKeyName))
                    {
                        if (mix)
                            character_data[myKeyName] = (character_data[myKeyName] + char_data[myKeyName]) / 2;
                        else
                            character_data[myKeyName] = char_data[myKeyName];
                    }
                    else
                    {
                        if (reset_unassigned)
                            if (mix)
                                character_data[myKeyName] = (character_data[myKeyName] + 0.5f) / 2f;
                            else
                                character_data[myKeyName] = 0.5f;
                    }
                }

            }

            List<string> keysListNew = new List<string>(character_metaproperties.Keys);
            foreach (string myname in keysListNew)
            {
                if (meta_data[myname] != 0f)
                    this.character_metaproperties[myname] = meta_data[myname];

            }

            

            //foreach (string name in character_material_properties.Keys)
            //    if( material_data[name] != null)
            //        this.character_material_properties[name] = material_data[name];

            this.update_character(update_mode);//mode = update_mode);

        }
        //load_character( , "nothing", false, false, UpdateMode.UpdateAll)

      
        private void clean_verts_to_process()
        {
            m_engine.verts_to_update.Clear();
        }
        public void update_character(UpdateMode mode)
        {

            this.clean_verts_to_process();

            if (mode == UpdateMode.test)
            {
                update_directly_verts = false;
                update_geometry_all = false;
                update_geometry_selective = false;
                update_armature = false;
                update_normals = false;
                update_proxy = false;
                update_measures = true;
                sync_morphdata = false;
                sync_GUI = true;
                sync_GUI_metadata = true;
            }

            if (mode == UpdateMode.update_all)
            {
                update_directly_verts = false;
                update_geometry_all = true;
                update_geometry_selective = false;
                update_armature = true;
                update_normals = true;
                update_proxy = false;
                update_measures = true;
                sync_morphdata = false;
                sync_GUI = true;
                sync_GUI_metadata = true;
            }

            if (mode == UpdateMode.update_metadata)
            {
                update_directly_verts = false;
                update_geometry_all = true;
                update_geometry_selective = false;
                update_armature = true;
                update_normals = true;
                update_proxy = false;
                update_measures = true;
                sync_morphdata = false;
                sync_GUI = true;
                sync_GUI_metadata = false;
            }

            if (mode == UpdateMode.update_directly_verts)
            {
                update_directly_verts = true;
                update_geometry_all = false;
                update_geometry_selective = false;
                update_armature = true;
                update_normals = true;
                update_proxy = false;
                update_measures = true;
                sync_morphdata = false;
                sync_GUI = true;
                sync_GUI_metadata = false;
            }

            if (mode == UpdateMode.update_only_morphdata)
            {
                update_directly_verts = false;
                update_geometry_all = false;
                update_geometry_selective = false;
                update_armature = false;
                update_normals = false;
                update_proxy = false;
                update_measures = false;
                sync_morphdata = false;
                sync_GUI = false;
                sync_GUI_metadata = false;
            }

            if (mode == UpdateMode.update_realtime)
            {
                update_directly_verts = false;
                update_geometry_all = false;
                update_geometry_selective = true;
                update_armature = true;
                update_normals = false;
                update_proxy = false;
                update_measures = false;
                sync_morphdata = true;
                sync_GUI = false;
                sync_GUI_metadata = false;
            }


            if (update_directly_verts)
            {
                // this.m_engine.update(update_all_verts=true);
            }
            else
            {
                //if( category_name != null)
                //{

                //    HumanCategory category = HumanCategory.GetByName(categories, category_name);
                //    List<HumanModifier> modified_modifiers = new List<HumanModifier>();
                //    //foreach( Modifier modifier in category.modifiers)
                //    //{
                //    //    if( modifier.is_changed(this.character_data))
                //    //    {
                //    //        modified_modifiers.Add(modifier);
                //    //    }
                //    //}
                //    foreach( HumanModifier modifier in modified_modifiers)
                //    {
                //        if( sync_morphdata)
                //        {
                //            modifier.sync_modifier_data_to_obj_prop(this.character_data);
                //        }
                //        this.combine_morphings(modifier);
                //    }
                //}
                //else
                //{
                foreach (HumanCategory category in this.categories)
                {
                    foreach (HumanModifier modifier in category.modifiers)
                    {
                        this.combine_morphings(modifier, false, true);
                    }
                }
                //}
            }

            if (update_geometry_all)
            {
                this.m_engine.update(this, true);
            }
            if (update_geometry_selective)
            {
                this.m_engine.update(this, false);
            }
            //if( sync_GUI)
            //{
            //    this.sync_obj_props_to_character_data();
            //}
            //if( sync_GUI_metadata)
            //{
            //    this.sync_obj_props_to_character_metadata();
            //}
            //if( update_measures)
            //{
            //    this.sync_gui_according_measures();
            //}
            //if( update_armature)
            //{
            //    this.armat.fit_joints();
            //}
            //if( update_normals)
            //{
            //    obj.data.calc_normals();
            //}
            //if( update_proxy)
            //{
            //    this.load_proxy();
            //}
            this.Vectors = this.m_engine.final_form;
        }
       
        private void Test()
        {
            Dictionary<string, List<float[]>> l = new Dictionary<string, List<float[]>>();
            List<float[]> lfloat = new List<float[]>();
            lfloat.Add(new float[4] { 1f, 2f, 3f, 4f });
            lfloat.Add(new float[4] { 1f, 2f, 3f, 4f });


            l.Add("Lid", lfloat);
            l.Add("Head", lfloat);

            File.WriteAllText(this.test_path, JsonConvert.SerializeObject(l));

        }
        private void sync_gui_according_measures()
        {
            Dictionary<string, float> measures = this.calculate_measures(null);
            //convert_to_inch = getattr(obj, "use_inch", False);
            //if( convert_to_inch)
            //{
            //    conversion_factor = 39.37001;
            //}
            //else
            //{
            //float conversion_factor = 100f;
            //}

            foreach (KeyValuePair<string, float> m in measures)
            {
                //EDGAR TODO!!
                // setattr(obj, measure_name, measure_val * conversion_factor);
            }
        }
        private void Test1()
        {
            Dictionary<string, List<object>> l = new Dictionary<string, List<object>>();

            l.Add("entry1", new List<object> { "jk", 0.1f, 0.2f });

            l.Add("entry2", new List<object> { "jk", 0.1f, 0.2f });


            File.WriteAllText(this.test_path, JsonConvert.SerializeObject(l));

        }
        private void load_transformation_database()
        {
           
            string shared_transform_data_path = this.PathCharacters + "\\shared_transformations\\" + this.NameGeneral + "_transf.json";
            //Dictionary<string, Dictionary<string, float[]>> sharedData = new Dictionary<string, Dictionary<string, float[]>>();


            Dictionary<string, List<object>> transformations_data = new Dictionary<string, List<object>>();



            if (System.IO.File.Exists(shared_transform_data_path))
            {
                transformations_data = JsonConvert.DeserializeObject<Dictionary<string, List<object>>>(File.ReadAllText(shared_transform_data_path));
                //database_file = open(this.shared_transform_data_path, "r");
                //try
                //{
                //    this.transformations_data = json.load(database_file);

                //}

            }
        }
        

        private float function_modifier_a(float val_x)
        {
            float val_y = 0.0f;
            if (val_x > 0.5f)
            {
                val_y = 2 * val_x - 1;
            }
            return val_y;

        }
        private float function_modifier_b(float val_x)
        {
            float val_y = 0.0f;
            if (val_x < 0.5f)
            {
                val_y = 1 - 2 * val_x;
            }
            return val_y;
        }
        private float Max(List<float> list)
        {
            float f = float.MinValue;
            for(int i = 0; i < list.Count; i++)
            {
                f = Math.Max(f, list[i]);
            }
            return f;
        }
        private void smart_combo(string prefix, List<KeyValuePair<float, float>> morph_values, out List<string> names, out List<float> weights)
        {
            //return names/weights

            List<string> tags = new List<string>() {"max", "min"};
            names = new List<string>();
            //weights = [];
            List<float> max_morph_values = new List<float>();
          
            //Compute the combinations and get the max values;
            foreach (KeyValuePair<float, float> pair in morph_values)
            {
                
                max_morph_values.Add(Math.Max(pair.Key, pair.Value));

            }

            for(int i = 0; i < tags.Count; i++)
            {
                names.Add(prefix + "_" + tags[i]);
            }
            //        }


            weights = new List<float>();
            ////Compute the weight of each combination;
            foreach (KeyValuePair<float, float> kv in morph_values)
            {
                weights.Add(kv.Key);
                weights.Add(kv.Value);

            }

            float factor = Max(max_morph_values);
            float best_val = Max(weights);
            float toll = 1.5f;

            //Filter on bestval and calculate the normalize factor;
            float summ = 0.0f;
            for(int i = 0; i <weights.Count; i++)
            {
                weights[i] = Math.Max(0, weights[i]-best_val/toll);
                summ += weights[i];
            }

            //Normalize using summ;
            if( summ != 0)
            {
                for(int i = 0; i < weights.Count; i++)
                {
                    weights[i] = factor*(weights[i]/summ);
                }
            }


        }

        private void combine_morphings(HumanModifier modifier, bool refresh_only, bool add_vertices_to_update)
        {
            //Dictionary<float, float> values = new Dictionary<float, float>();
            List<KeyValuePair<float, float>> values = new List<KeyValuePair<float, float>>();

            foreach (string prop in modifier.properties)
            {
                float val = this.character_data[prop];
                if (val > 1.0f)
                {
                    val = 1.0f;
                }
                if (val < 0f)
                {
                    val = 0f;
                }
                float val1 = function_modifier_a(val);
                float val2 = function_modifier_b(val);
                values.Add(new KeyValuePair<float, float> (val1, val2));

            }
            List<float> weights;
            List<string> names;
            smart_combo(modifier.name, values, out names, out weights);

            for (int i = 0; i < names.Count; i++ )
            {
                if (refresh_only)
                {
                    Morph.GetFromListByName(this.morph_data, names[i]).morph_values = weights[i];
                }
                else
                {
                    this.calculate_morph(names[i], weights[i], true);
                }
            }

        }
        private void calculate_morph(string morph_name, float val, bool add_vertices_to_update)
        {
            Morph morph = Morph.GetFromListByName(this.morph_data, morph_name);
            if (morph != null)
            {

                float real_val = val - Morph.GetFromListByName(this.morph_data, morph_name).morph_values;
                //if (this.morph_values.ContainsKey(real_val))
                if (real_val != 0.0f)
                {
                    foreach (KeyValuePair<uint, Vector3> kv in morph.morph_data)
                    {

                        int i = Convert.ToInt32(kv.Key);
                        Vector3 delta = kv.Value;
                        this.m_engine.final_form[i] = this.m_engine.final_form[i] + delta * real_val;
                    }

                    if (add_vertices_to_update)
                        this.m_engine.verts_to_update = this.m_engine.verts_to_update.Union(morph.morph_modified_verts).ToList<uint>();

                    Morph.GetFromListByName(this.morph_data, morph_name).morph_values = val;
                    //this.morph_values[morph_name] = val;

                }
            }
        }
        private float full_dist(Vector3 v1, Vector3 v2, string axis)
        {
            if (string.IsNullOrEmpty(axis))
                axis = "ALL";

            Vector3 v3 = Vector3.Zero;
           
            if (axis == "X")
            {
                return Math.Abs(v1[0] - v2[0]);
            }
            if (axis == "Y")
            {
                return Math.Abs(v1[1] - v2[1]);
            }
            if (axis == "Z")
            {
                return Math.Abs(v1[2] - v2[2]);
            }

            v3 = v1 - v2;
            return v3.Length;
            
        }





        private float length_of_strip(List<Vector3> vertices_coords, List<int> indices, string axis)
        {

            float strip_length = 0;
            for(int x = 0; x < indices.Count -1; x++)
            {
                Vector3 v1 = vertices_coords[indices[x]];
                Vector3 v2 = vertices_coords[indices[x + 1]];
                strip_length += full_dist(v1, v2, axis);
            }

            return (strip_length);

        }

        private float calc_measure_float(string measure_name, List<Vector3> vert_coords)
        {
            if (vert_coords == null)
                vert_coords = this.m_engine.final_form;
            MeasureData m = MeasureData.GetFromListByName(measures_data, measure_name);
            float f = 0f;
            if (m != null)
            {
                //indices =  this.measures_data[measure_name];
                string axis = measure_name[measure_name.Length - 1].ToString();
                f = length_of_strip(vert_coords, m.Indices, axis);
            }
            return f;

        }
     


        /// <summary>
        /// 
        /// </summary>
        /// <param name="measure_name"></param>
        /// <param name="vert_coords"></param>
        /// <returns></returns>
        private Dictionary<string, float> calculate_measures(List<Vector3> vert_coords)
        {
            Dictionary<string, float> measures = new Dictionary<string, float>();
          
            for (int i = 0; i < measures_data.Count; i++)
            {
               
                float f = calc_measure_float(measures_data[i].Name, vert_coords);

                measures.Add(measures_data[i].Name, f);

            }
          
            return measures;
        }
        private void init_delta_measures()
        {
            for (int i = 0; i < measures_relat_data.Count; i++)
            {
                string[] relation = measures_relat_data[i];
                string m_name = relation[0];
                string modifier_name = relation[1];
                for (int j = 0; j < this.categories.Count; j++)
                {
                    HumanCategory category = this.categories[j];
                    for (int k = 0; k < category.modifiers.Count; k++)
                    {
                        HumanModifier modifier = category.modifiers[k];
                        if (modifier.name == modifier_name)
                        {
                            foreach (string prop in modifier.properties)
                            {
                                this.character_data[prop] = 0.0f;
                                this.combine_morphings(modifier, false, true);
                                float measure1 = this.calc_measure_float(m_name, null);

                                this.character_data[prop] = 1.0f;
                                this.combine_morphings(modifier, false, true);
                                float measure3 = this.calc_measure_float(m_name, null);

                                //#Last measure also restores the value to 0.5
                                this.character_data[prop] = 0.5f;
                                this.combine_morphings(modifier, false, true);
                                float measure2 = this.calc_measure_float(m_name, null);

                                string delta_name = modifier_name + prop;

                                float delta1 = measure1 - measure2;
                                float delta3 = measure3 - measure2;


                                KeyValuePair<string, float[]> kvp = new KeyValuePair<string, float[]>(delta_name, new float[] { delta1, delta3 });
                                delta_measures.Add(kvp);

                            }
                        }
                    }

                }

            }
        }
     
        private void init_character_data()
        {
            categories = new List<HumanCategory>();


            for (int i = 0; i < this.morph_data.Count; i++)
            {
                string morph_name = this.morph_data[i].morph_name;
                string[] components = morph_name.Split(new Char[] { '_' });
                string compCut = components[0];
                if (components[0].Length >= 4)
                    compCut = components[0].Remove(4, components[0].Length - 4);

                //erst ab hier die Bastioni Funktion
                if (!this.no_categories.Contains(compCut))
                {
                    if (components.Length == 3)
                    {
                        category_name = components[0];
                        HumanCategory category = HumanCategory.GetByName(this.categories, category_name);

                        if (category == null)
                        {
                            category = new HumanCategory();
                            category.name = category_name;
                            this.categories.Add(category);
                        }


                        string modifier_name = components[0] + "_" + components[1];
                        HumanModifier modifier = HumanModifier.GetByName(category.modifiers, modifier_name);
                        if (modifier == null)
                        {
                            modifier = new HumanModifier();
                            modifier.name = modifier_name;
                            category.modifiers.Add(modifier);
                        }

                        string[] elements = components[1].Split(new Char[] { '-' });
                        for (int j = 0; j < elements.Length; j++)
                        {
                            string element = elements[j];
                            string prop = components[0] + "_" + element;
                            if (!modifier.properties.Contains(prop))
                            {
                                modifier.properties.Add(prop);

                            }
                            if (character_data.ContainsKey(prop))
                                character_data[prop] = 0.5f;
                            else
                                character_data.Add(prop, 0.5f);
                        }



                    }



                }


            }
        }
    
   
        //private void DeserializeVectors()
        //{
        //    List<float[]> listVectors = JsonConvert.DeserializeObject<List<float[]>>(File.ReadAllText(this.vertices_path));


        //    Vectors = new List<Vector3>();
        //    for (int i = 0; i < listVectors.Count; i++)
        //    {
        //        Vector3 v = new Vector3(listVectors[i][0], listVectors[i][1], listVectors[i][2]);
        //        Vectors.Add(v);
        //        m_engine.base_form.Add(v.Clone());
        //    }
        //}
        private void DeserializeVectors()
        {
            Vectors = JsonUtils.DeserializeVectors(this.vertices_path);

            m_engine.base_form = Vectors;

         
        }
     


        private List<MeasureData> DeserializeMeasures(string fileName)
        {
            if (!File.Exists(fileName))
                return null;
            List<MeasureData> list = new List<MeasureData>();

            Dictionary<string, List<int>> listRead = JsonConvert.DeserializeObject<Dictionary<string, List<int>>>(File.ReadAllText(fileName));

            foreach (var oBB in listRead)
            {

                MeasureData m = new MeasureData();
                m.Name = oBB.Key;
                m.Indices = oBB.Value;

                list.Add(m);
            }


            return list;

        }
        private List<MeasureData> DeserializeMeasuresOld(string fileName)
        {
            if (!File.Exists(fileName))
                return null;
            List<MeasureData> list = new List<MeasureData>();

            //var dyn = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(jsonAsFooString);
            using (StreamReader sr = new StreamReader(fileName))
            {
                string read = sr.ReadToEnd();
                dynamic x = Newtonsoft.Json.JsonConvert.DeserializeObject(read);
                var ff = Newtonsoft.Json.JsonConvert.DeserializeObject(read);



            }

            using (var reader = new StreamReader(fileName))
            using (var jsonReader = new JsonTextReader(reader))
            {
                jsonReader.SupportMultipleContent = true;



            }




            var serializer = new JsonSerializer();

            using (var reader = new StreamReader(fileName))
            using (var jsonReader = new JsonTextReader(reader))
            {
                jsonReader.SupportMultipleContent = true;

                while (jsonReader.Read())
                {
                    //yield return serializer.Deserialize<Measure>(jsonReader);
                }
            }


            return list;

        }
        private List<BoundingBox> DeserializeBoundingBoxes(string fileName)
        {
            List<BoundingBox> list = new List<BoundingBox>();
            Dictionary<uint, List<uint>> listBBRead = JsonConvert.DeserializeObject<Dictionary<uint, List<uint>>>(File.ReadAllText(fileName));

            foreach (var oBB in listBBRead)
            {
                BoundingBox b = new BoundingBox();
                b.idx = oBB.Key;
                b.idx_x_max = oBB.Value[0];
                b.idx_y_max = oBB.Value[1];
                b.idx_z_max = oBB.Value[2];
                b.idx_x_min = oBB.Value[3];
                b.idx_y_min = oBB.Value[4];
                b.idx_z_min = oBB.Value[5];
                list.Add(b);
            }
            return list;

        }
        private List<Morph> load_morphs_database(string fileName)
        {
            List<Morph> listMorphs = new List<Morph>();

            Dictionary<string, List<float[]>> listMorphsRead = JsonConvert.DeserializeObject<Dictionary<string, List<float[]>>>(File.ReadAllText(fileName));


            foreach (var oMorph in listMorphsRead)
            {
                Morph m = new Morph();
                m.morph_name = oMorph.Key;


                Dictionary<uint, Vector3> vDeltas = new Dictionary<uint, Vector3>();
                List<uint> vVerts = new List<uint>();

                foreach (var oDelta in oMorph.Value)
                {
                    uint ui = Convert.ToUInt32(oDelta[0]);
                    Vector3 v = new Vector3(oDelta[1], oDelta[2], oDelta[3]);
                    vDeltas.Add(ui, v);//append in python
                    vVerts.Add(ui);//add in python
                    ////vVerts.Insert(0, ui);//add in python


                }


                m.morph_data = vDeltas;
                m.morph_modified_verts = vVerts;
                m.morph_values = 0f;
                listMorphs.Add(m);

            }

            return listMorphs;

        }


    }


}
