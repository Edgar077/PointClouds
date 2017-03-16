using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using System.IO;
using OpenTK;
using System.Drawing;


namespace OpenTKExtension
{
    public class ShaderProgram : IDisposable
    {
        public int ProgramID = -1;

        public Shader VertexShader { get; private set; }
        public Shader FragmentShader { get; private set; }

        public Dictionary<String, AttributeInfo> Attributes = new Dictionary<string, AttributeInfo>();


        private Dictionary<String, UniformInfo> uniforms = new Dictionary<string, UniformInfo>();
        private Dictionary<String, uint> buffers = new Dictionary<string, uint>();

        
        public ShaderProgram()
        {
           
        }
        public bool InitializeShaders(string filename_vshader, string filename_fshader, string path)
        {
            try
            {
                Init();

                VertexShader = Shader.LoadShaderFromFile(path, filename_vshader, ShaderType.VertexShader);
                FragmentShader = Shader.LoadShaderFromFile(path, filename_fshader, ShaderType.FragmentShader);


                GL.AttachShader(ProgramID, VertexShader.ShaderID);
                GL.AttachShader(ProgramID, FragmentShader.ShaderID);
                GL.LinkProgram(ProgramID); // link shaders

                ValidateLink();
                SetAttributes();
                SetUniforms();
                GenBuffers();
            }
            catch(Exception err)
            {
                string errorMessage = "Error initializing shaderes. Error for fragment shader is: " + this.FragmentShader.ShaderLog + " : for vertex shader : " + this.VertexShader.ShaderLog;
           
                System.Windows.Forms.MessageBox.Show(errorMessage + err.Message);
                return false;
                //System.Diagnostics.Debug.WriteLine("Error in initializing Shaders " + err.Message);
            }
            return true;

        }
        public void Use()
        {
            
            GL.UseProgram(this.ProgramID);

        }
        public void UnUse()
        {
            GL.UseProgram(0);

        }
        private void Init()
        {
            if (!IsSupported)
            {
                System.Diagnostics.Debug.WriteLine("Failed to create Shader." +
                    Environment.NewLine + "Your system doesn't support Shader.", "Error");

                throw new Exception("Error: Shaders are not supported by this system");

            }
            ProgramID = GL.CreateProgram();
        }
        private bool ValidateLink()
        {
            GL.ValidateProgram(ProgramID);
            int program_ok;

            GL.GetProgram(ProgramID, GetProgramParameterName.LinkStatus, out program_ok);

            if (program_ok == 0)
            {
                throw new Exception("During link validation: Error , shader linking failed, error for fragment shader is: " + this.FragmentShader.ShaderLog + " : for vertex shader : " + this.VertexShader.ShaderLog);
            }
            return true;

        }
        public int EnableAttribute(string attributeName, int size, bool normalized )
        {
            int addr = this.GetAttributeAddress(attributeName);
            if (addr < 0)
            {
                System.Windows.Forms.MessageBox.Show("SW Error: Shader - no attribute " + attributeName + " in shader file");
                return addr;

            }
            int stride = 0;
            if (size == 3)
                stride = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Vector3));
            else if (size == 2)
                stride = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Vector2));

            GL.VertexAttribPointer(addr, size, VertexAttribPointerType.Float, normalized, stride, 0);
            
            GL.EnableVertexAttribArray(addr);
            return addr;

        }
    
   
        private void SetAttributes()
        {
            int number;
            GL.GetProgram(ProgramID, GetProgramParameterName.ActiveAttributes, out number);


            for (int i = 0; i < number; i++)
            {
                AttributeInfo info = new AttributeInfo();
                int length = 0;

                StringBuilder name = new StringBuilder();

                GL.GetActiveAttrib(ProgramID, i, 256, out length, out info.Size, out info.type, name);

                info.Name = name.ToString();
                info.Address = GL.GetAttribLocation(ProgramID, info.Name);
                Attributes.Add(name.ToString(), info);
            }
        }
        private void SetUniforms()
        {
            int number;
            GL.GetProgram(ProgramID, GetProgramParameterName.ActiveUniforms, out number);

            for (int i = 0; i < number; i++)
            {
                UniformInfo info = new UniformInfo();
                int length = 0;

                StringBuilder name = new StringBuilder();

                GL.GetActiveUniform(ProgramID, i, 256, out length, out info.Size, out info.type, name);

                info.Name = name.ToString();
                uniforms.Add(name.ToString(), info);
                info.Address = GL.GetUniformLocation(ProgramID, info.Name);
            }
        }
      

        private void GenBuffers()
        {
            for (int i = 0; i < Attributes.Count; i++)
            {
                uint buffer = 0;
                GL.GenBuffers(1, out buffer);

                buffers.Add(Attributes.Values.ElementAt(i).Name, buffer);
            }

            for (int i = 0; i < uniforms.Count; i++)
            {
                uint buffer = 0;
                GL.GenBuffers(1, out buffer);

                buffers.Add(uniforms.Values.ElementAt(i).Name, buffer);
            }
        }

        private void EnableVertexAttribArrays()
        {
            for (int i = 0; i < Attributes.Count; i++)
            {
                GL.EnableVertexAttribArray(Attributes.Values.ElementAt(i).Address);
            }
        }

        private void DisableVertexAttribArrays()
        {
            for (int i = 0; i < Attributes.Count; i++)
            {
                GL.DisableVertexAttribArray(Attributes.Values.ElementAt(i).Address);
            }
        }

        public int GetAttributeAddress(string name)
        {
            if (Attributes.ContainsKey(name))
            {
                return Attributes[name].Address;
            }
            else
            {
                return -1;
            }
        }

        public int GetUniformAddress(string name)
        {
            if (uniforms.ContainsKey(name))
            {
                return uniforms[name].Address;
            }
            else
            {
                return -1;
            }
        }

        public uint GetBuffer(string name)
        {
            if (buffers.ContainsKey(name))
            {
                return buffers[name];
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Get Whether the Shader function is Available on this Machine or not
        /// </summary>
        public static bool IsSupported
        {
            get
            {
                return (new Version(GL.GetString(StringName.Version).Substring(0, 3)) >= new Version(2, 0) ? true : false);
            }
        }
       
       
        /// <summary>
        /// Change a value Variable of the Shader
        /// </summary>
        /// <param name="name">Variable Name</param>
        /// <param name="x">Value</param>
        public void ChangeUniformX(string name, float x)
        {

            int location = GetUniformAddress(name);
            if (location != -1)
                  GL.Uniform1(location, x);

            this.UnUse();
           
        }

        /// <summary>
        /// Change a 2 value Vector Variable of the Shader
        /// </summary>
        /// <param name="name">Variable Name</param>
        /// <param name="x">First Vector Value</param>
        /// <param name="y">Second Vector Value</param>
        public void ChangeUniformXY(string name, float x, float y)
        {
            int location = GetUniformAddress(name);
            if (location != -1)
                GL.Uniform2(location, x, y);
            this.UnUse();


        }

        /// <summary>
        /// Change a 3 value Vector Variable of the Shader
        /// </summary>
        /// <param name="name">Variable Name</param>
        /// <param name="x">First Vector Value</param>
        /// <param name="y">Second Vector Value</param>
        /// <param name="z">Third Vector Value</param>
        public void ChangeUniformXYZ(string name, float x, float y, float z)
        {
            int location = GetUniformAddress(name);
            if (location != -1)
                GL.Uniform3(location, x, y, z);
            this.UnUse();

        }

        /// <summary>
        /// Change a 4 value Vector Variable of the Shader
        /// </summary>
        /// <param name="name">Variable Name</param>
        /// <param name="x">First Vector Value</param>
        /// <param name="y">Second Vector Value</param>
        /// <param name="z">Third Vector Value</param>
        /// <param name="w">Fourth Vector Value</param>
        public void ChangeUniformXYZW(string name, float x, float y, float z, float w)
        {
            int location = GetUniformAddress(name);
            if (location != -1)
                GL.Uniform4(location, x, y, z, w);
            this.UnUse();
        }

        /// <summary>
        /// Change a Matrix4 Variable of the Shader
        /// </summary>
        /// <param name="name">Variable Name</param>
        /// <param name="matrix">Matrix</param>
        public void ChangeUniformMatrix4(string name, Matrix4 matrix)
        {
            int location = GetUniformAddress(name);
            if (location != -1)
                GL.UniformMatrix4(location, false, ref matrix);
            this.UnUse();


        }

        /// <summary>
        /// Change a 2 value Vector Variable of the Shader
        /// </summary>
        /// <param name="name">Variable Name</param>
        /// <param name="vector">Vector Value</param>
        public void ChangeUniformVector2(string name, Vector2 vector)
        {
            ChangeUniformXY(name, vector.X, vector.Y);
        }

        /// <summary>
        /// Change a 3 value Vector Variable of the Shader
        /// </summary>
        /// <param name="name">Variable Name</param>
        /// <param name="vector">Vector Value</param>
        public void ChangeUniformVector3(string name, Vector3 vector)
        {
            ChangeUniformXYZ(name, vector.X, vector.Y, vector.Z);
        }

        /// <summary>
        /// Change a Color Variable of the Shader
        /// </summary>
        /// <param name="name">Variable Name</param>
        /// <param name="color">Color Value</param>
        public void ChangeUniformColor(string name, Color color)
        {
            ChangeUniformXYZW(name, color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
        }

        public class UniformInfo
        {
            public String Name = "";
            public int Address = -1;
            public int Size = 0;
            public ActiveUniformType type;
        }

        public class AttributeInfo
        {
            public String Name = "";
            public int Address = -1;
            public int Size = 0;
            public ActiveAttribType type;
        }

        #region IDisposable

        public void Dispose()
        {
            try
            {
                if (ProgramID != 0)
                {
                    GL.DetachShader(ProgramID, VertexShader.ShaderID);
                    GL.DetachShader(ProgramID, FragmentShader.ShaderID);

                    GL.DeleteProgram(ProgramID);
                    GL.UseProgram(0);

                    VertexShader.Dispose();
                    FragmentShader.Dispose();

                    this.ProgramID = 0;

                }
            }
            catch (Exception err)
            {
                System.Windows.Forms.MessageBox.Show("Error dispsing Shader program: " + err.Message);
                ErrorCode code = GL.GetError();
                if (code.ToString() != "NoError")
                    System.Diagnostics.Debug.WriteLine("Shader Error : " + code.ToString());
            }
        }
        
        #endregion
    }
}
