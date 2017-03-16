

using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;

using OpenTK;
using OpenTKExtension;
using System.Drawing;

namespace OpenTKExtension.Triangulation
{


    public class TriangulateTest
    {

        public static Mesh Triangulate(PointCloud pointCloud, int recursion)
        {
            Mesh m = new Mesh();
            m.Recursion = recursion;
            System.DateTime start = System.DateTime.Now;

            m.Compute(pointCloud);
           
            return m;
        }
        //public void ComputeDelaunayTriangulation(List<Vector3> points)
        //{
        //    //""" Takes a list of point objects (which must have x and y fields).
        //    //    Returns a list of 3-tuples: the indices of the points that form a
        //    //    Delaunay triangle.
        //    //""";
        //    //siteList = SiteList(points);
        //    //context  = Context();
        //    //context.triangulate = True;
        //    //voronoi(siteList,context);
        //    //return context.triangles;
        //}

    }



}
