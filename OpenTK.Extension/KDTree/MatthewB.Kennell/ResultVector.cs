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
    using System.Collections.Generic;

    public struct interval
    {
        public float lower;
        public float upper;
        public interval(float myLow, float myUp)
        {
            lower = myLow;
            upper = myUp;
        }
        public override string ToString()
        {
            return lower.ToString("F2") + " : " + upper.ToString("F2");
            
        }
    }

    public class Box
    {
        interval[] intervals = new interval[3];
        public interval this[int index]
        {
            get
            {
                return intervals[index];
            }
            set
            {
                intervals[index] = value;
            }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (interval intv in intervals)
                sb.Append(intv.ToString() + " : ");

            return sb.ToString();
        }
        

    }

    public class KDTreeResult
    {
        // 
        // the search routines return a (wrapped) vector
        // of these. 
        //
        public KDTreeResult(uint ind, float dist)
        {
            IndexNeighbour = ind;
            Distance = dist;
        }
        public float Distance; // its square Euclidean distance
        public uint IndexNeighbour; // which neighbor was found

        public static bool operator <(KDTreeResult e1, KDTreeResult e2)
        {
            return (e1.Distance < e2.Distance);
        }
        public static bool operator >(KDTreeResult e1, KDTreeResult e2)
        {
            return (e1.Distance > e2.Distance);
        }
    }



    public class SearchRecord
    {

     

        public Vector3 VectorTarget;

        public bool rearrange;
        public int NumberOfNeighbours; // , nfound;
        public float Radius;
      
        public ListKDTreeResultVectors SearchResult = new ListKDTreeResultVectors(); // results
       
        // constructor

        public SearchRecord(Vector3 qv_in)
       {
            this.VectorTarget = qv_in;
          
            Radius = float.MaxValue;
            NumberOfNeighbours = 0;
        }

    }


    public partial class ListKDTreeResultVectors : List<KDTreeResult>
    {
        
        public float ReplaceLast_ReturnFirst(KDTreeResult e)
        {
            this.RemoveAt(this.Count - 1);
            this.Add(e);

            return this[0].Distance;
            
        
     
        }

    }

}
