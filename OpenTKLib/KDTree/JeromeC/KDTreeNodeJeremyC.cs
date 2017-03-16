//A kd-tree implementation in C++ (and Fortran) by Matthew B. Kennel
//Article: https://arxiv.org/abs/physics/0408067
//ported to C+ by Jeff Hodges: 
//
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
	public class KDNodeJeremyC
	{
		/// <summary>
		/// Minimum vertex in this node
		/// </summary>
        Vector3 minV;
		
		/// <summary>
		/// Maximum vertex in this node
		/// </summary>
        Vector3 maxV;
		
		
		
		/// <summary>
		/// Range of this node's vertices
		/// </summary>
        Vector3 rangeV;

		/// <summary>
		/// Child nodes
		/// </summary>
		//KDNodeJeremyC[] children_				= new KDNodeJeremyC[2];				//Node children.
        public KDNodeJeremyC ChildLeft;
        public KDNodeJeremyC ChildRight;


		/// <summary>
		/// Axis across which the data are split.  
		/// </summary>
        public Axis SplitAxis = Axis.X;//		{ get { return split_axis; } set { split_axis = value; } }
		//Axis split_axis = Axis.X;
        //Axis split_axis = Axis.X;

		/// <summary>
		/// Node axis aligned bounding box, bounds vertex data.
		/// </summary>
		public BoundingBoxAxisAligned BoundingBox { get { if (!IsBuilt)Build(); return boundingBox; } set { boundingBox = value; } }
		//BoundingBoxAxisAligned boundingBox = new BoundingBoxAxisAligned();
        BoundingBoxAxisAligned boundingBox;
		

		/// <summary>
		/// Node split pivot.  Will be the center of the data
		/// </summary>
        public VertexKDTree Leaf;
		//VectorWithIndex pivot = new VectorWithIndex();
		
		/// <summary>
		/// Node parent.
		/// </summary>
		public KDNodeJeremyC Parent			{ get { return parent; } set { parent = value; } }
		KDNodeJeremyC parent ;
		
		/// <summary>
		/// Node sibling.
		/// </summary>
		public KDNodeJeremyC Sibling { get { return sibling_; } set { sibling_ = value; } }
		KDNodeJeremyC sibling_ ;

		/// <summary>
		/// Buffered list of indexes.  Values stored here will index in to the KDTree.vertices List
		/// </summary>
		public List<int> Indices = new List<int>();//			{ get { return indices; } private set { indices = value; }}
		//List<int> indices = new List<int>();

		
		/// <summary>
		/// True iff node contains more than 0 indices
		/// </summary>
		public bool IsLeaf					{ get { return (Indices.Count>0);} }

		/// <summary>
		/// True iff node has been built, used internall for lazy initialization of certain class members. 
		/// </summary>
		public bool IsBuilt					{ get; set; }

		public KDNodeJeremyC()
        {
			//IsBuilt = false;
		}
		
		public void Clear() {
			Indices.Clear();
		}

		/// <summary>
		/// Add a new vertex, and its index, to node.
		/// </summary>
		/// <param name="index">Index in to KDTree's vertices List</param>
		/// <param name="vertex">Actual vertex from KDTree's vertex list</param>
		public void AddVertex(int index, VertexKDTree vertex)
		{
			if(Indices.Count == 0)
            {
				minV = maxV = vertex.Vector;
			}
			minV = Vector3.Min(minV, vertex.Vector);
            maxV = Vector3.Max(maxV, vertex.Vector);
			Indices.Add(index);
			IsBuilt = false;
		}

		/// <summary>
		/// Utility method to build KDTreeNode variables, as long as min and max
		/// </summary>
		public void Build()
		{
			Leaf = new VertexKDTree(new Vector3(maxV + minV) / 2.0f, -1);
			rangeV = maxV - minV;

			SplitAxis = rangeV.LargestAxis();
			
			BoundingBox = new BoundingBoxAxisAligned(minV, maxV);

			IsBuilt = true;
		}

        ///// <summary>
        ///// Get child node.
        ///// </summary>
        ///// <param name="index">Child index, must been either 0 or 1</param>
        ///// <returns>Child node if 'index' is correct, otherwise null.</returns>
        //public KDNodeJeremyC Child(int index) 
        //{ 
        //    if(index != 0 && index != 1) 
        //        return null;  
        //    return children_[index]; 
        //}
			
		/// <summary>
		/// Return stored index.
		/// </summary>
		/// <param name="index">Index in to indices_ to return, must be between [0,indices_.Count]</param>
		/// <returns>Stored index value if 'index' is correct, else -1</returns>
		public int Index(int index)
        {
			if (index < 0 || index >= Indices.Count) return -1;
            return Indices[index]; 
		}

		/// <summary>
		/// Return the KDTreeNode that 'vertex' belongs to according to the split axis.
		/// </summary>
		/// <param name="vertex">3D vertex to bin</param>
		/// <returns>KDTreeNode containing 'vertex'</returns>
		public KDNodeJeremyC GetSplitNode(VertexKDTree vertex) 
        {
           
			if (ChildLeft == null || ChildRight == null) 
            {
				ChildLeft = new KDNodeJeremyC { Parent = this };
				ChildRight = new KDNodeJeremyC { Parent = this };

				ChildLeft.Sibling = ChildRight;
				ChildRight.Sibling = ChildLeft;
			}
		

			float mid_value = Leaf.Vector[(int)SplitAxis];
			float pt_value = vertex.Vector[(int)SplitAxis];
            
            //float diff = pt_value - mid_value;
            //float diff2 = Math.Abs(diff / pt_value);

            //if (diff > 0)
            //    return ChildLeft;
            //else if (diff < 0 && diff2 < 1e-7)
            //    return ChildLeft;
            //else
            //    return ChildRight;
            KDNodeJeremyC child = (pt_value >= mid_value) ? ChildLeft : ChildRight;

            return child;
		}
        public override string ToString()
        {
            return Leaf.ToString();
        }
	}
}
