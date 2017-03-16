using System.Collections.Generic;

using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Extension;

 


//Initialization:
//  Bind VAO
//  Bind Buffer
//  Upload Buffer Data
//  Enable Client States
//  Setup Pointers
//  Unbind VAO

//Update/Render Loop:
//  Bind Buffer
//  Update Buffer Data

//  Bind VAO
//  Draw
//  Unbind VAO

namespace OpenTK.Extension
{

    public abstract class RenderableObject
    {
        protected ShaderProgram shaderProgram;
        private ShaderProgram shaderFont;

        private uint vaoID;
        protected VBO<Vector3> vboVectors, vboNormals, vboTangents, vboColors;
        protected VBO<Vector2> vboUV;
        protected VBO<uint> vboTriangles;
       

        protected Texture brickDiffuse;
        protected Texture brickNormals;
        private BMFont font;

        protected bool colorOutput;
        protected bool initialized;

        public PointCloudGL PointCloudGL = new PointCloudGL();
        public Vector3 Position = Vector3.Zero;

        //public Vector3 Scale = Vector3.One;
        public float Scale = 1f;

        private Matrix4 p;
       
        Matrix4 m;
        Matrix4 v;
        Matrix4 mvp;


        protected string path = AppDomain.CurrentDomain.BaseDirectory;
        public Vector3 color = new Vector3(1, 1, 1);

        protected PrimitiveType primitiveType;
        public virtual void FillPointCloud()
        {
            
        }
        public virtual void CreateVBOs()
        {
            if (this.PointCloudGL.Vectors != null)
                    vboVectors = new VBO<Vector3>("Vertices", this.PointCloudGL.Vectors);

            if (this.PointCloudGL.Colors != null)
                    this.vboColors = new VBO<Vector3>("Colors", this.PointCloudGL.Colors);


            if (this.PointCloudGL.Triangles == null)
            {
                this.PointCloudGL.Triangles = new uint[PointCloudGL.Vectors.GetLength(0)];
                for (uint i = 0; i < PointCloudGL.Vectors.GetLength(0); i++)
                {
                    this.PointCloudGL.Triangles[i] = i;
                }
            }

            if (this.PointCloudGL.Triangles != null)
                vboTriangles = new VBO<uint>("Triangles", this.PointCloudGL.Triangles, OpenTK.Extension.BufferTarget.ElementArrayBuffer);

            if (this.PointCloudGL.UVS != null)
                vboUV = new VBO<Vector2>("UVS", PointCloudGL.UVS);

            if (this.PointCloudGL.Normals != null)
                vboNormals = new VBO<Vector3>("Normals", PointCloudGL.Normals);

            if (this.PointCloudGL.Tangents != null)
                vboTangents = new VBO<Vector3>("Tangents", PointCloudGL.Tangents);


        }
    
     
        public RenderableObject()
        {

        }
        protected void InitShader()
        {
            // create our shader program
            shaderProgram = new ShaderProgram();

            // set up the projection and view matrix
            shaderProgram.Use();
      
            //shaderProgram["light_direction"].SetValue(new Vector3(0, 0, 1));
            //shaderProgram["enable_lighting"].SetValue(GLSettings.Lighting);
            //shaderProgram["normalTexture"].SetValue(1);
            //shaderProgram["enable_mapping"].SetValue(GLSettings.NormalMapping);
            

        }
        public virtual void InitializeGL()
        {
            if (GLSettings.PointCloudCentered)
                this.PointCloudGL.ResetCentroid(true);
            if (GLSettings.PointCloudResize)
                this.PointCloudGL.ResizeVerticesTo1();

            InitShader();
            CreateVAO();
            InitFont();
            
        }
        public virtual void Reset()
        {
            this.Dispose();
            InitializeGL();
        }
        protected void CreateVAO()
        {
            if (Gl.Version() >= 3)
            {
                vaoID = Gl.GenVertexArray();
                if (vaoID != 0)
                {
                    Gl.BindVertexArray(vaoID);
                    CreateVBOs();
                    BindVBOsToShader();
                }
                // Bind back to the default state.
                Gl.BindVertexArray(0);
                Gl.BindBuffer(OpenTK.Extension.BufferTarget.ArrayBuffer, 0);
  
                //set render method
                Render = Render_OGL3;
            }
            else
            {
                CreateVBOs();
                //set render method
                Render = Render_OGL2;
            }

            

        }
        protected void InitFont()
        {
            // load the bitmap font for this tutorial
            font = new BMFont("Fonts\\font24.fnt", "Fonts\\font24.png");
            shaderFont = new ShaderProgram(BMFont.FontVertexSource, BMFont.FontFragmentSource);

            shaderFont.Use();
            shaderFont["ortho_matrix"].SetValue(Matrix4.CreateOrthographic(GLSettings.Width, GLSettings.Height, 0, 1000));
            shaderFont["color"].SetValue(new Vector3(1, 1, 1));


        }
    
