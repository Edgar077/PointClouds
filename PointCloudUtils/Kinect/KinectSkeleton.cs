using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using OpenTK;
using OpenTKExtension;
using System.Drawing;
using Newtonsoft.Json;


namespace PointCloudUtils
{
    public class KinectSkeleton
    {
        public List<Line> BonesAsLines;
        /// <summary>
        /// definition of bones
        /// </summary>
        private List<Tuple<JointType, JointType>> bones;
        //----Skeleton
        /// <summary>
        /// Radius of drawn hand circles
        /// </summary>
        private const double HandSize = 30;

        /// <summary>
        /// Thickness of drawn joint lines
        /// </summary>
        private const double JointThickness = 3;

        /// <summary>
        /// Thickness of clip edge rectangles
        /// </summary>
        private const double ClipBoundsThickness = 10;


        Dictionary<JointType, Vector3> joints = new Dictionary<JointType, Vector3>();

    
        private const float InferredZPositionClamp = 0.1f;

        public KinectSkeleton()
        {
            InitBones();
            InitJoints();
        }
        public static double Length(Joint p1, Joint p2)
        {
            return Math.Sqrt(
                Math.Pow(p1.Position.X - p2.Position.X, 2) +
                Math.Pow(p1.Position.Y - p2.Position.Y, 2) +
                Math.Pow(p1.Position.Z - p2.Position.Z, 2));
        }
        public static double Length(params Joint[] joints)
        {
            double length = 0;

            for (int index = 0; index < joints.Length - 1; index++)
            {
                length += Length(joints[index], joints[index + 1]);
            }

            return length;
        }
        //public static int NumberOfTrackedJoints(params Joint[] joints)
        //{
        //    int trackedJoints = 0;

        //    foreach (var joint in joints)
        //    {
        //        if (joint.TrackingState == JointTrackingState.Tracked)
        //        {
        //            trackedJoints++;
        //        }
        //    }

        //    return trackedJoints;
        //}
    
        /// <summary>
        /// Draws a body
        /// </summary>
        /// <param name="joints">joints to draw</param>
        /// <param name="jointPoints">translated positions of joints to draw</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        /// <param name="drawingPen">specifies color to draw a specific body</param>
        private List<Line> SetBodyLines(IReadOnlyDictionary<JointType, Joint> joints, IDictionary<JointType, Vector3> jointsToSerialize)
        {
            List<Line> jointLines = new List<Line>();
            // Draw the bones
            foreach (var bone in this.bones)
            {
                Line l = this.SetBoneLine(joints, jointsToSerialize, bone.Item1, bone.Item2);
                if (l != null)
                    jointLines.Add(l);
            }
            return jointLines;
        }
        /// <summary>
        /// Draws a body
        /// </summary>
        /// <param name="joints">joints to draw</param>
        /// <param name="jointPoints">translated positions of joints to draw</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        /// <param name="drawingPen">specifies color to draw a specific body</param>
        private void DrawBody(IReadOnlyDictionary<JointType, Joint> joints, IDictionary<JointType, Point> jointPoints, Pen drawingPen)
        {
            // Draw the bones
            foreach (var bone in this.bones)
            {
                this.DrawBone(joints, jointPoints, bone.Item1, bone.Item2, drawingPen);

            }

            // Draw the joints
            foreach (JointType jointType in joints.Keys)
            {
                //Brush drawBrush = null;

                TrackingState trackingState = joints[jointType].TrackingState;

                //if (trackingState == TrackingState.Tracked)
                //{
                //    drawBrush = this.trackedJointBrush;
                //}
                //else if (trackingState == TrackingState.Inferred)
                //{
                //    drawBrush = this.inferredJointBrush;
                //}

                //if (drawBrush != null)
                //{
                //    drawingContext.DrawEllipse(drawBrush, null, jointPoints[jointType], JointThickness, JointThickness);
                //}
            }
        }

