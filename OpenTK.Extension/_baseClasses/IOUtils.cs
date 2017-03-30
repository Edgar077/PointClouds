// Pogramming by
//     Edgar Maass: (email: maass@logisel.de)
//               
//
//Software used: 
//    OpenGL : http://www.opengl.org
//    OpenTK : http://www.opentk.com
//
// DISCLAIMER: Users rely upon this software at their own risk, and assume the responsibility for the results. Should this software or program prove defective, 
// users assume the cost of all losses, including, but not limited to, any necessary servicing, repair or correction. In no event shall the developers or any person 
// be liable for any loss, expense or damage, of any type or nature arising out of the use of, or inability to use this software or program, including, but not
// limited to, claims, suits or causes of action involving alleged infringement of copyrights, patents, trademarks, trade secrets, or unfair competition. 
//


using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Reflection;
using OpenTK;
using System.Windows.Forms;

namespace OpenTKExtension
{
    public class IOUtils
    {
        public static PointCloud ReadXYZFile_ToVertices(string fileNameLong, bool rotatePoints)
        {
            List<Vector3> listVector3 = ReadXYZFile(fileNameLong, rotatePoints);
            PointCloud listVertices = PointCloud.FromListVector3(listVector3);

            return listVertices;

        }
        public static List<Vector3> ReadXYZFile(string fileNameLong, bool rotatePoints)
        {

            string[] lines;
            try
            {
                lines = System.IO.File.ReadAllLines(fileNameLong);
            }
            catch(Exception err)
            {
                MessageBox.Show("ReadXYZFile: Read error - e.g. cannot be found:  " + fileNameLong + ": " + err.Message);
                return null;
            }
            return ConvertLinesToVector3(lines, rotatePoints);
        }
        private static List<Vector3> ConvertLinesToVector3(string[] lines, bool rotatePoints)
        {
            List<Vector3> listOfVectors = new List<Vector3>();

            int nCount = lines.GetLength(0);
            for (int i = 0; i < nCount; i++)
            {
                string[] arrStr1 = lines[i].Split(new Char[] { ' ' });
                try
                {

                    if (arrStr1.GetLength(0) > 2)
                        listOfVectors.Add(new Vector3(Convert.ToSingle(arrStr1[0], GlobalVariables.CurrentCulture), Convert.ToSingle(arrStr1[1], GlobalVariables.CurrentCulture), Convert.ToSingle(arrStr1[2], GlobalVariables.CurrentCulture)));

                }
                catch(Exception err)
                {
                    MessageBox.Show("Error parsing file at line: " + i.ToString() + " : " + err.Message);
                }

            }

            //if (rotatePoints)
            //{
            //    listOfVectors = RotatePointCloud(listOfVectors);
            //}


            return listOfVectors;
        }

