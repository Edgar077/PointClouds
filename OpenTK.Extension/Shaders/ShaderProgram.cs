using System;
using System.Collections.Generic;
using System.IO;

namespace OpenTK.Extension
{
    public class ShaderProgram : IDisposable
    {
        #region Properties
        /// <summary>
        /// Specifies the OpenGL shader program ID.
        /// </summary>
        public uint ProgramID { get; private set; }

        /// <summary>
        /// Specifies the vertex shader used in this program.
        /// </summary>
        public Shader VertexShader { get; private set; }

        /// <summary>
        /// Specifies the fragment shader used in this program.
        /// </summary>
        public Shader FragmentShader { get; private set; }

        /// <summary>
        /// Specifies whether this program will dispose of the child 
        /// vertex/fragment programs when the IDisposable method is called.
        /// </summary>
        public bool DisposeChildren { get; set; }

        private Dictionary<string, ShaderParam> shaderParams;

        /// <summary>
        /// Queries the shader parameter hashtable to find a matching attribute/uniform.
        /// </summary>
        /// <param name="name">Specifies the case-sensitive name of the shader attribute/uniform.</param>
        /// <returns>The requested attribute/uniform, or null on a failure.</returns>
        public ShaderParam this[string name]
        {
            get { return shaderParams.ContainsKey(name) ? shaderParams[name] : null; }
        }

        /// <summary>
        /// Returns Gl.GetShaderInfoLog(ShaderID), which contains any linking errors.
        /// </summary>
        public string ProgramLog
        {
            get { return Gl.GetProgramInfoLog(ProgramID); }
        }
        #endregion

        #region Constructors and Destructor
        
        public ShaderProgram()
        {
            ResetShader(new Shader(VertexShaderDefault, ShaderType.VertexShader), new Shader(FragmentShaderDefault, ShaderType.FragmentShader));
        }
        /// <summary>
        /// Links a vertex and fragment shader together to create a shader program.
        /// </summary>
        /// <param name="vertexShader">Specifies the vertex shader.</param>
        /// <param name="fragmentShader">Specifies the fragment shader.</param>
        public ShaderProgram(Shader vertexShader, Shader fragmentShader)
        {
            ResetShader(vertexShader, fragmentShader);
        }

        public ShaderProgram(string filename_vshader, string filename_fshader, string path)
        {
            try
            {
                Init();
                StreamReader sr = new StreamReader(path + filename_vshader);
                string vertexShaderSource = sr.ReadToEnd();

                sr = new StreamReader(path + filename_fshader);
                string fragmentShaderSource = sr.ReadToEnd();

                ResetShader(new Shader(vertexShaderSource, ShaderType.VertexShader), new Shader(fragmentShaderSource, ShaderType.FragmentShader));

            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine("Error in initializing Shaders " + err.Message);
            }
            
        }
     
      
        /// <summary>
        /// Creates two shaders and then links them together to create a shader program.
        /// </summary>
        /// <param name="vertexShaderSource">Specifies the source code of the vertex shader.</param>
        /// <param name="fragmentShaderSource">Specifies the source code of the fragment shader.</param>
        public ShaderProgram(string vertexShaderSource, string fragmentShaderSource)
            : this(new Shader(vertexShaderSource, ShaderType.VertexShader), new Shader(fragmentShaderSource, ShaderType.FragmentShader))
        {
            DisposeChildren = true;
        }

        private void ResetShader(Shader vertexShader, Shader fragmentShader)
        {
            this.VertexShader = vertexShader;
            this.FragmentShader = fragmentShader;
            this.ProgramID = Gl.CreateProgram();
            this.DisposeChildren = false;

            Gl.AttachShader(ProgramID, vertexShader.ShaderID);
            Gl.AttachShader(ProgramID, fragmentShader.ShaderID);
            Gl.LinkProgram(ProgramID);
            ValidateLink();
            GetParams();

        }
      
        private bool ValidateLink()
        {
            OpenTK.Graphics.OpenGL.GL.ValidateProgram(ProgramID);
            int program_ok;

            OpenTK.Graphics.OpenGL.GL.GetProgram(ProgramID, OpenTK.Graphics.OpenGL.GetProgramParameterName.LinkStatus, out program_ok);

            if (program_ok == 0)
            {
                throw new Exception("Shader linking failed ");
            }
            return true;

        }
        ~ShaderProgram()
        {
            if (ProgramID != 0) 
                System.Diagnostics.Debug.Fail("ShaderProgram was not disposed of properly.");
        }
        #endregion


        #region init

    
       
        private void Init()
        {
            if (!IsSupported)
            {
                System.Diagnostics.Debug.WriteLine("Failed to create Shader." +
                    Environment.NewLine + "Your system doesn't support Shader.", "Error");

                throw new Exception("Error: Shaders are not supported by this system");

            }
            //ProgramID = GL.CreateProgram();
        }
        public static bool IsSupported
        {
            get
            {
                return (new Version(OpenTK.Graphics.OpenGL.GL.GetString(OpenTK.Graphics.OpenGL.StringName.Version).Substring(0, 3)) >= new Version(2, 0) ? true : false);
            }
        }
        #endregion


