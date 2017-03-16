#version 130

uniform sampler2D myTexture;
varying vec2 uv;
out vec4 fragment;

void main(void)
{
	fragment = texture2D(texture, uv);
}