        /// <summary>
        /// Reads only position and color information (No normals, texture, triangles etc. etc)
        /// </summary>
        /// <param name="fileOBJ"></param>
        /// <param name="myNewModel"></param>
        public static PointCloud ReadObjFile_ToPointCloud(string fileOBJ)
        {
            PointCloud myPCL = new PointCloud();
            string line = string.Empty;
            uint indexInModel = 0;
            try
            {

                using (StreamReader streamReader = new StreamReader(fileOBJ))
                {
                    //Part p = new Part();
                    Vertex vertex = new Vertex();
                    //myNewModel.Part = new List<Part>();
                    while (!streamReader.EndOfStream)
                    {
                        line = streamReader.ReadLine().Trim();
                        while (line.EndsWith("\\"))
                            line = line.Substring(0, line.Length - 1) + streamReader.ReadLine().Trim();
                        string str1 = GlobalVariables.TreatLanguageSpecifics(line);
                        string[] strArrayRead = str1.Split();
                        if (strArrayRead.Length >= 0)
                        {
                            switch (strArrayRead[0].ToLower())
                            {
                                //case "mtllib":
                                //    if (strArrayRead.Length < 2)
                                //    {
                                //        System.Windows.Forms.MessageBox.Show("Error reading obj file in line : " + line);
                                //    }

                                //    myNewModel.GetTexture(strArrayRead[1], fileOBJ);
                                //    break;
                                case "v"://Vertex
                                    vertex = HelperReadVertex(strArrayRead);
                                    vertex.Index = indexInModel;
                                    indexInModel++;
                                    myPCL.Add(vertex);
                                    break;
                           
                            }
                        }
                    }
            
                }
            }
            catch (Exception err)
            {
                System.Windows.Forms.MessageBox.Show("Error reading obj file - Vertices: " + line + " ; " + err.Message);
            }
            return myPCL;

        }
        public static Vertex HelperReadVertex(string[] strArrayRead)
        {

            Vertex vertex = new Vertex();
            vertex.Vector = new Vector3(0, 0, 0);
           
            if (strArrayRead.Length > 3)
            {
                //double dx, dy, dz;
                float f;
                float.TryParse(strArrayRead[1], NumberStyles.Float | NumberStyles.AllowThousands, GlobalVariables.CurrentCulture, out f);
                vertex.Vector.X = f;
                float.TryParse(strArrayRead[2], NumberStyles.Float | NumberStyles.AllowThousands, GlobalVariables.CurrentCulture, out f);
                vertex.Vector.Y = f;
                float.TryParse(strArrayRead[3], NumberStyles.Float | NumberStyles.AllowThousands, GlobalVariables.CurrentCulture, out f);
                vertex.Vector.Z = f;
            
            }

           
            vertex.Color = new Vector3(Convert.ToSingle(System.Drawing.Color.White.R/255f), Convert.ToSingle(System.Drawing.Color.White.G/255f), Convert.ToSingle(System.Drawing.Color.White.B/255f));
            float fOutValue = 0f;
            float r,g,b, a;
            a = 255;
            if (strArrayRead.Length > 7)
            {
                float.TryParse(strArrayRead[7], NumberStyles.Float | NumberStyles.AllowThousands, GlobalVariables.CurrentCulture, out fOutValue);
                a = Convert.ToByte(fOutValue * 255);
                
            }

            if (strArrayRead.Length > 6)
            {
                //we have vertex AND color infos
                //double colorR = 
                
                float.TryParse(strArrayRead[4], NumberStyles.Float | NumberStyles.AllowThousands, GlobalVariables.CurrentCulture, out r);
                
                float.TryParse(strArrayRead[5], NumberStyles.Float | NumberStyles.AllowThousands, GlobalVariables.CurrentCulture, out g);
                
                float.TryParse(strArrayRead[6], NumberStyles.Float | NumberStyles.AllowThousands, GlobalVariables.CurrentCulture, out b);
                
                
                vertex.Color = new Vector3(r,g,b);
            
            }



            return vertex;
        }
        public static void HelperReadVector3AndColor(string[] strArrayRead, out Vector3 vector, out Vector3 color)
        {

            vector = new Vector3();
            color = new Vector3(1f, 1f, 1f);
           
            if (strArrayRead.Length > 3)
            {
                //double dx, dy, dz;
                float f;
                float.TryParse(strArrayRead[1], NumberStyles.Float | NumberStyles.AllowThousands, GlobalVariables.CurrentCulture, out f);
                vector.X = f;
                float.TryParse(strArrayRead[2], NumberStyles.Float | NumberStyles.AllowThousands, GlobalVariables.CurrentCulture, out f);
                vector.Y = f;
                 float.TryParse(strArrayRead[3], NumberStyles.Float | NumberStyles.AllowThousands, GlobalVariables.CurrentCulture, out f);
                 vector.Z = f;
            }

            
            double fOutValue = 0f;
            float r, g, b;
            
            if (strArrayRead.Length > 7)
            {
                double.TryParse(strArrayRead[7], NumberStyles.Float | NumberStyles.AllowThousands, GlobalVariables.CurrentCulture, out fOutValue);
                //a = Convert.ToByte(fOutValue * 255);

            }

            if (strArrayRead.Length > 6)
            {
                //we have vertex AND color infos
                //double colorR = 
                float.TryParse(strArrayRead[4], NumberStyles.Float | NumberStyles.AllowThousands, GlobalVariables.CurrentCulture, out r);
                float.TryParse(strArrayRead[5], NumberStyles.Float | NumberStyles.AllowThousands, GlobalVariables.CurrentCulture, out g);
                float.TryParse(strArrayRead[6], NumberStyles.Float | NumberStyles.AllowThousands, GlobalVariables.CurrentCulture, out b);
                color = new Vector3(r, g, b);
                
            }


        }
        public static Texture Read_MTLFile(string mtlFileName, string path)
        {
           // Bitmap textureBitmap = null;
            Texture texture = null;
            try
            {
                mtlFileName = mtlFileName.Replace('/', '\\');
                string pathIMG = IOUtils.ExtractDirectory(mtlFileName);
                
                
                using (StreamReader streamReader = new StreamReader(path + mtlFileName))
                {
                    while (!streamReader.EndOfStream)
                    {
                        string line = streamReader.ReadLine().Trim();
                        //while (line.EndsWith("\\"))
                        //    line = line.Substring(0, line.Length - 1) + streamReader.ReadLine().Trim();

                        string[] strArray2 = GlobalVariables.TreatLanguageSpecifics(line).Split();
                        if (strArray2[0].ToLower() == "map_kd")
                        {
                            //textureBitmap = new System.Drawing.Bitmap(path + strArray2[1]);
                            texture = new Texture(path + pathIMG + strArray2[1]);
                        }
                    }
                    streamReader.Close();
                }
            }
            catch (Exception err)
            {
                System.Windows.Forms.MessageBox.Show("Error reading par of obj file (MTL file does not exist): " + path + "\\" + mtlFileName);
                System.Diagnostics.Debug.WriteLine("Err :  " + err.Message);
            }
            return texture;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strArrayRead"></param>
        /// <param name="myNewModel"></param>
        /// <returns></returns>
        public static void ReadFaceLine(string[] strArrayRead, List<uint> indices, List<uint> normalIndices, List<Vector2> textures)
        {
            
            try
            {
                

                foreach (string strElement in strArrayRead)
                {
                    if (strElement.ToLower() != "f")
                    {
                        try
                        {
                            
                            string[] strSubArr = strElement.Split('/');
                            int result;
                            int.TryParse(strSubArr[0], out result);
                            //programs like Meshlab expect indices starting from one - OpenGL from 0
                            result -= 1;
                            indices.Add(Convert.ToUInt32(result));

                          
                            if (strSubArr.Length > 2)
                            {
                                int.TryParse(strSubArr[strSubArr.Length - 1], out result);
                                //programs like Meshlab expect indices starting from one - OpenGL from 0
                                result -= 1;
                                normalIndices.Add(Convert.ToUInt32(result));

                                if (strSubArr[strSubArr.Length - 2] != "")
                                {
                                    int.TryParse(strSubArr[strSubArr.Length - 2], out result);
                                    //programs like Meshlab expect indices starting from one - OpenGL from 0
                                    result -= 1;
                                   // textures.Add(Convert.ToUInt32(result ));
                                   
                                }
                            }
                        }
                        catch (Exception err1)
                        {
                            System.Windows.Forms.MessageBox.Show("Error reading obj file (triangles)  in line: " + strElement + " : " + err1.Message);
                            
                        }
                    }
                }

             
            }
            catch (Exception err)
            {
                System.Windows.Forms.MessageBox.Show("Error reading obj file (triangles)  " + err.Message);
               
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strArrayRead"></param>
        /// <param name="myNewModel"></param>
        /// <returns></returns>
        //public static Triangle helper_ReadTriangle(string[] strArrayRead, Model myNewModel)
        //{
        //    try
        //    {
        //        Triangle a = new Triangle();

        //        foreach (string str2 in strArrayRead)
        //        {
        //            if (str2.ToLower() != "f")
        //            {
        //                try
        //                {
        //                    // the index vertex starts with 1 !!
        //                    string[] strArray2 = str2.Split('/');
        //                    int result;
        //                    int.TryParse(strArray2[0], out result);
        //                    a.IndVertices.Add(result - 1);

        //                    int indVertex = a.IndVertices.Count - 1;
        //                    int indPart = myNewModel.Parts.Count - 1;
        //                    if (indPart < 0)
        //                        indPart = 0;
        //                    myNewModel.PointCloud[a.IndVertices[indVertex]].IndexTriangles.Add(indVertex);
        //                    myNewModel.PointCloud[a.IndVertices[indVertex]].IndexParts.Add(indPart);
        //                    if (strArray2.Length > 2)
        //                    {
        //                        int.TryParse(strArray2[strArray2.Length - 1], out result);
        //                        int index2 = result - 1;
        //                        if (!double.IsNaN(myNewModel.Normals[index2].X))
        //                        {
        //                            a.IndNormals.Add(index2);
        //                            int num2 = result - 1;
        //                            myNewModel.PointCloud[a.IndVertices[indVertex]].IndexNormals.Add(num2);
        //                        }
        //                        if (strArray2[strArray2.Length - 2] != "")
        //                        {
        //                            int.TryParse(strArray2[strArray2.Length - 2], out result);
        //                            int num2 = result - 1;
        //                            a.IndTextures.Add(num2);
        //                        }
        //                    }
        //                }
        //                catch (Exception err1)
        //                {
        //                    System.Windows.Forms.MessageBox.Show("Error reading obj file (triangles)  " + err1.Message);
        //                    return new Triangle();
        //                }
        //            }
        //        }

        //        return a;
        //    }
        //    catch (Exception err)
        //    {
        //        System.Windows.Forms.MessageBox.Show("Error reading obj file (triangles)  " + err.Message);
        //        return new Triangle();
        //    }

        //}
        public static void ExtractDirectoryAndNameFromFileName(string fileNameIn, out string fileNameShort, out string dirName)
        {
            string[] arrSplit = fileNameIn.Split(new Char[] { '\\' }, 100);
            fileNameShort = arrSplit[arrSplit.GetLength(0) - 1];


            System.Text.StringBuilder str = new System.Text.StringBuilder();
            for (int i = 0; i < arrSplit.GetLength(0) - 1; i++)
            {

                str.Append(arrSplit[i] + @"\");
              
            }

            dirName = str.ToString();



        }
        public static string ExtractDirectory(string fileNameIn)
        {
            string[] arrSplit = fileNameIn.Split(new Char[] { '\\' }, 100);
           

            System.Text.StringBuilder str = new System.Text.StringBuilder();
            for (int i = 0; i < arrSplit.GetLength(0) - 1; i++)
            {

                str.Append(arrSplit[i] + @"\");

            }

            return str.ToString();



        }
        public static string ExtractDirectoryLast(string dirPath)
        {
            string[] arrSplit = dirPath.Split(new Char[] { '\\' }, 100);
            string ret = arrSplit[arrSplit.Length - 1];

            return ret;


        }

        public static string ExtractExtension(string fileNameIn)
        {
            string[] arrSplit = fileNameIn.Split(new Char[] { '.' }, 100);
            if (arrSplit.Length == 1)
                return null;

            string ext = arrSplit[arrSplit.GetLength(0) - 1];

            return ext;
            
        }
        public static string ExtractFileNameWithoutExtension(string fileNameIn)
        {
            string[] arrSplit = fileNameIn.Split(new Char[] { '.' }, 100);
            string ext = arrSplit[0];
                  
            return ext;

        }
        public static string ExtractFileNameShort(string fileNameIn)
        {
            string[] arrSplit = fileNameIn.Split(new Char[] { '\\' }, 100);
            string fileNameShort = arrSplit[arrSplit.GetLength(0) - 1];

            return fileNameShort;
            
        }
        public static string ExtractFileNameShortWithoutExtension(string fileNameIn)
        {


            string fileNameShort = ExtractFileNameShort(fileNameIn);
            fileNameShort = ExtractFileNameWithoutExtension(fileNameShort);
            return fileNameShort;

        }
        public static List<triangleInd> getTriangleIndicesFromString(string stringInput)
        {
            List<triangleInd> listInd = new List<triangleInd>();
            string[] arrInd = stringInput.Split(new Char[] { ' ' });
            for(int i = 0; i < arrInd.Length; i+=3)
            {
                if( (i + 3) < arrInd.Length)
                {
                    triangleInd t = new triangleInd(Convert.ToUInt32(arrInd[i]), Convert.ToUInt32(arrInd[i + 1]), Convert.ToUInt32(arrInd[i + 2]));
                    listInd.Add(t);
                }

                


            }

            return listInd;

        }
        public static string  reduceTriangles(List<triangleInd> listInd, float factor, out int count)
        {
            int takeEvery = Convert.ToInt32(1f / factor);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            count = 0;
            for (int i = 0; i < listInd.Count; i += takeEvery )
            {
                count++;
                triangleInd t = listInd[i];
                sb.Append(" " + t.ToString());


            }
          
            return sb.ToString();


        }
        public static string ReduceTriangles(string stringInput, float factor, out int count)
        {
            List<triangleInd> listInd = getTriangleIndicesFromString(stringInput);
            string strReturn = reduceTriangles(listInd, factor, out count);

            
            
            return strReturn;


        }
    }
    public struct triangleInd
    {
        public uint A;
        public uint B;
        public uint C;
        public triangleInd(uint a, uint b, uint c)
        {
            A = a;
            B = b;
            C = c;

        }
        public override string ToString()
        {
            return A.ToString() + " " + B.ToString() + " " + C.ToString();
            
        }
    }
   
}
