using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows;
using OpenTK;
using System.Globalization;
using System.IO;


namespace OpenTKExtension
{
    public class UtilsPointCloudIO
    {
        public static int BYTES_PER_PIXEL = (PixelFormats.Bgr32.BitsPerPixel + 7) / 8;
        public static System.Globalization.CultureInfo CultureInfo = new System.Globalization.CultureInfo("en-US");

        /// <summary>
        /// reads the ply file ONLY with the special format used also in the write method
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileNameShort"></param>
        /// <param name="depthData"></param>
        /// <returns></returns>
        public static void FromPLYFile(string path, string fileNameShort, int width, int height, ref ushort[,] depth, ref byte[,] colorInfoR, ref byte[,] colorInfoG, ref byte[,] colorInfoB, ref byte[,] colorInfoA)
        {
            string fileName = path + "\\" + fileNameShort;
            if (!System.IO.File.Exists(fileName))
            {
                System.Diagnostics.Debug.WriteLine("File does not exist: ");
                return;

            }

            ushort[] lineData = new ushort[7];
            System.Collections.ArrayList lineList = new System.Collections.ArrayList();

            depth = new ushort[width, height];
            colorInfoR = new byte[width, height];
            colorInfoG = new byte[width, height];
            colorInfoB = new byte[width, height];
            colorInfoA = new byte[width, height];


            int numberOfDepthPointsNonZero = -1;

       

            try
            {
                string[] lines = System.IO.File.ReadAllLines(fileName);
                //the first 12 lines are meta data information, which I already know , see also comment on this method
                int nCount =  lines.GetLength(0);
                for (int i = 13; i < nCount; i++)
                {
                    string[] arrStr1 = lines[i].Split(new Char[] { ' ' });
                    if (arrStr1.Length < 6)
                    {
                        System.Windows.Forms.MessageBox.Show("Error reading file " + fileName + " in line i");
                    }
                    else
                    {

                        for (int j = 0; j < 6; j++)
                        {
                            lineData[j] = Convert.ToUInt16(arrStr1[j]);
                        }
                        depth[lineData[0], lineData[1]] = lineData[2];
                        colorInfoR[lineData[0], lineData[1]] = Convert.ToByte(lineData[3]);
                        colorInfoG[lineData[0], lineData[1]] = Convert.ToByte(lineData[4]);
                        colorInfoB[lineData[0], lineData[1]] = Convert.ToByte(lineData[5]);
                        colorInfoA[lineData[0], lineData[1]] = 255;

                        if (depth[lineData[0], lineData[1]] > 0)
                        {
                            numberOfDepthPointsNonZero++;

                        }

                    }
                }
            }
            catch(Exception err)
            {
                System.Windows.Forms.MessageBox.Show("Read_PLY: Read error - e.g. cannot be found:  " + fileName + ": " + err.Message);
            }

            
          
        }
        ///// <summary>
        ///// Save mesh in ASCII .PLY file with per-vertex color
        ///// </summary>
        ///// <param name="mesh">Calculated mesh object</param>
        ///// <param name="writer">The text writer</param>
        ///// <param name="flipAxes">Flag to determine whether the Y and Z values are flipped on save,
        ///// default should be true.</param>
        ///// <param name="outputColor">Set this true to write out the surface color to the file when it has been captured.</param>
        //public static void ToPlyNew(PointCloud pc, string path, string fileName)
        //{
         

        //    //var vertices = mesh.GetVertices();
        //    //var indices = mesh.GetTriangleIndexes();
        //    //var colors = mesh.GetColors();

        //    //// Check mesh arguments
        //    //if (0 == vertices.Count || 0 != vertices.Count % 3 || vertices.Count != indices.Count || (outputColor && vertices.Count != colors.Count))
        //    //{
        //    //    throw new ArgumentException(Properties.Resources.InvalidMeshArgument);
        //    //}

        //    //int faces = indices.Count / 3;

        //    // Write the PLY header lines
        //    StringBuilder sb = new StringBuilder();
        //    System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-US");
        //    List<string> lines = new List<string>();
        //    lines.Add("ply");
        //    lines.Add("format ascii 1.0");
        //    lines.Add("comment file created by OpenTKExtension");

        //    lines.Add("TO BE REPLACED ");
        //    lines.Add("property float x");
        //    lines.Add("property float y");
        //    lines.Add("property float z");
        //    lines.Add("property uchar red");
        //    lines.Add("property uchar green");
        //    lines.Add("property uchar blue");
           

        //    lines.Add("element face 0 ";
        //    lines.Add("property list uchar int vertex_index");
        //    lines.Add("end_header");

        //    // Sequentially write the 3 vertices of the triangle, for each triangle
        //    for (int i = 0; i < vertices.Count; i++)
        //    {
        //        var vertex = vertices[i];

        //        string vertexString = vertex.X.ToString(CultureInfo.InvariantCulture) + " ";

        //        if (flipAxes)
        //        {
        //            vertexString += (-vertex.Y).ToString(CultureInfo.InvariantCulture) + " " + (-vertex.Z).ToString(CultureInfo.InvariantCulture);
        //        }
        //        else
        //        {
        //            vertexString += vertex.Y.ToString(CultureInfo.InvariantCulture) + " " + vertex.Z.ToString(CultureInfo.InvariantCulture);
        //        }

        //        if (outputColor)
        //        {
        //            int red = (colors[i] >> 16) & 255;
        //            int green = (colors[i] >> 8) & 255;
        //            int blue = colors[i] & 255;

        //            vertexString += " " + red.ToString(CultureInfo.InvariantCulture) + " " + green.ToString(CultureInfo.InvariantCulture) + " "
        //                            + blue.ToString(CultureInfo.InvariantCulture);
        //        }

        //        lines.Add(vertexString);
        //    }

