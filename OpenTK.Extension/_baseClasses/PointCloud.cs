using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.IO;
using System.Globalization;
using System.Drawing;

namespace OpenTKExtension
{
   
    public partial class PointCloud 
    {
        public Vector3[] Vectors;
        public Vector3[] Colors;
        //triangle part
        public uint[] Indices;

        public Vector3[] Normals;
        public uint[] IndicesNormals;
   
      
        //public Bitmap TextureBitmap;

        public List<Triangle> Triangles;
        public bool DisregardCenteredShowing;
        //public List<float[]> TextureCoords;
        //for texture:
        public Texture Texture;  
        public Vector2[] TextureUVs;

        //public List<Triangle> Triangles = new List<Triangle>();

        //public List<float[]> TextureCoords = new List<float[]>();



        private BoundingBox boundingBox;
        //privates

        private Vector3 centroid;
        private Vector3 centroidOld;

        //Vector3 boundingBoxMax;
        //Vector3 boundingBoxMin;

        bool centroidAndBoundingBoxCalculated;

        public string Path;
        public string FileNameLong;
        public string Name;

        private List<VertexKDTree> vectorsWithIndex;

        public PointCloud()
        {
            Name = string.Empty;
        }
        public PointCloud(int dim) : this()
        {
            Vectors = new Vector3[dim];
            Colors = new Vector3[dim];
            Indices = new uint[dim];

        }

        public PointCloud(List<Vector3> vectors, List<Vector3> colors, List<Vector3> normals, List<uint> indices, List<uint> indicesNormals, List<Vector2> textures) :this()
        {
            AssignData(vectors, colors, normals, indices, indicesNormals, textures);

        }
        public PointCloud(Vector3[] vectors, Vector3[] colors, Vector3[] normals, uint[] indices, uint[] indicesNormals, Vector2[] textures) : this()
        {
            AssignData(vectors, colors, normals, indices, indicesNormals, textures);

        }
        public Vector3 CentroidVector
        {
            get
            {
                if (!centroidAndBoundingBoxCalculated)
                    CalculateBoundingBox();
                return centroid;

            }

        }
        public Vector3 CentroidVectorRecalc
        {
            get
            {
                CalculateBoundingBox();
                return centroid;

            }

        }
        public BoundingBox BoundingBox
        {
            get
            {
                if(!this.centroidAndBoundingBoxCalculated)
                    CalculateBoundingBox();
                return this.boundingBox;
            }
        }
        public void CalculateBoundingBox()
        {
            this.CalculateCentroidDouble();
            this.boundingBox = BoundingBox.FromPointCloud(this);
            
            centroidAndBoundingBoxCalculated = true;
        }
        private Vector3 CalculateCentroidDouble()
        {
            //have to calculate in doubles - otherwise we loose 1-2 digits of precision
            Vector3d vC = new Vector3d();
           
            for (int i = 0; i < Vectors.Length; i++)
            {
                vC.X += Vectors[i].X;
                vC.Y += Vectors[i].Y;
                vC.Z += Vectors[i].Z;
            }
            vC /= Vectors.Length;
            centroid = new Vector3(Convert.ToSingle(vC.X ), Convert.ToSingle(vC.Y ), Convert.ToSingle(vC.Z));
                        
            return centroid;
        }
        
