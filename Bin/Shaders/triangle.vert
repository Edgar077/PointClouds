#version 330

in  vec3 vVertex;
in  vec3 vColor;
uniform mat4 MVP;
out vec4 color;

void
main()
{
	gl_Position = MVP * vec4(vVertex, 1.0);
    color = vec4( vColor, 1.0);
}