using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenTK.Extension
{

    public class OpenGLContext : IDisposable
    {
        //camera
        public Camera Camera;

        public OpenTK.Graphics.OpenGL.PrimitiveType RenderMode;
        public OpenTK.Graphics.OpenGL.PolygonMode FillMode;

        public OpenGLControl OpenGLControlInstance;
        public List<RenderableObject> RenderableObjects = new List<RenderableObject>();
        public bool GLContextInitialized;
        FramesPerSecond fpsCalc = new FramesPerSecond();

        Axes axes;
        Grid grid;



        private bool cameraIsAlignedToObject;
        //old..........
        public Vector3 RotationForAnimation = Vector3.Zero;


        OpenGLTextWriter openGLTextwriter;
        //float axisLength = 1f;

        public OpenGLContext(OpenGLControl myGLControl)
        {
            OpenGLControlInstance = myGLControl;
            Camera = new Camera();
            RenderMode = PrimitiveType.Points;
        }
        
        public void InitDefaults()
        {
            this.GLContextInitialized = true;
            this.Camera.PerspectiveUpdate(OpenGLControlInstance.Width, OpenGLControlInstance.Height);
            Camera.SetDirection(new Vector3(0, 0, -1));



            ResetSettings();

            //initialize axes, for possible later use
            axes = new Axes(1f);
            axes.FillPointCloud();
            axes.InitializeGL();

            grid = new Grid(20, 20);
            grid.FillPointCloud();
            grid.InitializeGL();


            //initialize text writer for axes labels
            openGLTextwriter = new OpenGLTextWriter();

        }
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        public void Dispose()
        {

            axes.Dispose();
            grid.Dispose();

            for (int i = 0; i < this.RenderableObjects.Count; i++)
            {
                RenderableObject o = this.RenderableObjects[i];
                o.Dispose();
            }
        }
        public void ResetSettings()
        {
            GL.LineWidth(GLSettings.PointSizeAxis);
            GL.PointSize(GLSettings.PointSize);
            //GL.DepthRange(.PointSize(GLSettings.PointSize);

        }
        public void ResetAll()
        {
            ResetSettings();
            for (int i = 0; i < this.RenderableObjects.Count; i++)
            {
                RenderableObject o = this.RenderableObjects[i];
                o.Reset();
            }

        }
        public void Draw()
        {
            fpsCalc.newFrame();
            UpdateFramesPerSecond();
            GL.Viewport(0, 0, OpenGLControlInstance.Width, OpenGLControlInstance.Height);
            GL.Clear(OpenTK.Graphics.OpenGL.ClearBufferMask.ColorBufferBit | OpenTK.Graphics.OpenGL.ClearBufferMask.DepthBufferBit);

            
           
            if(GLSettings.OpenGL_FaceCull)
                GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.CullFace);
            //GL.CullFace(CullFaceMode.Back);
            
            GL.ClearColor(GLSettings.BackColor);
            GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.DepthTest);
            //GL.Enable(EnableCap.DepthClamp);

            //GL.Enable(EnableCap.ColorMaterial);

            //GL.Disable(EnableCap.Blend);
            //GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);


            for (int i = 0; i < this.RenderableObjects.Count; i++)
            {

                RenderableObject o = this.RenderableObjects[i];



                if (!cameraIsAlignedToObject && i == 0)
                {
                    System.Diagnostics.Debug.WriteLine("----Align camera to object - on first draw");
                    //o.PointCloudGL.ResetCentroid(true);

                    this.Camera.Position = new Vector3(0f, 0f, o.PointCloudGL.BoundingBoxMaxFloat + 2 * this.Camera.zNear);

                    cameraIsAlignedToObject = true;

                }
                o.P = this.Camera.P;
                o.V = this.Camera.V;
                o.M = this.Camera.M;

                o.MVP = this.Camera.MVP;
                
                o.Scale = 1;

                
                o.Render(this.RenderMode, FillMode);


            }
            if (GLSettings.ShowAxes)
            {
                axes.P = this.Camera.P;
                axes.V = this.Camera.V;
                axes.M = this.Camera.M;
           
                axes.MVP = this.Camera.MVP;
                axes.Render(OpenTK.Graphics.OpenGL.PrimitiveType.Lines, OpenTK.Graphics.OpenGL.PolygonMode.Line);

            }
            if (GLSettings.ShowGrid)
            {
                grid.P = this.Camera.P;
                grid.V = this.Camera.V;
                grid.M = this.Camera.M;

                grid.MVP = this.Camera.MVP;
                grid.Render(OpenTK.Graphics.OpenGL.PrimitiveType.Lines, OpenTK.Graphics.OpenGL.PolygonMode.Line);

            }

            //this.DrawAxesLabels();


            this.OpenGLControlInstance.SwapBuffers();
        }


        public void AddRenderableObject(RenderableObject o)
        {

            RenderableObjects.Add(o);
            if (GLContextInitialized)
            {
                o.InitializeGL();
                UpdateControl();
            }


        }
        public void AddModel(Model myModel)
        {
            //PointCloudRenderable pcr = new PointCloudRenderable();
            //pcr.PointCloudGL = myModel.pointCloudGL;
            //pcr.CreateVBOs();



            RenderableObjects.Add(myModel.RenderableObject);
            if (GLContextInitialized)
            {
                myModel.RenderableObject.InitializeGL();
                UpdateControl();
            }


        }
        public void ReplaceModel(Model myModel)
        {
            //PointCloudRenderable pcr = new PointCloudRenderable();
            //pcr.PointCloudGL = myModel.pointCloudGL;
            ReplaceRenderableObject(myModel.RenderableObject);


        }
        private void ClearAll()
        {
            if (RenderableObjects.Count > 0)
            {
                for (int i = 0; i < RenderableObjects.Count; i++)
                {
                    RenderableObject obj = RenderableObjects[i];
                    obj.Dispose();
                }
                RenderableObjects.Clear();

            }
        }
        public void ClearAllObjects()
        {
            ClearAll();
            UpdateControl();
        }
        public void ReplaceRenderableObject(RenderableObject o)
        {
            try
            {

                ClearAll();
                RenderableObjects.Add(o);

                if (GLContextInitialized)
                {
                    //System.Diagnostics.Debug.WriteLine("--Replace renderable object");
                    o.InitializeGL();
                    UpdateControl();
                }
            }
            catch (Exception err)
            {
                System.Windows.Forms.MessageBox.Show("Error in OpenGLContext.ReplaceRenderableObject : " + err.Message);
            }

        }




        public void UpdateControl()
        {
            if (this.RenderableObjects.Count > 0) //&& this.GLContextInitialized
            {

                //not clear why this has to be called. If the line is missing, update is not done every time. If set, PainControl is called twice in some cases for the same object... 
                this.OpenGLControlInstance.Invalidate();

            }

        }

        private void DrawAxesLabels()
        {


            this.openGLTextwriter.DrawString("X", 1, 0, 0);

            this.openGLTextwriter.DrawString("Y", 0, 1, 0);


            this.openGLTextwriter.DrawString("Z", 0, 0, 1);


        }
        private void UpdateFramesPerSecond()
        {

            if (this.OpenGLControlInstance.Parent.Parent.Parent != null)
                this.OpenGLControlInstance.Parent.Parent.Parent.Text = "OpenTK Form " + FramesPerSecond;
        }

        public string FramesPerSecond
        {
            get
            {
                return String.Format("FPS: {0:0.00}", fpsCalc.AvgFramesPerSecond);


            }
        }


    }

    public enum RenderMode
    {
        Point,
        Lines,
        LineStrip,
        LinesAdjacency,
        Wireframe,
        Triangle,
        TriangleFan,
        TriangleStrip,
        Quads,
        QuadsStrip,
        Polygon,
        Patches


    }
}
