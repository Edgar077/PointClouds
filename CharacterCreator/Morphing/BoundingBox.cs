using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterCreator
{

    public class BoundingBox
    {
        public uint idx;
        public uint idx_x_max;
        public uint idx_y_max;
        public uint idx_z_max;
        public uint idx_x_min;
        public uint idx_y_min;
        public uint idx_z_min;
        public override string ToString()
        {

            return this.idx.ToString() + " :[ " + this.idx_x_max.ToString() + ":" + this.idx_y_max.ToString() + ":" + this.idx_z_max.ToString() + ":"
                + this.idx_x_min.ToString() + ":" + this.idx_y_min.ToString() + ":" + this.idx_z_min.ToString() + "]";
        }
    }
}
