using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Text.RegularExpressions;
//using System.Linq;

namespace OpenTKExtension
{
    public class KeyValueComparer : IComparer<KeyValuePair<int, float>>
    {

        public int Compare(KeyValuePair<int, float> a, KeyValuePair<int, float> b)
        {

            if (a.Value > b.Value)
                return 1;
            else if (a.Value > b.Value)
                return -1;
            else
                return 0;


        }
    }
    public class DistanceComparer : IComparer<KeyValuePair<OpenTK.Vector3, OpenTK.Vector3>>
    {

        public int Compare(KeyValuePair<OpenTK.Vector3, OpenTK.Vector3> a, KeyValuePair<OpenTK.Vector3, OpenTK.Vector3> b)
        {
            float an = a.Key.NormSquared();
            float bn = a.Key.NormSquared();

            if (an > bn)
                return 1;
            else if (an < bn)
                return -1;
            else
                return 0;

            
        }
    }
    public class NeighboursComparer : IComparer<Neighbours>
    {

        public int Compare(Neighbours a, Neighbours b)
        {

            if (a.Angle > b.Angle)
                return 1;
            else if (a.Angle == b.Angle)
            {
                if (a.Distance > b.Distance)
                {
                    return 1;
                }
                else
                    return -1;
            }

            else
                return -1;

        }
    }
    public class NeighboursListComparer : IComparer<List<Neighbours>>
    {

        public int Compare(List<Neighbours> a, List<Neighbours> b)
        {
            try
            {

                if (a[0].Angle > b[0].Angle)
                    return 1;

                else if (a[0].Angle == b[0].Angle)
                {
                    if (a[0].Distance > b[0].Distance)
                    {
                        return 1;
                    }
                    else
                        return -1;
                }

                else
                    return -1;
            }
            catch(Exception err)
            {
                System.Diagnostics.Debug.WriteLine("Error in sort: " + err.Message);
                return 1;
            }

        }
    }
    public class TriangleComparer : IComparer<Triangle>
    {

        public int Compare(Triangle a, Triangle b)
        {

            if (a.IndVertices[0] > b.IndVertices[0])
                return 1;
            else if (a.IndVertices[0] == b.IndVertices[0])
            {
                if (a.IndVertices[1] > b.IndVertices[1])
                    return 1;
                else if (a.IndVertices[1] == b.IndVertices[1])
                {
                    if (a.IndVertices[2] > b.IndVertices[2])
                        return 1;
                    else
                        return -1;
                }
                else
                    return -1;
            }
            else
                return -1;

        


        }
    }
    public class VertexIndexComparer : IComparer<Vertex>
    {

        public int Compare(Vertex a, Vertex b)
        {
            try
            {
                //
                //a.IndexKDTreeTarget                //
                if (a.Index > b.Index)
                    return 1;
                else
                    return -1;
            }
            catch(Exception err)
            {
                System.Diagnostics.Debug.WriteLine("Error in VertexIndexComparer " + err.Message);
                //System.Windows.Forms.MessageBox.Show("Error in sort : " + err.Message);
                return -1;
            }
        }
    }
 
    public class Vector3_XYZ : IComparer<Vector3>
    {

        public int Compare(Vector3 a, Vector3 b)
        {

            for (int i = 0; i < 3; i++)
            {
                if (a[i] > b[i])
                    return 1;
                else if (a[i] < b[i])
                    return -1;

            }

            return 0;
        }
    }
    public class Vector_Length : IComparer<Vertex>
    {

        public int Compare(Vertex a, Vertex b)
        {
            if (a.Vector.Length >= b.Vector.Length)
                return 1;
            return -1;

        }
    }
    public class VectorWithIndex_XYZ : IComparer<VertexKDTree>
    {

        public int Compare(VertexKDTree a, VertexKDTree b)
        {

            for (int i = 0; i < 3; i++)
            {
                if (a.Vector[i] > b.Vector[i])
                    return 1;
                else if (a.Vector[i] < b.Vector[i])
                    return -1;

            }

            return 0;
        }

   
    }
    public class VectorWithIndex_X : IComparer<VertexKDTree>
    {

        public int Compare(VertexKDTree a, VertexKDTree b)
        {
            if (a.Vector.X > b.Vector.X)
                return 1;
            return -1;
        }
    }
    public class VectorY : IComparer<Vector3>
    {

        public int Compare(Vector3 a, Vector3 b)
        {
            if (a.Y > b.Y)
                return 1;
            return -1;
        }
    }
    public class VectorZ : IComparer<Vector3>
    {

        public int Compare(Vector3 a, Vector3 b)
        {
            if (a.Z > b.Z)
                return 1;
            return -1;
        }
    }
    public class NaturalStringComparer : IComparer<string>
    {
        private static readonly Regex _re = new Regex(@"(?<=\D)(?=\d)|(?<=\d)(?=\D)", RegexOptions.Compiled);

        public int Compare(string x, string y)
        {
            x = x.ToLower();
            y = y.ToLower();
            if (string.Compare(x, 0, y, 0, Math.Min(x.Length, y.Length)) == 0)
            {
                if (x.Length == y.Length) return 0;
                return x.Length<y.Length? -1 : 1;
            }
            var a = _re.Split(x);
            var b = _re.Split(y);
            int i = 0;
            while (true)
            {
                int r = PartCompare(a[i], b[i]);
                if (r != 0) return r;
                ++i;
            }
        }

        private static int PartCompare(string x, string y)
        {
            int a, b;
            if (int.TryParse(x, out a) && int.TryParse(y, out b))
                return a.CompareTo(b);
            return x.CompareTo(y);
        }
     }
}