        #region GetParams
        /// <summary>
        /// Parses all of the parameters (attributes/uniforms) from the two attached shaders
        /// and then loads their location by passing this shader program into the parameter object.
        /// </summary>
        private void GetParams()
        {
            shaderParams = new Dictionary<string, ShaderParam>();
            foreach (ShaderParam pParam in VertexShader.ShaderParams)
            {
                if (!shaderParams.ContainsKey(pParam.Name))
                {
                    shaderParams.Add(pParam.Name, pParam);
                    pParam.GetLocation(this);
                }
            }
            foreach (ShaderParam pParam in FragmentShader.ShaderParams)
            {
                if (!shaderParams.ContainsKey(pParam.Name))
                {
                    shaderParams.Add(pParam.Name, pParam);
                    pParam.GetLocation(this);
                }
            }
        }
        #endregion

        #region Methods
        public void Use()
        {
            if (Gl.CurrentProgram != ProgramID) 
                Gl.UseProgram(this.ProgramID);
        }

        public int GetUniformLocation(string Name)
        {
            Use();
            return Gl.GetUniformLocation(ProgramID, Name);
        }

        public int GetAttributeLocation(string Name)
        {
            Use();
            return Gl.GetAttribLocation(ProgramID, Name);
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
            if (ProgramID != 0)
            {
                // Make sure this program isn't being used
                if (Gl.CurrentProgram == ProgramID) Gl.UseProgram(0);

                Gl.DetachShader(ProgramID, VertexShader.ShaderID);
                Gl.DetachShader(ProgramID, FragmentShader.ShaderID);
                Gl.DeleteProgram(ProgramID);

                if (DisposeChildren)
                {
                    VertexShader.Dispose();
                    FragmentShader.Dispose();
                }

                this.ProgramID = 0;
            }
        }
        #endregion

        public static string VertexShaderDefault = @"
#version 130

in vec3 vertexPosition;
in vec3 vertexNormal;
in vec3 vertexTangent;
in vec2 vertexUV;
in vec3 vertexColor;

uniform vec3 light_direction;

out vec3 normal;
out vec2 uv;
out vec3 light;
out vec4 color;

uniform mat4 projection_matrix;
uniform mat4 view_matrix;
uniform mat4 model_matrix;
uniform bool enable_mapping;

void main(void)
{
    normal = normalize((model_matrix * vec4(floor(vertexNormal), 0)).xyz);
    uv = vertexUV;

    mat3 tbnMatrix = mat3(vertexTangent, cross(vertexTangent, normal), normal);
    light = (enable_mapping ? light_direction * tbnMatrix : light_direction);

    gl_Position = projection_matrix * view_matrix * model_matrix * vec4(vertexPosition, 1);
    color = vec4( vertexColor, 1.0);
}
";

        public static string FragmentShaderDefault = @"
#version 130

uniform sampler2D colorTexture;
uniform sampler2D normalTexture;

uniform bool enable_lighting;
uniform mat4 model_matrix;
uniform bool enable_mapping;
uniform bool colorOutput;

in vec3 normal;
in vec2 uv;
in vec3 light;
out vec4 fragment;
in vec4 color;
out vec4 outputColor;
void main(void)
{
    vec3 fragmentNormal = texture2D(normalTexture, uv).xyz * 2 - 1;
    vec3 selectedNormal = (enable_mapping ? fragmentNormal : normal);
    float diffuse = max(dot(selectedNormal, light), 0);
    float ambient = 0.3;
    float lighting = (enable_lighting ? max(diffuse, ambient) : 1);

    fragment = (colorOutput ? color : vec4(lighting * texture2D(colorTexture, uv).xyz, 1));
}
";

   
        public static string VertexShaderOld = @"
#version 130

in vec3 vertexPosition;
in vec3 vertexNormal;
in vec3 vertexTangent;
in vec2 vertexUV;

uniform vec3 light_direction;

out vec3 normal;
out vec2 uv;
out vec3 light;

uniform mat4 projection_matrix;
uniform mat4 view_matrix;
uniform mat4 model_matrix;
uniform bool enable_mapping;

void main(void)
{
    normal = normalize((model_matrix * vec4(floor(vertexNormal), 0)).xyz);
    uv = vertexUV;

    mat3 tbnMatrix = mat3(vertexTangent, cross(vertexTangent, normal), normal);
    light = (enable_mapping ? light_direction * tbnMatrix : light_direction);

    gl_Position = projection_matrix * view_matrix * model_matrix * vec4(vertexPosition, 1);
}
";

        public static string FragmentShaderOld = @"
#version 130

uniform sampler2D colorTexture;
uniform sampler2D normalTexture;

uniform bool enable_lighting;
uniform mat4 model_matrix;
uniform bool enable_mapping;

in vec3 normal;
in vec2 uv;
in vec3 light;

out vec4 fragment;

void main(void)
{
    vec3 fragmentNormal = texture2D(normalTexture, uv).xyz * 2 - 1;
    vec3 selectedNormal = (enable_mapping ? fragmentNormal : normal);
    float diffuse = max(dot(selectedNormal, light), 0);
    float ambient = 0.3;
    float lighting = (enable_lighting ? max(diffuse, ambient) : 1);

    fragment = vec4(lighting * texture2D(colorTexture, uv).xyz, 1);
}
";
    }
}