using System;
using System.Collections.Generic;
using System.IO;
using OpenTK.Graphics.OpenGL;
using ORTS.Core.GameObject;
using ORTS.Core.Graphics;
using ORTS.Core.OpenTKHelper;

namespace ORTS.VoxelRTS.GameObjectViews
{
    class VoxelGreenView : IGameObjectView
    {
        public bool Loaded { get; private set; }
        private const float Size = 0.5f;

        private ShaderProgram _shader;
        private int _squareVao, _squareVbo;
        private float[] _squareVertices;
        private int _count;

        private const int Floatsize = sizeof(float);

        public VoxelGreenView()
        {
            Loaded = false;
        }

        public void Load()
        {
            _squareVertices = new[]{
                //Front Face
                -Size, -Size, Size,
                Size, -Size, Size,
                Size, Size, Size,
                -Size, Size, Size,
                //Back Face
                -Size, -Size, -Size,
                -Size, Size, -Size,
                Size, Size, -Size,
                Size, -Size, -Size,
                //Top Face
                -Size, Size, -Size,
                -Size, Size, Size,
                Size, Size, Size,
                Size, Size, -Size,
                //Bottom Face
                -Size, -Size, -Size,
                Size, -Size, -Size,
                Size, -Size, Size,
                -Size, -Size, Size,
                //Right Face
                Size, -Size, -Size,
                Size, Size, -Size,
                Size, Size, Size,
                Size, -Size, Size,
                //Left Face
                -Size, -Size, -Size,
                -Size, -Size, Size,
                -Size, Size, Size,
                -Size, Size, -Size
            };
            _shader = new ShaderProgram();
            using (var sr = new StreamReader("Shaders/Voxel.Vert"))
            {
                _shader.AddShader(ShaderType.VertexShader, sr.ReadToEnd());
            }
            using (var sr = new StreamReader("Shaders/VoxelGreen.Frag"))
            {
                _shader.AddShader(ShaderType.FragmentShader, sr.ReadToEnd());
            }
            GL.BindAttribLocation(_shader.Program, 0, "position");
            GL.BindAttribLocation(_shader.Program, 1, "instance_position");
            GL.BindAttribLocation(_shader.Program, 2, "instance_rotation");

            _shader.Link();
            GL.GenVertexArrays(1, out _squareVao);
            GL.GenBuffers(1, out _squareVbo);
            GL.BindVertexArray(_squareVao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _squareVbo);
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);
            GL.Arb.VertexAttribDivisor(1, 1);
            GL.Arb.VertexAttribDivisor(2, 1);

            Loaded = true;
        }

        public void Unload()
        {
           // GL.DeleteLists(List, 1);
           // GL.DeleteTextures(1, ref texture);
            
        }
        

        private readonly List<float> _positions = new List<float>();
        private readonly List<float> _rotations = new List<float>();

        //private readonly List<float> _data = new List<float>();

        private readonly List<IGameObject> _objects = new List<IGameObject>();

        public void Add(IGameObject gameObject)
        {
            _objects.Add(gameObject);


            /*_data.Add((float)gameObject.Position.X);
            _data.Add((float)gameObject.Position.Y);
            _data.Add((float)gameObject.Position.Z);
            
            _data.Add((float)gameObject.Rotation.X);
            _data.Add((float)gameObject.Rotation.Y);
            _data.Add((float)gameObject.Rotation.Z);
            _data.Add((float)gameObject.Rotation.W);
             
            _positions.Add((float)gameObject.Position.X);
            _positions.Add((float)gameObject.Position.Y);
            _positions.Add((float)gameObject.Position.Z);
            _rotations.Add((float)gameObject.Rotation.X);
            _rotations.Add((float)gameObject.Rotation.Y);
            _rotations.Add((float)gameObject.Rotation.Z);
            _rotations.Add((float)gameObject.Rotation.W);
            ;*/ 
        }
        public void Update()
        {
            /* GL.BindVertexArray(_squareVao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _squareVbo);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr((_squareVertices.Length + _data.Count) * Floatsize), IntPtr.Zero, BufferUsageHint.StreamDraw);

            GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, new IntPtr(_squareVertices.Length * Floatsize), _squareVertices);
            GL.BufferSubData(BufferTarget.ArrayBuffer, new IntPtr(_squareVertices.Length * Floatsize), new IntPtr(_data.Count * Floatsize), _data.ToArray());

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 4 * Floatsize, _squareVertices.Length * Floatsize);
            GL.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, 3 * Floatsize, _squareVertices.Length * Floatsize + 3 * Floatsize);

            _data.Clear();
           
            _count = _count2;
            _count2 = 0;
             * */
            _count = 0;
            foreach (var o in _objects)
            {
                _positions.Add((float)o.Position.X);
                _positions.Add((float)o.Position.Y);
                _positions.Add((float)o.Position.Z);
                _rotations.Add((float)o.Rotation.X);
                _rotations.Add((float)o.Rotation.Y);
                _rotations.Add((float)o.Rotation.Z);
                _rotations.Add((float)o.Rotation.W);
                _count++;
            }

            GL.BindVertexArray(_squareVao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _squareVbo);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr((_squareVertices.Length + _count * 3 + _count * 4) * Floatsize), IntPtr.Zero, BufferUsageHint.StreamDraw);
            GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, new IntPtr(_squareVertices.Length * Floatsize), _squareVertices);
            GL.BufferSubData(BufferTarget.ArrayBuffer, new IntPtr(_squareVertices.Length * Floatsize), new IntPtr(_count * 3 * Floatsize), _positions.ToArray());
            GL.BufferSubData(BufferTarget.ArrayBuffer, new IntPtr((_squareVertices.Length + _count * 3) * Floatsize), new IntPtr(_count * 4 * Floatsize), _rotations.ToArray());
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 0, _squareVertices.Length * Floatsize);
            GL.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, 0, (_squareVertices.Length + _count * 3) * Floatsize);
            _objects.Clear();
            _rotations.Clear();
            _positions.Clear();
        }

        public void Render()
        {
            _shader.Enable();
            GL.BindVertexArray(_squareVao);
            GL.DrawArraysInstanced(BeginMode.Quads, 0, 24, _count);
            _shader.Disable();
        }
    }
}