        public Vector3 BoundingBoxMax
        {
            get
            {
                if (!centroidAndBoundingBoxCalculated)
                    CalculateBoundingBox();
                //return boundingBoxMax;
                return this.BoundingBox.Max;

            }

        }
        public float NormSquaredMax
        {
            get
            {
               float normMax = float.MinValue;
               for(int i = 0; i < this.Vectors.Length; i++)
               {
                   float n = this.Vectors[i].NormSquared();
                   if (n > normMax)
                       normMax = n;
               }
               return normMax;

            }
            

        }
        public float BoundingBoxMaxFloat
        {
            get
            {
                if (!centroidAndBoundingBoxCalculated)
                    CalculateBoundingBox();
                float f = float.MinValue;
                for (int i = 0; i < 2; i++)
                {
                    f = System.Math.Max(f, System.Math.Abs(this.BoundingBox.Min[i]));
                }
                for(int i = 0; i < 2; i++)
                {
                    f = System.Math.Max(f, System.Math.Abs(BoundingBox.Max[i]));
                }
               
                return f;
            }

        }
        public float BoundingBoxMinFloat
        {
            get
            {
                if (!centroidAndBoundingBoxCalculated)
                    CalculateBoundingBox();
                float f = float.MaxValue;
                for (int i = 0; i < 2; i++)
                {
                    f = System.Math.Min(f, System.Math.Abs(this.BoundingBox.Min[i]));
                }
                for (int i = 0; i < 2; i++)
                {
                    f = System.Math.Min(f, System.Math.Abs(this.BoundingBox.Max[i]));
                }

                return f;
            }

        }
        public Vector3 BoundingBoxMin
        {
            get
            {
                if (!centroidAndBoundingBoxCalculated)
                    CalculateBoundingBox();
                return this.BoundingBox.Min;
            }

        }
     

       
        public void ResizeTo1()
        {
            this.CalculateBoundingBox();

            
            Vector3 v = this.BoundingBox.Max - this.BoundingBox.Min;

            float d = Math.Max(Math.Abs(v.X), Math.Abs(v.Y));
            d = Math.Max(d, Math.Abs(v.Z));

            if (d > 0)
            {
                this.centroid.X /= d;
                this.centroid.Y /= d;
                this.centroid.Z /= d;
                for (int i = 0; i < this.Vectors.Length; i++)
                {
                    this.Vectors[i].X /= d;
                    this.Vectors[i].Y /= d;
                    this.Vectors[i].Z /= d;
                }
            }

            //recalc again 
            this.CalculateBoundingBox();


        }
        public void Translate_StartAt_Y0()
        {
            this.CalculateBoundingBox();
            //float y = this.BoundingBox.Max.Y - this.BoundingBox.Min.Y;


            this.Translate(0, -this.BoundingBox.Min.Y, 0);

           
            //recalc again 
            this.CalculateBoundingBox();


        }
        public void Translate_StartAt_000()
        {
            this.CalculateBoundingBox();
          
            this.Translate(-this.BoundingBox.Min.X, -this.BoundingBox.Min.Y, -this.BoundingBox.Min.Z);


            this.CalculateBoundingBox();


        }
      
        public Vector3 ResetCentroid(bool centered)
        {
            if (centered)
            {

                //center point cloud to centroid
                this.centroidAndBoundingBoxCalculated = false;
                this.centroidOld = this.CentroidVector;
                SubtractVectorRef(this, this.CentroidVector);
                //centroid - is now origin
                this.centroid = new Vector3(0, 0, 0);

                this.CalculateBoundingBox();


            }
            else
            {
                //reset to old center
                if (this.centroidOld != Vector3.Zero)
                {
                    AddVectorToAll(this, this.centroidOld);
                    this.CalculateCentroidDouble();//recalcs centrooid

                    this.centroidOld = Vector3.Zero;
                 

                }

            }
            return this.centroidOld;


        }
       
     
      
