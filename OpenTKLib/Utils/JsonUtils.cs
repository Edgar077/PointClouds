using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Newtonsoft.Json;
using System.IO;

namespace OpenTKExtension
{
    public class JsonUtils
    {
        public static void Serialize(List<Vector3> listVectors, string fileName)
        {

            List<float[]> listFloats = new List<float[]>();


            for (int i = 0; i < listVectors.Count; i++)
            {
                float[] fA = new float[] { listVectors[i].X, listVectors[i].Y, listVectors[i].Z };
                listFloats.Add(fA);

            }

            File.WriteAllText(fileName, JsonConvert.SerializeObject(listFloats));

        }
        public static void Serialize(Vector3[] arrVectors, string fileName)
        {

            List<float[]> listFloats = new List<float[]>();


            for (int i = 0; i < arrVectors.Length; i++)
            {
                float[] fA = new float[] { arrVectors[i].X, arrVectors[i].Y, arrVectors[i].Z };
                listFloats.Add(fA);

            }

            File.WriteAllText(fileName, JsonConvert.SerializeObject(listFloats));

        }
        public static List<Vector3> DeserializeVectors(string fileName)
        {
            List<float[]> listFloats = JsonConvert.DeserializeObject<List<float[]>>(File.ReadAllText(fileName));


            List<Vector3> listV= new List<Vector3>();
            for (int i = 0; i < listFloats.Count; i++)
            {
                Vector3 v = new Vector3(listFloats[i][0], listFloats[i][1], listFloats[i][2]);
                listV.Add(v);
                
            }
            return listV;

        }
    }
  
}
