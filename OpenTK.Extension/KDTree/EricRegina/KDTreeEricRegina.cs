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
    public class KDTreeEricRegina : KDTreeBase, IKDTree
    {


        KDTreeEricReginaGeneric<float, string> tree;
        
        // if rearrange is true then this is the rearranged data storage. 


        

        //----------
     

        public KDTreeEricRegina()
        {
            
        }
        public KDTreeEricRegina(PointCloud pc) : this()
        {
           
            Build(pc);

        }
        public bool Build(PointCloud pcTarget)
        {
            Vector3 v = Vector3.Zero;
            List<Vector3> listV = new List<Vector3>(pcTarget.Vectors);

            float[][] treePoints = v.VectorListToFloatArray(listV);
           
            string[] treeNodes = new string[listV.Count];
           
            tree = new KDTreeEricReginaGeneric<float, string>(3, treePoints, UtilitiesRegina.L2Norm_Squared_Float);

         
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
            float[] array = new float[] { vertex.Vector.X, vertex.Vector.Y, vertex.Vector.Z };
            Tuple<float[], string>[] treeNearest = tree.NearestNeighbors(array, 1);

            Tuple<float[], string> p = treeNearest[0];


            VertexKDTree v = new VertexKDTree();
            v.Vector = new Vector3(p.Item1[0], p.Item1[1], p.Item1[2]);

            return v;
        }
        
        /// <summary>
        /// returns the target (tree) points found for the input (source) points
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public PointCloud FindClosestPointCloud_Parallel(PointCloud source)
        {
            this.source = source;

            PointCloud pcResult = new PointCloud();
            pcResult.Vectors = new Vector3[source.Vectors.Length];
            List<Vector3> listV = new List<Vector3>(source.Vectors);
            Vector3 v = Vector3.Zero;
            float[][] testData = v.VectorListToFloatArray(listV);

            for (int i = 0; i < listV.Count; i++)
            {
                Tuple<float[], string>[] treeNearest = tree.NearestNeighbors(testData[i], 1);
                Tuple<float[], string> p = treeNearest[0];

                v = new Vector3(p.Item1[0], p.Item1[1], p.Item1[2]);
                pcResult.Vectors[i] = v;
                //var linearNearest = UtilitiesRegina.LinearSearch(treePoints, treeNodes, testData[i], UtilitiesRegina.L2Norm_Squared_Float);

                // Assert.That(UtilitiesRegina.L2Norm_Squared_Double(testData[i], linearNearest.Item1), Is.EqualTo(UtilitiesRegina.L2Norm_Squared_Double(testData[i], treeNearest[0].Item1)));


                //  Assert.That(treeNearest[0].Item2, Is.EqualTo(linearNearest.Item2));
            }
            this.result = pcResult;
            return pcResult;

        }

        /// <summary>
        /// returns the target (tree) points found for the input (source) points
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public PointCloud FindClosestPointCloud_NotParallel(PointCloud source)
        {
            this.source = source;

            PointCloud pcResult = new PointCloud();
            pcResult.Vectors = new Vector3[source.Vectors.Length];
            List<Vector3> listV = new List<Vector3>(source.Vectors);
            Vector3 v = Vector3.Zero;
            float[][] testData = v.VectorListToFloatArray(listV);

            for (int i = 0; i < listV.Count; i++)
            {
                Tuple<float[], string>[] treeNearest = tree.NearestNeighbors(testData[i], 1);
                Tuple<float[], string> p = treeNearest[0];

                v = new Vector3(p.Item1[0], p.Item1[1], p.Item1[2]);
                pcResult.Vectors[i] = v;
                //var linearNearest = UtilitiesRegina.LinearSearch(treePoints, treeNodes, testData[i], UtilitiesRegina.L2Norm_Squared_Float);

                // Assert.That(UtilitiesRegina.L2Norm_Squared_Double(testData[i], linearNearest.Item1), Is.EqualTo(UtilitiesRegina.L2Norm_Squared_Double(testData[i], treeNearest[0].Item1)));


                //  Assert.That(treeNearest[0].Item2, Is.EqualTo(linearNearest.Item2));
            }
            this.result = pcResult;
            return pcResult;


        }

        public PointCloud BuildAndFindClosestPoints(PointCloud source, PointCloud target, bool takenAlgorithm)
        {

         
            this.Build(target);



            this.result = FindClosestPointCloud_Parallel(source);
            return result;

        }
        public PointCloud BuildAndFindClosestPoints_NotParallel(PointCloud source, PointCloud target, bool takenAlgorithm)
        {

      
            this.Build(target);
            this.result = FindClosestPointCloud_NotParallel(source);

            return result;

        }


   

        #region base implementation

        public void Dispose()
        {
            //root = null;
        }

        #endregion



    
     
    }
   
}
