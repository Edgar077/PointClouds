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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenTKExtension
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
            this.textBoxPointSize.Text = GLSettings.PointSize.ToString("0.00" );
            this.textBoxPointSizeAxis.Text = GLSettings.PointSizeAxis.ToString("0.00");
            this.checkBoxPointCloudCentered.Checked = GLSettings.PointCloudCentered;
            this.checkBoxBoundingBoxAt000.Checked = GLSettings.BoundingBoxLeftStartsAt000;

            this.checkBoxShowAxesLabels.Checked = GLSettings.ShowAxesLabels;
            this.checkBoxShowModelAxes.Checked = GLSettings.ShowModelAxes;
            checkBoxShowXYZAxes.Checked = GLSettings.ShowAxes;
            this.checkBoxCameraFOV.Checked = GLSettings.ShowCameraFOV;
          
            checkBoxShowPointCloudAsTexture.Checked = GLSettings.ShowPointCloudAsTexture;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            GLSettings.PointSize = Convert.ToSingle(this.textBoxPointSize.Text);
            GLSettings.PointSizeAxis = Convert.ToSingle(this.textBoxPointSizeAxis.Text);

            //different usages 
            OpenGLUC myControl = ParentGLControl as OpenGLUC;
            if (myControl != null)
            {
                myControl.OGLControl.GLrender.ResetPointLineSizes();
                myControl.Refresh();
            }
            OGLControl myOGLControl = ParentGLControl as OGLControl;
            if (myOGLControl != null)
            {
                myOGLControl.GLrender.ResetPointLineSizes();
                myOGLControl.Update();
            }

            this.Close();
        }

        private void buttonColorModels_Click(object sender, EventArgs e)
        {
            ColorDialog dialogColor = new ColorDialog();
            
            // Update the text box color if the user clicks OK 
            if (dialogColor.ShowDialog() == DialogResult.OK)
            {
                GLSettings.ColorModels = dialogColor.Color;
                OpenGLUC myControl = ParentGLControl as OpenGLUC;
                if(myControl != null)
                    myControl.ChangeAllModelsColor(dialogColor.Color);
           
                
            }
        }

        private void buttonColorModel_Click(object sender, EventArgs e)
        {
            ColorDialog dialogColor = new ColorDialog();
            if (dialogColor.ShowDialog() == DialogResult.OK)
            {
                
                OpenGLUC myControl = ParentGLControl as OpenGLUC;
                if(myControl != null)
                    myControl.ChangeModelColor(dialogColor.Color);
               

            }

           
          

        }

        private void buttonBackColor_Click(object sender, EventArgs e)
        {
            ColorDialog dialogColor = new ColorDialog();
            if (dialogColor.ShowDialog() == DialogResult.OK)
            {
                GLSettings.BackColor = dialogColor.Color;
                OpenGLUC myControl = ParentGLControl as OpenGLUC;
                if (myControl != null)
                    myControl.Refresh();
                

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

        private void checkBoxCameraFOV_CheckedChanged(object sender, EventArgs e)
        {
            GLSettings.ShowCameraFOV = checkBoxCameraFOV.Checked;

        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            OpenGLUC myControl = ParentGLControl as OpenGLUC;
            if (myControl != null)
            {
                myControl.OGLControl.GLrender.ResetPointLineSizes();
                myControl.Refresh();
            }
        }

        private void checkBoxBoundingBoxAt000_CheckedChanged(object sender, EventArgs e)
        {
            GLSettings.BoundingBoxLeftStartsAt000 = checkBoxBoundingBoxAt000.Checked;

        }

        private void checkBoxShowPointCloudAsTexture_CheckedChanged(object sender, EventArgs e)
        {
            GLSettings.ShowPointCloudAsTexture = checkBoxShowPointCloudAsTexture.Checked ;
        }

        
    }
}