        public virtual void Dispose()
        {
            //Destroy shader
            shaderProgram.DisposeChildren = true;
            shaderProgram.Dispose();

            if (shaderFont != null)
            {
                shaderFont.DisposeChildren = true;
                shaderFont.Dispose();
            }

            // first try to dispose the vertex array
            if (vaoID != 0)
            {
                Gl.DeleteVertexArrays(1, new uint[] { vaoID });
                vaoID = 0;
            }
          

            vboVectors.Dispose();
            if (vboTriangles != null)
                vboTriangles.Dispose();

            if (vboNormals != null)
                vboNormals.Dispose();
            if (vboTangents != null)
                vboTangents.Dispose();
            if (vboColors != null)
                vboColors.Dispose();
            if (vboUV != null)
                vboUV.Dispose();
  
        
         
            if (brickDiffuse != null)
                brickDiffuse.Dispose();
            if (brickNormals != null) 
                brickNormals.Dispose();

            if (font != null)
                font.FontTexture.Dispose();
            

        }
         ~RenderableObject()
        {
            if (vaoID != 0) 
                System.Diagnostics.Debug.Fail("VAO was not disposed of properly.");
        }
     
        protected bool InitShaders(string vertShaderFilename, string fragShaderFilename, string mypath)
        {
            try
            {
                shaderProgram = new ShaderProgram(vertShaderFilename, fragShaderFilename, mypath);
               
                GL.UseProgram(shaderProgram.ProgramID);

            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine("Shader initialization failed - check shader files : " + err.Message);
                throw new Exception("Shader initialization failed - check shader files : " + err.Message);
                //return false;

            }
            return true;

        }
        protected void CheckGLError()
        {
            OpenTK.Graphics.OpenGL.ErrorCode code = GL.GetError();
            if (code != OpenTK.Graphics.OpenGL.ErrorCode.NoError)
            {
                throw new Exception("GL Error : " + code.ToString());
            }

        }
 


