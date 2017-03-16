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
using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenTK.Extension
{
    public partial class SettingsForm : Form
    {
        //public OpenGLControl ParentGLControl;
        public System.Windows.Forms.Control ParentGLControl;

        public SettingsForm(System.Windows.Forms.Control myParent)
        //public SettingsForm(OpenGLControl myParent)
        {
            this.ParentGLControl = myParent;
            InitializeComponent();
            this.textBoxPointSize.Text = GLSettings.PointSize.ToString();
            this.textBoxPointSizeAxis.Text = GLSettings.PointSizeAxis.ToString();
            //this.textBoxPointSize.Text = GLSettings.PointSize.ToString("0.00");
            //this.textBoxPointSizeAxis.Text = GLSettings.PointSizeAxis.ToString("0.00");

            this.checkBoxPointCloudCentered.Checked = GLSettings.PointCloudCentered;
            this.checkBoxShowAxesLabels.Checked = GLSettings.ShowAxesLabels;
            this.checkBoxShowModelAxes.Checked = GLSettings.ShowModelAxes;
            checkBoxShowXYZAxes.Checked = GLSettings.ShowAxes;
            this.checkBoxCull.Checked = GLSettings.OpenGL_FaceCull;
            checkBoxLighting.Checked = GLSettings.Lighting;


          
        }
        private void OKButton()
        {
            GLSettings.PointSize = Convert.ToInt32(this.textBoxPointSize.Text);
            GLSettings.PointSizeAxis = Convert.ToInt32(this.textBoxPointSizeAxis.Text);

            OpenGLUserControl myOldControl = ParentGLControl as OpenGLUserControl;
            if(myOldControl != null)
                myOldControl.RedrawAllModels(true);
            
            this.Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            OKButton();

        }

        private void buttonColorModels_Click(object sender, EventArgs e)
        {
            ColorDialog dialogColor = new ColorDialog();
            
            // Update the text box color if the user clicks OK 
            if (dialogColor.ShowDialog() == DialogResult.OK)
            {
                GLSettings.ColorModels = dialogColor.Color;
                OpenGLUserControl myOldControl = ParentGLControl as OpenGLUserControl;
                myOldControl.Invalidate();
                
            }
        }

        private void buttonColorBackground_Click(object sender, EventArgs e)
        {
            ColorDialog dialogColor = new ColorDialog();
            if (dialogColor.ShowDialog() == DialogResult.OK)
            {
                GLSettings.ColorModels = dialogColor.Color;
                OpenGLUserControl myOldControl = ParentGLControl as OpenGLUserControl;
                myOldControl.Invalidate();

            }
          

        }

        private void buttonBackColor_Click(object sender, EventArgs e)
        {
            ColorDialog dialogColor = new ColorDialog();
            if (dialogColor.ShowDialog() == DialogResult.OK)
            {
                GLSettings.BackColor = dialogColor.Color;
                OpenGLUserControl myOldControl = ParentGLControl as OpenGLUserControl;
                myOldControl.Invalidate();

            }
           
        }

        private void checkBoxPointCloudCentered_CheckedChanged(object sender, EventArgs e)
        {

            GLSettings.PointCloudCentered = checkBoxPointCloudCentered.Checked;
        }

        private void checkBoxShowModelAxes_CheckedChanged(object sender, EventArgs e)
        {
            GLSettings.ShowModelAxes = checkBoxShowModelAxes.Checked;
        }

        private void checkBoxShowAxesLabels_CheckedChanged(object sender, EventArgs e)
        {
            GLSettings.ShowAxesLabels = checkBoxShowAxesLabels.Checked;
        }

        private void checkBoxShowXYZAxes_CheckedChanged(object sender, EventArgs e)
        {
            GLSettings.ShowAxes = checkBoxShowXYZAxes.Checked;
        }

        private void checkBoxCull_CheckedChanged(object sender, EventArgs e)
        {
            GLSettings.OpenGL_FaceCull = checkBoxCull.Checked;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OKButton();
        }

        private void checkBoxLighting_CheckedChanged(object sender, EventArgs e)
        {
            GLSettings.Lighting = checkBoxLighting.Checked;
        }


      

        
    }
}
