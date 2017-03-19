
using System.Windows;
using MIConvexHull;
using System.Windows.Media;
using System.Collections.Generic;

namespace OpenTKExtension
{

    /// <summary>
    /// 
    /// </summary>
    public class Vertex2D : List<float>, IVector
    {
        //public float[] Position { get; set; }
        public int IndexInModel;
        float[] position = new float[2];

        public Vertex2D(float x, float y)
        {
            position = new float[] { x, y };
        }
        public Vertex2D(int indexInModel, float x, float y)
        {
            position = new float[] { x, y };
            IndexInModel = indexInModel;
        }
        

        public new float this[int index]
        {
            get
            {
                return position[index];
            }
            set
            {
                position[index] = value;
            }
        }
        public float[] PositionArray
        {
            get
            {
                return this.position;
            }
            set
            {
                this.position = value;
            }
        }
        
    }
}
