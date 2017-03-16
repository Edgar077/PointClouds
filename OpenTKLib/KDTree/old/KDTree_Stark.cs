//// KDTree.cs - A Stark, September 2009.

////	This class implements a data structure that stores a list of points in space.
////	A common task in game programming is to take a supplied point and discover which
////	of a stored set of points is nearest to it. For example, in path-plotting, it is often
////	useful to know which waypoint is nearest to the player's current
////	position. The kd-tree allows this "nearest neighbour" search to be carried out quickly,
////	or at least much more quickly than a simple linear search through the list.

////	At present, the class only allows for construction (using the MakeFromPoints static method)
////	and nearest-neighbour searching (using FindNearest). More exotic kd-trees are possible, and
////	this class may be extended in the future if there seems to be a need.

////	The nearest-neighbour search returns an integer index - it is assumed that the original
////	array of points is available for the lifetime of the tree, and the index refers to that
////	array.


//using System.Collections;
//using OpenTK;
//using System.Collections.Generic;
//using System;


//namespace OpenTKExtension
//{
//    public class KDTree_Stark
//    {

//        public KDTree_Stark[] innerTrees;

//        public static List<KeyValuePair<int, float>>  LatestSearchResults;
//        public Vertex pivot;
//        public int pivotIndex;
//        public int axis;
//        static List<int> takenPointIndices;

//        //	Change this value to 2 if you only need two-dimensional X,Y points. The search will
//        //	be quicker in two dimensions.
//        const int numDims = 3;


//        public KDTree_Stark()
//        {
//            innerTrees = new KDTree_Stark[2];
//            ResetSearch();
//        }

//        public static void Sample()
//        {
//            //tree = KDTreeNew.MakeFromPoints(pointsArray);
//            //var nearest: int = tree.FindNearest(targetPoint);
//        }
//        public void ResetSearch()
//        {
//            takenPointIndices = new List<int>();
//            LatestSearchResults = new List<KeyValuePair<int, float>>();
//        }
//        ////	Make a new tree from a list of points.
//        //public static KDTreeNew MakeFromVectors(params Vertex[] points)
//        //{
//        //    int[] indices = Iota(points.Length);
//        //    return MakeFromPointsInner(0, 0, points.Length - 1, points, indices);
//        //}
//        //	Make a new tree from a list of points.
//        public static KDTree_Stark Build(PointCloud points)
//        {
//            try
//            {
//                int[] indices = Iota(points.Count);
//                return MakeFromVectorsRecursive(0, 0, points.Count - 1, points, indices);
//            }
//            catch (System.Exception err)
//            {
//                System.Windows.Forms.MessageBox.Show("Error building kd-tree " + err.Message);
//                return null;

//            }
//        }

//        //	Find the nearest point in the set to the supplied point.
//        public List<int> FindNearestNPoints(Vertex pt, int n)
//        {
//            List<int> returnList = new List<int>();
//            returnList.Add(FindNearest_ExcludeTakenPoints(pt));

//            for(int i = 1; i < n; i++)
//            {
//                returnList.Add(FindNearest(pt));
//            }

//            return returnList;
//        }
//        //	Find the nearest point in the set to the supplied point.
//        public int FindNearestAdapted(Vertex pt, List<int> pointsFound)
//        {
//            float bestSqDist = float.MaxValue;
//            int bestIndex = -1;
//            LatestSearchResults = new List<KeyValuePair<int, float>>();
//            SearchRecursive(pt, ref bestSqDist, ref bestIndex);
            
//            return bestIndex;
//        }
        
//        //	Find the nearest point in the set to the supplied point.
//        public int FindNearest(Vertex pt)
//        {
//            float bestSqDist = float.MaxValue;
//            int bestIndex = -1;
//            LatestSearchResults = new List<KeyValuePair<int, float>>();
//            SearchRecursive(pt, ref bestSqDist, ref bestIndex);
           
