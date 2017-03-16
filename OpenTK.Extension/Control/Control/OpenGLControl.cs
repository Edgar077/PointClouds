using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;


namespace OpenTK.Extension
{
    public class OpenGLControl : OpenTK.GLControl
    {

        private bool mouseClicked = false;
        ContextMenuStrip menu;



        string path = AppDomain.CurrentDomain.BaseDirectory;
        public OpenGLContext openGLContext;

        float timeAnimation = 0.0f;
        private float animationAngle = 0;

        private float rotationSpeed = 1e-2f;
        private float scrollSpeed = 1e-3f;
        private delegate void AnimateDelegate();



        System.Timers.Timer timer = new System.Timers.Timer();
        bool animationStarted;

        public OpenGLControl()
            : base()
        {
            BackColor = Color.DarkBlue;
            CreateMenu();
            openGLContext = new OpenGLContext(this);
            System.Diagnostics.Debug.WriteLine("...ThreadID OpenTKFormSimple: " + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
            timer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Tick);

        }
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {

            this.openGLContext.Dispose();
            base.Dispose(disposing);
        }
        private void CreateMenu()
        {
            menu = new ContextMenuStrip();

            //ToolStripItem submenu = new ToolStripItem();
            //
            menu.Items.Add("Item");

        }

        protected override void OnLoad(EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("->Load");
            //to not be called during design time
            //            if (!DesignMode)
            if (!GLSettings.DesignMode)
            {


                openGLContext.InitDefaults();

                //initialize all objects added before GL was initialized
                for (int i = 0; i < this.openGLContext.RenderableObjects.Count; i++)
                {
                    this.openGLContext.RenderableObjects[i].InitializeGL();
                }
                SetupViewport();
                GL.ClearColor(Color.MidnightBlue);

                System.Diagnostics.Debug.WriteLine("--> OpenGL Version, renderer: " + GL.GetString(OpenTK.Graphics.OpenGL.StringName.Version) + " : " + GL.GetString(OpenTK.Graphics.OpenGL.StringName.Renderer));


            }


        }
        public void StartAnimationTimer()
        {
            if (!animationStarted)
            {

                animationStarted = true;
                this.timer.Enabled = true;
                this.timer.Interval = 200;
                this.timer.Start();
            }
            else
            {
                animationStarted = false;
                this.timer.Stop();
            }


        }

        protected override void OnHandleDestroyed(EventArgs e)
        {

            this.openGLContext.GLContextInitialized = false;
            base.OnHandleDestroyed(e);
        }


        protected override void OnResize(EventArgs e)
        {
            if (!GLSettings.DesignMode)
            //if (!DesignMode)
            {
                this.openGLContext.Camera.PerspectiveUpdate(this.Width, this.Height);
                //program["projection_matrix"].SetValue(p);

                if (this.openGLContext.GLContextInitialized)
                {

                    if (this.Width < 0)
                        this.Width = 1;
                    if (this.Height < 0)
                        this.Height = 1;
                    this.MakeCurrent();
                    GL.Viewport(0, 0, this.Width, this.Height);
                    this.Invalidate();


                }
            }

            base.OnResize(e);
        }

        private void Animate()
        {
            timeAnimation += (float)DateTime.Now.Millisecond;


            this.openGLContext.RotationForAnimation = new Vector3(animationAngle, animationAngle, 0f);


            this.Refresh();

            //GL.UniformMatrix4(modelviewMatrixLocation, false, ref MV);

            //var keyboard = OpenTK.Input.Keyboard.GetState();
            //if (keyboard[OpenTK.Input.Key.Escape])


        }


