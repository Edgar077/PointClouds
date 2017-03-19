using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTKExtension;

namespace NLinear
{
    public class Hull
    {
        public float R = float.MaxValue;
        public Vector3<float> Center;
        public List<VertexHull> Vertices;
        public List<Edge> Edges;

        public Hull(Box myBox, Vector3<float> myCenter)
        {
            this.Center = new Vector3<float>(myCenter);
            this.Vertices = new List<VertexHull>();
            this.Edges = new List<Edge>();
            for (int i = 0; i < myBox.Vertices.Count; i++)
            {
                this.Vertices.Add(new VertexHull(myBox.Vertices[i], this.Center.DistanceTo(myBox.Vertices[i], 1)));
            }
            for (int i = 0; i < myBox.PointIndices1.Count; i++)
            {
                this.Edges.Add(new Edge(Vertices[myBox.PointIndices1[i]], Vertices[myBox.PointIndices2[i]]));
            }
        }
        public void IntersectVoronoi(Plane3<float> myPlane)
        {
            SetVertexConditions(myPlane);

            SetHullVoronoiFromEdges(myPlane);

            ClearPointsWithNegativeDistanceToPlaneFromHull();
            //////////////////////////////////
            //TODO AddEdges(myPlane);

        }
        /// <summary>
        /// according to distance to plane
        /// </summary>
        /// <param name="myPlane"></param>
        private void SetVertexConditions(Plane3<float> myPlane)
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                float db = myPlane.DistanceTo(Vertices[i].Vector);

                if (Math.Abs(db) < GlobalVariables.AbsoluteTolerance)
                {
                    Vertices[i].Condition = 1;
                }

                else if (db > 0)
                {
                    Vertices[i].Condition = 2;
                }
                else if (db < 0)
                {
                    Vertices[i].Condition = 0;
                }
            }
        }
        private void SetHullVoronoiFromEdges(Plane3<float> myPlane)
        {
            ///////////////////////
            int ii = 0;
            while (ii < Edges.Count)
            {
                if (Edges[ii].p1.Condition == 0 && Edges[ii].p2.Condition == 0)
                {
                    Edges.RemoveAt(ii);
                }
                else if (Edges[ii].p1.Condition == 1 && Edges[ii].p2.Condition == 0)
                {
                    Edges.RemoveAt(ii);
                }
                else if (Edges[ii].p1.Condition == 1 && Edges[ii].p2.Condition == 1)
                {
                    Edges.RemoveAt(ii);
                }
                else if (Edges[ii].p1.Condition == 0 && Edges[ii].p2.Condition == 1)
                {
                    Edges.RemoveAt(ii);
                }
                else if (Edges[ii].p1.Condition == 0 && Edges[ii].p2.Condition == 2)
                {
                    //float u;
                    Line3<float> line = new Line3<float>(Edges[ii].p1.Vector, Edges[ii].p2.Vector);
                    Vector3<float> vIntersect = myPlane.Intersect(line);

                    //Rhino.Geometry.Intersect.Intersection.Line3<float>Plane(line, p, out u);
                    //pts.Add(new vertex(line.PointAt(u), this.center.DistanceTo(line.PointAt(u))));
                    Vertices.Add(new VertexHull(vIntersect, this.Center.DistanceTo(vIntersect, 1)));
                    VertexHull vert = new VertexHull(vIntersect, this.Center.DistanceTo(vIntersect, 1));
                    Vertices.Add(vert);

                    Edges[ii].p1 = Vertices[Vertices.Count - 1];
                    ii++;
                }
                else if (Edges[ii].p1.Condition == 2 && Edges[ii].p2.Condition == 0)
                {
                    //float u; 
                    Line3<float> line = new Line3<float>(Edges[ii].p1.Vector, Edges[ii].p2.Vector);
                    Vector3<float> vIntersect = myPlane.Intersect(line);
                    //TODO Rhino.Geometry.Intersect.Intersection.Line3<float>Plane(line, p, out u);
                    //pts.Add(new vertex(line.PointAt(u), this.center.DistanceTo(line.PointAt(u))));
                    Vertices.Add(new VertexHull(vIntersect, this.Center.DistanceTo(vIntersect, 1)));
                    Edges[ii].p2 = Vertices[Vertices.Count - 1];
                    ii++;
                }
                else { ii++; }
            }
        }
        //private void AddEdges(Plane3<float> myPlane)
        //{
        //    Transform w2p = Transform.PlaneToPlane(Plane3.WorldXY, myPlane);
        //    Transform p2w = Transform.PlaneToPlane(myPlane, Plane3.WorldXY);

        //    //TODO Grasshopper.Kernel.Geometry.Node2List ls = new Grasshopper.Kernel.Geometry.Node2List();

        //    List<int> count = new List<int>();
        //    for (int i = 0; i < pts.Count; i++)
        //    {
        //        if (pts[i].Condition == 1 || pts[i].Condition == -1)
        //        {
        //            pts[i].pos.Transform(w2p);

        //            //TODO ls.Append(new Grasshopper.Kernel.Geometry.Node2(pts[i].pos.X, pts[i].pos.Y));

        //            pts[i].pos.Transform(p2w);
        //            count.Add(i);
        //        }
        //    }
        //    if (count.Count == 2) edges.Add(new EdgeVoronoi(pts[count[0]], pts[count[1]]));
        //    else if (count.Count > 2)
        //    {
        //        List<int> count2 = new List<int>();

        //        //TODO Grasshopper.Kernel.Geometry.ConvexHull.Solver.Compute(ls, count2);

        //        for (int i = 0; i < count2.Count; i++)
        //        {
        //            int c = i + 1;
        //            if (c == count2.Count)
        //                c = 0;
        //            edges.Add(new EdgeVoronoi(pts[count[count2[i]]], pts[count[count2[c]]]));
        //        }
        //    }
        //}

        public void ClearPointsWithNegativeDistanceToPlaneFromHull()
        {
            int i = 0;
            float max = 0;
            while (i < this.Vertices.Count)
            {
                if (this.Vertices[i].Condition == 0)
                {
                    this.Vertices.RemoveAt(i);
                }
                else
                {
                    if (max < this.Vertices[i].Radius)
                    {
                        max = this.Vertices[i].Radius;
                    }
                    i++;
                }
            }
            this.R = max;
        }
    }
}