//            return bestIndex;
//        }
//        //	Find the nearest point in the set to the supplied point.
//        public int FindNearest_ExcludeTakenPoints(Vertex pt)
//        {
//            float bestSqDist = float.MaxValue;
//            int bestIndex = -1;
//            LatestSearchResults = new List<KeyValuePair<int, float>>();
//            //SearchRecursive_ExcludeTakenPoints(pt, ref bestSqDist, ref bestIndex);
//            SearchRecursive(pt, ref bestSqDist, ref bestIndex);

//            takenPointIndices.Add(bestIndex);
//            return bestIndex;
//        }
//        //	Recursively build a tree by separating points at plane boundaries.
//        static KDTree_Stark MakeFromVectorsRecursive(int depth, int stIndex, int enIndex, PointCloud points,int[] inds)
//        {

//            KDTree_Stark root = new KDTree_Stark();
//            root.axis = depth % numDims;
//            int splitPoint = FindPivotIndex(points, inds, stIndex, enIndex, root.axis);

//            root.pivotIndex = inds[splitPoint];
//            root.pivot = points[root.pivotIndex];

//            int leftEndIndex = splitPoint - 1;

//            if (leftEndIndex >= stIndex)
//            {
//                root.innerTrees[0] = MakeFromVectorsRecursive(depth + 1, stIndex, leftEndIndex, points, inds);
//            }

//            int rightStartIndex = splitPoint + 1;

//            if (rightStartIndex <= enIndex)
//            {
//                root.innerTrees[1] = MakeFromVectorsRecursive(depth + 1, rightStartIndex, enIndex, points, inds);
//            }

//            return root;
//        }


//        static void SwapElements(int[] arr, int a, int b)
//        {
//            int temp = arr[a];
//            arr[a] = arr[b];
//            arr[b] = temp;
//        }


//        //	Simple "median of three" heuristic to find a reasonable splitting plane.
//        static int FindSplitPoint(PointCloud points, int[] inds, int stIndex, int enIndex, int axis)
//        {
//            float a = Convert.ToSingle( points[inds[stIndex]].Vector[axis]);
//            float b = Convert.ToSingle( points[inds[enIndex]].Vector[axis]);
//            int midIndex = (stIndex + enIndex) / 2;
//            float m = Convert.ToSingle( points[inds[midIndex]].Vector[axis]);

//            if (a > b)
//            {
//                if (m > a)
//                {
//                    return stIndex;
//                }

//                if (b > m)
//                {
//                    return enIndex;
//                }

//                return midIndex;
//            }
//            else
//            {
//                if (a > m)
//                {
//                    return stIndex;
//                }

//                if (m > b)
//                {
//                    return enIndex;
//                }

//                return midIndex;
//            }
//        }


//        //	Find a new pivot index from the range by splitting the points that fall either side
//        //	of its plane.
//        public static int FindPivotIndex(PointCloud points, int[] inds, int stIndex, int enIndex, int axis)
//        {
//            int splitPoint = FindSplitPoint(points, inds, stIndex, enIndex, axis);
//            // int splitPoint = Random.Range(stIndex, enIndex);

//            Vertex pivot = points[inds[splitPoint]];
//            SwapElements(inds, stIndex, splitPoint);

//            int currPt = stIndex + 1;
//            int endPt = enIndex;

//            while (currPt <= endPt)
//            {
//                Vertex curr = points[inds[currPt]];

//                if ((curr.Vector[axis] > pivot.Vector[axis]))
//                {
//                    SwapElements(inds, currPt, endPt);
//                    endPt--;
//                }
//                else
//                {
//                    SwapElements(inds, currPt - 1, currPt);
//                    currPt++;
//                }
//            }

//            return currPt - 1;
//        }


//        public static int[] Iota(int num)
//        {
//            int[] result = new int[num];

//            for (int i = 0; i < num; i++)
//            {
//                result[i] = i;
//            }

//            return result;
//        }


//        //	Recursively search the tree.
//        void SearchRecursive(Vertex pt, ref float bestSqSoFar, ref int bestIndex)
//        {
//            float mySqDist = Distance_Euclid(pivot, pt);

