//
//A kd-tree implementation in C++ (and Fortran) by Matthew B. Kennel
//Article: https://arxiv.org/abs/physics/0408067
//C++, Fortran code : https://github.com/jmhodges/kdtree2
//ported to C# by Edgar Maass
//The KDTREE2 software is licensed under the terms of the Academic Free
//Software License, listed herein.  In addition, users of this software
//must give appropriate citation in relevant technical documentation or
//journal paper to the author, Matthew B. Kennel, Institute For
//Nonlinear Science, preferably via a reference to the www.arxiv.org
//repository of this document, {\tt www.arxiv.org e-print:
//physics/0408067}.  This requirement will be deemed to be advisory and
//not mandatory as is necessary to permit the free inclusion of the
//present software with any software licensed under the terms of any
//version of the GNU General Public License, or GNU Library General
//Public License.



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;


namespace OpenTKExtension
{
    public class KDTreeKennell : KDTreeBase, IKDTree
    {
       
        
        // "TreeVectors" is a reference to the underlying 
        // data to be included in the tree.
        //
        // it would be a very bad idea to change the underlying data
        // during use of the search facilities of this tree.



        public bool sort_results; // USERS set to 'true'.
        
        public KDTreeNodeKennell root; // the root node



        public uint[] Indices;
        // the index for the tree leaves.  Data in a leaf with bounds [l,u] are
        // in  'the_data[ind[l],*] to the_data[ind[u],*]

        public PointCloud pointCloudResult = new PointCloud();
        // if rearrange is true then this is the rearranged data storage. 


        

        //----------
     

        public KDTreeKennell()
        {
            
        }
        public KDTreeKennell(PointCloud pc) : this()
        {
           
            Build(pc);

        }
        public bool Build(PointCloud pcTarget)
        {
            this.targetTreee = pcTarget;
            
            this.TreeVectors = pcTarget.VectorsWithIndex;


            this.sort_results = false;
            this.root = null;

            this.Indices = new uint[pcTarget.Vectors.Length];

            this.TreeVectors = pcTarget.VectorsWithIndex;

            for (uint i = 0; i < this.TreeVectors.Count; i++)
            {
                Indices[i] = i;
            }
            root = new KDTreeNodeKennell(this);
            root = root.build_tree_for_range(0, TreeVectors.Count - 1, null);

            return true;
        }

        /// <summary>
        /// FInd the closest matching point using a full For-loop search: O(n)
        /// </summary>
        /// <param name="vertex">Vertex to match</param>
        /// <param name="nearest_index">Index of matching vertex in the KDTree vertex array</param>
        /// <returns>Nearest matching vertex</returns>
        public VertexKDTree FindClosestPoint(VertexKDTree vertex, ref float nearestDistance, ref int nearest_index)
        {
           
            VertexKDTree v = new VertexKDTree();
            
            ListKDTreeResultVectors listResult = Find_N_Nearest(vertex.Vector, 1);
            
            if(listResult != null && listResult.Count > 0)
            {
                nearest_index = Convert.ToInt32(listResult[0].IndexNeighbour);
                nearestDistance = listResult[0].Distance;
                v = this.TreeVectors[  Convert.ToInt32( listResult[0].IndexNeighbour)];
            }
                      
            return v;
        }

     
        public PointCloud RemoveDuplicates(PointCloud source, float threshold)
        {
            PointCloud pcResult = new PointCloud();
            VertexKDTree[] resultArray = new VertexKDTree[source.Count];

            try
            {
                List<Vector3> listV = new List<Vector3>();
                List<Vector3> listC = new List<Vector3>();

                System.Threading.Tasks.Parallel.For(0, source.Count, i =>
                {
                    VertexKDTree vSource = new VertexKDTree(source.Vectors[i], i);
                    int nearest_index = 0;
                    float nearest_distance = 0f;
                    VertexKDTree vTargetFound = FindClosestPoint(vSource, ref nearest_distance, ref nearest_index);
                    if (nearest_distance > threshold)
                    {
                        resultArray[i] = vSource;

                    }

                });

                for(int i = 0; i < source.Count; i++ )
                {
                    if(resultArray[i] != null)
                    {
                        listV.Add(resultArray[i].Vector);
                        listC.Add(resultArray[i].Color);
                    }
                }
                pcResult.Vectors = listV.ToArray();
                pcResult.Colors = listC.ToArray();
            }
            catch(Exception err)
            {
                System.Windows.Forms.MessageBox.Show("Error in KDTreeKennnellRemoveDuplicates: " + err.Message);
            }
            return pcResult;

        }
  
