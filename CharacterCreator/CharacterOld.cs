using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.IO;
using System.Collections;

using OpenTK;
using OpenTKLib;

namespace CharacterCreator
{
    public class CharacterOld
    {
        string path;
        string name;
        //public Vector3[] Vectors;
        public List<Vector3> Vectors;
    
        ArrayList base_form;
        ArrayList final_form ;
        ArrayList cache_form ;

        string vertices_path;//= os.path.join(data_path,self.obj_name,"vertices.json")
        string morph_data_path;//= os.path.join(data_path,self.obj_name,"morphs.json")
        string morph_forma_path;//= os.path.join(data_path,self.obj_name,"forma.json")
        string expressions_path;//= os.path.join(data_path,self.obj_name,"expressions.json")

        string test_path;//= os.path.join(data_path,self.obj_name,"expressions.json")


        string shared_morphs_filename; //obj.name[:len(obj.name)-2]+".json"
        string shared_bodies_path ;//= obj.name[:len(obj.name)-2]+"_bodies"
        string shared_measures_filename;// = obj.name[:len(obj.name)-2]+"_measures.json"
        string shared_bbox_filename ;//= obj.name[:len(obj.name)-2]+"_bbox.json"



        string measures_data_path ;//= os.path.join(data_path,"shared_measures",self.shared_measures_filename)
        string bodies_data_path ;//= os.path.join(data_path,"shared_bodies",self.shared_bodies_path)
        bool measures_database_exist;// = False
        string shared_morph_data_path;//= os.path.join(data_path,"shared_morphs",self.shared_morphs_filename)
      
     
         string bounding_box_path ;//= os.path.join(data_path,"shared_bboxes",self.shared_bbox_filename)
      
           ArrayList verts_to_update ;//= set()
            ArrayList morph_data ;//= {}
            ArrayList morph_data_cache;// = {}
            ArrayList forma_data ;//= None
            ArrayList bbox_data ;//= {}
            ArrayList morph_values ;//= {}
            ArrayList morph_modified_verts ;//= {}
            ArrayList boundary_verts ;//= None
            ArrayList measures_data ;//= {}
            ArrayList measures_relat_data ;//= []
            ArrayList measures_score_weights ;//= {}
            ArrayList body_height_Z_parts ;//= {}

            ArrayList proportions ;//= {}
            public CharacterOld(string myName)
        {
            name = myName;
            path = GLSettings.Path + GLSettings.PathCharacters;
            
            morph_data_path = path + "\\" + name + "\\morphs.json"; //" + shared_measures_filename;//= os.path.join(data_path,self.obj_name,"morphs.json")
            morph_forma_path = path + "\\" + name + "\\forma.json"; //= os.path.join(data_path,self.obj_name,"forma.json")
            vertices_path = path + "\\" + name + "\\vertices.json";//= os.path.join(data_path,self.obj_name,"vertices.json")
            expressions_path = path + "\\" + name + "\\expressions.json"; //= os.path.join(data_path,self.obj_name,"expressions.json")

            test_path = path + "\\" + name + "\\test.json"; //= os.path.join(data_path,self.obj_name,"expressions.json")


            string nameGeneral = name.PadRight(2);
            shared_bodies_path = GLSettings.Path + "\\" + nameGeneral + "_bodies"; //obj.name[:len(obj.name)-2]+"_bodies"
            shared_morphs_filename = nameGeneral + ".json"; //obj.name[:len(obj.name)-2]+".json"
            shared_measures_filename = nameGeneral + "_measures.json"; // = obj.name[:len(obj.name)-2]+"_measures.json"
            shared_bbox_filename = nameGeneral + "_bbox.json"; //= obj.name[:len(obj.name)-2]+"_bbox.json"



            measures_data_path = path + "\\sharedMeasures\\" + shared_measures_filename;//= os.path.join(data_path,"shared_measures",self.shared_measures_filename)
            bodies_data_path = path + "\\sharedMeasures\\" + shared_measures_filename; //= os.path.join(data_path,"shared_bodies",self.shared_bodies_path)
            shared_morph_data_path = path + "\\sharedMorphs\\" + shared_morphs_filename;//= os.path.join(data_path,"shared_morphs",self.shared_morphs_filename)
            bounding_box_path = path + "\\sharedbboxes\\" + shared_bbox_filename;//= os.path.join(data_path,"shared_bboxes",self.shared_bbox_filename)
            
            
        }

