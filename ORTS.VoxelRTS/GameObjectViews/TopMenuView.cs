using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using AwesomiumSharp;
using ORTS.Core.Graphics;
using ORTS.Core.GameObject;
using ORTS.Core.OpenTKHelper;
using OpenTK.Graphics.OpenGL;
using System.Windows.Forms;

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
        private WebView _webView;
        public void Load()
        {
            WebCore.Initialize(new WebCore.Config { customCSS = "body { font-size: 15px !important; color:white}", logLevel = LogLevel.None, enablePlugins = true, enableJavascript = true});

            _webView = WebCore.CreateWebview(_voxelRTSWindow.Width, _voxelRTSWindow.Height);
            _webView.SetTransparent(true);
            _webView.OnChangeCursor += new WebView.ChangeCursorEventArgsHandler(_webView_OnChangeCursor);
            _webView.ResetZoom();
            
            
            _voxelRTSWindow.Resize += (sender, e) => _webView.Resize(_voxelRTSWindow.Width, _voxelRTSWindow.Height);
            _voxelRTSWindow.Mouse.ButtonUp += (sender, e) => _webView.InjectMouseUp(MouseButton.Left);
            _voxelRTSWindow.Mouse.ButtonDown += (sender, e) => _webView.InjectMouseDown(MouseButton.Left);
            _voxelRTSWindow.Mouse.Move += (sender, e) => _webView.InjectMouseMove(e.X, e.Y);

            //_webView.LoadFile("/GUI/test.htm");
            _webView.LoadURL("http://youtube.com");

            
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

        void _webView_OnChangeCursor(object sender, WebView.ChangeCursorEventArgs e)
        {
            Cursor.Current = Cursors.IBeam;

            
        }

        void UpdateTexture()
        {
            GL.BindTexture(TextureTarget.Texture2D, _texture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, _voxelRTSWindow.Width, _voxelRTSWindow.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, _webView.Render().GetBuffer());
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
            if(_webView.IsDirty())
                UpdateTexture();
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
