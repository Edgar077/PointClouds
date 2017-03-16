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
using OpenTK.Extension.Properties;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
 
 

namespace OpenTK.Extension
{
    public partial class OpenGLUserControl
    {
        //Color BackColor;

     
        public void ChangeBackColor(Color color)
        {

            //GLSettings.BackColor = color;
            
            //this.BackColor = color;
            //this.GLrender.BackColor[0] = color.R;
            //this.GLrender.BackColor[1] = color.G;
            //this.GLrender.BackColor[2] = color.B;
            //    for (int index = 0; index < 3; ++index)
            //        this.GLrender.BackColor[index] /= (float)byte.MaxValue;
            //    this.glControl1.Invalidate();
            
        }
     
        public void ChangeModelColor()
        {
            //if (comboModels.SelectedIndex >= 0)
            //{
            //    ColorDialog colDiag = new ColorDialog();
            //    // Sets the initial color select to the current text color.

            //    // Update the text box color if the user clicks OK 
            //    if (colDiag.ShowDialog() == DialogResult.OK)
            //    {
            //        SetColorOfSelectedModel(colDiag.Color.R, colDiag.Color.G , colDiag.Color.B, colDiag.Color.A);
            //    }
            //}
            //else
            //{
            //    System.Windows.Forms.MessageBox.Show("Please load a 3D object first");

            //}
        }

     

        public void RedrawAllModels(bool resetCentroids)
        {
            //for (int i = 0; i < this.GLrender.Models3D.Count; i++)
            //{
            //    this.GLrender.Models3D[this.comboModels.SelectedIndex].ForceRedraw = true;
            //}
            //if(resetCentroids)
            //    GLrender.ResetAllModelsCentroid(GLSettings.PointCloudCentered);
            this.OpenGLControl.openGLContext.ResetAll();
            this.OpenGLControl.openGLContext.Draw();
            this.glControl1.Refresh();
        }

        public void ChangeColorOfModel(int modelIndex, int r, int g, int b)
        {

            //for (int index = 0; index < this.GLrender.Models3D[modelIndex].Parts.Count; ++index)
            //{
            //    this.GLrender.Models3D[modelIndex].Parts[index].ColorOverall = System.Drawing.Color.FromArgb(r, g, b);

            //}


            //RedrawAllModels(false);
        }

        public void ChangeColorOfModels(int r, int g, int b)
        {
            //for (int i = 0; i < this.GLrender.Models3D.Count; i++)
            //{
               
            //    ChangeColorOfModel(i , r, g, b);
          
                
            //}
            //RedrawAllModels(false);
        }

        
    
    }
}
