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
    public class morph_engine
    {
        public Dictionary<string, float> measures;
        public Dictionary<string, float> measures_score_weights;

        public Dictionary<string, float> proportions;
        private List<string> similar_characters_data;


        public List<Vector3> final_form;
        public List<Vector3> base_form = new List<Vector3>();

        // scores;


        string bodies_data_path;//= os.path.join(data_path,"shared_bodies",self.shared_bodies_path)
        string shared_bodies_path;//= obj.name[:len(obj.name)-2]+"_bodies"
        string shared_measures_filename;// = obj.name[:len(obj.name)-2]+"_measures.json"


        public List<uint> verts_to_update = new List<uint>();

        public morph_engine(string name)
        {

            this.final_form = new List<Vector3>();

            string nameGeneral = name.Substring(0, name.Length - 2);
           
            string path = AppDomain.CurrentDomain.BaseDirectory + GLSettings.PathCharacters;

            //string path = GLSettings.Path + GLSettings.PathCharacters;
            bodies_data_path = path + "\\shared_bodies\\" + nameGeneral + "_bodies";
            //morph_data_path = path + "\\" + name + "\\morphs.json"; //" + shared_measures_filename;//= os.path.join(data_path,self.obj_name,"morphs.json")

            //bodies_data_path = path + "\\shared_bodies\\" + shared_measures_filename; //= os.path.join(data_path,"shared_bodies",self.shared_bodies_path)
            // shared_bodies_path = GLSettings.Path + "\\" + nameGeneral + "_bodies"; //obj.name[:len(obj.name)-2]+"_bodies"

        }

        public void calculate_proportions(Dictionary<string, float> measures)
        {
            proportions = new Dictionary<string, float>();

            if (measures.ContainsKey("body_height_Z"))
            {
                float f = measures["body_height_Z"];
                foreach (KeyValuePair<string, float> v in measures)
                {
                    float prop = v.Value / f;
                    if (measures.ContainsKey(v.Key))
                    {
                        proportions.Add(v.Key, prop);
                    }
                }
            }

            //    for measure, value in measures.items():
            //        proportion = value/measures["body_height_Z"]
            //        if measure in self.measures:
            //            self.proportions[measure] = proportion
            //        else:
            //            lab_logger.warning("The measure {0} not present in the proportion database".format(measure))
            //else:
            //    lab_logger.error("The base measure not present in the analyzed database")
            //}
        }
        private float compare_file_proportions(string fileName)
        {
            Dictionary<string, Dictionary<string, float>> char_data = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, float>>>(File.ReadAllText(fileName));
            if (char_data.ContainsKey("proportions"))
            {
                return calculate_matching_score(char_data["proportions"]);
            }
            return 0f;
            //if( "proportions" in char_data)
            //{
            //    return (this.calculate_matching_score(char_data["proportions"]),filepath);
            //}
            //else
            //{
            //    lab_logger.info("File {0} does not contain proportions".format(algorithms.simple_path(filepath)));
            //}
        }
        private float calculate_matching_score(Dictionary<string, float> myproportions)
        {
            float data_score = 0f;
            float soglia = 0.025f;

            foreach (KeyValuePair<string, float> v in myproportions)
            {

                if (this.proportions.ContainsKey(v.Key))
                {
                    if (v.Key != "body_height_Z")
                    {
                        float proportion_score = 1f;
                        float difference_of_proportion = Math.Abs(this.proportions[v.Key] - v.Value);
                        if (difference_of_proportion > soglia)
                        {
                            proportion_score = 0;
                        }
                        data_score += proportion_score * this.measures_score_weights[v.Key];
                    }
                }
            }
            //else
            //{
            //    System.Diagnostics.Debug.WriteLine("Measure {0} not present in inner proportions database" + );
            //}

            return data_score;


        }
        public List<KeyValuePair<string, float>> compare_data_proportions()
        {

            //scores = new SortedDictionary<string, float>();
            List<KeyValuePair<string, float>> scores = new List<KeyValuePair<string, float>>();



            if (System.IO.Directory.Exists(bodies_data_path))
            {
                string[] files = System.IO.Directory.GetFiles(bodies_data_path);
                foreach (string fileName in files)
                {
                    scores.Add(new KeyValuePair<string, float>(fileName, compare_file_proportions(fileName)));

                    //     scores.Add(this.compare_file_proportions(os.path.join(this.bodies_data_path,database_file)));

                }

                scores.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));

                //scores = scores.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            }

            return scores;

        }
        public void update(Humanoid obj, bool update_all_verts)
        {

            if (update_all_verts)
            {
                if (final_form != null)
                {
                    for (int i = 0; i < this.final_form.Count; i++ )
                    {
                        obj.Vectors[i] = this.final_form[i];
                    }
                }
            }
            else
            {
                if (verts_to_update != null)
                {
                    for (int i = 0; i < this.verts_to_update.Count; i++)
                    {
                        obj.Vectors[i] = this.final_form[i];
                    }
                }
            }
        }
        //   /// <summary>
        ///// 
        ///// </summary>
        ///// <param name="measure_name"></param>
        ///// <param name="vert_coords"></param>
        ///// <returns></returns>
        //private Dictionary<string, float> calculate_measures(List<Vector3> vert_coords)
        //{
        //    Dictionary<string, float> measures = new Dictionary<string, float>();

        //    for (int i = 0; i < measures_data.Count; i++)
        //    {
        //        string name = measures_data[i].Name;
        //        float f = calc_measure_float(measures_data[i].Name, vert_coords);

        //        measures.Add(measures_data[i].Name, f);

        //    }


        //    return measures;
        //}
        public void init_final_form(Humanoid obj)
        {

            foreach (Vector3 vert in obj.Vectors)
                final_form.Add(vert.Clone());
        }
        public void reset(bool doupdate, Humanoid h)
        {
            this.final_form = new List<Vector3>();
            for (int i = 0; i < this.base_form.Count; i++)
            {
                this.final_form.Add(this.base_form[i].Clone());

            }

             foreach (Morph mo in h.morph_data)
            {
                 mo.morph_values = 0f;
                 if (doupdate)
                    update(h, true);

             }
            //Morph.GetFromListByName(h.morph_data, )
            //List<string> keyList = h.morph_values.Keys.ToList<string>();
            //foreach (string morph_name in keyList)
            //{
            //    h.morph_values[morph_name] = 0.0f;
            //    if (doupdate)
            //        update(h, true);
            //}
        }
    }
}
