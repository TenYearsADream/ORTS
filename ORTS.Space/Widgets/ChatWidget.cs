using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ORTS.Core.Graphics;
using ORTS.Core.Messaging;
using ORTS.Core.Messaging.Messages;
using OpenTK.Graphics.OpenGL;
using System.Drawing.Imaging;

namespace ORTS.Space.Widgets
{
    class ChatWidget : IWidget
    {
        public MessageBus Bus { get; private set; }
        public bool Active { get; private set; }
        public bool Loaded { get; private set; }
        private Rectangle _screen;

        Bitmap bitmap = new Bitmap(256, 256);
        int texture;

        public ChatWidget(MessageBus bus)
        {
            Bus = bus;
            Active = false;
            Loaded = false;
            
            
        }

        private readonly StringBuilder _message = new StringBuilder();

        public void Load(Rectangle screen)
        {
            Bus.OfType<ScreenResizeMessage>().Subscribe(m => _screen = m.Screen);


            GL.GenTextures(1, out texture);
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            using (Graphics gfx = Graphics.FromImage(bitmap))
            {
                gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                gfx.DrawString("testing", new Font(FontFamily.GenericMonospace, 11), Brushes.White, new PointF(10, 10));

            }

            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            
            bitmap.UnlockBits(data);



            Loaded = true;
        }

        public void Unload()
        {

        }

        public void Update(double delta)
        {

        }

        public void Render()
        {

            GL.Color4(1.0f, 1.0f, 1.0f, 1.0f);
            
            var _background = new Rectangle(10, 10, 400, 400);
            GL.Begin(BeginMode.Quads);
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(_background.Left, _background.Bottom, -0.1);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(_background.Right, _background.Bottom, -0.1);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(_background.Right, _background.Top, -0.1);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(_background.Left, _background.Top, -0.1);
            GL.End();
            
        }

    }
}
