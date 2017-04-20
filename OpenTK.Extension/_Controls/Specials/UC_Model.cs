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

        public OpenFileDialog openPointCloud;


        //public void ShowModel(PointCloud myModel)
        //{
        //    if (myPointCloud != null)
        //    {
        //        this.comboModels.Items.Add(myModel.Name);
        //        //this.comboModels.SelectedIndex = this.comboModels.Items.Count - 1;

        //        this.OGLControl.GLrender.SelectedModelIndex = this.comboModels.Items.Count - 2;
        //        this.glControl1.GLrender.AddModel(myModel);
        //    }

        //}
      
        public void ShowPointCloud(PointCloud pc)
        {
           

            if (pc != null)
            {
                this.comboModels.Items.Add(pc.Name);
                this.OGLControl.GLrender.SelectedModelIndex = this.comboModels.Items.Count - 2;
                this.glControl1.GLrender.AddPointCloud(pc);
            }

        }
        public void ShowPointCloud_ClearAllOthers(PointCloud pc)
        {
            if (pc != null)
            {
                this.comboModels.Items.Clear();
                PointCloudRenderable ro = new PointCloudRenderable();
                ro.PointCloud = pc;

                this.OGLControl.GLrender.ReplaceRenderableObject(ro, true);


                this.comboModels.Items.Add(pc.Name);
                if(this.comboModels.Items.Count > 1)
                    this.OGLControl.GLrender.SelectedModelIndex = this.comboModels.Items.Count - 2;
                else
                    this.OGLControl.GLrender.SelectedModelIndex = 0;

            }

        }
        public void ClearAll()
        {
            this.comboModels.Items.Clear();
            this.OGLControl.GLrender.ClearAllObjects();


        }
        public void ShowRenderableObject(RenderableObject ro)
        {
           

            if (ro.PointCloud != null)
            {
                this.comboModels.Items.Add(ro.PointCloud.Name);
                this.OGLControl.GLrender.SelectedModelIndex = this.comboModels.Items.Count - 2;
                this.glControl1.GLrender.AddPointCloud(ro.PointCloud);
            }

        }
        private string LoadFileDialog()
        {
            this.openPointCloud = new OpenFileDialog();
            if (this.openPointCloud.ShowDialog() != DialogResult.OK)
                return string.Empty;

            return this.openPointCloud.FileName;
            
        }

     
   

    }
}
