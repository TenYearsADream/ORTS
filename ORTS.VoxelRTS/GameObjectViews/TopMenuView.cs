using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ORTS.Core.Graphics;
using ORTS.Core.GameObject;
using ORTS.Core.OpenTKHelper;
using OpenTK.Graphics.OpenGL;


namespace ORTS.VoxelRTS.GameObjectViews
{

    class TopMenuView : IGameObjectView
    {
        public bool Loaded { get; private set; }


        private readonly VoxelRTSWindow _voxelRTSWindow;

        private ShaderProgram _shader;

        public TopMenuView(VoxelRTSWindow window)
        {
            Loaded = false;
            _voxelRTSWindow = window;

        }


        int _texture;
        private bool _isDirty = true;
        private Bitmap _bitmap;
        public void Load()
        {
            _bitmap = new Bitmap(_voxelRTSWindow.Width, _voxelRTSWindow.Height);
            _voxelRTSWindow.Resize += (sender, e) => _isDirty = true;

            GL.GenTextures(1, out _texture);
            GL.BindTexture(TextureTarget.Texture2D, _texture);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            UpdateTexture();

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
            Loaded = true;
        }



        void UpdateTexture()
        {
            _bitmap = new Bitmap(_voxelRTSWindow.Width, _voxelRTSWindow.Height);
            GL.BindTexture(TextureTarget.Texture2D, _texture);

            using (Graphics gfx = Graphics.FromImage(_bitmap))
            {
                gfx.Clear(Color.Transparent);
                gfx.FillRectangle(Brushes.White, new Rectangle(0, 50, 500, 100));
                gfx.DrawString("VoxelRTS", new Font(FontFamily.GenericSansSerif, 40, FontStyle.Regular), Brushes.Black, 200, 70);
            }

            var data = _bitmap.LockBits(new Rectangle(0, 0, _bitmap.Width, _bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            _bitmap.UnlockBits(data);
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
            throw new NotImplementedException();
        }

        public void Add(IGameObject gameObject)
        {

        }

        public void Update()
        {
            if(_isDirty)
            {
                UpdateTexture();
                _isDirty = false;
            }
               
        }

        public void Render()
        {
            _shader.Enable();
           // GL.BindVertexArray(_squareVao);
           // GL.DrawArrays(BeginMode.Quads,0,4);
            GL.PushMatrix();
            GL.BindTexture(TextureTarget.Texture2D, _texture);

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
