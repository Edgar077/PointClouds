using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;
namespace OpenTK.Extension
{
   


    public class VAO : IDisposable
    {
        #region Variables
        /// <summary>
        /// The ID of this Vertex Array Object for use in calls to 
        /// </summary>
        private uint vaoID; // { get; private set; }

        private VBO<Vector3> vboVectors, vboNormals, vboTangents, vboColors;
        private VBO<Vector2> vboUV;
        private VBO<uint> vboTriangles;
        
        #endregion

        #region Properties
        /// <summary>
        /// The number of vertices that make up this VAO.
        /// </summary>
        public int VertexCount { get; private set; }

        /// <summary>
        /// Specifies if the VAO should dispose of the child VBOs when Dispose() is called.
        /// </summary>
        public bool DisposeChildren { get; set; }

        /// <summary>
        /// The ShaderProgram associated with this VAO.
        /// </summary>
        public ShaderProgram Program { get; private set; }

        /// <summary>
        /// The drawing mode to use when drawing the arrays.
        /// </summary>
        public BeginMode DrawMode { get; set; }

      
        #endregion

        #region Constructors and Destructor
        public VAO(ShaderProgram program, VBO<Vector3> vertex, VBO<uint> mytriangles)
            : this(program, vertex, null, null, null, mytriangles)
        {
        }

        public VAO(ShaderProgram program, VBO<Vector3> vertex, VBO<Vector2> uv, VBO<uint> mytriangles)
            : this(program, vertex, null, null, uv, mytriangles)
        {
        }

        public VAO(ShaderProgram program, VBO<Vector3> vertex, VBO<Vector3> normal, VBO<uint> mytriangles)
            : this(program, vertex, normal, null, null, mytriangles)
        {
        }

        public VAO(ShaderProgram program, VBO<Vector3> vertex, VBO<Vector3> normal, VBO<Vector2> uv, VBO<uint> mytriangles)
            : this(program, vertex, normal, null, uv, mytriangles)
        {
        }

        public VAO(ShaderProgram program, VBO<Vector3> vertex, VBO<Vector3> normal, VBO<Vector3> tangent, VBO<Vector2> uv, VBO<uint> mytriangles)
        {
            this.Program = program;
            this.VertexCount = mytriangles.Count;
            this.DrawMode = BeginMode.Triangles;
            this.vboVectors = vertex;
            this.vboNormals = normal;
            this.vboTangents = tangent;
            this.vboUV = uv;
            this.vboTriangles = mytriangles;

            if (Gl.Version() >= 3)
            {
                vaoID = Gl.GenVertexArray();
                if (vaoID != 0)
                {
                    Gl.BindVertexArray(vaoID);
                    BindVBOsToShader(this.Program);
                }
                Gl.BindVertexArray(0);

                Render = Render_OGL3;
            }
            else
            {
                Render = Render_OGL2;
            }
        }

        ~VAO()
        {
            if (vaoID != 0) System.Diagnostics.Debug.Fail("VAO was not disposed of properly.");
        }
        #endregion

        #region Draw Methods (OGL2 and OGL3)

        //public void BindCachedAttributes(ShaderProgram program, int vertexAttributeLocation, int normalAttributeLocation = -1, int uvAttributeLocation = -1, int tangentAttributeLocation = -1)
        //{
        //    if (normalAttributeLocation != -1 && vboNormals.vboID != 0)
        //    {
        //        Gl.EnableVertexAttribArray((uint)normalAttributeLocation);
        //        Gl.BindBuffer(vboNormals.BufferTarget, vboNormals.vboID);
        //        Gl.VertexAttribPointer((uint)normalAttributeLocation, vboNormals.Size, vboNormals.PointerType, true, 12, IntPtr.Zero);
        //    }

        //    if (uvAttributeLocation != -1 && vboUV.vboID != 0)
        //    {
        //        Gl.EnableVertexAttribArray((uint)uvAttributeLocation);
        //        Gl.BindBuffer(vboUV.BufferTarget, vboUV.vboID);
        //        Gl.VertexAttribPointer((uint)uvAttributeLocation, vboUV.Size, vboUV.PointerType, true, 8, IntPtr.Zero);
        //    }

        //    if (tangentAttributeLocation != -1 && vboTangents.vboID != 0)
        //    {
        //        Gl.EnableVertexAttribArray((uint)tangentAttributeLocation);
        //        Gl.BindBuffer(vboTangents.BufferTarget, vboTangents.vboID);
        //        Gl.VertexAttribPointer((uint)tangentAttributeLocation, vboTangents.Size, vboTangents.PointerType, true, 12, IntPtr.Zero);
        //    }

        //    BindCachedAttributes(vertexAttributeLocation, program);
        //}

        //public void BindCachedAttributes(int vertexAttributeLocation, ShaderProgram program)
        //{
        //    if (vboVectors == null || vboVectors.vboID == 0) 
        //        throw new Exception("Error binding attributes.  No vertices were supplied.");
        //    if (vboTriangles == null || vboTriangles.vboID == 0) 
        //        throw new Exception("Error binding attributes.  No triangles were supplied.");

        //    Gl.EnableVertexAttribArray((uint)vertexAttributeLocation);
        //    Gl.BindBuffer(vboVectors.BufferTarget, vboVectors.vboID);
        //    Gl.VertexAttribPointer((uint)vertexAttributeLocation, vboVectors.Size, vboVectors.PointerType, true, 12, IntPtr.Zero);

        //    Gl.BindBuffer(BufferTarget.ElementArrayBuffer, vboTriangles.vboID);
        //}

