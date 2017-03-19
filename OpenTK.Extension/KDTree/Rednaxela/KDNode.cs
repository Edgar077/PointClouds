//
// C# KD Tree Implementation from //https://code.google.com/p/kd-sharp/
// Based on the Java implementation from : https://bitbucket.org/rednaxela/knn-benchmark/src/tip/ags/utils/dataStructures/trees/thirdGenKD/ </remarks>
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KDTreeRednaxela
{
    /// <summary>
    /// A KD-Tree node which supports a generic number of dimensions.  All data items
    /// need the same number of dimensions.
    /// This node splits based on the largest range of any dimension.
    /// </summary>
    /// <typeparam name="T">The generic data type this structure contains.</typeparam>
    public class KDNode_Rednaxela<T>
    {
        #region Internal properties and constructor
        // All types
        /// <summary>
        /// The number of dimensions for this node.
        /// </summary>
        protected internal int dimensions;

        /// <summary>
        /// The maximum capacity of this node.
        /// </summary>
        protected internal int bucketCapacity;

        // Leaf only
        /// <summary>
        /// The array of locations.  [index][dimension]
        /// </summary>
        protected internal float[][] points;

        /// <summary>
        /// The array of data values. [index]
        /// </summary>
        public T[] data;

        // Stem only
        /// <summary>
        /// The left and right children.
        /// </summary>
        public KDNode_Rednaxela<T> pLeft, pRight;
        /// <summary>
        /// The split dimension.
        /// </summary>
        protected internal int splitDimension;
        /// <summary>
        /// The split value (larger go into the right, smaller go into left)
        /// </summary>
        protected internal float fSplitValue;

        // Bounds
        /// <summary>
        /// The min and max bound for this node.  All dimensions.
        /// </summary>
        protected internal float[] minBound, maxBound;

        /// <summary>
        /// Does this node represent only one point.
        /// </summary>
        protected internal bool IsSinglePoint;

        /// <summary>
        /// Protected method which constructs a new KDNode.
        /// </summary>
        /// <param name="iDimensions">The number of dimensions for this node (all the same in the tree).</param>
        /// <param name="iBucketCapacity">The initial capacity of the bucket.</param>
        protected KDNode_Rednaxela(int iDimensions, int iBucketCapacity)
        {
            // Variables.
            this.dimensions = iDimensions;
            this.bucketCapacity = iBucketCapacity;
            this.Size = 0;
            this.IsSinglePoint = true;

            // Setup leaf elements.
            this.points = new float[iBucketCapacity+1][];
            this.data = new T[iBucketCapacity+1];
        }
        #endregion

        #region External Operations
        /// <summary>
        /// The number of items in this leaf node and all children.
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// Is this KDNode a leaf or not?
        /// </summary>
        public bool IsLeaf { get { return points != null; } }

        /// <summary>
        /// Insert a new point into this leaf node.
        /// </summary>
        /// <param name="tPoint">The position which represents the data.</param>
        /// <param name="kValue">The value of the data.</param>
        public void AddPoint(float[] tPoint, T kValue)
        {
            // Find the correct leaf node.
            KDNode_Rednaxela<T> pCursor = this;
            while (!pCursor.IsLeaf)
            {
                // Extend the size of the leaf.
                pCursor.ExtendBounds(tPoint);
                pCursor.Size++;

                // If it is larger select the right, or lower,  select the left.
                if (tPoint[pCursor.splitDimension] > pCursor.fSplitValue)
                {
                    pCursor = pCursor.pRight;
                }
                else
                {
                    pCursor = pCursor.pLeft;
                }
            }

            // Insert it into the leaf.
            pCursor.AddLeafPoint(tPoint, kValue);
        }
        #endregion

        #region Internal Operations
        /// <summary>
        /// Insert the point into the leaf.
        /// </summary>
        /// <param name="tPoint">The point to insert the data at.</param>
        /// <param name="kValue">The value at the point.</param>
        private void AddLeafPoint(float[] tPoint, T kValue)
        {
            // Add the data point to this node.
            points[Size] = tPoint;
            data[Size] = kValue;
            ExtendBounds(tPoint);
            Size++;

            // Split if the node is getting too large in terms of data.
            if (Size == points.Length - 1)
            {
                // If the node is getting too physically large.
                if (CalculateSplit())
                {
                    // If the node successfully had it's split value calculated, split node.
                    SplitLeafNode();
                }
                else
                {
                    // If the node could not be split, enlarge node data capacity.
                    IncreaseLeafCapacity();
                }
            }
        }

        /// <summary>
        /// If the point lies outside the boundaries, return false else true.
        /// </summary>
        /// <param name="tPoint">The point.</param>
        /// <returns>True if the point is inside the boundaries, false outside.</returns>
        private bool CheckBounds(float[] tPoint)
        {
            for (int i = 0; i < dimensions; ++i)
            {
                if (tPoint[i] > maxBound[i]) return false;
                if (tPoint[i] < minBound[i]) return false;
            }
            return true;
        }

        /// <summary>
        /// Extend this node to contain a new point.
        /// </summary>
        /// <param name="tPoint">The point to contain.</param>
        private void ExtendBounds(float[] tPoint)
        {
            // If we don't have bounds, create them using the new point then bail.
            if (minBound == null) 
            {
                minBound = new float[dimensions];
                maxBound = new float[dimensions];
                Array.Copy(tPoint, minBound, dimensions);
                Array.Copy(tPoint, maxBound, dimensions);
                return;
            }

            // For each dimension.
            for (int i = 0; i < dimensions; ++i)
            {
                if (float.IsNaN(tPoint[i]))
                {
                    if (!float.IsNaN(minBound[i]) || !float.IsNaN(maxBound[i]))
                        IsSinglePoint = false;
                    
                    minBound[i] = float.NaN;
                    maxBound[i] = float.NaN;
                }
                else if (minBound[i] > tPoint[i])
                {
                    minBound[i] = tPoint[i];
                    IsSinglePoint = false;
                }
                else if (maxBound[i] < tPoint[i])
                {
                    maxBound[i] = tPoint[i];
                    IsSinglePoint = false;
                }
            }
        }

        /// <summary>
        /// float the capacity of this leaf.
        /// </summary>
        private void IncreaseLeafCapacity()
        {   
            Array.Resize<float[]>(ref points, points.Length * 2);
            Array.Resize<T>(ref data, data.Length * 2);
        }

        /// <summary>
        /// Work out if this leaf node should split.  If it should, a new split value and dimension is calculated
        /// based on the dimension with the largest range.
        /// </summary>
        /// <returns>True if the node split, false if not.</returns>
        private bool CalculateSplit()
        {
            // Don't split if we are just one point.
            if (IsSinglePoint)
                return false;

            // Find the dimension with the largest range.  This will be our split dimension.
            float fWidth = 0;
            for (int i = 0; i < dimensions; i++)
            {
                float fDelta = (maxBound[i] - minBound[i]);
                if (float.IsNaN(fDelta))
                    fDelta = 0;

                if (fDelta > fWidth)
                {
                    splitDimension = i;
                    fWidth = fDelta;
                }
            }

            // If we are not wide (i.e. all the points are in one place), don't split.
            if (fWidth == 0)
                return false;

            // Split in the middle of the node along the widest dimension.
            fSplitValue = Convert.ToSingle( (minBound[splitDimension] + maxBound[splitDimension]) * 0.5);

            // Never split on infinity or NaN.
            if (fSplitValue == float.PositiveInfinity)
                fSplitValue = float.MaxValue;
            else if (fSplitValue == float.NegativeInfinity)
                fSplitValue = float.MinValue;
            
            // Don't let the split value be the same as the upper value as
            // can happen due to rounding errors!
            if (fSplitValue == maxBound[splitDimension])
                fSplitValue = minBound[splitDimension];

            // Success
            return true;
        }

        /// <summary>
        /// Split this leaf node by creating left and right children, then moving all the children of
        /// this node into the respective buckets.
        /// </summary>
        private void SplitLeafNode()
        {
            // Create the new children.
            pRight = new KDNode_Rednaxela<T>(dimensions, bucketCapacity);
            pLeft  = new KDNode_Rednaxela<T>(dimensions, bucketCapacity);

            // Move each item in this leaf into the children.
            for (int i = 0; i < Size; ++i)
            {
                // Store.
                float[] tOldPoint = points[i];
                T kOldData = data[i];

                // If larger, put it in the right.
                if (tOldPoint[splitDimension] > fSplitValue)
                    pRight.AddLeafPoint(tOldPoint, kOldData);

                // If smaller, put it in the left.
                else
                    pLeft.AddLeafPoint(tOldPoint, kOldData);
            }

            // Wipe the data from this KDNode.
            points = null;
            data = null;
        }
        #endregion
    }
}