        private void ValidateBufferSize()
        {
            int bufferSize = 0;
            GL.GetBufferParameter(OpenTK.Graphics.OpenGL.BufferTarget.ElementArrayBuffer, OpenTK.Graphics.OpenGL.BufferParameterName.BufferSize, out bufferSize);
            if (this.PointCloudGL.Triangles.Length * sizeof(int) != bufferSize)
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

        private void RenderVBOs(PrimitiveType myRenderMode, OpenTK.Graphics.OpenGL.PolygonMode myPolygonMode)
        {


            ActivateShader();

            //this.shaderProgram.Use();
            //GL.UniformMatrix4(shaderProgram.GetUniformAddress("MVP"), false, ref this.mvp);
            switch (myRenderMode)
            {

                case PrimitiveType.Triangles:
                    {
                        GL.PolygonMode(OpenTK.Graphics.OpenGL.MaterialFace.FrontAndBack, myPolygonMode);
                        //GL.PolygonMode(OpenTK.Graphics.OpenGL.MaterialFace.FrontAndBack, OpenTK.Graphics.OpenGL.PolygonMode.Line);
                        OpenTK.Extension.Gl.DrawElements(OpenTK.Extension.BeginMode.Triangles, vboTriangles.Count, OpenTK.Extension.DrawElementsType.UnsignedInt, IntPtr.Zero);
                        
                        // bind the font program as well as the font texture
                        Gl.UseProgram(shaderFont.ProgramID);
                        Gl.BindTexture(font.FontTexture);

                      
                      

                        break;
                    }
                case PrimitiveType.TriangleFan:
                    {
                        //GL.PolygonMode(OpenTK.Graphics.OpenGL.MaterialFace.FrontAndBack, OpenTK.Graphics.OpenGL.PolygonMode.Line);

                        GL.PolygonMode(OpenTK.Graphics.OpenGL.MaterialFace.FrontAndBack, myPolygonMode);

                        OpenTK.Extension.Gl.DrawElements(OpenTK.Extension.BeginMode.TriangleFan, vboTriangles.Count, OpenTK.Extension.DrawElementsType.UnsignedInt, IntPtr.Zero);
                        // bind the font program as well as the font texture
                        Gl.UseProgram(shaderFont.ProgramID);
                        Gl.BindTexture(font.FontTexture);

                        break;
                    }
                case PrimitiveType.TriangleStrip:
                    {
                        GL.PolygonMode(OpenTK.Graphics.OpenGL.MaterialFace.FrontAndBack, myPolygonMode);
                        //GL.PolygonMode(OpenTK.Graphics.OpenGL.MaterialFace.FrontAndBack, OpenTK.Graphics.OpenGL.PolygonMode.Line);
                        
                        OpenTK.Extension.Gl.DrawElements(OpenTK.Extension.BeginMode.TriangleStrip, vboTriangles.Count, OpenTK.Extension.DrawElementsType.UnsignedInt, IntPtr.Zero);
                        // bind the font program as well as the font texture
                        Gl.UseProgram(shaderFont.ProgramID);
                        Gl.BindTexture(font.FontTexture);

                        break;
                    }
                default:
                    {


                        //GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.PolygonOffsetFill);
                        //GL.PolygonOffset(1.0f, 1.0f);
                        //GL.DrawArrays(myRenderMode, 0, this.PointCloudGL.Triangles.Length);

                        //GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.PolygonOffsetPoint);
                        GL.PolygonMode(OpenTK.Graphics.OpenGL.MaterialFace.Back, myPolygonMode);
                        GL.DrawArrays(myRenderMode, 0, this.PointCloudGL.Triangles.Length);
                        break;
                    }

            }

            // bind the font program as well as the font texture
            if (shaderFont != null)
                OpenTK.Extension.Gl.UseProgram(shaderFont.ProgramID);
            if (font != null)
                OpenTK.Extension.Gl.BindTexture(font.FontTexture);

        }
        private void BindVBOsToShader()
        {
            this.shaderProgram["colorOutput"].SetValue(this.colorOutput);
            shaderProgram["light_direction"].SetValue(new Vector3(0, 0, 1));
            shaderProgram["enable_lighting"].SetValue(GLSettings.Lighting);
            shaderProgram["normalTexture"].SetValue(1);
            shaderProgram["enable_mapping"].SetValue(GLSettings.NormalMapping);


            Gl.BindBufferToShaderAttribute(vboVectors, shaderProgram, "vertexPosition");
            if (vboTriangles != null)
                Gl.BindBuffer(vboTriangles);

            if (vboNormals != null)
                Gl.BindBufferToShaderAttribute(vboNormals, shaderProgram, "vertexNormal");
            if (vboTangents != null)
                Gl.BindBufferToShaderAttribute(vboTangents, shaderProgram, "vertexTangent");
            if (vboUV != null)
                Gl.BindBufferToShaderAttribute(vboUV, shaderProgram, "vertexUV");
 
            if (vboColors != null)
                Gl.BindBufferToShaderAttribute(vboColors, shaderProgram, "vertexColor");
            //if (brickNormals != null)
            //{
            //    OpenTK.Extension.Gl.ActiveTexture(OpenTK.Extension.TextureUnit.Texture1);
            //    OpenTK.Extension.Gl.BindTexture(brickNormals);
            //}
            //if (brickDiffuse != null)
            //{
            //    OpenTK.Extension.Gl.ActiveTexture(OpenTK.Extension.TextureUnit.Texture0);
            //    OpenTK.Extension.Gl.BindTexture(brickDiffuse);
            //}
          
        }
        private void ActivateShader()
        {
            

            // make sure the shader program and texture are being used
            OpenTK.Extension.Gl.UseProgram(this.shaderProgram);
            
            //EDGAR - should not be necessary any more!
            //BindVBOsToShader();

          
            // bind the active texture

            //why cannot move this to method "BindVBOsToShader"??
            if (brickNormals != null)
            {
                OpenTK.Extension.Gl.ActiveTexture(OpenTK.Extension.TextureUnit.Texture1);
                OpenTK.Extension.Gl.BindTexture(brickNormals);
            }
            if (brickDiffuse != null)
            {
                OpenTK.Extension.Gl.ActiveTexture(OpenTK.Extension.TextureUnit.Texture0);
                OpenTK.Extension.Gl.BindTexture(brickDiffuse);
            }

            shaderProgram["projection_matrix"].SetValue(this.p);
            shaderProgram["view_matrix"].SetValue(this.v);
            shaderProgram["model_matrix"].SetValue(this.m);


        }

        public delegate void RenderMethod(PrimitiveType myRenderMode, OpenTK.Graphics.OpenGL.PolygonMode myPolygonMode);

        public RenderMethod Render;

        /// <summary>
        /// OGL3 method uses a vertex array object for quickly binding the VBOs to their attributes.
        /// </summary>
        private void Render_OGL3(PrimitiveType myRenderMode, OpenTK.Graphics.OpenGL.PolygonMode myPolygonMode)
        {
            if (vaoID == 0)
                return;
            Gl.BindVertexArray(vaoID);
            RenderVBOs(myRenderMode, myPolygonMode);
            //reset binding
            Gl.BindVertexArray(0);
        }

        /// <summary>
        /// OGL2 does not support VAOs, and instead must bind the VBOs to their attributes manually.
        /// </summary>
        private void Render_OGL2(PrimitiveType myRenderMode, OpenTK.Graphics.OpenGL.PolygonMode myPolygonMode)
        {
            
            BindVBOsToShader();
            RenderVBOs(myRenderMode, myPolygonMode);
        }

    }


}