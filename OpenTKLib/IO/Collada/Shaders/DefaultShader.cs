using OpenTK.Graphics.OpenGL;

namespace OpenTKExtension.Collada
{
    public class DefaultShader : BaseShader
	{
		public int ProjectionMatrix { get; private set; }
		public int ModelViewMatrix { get; private set; }
		public int Texture { get; private set; }
		public int HaveTexture { get; private set; }

		public DefaultShader(): base("default") {}

		protected override void SetUniforms()
		{
			ProjectionMatrix = GL.GetUniformLocation(ShaderProgram, "projection_matrix");
			ModelViewMatrix = GL.GetUniformLocation(ShaderProgram, "modelview_matrix");
			Texture = GL.GetUniformLocation(ShaderProgram, "main_texture");
			HaveTexture = GL.GetUniformLocation(ShaderProgram, "have_texture");
		}
	}
}