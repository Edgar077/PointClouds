using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Windows.Media;
using System.Diagnostics;
using OpenTK;


namespace OpenTKExtension
{
    public class KDNode
    {
        public VertexKDTree Leaf;
        public KDNode NodeLeft;
        public KDNode NodeRight;
        public bool Taken;
        //public int IndexTaken = -1;

        public Vector3 BoundMin;
        public Vector3 BoundMax;
        public KDNode(VertexKDTree v)
        {
            Leaf = v;
        }
        public KDNode(VertexKDTree v, List<VertexKDTree> myListLeft, List<VertexKDTree> myListRight)
        {
            Leaf = v;
            NodeLeft = KDTree.BuildNode(myListLeft);
            NodeRight = KDTree.BuildNode(myListRight);
            //ListLeft = myListLeft;
            //ListRight= myListRight;

        }
        private void CalculateBounds()
        {
            // If we don't have bounds, create them using the new point then bail.
            BoundMin = new Vector3(Leaf.Vector);
            BoundMax = new Vector3(Leaf.Vector);

           
            // For each dimension.
            for (int i = 0; i < 3; i++)
            {
                
                //if (minBound[i] > tPoint[i])
                //{
                //    minBound[i] = tPoint[i];
                //    IsSinglePoint = false;
                //}
                //else if (maxBound[i] < tPoint[i])
                //{
                //    maxBound[i] = tPoint[i];
                //    IsSinglePoint = false;
                //}
            }
        }
        public override string ToString()
        {
            return Leaf.Vector.ToString();
        }
       
    }
    public class KDTree
    {
        KDNode RootNode;
        public void BuildRootNode(List<VertexKDTree> list)
        {
            list.Sort(new VectorWithIndex_XYZ());
            RootNode = BuildNode(list);
        }
        public KDTree(PointCloud pcl)
        {
            // Select axis based on depth so that axis cycles through all valid values

            List<VertexKDTree> listVectorWithIndex = pcl.VectorsWithIndex;
            BuildRootNode(listVectorWithIndex);
            

        }
        public static KDNode BuildNode(List<VertexKDTree> list)
        {

            if (list.Count <= 1)
            {
                if (list.Count == 0)
                    return null;
                else
                    return new KDNode(list[0]);

            }

            
            //check length even or odd
            int medianIndex = list.Count / 2;
            if(list.Count % 2 == 0)
            {
                medianIndex = list.Count / 2;
            }
            else
            {
                medianIndex = list.Count / 2 ;
            }
           

            
            VertexKDTree leaf = list[medianIndex];

            List<VertexKDTree> listLeft = new List<VertexKDTree>();
            listLeft.AddRange(list);
            listLeft.RemoveRange(medianIndex, list.Count - medianIndex);
//            if(listLeft.Count > 0)
                

            List<VertexKDTree> listRight = new List<VertexKDTree>();
            listRight.AddRange(list);
            listRight.RemoveRange(0, medianIndex + 1);

            return new KDNode(leaf, listLeft, listRight);


        }
        public bool SearchNode(VertexKDTree v, KDNode node, float bestDistance)
        {
          //  float bestDistance = float.MaxValue;

            float dist = node.Leaf.Vector.Distance(v.Vector) ;
            if (dist < bestDistance && !node.Taken)
            {
                bestDistance = dist;
                node.Taken = true;
                v.Index = node.Leaf.Index;
                
            }

            if (node.NodeLeft != null)
            {
                //float dist = node.Leaf.Vector.Distance(v.Vector);
                if (dist < bestDistance && !node.Taken)
                {
                    bestDistance = dist;
                    node.Taken = true;
                    v.Index = node.Leaf.Index;

                }
            }
            return true;

        }
        public void Search(PointCloud pcl)
        {
            
            List<VertexKDTree> list = new List<VertexKDTree>(pcl.VectorsWithIndex);
            for(int i = 0; i < list.Count ; i++)
            {
                VertexKDTree v = list[i];
                //if(v.Vector.Distance())
                //for(int j = 0; j < this.RootNode.)

            }

        }
    
    }
}
