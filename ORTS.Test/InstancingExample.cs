using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;

namespace ORTS.Test
{
    class InstancingExample : GameWindow
    {

        private float[] square_vertices = new float[]{
            -1.0f, -1.0f, 0.0f, 1.0f,
             1.0f, -1.0f, 0.0f, 1.0f,
             1.0f,  1.0f, 0.0f, 1.0f,
            -1.0f,  1.0f, 0.0f, 1.0f
        };
        private float[] instance_colours = new float[]{
            1.0f, 0.0f, 0.0f, 1.0f,
            0.0f, 1.0f, 0.0f, 1.0f,
            0.0f, 0.0f, 1.0f, 1.0f,
            1.0f, 1.0f, 0.0f, 1.0f
        };

        private float[] instance_positions = new float[]{
            -2.0f, -2.0f, 0.0f, 0.0f,
             2.0f, -2.0f, 0.0f, 0.0f,
             2.0f,  2.0f, 0.0f, 0.0f,
            -2.0f,  2.0f, 0.0f, 0.0f
        };


        public InstancingExample()
            : base(800, 600, new GraphicsMode(32, 24, 0, 2), "ORTS.Test")
        {
            VSync = VSyncMode.Off;

        }

        int shaderProgram = 0;
        int square_vao, square_vbo;
        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(new Color4(0.137f, 0.121f, 0.125f, 0f));
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.EnableClientState(ArrayCap.ColorArray);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

            shaderProgram = GL.CreateProgram();

            string info;

            int vertHandle = GL.CreateShader(ShaderType.VertexShader);
            using (StreamReader sr = new StreamReader("instancing.vertexshader"))
            {
                GL.ShaderSource(vertHandle, sr.ReadToEnd());
            }
            GL.CompileShader(vertHandle);
            GL.GetShaderInfoLog(vertHandle, out info);
            Console.WriteLine(info);
            int compileResult;
            GL.GetShader(vertHandle, ShaderParameter.CompileStatus, out compileResult);
            if (compileResult != 1)
            {
                Console.WriteLine("Compile Error!");
            }
            GL.AttachShader(shaderProgram, vertHandle);

            int fragHandle = GL.CreateShader(ShaderType.FragmentShader);
            using (StreamReader sr = new StreamReader("instancing.fragmentshader"))
            {
                GL.ShaderSource(fragHandle, sr.ReadToEnd());
            }
            GL.CompileShader(fragHandle);
            GL.GetShaderInfoLog(fragHandle, out info);
            Console.WriteLine(info);
            GL.GetShader(fragHandle, ShaderParameter.CompileStatus, out compileResult);
            if (compileResult != 1)
            {
                Console.WriteLine("Compile Error!");
            }
            GL.AttachShader(shaderProgram, fragHandle);

            GL.BindAttribLocation(shaderProgram, 0, "position");
            GL.BindAttribLocation(shaderProgram, 1, "instance_color");
            GL.BindAttribLocation(shaderProgram, 2, "instance_position");

            GL.LinkProgram(shaderProgram);

            GL.GetProgramInfoLog(shaderProgram, out info);
            Console.WriteLine(info);

            if (vertHandle != 0)
                GL.DeleteShader(vertHandle);
            if (fragHandle != 0)
                GL.DeleteShader(fragHandle);

            int offset = 0;
            GL.GenVertexArrays(1, out square_vao);
            GL.GenBuffers(1, out square_vbo);
            GL.BindVertexArray(square_vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, square_vbo);

            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)((square_vertices.Length * sizeof(float)) + (instance_colours.Length * sizeof(float)) + (instance_positions.Length * sizeof(float))), (IntPtr)null, BufferUsageHint.StaticDraw);

            GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)offset, (IntPtr)(square_vertices.Length * sizeof(float)), square_vertices);
            offset += (square_vertices.Length * sizeof(float));
            GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)offset, (IntPtr)(instance_colours.Length * sizeof(float)), instance_colours);
            offset += (instance_colours.Length * sizeof(float));
            GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)offset, (IntPtr)(instance_positions.Length * sizeof(float)), instance_positions);
            offset += (instance_positions.Length * sizeof(float));

            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 0, 0);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 0, (square_vertices.Length * sizeof(float)));
            GL.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, 0, (square_vertices.Length * sizeof(float)) + (instance_colours.Length * sizeof(float)));

            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);

            GL.Arb.VertexAttribDivisor(1, 1);
            GL.Arb.VertexAttribDivisor(2, 1);

        }




        protected override void OnRenderFrame(FrameEventArgs e)
        {

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.LoadIdentity();

            Matrix4 lookat = Matrix4.LookAt(0, 0, 50, 0, 0, 0, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookat);
            GL.UseProgram(0);
            GL.Begin(BeginMode.Lines);
            GL.Color4(Color4.Red);
            GL.Vertex3(0f, 0f, 0f);
            GL.Vertex3(1f, 0f, 0f);
            GL.Color4(Color4.Green);
            GL.Vertex3(0f, 0f, 0f);
            GL.Vertex3(0f, 1f, 0f);
            GL.Color4(Color4.Blue);
            GL.Vertex3(0f, 0f, 0f);
            GL.Vertex3(0f, 0f, 1f);
            GL.End();
            GL.Color4(Color4.White);

            GL.UseProgram(shaderProgram);
            GL.BindVertexArray(square_vao);
            GL.DrawArraysInstanced(BeginMode.TriangleFan, 0, 4, 4);

            this.Title = "FPS: " + string.Format("{0:F}", 1.0 / e.Time);
            this.SwapBuffers();
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Width, Height);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)Width / (float)Height, 1, 128);
            GL.LoadMatrix(ref perspective);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }
        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }
    }
}
