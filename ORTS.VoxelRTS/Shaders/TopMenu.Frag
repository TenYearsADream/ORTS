#version 330
precision highp float;
uniform sampler2D tex;
void main (void)
{
	gl_FragColor =  texture2D(tex, gl_TexCoord[0].xy);
}