        /// <summary>
        /// Generic method for binding the VBOs to their respective attribute locations.
        /// dll assumes the common naming conventions below:
        ///     vertices: vec3 in_position
        ///     normals: vec3 in_normal
        ///     uv: vec2 in_uv
        ///     tangent: vec3 in_tangent
        /// </summary>
        public void BindVBOsToShader(ShaderProgram program)
        {
            if (vboVectors == null || vboVectors.vboID == 0) 
                throw new Exception("Error binding attributes.  No vertices were supplied.");
            if (vboTriangles == null || vboTriangles.vboID == 0) 
                throw new Exception("Error binding attributes.  No mytriangles array was supplied.");

            // Note:  Since the shader is already compiled, we cannot set the attribute locations.
            //  Instead we must query the shader for the locations that the linker chose and use them.
            int loc = Gl.GetAttribLocation(program.ProgramID, "vertexPosition");
            if (loc == -1)
                throw new Exception("Shader did not contain 'vertexPosition'.");

            Gl.EnableVertexAttribArray((uint)loc);
            Gl.BindBuffer(vboVectors.BufferTarget, vboVectors.vboID);
            Gl.VertexAttribPointer((uint)loc, vboVectors.Size, vboVectors.PointerType, true, 12, IntPtr.Zero);

            //triangles:
            Gl.BindBuffer(BufferTarget.ElementArrayBuffer, vboTriangles.vboID);

            if (vboColors != null && vboColors.vboID != 0)
            {
                loc = Gl.GetAttribLocation(program.ProgramID, "vertexColor");
                if (loc != -1)
                {
                    Gl.EnableVertexAttribArray((uint)loc);
                    Gl.BindBuffer(vboColors.BufferTarget, vboColors.vboID);
                    Gl.VertexAttribPointer((uint)loc, vboColors.Size, vboColors.PointerType, true, 12, IntPtr.Zero);
                }
            }

            if (vboNormals != null && vboNormals.vboID != 0)
            {
                loc = Gl.GetAttribLocation(program.ProgramID, "vertexNormal");
                if (loc != -1)
                {
                    Gl.EnableVertexAttribArray((uint)loc);
                    Gl.BindBuffer(vboNormals.BufferTarget, vboNormals.vboID);
                    Gl.VertexAttribPointer((uint)loc, vboNormals.Size, vboNormals.PointerType, true, 12, IntPtr.Zero);
                }
            }

            if (vboUV != null && vboUV.vboID != 0)
            {
                loc = Gl.GetAttribLocation(program.ProgramID, "vertexUV");
                if (loc != -1)
                {
                    Gl.EnableVertexAttribArray((uint)loc);
                    Gl.BindBuffer(vboUV.BufferTarget, vboUV.vboID);
                    Gl.VertexAttribPointer((uint)loc, vboUV.Size, vboUV.PointerType, true, 8, IntPtr.Zero);
                }
            }

            if (vboTangents != null && vboTangents.vboID != 0)
            {
                loc = Gl.GetAttribLocation(program.ProgramID, "vertexTangent");
                if (loc != -1)
                {
                    Gl.EnableVertexAttribArray((uint)loc);
                    Gl.BindBuffer(vboTangents.BufferTarget, vboTangents.vboID);
                    Gl.VertexAttribPointer((uint)loc, vboTangents.Size, vboTangents.PointerType, true, 12, IntPtr.Zero);
                }
            }

            
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
            Gl.DrawElements(DrawMode, VertexCount, DrawElementsType.UnsignedInt, IntPtr.Zero);
            Gl.BindVertexArray(0);
        }

        /// <summary>
        /// OGL2 does not support VAOs, and instead must bind the VBOs to their attributes manually.
        /// </summary>
        private void Render_OGL2(PrimitiveType myRenderMode, OpenTK.Graphics.OpenGL.PolygonMode myPolygonMode)
        {
            BindVBOsToShader(this.Program);
            Gl.DrawElements(DrawMode, VertexCount, DrawElementsType.UnsignedInt, IntPtr.Zero);
        }

        ///// <summary>
        ///// Performs the draw routine with a custom shader program.
        ///// </summary>
        ///// <param name="program"></param>
        //public void DrawProgram(ShaderProgram program)
        //{
        //    BindVBOsToShader(program);
        //    Gl.DrawElements(DrawMode, VertexCount, DrawElementsType.UnsignedInt, IntPtr.Zero);
        //}
        #endregion

        #region IDisposable
        /// <summary>
        /// Deletes the vertex array from the GPU and will also dispose of any child VBOs if (DisposeChildren == true).
        /// </summary>
        public void Dispose()
        {
            // first try to dispose of the vertex array
            if (vaoID != 0)
            {
                Gl.DeleteVertexArrays(1, new uint[] { vaoID });
                vaoID = 0;
            }

            // children must be disposed of separately since OpenGL 2.1 will not have a vertex array
            if (DisposeChildren)
            {
                if (vboVectors != null) vboVectors.Dispose();
                if (vboNormals != null) vboNormals.Dispose();
                if (vboTangents != null) vboTangents.Dispose();
                if (vboUV != null) vboUV.Dispose();
                if (vboColors != null) vboColors.Dispose();
                if (vboTriangles != null) vboTriangles.Dispose();

                vboVectors = null;
                vboNormals = null;
                vboTangents = null;
                vboUV = null;
                vboColors = null;
                vboTriangles = null;
            }
        }
        #endregion
    }

 

}
