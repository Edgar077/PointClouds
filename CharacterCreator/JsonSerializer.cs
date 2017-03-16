using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
/// <summary>
/// JSON Serialization and Deserialization Assistant Class
/// </summary>
public class JsonSerializer
{
    /// <summary>
    /// JSON Serialization
    /// </summary>
    public static string SerializeToString<T>(T t)
    {
        
        DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
        MemoryStream ms = new MemoryStream();
        ser.WriteObject(ms, t);
        string jsonString = Encoding.UTF8.GetString(ms.ToArray());
        ms.Close();
        return jsonString;
    }
    /// <summary>
    /// JSON Serialization
    /// </summary>
    public static void SerializeToFile<T>(T t, string fileName)
    {

        DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
        MemoryStream ms = new MemoryStream();
        ser.WriteObject(ms, t);
        string jsonString = Encoding.UTF8.GetString(ms.ToArray());
        ms.Close();
        StreamWriter sw = new StreamWriter(fileName);
        sw.Write(jsonString);
        sw.Close();

        
    }
    /// <summary>
    /// JSON Deserialization
    /// </summary>
    public static T DeserializeFromString<T>(string jsonString, string fileName)
    {

        DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
        MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
        T obj = (T)ser.ReadObject(ms);
        return obj;
    }
    /// <summary>
    /// JSON Deserialization
    /// </summary>
    public static T DeserializeFromFile<T>(string fileName)
    {
        using (StreamReader sr = new StreamReader(fileName))
        {
            string read = sr.ReadToEnd();

            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(read));
            T obj = (T)ser.ReadObject(ms);
            return obj;

        }


    }
   
    public static void SerializeListFloats(object o)
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
        //DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(List<float[]>));
        DataContractJsonSerializer js = new DataContractJsonSerializer(list.GetType());
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