        //    // Sequentially write the 3 vertex indices of the triangle face, for each triangle, 0-referenced in PLY files
        //    for (int i = 0; i < faces; i++)
        //    {
        //        string baseIndex0 = (i * 3).ToString(CultureInfo.InvariantCulture);
        //        string baseIndex1 = ((i * 3) + 1).ToString(CultureInfo.InvariantCulture);
        //        string baseIndex2 = ((i * 3) + 2).ToString(CultureInfo.InvariantCulture);

        //        string faceString = "3 " + baseIndex0 + " " + baseIndex1 + " " + baseIndex2;
        //        lines.Add(faceString);
        //    }

        //}
        /// <summary>
        /// write ply file from depth data and colorInfoPixels (color info)
        /// </summary>
        /// <param name="colorInfoPixels"></param>
        /// <param name="depthData"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool ToPLYFile(PointCloud pc, string path, string fileName)
        {
            StringBuilder sb = new StringBuilder();
            //CultureInfo.InvariantCulture
            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-US");

            List<string> lines = new List<string>();
            lines.Add("ply");
            lines.Add("format ascii 1.0");
            lines.Add("comment OpenTKExtension generated");
            lines.Add("element vertex HAVE TO REPLACE THIS LATER!!");
            lines.Add("property float x");
            lines.Add("property float y");
            lines.Add("property float z");
            lines.Add("property uchar red");
            lines.Add("property uchar green");
            lines.Add("property uchar blue");
            //lines.Add("property float red");
            //lines.Add("property float green");
            //lines.Add("property float blue");
            //lines.Add("property float alpha");
            //lines.Add("element face 0");
            //lines.Add("property list uchar int vertex_indices");
            lines.Add("end_header");


         
            if (pc.Colors != null && pc.Colors.GetLength(0) == pc.Vectors.Length)
            {


                for (int i = 0; i < pc.Vectors.Length; ++i)
                {

                    Vector3 v = pc.Vectors[i];
                    Vector3 colorVal = pc.Colors[i];
                    string coordinate = v.X.ToString(CultureInfo) + " " + v.Y.ToString(CultureInfo) + " " + v.Z.ToString(CultureInfo);
                    string color = string.Empty;
                    if (colorVal != null)
                    {
                        // 255F
                        color = Convert.ToUInt32(colorVal[0]*255).ToString(CultureInfo) + " " + Convert.ToUInt32(colorVal[1]*255).ToString(CultureInfo) + " " + Convert.ToUInt32(colorVal[2]*255).ToString(CultureInfo) + " ";
                        //color = colorVal[0].ToString(CultureInfo) + " " + colorVal[1].ToString(CultureInfo) + " " + colorVal[2].ToString(CultureInfo) + " 1" ;
                   
                    }
                    //string color = pixels[displayIndex].ToString() + " " + pixels[displayIndex + 1].ToString() + " " + pixels[displayIndex + 2].ToString() + " " + pixels[displayIndex + 3].ToString();
                    
                   
                    //string color = pixels[displayIndex].ToString() + " " + pixels[displayIndex + 1].ToString() + " " + pixels[displayIndex + 2].ToString() + " " + pixels[displayIndex + 3].ToString();
                    lines.Add(coordinate + " " + color);


                }
            }
            else
                for (int i = 0; i < pc.Vectors.Length; ++i)
                {

                    Vector3 v = pc.Vectors[i];

                    string coordinate = v.X.ToString(CultureInfo) + " " + v.Y.ToString(CultureInfo) + " " + v.Z.ToString(CultureInfo);

                    //string color = pixels[displayIndex].ToString() + " " + pixels[displayIndex + 1].ToString() + " " + pixels[displayIndex + 2].ToString() + " " + pixels[displayIndex + 3].ToString();
                    lines.Add(coordinate);

                }


          

            lines[3] = "element vertex " + pc.Vectors.Length.ToString();
            System.IO.StreamWriter writer = new System.IO.StreamWriter(path + "\\" + fileName);
            for(int i = 0; i < lines.Count; i++)
            {
                writer.WriteLine(lines[i]);
            }

            writer.Close();
          //  System.IO.File.WriteAllLines(path + "\\" + fileName, lines);

            return true;

        }
        /// <summary>
        /// write ply file from depth data and colorInfoPixels (color info)
        /// </summary>
        /// <param name="colorInfoPixels"></param>
        /// <param name="depthData"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool ToPLYFile(byte[] colorInfoPixels, ushort[] depthData, int width, int height, string path, string fileName)
        {
            StringBuilder sb = new StringBuilder();

            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-US");

            List<string> lines = new List<string>();
            lines.Add("ply");
            lines.Add("format ascii 1.0");
            lines.Add("comment OpenTKExtension generated");
            lines.Add("element vertex HAVE TO REPLACE THIS LATER!!");

            lines.Add("property float  x");
            lines.Add("property float y");
            lines.Add("property float z");
        
            //lines.Add("property uchar red");
            //lines.Add("property uchar green");
            //lines.Add("property uchar blue");
            //lines.Add("property uchar alpha");
            lines.Add("element face 0");
            lines.Add("property list uchar int vertex_indices");
            lines.Add("end_header");


            int iIndex = 0;

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    int depthIndex = (y * width) + x;
                    int displayIndex = depthIndex * BYTES_PER_PIXEL;
                    if (colorInfoPixels[displayIndex + 3] != 0 && depthData[depthIndex] != 0)
                    {

                        iIndex++;
                        string coordinate = x.ToString() + " " + y.ToString(CultureInfo) + " " + depthData[depthIndex].ToString(CultureInfo);
                        string color = colorInfoPixels[displayIndex].ToString(CultureInfo) + " " + colorInfoPixels[displayIndex + 1].ToString(CultureInfo) + " " + colorInfoPixels[displayIndex + 2].ToString(CultureInfo) + " ";

                        //string color = pixels[displayIndex].ToString() + " " + pixels[displayIndex + 1].ToString() + " " + pixels[displayIndex + 2].ToString() + " " + pixels[displayIndex + 3].ToString();
                        lines.Add(coordinate + " " + color);

                    }

                }
            }