        public void SubtractCloud(PointCloud pointCloud)
        {
            if (this.Vectors == null || pointCloud.Vectors == null || this.Vectors.Length != pointCloud.Vectors.Length)
            {
                System.Windows.Forms.MessageBox.Show("SW Error - cannot subtract point clouds since Vectors are not set correct");
                return;
            }

            
            for (int i = 0; i < pointCloud.Vectors.Length; i++)
            {
               this.Vectors[i] -= pointCloud.Vectors[i];
            }
         

        }
        public void SubtractVector( Vector3 centroid)
        {

            for (int i = 0; i < this.Vectors.Length; i++)
            {

                Vector3 v = this.Vectors[i];
                Vector3 translatedV = Vector3.Subtract(v, centroid);
                v = translatedV;
                this.Vectors[i] = v;
            }

        }
     
    
        public void SetColors(List<Vector3> colors)
        {
            this.Colors = new Vector3[colors.Count];
            for(int i = 0; i < colors.Count; i++)
            {
                this.Colors[i] = colors[i];
            }
        }
        public void SetColor(Vector3 color)
        {
            this.Colors = new Vector3[this.Vectors.Length];
            for (int i = 0; i < Colors.Length; i++)
            {
                this.Colors[i] = color;
            }
        }
      
        public PointCloud Clone()
        {

            return PointCloud.Clone(this);
           
        }
        public static PointCloud Clone(PointCloud pcOld)
        {
            
            PointCloud pc = new PointCloud(pcOld.Vectors, pcOld.Colors, pcOld.Normals, pcOld.Indices, pcOld.IndicesNormals, pcOld.TextureUVs);
            if (pcOld.PCAAxes != null)
                pc.PCAAxes = pcOld.PCAAxes.Clone();

            return pc;

        }
      
        public static PointCloud FromListVertexKDTree(List<VertexKDTree> vertices)
        {
            PointCloud pc = new PointCloud();
            pc.Vectors = new Vector3[vertices.Count];
            pc.Indices = new uint[vertices.Count];
            pc.Colors = new Vector3[vertices.Count];
            int notSet = 0;
            for (int i = 0; i < vertices.Count; i++)
            {
                if (vertices[i] != null)
                {
                    pc.Vectors[i] = vertices[i].Vector;
                    pc.Colors[i] = vertices[i].Color;
                    pc.Indices[i] = Convert.ToUInt32(vertices[i].Index);
                }
                else
                {
                    notSet += 1;
                }
            }
            if (notSet > 0)
                System.Diagnostics.Debug.WriteLine("Vectors not set: " + notSet.ToString() + " out of : " + vertices.Count.ToString());

            return pc;
        }
        public static PointCloud FromListVertices(List<Vertex> vertices)
        {
            PointCloud pc = new PointCloud();
            pc.Vectors = new Vector3[vertices.Count];
            pc.Indices = new uint[vertices.Count];
            pc.Colors = new Vector3[vertices.Count];

            int notSet = 0;
            for (int i = 0; i < vertices.Count; i++)
            {
                if (vertices[i] != null)
                {
                    pc.Vectors[i] = vertices[i].Vector;
                    pc.Indices[i] = Convert.ToUInt32(i);// Convert.ToUInt32(vertices[i].Index);
                    pc.Colors[i] = vertices[i].Color;
                }
                else
                {
                    notSet += 1;
                }
            }
            if (notSet > 0)
                System.Diagnostics.Debug.WriteLine("Vectors not set: " + notSet.ToString() + " out of : " + vertices.Count.ToString());

            return pc;
        }
        public static PointCloud FromListVector3(List<Vector3> vectors)
        {
            if (vectors == null || vectors.Count == 0)
                return null;
            
            List<Vector3> colors = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<uint> indices = new List<uint>();
            List<uint> indicesNormals = new List<uint>();
            List<Vector2> textures = new List<Vector2>();

            int check1 = vectors.Count;
            //System.Diagnostics.Debug.WriteLine("Number of vectors: " + vectors.Count.ToString());
            
            for (uint i = 0; i < vectors.Count; i++)
            {
                indices.Add(i);
                colors.Add(new Vector3(1f, 1f, 1f));
            }
            if (vectors.Count != check1)
            {
                //System.Windows.Forms.MessageBox.Show("Stop camera before saving");
                System.Diagnostics.Debug.WriteLine("SW error - vectors overriden during set: " + check1.ToString() + " should be: " + vectors.Count.ToString());
                return null;
            }
            else
            {
                PointCloud pc = new PointCloud(vectors, colors, normals, indices, indicesNormals, textures);
                return pc;
            }
            

        }
        public static List<Vertex> ToListVertices(PointCloud pc)
        {
            List<Vertex> vList = new List<Vertex>();
             int notSet = 0;
            for (int i = 0; i < pc.Vectors.Length; i++)
            {
                Vertex v = new Vertex(pc.Vectors[i], pc.Colors[i], pc.Indices[i]);
                vList.Add(v);


            }
            
            return vList;
        }

