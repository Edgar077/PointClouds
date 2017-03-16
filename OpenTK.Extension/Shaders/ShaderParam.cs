using System;
using System.Collections.Generic;
using OpenTK;
namespace OpenTK.Extension
{
    public enum ShaderParamType
    {
        Uniform,
        Attribute
    }

    public class ShaderParam
    {
        #region Variables
        private Type type;
        private int location;
        private uint programid;
        private ShaderParamType ptype;
        private string name;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a program parameter with a given type and name.
        /// The location must be found after the program is compiled
        /// by using the GetLocation(ShaderProgram Program) method.
        /// </summary>
        /// <param name="Type">Specifies the C# equivalent of the GLSL data type.</param>
        /// <param name="ParamType">Specifies the parameter type (either attribute or uniform).</param>
        /// <param name="Name">Specifies the case-sensitive name of the parameter.</param>
        public ShaderParam(Type Type, ShaderParamType ParamType, string Name)
        {
            type = Type;
            ptype = ParamType;
            name = Name;
        }

        /// <summary>
        /// Creates a program parameter with a type, name, program and location.
        /// </summary>
        /// <param name="Type">Specifies the C# equivalent of the GLSL data type.</param>
        /// <param name="ParamType">Specifies the parameter type (either attribute or uniform).</param>
        /// <param name="Name">Specifies the case-sensitive name of the parameter.</param>
        /// <param name="Program">Specifies the OpenGL program ID.</param>
        /// <param name="Location">Specifies the location of the parameter.</param>
        public ShaderParam(Type Type, ShaderParamType ParamType, string Name, uint Program, int Location)
            : this(Type, ParamType, Name)
        {
            programid = Program;
            location = Location;
        }
        #endregion


        #region Properties
        /// <summary>
        /// Specifies the C# equivalent of the GLSL data type.
        /// </summary>
        public Type Type { get { return type; } }

        /// <summary>
        /// Specifies the location of the parameter in the OpenGL program.
        /// </summary>
        public int Location { get { return location; } }

        /// <summary>
        /// Specifies the OpenGL program ID.
        /// </summary>
        public uint Program { get { return programid; } }

        /// <summary>
        /// Specifies the parameter type (either attribute or uniform).
        /// </summary>
        public ShaderParamType ParamType { get { return ptype; } }

        /// <summary>
        /// Specifies the case-sensitive name of the parameter.
        /// </summary>
        public string Name { get { return name; } }
        #endregion

     

        #region GetLocation
        /// <summary>
        /// Gets the location of the parameter in a compiled OpenGL program.
        /// </summary>
        /// <param name="Program">Specifies the shader program that contains this parameter.</param>
        public void GetLocation(ShaderProgram Program)
        {
            Program.Use();
            if (programid == 0)
            {
                programid = Program.ProgramID;
                location = (ptype == ShaderParamType.Uniform ? Program.GetUniformLocation(name) : Program.GetAttributeLocation(name));
            }
        }
        #endregion

        #region SetValue Overrides

        public void SetValue(bool param)
        {
            if (Type != typeof(bool)) throw new Exception(string.Format("SetValue({0}) was given a bool.", Type));
            Gl.Uniform1i(location, (param) ? 1 : 0);
        }

        public void SetValue(int param)
        {
            if (Type != typeof(int) && Type != typeof(Texture)) throw new Exception(string.Format("SetValue({0}) was given a int.", Type));
            Gl.Uniform1i(location, param);
        }

        public void SetValue(float param)
        {
            if (Type != typeof(float)) throw new Exception(string.Format("SetValue({0}) was given a float.", Type));
            Gl.Uniform1f(location, param);
        }

        public void SetValue(Vector2 param)
        {
            if (Type != typeof(Vector2)) throw new Exception(string.Format("SetValue({0}) was given a Vector2.", Type));
            Gl.Uniform2f(location, param.X, param.Y);
        }

        public void SetValue(Vector3 param)
        {
            if (Type != typeof(Vector3)) throw new Exception(string.Format("SetValue({0}) was given a Vector3.", Type));
            Gl.Uniform3f(location, param.X ,param.Y ,param.Z);
        }

        public void SetValue(Vector4 param)
        {
            if (Type != typeof(Vector4)) throw new Exception(string.Format("SetValue({0}) was given a Vector4.", Type));
            Gl.Uniform4f(location, param.X ,param.Y, param.Z ,param.W);
        }



        public void SetValue(Matrix4 param)
        {
            if (Type != typeof(Matrix4)) throw new Exception(string.Format("SetValue({0}) was given a Matrix4.", Type));

            Gl.UniformMatrix4fv(location, param);
        }

        public void SetValue(float[] param)
        {
            if (Type != typeof(Matrix4)) throw new Exception(string.Format("SetValue({0}) was given a Matrix4.", Type));
            if (param.Length != 16) throw new Exception(string.Format("Expected a float[] of 16 for a Matrix4, but instead got {0}.", param.Length));
            Gl.UniformMatrix4fv(location, 1, false, param);
        }

        /*public void SetValue(Texture param)
        {
            if (Type != typeof(Texture)) throw new Exception(string.Format("SetValue({0}) was given a Texture.", Type));
            Gl.Uniform1i(location, param.Binding);
        }*/
        #endregion
    }
}
