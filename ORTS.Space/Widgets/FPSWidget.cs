using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using ORTS.Core.Graphics;
using ORTS.Core.Messaging;
using ORTS.Core.Messaging.Messages;
using OpenTK.Graphics.OpenGL;

namespace ORTS.Space.Widgets
{
    class FPSWidget : IWidget
    {
        public MessageBus Bus { get; private set; }
        public bool Active { get; private set; }
        public bool Loaded { get; private set; }
        private IGraphics Graphics { get; set; }
        private Rectangle _screen;

        readonly Bitmap _bitmap = new Bitmap(128, 128);
        private Graphics gfx;
        private Rectangle _background = new Rectangle(2, 2, 128, 128);
        int texture;
        private int _list = -1;

        public FPSWidget(IGraphics graphics)
        {
            Bus = graphics.Bus;
            Graphics = graphics;
        }

        public void Load(Rectangle screen)
        {
            _screen = screen;
            Bus.OfType<ScreenResizeMessage>().Subscribe(m => _screen = m.Screen);
            GL.GenTextures(1, out texture);
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            gfx = System.Drawing.Graphics.FromImage(_bitmap);
            gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            UpdateImage();

            GL.NewList(_list, ListMode.Compile);
            GL.Color4(1.0f, 1.0f, 1.0f, 1.0f);
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(_background.Left, _background.Bottom, -0.1);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(_background.Right, _background.Bottom, -0.1);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(_background.Right, _background.Top, -0.1);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(_background.Left, _background.Top, -0.1);
            GL.End();
            GL.EndList();
            Loaded = true;
        }

        public void Unload()
        {
            gfx.Dispose();
        }

        private void UpdateImage()
        {
            GL.BindTexture(TextureTarget.Texture2D, texture);
            gfx.Clear(Color.FromArgb(0, Color.Empty));
            gfx.DrawString(string.Format("FPS: {0:F}", Graphics.FramesPerSecond), new Font(FontFamily.GenericMonospace, 11), Brushes.White, new PointF(0, 0));
            var data = _bitmap.LockBits(new Rectangle(0, 0, _bitmap.Width, _bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            _bitmap.UnlockBits(data);
        }
        private double _update = 0;
        public void Update(double delta)
        {
            _update = _update + delta;
            if (_update < .1) return;
            UpdateImage();
            _update = 0;
        }

        public void Render()
        {
            GL.CallList(_list);
        }
    }
}
