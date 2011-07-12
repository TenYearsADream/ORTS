using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORTS.Core.OpenTKHelper;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;

namespace ORTS.VoxelRTS.GameObjectViews
{
    public class VoxelView
    {
        
        float size = 0.5f;

        //X,Y,Z
        private float[] square_vertices;

        private static int number = 1600;

        //R,G,B,A
        private float[] instance_colours = new float[number*4];
        //X,Y,Z
        private float[] instance_positions = new float[number*3];

        private ShaderProgram shader;
        int square_vao, square_vbo;
        public VoxelView()
        {
            square_vertices = new float[]{
                //Front Face
                -size, -size, size,
                size, -size, size,
                size, size, size,
                -size, size, size,
                //Back Face
                -size, -size, -size,
                -size, size, -size,
                size, size, -size,
                size, -size, -size,
                //Top Face
                -size, size, -size,
                -size, size, size,
                size, size, size,
                size, size, -size,
                //Bottom Face
                -size, -size, -size,
                size, -size, -size,
                size, -size, size,
                -size, -size, size,
                //Right Face
                size, -size, -size,
                size, size, -size,
                size, size, size,
                size, -size, size,
                //Left Face
                -size, -size, -size,
                -size, -size, size,
                -size, size, size,
                -size, size, -size
            };
            
            Random rnd = new Random();
            for (int i = 0; i < number; i++)
            {
                instance_positions[(i * 3)] = (float)(rnd.Next(-20, 20));
                instance_positions[(i * 3) + 1] = (float)(rnd.Next(-20, 20));
                instance_positions[(i * 3) + 2] = (float)(rnd.Next(-20, 20));
                instance_colours[(i * 4)] = (float)rnd.NextDouble();
                instance_colours[(i * 4) + 1] = (float)rnd.NextDouble();
                instance_colours[(i * 4) + 2] = (float)rnd.NextDouble();
                instance_colours[(i * 4) + 3] = 0.5f;
            }

            shader = new ShaderProgram();
            shader.OnMessage += new ShaderMessageEventHandler(shader_OnMessage);
            using (StreamReader sr = new StreamReader("Shaders/instancing.vertexshader"))
            {
                shader.AddShader(ShaderType.VertexShader, sr.ReadToEnd());
            }
            using (StreamReader sr = new StreamReader("Shaders/instancing.fragmentshader"))
            {
                shader.AddShader(ShaderType.FragmentShader, sr.ReadToEnd());
            }
            GL.BindAttribLocation(shader.shaderProgram, 0, "position");
            GL.BindAttribLocation(shader.shaderProgram, 1, "instance_color");
            GL.BindAttribLocation(shader.shaderProgram, 2, "instance_position");
            shader.Link();
            int square_vertices_size = square_vertices.Length * sizeof(float);
            int instance_colours_size = instance_colours.Length * sizeof(float);
            int instance_positions_size = instance_positions.Length * sizeof(float);

            int offset = 0;
            

            GL.GenVertexArrays(1, out square_vao);
            GL.GenBuffers(1, out square_vbo);
            GL.BindVertexArray(square_vao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, square_vbo);
            Console.WriteLine(square_vertices_size + instance_colours_size + instance_positions_size);
            GL.BufferData<float>(BufferTarget.ArrayBuffer, (IntPtr)(square_vertices_size + instance_colours_size + instance_positions_size), new float[] { }, BufferUsageHint.DynamicDraw);

            GL.BufferSubData<float>(BufferTarget.ArrayBuffer, (IntPtr)offset, (IntPtr)square_vertices_size, square_vertices);
            offset += square_vertices_size;
            GL.BufferSubData<float>(BufferTarget.ArrayBuffer, (IntPtr)offset, (IntPtr)instance_colours_size, instance_colours);
            offset += instance_colours_size;
            GL.BufferSubData<float>(BufferTarget.ArrayBuffer, (IntPtr)offset, (IntPtr)instance_positions_size, instance_positions);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 0, square_vertices_size);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 0, square_vertices_size + instance_colours_size);

            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);

            GL.Arb.VertexAttribDivisor(1, 1);
            GL.Arb.VertexAttribDivisor(2, 1);
        }

        void shader_OnMessage(string error)
        {
            Console.WriteLine(error);
        }

        public void Render(){
            shader.Enable();
            GL.BindVertexArray(square_vao);
            GL.DrawArraysInstanced(BeginMode.Quads, 0, 4*6, number);
            shader.Disable();
        }
    }
}
