using System.Collections.Generic;
using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using Microsoft.Kinect;
using OpenTKExtension;
using Newtonsoft.Json;

namespace PointCloudUtils
{

    public class Skeleton : RenderableObject
    {

        List<Line> lines;
        Dictionary<JointType, Vector3> joints;


        public static Dictionary<ModelExchangeSizes, float> ModelExchangeDictionary;//= new KeyValuePair<ModelExchangeSizes, float>();

        public Skeleton()
        {
            if (ModelExchangeDictionary == null)
            {
                ModelExchangeDictionary = new Dictionary<ModelExchangeSizes, float>();
                for (int i = 0; i < Enum.GetValues(typeof(ModelExchangeSizes)).Length; i++)
                {
                    string modelName = Enum.GetValues(typeof(ModelExchangeSizes)).GetValue(i).ToString();
                    ModelExchangeSizes men = (ModelExchangeSizes)Enum.Parse(typeof(ModelExchangeSizes), modelName, true);
                    ModelExchangeDictionary[men] = 0f;
                }



            }
        }
        public Skeleton(List<Line> myLines) : this()
        {
            lines = myLines;
        }
        public Skeleton(Dictionary<JointType, Vector3> myjoints) : this()
        {
            joints = myjoints;
            CreateLinesFromJoints();
            updateModelExchangeSizes();
        }
        private void updateModelExchangeSizes()
        {
            ModelExchangeDictionary[ModelExchangeSizes.buttock_height_Z] = this.buttock_height_Z;
            ModelExchangeDictionary[ModelExchangeSizes.feet_height_Z] = this.feet_height_Z;
            ModelExchangeDictionary[ModelExchangeSizes.head_height_Z] = this.head_height_Z;
            ModelExchangeDictionary[ModelExchangeSizes.lowerleg_length] = this.lowerleg_length;
            ModelExchangeDictionary[ModelExchangeSizes.neck_height_Z] = this.neck_height_Z;
            ModelExchangeDictionary[ModelExchangeSizes.torso_height_Z] = this.torso_height_Z;
            ModelExchangeDictionary[ModelExchangeSizes.upperleg_length] = this.upperleg_length;
            ModelExchangeDictionary[ModelExchangeSizes.chest_width_X] = this.chest_width_X;
            ModelExchangeDictionary[ModelExchangeSizes.hands_length] = this.hands_length;
            ModelExchangeDictionary[ModelExchangeSizes.upperarm_length] = this.upperarm_length;
            ModelExchangeDictionary[ModelExchangeSizes.forearm_length] = this.forearm_length;


            
        }
        private void CreateLinesFromJoints()
        {
            lines = new List<Line>();

            float fupperHeadY = Math.Abs(joints[JointType.Head].Y - joints[JointType.Neck].Y);
            Vector3 upperHead = new Vector3(joints[JointType.Head].X, joints[JointType.Head].Y + fupperHeadY, joints[JointType.Head].Z);

            lines.Add(new Line(upperHead, joints[JointType.Head]));

            lines.Add(new Line(joints[JointType.Head], joints[JointType.Neck]));
            lines.Add(new Line(joints[JointType.Neck], joints[JointType.SpineShoulder]));
            lines.Add(new Line(joints[JointType.SpineShoulder], joints[JointType.SpineMid]));
            lines.Add(new Line(joints[JointType.SpineMid], joints[JointType.SpineBase]));
            lines.Add(new Line(joints[JointType.SpineShoulder], joints[JointType.ShoulderRight]));
            lines.Add(new Line(joints[JointType.SpineShoulder], joints[JointType.ShoulderLeft]));
            lines.Add(new Line(joints[JointType.SpineBase], joints[JointType.HipRight]));

            lines.Add(new Line(joints[JointType.SpineBase], joints[JointType.HipLeft]));
            lines.Add(new Line(joints[JointType.ShoulderRight], joints[JointType.ElbowRight]));
            lines.Add(new Line(joints[JointType.ElbowRight], joints[JointType.WristRight]));
            lines.Add(new Line(joints[JointType.WristRight], joints[JointType.HandRight]));
            lines.Add(new Line(joints[JointType.HandRight], joints[JointType.HandTipRight]));
            lines.Add(new Line(joints[JointType.WristRight], joints[JointType.ThumbRight]));
            lines.Add(new Line(joints[JointType.ShoulderLeft], joints[JointType.ElbowLeft]));

            lines.Add(new Line(joints[JointType.ElbowLeft], joints[JointType.WristLeft]));
            lines.Add(new Line(joints[JointType.WristLeft], joints[JointType.HandLeft]));
            lines.Add(new Line(joints[JointType.HandLeft], joints[JointType.HandTipLeft]));
            lines.Add(new Line(joints[JointType.WristLeft], joints[JointType.ThumbLeft]));
            lines.Add(new Line(joints[JointType.HipRight], joints[JointType.KneeRight]));
            lines.Add(new Line(joints[JointType.KneeRight], joints[JointType.AnkleRight]));

            lines.Add(new Line(joints[JointType.AnkleRight], joints[JointType.FootRight]));
            lines.Add(new Line(joints[JointType.HipLeft], joints[JointType.KneeLeft]));
            lines.Add(new Line(joints[JointType.KneeLeft], joints[JointType.AnkleLeft]));
            lines.Add(new Line(joints[JointType.AnkleLeft], joints[JointType.FootLeft]));

            this.PointCloud.Name = "Skeleton";
            FillPointCloud();
            FillIndexBuffer();
        }



