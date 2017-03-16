using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.IO;
using System.Drawing;
using System.Globalization;

namespace OpenTK.Extension
{
    public class Model
    {
        public RenderableObject RenderableObject;

        //public PointCloudGL pointCloudGL;
        public Bitmap Texture;
        public List<float[]> TextureCoords = new List<float[]>();


        public Model()
        {
        }
        public Model(RenderableObject myRenderableObject)
        {
            RenderableObject = myRenderableObject;
        }
        public Model(string path, string fileName)
            : this(path + "\\" + fileName)
        {

        }
        public Model(string fileName)
        {
            string str = Path.GetExtension(fileName).ToLower();
            if (str == ".obj")
                ReadObjFile(fileName);
            if (str == ".xyz")
                FromXYZFile(fileName);


        }
        private void FromXYZFile(string fileName)
        {
            List<Vector3> colors = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<uint> triangles = new List<uint>();
            List<uint> indicesNormals = new List<uint>();
            List<uint> indicesTexture = new List<uint>();

            List<Vector3> vectors = PointCloudIO.Read_XYZ_Vectors(fileName);
            for (uint i = 0; i < vectors.Count; i++)
            {
                triangles.Add(i);
                colors.Add(new Vector3(1f, 1f, 1f));
            }

            this.RenderableObject.PointCloudGL = new PointCloudGL(vectors, colors, normals, triangles, indicesNormals, indicesTexture);

        }
        private void ReadObjFile(string fileOBJ)
        {

            string line = string.Empty;

            Vector3 vector;
            Vector3 color;

            List<Vector3> vectors = new List<Vector3>();
            List<Vector3> colors = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<uint> triangles = new List<uint>();
            List<uint> indicesNormals = new List<uint>();
            List<uint> indicesTexture = new List<uint>();

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

                                        this.Texture = IOUtils.ReadTexture(strArrayRead[1], fileOBJ);
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
                                        Vector3 vector1 = new Vector3(0, 0, 0);
                                        float.TryParse(strArrayRead[1], NumberStyles.Float | NumberStyles.AllowThousands, (IFormatProvider)null, out vector1.X);
                                        float.TryParse(strArrayRead[2], NumberStyles.Float | NumberStyles.AllowThousands, (IFormatProvider)null, out vector1.Y);
                                        this.TextureCoords.Add(new float[2] { (float)vector1.X, (float)vector1.Y });
                                        break;
                                    case "vn"://Normals
                                        if (strArrayRead.Length < 4)
                                        {
                                            System.Windows.Forms.MessageBox.Show("Error reading obj file (Normals) in line : " + line);
                                        }
                                        Vector3 vector2 = new Vector3(0, 0, 0);
                                        float.TryParse(strArrayRead[1], NumberStyles.Float | NumberStyles.AllowThousands, (IFormatProvider)null, out vector2.X);
                                        float.TryParse(strArrayRead[2], NumberStyles.Float | NumberStyles.AllowThousands, (IFormatProvider)null, out vector2.Y);
                                        float.TryParse(strArrayRead[3], NumberStyles.Float | NumberStyles.AllowThousands, (IFormatProvider)null, out vector2.Z);
                                        //vector2.NormalizeNew();
                                        normals.Add(vector2);
                                        break;

                                    case "f":
                                        IOUtils.ReadIndicesLine(strArrayRead, triangles, indicesNormals, indicesTexture);
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
            if (triangles.Count != vectors.Count)
            {
                for (uint i = Convert.ToUInt32(triangles.Count); i < vectors.Count; i++)
                {
                    triangles.Add(i);

                }
                
                //for (uint i = Convert.ToUInt32(triangles.Count); i < vectors.Count; i++)
                //{
                //    triangles.Add(i);

                //}
            }
            this.RenderableObject = new PointCloudRenderable();
            this.RenderableObject.PointCloudGL = new PointCloudGL(vectors, colors, normals, triangles, indicesNormals, indicesTexture);
            

        }
    }
}