            lines[3] = "element vertex " + iIndex.ToString();

            System.IO.File.WriteAllLines(path + "\\" + fileName, lines);
            
            return true;

        }
        public static bool ToXYZFile(List<Vector3> listOfPoints, string fileNameShort, string path)
        {
            string[] lines = new string[listOfPoints.Count];
            for (int i = 0; i < listOfPoints.Count; i++)
            {
                lines[i] = listOfPoints[i].X.ToString("0.0", CultureInfo) + " " + listOfPoints[i].Y.ToString("0.0", CultureInfo) + " " + listOfPoints[i].Z.ToString("0.0", CultureInfo);

            }
                 
            System.IO.File.WriteAllLines(path + "\\" + fileNameShort, lines);

            return true;

        }
        public static bool ToXYZFile(int width, int height, ushort[] uArray, string fileName, string path)
        {
            string[] lines = new string[uArray.GetLength(0)];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    int depthIndex = (j * width) + i;
                    lines[depthIndex] = i.ToString() + " " + j.ToString() + " " + uArray[depthIndex].ToString();

                }
            }
            System.IO.File.WriteAllLines(path + "\\" + fileName, lines);

            return true;

        }
    
        public static List<Vector3> FromXYZ_Vectors(string fileName)
        {
            List<Vector3> listOfPoints = new List<Vector3>();
            int i = 0;
            try
            {
                string[] lines = System.IO.File.ReadAllLines(fileName);

                for (i = 0; i < lines.GetLength(0); i++)
                {
                    string[] arrStr1 = lines[i].Split(new Char[] { ' ' });
                    Vector3 p3D = new Vector3(Convert.ToSingle(arrStr1[0], CultureInfo), Convert.ToSingle(arrStr1[1], CultureInfo), Convert.ToSingle(arrStr1[2], CultureInfo));

                    listOfPoints.Add(p3D);


                }
            }
            catch (Exception err)
            {
                System.Windows.Forms.MessageBox.Show("Read_XYZ: read error in file :  " + fileName + " : at line: " + i.ToString() + " ; " + err.Message);
            }

            return listOfPoints;
        }
        public static List<Vector3> Read_XYZ_Vectors(string path, string fileName)
        {
            string fileNameLong = path + "\\" + fileName;
            return FromXYZ_Vectors(fileNameLong);

        }

        /// <summary>
        /// write ply file from depth data and colorInfoPixels (color info)
        /// </summary>
        /// <param name="colorInfoPixels"></param>
        /// <param name="depthData"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        //public static bool ToObjFile(byte[] colorInfoPixels, ushort[] depthData, int width, int height, string path, string fileName)
        //{
        //    StringBuilder sb = new StringBuilder();



        //    List<string> lines = new List<string>();
        //    lines.Add("####");
        //    lines.Add("#");
        //    lines.Add("# OBJ File Generated by ScannerWPF");
        //    lines.Add("#");
        //    lines.Add("####");
        //    lines.Add("#");
        //    lines.Add("# Vertices: " + depthData.Length.ToString());
        //    lines.Add("#");
        //    lines.Add("####");


        //    int iIndex = 0;

        //    for (int x = 0; x < width; ++x)
        //    {
        //        for (int y = 0; y < height; ++y)
        //        {
        //            int depthIndex = (y * width) + x;
        //            int displayIndex = depthIndex * BYTES_PER_PIXEL;
        //            if (colorInfoPixels[displayIndex + 3] != 0 && depthData[depthIndex] != 0)
        //            {

        //                iIndex++;
        //                string coordinate = x.ToString(CultureInfo) + " " + y.ToString(CultureInfo) + " " + depthData[depthIndex].ToString(CultureInfo);
        //                string color = (colorInfoPixels[displayIndex] / 255F).ToString(CultureInfo) + " " + (colorInfoPixels[displayIndex + 1] / 255F).ToString(CultureInfo) + " " + (colorInfoPixels[displayIndex + 2] / 255F).ToString(CultureInfo) + " ";

        //                //string color = pixels[displayIndex].ToString() + " " + pixels[displayIndex + 1].ToString() + " " + pixels[displayIndex + 2].ToString() + " " + pixels[displayIndex + 3].ToString();
        //                lines.Add("v " + coordinate + " " + color);

        //            }

        //        }
        //    }


        //    System.IO.File.WriteAllLines(path + "\\" + fileName, lines);

        //    return true;

        //}
        /// <summary>
        /// write ply file from depth data and colorInfoPixels (color info)
        /// </summary>
        /// <param name="colorInfoPixels"></param>
        /// <param name="depthData"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        //public static bool ToObjFile(List<Vector3> listVector3, string path, string fileName)
        //{
        //    StringBuilder sb = new StringBuilder();



        //    List<string> lines = new List<string>();
        //    lines.Add("####");
        //    lines.Add("#");
        //    lines.Add("# OBJ File Generated by OpenTKExtension");
        //    lines.Add("#");
        //    lines.Add("####");
        //    lines.Add("#");
        //    lines.Add("# Vertices: " + listVector3.Count.ToString());
        //    lines.Add("#");
        //    lines.Add("####");

        //    for (int i = 0; i < listVector3.Count; ++i)
        //    {

        //        Vector3 v = listVector3[i];
              
        //        string coordinate = v.X.ToString(CultureInfo) + " " + v.Y.ToString(CultureInfo) + " " + v.Z.ToString(CultureInfo);
        //        string color = string.Empty;
              
        //        //string color = pixels[displayIndex].ToString() + " " + pixels[displayIndex + 1].ToString() + " " + pixels[displayIndex + 2].ToString() + " " + pixels[displayIndex + 3].ToString();
        //        lines.Add("v " + coordinate + " " + color);

        //    }


        //    System.IO.File.WriteAllLines(path + "\\" + fileName, lines);

        //    return true;

        //}
        /// <summary>
        /// write ply file from depth data and colorInfoPixels (color info)
        /// </summary>
        /// <param name="colorInfoPixels"></param>
        /// <param name="depthData"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        //public static bool ToObjFile_ColorInVertex_NotUsed(PointCloud pc, string path, string fileName)
        //{
        //    StringBuilder sb = new StringBuilder();



        //    List<string> lines = new List<string>();
        //    lines.Add("####");
        //    lines.Add("#");
        //    lines.Add("# OBJ File Generated by OpenTKExtension");
        //    lines.Add("#");
        //    lines.Add("####");
        //    lines.Add("#");
        //    lines.Add("# Vertices: " + pc.Count.ToString());
        //    lines.Add("#");
        //    lines.Add("####");

        //    for (int i = 0; i < pc.Count; ++i)
        //    {

        //        Vertex v = pc[i];
        //        //float[] colorVal = pc[i].Color.ToF();
        //        string coordinate = v.Vector.X.ToString(CultureInfo) + " " + v.Vector.Y.ToString(CultureInfo) + " " + v.Vector.Z.ToString(CultureInfo);
        //        string color = string.Empty;
        //        if (v.Color != null)
        //        {
        //            // 255F
        //            color = v.Color[0].ToString(CultureInfo) + " " + v.Color[1].ToString(CultureInfo) + " " + v.Color[2].ToString(CultureInfo) + " ";
        //        }
        //        //string color = pixels[displayIndex].ToString() + " " + pixels[displayIndex + 1].ToString() + " " + pixels[displayIndex + 2].ToString() + " " + pixels[displayIndex + 3].ToString();
        //        lines.Add("v " + coordinate + " " + color);

        //    }


        //    System.IO.File.WriteAllLines(path + "\\" + fileName, lines);

        //    return true;

        //}
        /// <summary>
        /// write ply file from depth data and colorInfoPixels (color info)
        /// </summary>
        /// <param name="colorInfoPixels"></param>
        /// <param name="depthData"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool ToObjFile_ColorInVertex(PointCloud pc, string path, string fileName)
        {
            return ToObjFile_ColorInVertex(pc, path + "\\" + fileName);
       

        }
   

      
          /// <summary>
        /// write ply file from depth data and colorInfoPixels (color info)
        /// </summary>
        /// <param name="colorInfoPixels"></param>
        /// <param name="depthData"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool ToObjFile_ColorInVertex(PointCloud pc, string fileNameWithPath)
        {
            if(pc == null || pc.Vectors == null || pc.Vectors.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("No Point Cloud available to save");
                return false;

            }
            StringBuilder sb = new StringBuilder();



            List<string> lines = new List<string>();
            lines.Add("####");
            lines.Add("#");
            lines.Add("# OBJ File Generated by OpenTKExtension");
            lines.Add("#");
            lines.Add("####");
            lines.Add("#");
            lines.Add("# Vertices: " + pc.Vectors.Length.ToString());
            if (pc.Triangles != null)
                lines.Add("# Faces: " + pc.Triangles.Count.ToString());
            lines.Add("#");
            lines.Add("####");

            if (pc.Colors != null && pc.Colors.GetLength(0) == pc.Vectors.Length)
            {


                for (int i = 0; i < pc.Vectors.Length; ++i)
                {

                    Vector3 v = pc.Vectors[i];
                    Vector3 colorVal = pc.Colors[i];
                    string coordinate = v.X.ToString(CultureInfo) + " " + v.Y.ToString(CultureInfo) + " " + v.Z.ToString(CultureInfo);
                    string color = string.Empty;
                    if (colorVal != null)
                    {
                        // 255F
                        color = colorVal[0].ToString(CultureInfo) + " " + colorVal[1].ToString(CultureInfo) + " " + colorVal[2].ToString(CultureInfo) + " ";
                    }
                    //string color = pixels[displayIndex].ToString() + " " + pixels[displayIndex + 1].ToString() + " " + pixels[displayIndex + 2].ToString() + " " + pixels[displayIndex + 3].ToString();
                    lines.Add("v " + coordinate + " " + color);

                }
            }
            else
                for (int i = 0; i < pc.Vectors.Length; ++i)
                {

                    Vector3 v = pc.Vectors[i];

                    string coordinate = v.X.ToString(CultureInfo) + " " + v.Y.ToString(CultureInfo) + " " + v.Z.ToString(CultureInfo);

                    //string color = pixels[displayIndex].ToString() + " " + pixels[displayIndex + 1].ToString() + " " + pixels[displayIndex + 2].ToString() + " " + pixels[displayIndex + 3].ToString();
                    lines.Add("v " + coordinate);

                }

            if (pc.Triangles != null)
            {
                for (int i = 0; i < pc.Triangles.Count; i++)
                {
                    Triangle t = pc.Triangles[i];


                    //programs like Meshlab expect indices starting from one!
                    string face = (t.IndVertices[0] + 1).ToString(CultureInfo) + " " + (t.IndVertices[1] + 1).ToString(CultureInfo) + " " + (t.IndVertices[2] + 1).ToString(CultureInfo);


                    lines.Add("f " + face);
                }

            }
                System.IO.File.WriteAllLines(fileNameWithPath, lines);
            return true;
        }
          /// <summary>
        /// write ply file from depth data and colorInfoPixels (color info)
        /// </summary>
        /// <param name="colorInfoPixels"></param>
        /// <param name="depthData"></param>
        /// <param name="path"></param>
        /// <param name="fileNameWithPath"></param>
        /// <returns></returns>
        public static bool ToObjFile_Texture(PointCloud pc, string path, string fileName)
        {
            return ToObjFile_Texture(pc, path + "\\" + fileName);
        }
        /// <summary>
        /// write ply file from depth data and colorInfoPixels (color info)
        /// </summary>
        /// <param name="colorInfoPixels"></param>
        /// <param name="depthData"></param>
        /// <param name="path"></param>
        /// <param name="fileNameWithPath"></param>
        /// <returns></returns>
        public static bool ToObjFile_Texture(PointCloud pc, string fileNameWithPath)
        {
            if (pc == null || pc.Vectors == null || pc.Vectors.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("No Point Cloud available to save");
                return false;

            }

           

            //System.Drawing.Image im = pc.Texture.BitmapTexture;

            string fileName, path;
            IOUtils.ExtractDirectoryAndNameFromFileName(fileNameWithPath, out fileName,  out path);
            fileName = IOUtils.ExtractFileNameWithoutExtension(fileName);
            string fileNameMTL = fileName + "_Materials.mtl";
            fileName = fileName + "_Texture.png";
            

            StringBuilder sb = new StringBuilder();



            List<string> lines = new List<string>();
            lines.Add("####");
            lines.Add("#");
            lines.Add("# OBJ File Generated by OpenTKExtension");
            lines.Add("#");
            //texture:
            pc.TextureCreateFromColors();
            if(pc.Texture != null)
            {
                lines.Add("mtllib " + fileNameMTL);

                string[] linesTexture = new string[] {"map_Kd " + fileName};
                System.IO.File.WriteAllLines(path + fileNameMTL, linesTexture);

               
                pc.Texture.BitmapTexture.SaveImage(path + fileName);
                //pc.InitUVsFromVectors();
                //pc.Texture.To
            }
          

            lines.Add("####");
            lines.Add("#");
            lines.Add("# Vertices: " + pc.Vectors.Length.ToString());
            if (pc.Triangles != null)
                lines.Add("# Faces: " + pc.Triangles.Count.ToString());
            lines.Add("#");
            lines.Add("####");


            for (int i = 0; i < pc.Vectors.Length; ++i)
            {

                Vector3 v = pc.Vectors[i];
                string coordinate = v.X.ToString(CultureInfo) + " " + v.Y.ToString(CultureInfo) + " " + v.Z.ToString(CultureInfo);

                lines.Add("v " + coordinate);

            }

            if (pc.Triangles != null)
            {
                for (int i = 0; i < pc.Triangles.Count; i++)
                {
                    Triangle t = pc.Triangles[i];

                    //programs like Meshlab expect indices starting from one!
                    string face = (t.IndVertices[0] + 1).ToString(CultureInfo) + " " + (t.IndVertices[1] + 1).ToString(CultureInfo) + " " + (t.IndVertices[2] + 1).ToString(CultureInfo);


                    lines.Add("f " + face);
                }

            }
            if (pc.Texture != null)
            {
                for (int i = 0; i < pc.TextureUVs.Length; i++)
                {
                    Vector2 v = pc.TextureUVs[i];
                    string coordinate = v.X.ToString(CultureInfo) + " " + v.Y.ToString(CultureInfo);

                    lines.Add("vt " + coordinate);

                    
                }

            }
            System.IO.File.WriteAllLines(fileNameWithPath, lines);
            return true;
        }
        /// <summary>
        /// write ply file from depth data and colorInfoPixels (color info)
        /// </summary>
        /// <param name="colorInfoPixels"></param>
        /// <param name="depthData"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        //public static bool ToObjFile(Vector2[] pc, string fileNameWithPath)
        //{
        //    StringBuilder sb = new StringBuilder();



        //    List<string> lines = new List<string>();
        //    lines.Add("####");
        //    lines.Add("#");
        //    lines.Add("# OBJ File Generated by OpenTKExtension");
        //    lines.Add("#");
        //    lines.Add("####");
        //    lines.Add("#");
        //    lines.Add("# Vertices: " + pc.Length.ToString());
        //    lines.Add("#");
        //    lines.Add("####");


        //    for (int i = 0; i < pc.Length; ++i)
        //    {

        //        Vector2 v = pc[i];

        //        string coordinate = v.X.ToString(CultureInfo) + " " + v.Y.ToString(CultureInfo) + " 1.";

        //        //string color = pixels[displayIndex].ToString() + " " + pixels[displayIndex + 1].ToString() + " " + pixels[displayIndex + 2].ToString() + " " + pixels[displayIndex + 3].ToString();
        //        lines.Add("v " + coordinate);

        //    }


        //    System.IO.File.WriteAllLines(fileNameWithPath, lines);
        //    return true;
        //}
   
        /// <summary>
        /// write ply file from depth data and colorInfoPixels (color info)
        /// </summary>
        /// <param name="colorInfoPixels"></param>
        /// <param name="depthData"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool Write_OBJ_Test(PointCloud pc, PointCloud pcTest, string path, string fileName)
        {
            StringBuilder sb = new StringBuilder();



            List<string> lines = new List<string>();
            lines.Add("####");
            lines.Add("#");
            lines.Add("# OBJ File Generated by OpenTKExtension");
            lines.Add("#");
            lines.Add("####");
            lines.Add("#");
            lines.Add("# Vertices: " + pc.Vectors.Length.ToString());
            lines.Add("#");
            lines.Add("####");

            if (pc.Colors != null && pc.Colors.GetLength(0) == pc.Vectors.Length)
            {


                for (int i = 0; i < pc.Vectors.Length; ++i)
                {

                    Vector3 v = pc.Vectors[i];
                    Vector3 vTest = pcTest.Vectors[i];
                    Vector3 colorVal = pc.Colors[i];
                    string coordinate = v.X.ToString(CultureInfo) + " " + v.Y.ToString(CultureInfo) + " " + v.Z.ToString(CultureInfo);
                    string color = string.Empty;
                    if (colorVal != null)
                    {
                        // 255F
                        color = colorVal[0].ToString(CultureInfo) + " " + colorVal[1].ToString(CultureInfo) + " " + colorVal[2].ToString(CultureInfo) + " ";
                    }
                    //string color = pixels[displayIndex].ToString() + " " + pixels[displayIndex + 1].ToString() + " " + pixels[displayIndex + 2].ToString() + " " + pixels[displayIndex + 3].ToString();
                    lines.Add("v " + coordinate + " " + color);

                    string coordinate1 = vTest.X.ToString(CultureInfo) + " " + vTest.Y.ToString(CultureInfo) + " " + vTest.Z.ToString(CultureInfo);
                    lines.Add("- " + coordinate1);


                }
            }
            else
                for (int i = 0; i < pc.Vectors.Length; ++i)
                {

                    Vector3 v = pc.Vectors[i];
                    Vector3 vTest = pcTest.Vectors[i];
                    

                    string coordinate = v.X.ToString(CultureInfo) + " " + v.Y.ToString(CultureInfo) + " " + v.Z.ToString(CultureInfo);
                    string coordinate1 = vTest.X.ToString(CultureInfo) + " " + vTest.Y.ToString(CultureInfo) + " " + vTest.Z.ToString(CultureInfo);

                    //string color = pixels[displayIndex].ToString() + " " + pixels[displayIndex + 1].ToString() + " " + pixels[displayIndex + 2].ToString() + " " + pixels[displayIndex + 3].ToString();
                    lines.Add("v " + coordinate);
                    lines.Add("-" + coordinate1);


                }


            System.IO.File.WriteAllLines(path + "\\" + fileName, lines);

            return true;

        }
        ///// <summary>
        ///// write ply file from depth data and colorInfoPixels (color info)
        ///// </summary>
        ///// <param name="colorInfoPixels"></param>
        ///// <param name="depthData"></param>
        ///// <param name="path"></param>
        ///// <param name="fileName"></param>
        ///// <returns></returns>
        //public static bool ToObjFile(byte[] colorInfoPixels, double[] depthData, int width, int height, string path, string fileName)
        //{
        //    StringBuilder sb = new StringBuilder();



        //    List<string> lines = new List<string>();
        //    lines.Add("####");
        //    lines.Add("#");
        //    lines.Add("# OBJ File Generated by OpenTKExtension");
        //    lines.Add("#");
        //    lines.Add("####");
        //    lines.Add("#");
        //    lines.Add("# Vertices: " + depthData.Length.ToString());
        //    lines.Add("#");
        //    lines.Add("####");


        //    int iIndex = 0;

        //    for (int x = 0; x < width; ++x)
        //    {
        //        for (int y = 0; y < height; ++y)
        //        {
        //            int depthIndex = (y * width) + x;
        //            int displayIndex = depthIndex * BYTES_PER_PIXEL;
        //            if (colorInfoPixels[displayIndex + 3] != 0 && depthData[depthIndex] != 0)
        //            {

        //                iIndex++;
        //                string coordinate = x.ToString(CultureInfo) + " " + y.ToString(CultureInfo) + " " + depthData[depthIndex].ToString(CultureInfo);
        //                string color = (colorInfoPixels[displayIndex] / 255F).ToString(CultureInfo) + " " + (colorInfoPixels[displayIndex + 1] / 255F).ToString(CultureInfo) + " " + (colorInfoPixels[displayIndex + 2] / 255F).ToString(CultureInfo) + " ";

        //                //string color = pixels[displayIndex].ToString() + " " + pixels[displayIndex + 1].ToString() + " " + pixels[displayIndex + 2].ToString() + " " + pixels[displayIndex + 3].ToString();
        //                lines.Add("v " + coordinate + " " + color);

        //            }

        //        }
        //    }


        //    System.IO.File.WriteAllLines(path + "\\" + fileName, lines);

        //    return true;

        //}

        /// <summary>
        /// reads the OBJ file ONLY with the special format used also in the write_OBJ method
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileNameShort"></param>
        /// <param name="depthData"></param>
        /// <returns></returns>
        public static void ObjFileToArray(string path, string fileNameShort, int width, int height, ref ushort[,] depth, ref byte[,] colorInfoR, ref byte[,] colorInfoG, ref byte[,] colorInfoB, ref byte[,] colorInfoA)
        {
            string fileName = path + "\\" + fileNameShort;
            if (!System.IO.File.Exists(fileName))
            {
                System.Diagnostics.Debug.WriteLine("File does not exist: ");
                return ;

            }

            ushort[] p3D = new ushort[3];
            System.Collections.ArrayList lineList = new System.Collections.ArrayList();

            depth = new ushort[width, height];
            colorInfoR = new byte[width, height];
            colorInfoG = new byte[width, height];
            colorInfoB = new byte[width, height];
            colorInfoA = new byte[width, height];



            int numberOfDepthPointsNonZero = -1;
            int i = 0;
            int startIndex = 0;
            try
            {
                string[] lines = System.IO.File.ReadAllLines(fileName);

                //ignore the comment lines
                
                for (i = 0; i < lines.Length; i++)
                {
                    if (!lines[i].Contains('#'))
                    {
                        startIndex = i;
                        break;
                    }

                }

                for ( i = startIndex + 1; i < lines.GetLength(0); i++)
                {
                    
                    string[] arrStr1 = lines[i].Split(new Char[] { ' ' });
                    if (lines[i].Contains('#') || arrStr1.Length < 4)
                    {
                        //ignore empty and comment lines
                        continue;
                    }
                    //if (arrStr1.Length < 4)
                    //{
                    //    MessageBox.Show("Error reading file " + fileName + " in line : " + i.ToString() + " : " + lines[i]);
                    //}
                    if (arrStr1[0] == "vn")
                    {
                        //ignore vector normals for now
                    }
                    if (arrStr1[0] == "v")
                    {
                        if (arrStr1.Length < 7)
                        {
                            System.Windows.Forms.MessageBox.Show("Error reading file " + fileName + " in line i");
                        }
                        else
                        {

                            for (int j = 0; j < 3; j++)
                            {
                                double f = Convert.ToSingle(arrStr1[j + 1], CultureInfo);
                                p3D[j] = Convert.ToUInt16(f);
                            }
                            depth[p3D[0], p3D[1]] = p3D[2];


                            colorInfoR[p3D[0], p3D[1]] = Convert.ToByte(255 * Convert.ToSingle(arrStr1[4], CultureInfo));
                            colorInfoG[p3D[0], p3D[1]] = Convert.ToByte(255 * Convert.ToSingle(arrStr1[5], CultureInfo));
                            colorInfoB[p3D[0], p3D[1]] = Convert.ToByte(255 * Convert.ToSingle(arrStr1[6], CultureInfo));

                            colorInfoA[p3D[0], p3D[1]] = 255;

                            if (depth[p3D[0], p3D[1]] > 0)
                            {
                                numberOfDepthPointsNonZero++;

                            }

                        }
                    }                    
                   
                }
            }
            catch (Exception err)
            {
                System.Windows.Forms.MessageBox.Show("Read_OBJ: read error in file :  " + fileName + " : at line: " + i.ToString() + " ; " + err.Message);
            }


        }

        ///// <summary>
        ///// reads the OBJ file ONLY with the special format used also in the write_OBJ method
        ///// </summary>
        ///// <param name="path"></param>
        ///// <param name="fileNameShort"></param>
        ///// <param name="depthData"></param>
        ///// <returns></returns>
        //public static PointCloud FromObjFileToVertices_NotUsed(string path, string fileNameShort)
        //{
        //    string fileName = path + "\\" + fileNameShort;
        //    if (!System.IO.File.Exists(fileName))
        //    {
        //        System.Diagnostics.Debug.WriteLine("File does not exist: ");
        //        return null;

        //    }
        //    double[] p3D = new double[3];
        //    System.Collections.ArrayList lineList = new System.Collections.ArrayList();


        //    PointCloud pointCloud = new PointCloud();


        //    //int numberOfDepthPointsNonZero = -1;
        //    int i = 0;
        //    int startIndex = 0;
        //    try
        //    {
        //        string[] lines = System.IO.File.ReadAllLines(fileName);

        //        //ignore the comment lines

        //        for (i = 0; i < lines.Length; i++)
        //        {
        //            if (!lines[i].Contains('#'))
        //            {
        //                startIndex = i;
        //                break;
        //            }

        //        }

        //        for (i = startIndex; i < lines.GetLength(0); i++)
        //        //for (i = lines.Length -1 ; i >= startIndex + 1; i--)
        //        {

        //            string[] arrStr1 = lines[i].Split(new Char[] { ' ' });
        //            if (lines[i].Contains('#') || arrStr1.Length < 4)
        //            {
        //                //ignore empty and comment lines
        //                continue;
        //            }

        //            if (arrStr1[0] == "vn")
        //            {
        //                //ignore vector normals for now
        //            }
        //            if (arrStr1[0] == "v")
        //            {
        //                if (arrStr1.Length < 7)
        //                {
        //                    System.Windows.Forms.MessageBox.Show("Error reading file " + fileNameShort + " in line i");
        //                }
        //                else
        //                {

        //                    for (int j = 0; j < 3; j++)
        //                    {
        //                        p3D[j] = Convert.ToSingle(arrStr1[j + 1], CultureInfo);
        //                    }
        //                    Vector3 vec = new Vector3(p3D[0], p3D[1], p3D[2]);
        //                    float[] color = new float[4] {Convert.ToSingle(arrStr1[4], CultureInfo), Convert.ToSingle(arrStr1[5], CultureInfo), Convert.ToSingle(arrStr1[6], CultureInfo), 1f};
        //                    //System.Drawing.Color c = System.Drawing.Color.White;
        //                    //c = c.FromFloatsARGB(color[3], color[0], color[1], color[2]);
        //                    Vector3 c = new Vector3(color[0], color[1], color[2]);
        //                    Vertex v = new Vertex(vec, c, 0);
        //                    pointCloud.Add(v);



        //                }
        //            }

        //        }
        //    }
        //    catch (Exception err)
        //    {
        //        System.Windows.Forms.MessageBox.Show("Read_OBJ: read error in file :  " + fileNameShort + " : at line: " + i.ToString() + " ; " + err.Message);
        //    }

        //    return pointCloud;

        //}
        public static void PointCloudFromObjectFile(PointCloud pc, string fileOBJ)
        {
            IOUtils.ExtractDirectoryAndNameFromFileName(fileOBJ, out pc.Name, out pc.Path);

            pc.FileNameLong = pc.Path + "\\" + pc.Name;


            string line = string.Empty;

            Vector3 vector;
            Vector3 color;

            List<Vector3> vectors = new List<Vector3>();
            List<Vector3> colors = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> textureCoords = new List<Vector2>();

            List<uint> indices = new List<uint>();
            List<uint> indicesNormals = new List<uint>();
            //List<uint> indicesTexture = new List<uint>();

            try
            {

                using (StreamReader streamReader = new StreamReader(fileOBJ))
                {

                    while (!streamReader.EndOfStream)
                    {
                        line = streamReader.ReadLine().Trim();

                        if (!line.StartsWith("#"))
                        {
                            while (line.EndsWith("\\"))
                                line = line.Substring(0, line.Length - 1) + streamReader.ReadLine().Trim();
                            string str1 = GlobalVariables.TreatLanguageSpecifics(line);
                            string[] strArrayRead = str1.Split();
                            if (strArrayRead.Length >= 0)
                            {
                                switch (strArrayRead[0].ToLower())
                                {
                                    case "mtllib":
                                        if (strArrayRead.Length < 2)
                                        {
                                            System.Windows.Forms.MessageBox.Show("Error reading obj file (mtllib) in line : " + line);
                                        }

                                        pc.Texture = IOUtils.Read_MTLFile(strArrayRead[1], pc.Path);
                                        //this.Texture = new Texture();
                                        break;
                                    case "v"://Vertex
                                        IOUtils.HelperReadVector3AndColor(strArrayRead, out vector, out color);
                                        vectors.Add(vector);
                                        colors.Add(color);


                                        break;
                                    case "vt"://Texture
                                        if (strArrayRead.Length < 3)
                                        {
                                            System.Windows.Forms.MessageBox.Show("Error reading obj file (Texture) in line : " + line);
                                        }
                                        Vector2 vector1 = new Vector2(0, 0);
                                        float.TryParse(strArrayRead[1], NumberStyles.Float | NumberStyles.AllowThousands, GlobalVariables.CurrentCulture, out vector1.X);
                                        float.TryParse(strArrayRead[2], NumberStyles.Float | NumberStyles.AllowThousands, GlobalVariables.CurrentCulture, out vector1.Y);
                                        textureCoords.Add(vector1);
                                        break;
                                    case "vn"://Normals
                                        if (strArrayRead.Length < 4)
                                        {
                                            System.Windows.Forms.MessageBox.Show("Error reading obj file (Normals) in line : " + line);
                                        }
                                        Vector3 vector2 = new Vector3(0, 0, 0);
                                        float.TryParse(strArrayRead[1], NumberStyles.Float | NumberStyles.AllowThousands, GlobalVariables.CurrentCulture, out vector2.X);
                                        float.TryParse(strArrayRead[2], NumberStyles.Float | NumberStyles.AllowThousands, GlobalVariables.CurrentCulture, out vector2.Y);
                                        float.TryParse(strArrayRead[3], NumberStyles.Float | NumberStyles.AllowThousands, GlobalVariables.CurrentCulture, out vector2.Z);
                                        //vector2.NormalizeNew();
                                        normals.Add(vector2);
                                        break;

                                    case "f":
                                        IOUtils.ReadFaceLine(strArrayRead, indices, indicesNormals, textureCoords);
                                        break;
                                    case "g":
                                        //if (myNewModel.Triangles.Count > 0)
                                        //{
                                        //    if (myNewModel.TextureBitmap != null)
                                        //    {
                                        //        p.ColorOverall = System.Drawing.Color.FromArgb(1, 1, 1);
                                        //    }
                                        //    else
                                        //    {
                                        //        float r = Convert.ToSingle(0.3 * Math.Cos((float)(23 * myNewModel.Parts.Count)) + 0.5);
                                        //        float g = Convert.ToSingle(0.5f * Math.Cos((float)(17 * myNewModel.Parts.Count + 1)) + 0.5);
                                        //        float b = Convert.ToSingle(0.5f * Math.Cos((float)myNewModel.Parts.Count) + 0.5);


                                        //        p.ColorOverall = System.Drawing.Color.FromArgb(Convert.ToInt32(r * byte.MaxValue), Convert.ToInt32(g * byte.MaxValue), Convert.ToInt32(b * byte.MaxValue));
                                        //    }
                                        //    //p.ColorOverall = myNewModel.TextureBitmap != null ? new Vector3(1, 1, 1) : new Vector3(0.3 * Math.Cos((float)(23 * myNewModel.Parts.Count)) + 0.5, 0.5f * Math.Cos((float)(17 * myNewModel.Parts.Count + 1)) + 0.5, 0.5f * Math.Cos((float)myNewModel.Parts.Count) + 0.5);
                                        //    myNewModel.Parts.Add(new Part(p));
                                        //}
                                        //if (strArrayRead.Length > 1)
                                        //    p.Name = str1.Replace(strArrayRead[1], "");
                                        //myNewModel.Triangles.Clear();
                                        break;
                                }
                            }
                        }
                    }

                    streamReader.Close();

                }


            }
            catch (Exception err)
            {
                System.Windows.Forms.MessageBox.Show("Error reading obj file (general): " + line + " ; " + err.Message);
            }
            if (indices.Count != vectors.Count)
            {
                for (uint i = Convert.ToUInt32(indices.Count); i < vectors.Count; i++)
                {
                    indices.Add(i);

                }
            }


            //add texture coordinates!!
            pc.AssignData(vectors, colors, normals, indices, indicesNormals, textureCoords);


            //this.pointCloudSource.Texture = new Texture(path + "Textures\\AlternatingBrick-ColorMap.png");
            //this.pointCloudSource.InitCubeUVs();
        }
        public static PointCloud FromObjFile(string fileOBJ)
        {
            
            PointCloud pc = new PointCloud();
            PointCloudFromObjectFile(pc, fileOBJ);

            return pc;

        }
        /// <summary>
     
        /// write obj file from depth data and colorInfoPixels (color info)
        /// </summary>
        /// <param name="colorInfoPixels"></param>
        /// <param name="depthData"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        //public static bool ToObjFile(List<byte[]> depthData, string path, string fileName)
        //{
        //    StringBuilder sb = new StringBuilder();



        //    List<string> lines = new List<string>();
          
        //    lines.Add("# Number of vectors: " + depthData.Count.ToString());
        

        //    int iIndex = 0;

        //    for (int i = 0; i < depthData.Count; ++i)
        //    {

        //        iIndex++;
        //        string coordinate = depthData[i][0].ToString(CultureInfo) + " " + depthData[i][1].ToString(CultureInfo) + " " + depthData[i][2].ToString(CultureInfo);

        //        //string color = pixels[displayIndex].ToString() + " " + pixels[displayIndex + 1].ToString() + " " + pixels[displayIndex + 2].ToString() + " " + pixels[displayIndex + 3].ToString();
        //        lines.Add(coordinate + "   ");
        //       // lines.Add("v " + coordinate + " ");


        //    }





        //    System.IO.File.WriteAllLines(path + "\\" + fileName, lines);

        //    return true;

        //}


        public static ushort[] CreatePointArrayOneDim(ushort[,] pointMatrix, int width, int height)
        {
            ushort[] pointArray = new ushort[width * height];

            //int nIndex = -1;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int depthIndex = (y * width) + x;
                    pointArray[depthIndex] = pointMatrix[x, y];

                }


            }
            return pointArray;


        }



    }
}
