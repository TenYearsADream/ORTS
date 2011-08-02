#version 330
precision highp float;
uniform sampler2D tex;
void main (void)
{
	gl_FragColor =  vec4(vec3(texture2D(tex, gl_TexCoord[0].st)),1);
}