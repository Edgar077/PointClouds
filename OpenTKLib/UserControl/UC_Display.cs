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

      
        private void initGLControl()
        {

            this.glControl1.BackColor = System.Drawing.Color.Black;

            this.glControl1.Name = "glControl1";
            this.glControl1.VSync = false;
            this.glControl1.Top = 0;
            this.glControl1.Left = 0;
            this.glControl1.Width = this.Width;
            this.glControl1.Height = this.Height;
            this.glControl1.Cursor = Cursors.Cross;

          

            int i = 0;
            while (i <= 100)
            {
                this.comboTransparency.Items.Add((object)i.ToString());
                i += 10;
            }

          
            this.comboModels.Items.Add("All");
            this.comboCameraModel.Items.Add("Camera");
            this.comboCameraModel.Items.Add("Model");
           
            this.comboCameraModel.SelectedIndex = 0;
            this.comboModels.SelectedIndex = 0;



            InitialSettingsOnLoad();

        }
        private void SetComboSelection(ToolStripComboBox combo, string selection)
        {
            for (int i = 0; i < combo.Items.Count; i++)
            {
                if (combo.Items[i].ToString() == selection)
                {
                    combo.SelectedIndex = i;
                    break;
                }
            }
        }
        private void InitialSettingsOnLoad()
        {
           
            this.comboTransparency.SelectedIndex = 0;


            if (GLSettings.ShowAxes)
                showAxesToolStripMenuItem.Text = "Hide Axis";
            else
                showAxesToolStripMenuItem.Text = "Show Axis";

            SetComboSelection(this.comboRenderMode, GLSettings.ViewMode);

        }
     
    }
}
