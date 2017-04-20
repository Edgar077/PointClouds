using System;
using System.Collections.Generic;

using OpenTK;
using OpenTKExtension;

using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using PointCloudUtils;


namespace CharacterCreator
{
    public class FaceMatcher
    {
        //string pathModels;

        public float FaceHeight;
        public float FaceWidth;

        public PointCloud FaceNew;
        public PointCloud FaceCut;
        public Skeleton Skeleton;
        public Humanoid Humanoid;


        public FaceMatcher()
        {
            Skeleton = new Skeleton();
           
        }
        public void LoadFace(string path, string fileName)
        {
            
            FaceNew = PointCloud.FromObjFile(path, fileName);

        }
     
        /// <summary>
        /// skeleton is rotated around y axis
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        public void LoadSkeleton(string path, string fileName)
        {

            
            Skeleton = PointCloudUtils.Skeleton.FromJsonFile(path,  fileName);
           
            Skeleton.PointCloud.RotateDegrees(0, 180, 0);

            Skeleton.head_width_X = FaceWidth;

           

        }
    
        public void UpdateModel_Joints()
        {

            for (int i = 0; i < Enum.GetValues(typeof(ModelExchangeSizes)).Length; i++)
            {
                string modelName = Enum.GetValues(typeof(ModelExchangeSizes)).GetValue(i).ToString();
                ModelExchangeSizes men = (ModelExchangeSizes)Enum.Parse(typeof(ModelExchangeSizes), modelName, true);
                float modelValue = Skeleton.ModelExchangeDictionary[men];
                Humanoid.updateWishList(modelName, modelValue);

            }



            Humanoid.UpdateModel_WishedMeasures(true);
            //Humanoid.update_character(UpdateMode.update_directly_verts);
            
        }

        private void CheckMeasures()
        {

            float fTotal = 0f;
            float fTotalWish = 0f;

            foreach (string measure_name in this.Humanoid.body_height_Z_parts)
            {
                float f = 0;
                if (this.Humanoid.wished_measures.ContainsKey(measure_name))
                    f = this.Humanoid.wished_measures[measure_name];
                else
                    f = this.Humanoid.m_engine.measures[measure_name];

                fTotalWish += f;
                f = this.Humanoid.m_engine.measures[measure_name];

                System.Diagnostics.Debug.WriteLine(measure_name + " : " + f.ToString());
                fTotal += f;
            }
            System.Diagnostics.Debug.WriteLine("New height: " + fTotal.ToString() + " wish: " + fTotalWish.ToString());

        }
        public void CutFace()
        {
            Humanoid.CutFace();
            FaceCut = PointCloud.FromListVector3(Humanoid.FaceVectors);
            //Humanoid.RearrangePointCloud(FaceCut);


        }
        public void SaveCutFace(string fileName)
        {
            if(FaceCut != null)
            {
                JsonUtils.Serialize(FaceCut.Vectors, fileName);

            }


        }
        public void AdjustFaceToHumanoid()
        {

            
            if (FaceNew != null)
            {
                Humanoid.RearrangePointCloud(FaceCut);
                //FaceNew.RotateDegrees(0, 180, 0);
                
                FaceNew.CalculateBoundingBox();
                FaceCut.CalculateBoundingBox();

                Vector3 v = FaceCut.BoundingBoxMin - FaceNew.BoundingBoxMin;
                FaceNew.Translate(v.X, v.Y, v.Z);
                FaceNew.CalculateBoundingBox();
            }



        }
        public void AlignFaces()
        {
            ICPLib.IterativeClosestPointTransform icp = new ICPLib.IterativeClosestPointTransform();
            //icp.ICPSettings.ShuffleEffect = true;
            icp.ICPSettings.MaximumNumberOfIterations = 10;
            icp.ICPSettings.SingleSourceTargetMatching = true;

            icp.ICPSettings.ICPVersion = ICP_VersionUsed.NoScaling;
            icp.TakenAlgorithm = true;
            FaceNew = icp.PerformICP(FaceCut, FaceNew);
        }
        public void LoadHumanoid(int indexHumanoid)
        {
            if (Humanoid == null)
            {
                CharactersBO cc = new CharactersBO();
                Humanoid = new Humanoid(cc.Names[indexHumanoid]);
                Humanoid.init_database();
            }
        }

