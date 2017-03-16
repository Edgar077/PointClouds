using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;


namespace OpenTKExtension
{
    public interface IKDTree
    {
        PointCloud BuildAndFindClosestPoints(PointCloud source, PointCloud target, bool takenAlgorithm);
        PointCloud BuildAndFindClosestPoints_NotParallel(PointCloud source, PointCloud target, bool takenAlgorithm);

        bool Build(PointCloud pcTarget);
        PointCloud FindClosestPointCloud_Parallel(PointCloud source);
        PointCloud FindClosestPointCloud_NotParallel(PointCloud source);

        VertexKDTree FindClosestPoint(VertexKDTree vertex, ref float nearest_distance, ref int nearest_index);
        
        bool TakenAlgorithm {get;set;}
        
        float MeanDistance{get;}



    }
}
