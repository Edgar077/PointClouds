using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;


namespace grendgine_collada
{
	
	[System.SerializableAttribute()]

	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
	[System.Xml.Serialization.XmlRootAttribute(ElementName="COLLADA", Namespace="http://www.collada.org/2005/11/COLLADASchema", IsNullable=false)]
	public partial class Grendgine_Collada
	{
		
		[XmlAttribute("version")]
		public string Collada_Version;
			
			
		[XmlElement(ElementName = "asset")]
		public Grendgine_Collada_Asset Asset;
		
		
		
		//Core Elements
		[XmlElement(ElementName = "library_animation_clips")]
		public Grendgine_Collada_Library_Animation_Clips Library_Animation_Clips;

		[XmlElement(ElementName = "library_animations")]
		public Grendgine_Collada_Library_Animations Library_Animations;

		[XmlElement(ElementName = "library_cameras")]
		public Grendgine_Collada_Library_Cameras Library_Cameras;
		

		[XmlElement(ElementName = "library_controllers")]
		public Grendgine_Collada_Library_Controllers Library_Controllers;

		[XmlElement(ElementName = "library_formulas")]
		public Grendgine_Collada_Library_Formulas Library_Formulas;

		[XmlElement(ElementName = "library_geometries")]
		public Grendgine_Collada_Library_Geometries Library_Geometries;

		[XmlElement(ElementName = "library_lights")]
		public Grendgine_Collada_Library_Lights Library_Lights;

		[XmlElement(ElementName = "library_nodes")]
		public Grendgine_Collada_Library_Nodes Library_Nodes;

		[XmlElement(ElementName = "library_visual_scenes")]
		public Grendgine_Collada_Library_Visual_Scenes Library_Visual_Scene;
				
		//Physics Elements

		[XmlElement(ElementName = "library_force_fields")]
		public Grendgine_Collada_Library_Force_Fields Library_Force_Fields;
		
		[XmlElement(ElementName = "library_physics_materials")]
		public Grendgine_Collada_Library_Physics_Materials Library_Physics_Materials;
		
		[XmlElement(ElementName = "library_physics_models")]
		public Grendgine_Collada_Library_Physics_Models Library_Physics_Models;
		
		[XmlElement(ElementName = "library_physics_scenes")]
		public Grendgine_Collada_Library_Physics_Scenes Library_Physics_Scenes;
		
		
		//FX Elements
		[XmlElement(ElementName = "library_effects")]
		public Grendgine_Collada_Library_Effects Library_Effects;
		
		[XmlElement(ElementName = "library_materials")]
		public Grendgine_Collada_Library_Materials Library_Materials;
		
		[XmlElement(ElementName = "library_images")]
		public Grendgine_Collada_Library_Images Library_Images;
		
		//Kinematics
		[XmlElement(ElementName = "library_articulated_systems")]
		public Grendgine_Collada_Library_Articulated_Systems Library_Articulated_Systems;
		
		[XmlElement(ElementName = "library_joints")]
		public Grendgine_Collada_Library_Joints Library_Joints;
		
		[XmlElement(ElementName = "library_kinematics_models")]
		public Grendgine_Collada_Library_Kinematics_Models Library_Kinematics_Models;
		
		[XmlElement(ElementName = "library_kinematics_scenes")]
		public Grendgine_Collada_Library_Kinematics_Scene Library_Kinematics_Scene;
		
		
		
		[XmlElement(ElementName = "scene")]
		public Grendgine_Collada_Scene Scene;

		[XmlElement(ElementName = "extra")]
		public Grendgine_Collada_Extra[] Extra;
		
		public static Grendgine_Collada Load_File(string file_name)
        {
            try
            {
				Grendgine_Collada col_scenes = null;
                
				XmlSerializer sr = new XmlSerializer(typeof(Grendgine_Collada));
                TextReader tr = new StreamReader(file_name);
                col_scenes = (Grendgine_Collada)(sr.Deserialize(tr));
				
                tr.Close();
                
				return col_scenes;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error in Load_File: " + ex.Message);
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
				return null;
            }			
		}
        public static Grendgine_Collada Save(string file_name, Grendgine_Collada col_scenes)
        {
            try
            {

               
                string path = OpenTKExtension.IOUtils.ExtractDirectory(file_name);

                TextWriter tw = new StreamWriter(path + "new.dae");
                XmlSerializer sr = new XmlSerializer(typeof(Grendgine_Collada));
                sr.Serialize(tw, col_scenes);
                tw.Close();
                return col_scenes;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error in Save: " + ex.Message);
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
                return null;
            }
        }
        
        public static bool ReduceMesh(string file_name, float factor)
        {
            bool b = true;
            if (b)
            {
                System.Diagnostics.Debug.WriteLine("sds");

            }

            //removeEveryNth hast to be < 1 
            if (factor >= 1)
            {
                System.Windows.Forms.MessageBox.Show("SW Error reducing mesh - reduce factor has to be less than 1");
            }
            try
            {
                Grendgine_Collada col = Grendgine_Collada.Load_File(file_name);
                

                if(col.Library_Geometries != null)
                {
                    if(col.Library_Geometries.Geometry != null)
                    {

                        for (int i = 0; i < col.Library_Geometries.Geometry.Length; i++)
                        {
                            Grendgine_Collada_Geometry geo = col.Library_Geometries.Geometry[i];
                            if (geo.Mesh != null)
                                if (geo.Mesh.Triangles != null)
                                {
                                    if (geo.Mesh.Triangles != null)
                                    {
                                        for (int j = 0; j < geo.Mesh.Triangles.Length; j++)
                                        {
                                            Grendgine_Collada_Triangles triangles = geo.Mesh.Triangles[j];
                                            if (triangles.P != null && triangles.P.Value_As_String != null)
                                            {
                                                string triangleString = triangles.P.Value_As_String;
                                                int newCount;
                                                string newTriangles = OpenTKExtension.IOUtils.ReduceTriangles(triangleString, factor, out newCount);
                                                triangles.P.Value_As_String = newTriangles;
                                                triangles.Count = newCount;
                                                col.Library_Geometries.Geometry[i].Mesh.Triangles[j].P.Value_As_String = newTriangles;

                                                System.Diagnostics.Debug.Write(triangleString);

                                                System.Diagnostics.Debug.Write(newTriangles);

                                            }
                                            else
                                            {
                                                System.Diagnostics.Debug.Write("CHECK");
                                            }

                                        }
                                    }
                         

                                }

                        }
                    }
                }

                string path = OpenTKExtension.IOUtils.ExtractDirectory(file_name);
                Grendgine_Collada.Save(path + "new.dae", col);


                return true;

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error in Save: " + ex.Message);
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
                return false;
            }
        }
    }
}

