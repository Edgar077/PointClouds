using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using OpenTKExtension;
using OpenTK;
using System.Drawing;
using System.Diagnostics;
using Microsoft.Kinect;
using System.Windows.Forms;

namespace PointCloudUtils
{
    public class OpenGLPart
    {
        public bool ShowingIn3DControl = false;
        private delegate void UpdateShowFormOpenGLDelegate();
    
        public IScanner parentScanner;
        Control parentControl;
        OGLControl openGLControl;


        public OpenGLPart(IScanner myparentScanner, Control myparentControl, OGLControl myopenGLControl)
        {
            parentScanner = myparentScanner;
            parentControl = myparentControl;
            openGLControl = myopenGLControl;
            

        }
        public void Start3DShow()
        {
            this.ShowingIn3DControl = true;
            if (this.openGLControl.GLrender.GLContextInitialized)
                Show3DInControl(true);

          
        }
        private void Show3DInControl(bool show)
        {
            if (show)
            {
                ShowingIn3DControl = true;
                

            }
            else
            {
                ShowingIn3DControl = false;


            }

            //if (show)
            //{
            //    ShowingIn3DControl = true;
            //    timer3DShowControl.Start();

            //    this.parentControl.BeginInvoke(new UpdateShowFormOpenGLDelegate(UpdateOpenGLControl));

            //}
            //else
            //{
            //    ShowingIn3DControl = false;
            //    timer3DShowControl.Stop();


            //}


        }
    
        public void Stop3dShow()
        {
            Show3DInControl(false);

            //ShowingIn3DControl = false;

            //if (ShowingIn3DControl)
            //{
                
            //}
        }

     


    }
}
