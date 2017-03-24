using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace OpenTKExtension.DelaunayVoronoi
{
    public class Delaunay
    {
        //public static Triangle ComputeSuperTrianglePC(IList<Vector3> listVectors)
        //{
        //    double xmin = listVectors[0].X;
        //    double xmax = xmin;
        //    double ymin = listVectors[0].Y;
        //    double ymax = ymin;

        //    foreach (Vector3 p in listVectors)
        //    {
        //        if (p.X < xmin) xmin = p.X;
        //        if (p.X > xmax) xmax = p.X;
        //        if (p.Y < ymin) ymin = p.Y;
        //        if (p.Y > ymax) ymax = p.Y;
        //    }

        //    double dx = xmax - xmin;
        //    double dy = ymax - ymin;
        //    double dmax = (dx > dy) ? dx : dy;

        //    double xmid = (xmax + xmin) * 0.5;
        //    double ymid = (ymax + ymin) * 0.5;

        //    Vector3 p1 = new Vector3((xmid - 2 * dmax), (ymid - dmax), 0);
        //    Vector3 p2 = new Vector3(xmid, (ymid + 2 * dmax), 0);
        //    Vector3 p3 = new Vector3((xmid + 2 * dmax), (ymid - dmax), 0);

        //    return new TriangleVectors(p1, p2, p3);
        //}

        //public static List<Triangle> ComputeSuperTrianglePC(IList<Vector3> listVectors)
        //{
        //    if (listVectors.Count == 3)
        //    {
        //        return new List<Triangle> { new Triangle(0,1,2)  };
        //    }
        //    var triangles = new List<Triangle>();

        //    Triangle superTriangle = ComputeSuperTriangle(listVectors);

        //    triangles.Add(superTriangle);

        //    listVectors.Add(superTriangle.P1);
        //    listVectors.Add(superTriangle.P2);
        //    listVectors.Add(superTriangle.P3);

        //    foreach (Vector3 p in listVectors)
        //    {
        //        var edges = new List<Line>();

        //        for (int i = triangles.Count - 1; i >= 0; i--)
        //        {
        //            TriangleVectors t = triangles[i];
        //            var c = new Circle(t);

        //            if (c.Contains(p))
        //            {
        //                edges.Add(new Line(t.P1, t.P2));
        //                edges.Add(new Line(t.P2, t.P3));
        //                edges.Add(new Line(t.P3, t.P1));
        //                triangles.RemoveAt(i);
        //            }
        //        }

        //        for (int i = edges.Count - 2; i >= 0; i--)
        //        {
        //            for (int j = edges.Count - 1; j > i; j--)
        //            {
        //                if (edges[i].Equals(edges[j]))
        //                {
        //                    edges.RemoveAt(j);
        //                    edges.RemoveAt(i);
        //                    break;
        //                }
        //            }
        //        }

        //        foreach (Line l in edges)
        //        {
        //            triangles.Add(new TriangleVectors(l.P1, l.P2, p));
        //        }
        //    }

        //    for (int i = triangles.Count - 1; i >= 0; i--)
        //    {
        //        TriangleVectors t = triangles[i];

        //        foreach (Vector3 p in superTriangle.Vertices)
        //        {
        //            if (t.P1.Equals(p) || t.P2.Equals(p) || t.P3.Equals(p))
        //            {
        //                triangles.RemoveAt(i);
        //                break;
        //            }
        //        }
        //    }

        //    listVectors.RemoveAt(listVectors.Count - 1);
        //    listVectors.RemoveAt(listVectors.Count - 1);
        //    listVectors.RemoveAt(listVectors.Count - 1);

        //    return triangles;
        //}


        public static List<TriangleVectors> DelaunayTriangulation(IList<Vector3> listVectors)
        {
            if (listVectors.Count == 3)
            {
                return new List<TriangleVectors> { new TriangleVectors(listVectors[0], listVectors[1], listVectors[2]) };
            }
            var triangles = new List<TriangleVectors>();

            TriangleVectors superTriangle = ComputeSuperTriangle(listVectors);

            triangles.Add(superTriangle);

            listVectors.Add(superTriangle.P1);
            listVectors.Add(superTriangle.P2);
            listVectors.Add(superTriangle.P3);

            foreach (Vector3 p in listVectors)
            {
                var edges = new List<Line>();

                for (int i = triangles.Count - 1; i >= 0; i--)
                {
                    TriangleVectors t = triangles[i];
                    var c = new Circle(t);

                    if (c.Contains(p))
                    {
                        edges.Add(new Line(t.P1, t.P2));
                        edges.Add(new Line(t.P2, t.P3));
                        edges.Add(new Line(t.P3, t.P1));
                        triangles.RemoveAt(i);
                    }
                }

                for (int i = edges.Count - 2; i >= 0; i--)
                {
                    for (int j = edges.Count - 1; j > i; j--)
                    {
                        if (edges[i].Equals(edges[j]))
                        {
                            edges.RemoveAt(j);
                            edges.RemoveAt(i);
                            break;
                        }
                    }
                }

                foreach (Line l in edges)
                {
                    triangles.Add(new TriangleVectors(l.P1, l.P2, p));
                }
            }

            for (int i = triangles.Count - 1; i >= 0; i--)
            {
                TriangleVectors t = triangles[i];

                foreach (Vector3 p in superTriangle.Vertices)
                {
                    if (t.P1.Equals(p) || t.P2.Equals(p) || t.P3.Equals(p))
                    {
                        triangles.RemoveAt(i);
                        break;
                    }
                }
            }

            listVectors.RemoveAt(listVectors.Count - 1);
            listVectors.RemoveAt(listVectors.Count - 1);
            listVectors.RemoveAt(listVectors.Count - 1);

            return triangles;
        }


        public static TriangleVectors ComputeSuperTriangle(IList<Vector3> listVectors)
        {
            double xmin = listVectors[0].X;
            double xmax = xmin;
            double ymin = listVectors[0].Y;
            double ymax = ymin;

            foreach (Vector3 p in listVectors)
            {
                if (p.X < xmin) xmin = p.X;
                if (p.X > xmax) xmax = p.X;
                if (p.Y < ymin) ymin = p.Y;
                if (p.Y > ymax) ymax = p.Y;
            }

            double dx = xmax - xmin;
            double dy = ymax - ymin;
            double dmax = (dx > dy) ? dx : dy;

            double xmid = (xmax + xmin) * 0.5;
            double ymid = (ymax + ymin) * 0.5;

            Vector3 p1 = new Vector3((xmid - 2 * dmax), (ymid - dmax), 0);
            Vector3 p2 = new Vector3(xmid, (ymid + 2 * dmax), 0);
            Vector3 p3 = new Vector3((xmid + 2 * dmax), (ymid - dmax), 0);

            return new TriangleVectors(p1, p2, p3);
        }
   
    }
}
