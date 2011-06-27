using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using drw = System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing.Imaging;

namespace ORTS.Core.OpenTKHelper
{
    public class TextWriter
    {
        private readonly drw.Font TextFont = new drw.Font(drw.FontFamily.GenericSansSerif, 8);
        private readonly drw.Bitmap TextBitmap;
        private List<drw.PointF> _positions;
        private List<string> _lines;
        private List<drw.Brush> _colours;
        private int _textureId;

        public void Update(int ind, string newText)
        {
            if (ind < _lines.Count)
            {
                _lines[ind] = newText;
                UpdateText();
            }
        }


        public TextWriter(drw.Size areaSize)
        {
            _positions = new List<drw.PointF>();
            _lines = new List<string>();
            _colours = new List<drw.Brush>();

            TextBitmap = new drw.Bitmap(areaSize.Width, areaSize.Height);
             _textureId = CreateTexture();
        }

        private int CreateTexture()
        {
            int textureId;
            GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (float)TextureEnvMode.Replace);//Important, or wrong color on some computers
            drw.Bitmap bitmap = TextBitmap;
            GL.GenTextures(1, out textureId);
            GL.BindTexture(TextureTarget.Texture2D, textureId);
            BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.Finish();
            bitmap.UnlockBits(data);
            return textureId;
        }

        public void Dispose()
        {
            if (_textureId > 0)
                GL.DeleteTexture(_textureId);
        }

        public void Clear()
        {
            _lines.Clear();
            _positions.Clear();
            _colours.Clear();
        }

        public void AddLine(string s, drw.PointF pos, drw.Brush col)
        {
            _lines.Add(s);
            _positions.Add(pos);
            _colours.Add(col);
            UpdateText();
        }

        public void UpdateText()
        {
            if (_lines.Count > 0)
            {
                using (drw.Graphics gfx = drw.Graphics.FromImage(TextBitmap))
                {
                    gfx.Clear(drw.Color.Black);
                    gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                    for (int i = 0; i < _lines.Count; i++)
                        gfx.DrawString(_lines[i], TextFont, _colours[i], _positions[i]);
                }

                System.Drawing.Imaging.BitmapData data = TextBitmap.LockBits(new drw.Rectangle(0, 0, TextBitmap.Width, TextBitmap.Height),
                    System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, TextBitmap.Width, TextBitmap.Height, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
                TextBitmap.UnlockBits(data);
            }
        }

        public void Draw()
        {
            GL.PushMatrix();
            GL.LoadIdentity();

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.DstColor);
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, _textureId);


            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(0, 0); GL.Vertex2(0, 0);
            GL.TexCoord2(1, 0); GL.Vertex2(TextBitmap.Width, 0);
            GL.TexCoord2(1, 1); GL.Vertex2(TextBitmap.Width, TextBitmap.Height);
            GL.TexCoord2(0, 1); GL.Vertex2(0, TextBitmap.Height);
            GL.End();

            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.PopMatrix();
        }
    }
}
