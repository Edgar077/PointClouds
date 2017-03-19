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

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Reflection;
using OpenTK;
using System.Windows.Forms;

namespace OpenTK.Extension
{
    public class IOUtils
    {
     
        public static List<Vector3> ReadXYZFile(string fileNameLong, bool rotatePoints)
        {

            string[] lines;
            try
            {
                lines = System.IO.File.ReadAllLines(fileNameLong);
            }
            catch (Exception err)
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
                catch (Exception err)
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

   
        public static void HelperReadVector3AndColor(string[] strArrayRead, out Vector3 vector, out Vector3 color)
        {

            vector = new Vector3();
            color = new Vector3(1f, 1f, 1f);

            if (strArrayRead.Length > 3)
            {
                //float dx, dy, dz;
                float.TryParse(strArrayRead[1], NumberStyles.Float | NumberStyles.AllowThousands, GlobalVariables.CurrentCulture, out vector.X);
                float.TryParse(strArrayRead[2], NumberStyles.Float | NumberStyles.AllowThousands, GlobalVariables.CurrentCulture, out vector.Y);
                float.TryParse(strArrayRead[3], NumberStyles.Float | NumberStyles.AllowThousands, GlobalVariables.CurrentCulture, out vector.Z);

            }


            float fOutValue = 0f;
            float r, g, b;

            if (strArrayRead.Length > 7)
            {
                float.TryParse(strArrayRead[7], NumberStyles.Float | NumberStyles.AllowThousands, GlobalVariables.CurrentCulture, out fOutValue);
                //a = Convert.ToByte(fOutValue * 255);

            }

            if (strArrayRead.Length > 6)
            {
                //we have vertex AND color infos
                //float colorR = 
                float.TryParse(strArrayRead[4], NumberStyles.Float | NumberStyles.AllowThousands, GlobalVariables.CurrentCulture, out r);
                float.TryParse(strArrayRead[5], NumberStyles.Float | NumberStyles.AllowThousands, GlobalVariables.CurrentCulture, out g);
                float.TryParse(strArrayRead[6], NumberStyles.Float | NumberStyles.AllowThousands, GlobalVariables.CurrentCulture, out b);
                color = new Vector3(r, g, b);

            }


        }
        public static Bitmap ReadTexture(string TexFile, string OBJFile)
        {
            Bitmap textureBitmap = null;
            try
            {
                string[] strArray1 = OBJFile.Split('\\');
                string str = "";
                TexFile = TexFile.Replace(",", ".");
                for (int index = strArray1.Length - 2; index >= 0; --index)
                    str = strArray1[index] + "\\" + str;
                using (StreamReader streamReader = new StreamReader(str + TexFile))
                {
                    while (!streamReader.EndOfStream)
                    {
                        string line = streamReader.ReadLine().Trim();
                        while (line.EndsWith("\\"))
                            line = line.Substring(0, line.Length - 1) + streamReader.ReadLine().Trim();
                        string[] strArray2 = GlobalVariables.TreatLanguageSpecifics(line).Split();
                        if (strArray2[0].ToLower() == "map_kd")
                            textureBitmap = new System.Drawing.Bitmap(str + strArray2[1].Replace(",", "."));
                    }
                    streamReader.Close();
                }
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine("Err :  " + err.Message);
            }
            return textureBitmap;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strArrayRead"></param>
        /// <param name="myNewModel"></param>
        /// <returns></returns>
        public static void ReadIndicesLine(string[] strArrayRead, List<uint> triangles, List<uint> normalIndices, List<uint> textureIndices)
        {
            //indices = new List<uint>();
            //normalIndices = new List<uint>();
            //textureIndices = new List<uint>();

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
                            triangles.Add(Convert.ToUInt32(result - 1));
                            

                            if (strSubArr.Length > 2)
                            {
                                int.TryParse(strSubArr[strSubArr.Length - 1], out result);
                                normalIndices.Add(Convert.ToUInt32(result - 1));

                                if (strSubArr[strSubArr.Length - 2] != "")
                                {
                                    int.TryParse(strSubArr[strSubArr.Length - 2], out result);
                                    int num2 = result - 1;
                                    textureIndices.Add(Convert.ToUInt32(result - 1));

                                }
                            }
                        }
                        catch (Exception err1)
                        {
                            System.Windows.Forms.MessageBox.Show("Error reading obj file (triangles)  " + err1.Message);

                        }
                    }
                }


            }
            catch (Exception err)
            {
                System.Windows.Forms.MessageBox.Show("Error reading obj file (triangles)  " + err.Message);

            }

        }
     
    }
}
