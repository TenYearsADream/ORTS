#version 330
precision highp float;
uniform vec3 in_camera;

in vec3 in_position;
in float in_size;

void main(void)
{
    mat4 temp = gl_ModelViewProjectionMatrix;
	temp[3].x =  temp[3].x / 2.0;
	temp[3].y =  temp[3].y / 2.0;
	temp[3].z =  temp[3].z / 2.0;
    gl_Position = temp * vec4(in_position,1);
	gl_PointSize = in_size;
}