        public PointCloud MergeResultModelAndSave(string path, string fileName)
        {
            //adjust faceNew vectors to old Face
            //FaceNew.RotateDegrees(0, 180, 0);
            //AlignFaces();

           // Humanoid.RearrangePointCloud(FaceCut);



            Humanoid.RearrangeBack(FaceNew);
            

            float conv = Convert.ToSingle(FaceNew.Count) / this.Humanoid.FaceIndices.Count;
            for(int i = 0; i < this.Humanoid.FaceIndices.Count; i++)
            {
                int ind = Convert.ToInt32(conv * i);
                if (ind < FaceNew.Vectors.Length)
                    //this.Humanoid.Vectors[Convert.ToInt32(this.Humanoid.FaceIndices[i])] = FaceNew.Vectors[ind];
                    Humanoid.Vectors[Convert.ToInt32(this.Humanoid.FaceIndices[i])] = FaceNew.Vectors[ind];
                else
                {
                    System.Windows.Forms.MessageBox.Show("SW Error in MergeResultModelAndSave");
                }
            }

            PointCloud pc = Humanoid.ToPointCloud();

            //Humanoid.Vectors = new List<Vector3>(pc.Vectors);
            //pc.AddPointCloud(FaceNew);
            
            pc.ToObjFile(path, fileName);
            return pc;
        }
        public PointCloud MergeResultModelAndSaveOld(string path, string fileName)
        {
            //adjust faceNew vectors to old Face
            float conv = FaceNew.Count / this.Humanoid.FaceIndices.Count;
            PointCloud pc = Humanoid.ToPointCloud();

            for (int i = 0; i < this.Humanoid.FaceIndices.Count; i++)
            {
                int ind = Convert.ToInt32(conv * i);
                if (ind < FaceNew.Vectors.Length)
                    //this.Humanoid.Vectors[Convert.ToInt32(this.Humanoid.FaceIndices[i])] = FaceNew.Vectors[ind];
                    pc.Vectors[Convert.ToInt32(this.Humanoid.FaceIndices[i])] = FaceNew.Vectors[ind];
                else
                {
                    System.Windows.Forms.MessageBox.Show("SW Error in MergeResultModelAndSave");
                }
            }

            Humanoid.Vectors = new List<Vector3>(pc.Vectors);
            //pc.AddPointCloud(FaceNew);

            pc.ToObjFile(path, fileName);
            return pc;
        }
        public PointCloud MergeResultModelAndSave_FullFace(string path, string fileName)
        {
            PointCloud pc = Humanoid.ToPointCloud();
            pc.AddPointCloud(FaceNew);

            pc.ToObjFile(path, fileName);
            return pc;
        }
      
        public void Rotate_AdjustFaceDepth()
        {

            

            FaceNew.RotateDegrees(0, 180, 0);

            //float fDepthMax = FaceNew.BoundingBox.Max.Z - 0.06f;
            float fDepthMax = FaceNew.BoundingBox.Min.Z + 0.06f;


            List<Vector3> vNew = new List<Vector3>();
            List<Vector3> cNew = new List<Vector3>();


            for (int i = 0; i < FaceNew.Vectors.Length; i++)
            {
                //if (FaceNew.Vectors[i].Z <= fDepthMax)
                if (FaceNew.Vectors[i].Z >= fDepthMax)
                {
                    vNew.Add(FaceNew.Vectors[i]);
                    cNew.Add(FaceNew.Colors[i]);
                }
            }

            FaceNew.Vectors = vNew.ToArray();
            FaceNew.Colors = cNew.ToArray();
            FaceNew.SetDefaultIndices();

            FaceNew.CalculateBoundingBox();

            FaceNew.DisregardCenteredShowing = true;
            FaceHeight = FaceNew.BoundingBox.Max.Y - FaceNew.BoundingBox.Min.Y;
            FaceWidth = FaceNew.BoundingBox.Max.X - FaceNew.BoundingBox.Min.X;

        }
    }
}