        protected override void OnMouseDown(MouseEventArgs e)
        {

            System.Diagnostics.Debug.WriteLine("Mouse Down");
            //--------------
            if (e.Button == MouseButtons.Left && !this.mouseClicked)
            {
                System.Diagnostics.Debug.WriteLine(" ->Clicked Left");
                this.mouseClicked = true;
                this.openGLContext.Camera.XRot = this.openGLContext.Camera.XTrans = e.X;
                this.openGLContext.Camera.YRot = this.openGLContext.Camera.YTrans = e.Y;

                //mousePosOriginal.X = e.X;
                //mousePosOriginal.Y = e.Y;
            }
            if (e.Button == MouseButtons.Right && !this.mouseClicked)
            {
                System.Diagnostics.Debug.WriteLine(" ->Clicked Right");
                this.mouseClicked = true;

                this.openGLContext.Camera.XTrans = e.X;
                this.openGLContext.Camera.YTrans = e.Y;
            }




            //-----------------
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {

            this.mouseClicked = false;

        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("Mouse move");
            if (e.Button == MouseButtons.Right && this.mouseClicked)
            {
                //System.Diagnostics.Debug.WriteLine("-> Right Mouse move");
                if (e.X == this.openGLContext.Camera.XTrans && e.Y == this.openGLContext.Camera.YTrans)
                    return;
                this.openGLContext.Camera.Yangle += (e.X - this.openGLContext.Camera.XTrans) * rotationSpeed;
                this.openGLContext.Camera.Xangle += (e.Y - this.openGLContext.Camera.YTrans) * rotationSpeed;


                this.openGLContext.Camera.XTrans = e.X;
                this.openGLContext.Camera.YTrans = e.Y;

                this.Refresh();


            }
            else if (e.Button == MouseButtons.Left && this.mouseClicked)
            {
                //System.Diagnostics.Debug.WriteLine("-> Left Mouse move");
                if (e.X == this.openGLContext.Camera.XRot && e.Y == this.openGLContext.Camera.YRot) return;


                float yaw = -(this.openGLContext.Camera.XRot - e.X);
                this.openGLContext.Camera.Yaw(yaw);

                float pitch = -(this.openGLContext.Camera.YRot - e.Y);
                this.openGLContext.Camera.Pitch(pitch);

                this.openGLContext.Camera.XRot = e.X;
                this.openGLContext.Camera.YRot = e.Y;

                this.openGLContext.Camera.Recalculate = true;

                this.Refresh();

            }


        }


        protected override void OnKeyDown(KeyEventArgs e)
        {


            if (e.KeyCode == Keys.ControlKey)
            {
                if (this.mouseClicked)
                {
                    menu.Left = this.openGLContext.Camera.XTrans;
                    menu.Top = this.openGLContext.Camera.YTrans;

                    menu.Show();
                }
            }

            //System.Diagnostics.Debug.WriteLine("Key Down: ");

            //bool flag = false;
            //if (e.KeyCode == Keys.W)
            //{
            //    this.GLrender.Camera.CenterOfInterest -= (float)this.GLrender.Camera.zFar * 0.002f * this.GLrender.Camera.X;
            //    this.GLrender.Camera.Position -= (float)this.GLrender.Camera.zFar * 0.002f * this.GLrender.Camera.X;
            //    flag = true;
            //}
            //if (e.KeyCode == Keys.S)
            //{
            //    this.GLrender.Camera.CenterOfInterest += (float)this.GLrender.Camera.zFar * 0.002f * this.GLrender.Camera.X;
            //    this.GLrender.Camera.Position += (float)this.GLrender.Camera.zFar * 0.002f * this.GLrender.Camera.X;
            //    flag = true;
            //}
            //if (e.KeyCode == Keys.A)
            //{
            //    this.GLrender.Camera.CenterOfInterest -= (float)this.GLrender.Camera.zFar * 0.001f * this.GLrender.Camera.Z;
            //    this.GLrender.Camera.Position -= (float)this.GLrender.Camera.zFar * 0.001f * this.GLrender.Camera.Z;
            //    flag = true;
            //}
            //if (e.KeyCode == Keys.D)
            //{
            //    this.GLrender.Camera.CenterOfInterest += (float)this.GLrender.Camera.zFar * 0.001f * this.GLrender.Camera.Z;
            //    this.GLrender.Camera.Position += (float)this.GLrender.Camera.zFar * 0.001f * this.GLrender.Camera.Z;
            //    flag = true;
            //}
            //float num1 = Convert.ToSingle(Math.Cos(0.01f));
            //float num2 = Convert.ToSingle(Math.Sin(0.01f));
            //if (e.KeyCode == Keys.NumPad4)
            //{
            //    Vector3 vector1 = new Vector3(this.GLrender.Camera.X);
            //    Vector3 vector2 = new Vector3(this.GLrender.Camera.Z);
            //    this.GLrender.Camera.X = num1 * vector1 + num2 * vector2;
            //    this.GLrender.Camera.Z = -num2 * vector1 + num1 * vector2;
            //    this.GLrender.Camera.CenterOfInterest = this.GLrender.Camera.Position - this.GLrender.Camera.X * this.GLrender.Camera.distEye;
            //    flag = true;
            //}
            //if (e.KeyCode == Keys.NumPad6)
            //{
            //    Vector3 vector1 = new Vector3(this.GLrender.Camera.X);
            //    Vector3 vector2 = new Vector3(this.GLrender.Camera.Z);
            //    this.GLrender.Camera.X = num1 * vector1 - num2 * vector2;
            //    this.GLrender.Camera.Z = num2 * vector1 + num1 * vector2;
            //    this.GLrender.Camera.CenterOfInterest = this.GLrender.Camera.Position - this.GLrender.Camera.X * this.GLrender.Camera.distEye;
            //    flag = true;
            //}
            //if (e.KeyCode == Keys.NumPad2)
            //{
            //    Vector3 vector1 = new Vector3(this.GLrender.Camera.X);
            //    Vector3 vector2 = new Vector3(this.GLrender.Camera.Y);
            //    this.GLrender.Camera.X = num1 * vector1 + num2 * vector2;
            //    this.GLrender.Camera.Y = -num2 * vector1 + num1 * vector2;
            //    this.GLrender.Camera.CenterOfInterest = this.GLrender.Camera.Position - this.GLrender.Camera.X * this.GLrender.Camera.distEye;
            //    flag = true;
            //}
            //if (e.KeyCode == Keys.NumPad8)
            //{
            //    Vector3 vector1 = new Vector3(this.GLrender.Camera.X);
            //    Vector3 vector2 = new Vector3(this.GLrender.Camera.Y);
            //    this.GLrender.Camera.X = num1 * vector1 - num2 * vector2;
            //    this.GLrender.Camera.Y = num2 * vector1 + num1 * vector2;
            //    this.GLrender.Camera.CenterOfInterest = this.GLrender.Camera.Position - this.GLrender.Camera.X * this.GLrender.Camera.distEye;
            //    flag = true;
            //}
            //if (!flag)
            //    return;
            //this.Invalidate();
            base.OnKeyDown(e);
        }


        private void SetupViewport()
        {
            GL.Viewport(this.ClientRectangle);//sets Viewport to the client rectangle

            int w = Width;
            int h = Height;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            //GL.Ortho(0, w, 0, h, -1, 1); // Bottom-left corner pixel has coordinate (0, 0)
            // GL.Ortho(-w / 2, w / 2, -h / 2, h / 2, -50, this.GLrender.Camera.zFar); // coordinate (0, 0) is in the middle


        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            animationAngle += 0.01f;
            if (animationAngle > float.MaxValue)
                animationAngle = 0f;
            //System.Diagnostics.Debug.WriteLine("Timer_Tick: ");
            this.BeginInvoke(new AnimateDelegate(Animate));


        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);


            if (!DesignMode)
            {

                this.openGLContext.Draw();

            }

        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {

            MouseWheelActions(e);



        }
        public void MouseWheelActions(MouseEventArgs e)
        {
            float v = Convert.ToSingle(e.Delta * scrollSpeed);
            this.openGLContext.Camera.Move(new Vector3(0f, 0f, v));

            this.Invalidate();
            this.Refresh();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            this.Focus();
            base.OnMouseClick(e);
        }
    }
}
