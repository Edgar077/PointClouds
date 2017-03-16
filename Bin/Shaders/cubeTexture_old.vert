#version 130

in vec3 vVertex;  //object space vertex position
in vec2 vUV;

out vec2 uv;
uniform mat4 MVP;

void main(void)
{
    uv = vUV;
    gl_Position = MVP*vec4(vVertex,1);
}