        public void ToObjFile(string path, string fileName)
        {
            if (GLSettings.ShowPointCloudAsTexture)
            {
                UtilsPointCloudIO.ToObjFile_Texture(this, path, fileName);
            }
            else
            {
                UtilsPointCloudIO.ToObjFile_ColorInVertex(this, path, fileName);
            }
            

        }
        public bool ToObjFile(string fileNameWithPath)
        {
            if (GLSettings.ShowPointCloudAsTexture)
            {
                return UtilsPointCloudIO.ToObjFile_Texture(this, fileNameWithPath);
            }
            else
            {
                return UtilsPointCloudIO.ToObjFile_ColorInVertex(this, fileNameWithPath);
            }
        }
        public PointCloudRenderable ToPointCloudRenderable()
        {
            PointCloudRenderable pcr = new PointCloudRenderable();
            pcr.PointCloud = this;
            return pcr;


        }


        public static PointCloud FromObjFile(string fileOBJ)
        {

            return UtilsPointCloudIO.FromObjFile(fileOBJ);

        }
        public static PointCloud FromObjFile(string path, string fileOBJ)
        {
            return UtilsPointCloudIO.FromObjFile(path + "\\" + fileOBJ);

            

        }
        public static PointCloud FromXYZFile(string fileOBJ)
        {
            PointCloud pc = new PointCloud();
            pc.ReadXYZFile(fileOBJ);
            return pc;

        }
        private void ReadXYZFile(string fileName)
        {
            this.FileNameLong = fileName;
            IOUtils.ExtractDirectoryAndNameFromFileName(this.FileNameLong, out this.Name, out this.Path);

            List<Vector3> colors = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<uint> indices = new List<uint>();
            List<uint> indicesNormals = new List<uint>();
            List<Vector2> textures = new List<Vector2>();

            List<Vector3> vectors = UtilsPointCloudIO.FromXYZ_Vectors(fileName);
            for (uint i = 0; i < vectors.Count; i++)
            {
                indices.Add(i);
                colors.Add(new Vector3(1f, 1f, 1f));
            }

            AssignData(vectors, colors, normals, indices, indicesNormals, textures);

        }
        public void AssignData(List<Vector3> vectors, List<Vector3> colors, List<Vector3> normals, List<uint> indices, List<uint> indicesNormals, List<Vector2> textures)
        {
            if (vectors != null)
                this.Vectors = vectors.ToArray();
            if (colors != null)
                this.Colors = colors.ToArray();
            if (normals != null)
                this.Normals = normals.ToArray();
            if (indices != null)
                this.Indices = indices.ToArray();
            else
                SetDefaultIndices();
            if (indicesNormals != null)
                this.IndicesNormals = indicesNormals.ToArray();
            if (textures != null)
                this.TextureUVs = textures.ToArray();

        }
        public void AssignData(Vector3[] vectors, Vector3[] colors, Vector3[] normals, uint[] indices, uint[] indicesNormals, Vector2[] textures)
        {
            if (vectors != null)
            {
                this.Vectors = new Vector3[vectors.Length];
                vectors.CopyTo(this.Vectors, 0);
            }
            if (colors != null)
            {
                this.Colors = new Vector3[colors.Length];
                colors.CopyTo(this.Colors, 0);
            }
            if (normals != null)
            {
                this.Normals = new Vector3[normals.Length];
                normals.CopyTo(this.Normals, 0);
            }
            if (indices != null)
            {
                this.Indices = new uint[indices.Length];
                indices.CopyTo(this.Indices, 0);
            }
            if (indicesNormals != null)
            {
                this.IndicesNormals = new uint[indicesNormals.Length];
                indicesNormals.CopyTo(this.IndicesNormals, 0);
            }
            if (textures != null)
            {
                this.TextureUVs= new Vector2[textures.Length];
                textures.CopyTo(this.TextureUVs, 0);
            }

        
        }
        public void SetDefaultIndices()
        {
            this.Indices = new uint[this.Vectors.Length];

            for(int i = 0; i < this.Vectors.Length; i++)
            {
                this.Indices[i] = Convert.ToUInt32(i);
            }
        }
        public void SetDefaultColors()
        {
            this.Colors = new Vector3[this.Vectors.Length];

            for (int i = 0; i < this.Vectors.Length; i++)
            {
                this.Colors[i] = new Vector3(0.5f, 1f, 0.5f);
            }
        }
        /// <summary>
        /// x,y and z are the angles in degrees
        /// </summary>
        /// <param name="pointCloud"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void RotateDegrees(float x, float y, float z)
        {
            Matrix3 R = new Matrix3();
            
            R = R.RotationXYZDegrees(x, y, z);
            this.Rotate(R);

        }
        public void Rotate(Matrix3 R)
        {
            for (int i = 0; i < this.Vectors.Length; i++)
            {
                Vector3 v = Vectors[i];
                Vectors[i] = R.MultiplyVector(v);
                
            }
        }
        public void Scale(float scale)
        {
            Vector3 scaleVector = new Vector3(scale, scale, scale);

            Matrix3 R = Matrix3.Identity;
            R[0, 0] = scaleVector[0];
            R[1, 1] = scaleVector[1];
            R[2, 2] = scaleVector[2];

            Rotate(R);

        }
        public void Translate(float x, float y, float z)
        {
            Vector3 translation = new Vector3(x, y, z);
            for (int i = 0; i < this.Vectors.Length; i++)
            {

                Vector3 v = this.Vectors[i];
                Vector3 translatedV = Vector3.Add(v, translation);
                v = translatedV;
                this.Vectors[i] = v;
            }


        }
        public void Sort_DistanceToCenter()
        {

            List<KeyValuePair<Vector3, Vector3>> listNew = new List<KeyValuePair<Vector3, Vector3>>();

            for(int i = 0; i < this.Vectors.Length; i++)
            {
                KeyValuePair<Vector3, Vector3> k = new KeyValuePair<Vector3, Vector3>(this.Vectors[i], this.Colors[i]);
                listNew.Add(k);

            }

            listNew.Sort(new DistanceComparer());

            for (int i = 0; i < listNew.Count; i++)
            {
                KeyValuePair<Vector3, Vector3> k = listNew[i];
                this.Vectors[i] = k.Key;
                this.Colors[i] = k.Value;


            }


        }

    
        //extract the custom made calibration object - a model consisting of three axes
        public PointCloud ExtractCalibrationObject()
        {
            PointCloud pcResult = null;
            try
            {
                float rLow = 0.5f;
                float rHigh = 0.85f;

                float gLow = 1f;
                float gHigh = 0.5f;

                float bLow = 1f;
                float bHigh = 0.7f;

                List<Vector3> colorList = new List<Vector3>();
                List<Vector3> vectorList = new List<Vector3>();

                for (int i = 0; i < this.Colors.Length; i++)
                {
                    if (this.Colors[i].X > rHigh && this.Colors[i].Y < gLow && this.Colors[i].Z < bLow)
                    {
                        colorList.Add(new Vector3(1f,0f,0f));
                        vectorList.Add(this.Vectors[i]);
                    }
                    else if (this.Colors[i].X < rLow && this.Colors[i].Y < gLow && this.Colors[i].Z > bHigh)
                    {
                        colorList.Add(new Vector3(0f, 0f, 1f));
                        //colorList.Add(this.Colors[i]);
                        vectorList.Add(this.Vectors[i]);
                    }
                    else if (this.Colors[i].X < rLow && this.Colors[i].Y > gHigh && this.Colors[i].Z < bLow)
                    {
                        colorList.Add(new Vector3(0f,1f,0f));
                        //colorList.Add(this.Colors[i]);
                        vectorList.Add(this.Vectors[i]);
                    }
                    

                }

                List<uint> indicesList = new List<uint>();
                for (uint i = 0; i < colorList.Count; i++)
                    indicesList.Add(i);

                pcResult = new PointCloud(vectorList, colorList, null, indicesList, null, null);
            }
            catch(Exception err)
            {
                System.Diagnostics.Debug.WriteLine("Error in ExtractFace " + err.Message);
            }
            return pcResult;



        }