        /// <summary>
        /// Draws one bone of a body (joint to joint)
        /// </summary>
        /// <param name="joints">joints to draw</param>
        /// <param name="jointPoints">translated positions of joints to draw</param>
        /// <param name="jointType0">first joint of bone to draw</param>
        /// <param name="jointType1">second joint of bone to draw</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        /// /// <param name="drawingPen">specifies color to draw a specific bone</param>
        private Line SetBoneLine(IReadOnlyDictionary<JointType, Joint> joints, IDictionary<JointType, Vector3> jointsToSerialize, JointType jointType0, JointType jointType1)
        {

            Joint joint0 = joints[jointType0];
            Joint joint1 = joints[jointType1];

            // If we can't find either of these joints, exit
            if (joint0.TrackingState == TrackingState.NotTracked || joint1.TrackingState == TrackingState.NotTracked)
            {
                return null;
            }


            if (jointsToSerialize[jointType0] != null && jointsToSerialize[jointType1] != null)
            {
                //System.Diagnostics.Debug.WriteLine("Bone between joints: " + jointType0.ToString() + " " + jointType1.ToString());
                return new Line(jointsToSerialize[jointType0], jointsToSerialize[jointType1]);
            }
            return null;

            
        }
        /// <summary>
        /// Draws one bone of a body (joint to joint)
        /// </summary>
        /// <param name="joints">joints to draw</param>
        /// <param name="jointPoints">translated positions of joints to draw</param>
        /// <param name="jointType0">first joint of bone to draw</param>
        /// <param name="jointType1">second joint of bone to draw</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        /// /// <param name="drawingPen">specifies color to draw a specific bone</param>
        private void DrawBone(IReadOnlyDictionary<JointType, Joint> joints, IDictionary<JointType, Point> jointPoints, JointType jointType0, JointType jointType1, Pen drawingPen)
        {
            Joint joint0 = joints[jointType0];
            Joint joint1 = joints[jointType1];

            // If we can't find either of these joints, exit
            if (joint0.TrackingState == TrackingState.NotTracked ||
                joint1.TrackingState == TrackingState.NotTracked)
            {
                return;
            }

            // We assume all drawn bones are inferred unless BOTH joints are tracked
            //Pen drawPen = this.inferredBonePen;
            //if ((joint0.TrackingState == TrackingState.Tracked) && (joint1.TrackingState == TrackingState.Tracked))
            //{
            //    drawPen = drawingPen;
            //}

            //drawingContext.DrawLine(drawPen, jointPoints[jointType0], jointPoints[jointType1]);
        }

