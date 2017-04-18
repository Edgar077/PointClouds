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
using OpenTKExtension.Properties;

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
 
 

namespace OpenTKExtension
{
    public partial class OpenGLUC
    {
        
        public void ChangeModelColor(Color c)
        {
           
            if (comboModels.SelectedIndex >= 0)
            {
                this.OGLControl.GLrender.SelectedModelIndex = comboModels.SelectedIndex;
                SetColorOfModel(this.OGLControl.GLrender.SelectedModelIndex, c.R, c.G, c.B, c.A);

            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please load a 3D object first");

            }
            this.OGLControl.Refresh();

          
        }
        public void ChangeAllModelsColor(Color c)
        {
            for (int i = 0; i < this.OGLControl.GLrender.RenderableObjects.Count; i++ )
            {
                SetColorOfModel(i, c.R, c.G, c.B, c.A);
            }

            this.OGLControl.Refresh();

          
        }
        private void SetColorOfModel(int iModel, byte r, byte g, byte b, byte a)
        {
            Vector3 colorVector = new Vector3(r / 255f, g / 255f, b / 255f);

            RenderableObject pc = this.OGLControl.GLrender.RenderableObjects[iModel];
            pc.PointCloud.SetColor(colorVector);

          

        }
        public override void Refresh()
        {
            this.OGLControl.GLrender.Refresh();
            base.Refresh();
        }
       

       

      
        
    
    }
}
