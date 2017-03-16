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
    public class KDTreeBase 
    {
       
        #region IKDTree 
        
        public List<VertexKDTree> TreeVectors;
        

        public bool TakenAlgorithm { get; set; }
        protected PointCloud source;
        protected PointCloud targetTreee;
        protected PointCloud result;

        #endregion



        protected void ResetTaken()
        {
            if (this.TakenAlgorithm)
            {
                for (int i = 0; i < this.TreeVectors.Count; i++)
                    this.TreeVectors[i].TakenInTree = false;
            }
        }
        public float MeanDistance
        {
            get
            {
                return PointCloud.MeanDistance(this.source, this.result);

            }
        }

    }
}