        public override void InitializeGL()
        {
            this.primitiveType = PrimitiveType.Lines;

            if (initialized)
                this.Dispose();

            initialized = true;


            if (InitShaders("PointCloud.vert", "PointCloud.frag", path + "Shaders\\"))
            {
                this.initBuffers();
                this.FillPointCloud();
                FillIndexBuffer();
                //this.RefreshRenderableData();
            }

        }

        public override void Dispose()
        {

            base.Dispose();
        }



        public override void FillPointCloud()
        {
            this.PointCloud.Vectors = new Vector3[lines.Count * 2];

            int vectIndex = -1;
            for (int i = 0; i < lines.Count; i++)
            {
                vectIndex++;
                this.PointCloud.Vectors[vectIndex] = lines[i].PStart;
                vectIndex++;
                this.PointCloud.Vectors[vectIndex] = lines[i].PEnd;

            }

            this.PointCloud.Colors = new Vector3[this.PointCloud.Vectors.Length];
            for (int i = 0; i < this.PointCloud.Vectors.Length; i++)
            {
                this.PointCloud.Colors[i] = new Vector3(0.0f, 1.0f, 1.0f);

            }


        }

        public override void FillIndexBuffer()
        {
            this.PointCloud.Indices = new uint[this.PointCloud.Vectors.Length];

            for (int i = 0; i < this.PointCloud.Vectors.Length; i++)
            {
                this.PointCloud.Indices[i] = Convert.ToUInt32(i);

            }



        }

        public static Dictionary<string, float[]> JointsToSerializable(Dictionary<JointType, Vector3> joints)
        {
            //Dictionary<string, float[]> jointVectorsToSerialize = new Dictionary<string, float[]>();
            Dictionary<string, float[]> jointVectorsToSerialize = new Dictionary<string, float[]>();

            foreach (var l in joints)
            {
                jointVectorsToSerialize.Add(l.Key.ToString(), new float[] { l.Value.X, l.Value.Y, l.Value.Z });

            }
            return jointVectorsToSerialize;

        }
        public static Dictionary<JointType, Vector3> SerializedToJoints(Dictionary<string, float[]> jointVectorsDeserialize)
        {

            Dictionary<JointType, Vector3> joints = new Dictionary<JointType, Vector3>();
            foreach (var l in jointVectorsDeserialize)
            {

                JointType j = (JointType)Enum.Parse(typeof(JointType), l.Key, true);
                joints.Add(j, new Vector3(l.Value[0], l.Value[1], l.Value[2]));

            }
            return joints;
        }
        public void ToJsonFile(string fileNameShort)
        {

            string pathModels = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Models";
            string fileName = pathModels + "\\" + fileNameShort + ".json";
            Dictionary<string, float[]> jointVectorsToSerialize = JointsToSerializable(joints);


            System.IO.File.WriteAllText(fileName, JsonConvert.SerializeObject(jointVectorsToSerialize));
        }

        public static Skeleton FromJsonFile(string path, string fileNameShort)
        {
            string fileName = path + "\\" + fileNameShort;

            Dictionary<string, float[]> jointVectorsDeserialize = JsonConvert.DeserializeObject<Dictionary<string, float[]>>(System.IO.File.ReadAllText(fileName));
            Dictionary<JointType, Vector3> newjoints = Skeleton.SerializedToJoints(jointVectorsDeserialize);

            Skeleton sk = new Skeleton(newjoints);

            return sk;


        }

        public float Height
        {

            get
            {
                float leg = feet_height_Z + lowerleg_length + upperleg_length + buttock_height_Z;


                float height = leg + torso_height_Z + neck_height_Z + head_height_Z;
                return height;
            }
        }

