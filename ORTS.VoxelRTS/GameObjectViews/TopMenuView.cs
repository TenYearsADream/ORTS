using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using AwesomiumSharp;
using ORTS.Core.Graphics;
using ORTS.Core.GameObject;
using ORTS.Core.OpenTKHelper;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ORTS.VoxelRTS.GameObjectViews
{

    class TopMenuView : IGameObjectView
    {
        public bool Loaded { get; private set; }


        private struct Vertex
        {
            public Vector3 Position;
            public Vector4 Colour;
            public const int SizeInBytes = 28;
        }


        private VoxelRTSWindow _voxelRTSWindow;

        private ShaderProgram _shader;

        public TopMenuView(VoxelRTSWindow window)
        {
            Loaded = false;
            _voxelRTSWindow = window;
        }

        int _squareVao, _squareVbo;
        private Bitmap bitmap = new Bitmap("Textures/Blue.png");
        int texture;
        private WebView webView;
        public void Load()
        {

            WebCore.Initialize(new WebCore.Config());
            webView = WebCore.CreateWebview(_voxelRTSWindow.Width, _voxelRTSWindow.Height);
            webView.LoadURL("http://www.google.com");
            GL.GenTextures(1, out texture);
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            /*
            var data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            bitmap.UnlockBits(data);*/


            _shader = new ShaderProgram();
            _shader.OnMessage += _shader_OnMessage;
            using (var sr = new StreamReader("Shaders/TopMenu.Vert"))
            {
                _shader.AddShader(ShaderType.VertexShader, sr.ReadToEnd());
            }
            using (var sr = new StreamReader("Shaders/TopMenu.Frag"))
            {
                _shader.AddShader(ShaderType.FragmentShader, sr.ReadToEnd());
            }
            _shader.Link();

            UpdateTexture();

            /*
            var vertexData = new Vertex[4];
            vertexData[0].Position = new Vector3(-1f, -1f, 0f);
            vertexData[1].Position = new Vector3( 1f, -1f, 0f);
            vertexData[2].Position = new Vector3( 1f,  1f, 0f);
            vertexData[3].Position = new Vector3(-1f,  1f, 0f);

            vertexData[0].Colour = new Vector4(255f, 0f, 0f, 1f);
            vertexData[1].Colour = new Vector4(255f, 0f, 0f, 1f);
            vertexData[2].Colour = new Vector4(255f, 0f, 0f, 1f);
            vertexData[3].Colour = new Vector4(255f, 0f, 0f, 1f);


            _shader = new ShaderProgram();
            _shader.OnMessage += _shader_OnMessage;
            using (var sr = new StreamReader("Shaders/TopMenu.Vert"))
            {
                _shader.AddShader(ShaderType.VertexShader, sr.ReadToEnd());
            }
            using (var sr = new StreamReader("Shaders/TopMenu.Frag"))
            {
                _shader.AddShader(ShaderType.FragmentShader, sr.ReadToEnd());
            }

            GL.BindAttribLocation(_shader.Program, 0, "position");
            GL.BindAttribLocation(_shader.Program, 1, "colour");

            _shader.Link();
            GL.GenVertexArrays(1, out _squareVao);
            GL.GenBuffers(1, out _squareVbo);
            GL.BindVertexArray(_squareVao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _squareVbo);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vertexData.Length * Vertex.SizeInBytes), vertexData, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 0);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, (IntPtr)(3 * sizeof(float)));

            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
             */ 
            Loaded = true;
        }

        void UpdateTexture()
        {
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, _voxelRTSWindow.Width, _voxelRTSWindow.Height, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, webView.Render().GetBuffer());
        }

        void _shader_OnMessage(string error)
        {
            Console.WriteLine(error);
        }

        public void Unload()
        {
            WebCore.Shutdown();
        }

        public void Add(IGameObject gameObject)
        {

        }

        public void Update()
        {
            WebCore.Update();
            if(webView.IsDirty())
                UpdateTexture();
        }

        public void Render()
        {
            _shader.Enable();
           // GL.BindVertexArray(_squareVao);
           // GL.DrawArrays(BeginMode.Quads,0,4);
            GL.PushMatrix();
            GL.BindTexture(TextureTarget.Texture2D, texture);

            GL.Begin(BeginMode.Quads);

            GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(-1f, -1f);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex2( 1f, -1f);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex2( 1f,  1f);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(-1f,  1f);

            GL.End();
            GL.PopMatrix();

            _shader.Disable();
        }
    }
}
