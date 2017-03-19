using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;
namespace OpenTK.Extension
{
    public class GenericVAO : IDisposable
    {
        #region Generic VBO
        public GenericVBO[] vbos;

        public struct GenericVBO
        {
            public string name;
            public VertexAttribPointerType pointerType;
            public int length;
            public BufferTarget bufferTarget;
            public uint vboID;
            public int size;

            public GenericVBO(uint VboID, string Name, int Length, int Size, VertexAttribPointerType PointerType, BufferTarget BufferTarget)
            {
                vboID = VboID;
                name = Name;
                length = Length;
                size = Size;
                pointerType = PointerType;
                bufferTarget = BufferTarget;
            }
        }
        #endregion

        #region Constructor and Destructor

        public GenericVAO(ShaderProgram program)
        {
            this.Program = program;
            this.DrawMode = BeginMode.Triangles;
        }

        public void Init(GenericVBO[] vbos)
        {
            this.vbos = vbos;
            
            if (Gl.Version() >= 3)
            {
                vaoID = Gl.GenVertexArray();
                if (vaoID != 0)
                {
                    Gl.BindVertexArray(vaoID);
                    BindAttributes(this.Program);
                }
                Gl.BindVertexArray(0);

                Draw = DrawOGL3;
            }
            else
            {
                Draw = DrawOGL2;
            }
        }

        ~GenericVAO()
        {
            if (vaoID != 0) System.Diagnostics.Debug.Fail("VAO was not disposed of properly.");
        }
        #endregion

        #region Properties
        /// <summary>
        /// The number of vertices that make up this VAO.
        /// </summary>
        public int VertexCount { get; set; }

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

        /// <summary>
        /// The ID of this Vertex Array Object for use in calls to 
        /// </summary>
        public uint vaoID { get; private set; }
        #endregion

        #region Draw Methods (OGL2 and OGL3)
        private int SizeOfType(VertexAttribPointerType type)
        {
            switch (type)
            {
                case VertexAttribPointerType.Byte:
                case VertexAttribPointerType.UnsignedByte: return 1;
                case VertexAttribPointerType.Short:
                case VertexAttribPointerType.UnsignedShort:
                case VertexAttribPointerType.HalfFloat: return 2;
                case VertexAttribPointerType.Int:
                case VertexAttribPointerType.Float: return 4;
                case VertexAttribPointerType.Double: return 8;
            }
            return 1;
        }

        public void BindAttributes(ShaderProgram program)
        {
            GenericVBO elementArray = new GenericVBO(0, "", 0, 0, VertexAttribPointerType.Byte, BufferTarget.ArrayBuffer);

            for (int i = 0; i < vbos.Length; i++)
            {
                if (vbos[i].bufferTarget == BufferTarget.ElementArrayBuffer)
                {
                    elementArray = vbos[i];
                    continue;
                }

                int loc = Gl.GetAttribLocation(program.ProgramID, vbos[i].name);
                if (loc == -1) throw new Exception(string.Format("Shader did not contain '{0}'.", vbos[i].name));

                Gl.EnableVertexAttribArray((uint)loc);
                Gl.BindBuffer(vbos[i].bufferTarget, vbos[i].vboID);
                Gl.VertexAttribPointer((uint)loc, vbos[i].size, vbos[i].pointerType, true, vbos[i].size * SizeOfType(vbos[i].pointerType), IntPtr.Zero);
            }

            if (elementArray.vboID != 0)
            {
                Gl.BindBuffer(BufferTarget.ElementArrayBuffer, elementArray.vboID);
                VertexCount = elementArray.length;
            }
        }

        public delegate void DrawFunc();

        public DrawFunc Draw;

        /// <summary>
        /// OGL3 method uses a vertex array object for quickly binding the VBOs to their attributes.
        /// </summary>
        private void DrawOGL3()
        {
            if (vaoID == 0 || VertexCount == 0) return;
            Gl.BindVertexArray(vaoID);
            Gl.DrawElements(DrawMode, VertexCount, DrawElementsType.UnsignedInt, IntPtr.Zero);
            Gl.BindVertexArray(0);
        }

        /// <summary>
        /// OGL2 does not support VAOs, and instead must bind the VBOs to their attributes manually.
        /// </summary>
        private void DrawOGL2()
        {
            if (VertexCount == 0) return;
            BindAttributes(this.Program);
            Gl.DrawElements(DrawMode, VertexCount, DrawElementsType.UnsignedInt, IntPtr.Zero);
        }

        /// <summary>
        /// Performs the draw routine with a custom shader program.
        /// </summary>
        /// <param name="program"></param>
        public void DrawProgram(ShaderProgram program)
        {
            BindAttributes(program);
            Gl.DrawElements(DrawMode, VertexCount, DrawElementsType.UnsignedInt, IntPtr.Zero);
        }
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
                for (int i = 0; i < vbos.Length; i++)
                    Gl.DeleteBuffer(vbos[i].vboID);
            }
        }
        #endregion
    }

}
