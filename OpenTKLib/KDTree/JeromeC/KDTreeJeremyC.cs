//
//A kd-tree implementation in C++ (and Fortran) by Matthew B. Kennel
//Article: https://arxiv.org/abs/physics/0408067
//
//ported to C# by Jeremy C.
 //https://github.com/Jerdak/KDTree2
//
//wrong: https://github.com/jmhodges/kdtree2
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
    public class KDTreeJeremyC : KDTreeBase, IKDTree
	{
        

        //private PointCloud pointsSource;
        private PointCloud treePointCloud;
       

       
        KDNodeJeremyC root = new KDNodeJeremyC();
     
    
		public KDNodeJeremyC Root { get { return root; } private set { root = value; } }

		/// <summary>
		/// Number of nodes in the tree (Currently Unused)
		/// </summary>
		public int NodeCount { get; set; }

		/// <summary>
		/// Return the number of stored vertices.
		/// </summary>
		public int VertexCount { get { return TreeVectors.Count; } }

		/// <summary>
		/// Maximum allowable node size (# of vertices in a node)
		/// </summary>
		public int MaxNodeSize { get; set; }

		/// <summary>
		/// Maximum allowable recursion depth for building the KDTree
		/// </summary>
		public int MaxNodeDepth { get; set; }

		public KDTreeJeremyC()
        {
			NodeCount = 0;
			MaxNodeSize = 100;
			MaxNodeDepth = 25;
            
            //MaxNodeSize = 200;
            //MaxNodeDepth = 50;
		}
        public KDTreeJeremyC(PointCloud pc) : this()
        {
            
            Build(pc);

        }
        ///// <summary>
        ///// Add a new vertex/point to the KDTree build process.  Points must be added before calling KDTree.Build()
        ///// </summary>
        ///// <param name="vertex">3D vertex</param>
        //public void AddPoint(VectorWithIndex vertex) 
        //{
        //    Vectors.Add(vertex);
        //}

        ///// <summary>
        ///// Add a new vertex/point to the KDTree build process.  Points must be added before calling KDTree.Build()
        ///// </summary>
        ///// <param name="x">VectorWithIndex.x</param>
        ///// <param name="y">VectorWithIndex.y</param>
        ///// <param name="z">VectorWithIndex.z</param>
        //public void AddPoint(float x, float y, float z)
        //{
        //    Vectors.Add(new VectorWithIndex(new Vector3(x,y,z), -1));
        //}

		/// <summary>
		/// Add a new list of vertices/points to the KDTree build process.  Points must be added before calling KDTree.Build()
		/// </summary>
		/// <param name="vertex">List of 3D vertices</param>
        //public void AddPoints(List<VectorWithIndex> vertices)
        //{
        //    Vectors = Vectors.Concat(vertices).ToList();
        //}

        ///// <summary>
        ///// Add a new list of vertices/points to the KDTree build process.  Points must be added before calling KDTree.Build()
        ///// </summary>
        ///// <param name="vertex">List of 3D vertices stored as a 1D array w/ every 3 elements defining a vertex.</param>
        //public void AddPoints(List<float> vertices)
        //{
        //    int element_size = 3;
        //    if (vertices.Count % element_size != 0) return;	//vertex count must be a multiple of element_size
        //    for(int i = 0; i < vertices.Count; i+=element_size){
        //        float x = vertices[i];
        //        float y = vertices[i+1];
        //        float z = vertices[i+2];
        //        Vectors.Add(new VectorWithIndex(new Vector3(x, y, z), -1));
        //    }
			
        //}
		/// <summary>
		/// Build KDTree
		/// </summary>
		/// <notes>
		/// Points are added via KDTree.AddPoint(s) methods, the Build process uses
		/// those points to create the final tree.  If new points are added after a
		/// tree has been built, a new tree must be created.
		/// </notes>
		/// <returns></returns>
		public bool Build(PointCloud pcTarget)
         {
            this.targetTreee = pcTarget;

            
            this.treePointCloud = pcTarget;
            this.TreeVectors = treePointCloud.VectorsWithIndex;


			if(VertexCount <= 0) 
            {
				Console.WriteLine("[Warning] - No vertices added to KDTree, aborting build.");
				return false;
			}
			Root = new KDNodeJeremyC();// { SplitAxis = largest_split_axis, AABB = new BoundingBoxAxisAligned(min,max), MidPoint = center, Parent = null};
            


			{	// Fill the Root
				int index = 0;
				foreach (var vertex in TreeVectors )
                {
					root.AddVertex(index, vertex);
					index++;
				}
			}

			BuildNode(Root, 0);
			return true;
		}

		/// <summary>
		/// Recursively split node across its pivot axis.
		/// </summary>
		/// <param name="node">KDTreeNode to split</param>
		/// <param name="depth">Current recursion depth, set lower if you get stack overflow</param>
		/// <returns>True if split was a success or max_depth/max_node_size criterion met</returns>
		bool BuildNode(KDNodeJeremyC node, int depth)
        {
			if (depth >= MaxNodeDepth) 
                return true;
			if (node.Indices.Count <= MaxNodeSize) 
                return true;

			foreach(var index in node.Indices)
            {
				VertexKDTree vertex = TreeVectors[index];

                if (!node.IsBuilt)
                    node.Build();
				KDNodeJeremyC child = node.GetSplitNode(vertex);
				child.AddVertex(index, vertex);
			}

			// TODO:  Do we need to check if either child is empty?  Since we're calculating the split axis
			//		  using raw data it's unlikely.
			node.Clear();
			node.ChildLeft.Build();
			node.ChildRight.Build();

			BuildNode(node.ChildLeft, depth + 1);
			BuildNode(node.ChildRight, depth + 1);
			return true;
		}

		/// <summary>
		/// Iterate through System.IO.Collections.Generic.List<int> to find closest matching vertex
		/// </summary>
		/// <param name="indices">List of integer indices that index vertices</param>
		/// <param name="vertex">Vertex to match</param>
		/// <returns>Closest stored 'vertex' to given 'vertex'</returns>
		VertexKDTree GetClosestVertexFromIndices(List<int> indices, VertexKDTree vertex, ref int nearest_index)
		{
			int min_index = -1;
			float min_dist = 0.0f;

			foreach (var index in indices)
			{
				VertexKDTree tmp_vertex = TreeVectors[index];
                if (! ( TakenAlgorithm && tmp_vertex.TakenInTree))
                {
                    if (min_index == -1)
                    {
                        //min_dist = tmp_vertex.Vector.Distance(vertex.Vector);
                        min_dist = tmp_vertex.Vector.DeltaSquared(vertex.Vector);
                        min_index = index;
                    }
                    else
                    {
                        //float tmp_dist = tmp_vertex.Vector.Distance(vertex.Vector);
                        float tmp_dist = tmp_vertex.Vector.DistanceSquared(vertex.Vector);

                        if (tmp_dist < min_dist)
                        {
                            min_dist = tmp_dist;
                            min_index = index;
                        }
                    }
                }
				
			}
			nearest_index = min_index;
            if (min_index != -1)
            {
                TreeVectors[min_index].TakenInTree = true;
                return TreeVectors[min_index];
            }
            return null;
		}
		/// <summary>
		/// Find closest point using an axis aligned search boundary
		/// </summary>
		/// <param name="node"></param>
		/// <param name="point"></param>
		/// <returns></returns>
        VertexKDTree FindClosestPoint_Recursive(KDNodeJeremyC node, VertexKDTree vertex, BoundingBoxAxisAligned search_bounds, ref int nearest_index)
        {
            int tmp_index = -1;
            if (node.IsLeaf)
            {
                tmp_index = -1;
                VertexKDTree result = GetClosestVertexFromIndices(node.Indices, vertex, ref tmp_index);
                if (result != null)
                {
                    nearest_index = tmp_index;
                    return result;
                }
                else
                {
                    return null;
                }
            }

            tmp_index = -1;
            KDNodeJeremyC near_child = node.GetSplitNode(vertex);

            VertexKDTree retV = FindClosestPoint_Recursive(near_child, vertex, search_bounds, ref tmp_index);
            //Edgar - TakenVector implementation - have to go one level up if vector is taken
            float near_distance = float.MaxValue;
            if (retV != null)
            {
                //near_distance = retV.Vector.Distance(vertex.Vector);
                near_distance = retV.Vector.DistanceSquared(vertex.Vector);
                nearest_index = tmp_index;
            }

            KDNodeJeremyC far_child = near_child.Sibling;
            if (search_bounds != null && far_child.BoundingBox.Intersects(search_bounds))
            {
                VertexKDTree far_result = FindClosestPoint_Recursive(far_child, vertex, search_bounds, ref tmp_index);
                //Edgar - TakenVector implementation - have to go one level up if vector is taken
                if (far_result != null)
                {
                    //float far_distance = far_result.Vector.Distance(vertex.Vector);
                    float far_distance = far_result.Vector.DistanceSquared(vertex.Vector);
                    if (far_distance < near_distance)
                    {
                        nearest_index = tmp_index;
                        retV = far_result;
                    }
                }
            }

            if(retV == null)
            {

            }
            return retV;
        }

		/// <summary>
		/// Find closest point using a centered bounding sphere
		/// </summary>
		/// <param name="node"></param>
		/// <param name="vertex"></param>
		/// <param name="search_bounds"></param>
		/// <param name="nearest_index"></param>
		/// <returns></returns>
		VertexKDTree FindClosestPoint(KDNodeJeremyC node, VertexKDTree vertex, BoundingSphere search_bounds, ref int nearest_index)
		{
			int tmp_index = -1;
			if (node.IsLeaf)
			{
				tmp_index = -1;
				VertexKDTree result = GetClosestVertexFromIndices(node.Indices, vertex, ref tmp_index);
				nearest_index = tmp_index;
				return result;
			}

			tmp_index = -1;
			KDNodeJeremyC near_child = node.GetSplitNode(vertex);
			VertexKDTree near_result = FindClosestPoint(near_child, vertex, search_bounds, ref tmp_index);
			VertexKDTree ret = near_result;
			//float near_distance = near_result.Vector.Distance(vertex.Vector);
            float near_distance = near_result.Vector.DistanceSquared(vertex.Vector);
			nearest_index = tmp_index;

			KDNodeJeremyC far_child = near_child.Sibling;

			if (far_child.BoundingBox.Intersects(search_bounds))
			{
				VertexKDTree far_result = FindClosestPoint(far_child, vertex, search_bounds, ref tmp_index);
                float far_distance = far_result.Vector.DistanceSquared(vertex.Vector);
                //float far_distance = far_result.Vector.Distance(vertex.Vector);
				if (far_distance < near_distance)
				{
					nearest_index = tmp_index;
					ret = far_result;
				}
			}
			return ret;
		}

      
		/// <summary>
		/// Find the closest matching vertex
		/// </summary>
		/// <param name="vertex">Vertex to find closest match.</param>
		/// <param name="distance">Search distance.</param>
		/// <param name="nearest_index">Index of matching vertex in the KDTree vertex array</param>
		/// <returns>Nearest matching vertex</returns>
		public VertexKDTree FindClosestPoint(VertexKDTree vertex, ref float distance, ref int nearest_index)
		{
			if(root == null)
            {
				Console.WriteLine("Null root, Build() must be called before using the KDTree.");
				return new VertexKDTree(new Vector3(0, 0, 0), -1);
			}

            BoundingBoxAxisAligned search_bounds = null;
            if (distance != 0)
            {
                Vector3 distance_vector = new Vector3(distance, distance, distance);
                search_bounds = new BoundingBoxAxisAligned(vertex.Vector - distance_vector, vertex.Vector + distance_vector);
            }
			return FindClosestPoint_Recursive(Root, vertex, search_bounds, ref nearest_index);

		}
        private List<VertexKDTree> FindClosestPoint_List_NotParallel(List<VertexKDTree> source)
        {
            this.ResetTaken();
            
            VertexKDTree[] vArray = source.ToArray();

            for (int i = 0; i < source.Count; i++)
            //System.Threading.Tasks.Parallel.For(0, source.Count, i =>
            {

                int nearest_index = 0;
                float nearest_distance = 0f;

                VertexKDTree vTargetFound = FindClosestPoint(vArray[i], ref nearest_distance, ref nearest_index);
                vArray[i] = vTargetFound.Clone();


            };
            return new List<VertexKDTree>(vArray);
        }
        private List<VertexKDTree> FindClosestPoint_List_Parallel(List<VertexKDTree> source)
        {
            this.ResetTaken();
            

            VertexKDTree[] vArray = source.ToArray();
            System.Threading.Tasks.Parallel.For(0, source.Count, i =>
            {
                int nearest_index = 0;
                float nearest_distance = 0f;
                VertexKDTree vTargetFound = FindClosestPoint(vArray[i], ref nearest_distance, ref nearest_index);
                vArray[i] = vTargetFound.Clone();
                

            });

            return new List<VertexKDTree>(vArray);
        }
        private PointCloud FindClosestPointCloud(List<VertexKDTree> source)
        {
            
            return FindClosestPointCloud_Parallel(PointCloud.FromListVertexKDTree(source));

        }
        /// <summary>
        /// the parallel method is not working - no time to find reason for that
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public PointCloud FindClosestPointCloud_Parallel(PointCloud source)
        {
            this.source = source;
            this.ResetTaken();
            /// the parallel method is not working - no time to find reason for that, calling normal method
            List<VertexKDTree> resultList = FindClosestPoint_List_Parallel(source.VectorsWithIndex);
            this.result = PointCloud.FromListVertexKDTree(resultList);

           
            return PointCloud.FromListVertexKDTree(resultList);
            


        }
        /// <summary>
        /// the parallel method is not working - no time to find reason for that
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public PointCloud FindClosestPointCloud_NotParallel(PointCloud source)
        {
            this.source = source;
            this.ResetTaken();
            /// the parallel method is not working - no time to find reason for that, calling normal method
            List<VertexKDTree> resultList = FindClosestPoint_List_NotParallel(source.VectorsWithIndex);
            
            return PointCloud.FromListVertexKDTree(resultList);



        }


        /// <summary>
        /// Purge vertex memory and root
        /// </summary>
        public void Clear() {
			TreeVectors.Clear();
			root = null;
		}
        public PointCloud BuildAndFindClosestPoints(PointCloud source, PointCloud target, bool takenAlgorithm)
        {
            
            Build(target);
            TakenAlgorithm = takenAlgorithm;
            //GlobalVariables.ShowLastTimeSpan("Build Kennell");

            //PointCloud result = FindClosestPointCloud(source.VectorsWithIndex);
            result = FindClosestPointCloud_Parallel(source);

            //GlobalVariables.ShowLastTimeSpan("Search Kennell");

            
           

            return result;
            
        }
        public PointCloud BuildAndFindClosestPoints_NotParallel(PointCloud source, PointCloud target, bool takenAlgorithm)
        {

            Build(target);
            TakenAlgorithm = takenAlgorithm;
            //GlobalVariables.ShowLastTimeSpan("Build Kennell");

            //PointCloud result = FindClosestPointCloud(source.VectorsWithIndex);
            result = FindClosestPointCloud_NotParallel(source);

            //GlobalVariables.ShowLastTimeSpan("Search Kennell");



            return result;

        }
    }
}