        /// <summary>
        /// returns the target (tree) points found for the input (source) points
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public PointCloud FindClosestPointCloud_Parallel(PointCloud source)
        {
            this.source = source;
            this.ResetTaken();
           

            VertexKDTree[] resultArray = new VertexKDTree[source.Count];

            //shuffle points for the "Taken" algorithm
            PointCloud sourceShuffled;
            if (this.TakenAlgorithm)
            {
                sourceShuffled = source.Clone();
                sourceShuffled.SetDefaultIndices();
                sourceShuffled = PointCloud.Shuffle(sourceShuffled);

            }
            else
            {
                sourceShuffled = source;
            }

            int nearest_index = 0;
            float nearest_distance = 0f;

            System.Threading.Tasks.Parallel.For(0, source.Count, i =>
            //for (int i = 0; i < sourceShuffled.Count; i++)
            {
                VertexKDTree vSource = new VertexKDTree(sourceShuffled.Vectors[i], i);
                VertexKDTree vTargetFound = FindClosestPoint(vSource, ref nearest_distance, ref nearest_index);
                //resultArray[i] = vTargetFound.Clone();
                resultArray[i] = vTargetFound;


            });

            List<VertexKDTree> resultList = new List<VertexKDTree>(resultArray);
            result = PointCloud.FromListVertexKDTree(resultList);
            
            //shuffle back
            //float f = PointCloud.MeanDistance(sourceShuffled, pcResult);
            PointCloud pcResultShuffledBack;
            if (this.TakenAlgorithm)
            {
                pcResultShuffledBack = result.Clone();
                for (int i = 0; i < pcResultShuffledBack.Count; i++)
                {
                    //pcResultShuffled.Vectors[i] = pcResult.Vectors[Convert.ToInt32(sourceShuffled.Indices[i])];
                    pcResultShuffledBack.Vectors[Convert.ToInt32(sourceShuffled.Indices[i])] = result.Vectors[i];

                }
            }
            else
            {
                pcResultShuffledBack = result;
            }
            //this.MeanDistance = PointCloud.MeanDistance(source, pcResultShuffledBack);
            return pcResultShuffledBack;
            
        }

        /// <summary>
        /// returns the target (tree) points found for the input (source) points
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public PointCloud FindClosestPointCloud_NotParallel(PointCloud source)
        {
            this.source = source;
            this.ResetTaken();
          

            VertexKDTree[] resultArray = new VertexKDTree[source.Count];

            PointCloud sourceShuffled;
            if (this.TakenAlgorithm)
            {
                sourceShuffled = source.Clone();
                sourceShuffled.SetDefaultIndices();
                sourceShuffled = PointCloud.Shuffle(sourceShuffled);

            }
            else
            {
                sourceShuffled = source;
            }
            int nearest_index = 0;
            float nearest_distance = 0f;
            //System.Threading.Tasks.Parallel.For(0, source.Count, i =>
            for (int i = 0; i < sourceShuffled.Count; i++)
            {
                VertexKDTree vSource = new VertexKDTree(sourceShuffled.Vectors[i], i);
              
                VertexKDTree vTargetFound = FindClosestPoint(vSource, ref nearest_distance, ref nearest_index);
                //resultArray[i] = vTargetFound.Clone();
                resultArray[i] = vTargetFound;


            };

            List<VertexKDTree> resultList = new List<VertexKDTree>(resultArray);
            result = PointCloud.FromListVertexKDTree(resultList);

     
           
            //shuffle back
            //float f = PointCloud.MeanDistance(sourceShuffled, pcResult);
            PointCloud pcResultShuffledBack;
            if (this.TakenAlgorithm)
            {
                pcResultShuffledBack = result.Clone();
                for (int i = 0; i < pcResultShuffledBack.Count; i++)
                {
                    //pcResultShuffled.Vectors[i] = pcResult.Vectors[Convert.ToInt32(sourceShuffled.Indices[i])];
                    pcResultShuffledBack.Vectors[Convert.ToInt32(sourceShuffled.Indices[i])] = result.Vectors[i];

                }
            }
            else
            {
                pcResultShuffledBack = result;
            }

           // this.MeanDistance = PointCloud.MeanDistance(source, pcResultShuffledBack);

            return pcResultShuffledBack;


        }

        public PointCloud BuildAndFindClosestPoints(PointCloud source, PointCloud target, bool takenAlgorithm)
        {
        
            this.Build(target);


            TakenAlgorithm = takenAlgorithm;
            if(takenAlgorithm)
            {

            }
            result = FindClosestPointCloud_Parallel(source);
           
            return result;

        }
        public PointCloud BuildAndFindClosestPoints_NotParallel(PointCloud source, PointCloud target, bool takenAlgorithm)
        {
           
            this.Build(target);


            TakenAlgorithm = takenAlgorithm;
            if (takenAlgorithm)
            {

            }
            result = FindClosestPointCloud_NotParallel(source);

            return result;

        }


        #region utils

        // utility



