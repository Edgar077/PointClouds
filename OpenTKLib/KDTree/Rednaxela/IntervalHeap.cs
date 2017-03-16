//
// C# KD Tree Implementation from //https://code.google.com/p/kd-sharp/
// Based on the Java implementation from : https://bitbucket.org/rednaxela/knn-benchmark/src/tip/ags/utils/dataStructures/trees/thirdGenKD/ </remarks>
//
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KDTreeRednaxela
{
    /// <summary>
    /// A binary interval heap is float-ended priority queue is a priority queue that it allows
    /// for efficient removal of both the maximum and minimum element.
    /// </summary>
    /// <typeparam name="T">The data type contained at each key.</typeparam>
    public class IntervalHeap<T>
    {
        /// <summary>
        /// The default size for a new interval heap.
        /// </summary>
        private const int DEFAULT_SIZE = 64;

        /// <summary>
        /// The internal data array which contains the stored objects.
        /// </summary>
        private T[] pointsFound;

        /// <summary>
        /// The array of keys which 
        /// </summary>
        private float[] distances;

        private int[] indexes;

        /// <summary>
        /// Construct a new interval heap with the default capacity.
        /// </summary>
        public IntervalHeap() : this(DEFAULT_SIZE)
        {
        }

        /// <summary>
        /// Construct a new interval heap with a custom capacity.
        /// </summary>
        /// <param name="capacity"></param>
        public IntervalHeap(int capacity)
        {
            this.pointsFound = new T[capacity];
            this.distances = new float[capacity];
            indexes = new int[capacity];
            this.Capacity = capacity;
            this.Size = 0;
        }

        /// <summary>
        /// The number of items in this interval heap.
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// The current capacity of this interval heap.
        /// </summary>
        public int Capacity { get; private set; }

        /// <summary>
        /// Get the data with the smallest key.
        /// </summary>
        public T ClosestPoint
        {
            get
            {
                if (Size == 0)
                    throw new Exception();
                return pointsFound[0];
            }
        }
        /// <summary>
        /// Get the data with the smallest key.
        /// </summary>
        public int ClosestPointIndex
        {
            get
            {
                if (Size == 0)
                    throw new Exception();
                return indexes[0];
            }
        }

        ///// <summary>
        ///// Get the data with the largest key.
        ///// </summary>
        //public T Max
        //{
        //    get
        //    {
        //        if (Size == 0)
        //        {
        //            throw new Exception();
        //        }
        //        else if (Size == 1)
        //        {
        //            return pointsFound[0];
        //        }

        //        return pointsFound[1];
        //    }
        //}

        /// <summary>
        /// Get the smallest key.
        /// </summary>
        public float DistanceMin
        {
            get
            {
                if (Size == 0)
                    throw new Exception();
                return distances[0];
            }
        }

        /// <summary>
        /// Get the largest key.
        /// </summary>
        public float MaxKey
        {
            get
            {
                if (Size == 0)
                {
                    throw new Exception();
                }
                else if (Size == 1)
                {
                    return distances[0];
                }

                return distances[1];
            }
        }

        /// <summary>
        /// Insert a new data item at a given key.
        /// </summary>
        /// <param name="key">The value which represents our data (i.e. a distance).</param>
        /// <param name="value">The data we want to store.</param>
        public void Insert(float key, T value, int index)
        {
            // If more room is needed, float the array size.
            if (Size >= Capacity)
            {
                // float the capacity.
                Capacity *= 2;

                // Expand the data array.
                var newData = new T[Capacity];
                Array.Copy(pointsFound, newData, pointsFound.Length);
                pointsFound = newData;

                // Expand the key array.
                var newKeys = new float[Capacity];
                Array.Copy(distances, newKeys, distances.Length);
                distances = newKeys;

                // Expand the indexes array.
                var newIndexes = new int[Capacity];
                Array.Copy(indexes, newIndexes, indexes.Length);
                indexes = newIndexes;

            }

            // Insert the new value at the end.
            Size++;
            pointsFound[Size-1] = value;
            distances[Size-1] = key;
            indexes[Size - 1] = index;


            // Ensure it is in the right place.
            SiftInsertedValueUp();
        }

        /// <summary>
        /// Remove the item with the smallest key from the queue.
        /// </summary>
        public void RemoveMin()
        {
            // Check for errors.
            if (Size == 0)
                throw new Exception();

            // Remove the item by 
            Size--;
            pointsFound[0] = pointsFound[Size];
            distances[0] = distances[Size];

            pointsFound[Size] = default(T);
            indexes[0] = indexes[Size];
            indexes[Size] = -1;

            SiftDownMin(0);
        }

        /// <summary>
        /// Replace the item with the smallest key in the queue.
        /// </summary>
        /// <param name="key">The new minimum key.</param>
        /// <param name="value">The new minumum data value.</param>
        public void ReplaceMin(float key, T value, int index)
        {
            // Check for errors.
            if (Size == 0)
                throw new Exception();

            // Add the data.
            pointsFound[0] = value;
            distances[0] = key;
            indexes[0] = index;


            // If we have more than one item.
            if (Size > 1)
            {
                // Swap with pair if necessary.
                if (distances[1] < key)
                    Swap(0, 1);
                SiftDownMin(0);
            }
        }

        /// <summary>
        /// Remove the item with the largest key in the queue.
        /// </summary>
        public void RemoveMax()
        {
            // If we have no items in the queue.
            if (Size == 0)
            {
                throw new Exception();
            }

            // If we have one item, remove the min.
            else if (Size == 1)
            {
                RemoveMin();
                return;
            }

            // Remove the max.
            Size--;
            pointsFound[1] = pointsFound[Size];
            distances[1] = distances[Size];
            pointsFound[Size] = default(T);

            indexes[1] = indexes[Size];
            indexes[Size] = -1;

            SiftDownMax(1);
        }

        /// <summary>
        /// Swap out the item with the largest key in the queue.
        /// </summary>
        /// <param name="key">The new key for the largest item.</param>
        /// <param name="value">The new data for the largest item.</param>
        public void ReplaceMax(float key, T value, int index)
        {
            if (Size == 0)
            {
                throw new Exception();
            }
            else if (Size == 1)
            {
                ReplaceMin(key, value, index);
                return;
            }

            pointsFound[1] = value;
            distances[1] = key;
            indexes[1] = index;
            // Swap with pair if necessary
            if (key < distances[0]) 
            {
                Swap(0, 1);
            }
            SiftDownMax(1);
        }


        /// <summary>
        /// Internal helper method which swaps two values in the arrays.
        /// This swaps both data and key entries.
        /// </summary>
        /// <param name="x">The first index.</param>
        /// <param name="y">The second index.</param>
        /// <returns>The second index.</returns>
        private int Swap(int x, int y)
        {
            // Store temp.
            T yData = pointsFound[y];
            float yDist = distances[y];
            int index = indexes[y];

            // Swap
            pointsFound[y] = pointsFound[x];
            distances[y] = distances[x];
            indexes[y] = indexes[x];

            pointsFound[x] = yData;
            distances[x] = yDist;
            indexes[x] = index;
            // Return.
            return y;
        }

        /**
         * Min-side (u % 2 == 0):
         * - leftchild:  2u + 2
         * - rightchild: 2u + 4
         * - parent:     (x/2-1)&~1
         *
         * Max-side (u % 2 == 1):
         * - leftchild:  2u + 1
         * - rightchild: 2u + 3
         * - parent:     (x/2-1)|1
         */

        /// <summary>
        /// Place a newly inserted element a into the correct tree position.
        /// </summary>
        private void SiftInsertedValueUp()
        {
            // Work out where the element was inserted.
            int u = Size-1;

            // If it is the only element, nothing to do.
            if (u == 0)
            {
            }

            // If it is the second element, sort with it's pair.
            else if (u == 1)
            {
                // Swap if less than paired item.
                if  (distances[u] < distances[u-1])
                    Swap(u, u-1);
            }

            // If it is on the max side, 
            else if (u % 2 == 1)
            {
                // Already paired. Ensure pair is ordered right
                int p = (u/2-1)|1; // The larger value of the parent pair
                if  (distances[u] < distances[u-1])
                { // If less than it's pair
                    u = Swap(u, u-1); // Swap with it's pair
                    if (distances[u] < distances[p-1])
                    { // If smaller than smaller parent pair
                        // Swap into min-heap side
                        u = Swap(u, p-1);
                        SiftUpMin(u);
                    }
                }
                else
                {
                    if (distances[u] > distances[p])
                    { // If larger that larger parent pair
                        // Swap into max-heap side
                        u = Swap(u, p);
                        SiftUpMax(u);
                    }
                }
            }
            else
            {
                // Inserted in the lower-value slot without a partner
                int p = (u/2-1)|1; // The larger value of the parent pair
                if (distances[u] > distances[p])
                { // If larger that larger parent pair
                    // Swap into max-heap side
                    u = Swap(u, p);
                    SiftUpMax(u);
                }
                else if (distances[u] < distances[p-1])
                { // If smaller than smaller parent pair
                    // Swap into min-heap side
                    u = Swap(u, p-1);
                    SiftUpMin(u);
                }
            }
        }

        /// <summary>
        /// Bubble elements up the min side of the tree.
        /// </summary>
        /// <param name="iChild">The child index.</param>
        private void SiftUpMin(int iChild)
        {
            // Min-side parent: (x/2-1)&~1
            for (int iParent = (iChild/2-1)&~1; 
                iParent >= 0 && distances[iChild] < distances[iParent]; 
                iChild = iParent, iParent = (iChild/2-1)&~1)
            {
                Swap(iChild, iParent);
            }
        }

        /// <summary>
        /// Bubble elements up the max side of the tree.
        /// </summary>
        /// <param name="iChild">The child index.</param>
        private void SiftUpMax(int iChild)
        {
            // Max-side parent: (x/2-1)|1
            for (int iParent = (iChild/2-1)|1; 
                iParent >= 0 && distances[iChild] > distances[iParent]; 
                iChild = iParent, iParent = (iChild/2-1)|1)
            {
                Swap(iChild, iParent);
            }
        }

        /// <summary>
        /// Bubble elements down the min side of the tree.
        /// </summary>
        /// <param name="iParent">The parent index.</param>
        private void SiftDownMin(int iParent)
        {
            // For each child of the parent.
            for (int iChild = iParent * 2 + 2; iChild < Size; iParent = iChild, iChild = iParent * 2 + 2)
            {
                // If the next child is less than the current child, select the next one.
                if (iChild + 2 < Size && distances[iChild + 2] < distances[iChild])
                {
                    iChild += 2;
                }

                // If it is less than our parent swap.
                if (distances[iChild] < distances[iParent])
                {
                    Swap(iParent, iChild);

                    // Swap the pair if necessary.
                    if (iChild+1 < Size && distances[iChild+1] < distances[iChild])
                    {
                        Swap(iChild, iChild+1);
                    }
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Bubble elements down the max side of the tree.
        /// </summary>
        /// <param name="iParent"></param>
        private void SiftDownMax(int iParent)
        {
            // For each child on the max side of the tree.
            for (int iChild = iParent * 2 + 1; iChild <= Size; iParent = iChild, iChild = iParent * 2 + 1)
            {
                // If the child is the last one (and only has half a pair).
                if (iChild == Size)
                {
                    // CHeck if we need to swap with th parent.
                    if (distances[iChild - 1] > distances[iParent])
                        Swap(iParent, iChild - 1);
                    break;
                }

                // If there is only room for a right child lower pair.
                else if (iChild + 2 == Size)
                {
                    // Swap the children.
                    if (distances[iChild + 1] > distances[iChild])
                    {
                        // Swap with the parent.
                        if (distances[iChild + 1] > distances[iParent])
                           Swap(iParent, iChild + 1);
                        break;
                    }
                }

                // 
                else if (iChild + 2 < Size)
                {
                    // If there is room for a right child upper pair
                    if (distances[iChild + 2] > distances[iChild])
                    {
                        iChild += 2;
                    }
                }
                if (distances[iChild] > distances[iParent])
                {
                    Swap(iParent, iChild);
                    // Swap with pair if necessary
                    if (distances[iChild-1] > distances[iChild])
                    {
                        Swap(iChild, iChild-1);
                    }
                }
                else
                {
                    break;
                }
            }
        }
    }
}