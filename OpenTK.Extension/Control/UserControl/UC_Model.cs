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
 
 

namespace OpenTK.Extension
{
    public partial class OpenGLUserControl
    {
     
        public OpenFileDialog openModel;


        public void LoadModelFromFile(string fileName)
        {
            Model myModel = new Model(fileName);
            //PointCloudRenderable pcr = new PointCloudRenderable();
            //pcr.PointCloudGL = myModel.pointCloudGL;

            //this.glControl1.GLrender.ReplaceRenderableObject(pcr);

            this.glControl1.openGLContext.AddModel(myModel);

        }

        private void LoadFileDialog()
        {
            this.openModel = new OpenFileDialog();
            if (this.openModel.ShowDialog() != DialogResult.OK)
                return;
           
                LoadModelFromFile(this.openModel.FileName);

        }

   

        public void ShowModels()
        {
            //if (this.GLrender.Models3D == null)
            //    return;
            //if (this.GLrender.Models3D.Count > 1)
            //    this.modelOld = this.GLrender.Models3D[this.GLrender.Models3D.Count - 1];
            //this.RefreshView_MakeCurrent();

        }
   
        public void RemoveModel(int indModel)
        {
            //if (indModel < 0 || this.GLrender.Models3D[indModel].Name.StartsWith("|"))
            //    return;
            //for (int index = 0; index < this.GLrender.Models3D[indModel].Parts.Count; ++index)
            //    GL.DeleteLists(this.GLrender.Models3D[indModel].Parts[index].GLListNumber, 1);
            //lock (this.GLrender.Models3D)
            //    this.GLrender.RemoveModel(indModel);
            //this.comboModels.Items.RemoveAt(indModel);
           
            //if(comboModels.Items.Count > 0)
            //    comboModels.SelectedIndex = 0;
            //GC.Collect();

            //if(GLrender.Models3D.Count > 0)
            //{
            //    Model3D model3D = GLrender.Models3D[0];
            //    ShowModels();
            //    GLrender.SelectModel(0);
            //}
            //this.glControl1.Refresh();
        }
        private void WriteModelName(int modelInd)
        {
           // this.comboModels.Items.Add((object)this.GLrender.Models3D[modelInd].Name);
        }

  

    

        private void loadFile()
        {
            //Model3D model3D;
            //for (int index = 0; index < this.openModel.FileNames.Length; ++index)
            //{
            //    string errorText = string.Empty;
            //    model3D = this.GLrender.LoadModel(this.openModel.FileNames[index], errorText);
            //    if (model3D != null)
            //    {
            //        AddModel(model3D);
            //        ShowModels();

            //    }
            //}
        }
       


    }
}
