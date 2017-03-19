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
    public class Shader : IDisposable
    {

        #region Properties
        /// <summary>
        /// Specifies the OpenGL ShaderID.
        /// </summary>
        public int ShaderID;
        private string shaderString;
        /// <summary>
        /// Specifies the type of shader.
        /// </summary>
        public ShaderType ShaderType { get; private set; }

        /// <summary>
        /// Contains all of the attributes and uniforms parsed from this shader source.
        /// </summary>
        //public ProgramParam[] ShaderParams { get; private set; }

        /// <summary>
        /// Returns Gl.GetShaderInfoLog(ShaderID), which contains any compilation errors.
        /// </summary>
        public string ShaderLog
        {
            get { return GetShaderInfoLog(ShaderID); }
        }
        #endregion
        
        public Shader()
        {
           
        }
        
        private static Shader loadShader(String code, ShaderType type)
        {
            
            Shader sh = new Shader();
            sh.shaderString = code;

            sh.ShaderType = type;
            sh.ShaderID = GL.CreateShader(type);
            GL.ShaderSource(sh.ShaderID, code);
            GL.CompileShader(sh.ShaderID);
           

            //GetParams(source);

            return sh;
        }

        public static Shader LoadShaderFromFile(string path, String filename, ShaderType type)
        {
            using (StreamReader sr = new StreamReader(path + filename))
            {
                if (type == ShaderType.VertexShader)
                {
                    return loadShader(sr.ReadToEnd(), type);
                }
                else if (type == ShaderType.FragmentShader)
                {
                    return loadShader(sr.ReadToEnd(), type);
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the program info from a shader program.
        /// </summary>
        /// <param name="program">The ID of the shader program.</param>
        public static string GetShaderInfoLog(int shader)
        {
            //return string.Empty;
        
            string strError = string.Empty;
            GL.GetShaderInfoLog(shader, out strError);

           
            return strError;
        }


        #region IDisposable

        ~Shader()
        {
            if (ShaderID != 0) 
                System.Diagnostics.Debug.Fail("Shader was not disposed properly.");
        }

        public void Dispose()
        {
            if (ShaderID != 0)
            {
                GL.DeleteShader(ShaderID);
                this.ShaderID = 0;
            }
        }

        #endregion
    }
}
