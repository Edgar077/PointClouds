using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTKExtension;

namespace NLinear
{
   
    public class Box
    {
        public List<int> PointIndices1 = new List<int>();
        public List<int> PointIndices2 = new List<int>();
        public List<Vector3<float>> Vertices = new List<Vector3<float>>();

        public Box(List<Line3<float>> lineList)
        {
            Vertices.Add(new Vector3<float>(lineList[0].From));
            Vertices.Add(new Vector3<float>(lineList[0].To));

            PointIndices1.Add(0); PointIndices2.Add(1);
            for (int i = 1; i < lineList.Count; i++)
            {
                bool sign1 = false; bool sign2 = false;
                int a = -1; int b = -1;
                for (int j = 0; j < Vertices.Count; j++)
                {
                    Line3<float> line = lineList[i];
                    line.DistanceTo(Vertices[j], 1);

                    if (Vertices[j].DistanceTo(lineList[i].From, 1) < 0.0000001) { sign1 = true; a = j; }
                    if (Vertices[j].DistanceTo(lineList[i].To, 1) < 0.000001) { sign2 = true; b = j; }
                    if (sign1 && sign2) break;
                }
                if (sign1 == false) 
                { 
                    Vertices.Add(new Vector3<float>(lineList[i].From)); a = Vertices.Count - 1; 
                }
                if (sign2 == false) 
                {
                    Vertices.Add(new Vector3<float>(lineList[i].To)); b = Vertices.Count - 1; 
                }
                PointIndices1.Add(a);
                PointIndices2.Add(b);
            }
        }
    }
}
