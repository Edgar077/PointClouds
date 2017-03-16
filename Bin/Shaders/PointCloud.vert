#version 330 core
  
layout(location = 0) in vec3 vVertex;  //object space vertex position
layout(location = 1) in  vec3 vColor;

uniform mat4 MVP;

out vec4 color;

void main()
{ 	 
	//get clipspace position
	gl_Position = MVP * vec4(vVertex, 1);
	color = vec4( vColor, 1.0);
}