using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//from https://bitbucket.org/nguyenivan/kinect2bvh.v2/src/d19ccd4e7631?at=master

namespace PointCloudUtils
{

    public class BVHBone
    {
        BVHBone parent;
        List<BVHBone> children;
        string name;
        int depth;
        static int index = 1;
        BVHChannel[] channels;
        public double[] rotOffset = new double[] { 0, 0, 0 };
        public double[] translOffset = new double[] { 0, 0, 0 };
        bool end;
        bool root;
        int motionSpace; // gibt erstes Element in der Motionspalte an
        TransAxis axis;
        bool isKinectJoint;


        public List<BVHBone> Children
        {
            get { return children; }
            set { children = value; }
        }

        public bool IsKinectJoint
        {
            get { return isKinectJoint; }
            set { isKinectJoint = value; }
        }

        public bool Root
        {
            get { return root; }
            set { root = value; }
        }

        public bool End
        {
            get { return end; }
            set { end = value; }
        }

        public TransAxis Axis
        {
            get { return axis; }
            set { axis = value; }
        }

        public int MotionSpace
        {
            get { return motionSpace; }
            set { motionSpace = value; }
        }

        public int Depth
        {
            get { return depth; }
        }
        public int ChannelCount
        {
            get { return channels.Length; }
        }
        public string Name
        {
            get { return name; }
        }
        public BVHBone Parent
        {
            get { return parent; }
        }
        public BVHChannel[] Channels
        {
            get { return channels; }
        }

        public BVHBone(BVHBone Parent, string Name, int nrChannels, TransAxis Axis, bool IsKinectJoint)
        {
            parent = Parent;
            index += index;
            name = Name;
            isKinectJoint = IsKinectJoint;
            axis = Axis;
            if (parent != null)
                depth = parent.Depth + 1;
            else
            {
                depth = 0;
                root = true;
            }
            channels = new BVHChannel[nrChannels];
            int ind = 5;
            for (int k = nrChannels - 1; k >= 0; k--)
            {
                channels[k] = (BVHChannel)ind;
                ind--;
            }
        }

        public void setTransOffset(double xOff, double yOff, double zOff)
        {
            translOffset = new double[] { xOff, yOff, zOff };
        }

        public void setRotOffset(double xOff, double yOff, double zOff)
        {
            rotOffset = new double[] { xOff, yOff, zOff };
        }
    }

}
