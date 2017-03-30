using System.Collections.Generic;

using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenTKExtension
{

    public abstract class RenderableObject : IDisposable
    {
        
        public PointCloud PointCloud = new PointCloud();
        public ShaderProgram shaderProgram = new ShaderProgram();
        
        protected bool initialized;

        public Vector3 Position = Vector3.Zero;
       
        
        public float Scale = 1f;
        
        private Matrix4 p;
      
        Matrix4 m;
        Matrix4 v;
        Matrix4 mvp;


        protected string path = AppDomain.CurrentDomain.BaseDirectory;
        public Vector3 color = new Vector3(1, 1, 1);

        protected int vaoID;
        protected int vboVerticesID;
        protected int vboColorsID;
        protected int vboIndicesID;
        protected int vboNormalsID;
        protected int vboTexturesID;
        //protected int vboUniform;

       


        protected PrimitiveType primitiveType;

        public virtual void FillPointCloud()
        { }
        public virtual void FillIndexBuffer()
        { }
       
      
       
        public RenderableObject()
        {

        }
        public RenderableObject Clone()
        {
            RenderableObject o = new PointCloudRenderable();
            o.PointCloud = this.PointCloud;

            return o;

        }
        public virtual void InitializeGL()
        {
        }
    
        protected void initBuffers()
        {
               
            // Generate Array Buffer Id-s
            GL.GenVertexArrays(1, out vaoID);
            GL.GenBuffers(1, out vboVerticesID);
            GL.GenBuffers(1, out vboColorsID);
            GL.GenBuffers(1, out vboIndicesID);
            GL.GenBuffers(1, out vboNormalsID);
            GL.GenBuffers(1, out vboTexturesID);

        }
        protected bool InitShaders(string vertShaderFilename, string fragShaderFilename, string mypath)
        {
            try
            {
                shaderProgram = new ShaderProgram();
                bool initialized = shaderProgram.InitializeShaders(vertShaderFilename, fragShaderFilename, mypath);

                if (!initialized)
                    return false;
                    //return false;
                GL.UseProgram(shaderProgram.ProgramID);

            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine("Shader initialization failed - check if shader files are available : " + vertShaderFilename + " : " + err.Message);
                System.Windows.Forms.MessageBox.Show("Shader initialization failed - check if shader files are available: " + mypath + "\\" + vertShaderFilename + " : " + err.Message);
                throw new Exception("Shader initialization failed - check if shader files are available : " + vertShaderFilename + " : " + err.Message);
                //return false;

            }
            return true;

        }
        protected void CheckGLError()
        {
            ErrorCode code = GL.GetError();
            if(code != ErrorCode.NoError)
            {
                throw new Exception("GL Error : " + code.ToString());
            }

        }
        protected void RefreshRenderableData()
        {
            try
            {
                //seems to be essential for multiple GL contexts
                deleteBuffers();
                initBuffers();

              
                GL.BindVertexArray(vaoID);

                //bind vertices: this.PointCloud.Vectors (pointer: vboVerticesID) to -> "vVertex" in shader 
                GL.BindBuffer(BufferTarget.ArrayBuffer, vboVerticesID);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(PointCloud.Vectors.Length * Vector3.SizeInBytes), this.PointCloud.Vectors, BufferUsageHint.StaticDraw);
                //CheckGLError();

                shaderProgram.EnableAttribute("vVertex", 3, false);

                //CheckGLError();
                //texture cannot be together with colors!
                if (this.PointCloud.Texture != null && this.PointCloud.TextureUVs != null)
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, vboTexturesID);
                    GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(this.PointCloud.TextureUVs.Length * Vector2.SizeInBytes), this.PointCloud.TextureUVs, BufferUsageHint.StreamDraw);

                    shaderProgram.EnableAttribute("vUV", 2, true);


                }
                else if (this.PointCloud.Colors != null)
                {

                    //bind vertices: this.PointCloud.Colors (pointer: vboColorsID) to -> "vColor" in shader 
                    GL.BindBuffer(BufferTarget.ArrayBuffer, vboColorsID);
                    GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(this.PointCloud.Colors.Length * Vector3.SizeInBytes), this.PointCloud.Colors, BufferUsageHint.StaticDraw);

                    shaderProgram.EnableAttribute("vColor", 3, false);

                    //CheckGLError();

                }

                //normals - bound to Position vectors
                //only if normals are there in attributes
                if (shaderProgram.GetAttributeAddress("vNormal") != -1)
                {

                    GL.BindBuffer(BufferTarget.ArrayBuffer, vboNormalsID);
                    GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(PointCloud.Vectors.Length * Vector3.SizeInBytes), this.PointCloud.Vectors, BufferUsageHint.StaticDraw);

                    shaderProgram.EnableAttribute("vNormal", 3, false);

                }

               

                // Bind current context to Array Buffer ID
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, vboIndicesID);
                // Send data to buffer
                if (this.PointCloud.Indices != null && this.PointCloud.Indices.Length > 0)
                    GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(this.PointCloud.Indices.Length * sizeof(uint)), this.PointCloud.Indices, BufferUsageHint.StaticDraw);



            }
            catch(Exception err)
            {
                //System.Diagnostics.Debug.WriteLine("Error in Activate Shaders " + err.Message);
                System.Windows.Forms.MessageBox.Show("OpenGL Error in Shaders: " + err.Message);
                //throw new Exception("Error in Activate Shaders " + err.Message);

            }
        }
       
  
        private void ValidateBufferSize()
        {
            int bufferSize = 0;
            GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out bufferSize);
            if (this.PointCloud.Indices.Length * sizeof(uint) != bufferSize)
                throw new ApplicationException("Indices array not uploaded correctly");

            //CheckGLError();
        }

        public PrimitiveType PrimitiveType
        {
            get
            {
                return primitiveType;
            }
        }

        #region public properties
        public virtual Matrix4 M
        {
            get
            {
                return m;
            }
            set
            {
                m = value;
            }
        }
     
        public virtual Matrix4 V
        {
            get
            {

                return v;
            }
            set
            {
                v = value;

            }
        }
       
        
        public virtual Matrix4 P
        {
            get
            {
                return p;

            }
            set
            {
                p = value;
            }

        }
        public virtual Matrix4 MVP
        {
            get
            {
                return mvp;

            }
            set
            {
                mvp = value;
            }

        }

        #endregion

        public void Render(PrimitiveType myRenderMode, PolygonMode myPolygonMode)
        {
            try
            {
                RefreshRenderableData();//essential if data has changed after initial call of ActivateShaders

                try
                {
                    this.shaderProgram.Use();
                }
                catch (Exception err)
                {
                    System.Windows.Forms.MessageBox.Show("Error using shader for object: " + this.PointCloud.Name + " : " + err.Message);
                }

                // set the transformation of the object to the MVP matrix
                GL.UniformMatrix4(shaderProgram.GetUniformAddress("MVP"), false, ref this.mvp);

                if (myPolygonMode == PolygonMode.Fill && this.PointCloud.Texture != null)
                {
                    GL.Enable(EnableCap.Texture2D);
                    //GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, Convert.ToSingle(TextureEnvMode.Modulate));
                    GL.BindTexture(TextureTarget.Texture2D, this.PointCloud.Texture.TextureID);
                    //GL.ActiveTexture(TextureUnit.Texture0);
                    
                }
                else
                {
                    GL.Enable(EnableCap.DepthTest);
                }

                GL.PolygonMode(MaterialFace.FrontAndBack, myPolygonMode);
                if (this.PointCloud.Indices == null || this.PointCloud.Indices.Length == 0)
                {
                    System.Windows.Forms.MessageBox.Show("SW Error: Indices of PointCloud are not set, cannot draw in OpenGL");
                }
                else
                {
                    GL.DrawElements(myRenderMode, this.PointCloud.Indices.Length, DrawElementsType.UnsignedInt, 0);
                    //GL.DrawArrays(myRenderMode, 0, this.PointCloud.Indices.Length);
                }
               
                

               

               
            }
            catch (Exception err1)
            {
                System.Windows.Forms.MessageBox.Show("Error rendering object: " + this.PointCloud.Name + " : " + err1.Message);
            }


        }

  

        #region IDisposable

        public virtual void Dispose()
        {
            //Destroy shader
            shaderProgram.Dispose();
            shaderProgram = null;
            if (this.PointCloud.Texture != null)
                this.PointCloud.Texture.Dispose();

            this.PointCloud = null;
            deleteBuffers();
            vaoID = 0;
        }

        private void deleteBuffers()
        {
           
            GL.DeleteBuffer(vboVerticesID);
            GL.DeleteBuffer(vboIndicesID);
            GL.DeleteBuffer(vboColorsID);
            GL.DeleteBuffer(vboNormalsID);
            GL.DeleteBuffer(vboTexturesID);

            GL.DeleteVertexArrays(1, ref vaoID);
        }
          /// <summary>
        /// Check to ensure that the VBO was disposed of properly.
        /// </summary>
        ~RenderableObject()
        {
            if (vaoID != 0) 
                System.Windows.Forms.MessageBox.Show("Renderable object was not disposed of properly");
               // System.Diagnostics.Debug.Fail("Renderable object was not disposed of properly.");
        }

        #endregion

    }


}