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
 
 
using OpenTK;
namespace OpenTK.Extension
{
    public partial class OpenGLUserControl
    {
      
   
      
        public void RemoveFirstModel(bool refresh)
        {
            //Model3D myModel = this.GLrender.Models3D[0];
            //for (int index = 0; index < myModel.Parts.Count; ++index)
            //    GL.DeleteLists(myModel.Parts[index].GLListNumber, 1);
            
            //lock (this.GLrender.Models3D)
            //    this.GLrender.RemoveModel(0);

            //if (refresh)
            //{
            //    GC.Collect();
            //    this.glControl1.Refresh();
            //}
        }
        public void RemoveAllModels()
        {
            //try
            //{
            //    for (int i = GLrender.Models3D.Count - 1; i >= 0; i--)
            //    {
            //        for (int index = 0; index < this.GLrender.Models3D[i].Parts.Count; ++index)
            //            GL.DeleteLists(this.GLrender.Models3D[i].Parts[index].GLListNumber, 1);

            //        lock (this.GLrender.Models3D)
            //            this.GLrender.RemoveModel(i);
            //        //this.comboModels.Items.RemoveAt(i);

            //        //if (comboModels.Items.Count > 0)
            //        //    comboModels.SelectedIndex = 0;

            //    }
            //    GC.Collect();
            //    this.glControl1.Refresh();
            //}
            //catch(Exception err)
            //{
            //    System.Diagnostics.Debug.WriteLine("!! Error in RemoveAllModels : " + err.Message);
            //   //throw new Exception(err.Message);
            //}
        }
        public void RemoveAllModelsSlow()
        {
            //for (int i = GLrender.Models3D.Count - 1; i >= 0; i--)
            //{
            //    RemoveModel(i);
            //}
        }
        public void OpenTwoTrialPointClouds()
        {


            //string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Models";
            //path = AppDomain.CurrentDomain.BaseDirectory + "Models";
            //string errorText = string.Empty;
            //string fileName = path + "\\KinectFace1.obj";
            //Model3D model = this.GLrender.LoadModel(fileName, errorText);
            //AddModel(model);
            //ShowModels();

            //fileName = path + "\\KinectFace2.obj";
            //model = this.GLrender.LoadModel(fileName, errorText);
            //AddModel(model);
            //ShowModels();


        }


    }
}