        //extract the custom made calibration object - a model consisting of three axes
        public PointCloud ExtractFace(Ellipse ellipseFace)
        {

            PointCloud pcResult = null;
            List<Vector3> newVectorList = new List<Vector3>();
            List<Vector3> newColorList = new List<Vector3>();

            try
            {

                //List<Vector3> boundingBox = ellipseFace.BoundingBox;
                BoundingBox bb = ellipseFace.PointCloud.BoundingBox;
                for (int i = 0; i < this.Vectors.Length; i++)
                {
                    Vector3 v = this.Vectors[i];
                    if (v.X > bb.Min.X && v.X < bb.Max.X && v.Y > bb.Min.Y && v.Y < bb.Max.Y)
                    //if (v.X > boundingBox[0].X && v.X < boundingBox[1].X && v.Y > boundingBox[0].Y && v.Y < boundingBox[1].Y)
                    {
                        newVectorList.Add(v);
                        newColorList.Add(this.Colors[i]);
                    }

                }
                

           
                List<uint> indicesList = new List<uint>();
                for (uint i = 0; i < newColorList.Count; i++)
                    indicesList.Add(i);

                pcResult = new PointCloud(newVectorList, newColorList, null, indicesList, null, null);
            }
            catch (Exception err)
            {
                System.Windows.Forms.MessageBox.Show("SW Error with face in ExtractFace: " + err.Message);
                System.Diagnostics.Debug.WriteLine("Error in ExtractFace " + err.Message);
            }
            return pcResult;


        }
        public void CreateIndicesFromTriangles(List<TriangleVectors> listTriangleVectors)
        {
            List<Triangle> listTriangles = new List<Triangle>();


            for (int i = 0; i < listTriangleVectors.Count; i++)
            {

                TriangleVectors tv = listTriangleVectors[i];
                Triangle t = new Triangle(tv.A.Index, tv.B.Index, tv.C.Index);

                listTriangles.Add(t);


            }



            this.Triangles = listTriangles;
            this.CreateIndicesFromTriangles();

        }
          
   
        public void CreateIndicesFromTriangles()
        {
            if (this.Triangles != null)
            {
                this.Indices = new uint[this.Triangles.Count * 3];
                int ind = 0;
                foreach (Triangle t in this.Triangles)
                {
                    this.Indices[ind++] = t.IndVertices[0];
                    this.Indices[ind++] = t.IndVertices[1];
                    this.Indices[ind++] = t.IndVertices[2];

                }
            }

            ////in case there are more vectors than indices
            //if (this.Indices.Length < this.Vectors.Length)
            //{
            //    for (int i = this.Indices.Length; i < this.Vectors.Length; i++)
            //    {
            //        this.Indices[i] = Convert.ToUInt32(i);

            //    }
            //}
        }
        public static PointCloud FromTriangleVectors(List<TriangleVectors> listTriangleVectors)
        {
            List<Triangle> listTriangles = new List<Triangle>();
            List<Vector3> newVectors = new List<Vector3>();

            for (int i = 0; i < listTriangleVectors.Count; i++)
            {
                Triangle t = new Triangle(newVectors.Count, newVectors.Count + 1, newVectors.Count + 2);
                TriangleVectors tv = listTriangleVectors[i];

                listTriangles.Add(t);
                //if(!newVectors.Contains(tv.A))
                //    newVectors.Add(tv.A);
                //if (!newVectors.Contains(tv.B))
                //    newVectors.Add(tv.B);
                //if (!newVectors.Contains(tv.C))
                //    newVectors.Add(tv.C);

                newVectors.Add(tv.A.Vector);
                newVectors.Add(tv.B.Vector);
                newVectors.Add(tv.C.Vector);

            }


            PointCloud pcNew = PointCloud.FromListVector3(newVectors);
            pcNew.Triangles = listTriangles;
            pcNew.CreateIndicesFromTriangles();

            return pcNew;

        }
        public void CreateIndicesDefault()
        {
            if (this.Vectors != null)
            {
                this.Indices = new uint[this.Vectors.Length];
               
                for (uint i = 0; i < this.Vectors.Length; i++ )
                {

                    this.Indices[Convert.ToInt32(i)] = i;
                    
                }
            }
        }

