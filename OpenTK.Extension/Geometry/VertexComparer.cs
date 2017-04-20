using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKExtension
{
    /// <summary>
    /// Compare pointCloud based on their indices in the model
    /// </summary>
    public class VertexComparerIndexInPointCloud : IComparer<Vertex>
    {
        public static readonly VertexComparerIndexInPointCloud IndexInPointCloud = new VertexComparerIndexInPointCloud();

        public int Compare(Vertex x, Vertex y)
        {
            return x.Index.CompareTo(y.Index);
        }
    }
    
}