        private void DeserializeVectors()
        {
            //JsonSerializer.SerializeListFloats(new Object());
            List<float[]> listVectors = JsonSerializer.DeserializeFromFile<List<float[]>>(this.vertices_path);
            //Vectors = new Vector3[listVectors.Count];
            Vectors = new List<Vector3>();
            for(int i = 0; i < listVectors.Count; i++)
            {
                Vector3 v = new Vector3(listVectors[i][0], listVectors[i][1], listVectors[i][2]);
                Vectors.Add(v);
                //Vectors[i] = v;
            }
        }
        public PointCloud ToPointCloud()
        {
            PointCloud pc = PointCloud.FromVector3List(this.Vectors);

            //pc.Vectors = Vectors;
            return pc;

        }
        public void DeserializeAll()
        {
            DeserializeVectors();
            DeserializeMorphs();
            // self.load_morphs_database(self.morph_data_path)
            //self.load_morphs_database(self.shared_morph_data_path)
            //self.load_morphs_database(self.expressions_path)
            //self.load_bboxes_database(self.bounding_box_path)
            //self.load_measures_database(self.measures_data_path)

        }
        private void DeserializeMorphs()
        {


            //List<Morphs> listMorphClasses = JsonSerializer.DeserializeFromFile<List<Morphs>>(this.morph_data_path);

            //List<Morphs> listMorphClasses = new List<Morphs>();
            //Morphs m = new Morphs();

            //m.Name = "MyMorph";
            //m.Data = new List<float[]>();
            //m.Data.Add(new float[4] {1f, 2f, 3f, 4f});
            //m.Data.Add(new float[4] { 0f,1f, 3f, 4f });

            //listMorphClasses.Add(m);
            //listMorphClasses.Add(m);

            //JsonSerializer.SerializeToFile<List<Morphs>>(listMorphClasses, this.test_path);

            Dictionary<string, List<float[]>> l = new Dictionary<string, List<float[]>>();
            List<float[]> lfloat = new List<float[]>();
            lfloat.Add(new float[4] {1f, 2f, 3f, 4f});
            lfloat.Add(new float[4] {1f, 2f, 3f, 4f});
        
            
            l.Add("Lid", lfloat);
            l.Add("Head", lfloat);

            JsonSerializer.SerializeToFile <Dictionary<string, List<float[]>>> (l, this.test_path);

            //System.Collections.Generic.
            //object o = JsonSerializer.DeserializeFromFile<object>(this.morph_data_path);


            KeyValuePair<string, List<float[]>> v = new KeyValuePair<string, List<float[]>>();
            //v.Key = "Data";
            float[] val = new float[4];
            val[0] = 1;

            //v.Value

            List<KeyValuePair<string, List<float[]>>> listMorphs = JsonSerializer.DeserializeFromFile<List<KeyValuePair<string, List<float[]>>>>(this.morph_data_path);

                //     def load_morphs_database(self, morph_data_path):
        //time1 = time.time()
        //if os.path.isfile(morph_data_path):
        //    database_file = open(morph_data_path, "r")
        //    m_data = json.load(database_file)
        //    database_file.close()

        //    for morph_name, deltas in m_data.items():
        //        morph_deltas = []
        //        modified_verts = set()
        //        for d_data in deltas:
        //            t_delta = Vector(d_data[1:])
        //            morph_deltas.append([d_data[0], t_delta])
        //            modified_verts.add(d_data[0])
        //        if morph_name in self.morph_data:
        //            lab_logger.warning("Morph {0} duplicated while loading morphs from file".format(morph_name))

        //        self.morph_data[morph_name] = morph_deltas
        //        self.morph_values[morph_name] = 0.0
        //        self.morph_modified_verts[morph_name] = modified_verts
        //    lab_logger.info("Morph database {0} loaded in {1} secs".format(algorithms.simple_path(morph_data_path),time.time()-time1))
        //else:
        //    self.error_msg(morph_data_path)

        }

    }
    //[DataContract]
    //public class Morphs
    //{
    //    [DataMember(Name = "", Order = 1)]
    //    public string Name;
    //    [DataMember(Name = "", Order = 2)]
    //    public List<float[] >Data;


    //}
}
