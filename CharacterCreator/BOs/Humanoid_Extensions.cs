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

        
        public List<Vector3> FaceVectors;
        public List<uint> FaceIndices;
        public Dictionary<string, float> wished_measures;

        public void ReplaceFaceVectors()
        {
            for (int i = 0; i < FaceIndices.Count; i++)
            {
                int ind = Convert.ToInt32(FaceIndices[i]);
                this.m_engine.final_form[ind] = FaceVectors[i];
            }

        }
        //public void ChangeSizes()
        //{


        //    headIndices = new List<uint>();
        //    HeadVectors = new List<Vector3>();
        //    string[] arrLines = System.IO.File.ReadAllLines(this.PathCharacters + "\\FaceMorphs.txt");
        //    foreach (string mName in arrLines)
        //    {
        //        foreach (Morph m in this.morph_data)
        //        {

        //            if (m.morph_name == mName)
        //            {
        //                foreach (uint i in m.morph_modified_verts)
        //                {
        //                    if (!headIndices.Contains(i))
        //                    {
        //                        headIndices.Add(i);
        //                        int ind = Convert.ToInt32(i);
        //                        HeadVectors.Add(this.m_engine.final_form[ind]);
        //                        this.m_engine.final_form[ind] = Vector3.Zero;
        //                    }
        //                    //int ind = Convert.ToInt32(i);
        //                    //this.m_engine.final_form[ind] = Vector3.Zero;
        //                }
        //                break;
        //            }
        //        }

        //    }

        //    System.Diagnostics.Debug.WriteLine("Number of vectors changed: " + headIndices.Count.ToString());

        //}
        public void CutFace()
        {

            
            FaceIndices = new List<uint>();
            FaceVectors = new List<Vector3>();
            string[] arrLines = System.IO.File.ReadAllLines(this.PathCharacters + "\\FaceMorphs.txt");
            foreach (string mName in arrLines)
            {
                foreach (Morph m in this.morph_data)
                {

                    if (m.morph_name == mName)
                    {
                        foreach(uint i in m.morph_modified_verts)
                        {
                            if (!FaceIndices.Contains(i))
                            {
                                FaceIndices.Add(i);
                                int ind = Convert.ToInt32(i);
                                FaceVectors.Add(this.m_engine.final_form[ind]);
                                this.Vectors[ind] = Vector3.Zero;
                                this.m_engine.final_form[ind] = Vector3.Zero;
                            }
                            //int ind = Convert.ToInt32(i);
                            //this.m_engine.final_form[ind] = Vector3.Zero;
                        }
                        break;
                    }
                }

            }
          
            System.Diagnostics.Debug.WriteLine("Number of vectors changed: " + FaceIndices.Count.ToString());

        }
        public void updateWishList(string myMeasureName, float newValue)
        {
            //1. extract check old value
            float zHeadOld = this.m_engine.measures[myMeasureName];
            foreach (string measure_name in this.body_height_Z_parts)
            {
                if (measure_name != myMeasureName)
                {
                    float f = this.m_engine.measures[measure_name];
                    System.Diagnostics.Debug.WriteLine(measure_name + f.ToString());
                   
                }

            }


            //1. add new measure
            if (!wished_measures.ContainsKey(myMeasureName))
                wished_measures.Add(myMeasureName, newValue);
            else
                wished_measures[myMeasureName] = newValue;
         
       
            //2. calculate the new body_height_Z
            //4. check the result - sum up all:
            float fHeight = 0f;
            foreach (string measure_name in this.body_height_Z_parts)
            {
                if (wished_measures.ContainsKey(measure_name))
                    fHeight += wished_measures[measure_name];
                else
                    fHeight += this.m_engine.measures[measure_name];
            }
            System.Diagnostics.Debug.WriteLine("New height: " + fHeight.ToString());

            
            //3. set the new height (?? useful)
            wished_measures["body_height_Z"] = fHeight;
            System.Diagnostics.Debug.WriteLine("New total height: " + fHeight.ToString());

        }
        public void updateWishList_Bodyheight(float total_height_Z)
        {
            float conversion_factor = 100f;

            //1. sum up all z parts that contribute to height (without "body_height_Z")
            float f = 0f;
            foreach (string measure_name in this.body_height_Z_parts)
            {
                if (measure_name != "body_height_Z")
                {
                    if (wished_measures.ContainsKey(measure_name))
                    {
                        wished_measures[measure_name] = this.m_engine.measures[measure_name] / conversion_factor;
                        f += wished_measures[measure_name];
                    }
                    else
                    {
                        wished_measures.Add(measure_name, this.m_engine.measures[measure_name] / conversion_factor);
                        f += wished_measures[measure_name];
                    }
                }

            }
            f = total_height_Z / f;
            //2. multiply all of them so that they sum up to the new total_height
            foreach (string measure_name in this.body_height_Z_parts)
            {
                if (measure_name != "body_height_Z")
                {
                    wished_measures[measure_name] *= f;
                }

            }

            //3. set the new body_height_Z
            wished_measures["body_height_Z"] = total_height_Z;

            //4. check the result - sum up all:
            f = 0f;
            foreach (string measure_name in this.body_height_Z_parts)
            {
                f += wished_measures[measure_name];
            }
            System.Diagnostics.Debug.WriteLine("New height: " + f.ToString());

        }
  
        public float GetHeight()
        {
            float f = 0f;
            foreach (string measure_name in this.body_height_Z_parts)
            {
                f += this.m_engine.measures[measure_name];

            }
            System.Diagnostics.Debug.WriteLine("New height: " + f.ToString() + " : " + this.m_engine.measures["body_height_Z"].ToString());
            return f;
        }
        public static void RearrangePointCloud(PointCloud pc)
        {
            
            //invert y and z vectors 
            for (int i = 0; i < pc.Vectors.Length; i++)
            {
                float fz = pc.Vectors[i].Z;
                pc.Vectors[i].Z = pc.Vectors[i].Y;
                pc.Vectors[i].Y = fz;
            }
            pc.RotateDegrees(0, 180, 0);
           // pc.RotateDegrees(0, 180, 0);

            
        }
      
        public static void RearrangeBack(PointCloud pc)
        {

            pc.RotateDegrees(0, 180, 0);
            //invert y and z vectors 
            for (int i = 0; i < pc.Vectors.Length; i++)
            {
                float fz = pc.Vectors[i].Z;
                pc.Vectors[i].Z = pc.Vectors[i].Y;
                pc.Vectors[i].Y = fz;
            }

            //pc.RotateDegrees(0, 0, 180);
        }
        public PointCloud ToPointCloud()
        {
            PointCloud pc = PointCloud.FromListVector3(this.Vectors);
            RearrangePointCloud(pc);


            pc.Name = "humanoid";
            pc.CalculateBoundingBox();
            //pc.Vectors = Vectors;
            return pc;

        }
       

        public void LoadMeasures()
        {
            string newMeasures = PathCharacters + "\\NewMeasures.json";//= os.path.join(data_path,"shared_measures",self.shared_measures_filename)
            char_data = JsonConvert.DeserializeObject<Dictionary<string, float>>(File.ReadAllText(newMeasures));



        }
        private void printCategories()
        {
            System.Diagnostics.Debug.WriteLine("---------------- Categories");
            foreach (HumanCategory c in categories)
            {
                System.Diagnostics.Debug.WriteLine(c.name);
                foreach (HumanModifier hm in c.modifiers)
                {
                    System.Diagnostics.Debug.WriteLine(" " + hm.name);
                }

            }
        }

        private void printJoints()
        {

            foreach (KeyValuePair<string, List<uint>> j in joints)
            {
                System.Diagnostics.Debug.WriteLine(j.Key.ToString());

            }
        }

        private void printMorphs(List<Morph> list)
        {
            int numberOfVectors = 0;
            int numberOfModifiedVectors = 0;


            foreach (Morph m in list)
            {
                numberOfVectors += m.morph_data.Count;
                numberOfModifiedVectors += m.morph_modified_verts.Count;
                System.Diagnostics.Debug.WriteLine(m.morph_name + " ; " + m.morph_modified_verts.Count.ToString());

            }
            System.Diagnostics.Debug.WriteLine("-----------------------------Number of Vectors: " + this.Vectors.Count.ToString());
            System.Diagnostics.Debug.WriteLine("Number of morph vectors: " + numberOfVectors.ToString() + " : " + numberOfModifiedVectors.ToString());

        }
        public void ToJson(string fileName)
        {
            JsonUtils.Serialize(this.Vectors, fileName);
        }
    }


}