        //public void CreateIndicesFromNewTriangles(List<OpenTKExtension.TriangleVectors> listTriangles)
        //{
        //    if (listTriangles != null)
        //    {
        //        this.Indices = new uint[listTriangles.Count * 3];
        //        int ind = 0;
        //        Triangles = new List<Triangle>();
        //        foreach (OpenTKExtension.TriangleVectors t in listTriangles)
        //        {

        //            this.Indices[ind++] = Convert.ToUInt32(t.A_Index);
        //            this.Indices[ind++] = Convert.ToUInt32(t.B_Index);
        //            this.Indices[ind++] = Convert.ToUInt32(t.C_Index);
        //            Triangles.Add(new Triangle(Convert.ToInt32(t.A_Index), Convert.ToInt32(t.B_Index), Convert.ToInt32(t.C_Index)));
        //        }
        //    }
        //}

     
        private List<List<VertexKDTree>> SortVectorsWithIndex()
        {
            List<List<VertexKDTree>> listNew = new List<List<VertexKDTree>>();

            List<VertexKDTree> listOld = new List<VertexKDTree>(this.VectorsWithIndex);

            List<VertexKDTree> column = new List<VertexKDTree>();
            bool positiveValues = false;
            //for (int i = listOld.Count - 1; i >= 0; i--)
            for (int i = 0; i < listOld.Count ; i++)
            {
                VertexKDTree v = listOld[i];

                if (v.Vector.Y < 0 && !positiveValues)
                    column.Add(v);
                else if(v.Vector.Y > 0 && !positiveValues)
                {
                    positiveValues = true;
                    column.Add(v);    
                }
                else if(v.Vector.Y > 0)
                {
                    column.Add(v);
                }
                else if (v.Vector.Y < 0 && positiveValues)
                {
                    positiveValues = false;
                    listNew.Add(column);
                    column = new List<VertexKDTree>();

                }
                

                

            }
            return listNew;

        }
        public List<VertexKDTree> VectorsWithIndex
        {
            get
            {
                if(vectorsWithIndex == null)
                {
                    vectorsWithIndex = new List<VertexKDTree>();
                    if(this.Indices == null || this.Indices.Length == 0)
                    {
                        this.Indices = new uint[this.Vectors.Length];
                        for(int i = 0; i < this.Indices.Length; i++)
                        {
                            this.Indices[i] = Convert.ToUInt32(i);

                        }
                    }
                    for(int i = 0; i < this.Vectors.Length; i++)
                    {
                        vectorsWithIndex.Add(new VertexKDTree(this.Vectors[i], Convert.ToInt32(this.Indices[i])));
                    }
                    
                }
                return vectorsWithIndex;
            }

        }
        public PointCloud PCAAxesNormalized
        {
            get
            {
                PointCloud axesNormalized = new PointCloud();
                {
                    for (int i = 0; i < 3; i++)
                    {
                        axesNormalized.AddVector(new Vector3(PCAAxes.Vectors[i].NormalizeV()));
                    }
                }
                return axesNormalized;
            }

        }
        public string PrintVectors()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < this.Count; i++)
            {
                Vector3 v = this.Vectors[i];
                sb.Append(v.ToString());

            }
            return sb.ToString();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (this.Vectors != null)
            {
                sb.Append(this.Vectors.Length.ToString());
                if(this.Vectors.Length > 2)
                {
                    sb.Append(this.Vectors[0].ToString() + " : ");
                    sb.Append(this.Vectors[1].ToString() + " : ");
                    sb.Append(this.Vectors[2].ToString() + " : ");
                }

                return sb.ToString();
            }
            return string.Empty;
        }
     
        public void RemovePoints(List<int> listPoints)
        {
            List<Vector3> listv = new List<Vector3>(this.Vectors);
            List<Vector3> listc = new List<Vector3>(this.Vectors);

            for (int i = listPoints.Count; i >= 0; i--)
            {
                listv.RemoveAt(listPoints[i]);
                listc.RemoveAt(listPoints[i]);

            }

          
            this.Vectors = listv.ToArray();
            this.Colors = listc.ToArray();
            SetDefaultIndices();

        }
     
        public static void ToJsonFile(PointCloud pc, string fileName)
        {
            JsonUtils.Serialize(pc.Vectors, fileName);
        }
        public static PointCloud FromJsonFile(string fileName)
        {

            List<Vector3> list = JsonUtils.DeserializeVectors(fileName);
            PointCloud pc = new PointCloud(list, null, null, null, null, null);
            return pc;
            
        }

        public void InitCubeUVs()
        {
            TextureUVs = new Vector2[] {
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1),
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1),
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1),
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1),
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1),
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1) };
        }
        public void InitUVsFromVectors()
        {
            List<Vector2> uvList = new List<Vector2>();
            for (int i = 0; i < this.Vectors.Length; i++ )
            {
                Vector2 v = new Vector2(this.Vectors[i].X, this.Vectors[i].Y);
                uvList.Add(v);
            }

            this.TextureUVs = uvList.ToArray();

        }

        public bool TextureCreateFromColors()
        {
            int xSize = Convert.ToInt32(Math.Sqrt(this.Vectors.Length));

            return TextureCreateFromColors(xSize, xSize);

        }
        public bool TextureCreateFromColors(int width, int height)
        {
            if (this.Colors == null || this.Colors.Length != this.Vectors.Length)
                return false;


            Bitmap bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            System.Drawing.Image bitmapCustom = bmp.UpdateFromPointCloud_Color(this, width, height);
            if(this.Path == null)
            {
                this.Path = GLSettings.Path;
            }
            //bitmapCustom.SaveImage(this.Path + "\\Models", "temp_", true);

            this.Texture = new Texture((Bitmap)bitmapCustom, false);


            //create also UVs

            float xMin = this.BoundingBox.Min.X;
            float yMin = this.BoundingBox.Min.Y;


            float xDiff = this.BoundingBox.Max.X - xMin;
            float yDiff = this.BoundingBox.Max.Y - yMin;

            this.TextureUVs = new Vector2[this.Vectors.Length];
            for (int i = 0; i < this.Vectors.Length; i++)
            {
                //this.UVs[i] = new Vector2( ((this.Vectors[i].X - xMin)*width ) / xDiff, (this.Vectors[i].Y - yMin) * height/yDiff);
                this.TextureUVs[i] = new Vector2(((this.Vectors[i].X - xMin) ) / xDiff, (this.Vectors[i].Y - yMin) / yDiff);

            }
              

            return true;

        }
       
    }
}
