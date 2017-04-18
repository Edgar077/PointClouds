using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
//from https://bitbucket.org/nguyenivan/kinect2bvh.v2/src/d19ccd4e7631?at=master

namespace PointCloudUtils
{
  
    public class BVHSkeleton
    {
        List<BVHBone> bones;
        int maxDepth = 0;
        int nrBones;
        int channels;

        public List<BVHBone> Bones
        {
            get { return bones; }
        }

        public int Channels
        {
            get { return channels; }
        }

        public BVHSkeleton()
        {
            bones = new List<BVHBone>();
        }

        public void AddBone(BVHBone Bone)
        {
            if (!Bones.Contains(Bone))
            {
                bones.Add(Bone);
            }
        }

        public void FinalizeBVHSkeleton()
        {
            for (int k = 0; k < Bones.Count(); k++)
            {
                // set max Depth
                if (Bones[k].Depth > maxDepth)
                    maxDepth = Bones[k].Depth;

                //set Bone Index for Motion Values Array
                int motionCount = 0;
                for (int n = 0; n < k; n++)
                {
                    motionCount += Bones[n].ChannelCount;
                }
                Bones[k].MotionSpace = motionCount;

                //set Count of Channels for Skeleton
                channels += Bones[k].ChannelCount;

                //set Children
                List<BVHBone> childBoneList = Bones.FindAll(i => i.Parent == Bones[k]);
                if (childBoneList.Count == 0)
                {
                    Bones[k].End = true;
                }
                else
                {
                    Bones[k].Children = childBoneList;

                }
            }
        }

        public void copyParameters(BVHSkeleton input)
        {
            channels = input.Channels;
            maxDepth = input.maxDepth;
            nrBones = input.nrBones;
        }

        public int getMaxDepth()
        {
            return maxDepth;
        }
    }


    public enum BVHChannel
    {
        Xposition,
        Yposition,
        Zposition,
        Xrotation,
        Yrotation,
        Zrotation
    }

    public enum TransAxis
    {
        None,
        X,
        Y,
        Z,
        nX,
        nY,
        nZ
    }


}