//            LatestSearchResults.Add(new KeyValuePair<int, float>(pivotIndex, mySqDist));
//            if (mySqDist < bestSqSoFar)
//            {

//                bestSqSoFar = mySqDist;
//                bestIndex = pivotIndex;
//            }

//            float planeDist = Convert.ToSingle(pt.Vector[axis] - pivot.Vector[axis]); //DistFromSplitPlane(pt, pivot, axis);

//            int selector = planeDist <= 0 ? 0 : 1;

//            if (innerTrees[selector] != null)
//            {
//                innerTrees[selector].SearchRecursive(pt, ref bestSqSoFar, ref bestIndex);
//            }

//            selector = (selector + 1) % 2;

//            float sqPlaneDist = planeDist * planeDist;

//            if ((innerTrees[selector] != null) && (bestSqSoFar > sqPlaneDist))
//            {
//                innerTrees[selector].SearchRecursive(pt, ref bestSqSoFar, ref bestIndex);
//            }
//        }
//        //	Recursively search the tree.
//        void SearchRecursive_ExcludeTakenPoints(Vertex pt, ref float bestSqSoFar, ref int bestIndex)
//        {
//            float mySqDist = Distance_Euclid(pivot, pt);

//            if (mySqDist < bestSqSoFar && !takenPointIndices.Contains(pivotIndex))
//            {
//                bestSqSoFar = mySqDist;
//                bestIndex = pivotIndex;
//            }

//            float planeDist = Convert.ToSingle(pt.Vector[axis] - pivot.Vector[axis]); //DistFromSplitPlane(pt, pivot, axis);

//            int selector = planeDist <= 0 ? 0 : 1;

//            if (innerTrees[selector] != null)
//            {
//                innerTrees[selector].SearchRecursive_ExcludeTakenPoints(pt, ref bestSqSoFar, ref bestIndex);
//            }

//            selector = (selector + 1) % 2;

//            float sqPlaneDist = planeDist * planeDist;

//            if ((innerTrees[selector] != null) && (bestSqSoFar > sqPlaneDist))
//            {
//                innerTrees[selector].SearchRecursive_ExcludeTakenPoints(pt, ref bestSqSoFar, ref bestIndex);
//            }
//        }

//        //	Get a point's distance from an axis-aligned plane.
//        float DistFromSplitPlane(Vertex pt, Vertex planePt, int axis)
//        {
//            return Convert.ToSingle(pt.Vector[axis] - planePt.Vector[axis]);
//        }


//        //	Simple output of tree structure - mainly useful for getting a rough
//        //	idea of how deep the tree is (and therefore how well the splitting
//        //	heuristic is performing).
//        public string Dump(int level)
//        {
//            string result = pivotIndex.ToString().PadLeft(level) + "\n";

//            if (innerTrees[0] != null)
//            {
//                result += innerTrees[0].Dump(level + 2);
//            }

//            if (innerTrees[1] != null)
//            {
//                result += innerTrees[1].Dump(level + 2);
//            }

//            return result;
//        }
//        public float Distance_Euclid(float[] p1, float[] p2)
//        {
//            float fSum = 0;
//            for (int i = 0; i < p1.Length; i++)
//            //for (int i = 0; i < 3; i++)
//            {
//                float fDifference = (p1[i] - p2[i]);
//                fSum += fDifference * fDifference;
//            }
//            return System.Convert.ToSingle(System.Math.Sqrt(fSum));
//        }
//        public float Distance_Euclid(Vertex p1, Vertex p2)
//        {
//            float fSum = 0;
//            float fx = Convert.ToSingle(p1.Vector.X - p2.Vector.X);
//            float fy = Convert.ToSingle(p1.Vector.Y - p2.Vector.Y);
//            float fz = Convert.ToSingle(p1.Vector.Z - p2.Vector.Z);

//            fSum += fx * fx + fy * fy + fz * fz;
//            return Convert.ToSingle(System.Math.Sqrt(fSum));
//        }
//    }
//}