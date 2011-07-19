using System;
using System.Collections.Generic;
using System.IO;
using OpenTK.Graphics.OpenGL;
using ORTS.Core.GameObject;
using ORTS.Core.Graphics;
using ORTS.Core.OpenTKHelper;
using ORTS.VoxelRTS.GameObjects;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;
namespace ORTS.VoxelRTS.GameObjectViews
{
    class VoxelGreenView : IGameObjectView
    {
        public bool Loaded { get; private set; }
        private float size = 0.5f;

        private ShaderProgram shader;
        private int square_vao, square_vbo;
        private float[] square_vertices;
        private int count = 0;

        private int floatsize = sizeof(float);

        public VoxelGreenView()
        {
            this.Loaded = false;
        }

        public void Load()
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
            shader = new ShaderProgram();
            using (StreamReader sr = new StreamReader("Shaders/Voxel.Vert"))
            {
                shader.AddShader(ShaderType.VertexShader, sr.ReadToEnd());
            }
            using (StreamReader sr = new StreamReader("Shaders/VoxelGreen.Frag"))
            {
                shader.AddShader(ShaderType.FragmentShader, sr.ReadToEnd());
            }
            GL.BindAttribLocation(shader.shaderProgram, 0, "position");
            GL.BindAttribLocation(shader.shaderProgram, 1, "instance_position");
            GL.BindAttribLocation(shader.shaderProgram, 2, "instance_rotation");

            shader.Link();
            GL.GenVertexArrays(1, out square_vao);
            GL.GenBuffers(1, out square_vbo);


            this.Loaded = true;
        }

        /*
        public void Render(IHasGeometry GameObject)
        {
            GL.Translate(GameObject.Position.ToVector3());
            AxisAngle aa = GameObject.Rotation.toAxisAngle();
            GL.Rotate(aa.Angle.Degrees, aa.Axis.ToVector3d());
           // GL.CallList(List);
        }*/
        public void Unload()
        {
           // GL.DeleteLists(List, 1);
           // GL.DeleteTextures(1, ref texture);
            
        }
        
        private List<VoxelGreen> list = new List<VoxelGreen>();
        public void Add(IGameObject GameObject)
        {
            list.Add(GameObject as VoxelGreen);
        }


        IntPtr positionmemIntPtr;
        IntPtr rotationmemIntPtr;
        public void Update()
        {
            count = list.Count;
            int positionstreamlength = count * 3 * sizeof(float);
            int rotationstreamlength = count * 4 * sizeof(float);
            unsafe
            {
                positionmemIntPtr = Marshal.AllocHGlobal(positionstreamlength);
                UnmanagedMemoryStream positionmemstream = new UnmanagedMemoryStream((byte*)positionmemIntPtr.ToPointer(), positionstreamlength, positionstreamlength, FileAccess.Write);
                BinaryWriter position_memstream_writer = new BinaryWriter(positionmemstream);

                rotationmemIntPtr = Marshal.AllocHGlobal(rotationstreamlength);
                UnmanagedMemoryStream rotationmemstream = new UnmanagedMemoryStream((byte*)rotationmemIntPtr.ToPointer(), rotationstreamlength, rotationstreamlength, FileAccess.Write);
                BinaryWriter rotation_memstream_writer = new BinaryWriter(rotationmemstream);

                //W,X,Y,Z

                foreach (VoxelGreen voxel in list)
                {
                    position_memstream_writer.Write((float)voxel.Position.X);
                    position_memstream_writer.Write((float)voxel.Position.Y);
                    position_memstream_writer.Write((float)voxel.Position.Z);

                    rotation_memstream_writer.Write((float)voxel.Rotation.X);
                    rotation_memstream_writer.Write((float)voxel.Rotation.Y);
                    rotation_memstream_writer.Write((float)voxel.Rotation.Z);
                    rotation_memstream_writer.Write((float)voxel.Rotation.W);
                }
                position_memstream_writer.Close();
                rotation_memstream_writer.Close();

                GL.BindVertexArray(square_vao);
                GL.BindBuffer(BufferTarget.ArrayBuffer, square_vbo);
                GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr((square_vertices.Length) * 4 + positionstreamlength + rotationstreamlength), IntPtr.Zero, BufferUsageHint.StreamDraw);

                GL.BufferSubData<float>(BufferTarget.ArrayBuffer, IntPtr.Zero, new IntPtr(square_vertices.Length * 4), square_vertices);
                GL.BufferSubData(BufferTarget.ArrayBuffer, new IntPtr(square_vertices.Length * 4), new IntPtr(positionstreamlength), positionmemIntPtr);

                GL.BufferSubData(BufferTarget.ArrayBuffer, new IntPtr(square_vertices.Length * 4 + positionstreamlength), new IntPtr(rotationstreamlength), rotationmemIntPtr);

                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
                GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 0, square_vertices.Length * 4);
                GL.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, 0, square_vertices.Length * 4 + positionstreamlength);
                GL.EnableVertexAttribArray(0);
                GL.EnableVertexAttribArray(1);
                GL.EnableVertexAttribArray(2);
                GL.Arb.VertexAttribDivisor(1, 1);
                GL.Arb.VertexAttribDivisor(2, 1);
                list.Clear();

                positionmemstream.Close();
                rotationmemstream.Close();
                Marshal.FreeHGlobal(positionmemIntPtr);
                Marshal.FreeHGlobal(rotationmemIntPtr);
            }




            
        }

        public void Render()
        {
            shader.Enable();
            GL.PushMatrix();
            GL.BindVertexArray(square_vao);
            GL.DrawArraysInstanced(BeginMode.Quads, 0, 4 * 6, count);
            GL.PopMatrix();
            shader.Disable();
        }
    }
}
