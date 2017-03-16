#version 130

in vec3 vVertex;  //object space vertex position
in vec2 vUV;
uniform mat4 MVP;
out vec2 uv;
void main(void)
{
    uv = vUV;
    gl_Position = MVP*vec4(vVertex,1);
}