#version 330
precision highp float;

varying vec4 vTexCoord;

void main(void)
{
    //gl_Position = position;
	//fragment.color = colour;
	gl_Position = gl_Vertex;
	gl_TexCoord[0] = gl_MultiTexCoord0;
}