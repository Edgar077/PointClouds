
using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Collections;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenTKExtension;


namespace NLinear
{
    public class HullFrame
    {
        public HullFrame() { }
        public static Box Box;
        public static Edge Edge;


        public List<Line3<float>> ComputeVoronoi3d(List<Line3<float>> myLines, List<Vector3<float>> myPCL)
        {
            Box hu = new Box(myLines);
            Hull[] hulls = new Hull[myPCL.Count];
            /*
                  for (int ii = 0;ii < y.Count;ii++){
                    hull h = new hull(hu, y[ii]);
                    for(int i = 0;i < y.Count;i++){
                      if( i != ii && y[i].DistanceTo(y[ii]) < h.R * 2){
                        Line3<float> cen = new Line3<float>(y[ii]);cen += y[i];cen /= 2;
                        Vector3<float> v = y[ii] - y[i];
                        Plane3<float> plane = new Plane3<float>(cen, v);
                        h.IntersectVoronoi(plane);}
                    }
                    hulls.Add(h);
                  }
            */
            ///*
            //  System.Threading.Tasks.Parallel.ForEach(y, pt =>
            // {
            System.Threading.Tasks.Parallel.For(0, myPCL.Count, (iii) =>
            {
                Vector3<float> pt = myPCL[iii];
                Hull h = new Hull(hu, pt);
                for (int i = 0; i < myPCL.Count; i++)
                {
                    float t = myPCL[i].DistanceTo(pt, 1);
                    if (t > 0.001 && t < h.R * 2)
                    {
                        Vector3<float> cen = new Vector3<float>(pt); 
                        cen += myPCL[i]; cen /= 2;
                        Vector3<float> v = pt - myPCL[i];
                        Plane3<float> plane = new Plane3<float>(cen, v, 1);
                        h.IntersectVoronoi(plane);
                    }
                }
                hulls[iii] = h;
            });
            //  */
            List<Line3<float>> tree = new List<Line3<float>>();
            for (int k = 0; k < hulls.Length; k++)
            {
                Hull h = hulls[k];
                for (int i = 0; i < h.Edges.Count; i++)
                {
                    tree.Add(new Line3<float>(h.Edges[i].p1.Vector, h.Edges[i].p2.Vector));
                }
            }
            return tree;
        }
     
        
        //public List<Line3<float>> Offset3D(List<Polyline<float>> x, float y)
        //{
        //    List<Line3<float>> output = new List<Line3<float>>();
        //    if (x.Count < 4) return output;
        //    List<Line3<float>> lines = breakPoly(x[0]);

        //    for (int i = 1; i < x.Count; i++)
        //    {
        //        List<Line3<float>> ls = breakPoly(x[i]);
        //        //Print(ls.Count.ToString());
        //        for (int ii = 0; ii < ls.Count; ii++)
        //        {
        //            bool sign = true;
        //            for (int j = 0; j < lines.Count; j++)
        //            {
        //                if (isDumpLines(lines[j], ls[ii])) { sign = false; break; }
        //            }
        //            //Print(sign.ToString());
        //            if (sign) lines.Add(ls[ii]);
        //        }
        //    }
        //    Vector3<float> cen = new Vector3<float>();
        //    for (int i = 0; i < lines.Count; i++)
        //    {
        //        cen += lines[i].From; cen += lines[i].To;
        //    }
        //    // B = lines;
        //    cen /= 2 * lines.Count;
        //    BoxVoronoi box = new BoxVoronoi(lines);
        //    HullVoronoi hull = new HullVoronoi(box, cen);
        //    for (int i = 0; i < x.Count; i++)
        //    {
        //        if (x[i].Count < 3)
        //        {//Print("00001");
        //            return output;
        //        }
        //        Polyline<float> cp = x[i];
        //        Vector3<float> vi0 = cp[0];

        //        Vector3<float> xi0 = x[i][0];


        //        Plane3<float> p = new Plane3<float>(x[i][0], x[i][1], x[i][2], 1);
                
        //        Vector3<float> v = cen - p.ClosestPoint(cen);
        //        v.Normalize(1);
                
        //        p = new Plane3<float>(x[i][0], v, 1);
        //        p.Transform(Transform.Translation(v * y));
        //        hull.intersect(p);
        //        hull.clearnull();
        //    }

        //    for (int i = 0; i < hull.edges.Count; i++)
        //    {
        //        output.Add(new Line3<float>(hull.edges[i].p1.pos, hull.edges[i].p2.pos));
        //    }
        //    List<Vector3<float>> pt = new List<Vector3<float>>();
        //    for (int i = 0; i < hull.pts.Count; i++)
        //    {
        //        pt.Add(hull.pts[i].pos);
        //    }
        //    return output;
        //}
        public List<Line3<float>> BreakPoly(Polyline<float> pl)
        {
            List<Line3<float>> ls = new List<Line3<float>>();
            
            Vector3<float> v = pl[0];

            if (pl.Count < 1) return ls;
            for (int i = 1; i < pl.Count; i++)
            {
                ls.Add(new Line3<float>(pl[i], pl[i - 1], 1));
            }
            return ls;
        }
        public bool IsDumpLines(Line3<float> l1, Line3<float> l2)
        {
            //if ((l1.From.DistanceTo(l2.From) < RhinoDoc.ActiveDoc.ModelAbsoluteTolerance) && (l1.To.DistanceTo(l2.To) < RhinoDoc.ActiveDoc.ModelAbsoluteTolerance)) return true;
            //if ((l1.From.DistanceTo(l2.To) < RhinoDoc.ActiveDoc.ModelAbsoluteTolerance) && (l1.To.DistanceTo(l2.From) < RhinoDoc.ActiveDoc.ModelAbsoluteTolerance)) return true;

            if ((l1.From.DistanceTo(l2.From, 1) < GlobalVariables.AbsoluteTolerance) && (l1.To.DistanceTo(l2.To, 1) < GlobalVariables.AbsoluteTolerance)) 
                return true;
            if ((l1.From.DistanceTo(l2.To, 1) < GlobalVariables.AbsoluteTolerance) && (l1.To.DistanceTo(l2.From, 1) < GlobalVariables.AbsoluteTolerance)) 
                return true;

            return false;
        }
    }
}