        //public float Height_Right
        //{

        //    get
        //    {

        //        float f = joints[JointType.Neck].Y - joints[JointType.FootRight].Y + Head_Y;
        //        return f;
        //    }
        //}
        //public float Height_Left
        //{

        //    get
        //    {

        //        float f = joints[JointType.Neck].Y - joints[JointType.FootLeft].Y + Head_Y;
        //        return f;
        //    }
        //}


        public float feet_height_Z
        {

            get
            {

                float f = (joints[JointType.AnkleLeft].Y - joints[JointType.FootLeft].Y) +
                   (joints[JointType.AnkleRight].Y - joints[JointType.FootRight].Y);
                f /= 2;
                ModelExchangeDictionary[ModelExchangeSizes.feet_height_Z] = f;
                return f;
            }
        }


        public float lowerleg_length
        {

            get
            {
                float f = (joints[JointType.KneeLeft].Distance(joints[JointType.FootLeft])) +
                    (joints[JointType.KneeRight].Distance(joints[JointType.FootRight]));
                f /= 2;
                ModelExchangeDictionary[ModelExchangeSizes.lowerleg_length] = f;
                return f;



            }
        }

        public float upperleg_length
        {

            get
            {
                float f = (joints[JointType.HipRight].Distance(joints[JointType.KneeRight])) +
                    (joints[JointType.HipLeft].Distance(joints[JointType.KneeLeft]));
                f /= 2;
                ModelExchangeDictionary[ModelExchangeSizes.upperleg_length] = f;
                return f;

            }
        }

        public float buttock_height_Z
        {

            get
            {
                float f = (joints[JointType.SpineBase].Y - joints[JointType.HipLeft].Y);
                f += (joints[JointType.SpineBase].Y - joints[JointType.HipRight].Y);
                f /= 2;

                ModelExchangeDictionary[ModelExchangeSizes.buttock_height_Z] = f;
                return f;
            }
        }

        public float torso_height_Z
        {

            get
            {
                float f = joints[JointType.SpineBase].Distance(joints[JointType.SpineMid]) +
                    joints[JointType.SpineMid].Distance(joints[JointType.SpineShoulder]);

                ModelExchangeDictionary[ModelExchangeSizes.torso_height_Z] = f;
                return f;


            }
        }
        public float neck_height_Z
        {

            get
            {
                float f = joints[JointType.Neck].Distance(joints[JointType.SpineShoulder]);
                ModelExchangeDictionary[ModelExchangeSizes.neck_height_Z] = f;
                return f;



            }
        }
        public float head_height_Z
        {

            get
            {
                float f = 2 * (joints[JointType.Head].Y - joints[JointType.Neck].Y);
                ModelExchangeDictionary[ModelExchangeSizes.head_height_Z] = f;
                return f;

            }
        }
   

        public float chest_width_X
        {

            get
            {
                float f = joints[JointType.ShoulderRight].Distance(joints[JointType.ShoulderLeft]);
                ModelExchangeDictionary[ModelExchangeSizes.chest_width_X] = f;
                return f;

            }
        }
        public float hands_length
        {

            get
            {
                float f = joints[JointType.WristRight].Distance(joints[JointType.HandRight]) +
                    joints[JointType.HandRight].Distance(joints[JointType.HandTipRight]);
                ModelExchangeDictionary[ModelExchangeSizes.hands_length] = f;
                return f;

            }
        }
        public float upperarm_length
        {

            get
            {
                float f = joints[JointType.ShoulderLeft].Distance(joints[JointType.ElbowLeft]);
                ModelExchangeDictionary[ModelExchangeSizes.upperarm_length] = f;
                return f;

            }
        }
        public float forearm_length
        {

            get
            {
                float f = joints[JointType.WristRight].Distance(joints[JointType.ElbowRight]);
                ModelExchangeDictionary[ModelExchangeSizes.forearm_length] = f;
                return f;

            }
        }
        private float head_width_X_private;
        
        public float head_width_X
        {
            get
            {
                
                
                return head_width_X_private;
            }
            set
            {
                ModelExchangeDictionary[ModelExchangeSizes.head_width_X] = value;
                head_width_X_private = value;
            }


        }

      
    }
    public enum ModelExchangeSizes
    {
        feet_height_Z,
        lowerleg_length,
        upperleg_length,
        buttock_height_Z,
        torso_height_Z,
        neck_height_Z,
        head_height_Z,
        upperarm_length,
        forearm_length,
        chest_width_X,
        hands_length,
        head_width_X

    }
}