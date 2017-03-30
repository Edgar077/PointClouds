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


namespace OpenTKExtension
{
    public class OGLControl : OpenTK.GLControl
    {
        public OpenGLContext GLrender;
        public ModelViewMode ModelViewMode = ModelViewMode.ReadOnly;
       
        private bool mouseClicked = false;
        ContextMenuStrip menu;



        string path = AppDomain.CurrentDomain.BaseDirectory;
       

        float timeAnimation = 0.0f;
        private float animationAngle = 0;

        private float rotationSpeed = 1e-3f;
        private float translationSpeed = 1e-4f;

        private float scrollSpeed = 1e-3f;
        private delegate void AnimateDelegate();


        System.Timers.Timer timer = new System.Timers.Timer();
        bool animationStarted;
       
        public OGLControl() : base()
        {
         
            try
            {
                if (!GLSettings.IsInitializedFromSettings)
                    GLSettings.InitFromSettings();

               

                BackColor = GLSettings.BackColor;
                CreateMenu();
                GLrender = new OpenGLContext(this);
                System.Diagnostics.Debug.WriteLine("====> ThreadID OpenControl : " + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
                timer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Tick);


                this.ModelViewMode = ModelViewMode.Camera;
                
            }
            catch(Exception err)
            {
                System.Windows.Forms.MessageBox.Show("Error in OGLControl constructur : " + err.Message);

            }
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
            //System.Diagnostics.Debug.WriteLine("->Load");
            //to not be called during design time
            //            if (!DesignMode)
            if (!GLSettings.DesignMode)
            {
                System.Diagnostics.Debug.WriteLine("--> OpenGL Version, renderer: " + GL.GetString(StringName.Version) + " : " + GL.GetString(StringName.Renderer));

                //GL.ShadeModel(ShadingModel.Flat);
                //GL.
                GLrender.InitDefaults();

                //initialize all objects added before GL was initialized
                for (int i = 0; i < this.GLrender.RenderableObjects.Count; i++)
                {
                    this.GLrender.RenderableObjects[i].InitializeGL();
                }
                SetupViewport();
                GL.ClearColor(Color.MidnightBlue);

                


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
            try
            {
                this.GLrender.GLContextInitialized = false;
                GLrender.Dispose();
                base.OnHandleDestroyed(e);
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine("Error distroying OpenGL Handle: " + err.Message);
            }
        }


        protected override void OnResize(EventArgs e)
        {
            if (!GLSettings.DesignMode)
            //if (!DesignMode)
            {
                //sometimes this is called during constructor
                if (GLrender != null)
                {
                    this.GLrender.Camera.PerspectiveUpdate(this.Width, this.Height);
                    //program["projection_matrix"].SetValue(p);

                    if (this.GLrender.GLContextInitialized)
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
            }

            base.OnResize(e);
        }

        private void Animate()
        {
            timeAnimation += (float)DateTime.Now.Millisecond;


            this.GLrender.RotationForAnimation = new Vector3(animationAngle, animationAngle, 0f);

            this.Refresh();

            //GL.UniformMatrix4(modelviewMatrixLocation, false, ref MV);

            //var keyboard = OpenTK.Input.Keyboard.GetState();
            //if (keyboard[OpenTK.Input.Key.Escape])


        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            this.mouseClicked = false;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {

            //System.Diagnostics.Debug.WriteLine("Mouse Down");
            //--------------
            if (e.Button == MouseButtons.Left && !this.mouseClicked)
            {
                //System.Diagnostics.Debug.WriteLine(" ->Clicked Left");
                this.mouseClicked = true;
                this.GLrender.Camera.XTrans = e.X;
                this.GLrender.Camera.YTrans = e.Y;
             

            }
            if (e.Button == MouseButtons.Right && !this.mouseClicked)
            {
                //System.Diagnostics.Debug.WriteLine(" ->Clicked Right");
                this.mouseClicked = true;

                this.GLrender.Camera.XRot = e.X;
                this.GLrender.Camera.YRot = e.Y;

               
            }


        }


        protected override void OnMouseMove(MouseEventArgs e)
        {
            //left mouse: pan
            if (e.Button == MouseButtons.Left && this.mouseClicked)
            {
                if (e.X == this.GLrender.Camera.XTrans && e.Y == this.GLrender.Camera.YTrans)
                    return;

                float yaw = -(this.GLrender.Camera.XTrans - e.X);
                float pitch = -(this.GLrender.Camera.YTrans - e.Y);

                if (ModelViewMode == ModelViewMode.ModelMove)
                {

                    this.GLrender.TranslateModels(yaw * this.translationSpeed, pitch * this.translationSpeed, 0);

                }
                else
                {

                    this.GLrender.Camera.Yaw(yaw);
                    this.GLrender.Camera.Pitch(pitch);


                }

                this.GLrender.Camera.XTrans = e.X;
                this.GLrender.Camera.YTrans = e.Y;

              
                this.Refresh();
            }


                //right button: rotate
            else if (e.Button == MouseButtons.Right && this.mouseClicked)
            {
                if (e.X == this.GLrender.Camera.XRot && e.Y == this.GLrender.Camera.YRot)
                    return;

                float yangle = (e.X - this.GLrender.Camera.XRot) * rotationSpeed;
                float xAngle = (e.Y - this.GLrender.Camera.YRot) * rotationSpeed;
                //System.Diagnostics.Debug.WriteLine("-> Right Mouse move");

                if (ModelViewMode == ModelViewMode.ModelMove)
                {

                    this.GLrender.RotateModels(-xAngle, -yangle);

                }
                else
                {

                    this.GLrender.Camera.Yangle += yangle;
                    this.GLrender.Camera.Xangle += xAngle;

                }


                this.GLrender.Camera.XRot = e.X;
                this.GLrender.Camera.YRot = e.Y;

                this.Refresh();


            }


        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.GLrender.AlignCameraToObject = true;
                this.GLrender.Draw();

            }
            base.OnMouseDoubleClick(e);
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {


            if (e.KeyCode == Keys.ControlKey)
            {
                if (this.mouseClicked)
                {
                    menu.Left = this.GLrender.Camera.XTrans;
                    menu.Top = this.GLrender.Camera.YTrans;

                    menu.Show();
                }
            }
            if (e.KeyCode == Keys.M)
            {
                ModelViewMode = ModelViewMode.ModelMove;

            }
            if (e.KeyCode == Keys.C)
            {
                ModelViewMode = ModelViewMode.Cut;

            }
            if (e.KeyCode == Keys.F)
            {
                ModelViewMode = ModelViewMode.ChangeFieldOfView;

            }

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
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.M)
            {
                ModelViewMode = ModelViewMode.ReadOnly;

            }
            if (e.KeyCode == Keys.C)
            {
                ModelViewMode = ModelViewMode.ReadOnly;

            }
            if (e.KeyCode == Keys.F)
            {
                ModelViewMode = ModelViewMode.ReadOnly;

            }
            if (e.KeyCode == Keys.Y)
            {

                this.GLrender.CutUndo();
                this.Invalidate();
                this.Refresh();

            }
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
            try
            {
                this.MakeCurrent();//this line is essential if more than one control is used!!
                //if the line is missing: If the second control is disposed, error that no Graphic Context available - on first control
            }
            catch { }
            //if (!DesignMode) - this does not work
            if (!GLSettings.DesignMode)
            {
                base.OnPaint(e);

                this.GLrender.Draw();

            }

        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {

            MouseWheelActions(e);



        }
        public void MouseWheelActions(MouseEventArgs e)
        {
            float v = Convert.ToSingle(e.Delta * scrollSpeed * this.GLrender.Camera.ZNear);

            if (this.ModelViewMode == ModelViewMode.ModelMove)
            {

                this.GLrender.TranslateModels(0, 0, Convert.ToSingle(v));
            }
            else if (this.ModelViewMode == ModelViewMode.Cut)
            {
                this.GLrender.CutModel(v);

            }
            else if (this.ModelViewMode == ModelViewMode.ChangeFieldOfView)
            {
                this.GLrender.ChangeFieldOfView(v);

            }
            else
            {
                this.GLrender.Camera.Move(new Vector3(0f, 0f, v));
            }
            this.Invalidate();
            this.Refresh();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            this.Focus();
            base.OnMouseClick(e);
        }
        public bool SaveSelectedModelAs()
        {

            SaveFileDialog sf = new SaveFileDialog();
            sf.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "Models";

            if (sf.ShowDialog() == DialogResult.OK)
            {
                string name = sf.FileName;

                if (IOUtils.ExtractExtension(sf.FileName) == null)
                    sf.FileName = sf.FileName + ".obj";


                if (this.GLrender.RenderableObjects.Count > 0)
                {
                    RenderableObject o = null;
                    o = this.GLrender.RenderableObjects[this.GLrender.SelectedModelIndex];

                    if (o != null)
                    {
                        PointCloud pc = o.PointCloud;
                        pc.ToObjFile(sf.FileName);

                    }

                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("please switch to 3D Tab if scanning is in progress - (nothing to save) ");
                    return false;
                }
            }
            return true;

        }
        public bool SaveSelectedModel()
        {


           
            if (this.GLrender.RenderableObjects.Count > 0)
            {
                RenderableObject o = null;
                o = this.GLrender.RenderableObjects[this.GLrender.SelectedModelIndex];

                if (o != null)
                {
                    PointCloud pc = o.PointCloud;
                    pc.ToObjFile(pc.FileNameLong);

                }

            }
            else
            {
                System.Windows.Forms.MessageBox.Show("please switch to 3D Tab if scanning is in progress - (nothing to save) ");
                return false;
            }
            return true;
        }
        public Matrix4 CalculateRegistrationMatrix1_2()
        {
            //Vector3 v = new Vector3();
            Matrix4 myMatrix = Matrix4.Identity;

            if (this.GLrender.RenderableObjects.Count > 1)
            {
                RenderableObject o = this.GLrender.RenderableObjects[0];
                PointCloud pc1 = o.PointCloud;
                o = this.GLrender.RenderableObjects[1];
                PointCloud pc2 = o.PointCloud;

                myMatrix = SVD_Float.FindTransformationMatrix(pc1, pc2, ICP_VersionUsed.Zinsser);

                //myMatrix = SVD.FindTransformationMatrix_WithoutCentroids(v.ArrayVector3ToList(pc1.Vectors), v.ArrayVector3ToList(pc2.Vectors), ICP_VersionUsed.Scaling_Zinsser);
                    


            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please load two Point Clouds first");
                
            }
            return myMatrix;
        }
        public Matrix4 CalculateRegistrationMatrix(PointCloud pcOriginal)
        {
            //Vector3 v = new Vector3();
            Matrix4 myMatrix = Matrix4.Identity;

            if (this.GLrender.RenderableObjects.Count > 1)
            {
                RenderableObject o = this.GLrender.RenderableObjects[0];
                PointCloud pc1 = o.PointCloud;
                

                myMatrix = SVD_Float.FindTransformationMatrix(pcOriginal, pc1, ICP_VersionUsed.Zinsser);

                //myMatrix = SVD.FindTransformationMatrix_WithoutCentroids(v.ArrayVector3ToList(pc1.Vectors), v.ArrayVector3ToList(pc2.Vectors), ICP_VersionUsed.Scaling_Zinsser);



            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please load two Point Clouds first");

            }
            return myMatrix;
        }
        public bool AlignFirstModelFromRegistratioMatrix(Matrix4 myMatrix)
        {
            //Vector3 v = new Vector3();
          
            if (this.GLrender.RenderableObjects.Count > 0)
            {
                RenderableObject o = this.GLrender.RenderableObjects[0];
                PointCloud pc1 = o.PointCloud;
                myMatrix.TransformPointCloud(pc1);
                

            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please load two Point Clouds first");
                return false;
            }
            return true;
        }
        public override void Refresh()
        {
            this.GLrender.Refresh();
            base.Refresh();
        }
        protected override void Dispose(bool disposing)
        {
            if(GLrender != null)
                GLrender.Dispose();
            base.Dispose(disposing);
        }
   
        
    }
    public enum ModelViewMode
    {
        Cut,
        ModelMove,
        Camera,
        ChangeFieldOfView,
        ReadOnly
    }
}