        //public static void swap(ref uint a, ref uint b)
        //{
        //    uint tmp;
        //    tmp = a;
        //    a = b;
        //    b = tmp;
        //}

        //public static void swap(ref float a, ref float b)
        //{
        //    float tmp;
        //    tmp = a;
        //    a = b;
        //    b = tmp;
        //}






        #endregion

        #region base implementation

        public void Dispose()
        {
            root = null;
        }
      
        #endregion



     

        //public ListKDTreeResultVectors n_nearest_brute_force(List<float> qv, int nn)
        //{
        //    ListKDTreeResultVectors result = new ListKDTreeResultVectors();

        //    for (int i = 0; i < TreeVectors.Count; i++)
        //    {
        //        float dis = 0.0F;
                
        //        for (int j = 0; j < 3; j++)
        //        {
        //            float f = TreeVectors[i].Vector[j] - qv[j];
        //            dis += f * f;
        //        }
        //        KDTreeResult e = new KDTreeResult(Convert.ToUInt32(i), dis);
              
        //        result.Add(e);
        //    }

        //    result.Sort();
        //    //sort(result.begin(), result.end());

        //    return result;
        //}
        public ListKDTreeResultVectors Find_N_Nearest(Vector3 qv, int numberOfNeighbours)
        {
            
            SearchRecord sr = new SearchRecord(qv);
           
            sr.NumberOfNeighbours = numberOfNeighbours;

            root.search(sr);
            return sr.SearchResult;
        }
        public ListKDTreeResultVectors n_nearest_around_point(int idxin, int correltime, int numberOfNeighbours)
        {

            Vector3 qv = new Vector3();

            qv = TreeVectors[idxin].Vector.Clone();

            // copy the query vector.


            SearchRecord sr = new SearchRecord(qv);
            // construct the search record.
            //sr.IndexCenter = idxin;
            //sr.correltime = correltime;
            sr.NumberOfNeighbours = numberOfNeighbours;
            root.search(sr);


            //if (sort_results)
            //{
            //    sort(result.begin(), result.end());
            //}
            //            ListKDTreeResultVectors result = new ListKDTreeResultVectors();

            return sr.SearchResult;
        }

        public ListKDTreeResultVectors r_nearest(Vector3 qv, float r2)
        {
            // search for all within a ball of a certain radius
            //ListKDTreeResultVectors result = new ListKDTreeResultVectors();

            SearchRecord sr = new SearchRecord(qv);
            // Vector3 vdiff = new Vector3();



          
            sr.NumberOfNeighbours = 0;
            sr.Ballsize = r2;

            root.search(sr);

            //if (sort_results)
            //{
            //    sort(result.begin(), result.end());
            //}
            return sr.SearchResult;

        }
        ///// <summary>
        ///// number of search result
        ///// </summary>
        ///// <param name="qv"></param>
        ///// <param name="r2"></param>
        ///// <returns></returns>
        //public int r_count(Vector3 qv, float r2)
        //{
        //    {
        //        // search for all within a ball of a certain radius
        //        ListKDTreeResultVectors result = new ListKDTreeResultVectors();
        //        SearchRecord sr = new SearchRecord(qv, this, result);

        //        sr.IndexCenter = -1;
        //        sr.correltime = 0;
        //        sr.IndexNeighbour = 0;
        //        sr.Ballsize = r2;

        //        root.search(sr);
        //        return (result.Count);
        //    }


        //}
        public ListKDTreeResultVectors r_nearest_around_point(int idxin, int correltime, float r2)
        {
            ListKDTreeResultVectors result = new ListKDTreeResultVectors();
            Vector3 qv = TreeVectors[idxin].Vector.Clone(); //  query vector

          
            // copy the query vector.


            SearchRecord sr = new SearchRecord(qv);
            // construct the search record.
         
            sr.Ballsize = r2;
            sr.NumberOfNeighbours = 0;
            root.search(sr);


            //if (sort_results)
            //{
            //    result.So
            //    sort(result.begin(), result.end());
            //}

            return sr.SearchResult;
        }

        //public int r_count_around_point(int idxin, int correltime, float r2)
        //{
        //    Vector3 qv = new Vector3(); //  query vector


        //    for (int i = 0; i < 3; i++)
        //    {
        //        qv[i] = TreeVectors[idxin][i];
        //    }
        //    // copy the query vector.

        //    {
        //        ListKDTreeResultVectors result = new ListKDTreeResultVectors();
        //        SearchRecord sr = new SearchRecord(qv, this, result);
        //        // construct the search record.
        //        sr.IndexCenter = idxin;
        //        sr.correltime = correltime;
        //        sr.Ballsize = r2;
        //        sr.IndexNeighbour = 0;
        //        root.search(sr);
        //        return result.Count;
        //    }


        //}
     
    }
   
}
