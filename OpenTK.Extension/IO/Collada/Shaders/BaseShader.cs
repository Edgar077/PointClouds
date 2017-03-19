using System;

using OpenTK.Graphics.OpenGL;

namespace OpenTKExtension.Collada
{
    public class BaseShader
	{
		public int ShaderProgram;

		public BaseShader(string name) 
		{
			var vertexShader = GL.CreateShader(ShaderType.VertexShader);
			var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);

			GL.ShaderSource(vertexShader, SourceLoader.Read(@"shaders.{name}.vs.glsl"));
			GL.CompileShader(vertexShader);
			checkCompileStatus(@"Vertex Shader: {name}", vertexShader);

			GL.ShaderSource(fragmentShader, SourceLoader.Read(@"shaders.{name}.fs.glsl"));
			GL.CompileShader(fragmentShader);
			checkCompileStatus(@"Fragment Shader: {name}", fragmentShader);
			
			ShaderProgram = GL.CreateProgram();
			GL.AttachShader(ShaderProgram, fragmentShader);
			GL.AttachShader(ShaderProgram, vertexShader);
			GL.LinkProgram(ShaderProgram);
			GL.UseProgram(ShaderProgram);

			GL.ValidateProgram(ShaderProgram);
			Console.WriteLine(GL.GetProgramInfoLog(ShaderProgram));

			SetUniforms();
		}

		protected virtual void SetUniforms() 
		{

		}

		private void checkCompileStatus(string shaderName, int shader)
		{
			int compileStatus;

			GL.GetShader(shader, ShaderParameter.CompileStatus, out compileStatus);
			if (compileStatus != 1)
                throw new ApplicationException(@"Filed to Compiler {shaderName}: {GL.GetShaderInfoLog(shader)}");
		}
	}
}
