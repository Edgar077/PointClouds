using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenTKExtension
{


    public class OpenGLContext : IDisposable 
    {
        //camera
        public Camera Camera;
        private bool disposed = false;

        public PrimitiveType PrimitiveTypes;
        public PolygonMode PoygonModes;

        public OGLControl GLControlInstance;
        public List<RenderableObject> RenderableObjects = new List<RenderableObject>();
        public bool GLContextInitialized;
        public int SelectedModelIndex;
        //Skeleton Skeleton;
        public List<RenderableObject> AdditionalObjectsToDraw = new List<RenderableObject>();
        FramesPerSecond fpsCalc = new FramesPerSecond();

        Axes axes;
        Grid grid;
        CameraFOV cameraFOV;



        private bool cameraIsAlignedToObject;
        //old..........
        public Vector3 RotationForAnimation = Vector3.Zero;


        OpenGLTextWriter openGLTextwriter;
        //float axisLength = 1f;
        public bool AlignCameraToObject;


        //RenderableObject objectOld;
        PointCloud PointCloud;
        

        private bool isDrawing;
        public OpenGLContext(OGLControl myGLControl)
        {
            GLControlInstance = myGLControl;
            Camera = new Camera();
            PrimitiveTypes = PrimitiveType.Points;
        }

        public void InitDefaults()
        {
            this.GLContextInitialized = true;
            this.Camera.PerspectiveUpdate(GLControlInstance.Width, GLControlInstance.Height);
            Camera.SetDirection(new Vector3(0, 0, -1));

            ResetPointLineSizes();

            //initialize axes, for possible later use
            axes = new Axes(1f);
            axes.InitializeGL();


            grid = new Grid(20, 20);
            grid.InitializeGL();

            cameraFOV = new CameraFOV(0.5f, 5f, 70.6f, 60f);

            cameraFOV.InitializeGL();
            //initialize text writer for axes labels
            openGLTextwriter = new OpenGLTextWriter();

        }

        public void ResetPointLineSizes()
        {
            GL.LineWidth(GLSettings.PointSizeAxis);
            GL.PointSize(GLSettings.PointSize);
        }
        public void Draw()
        {
            try
            {
                if (isDrawing)
                    return;
                isDrawing = true;

                fpsCalc.newFrame();
                UpdateFramesPerSecond();
                
                GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
                


                GL.ClearColor(GLSettings.BackColor);
                //GL.Enable(EnableCap.Texture2D);
                GL.Enable(EnableCap.DepthTest);
                //GL.Disable(EnableCap.CullFace);//.glDisable(GL10.GL_CULL_FACE);

                //or
                //GL.Enable(EnableCap.CullFace);
                //GL.FrontFace(FrontFaceDirection.Ccw);//.glFrontFace(GL10.GL_CCW);
                //GL.CullFace(CullFaceMode.Back);//.glCullFace(GL10.GL_BACK);


                for (int i = 0; i < this.RenderableObjects.Count; i++)
                {

                    RenderableObject o = this.RenderableObjects[i];

                    if (AlignCameraToObject || (!cameraIsAlignedToObject && i == 0))
                    {
                        AlignCameraToObject = false;
                        //System.Diagnostics.Debug.WriteLine("----Align camera to object - on first draw");
                        //o.PointCloudGL.ResetCentroid(true);
                        Camera.ZNear = Convert.ToSingle(o.PointCloud.BoundingBoxMaxFloat);
                        //Convert.ToSingle(o.PointCloud.BoundingBoxMinFloat );
                        Camera.PerspectiveUpdate();


                        this.Camera.Position = new Vector3(0f, 0f, o.PointCloud.BoundingBoxMaxFloat + 5 * this.Camera.ZNear);
                        //Camera.SetDirection(new Vector3(0, 0, 0));

                        //Vector3 axis = Vector3.UnitY;

                        //this.Camera.Rotate(Convert.ToSingle(Math.PI), axis);
                        cameraIsAlignedToObject = true;





                    }

                    o.MVP = this.Camera.MVP;
                    o.Scale = 1;

                    o.Render(this.PrimitiveTypes, PoygonModes);


                }

                DrawAdditionalObjects();

            }
            catch(Exception err)
            {
                System.Diagnostics.Debug.WriteLine("GL Draw Error : ");
                System.Windows.Forms.MessageBox.Show("GL Draw Error: " + err.Message);
            }
            isDrawing = false;
        }
    
        private void DrawAdditionalObjects()
        {
            try
            {
                if (GLSettings.ShowAxes)
                {
                    //AdditionalObjectsToDraw.Add(axes);
                    axes.MVP = this.Camera.MVP;
                    axes.Render(PrimitiveType.Lines, PolygonMode.Line);
                    
                }
                if (GLSettings.ShowGrid)
                {
                    //AdditionalObjectsToDraw.Add(grid);
                    grid.MVP = this.Camera.MVP;
                    grid.Render(PrimitiveType.Lines, PolygonMode.Line);

                }
                if (GLSettings.ShowCameraFOV)
                {
                    //AdditionalObjectsToDraw.Add(cameraFOV);
                    cameraFOV.MVP = this.Camera.MVP;
                    cameraFOV.Render(PrimitiveType.Lines, PolygonMode.Line);

                }

                //Skeleton - used for Kinect
                for (int i = 0; i < AdditionalObjectsToDraw.Count; i++)
                {

                    AdditionalObjectsToDraw[i].MVP = this.Camera.MVP;
                    AdditionalObjectsToDraw[i].Render(PrimitiveType.Lines, PolygonMode.Line);

                }


                try
                {
                    this.GLControlInstance.SwapBuffers();
                }
                catch (Exception err)
                {
                    System.Diagnostics.Debug.WriteLine("SwapBuffers Error : " + err.Message);
                    ErrorCode code = GL.GetError();

                    System.Windows.Forms.MessageBox.Show("SwapBuffers Error: " + err.Message);

                    if (code.ToString() != "NoError")
                        System.Diagnostics.Debug.WriteLine("SwapBuffers Error : " + code.ToString());
                }
            }
            catch (Exception err2)
            {
                System.Diagnostics.Debug.WriteLine("GL Error DrawAdditionalObjects: " + err2.Message);
                ErrorCode code = GL.GetError();
                if (code.ToString() != "NoError")
                    System.Diagnostics.Debug.WriteLine("GL Error DrawAdditionalObjects: " + code.ToString());
            }
        }
        public void AddRenderableObject(RenderableObject o)
        {

            RenderableObjects.Add(o);
            if (GLContextInitialized)
            {
                o.InitializeGL();
                Refresh();
            }


        }
        public void AddModel(Model myModel)
        {
            PointCloudRenderable pcr = new PointCloudRenderable();
            pcr.PointCloud = myModel.PointCloud;

            RenderableObjects.Add(pcr);
            if (GLContextInitialized)
            {
                pcr.InitializeGL();
                Refresh();
            }


        }
        public void ReplaceModel(Model myModel, bool clearPrevious)
        {
            PointCloudRenderable pcr = new PointCloudRenderable();
            pcr.PointCloud = myModel.PointCloud;
            ReplaceRenderableObject(pcr, clearPrevious);


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
            if(AdditionalObjectsToDraw.Count > 0)
            {
                for (int i = 0; i < AdditionalObjectsToDraw.Count; i++)
                {
                    RenderableObject obj = AdditionalObjectsToDraw[i];
                    obj.Dispose();
                }
                AdditionalObjectsToDraw.Clear();
            }
        }
        public void ClearAllObjects()
        {
            ClearAll();
            Refresh();
        }
        //public void RemoveAllModels()
        //{

        //    for (int i = 0; i < RenderableObjects.Count; i++)
        //    {
        //        RenderableObjects[i].Dispose();

        //    }
        //    RenderableObjects.Clear();

        //    if (GLContextInitialized)
        //    {

        //        UpdateControl();
        //    }

        //}
        public void ClearSelectedModel()
        {
            this.RenderableObjects[this.SelectedModelIndex].Dispose();
            this.RenderableObjects.RemoveAt(this.SelectedModelIndex);
            

            

            Refresh();
        }
        public void ReplaceRenderableObject(RenderableObject o, bool clearPrevious)
        {
            try
            {

                this.PointCloud = o.PointCloud;

               // if (clearPrevious)
                    ClearAll();

                if (GLContextInitialized)
                {
                    //System.Diagnostics.Debug.WriteLine("--Replace renderable object");
                    if (this.RenderableObjects.Count > 0)
                    {
                        RenderableObject oldObject = RenderableObjects[0];
                        oldObject.PointCloud = o.PointCloud;
                    }
                    else
                    {
                        o.InitializeGL();
                        RenderableObjects.Add(o);
                    }
                    Refresh();
                }
            }
            catch (Exception err)
            {
                System.Windows.Forms.MessageBox.Show("Error in OpenGLContext.ReplaceRenderableObject : " + err.Message);
            }

        }




        public void Refresh()
        {
            //ResetPointLineSizes();
            if (this.RenderableObjects.Count > 0) //&& this.GLContextInitialized
            {

                //not clear why this has to be called. If the line is missing, update is not done every time. If set, PainControl is called twice in some cases for the same object... 
                this.GLControlInstance.Invalidate();

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

            if (this.GLControlInstance.Parent != null)
            {
                if (this.GLControlInstance.Parent.GetType() == typeof(System.Windows.Forms.Form))
                    this.GLControlInstance.Parent.Text = "OpenGL: " + FramesPerSecond;
            }
        }

        public string FramesPerSecond
        {
            get
            {
                return String.Format("FPS: {0:0.00}", fpsCalc.AvgFramesPerSecond);


            }
        }
        public bool ScaleModels(float scale)
        {


            if (SelectedModelIndex < 0)
            {
                for (int i = 0; i < RenderableObjects.Count; i++)
                    ScaleModel(i, scale);
            }
            else
            {

                ScaleModel(this.SelectedModelIndex, scale);
            }
            //AlignCameraToObject = true;
            return true;
        }
        private void ScaleModel(int iObject, float scale)
        {
            RenderableObject o = this.RenderableObjects[iObject];
            o.PointCloud.Scale(scale);

        }
        public bool RotateModels(float xAngle, float yAngle)
        {

            
            if (SelectedModelIndex < 0)
            {
                for (int i = 0; i < RenderableObjects.Count; i++)
                    RotateModel(i, xAngle, yAngle);
            }
            else
            {

                RotateModel(this.SelectedModelIndex, xAngle, yAngle);
            }
            //AlignCameraToObject = true;
            return true;
        }
        private void RotateModel(int iObject, float xAngle, float yAngle)
        {
            RenderableObject o = this.RenderableObjects[iObject];

            o.PointCloud.RotateDegrees(xAngle, yAngle, 0f);

        }
        public bool TranslateModels(float deltax, float deltay, float deltaz)
        {

            Vector3 v = new Vector3(deltax, deltay, deltaz);
            if (SelectedModelIndex < 0)
            {
                for(int i = 0; i < RenderableObjects.Count; i++)
                    TranslateModel(i, v);
            }
            else
            {
                TranslateModel(this.SelectedModelIndex, v);
            }
            //AlignCameraToObject = true;
            return true;
        }
        private void TranslateModel(int iObject, Vector3 v)
        {
            RenderableObject o = this.RenderableObjects[iObject];
            for (int i = 0; i < o.PointCloud.Vectors.Length; i++)
            {
                o.PointCloud.Vectors[i] = Vector3.Subtract(o.PointCloud.Vectors[i], v);
            }

        }
        public bool CutUndo()
        {
            if (this.PointCloud == null)
                return false;

            RenderableObject o = this.RenderableObjects[0];
            o.PointCloud = PointCloud.Clone(PointCloud);

            AlignCameraToObject = true;
            //ReplaceRenderableObject(objectOld, true);
            return true;


        }
        public void ChangeFieldOfView(float deltaz)
        {
            if (deltaz > 0)
            {
                if (this.Camera.FieldOfView < 2 )
                    this.Camera.FieldOfView *= 2;

                
            }
            else
            {
                this.Camera.FieldOfView /= 2;
            }
            this.Camera.PerspectiveUpdate();
            

        }
        public bool CutModel(float deltaz)
        {

            RenderableObject o = this.RenderableObjects[0];
           // System.Diagnostics.Debug.WriteLine("  Vector before: " + o.PointCloudOpenGL.Vectors[0].X.ToString() + " : " + o.PointCloudOpenGL.Vectors[0].Y.ToString());


            Vector3 v = new Vector3(deltaz, deltaz, deltaz);
            o = o.Clone();
            float factorDelete = 0.9f;
            if(deltaz > 0)
                return false; 
            factorDelete = Convert.ToSingle(o.PointCloud.NormSquaredMax * factorDelete);
            List<Vector3> vectorsNew = new List<Vector3>();
            List<Vector3> colorsNew = new List<Vector3>();
            List<uint> indicesNew = new List<uint>();

            for (int i = 0; i < o.PointCloud.Vectors.Length; i++)
            {
                float maxNorm = o.PointCloud.Vectors[i].NormSquared();
                if (maxNorm < factorDelete)
                {
                   vectorsNew.Add(o.PointCloud.Vectors[i]);
                   colorsNew.Add(o.PointCloud.Colors[i]);
                   indicesNew.Add(o.PointCloud.Indices[i]);

                }
               
                                
            }
            System.Diagnostics.Debug.WriteLine("Cut, before : " + o.PointCloud.Vectors.Length.ToString() + " : Factor : " + factorDelete.ToString() + " : after: " + vectorsNew.Count.ToString());

            
            o.PointCloud.Vectors = new Vector3[vectorsNew.Count];
            o.PointCloud.Colors = new Vector3[colorsNew.Count];
            o.PointCloud.Indices = new uint[indicesNew.Count];

            for (int i = 0; i < vectorsNew.Count; i++ )
            {
                o.PointCloud.Vectors[i] = vectorsNew[i];
                o.PointCloud.Colors[i] = colorsNew[i];
                o.PointCloud.Indices[i] = indicesNew[i];


            }

            AlignCameraToObject = true;
            
            return true;


        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called. 
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources. 
                if (disposing)
                {
                    
                    for(int i = 0; i < this.RenderableObjects.Count; i++)
                    {
                        RenderableObjects[i].Dispose();
                       
                    }
                    RenderableObjects.Clear();
                    for (int i = 0; i < this.AdditionalObjectsToDraw.Count; i++)
                    {
                        AdditionalObjectsToDraw[i].Dispose();
                    }
                    AdditionalObjectsToDraw.Clear();

                    if (axes != null)
                        axes.Dispose();
                    if (grid != null)
                        grid.Dispose();
                    if (cameraFOV != null)
                        cameraFOV.Dispose();
                    if (openGLTextwriter != null)
                        openGLTextwriter.Dispose();
                   
                  
                }

              
                // Note disposing has been done.
                disposed = true;

            }
        }
       
        //public bool CutModel(float deltaz)
        //{

        //    RenderableObject o = this.RenderableObjects[0];
        //    // System.Diagnostics.Debug.WriteLine("  Vector before: " + o.PointCloudOpenGL.Vectors[0].X.ToString() + " : " + o.PointCloudOpenGL.Vectors[0].Y.ToString());


        //    Vector3 v = new Vector3(deltaz, deltaz, deltaz);
        //    o = o.Clone();
        //    float factorDelete = 0.9f;
        //    if (deltaz > 0)
        //        return false;
        //    factorDelete = Convert.ToSingle(o.PointCloud.BoundingBoxMax.MaxCoordinate() * factorDelete);
        //    List<Vector3> vectorsNew = new List<Vector3>();
        //    for (int i = 0; i < o.PointCloud.Vectors.Length; i++)
        //    {
        //        float maxCoordinate = o.PointCloud.Vectors[i].MaxCoordinate();
        //        if (maxCoordinate >= 0)
        //        {
        //            if (maxCoordinate < factorDelete)
        //            {
        //                vectorsNew.Add(o.PointCloud.Vectors[i]);
        //            }
        //        }
        //        else
        //        {
        //            if (maxCoordinate > -factorDelete)
        //            {
        //                vectorsNew.Add(o.PointCloud.Vectors[i]);
        //            }
        //        }

        //    }
        //    System.Diagnostics.Debug.WriteLine("Cut, before : " + o.PointCloud.Vectors.Length.ToString() + " : Factor : " + factorDelete.ToString() + " : after: " + vectorsNew.Count.ToString());


        //    o.PointCloud.Vectors = new Vector3[vectorsNew.Count];
        //    for (int i = 0; i < vectorsNew.Count; i++)
        //    {
        //        o.PointCloud.Vectors[i] = vectorsNew[i];
        //    }

        //    AlignCameraToObject = true;

        //    return true;


        //}
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