        /// <summary>
        /// Draws a hand symbol if the hand is tracked: red circle = closed, green circle = opened; blue circle = lasso
        /// </summary>
        /// <param name="handState">state of the hand</param>
        /// <param name="handPosition">position of the hand</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        private void DrawHand(HandState handState, Point handPosition)
        {
            //switch (handState)
            //{
            //    case HandState.Closed:
            //        drawingContext.DrawEllipse(this.handClosedBrush, null, handPosition, HandSize, HandSize);
            //        break;

            //    case HandState.Open:
            //        drawingContext.DrawEllipse(this.handOpenBrush, null, handPosition, HandSize, HandSize);
            //        break;

            //    case HandState.Lasso:
            //        drawingContext.DrawEllipse(this.handLassoBrush, null, handPosition, HandSize, HandSize);
            //        break;
            //}
        }
        private void InitBones()
        {
            // a bone defined as a line between two joints
            this.bones = new List<Tuple<JointType, JointType>>();

            // Torso
            this.bones.Add(new Tuple<JointType, JointType>(JointType.Head, JointType.Neck));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.Neck, JointType.SpineShoulder));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineShoulder, JointType.SpineMid));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineMid, JointType.SpineBase));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineShoulder, JointType.ShoulderRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineShoulder, JointType.ShoulderLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineBase, JointType.HipRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineBase, JointType.HipLeft));

            // Right Arm
            this.bones.Add(new Tuple<JointType, JointType>(JointType.ShoulderRight, JointType.ElbowRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.ElbowRight, JointType.WristRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.WristRight, JointType.HandRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.HandRight, JointType.HandTipRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.WristRight, JointType.ThumbRight));

            // Left Arm
            this.bones.Add(new Tuple<JointType, JointType>(JointType.ShoulderLeft, JointType.ElbowLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.ElbowLeft, JointType.WristLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.WristLeft, JointType.HandLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.HandLeft, JointType.HandTipLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.WristLeft, JointType.ThumbLeft));

            // Right Leg
            this.bones.Add(new Tuple<JointType, JointType>(JointType.HipRight, JointType.KneeRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.KneeRight, JointType.AnkleRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.AnkleRight, JointType.FootRight));

            // Left Leg
            this.bones.Add(new Tuple<JointType, JointType>(JointType.HipLeft, JointType.KneeLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.KneeLeft, JointType.AnkleLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.AnkleLeft, JointType.FootLeft));

            //foreach (JointType jointType in joints.Keys)
            //{
            //    // sometimes the depth(Z) of an inferred joint may show as negative
            //    // clamp down to 0.1f to prevent coordinatemapper from returning (-Infinity, -Infinity)
            //    CameraSpacePoint position = joints[jointType].Position;
            //    if (position.Z < 0)
            //    {
            //        position.Z = InferredZPositionClamp;
            //    }

            //    //CameraSpacePoint[] myRealWorldPoints = new CameraSpacePoint[DepthMetaData.XDepthMaxKinect * DepthMetaData.YDepthMaxKinect];
            //    //
            //    //coordinateMapper.MapDepthFrameToCameraSpace(myDepthMetaData.FrameData, myRealWorldPoints);
            //    DepthSpacePoint depthSpacePoint = coordinateMapper.MapCameraPointToDepthSpace(position);
            //    if (depthSpacePoint.X < MetaDataBase.XDepthMaxKinect && depthSpacePoint.Y < MetaDataBase.XDepthMaxKinect)
            //    {
            //        //jointPoints[jointType] = new Point(Convert.ToInt32(depthSpacePoint.X), Convert.ToInt32(depthSpacePoint.Y));
            //        jointVectors[jointType] = new Vector3(position.X, position.Y, position.Z);
            //    }
            //    bonesAsLines = this.SetBodyLines(joints, jointVectors);
            //    float f = Height(body);
            //    System.Diagnostics.Debug.WriteLine("Height: " + f.ToString());

            //}
        }
        private void InitJoints()
        {
            for (int i = 0; i < Enum.GetValues(typeof(JointType)).GetLength(0); i++)
            {

               // JointType jointType = Enum.GetValues(typeof(JointType)).GetValue(i);
                string strVal = Enum.GetValues(typeof(JointType)).GetValue(i).ToString();
                JointType jointType = (JointType)Enum.GetValues(typeof(JointType)).GetValue(i);

                joints[jointType] = Vector3.Zero;
              
            }
        }
        public void Update(Body body, CoordinateMapper coordinateMapper)
        {
            //this.DrawClippedEdges(body, dc);

            IReadOnlyDictionary<JointType, Joint> jointsMS = body.Joints;

            // convert the joint points to depth (display) space
            Dictionary<JointType, Point> jointPoints = new Dictionary<JointType, Point>();
            

            foreach (JointType jointType in jointsMS.Keys)
            {
                // sometimes the depth(Z) of an inferred joint may show as negative
                // clamp down to 0.1f to prevent coordinatemapper from returning (-Infinity, -Infinity)
                CameraSpacePoint position = jointsMS[jointType].Position;
                if (position.Z < 0)
                {
                    position.Z = InferredZPositionClamp;
                }

                //CameraSpacePoint[] myRealWorldPoints = new CameraSpacePoint[DepthMetaData.XDepthMaxKinect * DepthMetaData.YDepthMaxKinect];
                //
                //coordinateMapper.MapDepthFrameToCameraSpace(myDepthMetaData.FrameData, myRealWorldPoints);
                DepthSpacePoint depthSpacePoint = coordinateMapper.MapCameraPointToDepthSpace(position);
                if (depthSpacePoint.X < MetaDataBase.XDepthMaxKinect && depthSpacePoint.Y < MetaDataBase.XDepthMaxKinect)
                {
                    //jointPoints[jointType] = new Point(Convert.ToInt32(depthSpacePoint.X), Convert.ToInt32(depthSpacePoint.Y));
                    joints[jointType] = new Vector3(position.X, position.Y, position.Z);
                }
                BonesAsLines = this.SetBodyLines(jointsMS, joints);
                //float f = Height(body);
                //System.Diagnostics.Debug.WriteLine("Height: " + f.ToString());

            }
        }
        private Vector3 average(Vector3 joint1, Vector3 joint2)
        {
            Vector3 avg_joint = new Vector3();
            avg_joint.X = (joint1.X + joint2.X) / 2f;
            avg_joint.Y = (joint1.Y + joint2.Y) / 2f;
            avg_joint.Z = (joint1.Z + joint2.Z) / 2f;
            
            return avg_joint;
        }
    
        public float Distance_Hips_X
        {
            get
            {
                return joints[JointType.HipLeft].Distance(joints[JointType.HipRight]);
            }

        }
     
        public float Distance_torso_Z
        {
            get
            {

                return joints[JointType.HipLeft].Distance(joints[JointType.SpineBase])
                     + joints[JointType.SpineMid].Distance(joints[JointType.SpineBase])
                + joints[JointType.SpineShoulder].Distance(joints[JointType.SpineMid])
                + joints[JointType.Neck].Distance(joints[JointType.SpineShoulder]) 
                    + joints[JointType.Head].Distance(joints[JointType.Neck]);

                //Distance_Hips;
                   
                   
            }
        }
        public float Height(Body body)
        {

            float leftLeg = joints[JointType.HipLeft].Distance(joints[JointType.KneeLeft]) +
                joints[JointType.KneeLeft].Distance(joints[JointType.AnkleLeft]) +
                joints[JointType.AnkleLeft].Distance(joints[JointType.FootLeft]);


            float rightLeg = joints[JointType.HipRight].Distance(joints[JointType.KneeRight]) +
                joints[JointType.KneeRight].Distance(joints[JointType.AnkleRight]) +
                joints[JointType.AnkleRight].Distance(joints[JointType.FootRight]);


            float f = Distance_torso_Z + (leftLeg + rightLeg) / 2;


            return f;
        }
        public void ToJsonFile(string fileNameShort)
        {

            Skeleton sk = new Skeleton(joints);
            sk.ToJsonFile(fileNameShort);
            
            //string pathModels = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Models";
            //string fileName = pathModels + "\\" + fileNameShort + ".json";


            ////Dictionary<string, float[]> jointVectorsToSerialize = new Dictionary<string, float[]>();
            //Dictionary<string, float[]> jointVectorsToSerialize = new Dictionary<string, float[]>();

            //foreach (var l in joints)
            //{
            //    jointVectorsToSerialize.Add(l.Key.ToString(), new float[] {l.Value.X, l.Value.Y, l.Value.Z});

            //}
       
           
            //System.IO.File.WriteAllText(fileName, JsonConvert.SerializeObject(jointVectorsToSerialize));

        }
      
    }
}
