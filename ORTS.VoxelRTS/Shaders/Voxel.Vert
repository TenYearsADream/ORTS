#version 330
precision highp float;

layout (location = 0) in vec3 position;
layout (location = 1) in vec3 instance_position;
layout (location = 2) in vec4 instance_rotation;


vec3 qrot(vec4 q, vec3 v)       {
        return v + 2.0*cross(q.xyz, cross(q.xyz,v) + q.w*v);
}

void main(void)
{
    gl_Position = gl_ModelViewProjectionMatrix * vec4(instance_position + qrot(instance_rotation,position) , 1.0);
}