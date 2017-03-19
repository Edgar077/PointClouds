using System.Collections.Generic;
using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using OpenTK.Extension;

namespace OpenTK.Extension
{

    public class PointCloudRenderable : RenderableObject
    {

        public PointCloudRenderable(Vector3 mycolor)
        {
            color = mycolor;
            colorOutput = true;
        }
        public PointCloudRenderable() : this(new Vector3(1, 1, 1))
        {

            this.Position = Vector3.Zero;
            this.Scale = 1f;
        }
   

    }
}