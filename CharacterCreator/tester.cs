using System;
using System.Collections.Generic;

using System.Runtime.Serialization.Json;
using System.IO;
using System.Runtime.Serialization;
using OpenTK;


namespace CharacterCreator
{
    public class tester
    {
        public tester()
        {
           

        }
        public void CreateVector()
        {
            Vector3 v = Vector3.UnitX;

            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(Vector3));
            MemoryStream ms = new MemoryStream();
            js.WriteObject(ms, v);

            Console.WriteLine("\r\nUdemy.com - Serializing and Deserializing JSON in C#\r\n");
            ms.Position = 0;
            StreamReader sr = new StreamReader(ms);
            System.Diagnostics.Debug.Write(sr.ReadToEnd());
            Console.WriteLine(sr.ReadToEnd());
            sr.Close();
            ms.Close();


        }
        public void ListVectors()
        {
            Vector3 v = Vector3.UnitX;

            List<Vector3> list = new List<Vector3>();
            for(int i = 0; i < 10; i++)
            {
                list.Add(new Vector3(i, 0, i));
            }
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(List<Vector3>));
            MemoryStream ms = new MemoryStream();
            js.WriteObject(ms, list);

            //Console.WriteLine("\r\nUdemy.com - Serializing and Deserializing JSON in C#\r\n");
            ms.Position = 0;
            StreamReader sr = new StreamReader(ms);
            System.Diagnostics.Debug.Write(sr.ReadToEnd());
            //Console.WriteLine(sr.ReadToEnd());
            sr.Close();
            ms.Close();


        }
        public void SerializeListFloats()
        {
           
            List<float[]> list = new List<float[]>();
            for (int i = 0; i < 10; i++)
            {
                float[] arr = new float[3];
                arr[0] = 1;
                arr[1] = 1;
                arr[2] = 1;

            
                list.Add(arr);
                
            }
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(List<float[]>));
            MemoryStream ms = new MemoryStream();
            js.WriteObject(ms, list);

            //Console.WriteLine("\r\nUdemy.com - Serializing and Deserializing JSON in C#\r\n");
            ms.Position = 0;
            StreamReader sr = new StreamReader(ms);
            System.Diagnostics.Debug.Write(sr.ReadToEnd());
            //Console.WriteLine(sr.ReadToEnd());
            sr.Close();
            ms.Close();


        }
    }
}
