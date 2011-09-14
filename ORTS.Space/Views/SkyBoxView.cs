using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using ORTS.Core.Attributes;
using ORTS.Core.GameObject;
using ORTS.Core.Graphics;
using ORTS.Core.OpenTKHelper;
using ORTS.Space.GameObjects;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ORTS.Space.Views
{
    [BindView(typeof(SkyBox))]
    public class SkyBoxView : IGameObjectView
    {
        private readonly ICamera _camera;
        private ShaderProgram _shader;
        public bool Loaded { get; private set; }
        public SkyBoxView(ICamera camera)
        {
            _camera = camera;
            Loaded = false;
        }

        private int _vao, _vbo;
        private const int StarCount = 500;

        int _uniformCamera, _attribPosition, _attribSize;


        private Tuple<float,float,float> PointLoc(double x1, double x2)
        {
            //Marsaglia (1972)
            return new Tuple<float, float, float>(
                (float)(2 * x1 * Math.Sqrt(1 - Math.Pow(x1, 2) - Math.Pow(x2, 2))), 
                (float)(2 * x2 * Math.Sqrt(1 - Math.Pow(x1, 2) - Math.Pow(x2, 2))), 
                (float)(1 - 2 * (Math.Pow(x1, 2) + Math.Pow(x2, 2))));
        } 

        public void Load()
        {
            GL.Enable(EnableCap.VertexProgramPointSize);
            _shader = new ShaderProgram();
            //_shader.OnMessage += _shader_OnMessage;
            using (var sr = new StreamReader("Shaders/SkyBox.Vert"))
            {
                _shader.AddShader(ShaderType.VertexShader, sr.ReadToEnd());
            }
            using (var sr = new StreamReader("Shaders/SkyBox.Frag"))
            {
                _shader.AddShader(ShaderType.FragmentShader, sr.ReadToEnd());
            }

            _shader.Link();


            _attribPosition = GL.GetAttribLocation(_shader.Program, "in_position");
            _attribSize = GL.GetAttribLocation(_shader.Program, "in_size");
            GL.BindAttribLocation(_shader.Program, _attribPosition, "in_position");
            GL.BindAttribLocation(_shader.Program, _attribSize, "in_size");
           
            _shader.Enable();
            _uniformCamera = GL.GetUniformLocation(_shader.Program, "in_camera");
            GL.Uniform3(_uniformCamera, new Vector3(0,0,-30));
            _shader.Disable();

            //Console.WriteLine("pos:" + _attribPosition);
            //Console.WriteLine("size:" + _attribSize);
            //Console.WriteLine("uni:"+ _uniformCamera);

            var dataList = new List<float>();
            
            var rnd = new Random();

            double x1;
            double x2;
            const float radius = 30f;
            for (int i = 0; i < StarCount; i++)
            {

                    x1 = rnd.Next(-1, 2)*rnd.NextDouble();
                    x2 = rnd.Next(-1, 2)*rnd.NextDouble();


                var temp = PointLoc(x1,x2);
                dataList.Add(temp.Item1*radius);
                dataList.Add(temp.Item2*radius);
                dataList.Add(temp.Item3*radius);
                dataList.Add((float)rnd.NextDouble() * 10f);
            }


            var data = dataList.ToArray();


            GL.GenVertexArrays(1, out _vao);
            GL.GenBuffers(1, out _vbo);
            GL.BindVertexArray(_vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(data.Length * sizeof(float)), data, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(_attribPosition, 3, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
            GL.VertexAttribPointer(_attribSize, 1, VertexAttribPointerType.Float, false, 4 * sizeof(float), 3 * sizeof(float));



            GL.EnableVertexAttribArray(_attribPosition);
            GL.EnableVertexAttribArray(_attribSize);
            GL.Arb.VertexAttribDivisor(_attribPosition, 1);
            GL.Arb.VertexAttribDivisor(_attribSize, 1);
            Loaded = true;
        }

        void _shader_OnMessage(string error)
        {
            Console.WriteLine(error);
        }

        public void Unload()
        {

        }

        public void Update(IEnumerable<IGameObject> gameObjects, double delta)
        {
            //Console.WriteLine(_camera.Postion);
            

            
        }



        public void Render()
        {
            /*
            _shader.Enable();
            GL.Uniform3(_uniformCamera, new Vector3((float)_camera.Postion.X, (float)_camera.Postion.Y, (float)-_camera.Postion.Z));
            GL.PointSize(1.0f);
            GL.BindVertexArray(_vao);
            GL.DrawArraysInstanced(BeginMode.Points, 0, 1, StarCount);
            _shader.Disable();*/
        }
    }

    struct StarVertex
    {
        public Vector3 Position;
        public static int SizeInBytes = 12;
    }
}
