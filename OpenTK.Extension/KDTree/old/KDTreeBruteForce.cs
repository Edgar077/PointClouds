

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;


namespace OpenTKExtension
{
    public class KDTreeBruteForce : KDTreeBase, IKDTree
    {
       
        public KDTreeBruteForce()
        {
           
		}
        public KDTreeBruteForce(PointCloud pc)
            : this()
        {
            this.TreeVectors = pc.VectorsWithIndex;

            

        }
        /// <summary>
        /// FInd the closest matching point using a full For-loop search: O(n)
        /// </summary>
        /// <param name="vertex">Vertex to match</param>
        /// <param name="nearest_index">Index of matching vertex in the KDTree vertex array</param>
        /// <returns>Nearest matching vertex</returns>
        public VertexKDTree FindClosestPoint(VertexKDTree vertex, ref float nearestDistance, ref int nearest_index)
        {
          
            float min_dist = 0.0f;
            int min_index = -1;
            for (int j = 0; j < TreeVectors.Count; j++)
            {
                VertexKDTree tmp_vertex = TreeVectors[j];
                //float distance = tmp_vertex.Vector.Distance(vertex.Vector);
                float distance = tmp_vertex.Vector.DistanceSquared(vertex.Vector);
                if (min_index == -1)
                {
                    min_dist = distance;
                    min_index = j;
                }
                else
                {
                    if (distance < min_dist)
                    {
                        if (!this.TakenAlgorithm)
                        {
                            min_dist = distance;
                            min_index = j;
                        }
                        else
                        {
                            if(!tmp_vertex.TakenInTree)
                            {
                                tmp_vertex.TakenInTree = true;
                                min_dist = distance;
                                min_index = j;
                            }
                        }

                    }
                }
            }
            nearest_index = min_index;
            return TreeVectors[min_index];
        }
     
        public bool Build(PointCloud pcTarget)
        {
            this.targetTreee = pcTarget;
            this.TreeVectors = pcTarget.VectorsWithIndex;
            return true;
		}

		
        public PointCloud FindClosestPointCloud_Parallel(PointCloud source)
        {
            this.source = source;
            this.ResetTaken();
           
            
            VertexKDTree[] resultArray = new VertexKDTree[source.Count];

            System.Threading.Tasks.Parallel.For(0, source.Count, i =>
            //for (int i = 0; i < source.Count; i++)
            {
                VertexKDTree vSource = new VertexKDTree(source.Vectors[i], i);

                int nearest_index = 0;
                float nearest_distance = 0f;
                VertexKDTree vTargetFound = FindClosestPoint(vSource, ref nearest_distance, ref nearest_index);

                //resultArray[i] = vTargetFound.Clone();
                resultArray[i] = vTargetFound;

            });

            List<VertexKDTree> resultList = new List<VertexKDTree>(resultArray);
            result = PointCloud.FromListVertexKDTree(resultList);
            
         
            return result;


        }


        public PointCloud FindClosestPointCloud_NotParallel(PointCloud source)
        {
            this.source = source;
            this.ResetTaken();

            VertexKDTree[] resultArray = new VertexKDTree[source.Count];

            // System.Threading.Tasks.Parallel.For(0, pointsSource.Count, i =>
            for (int i = 0; i < source.Count; i++)
            {
                VertexKDTree vSource = new VertexKDTree(source.Vectors[i], i);

                int nearest_index = 0;
                float nearest_distance = 0f;
                VertexKDTree vTargetFound = FindClosestPoint(vSource, ref nearest_distance, ref nearest_index);

                //resultArray[i] = vTargetFound.Clone();
                resultArray[i] = vTargetFound;


            }

            List<VertexKDTree> resultList = new List<VertexKDTree>(resultArray);
            result = PointCloud.FromListVertexKDTree(resultList);


            return result;


        }

        public PointCloud BuildAndFindClosestPoints(PointCloud source, PointCloud target, bool takenAlgorithm)
        {
           
            this.Build(target);
            TakenAlgorithm = takenAlgorithm;
           
            result = FindClosestPointCloud_Parallel(source);
        
            return result;

        }
        public PointCloud BuildAndFindClosestPoints_NotParallel(PointCloud source, PointCloud target, bool takenAlgorithm)
        {
          
            this.Build(target);
            
            TakenAlgorithm = takenAlgorithm;

            result = FindClosestPointCloud_NotParallel(source);
            return result;

        }



    }


}
