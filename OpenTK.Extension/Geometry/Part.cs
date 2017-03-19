// Pogramming by
//     Douglas Andrade ( http://www.cmsoft.com.br, email: cmsoft@cmsoft.com.br)
//               Implementation of most of the functionality
//     Edgar Maass: (email: maass@logisel.de)
//               Code adaption, changed to user control
//
//Software used: 
//    OpenGL : http://www.opengl.org
//    OpenTK : http://www.opentk.com
//
// DISCLAIMER: Users rely upon this software at their own risk, and assume the responsibility for the results. Should this software or program prove defective, 
// users assume the cost of all losses, including, but not limited to, any necessary servicing, repair or correction. In no event shall the developers or any person 
// be liable for any loss, expense or damage, of any type or nature arising out of the use of, or inability to use this software or program, including, but not
// limited to, claims, suits or causes of action involving alleged infringement of copyrights, patents, trademarks, trade secrets, or unfair competition. 
//


using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Reflection;
using OpenTK;

namespace OpenTKExtension
{
 
    /// <summary>
    /// a part of a 3D model
    /// </summary>
    public class Part
    {
        public string Name;
        
        public System.Drawing.Color ColorOverall;
        public float Transparency;
        public bool Selected;
        public int GLListNumber;
        public int[] GLBuffers;
        
        private int gLNumElements;

        public Part(Part p)
        {
            
            this.Name = p.Name;
            this.ColorOverall = System.Drawing.Color.White;

            this.Transparency = p.Transparency;
            this.Selected = p.Selected;
            this.GLListNumber = p.GLListNumber;
        }

        public Part()
        {
            this.Name = "";
            this.ColorOverall = System.Drawing.Color.White;
            this.Transparency = 0.0f;
            this.Selected = false;
            this.GLListNumber = -1;
        }
        public int GLNumElements
        {
            get
            {
                //if (gLNumElements == 0)
                //{
                //    if(Triangles != null)
                //    {
                //        gLNumElements = Triangles.Count;
                //    }
                    
                //}
                return gLNumElements;
            }
            set
            {
                gLNumElements = value;
            }
        }
    }



}
