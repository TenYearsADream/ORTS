using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORTS.Core.Graphics;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using ORTS.Core.OpenTKHelper;
using ORTS.Core.GameObject;
using System.Drawing;
using ORTS.Core.Maths;
using OpenTK;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace ORTS.VoxelRTS.GameObjectViews
{
    class VoxelGreenView : IGameObjectView
    {
        public bool Loaded { get; private set; }
        private int List;
        private float size = 0.5f;
        private int texture;

        public VoxelGreenView()
        {
            this.Loaded = false;

        }

        public void Load()
        {

            Bitmap bitmap = new Bitmap("Textures/blue.png");
            GL.GenTextures(1, out texture);
            GL.BindTexture(TextureTarget.Texture2D, texture);
            BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bitmap.UnlockBits(data);
            bitmap.Dispose();
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            List = GL.GenLists(1);
            GL.NewList(List, ListMode.Compile);
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.Color3(Color.White);
            GL.Begin(BeginMode.Quads);

            //Front Face
            GL.TexCoord2(0.0, 0.0); 
            GL.Vertex3(-size, -size, size);
            GL.TexCoord2(1.0, 0.0); 
            GL.Vertex3(size, -size, size);
            GL.TexCoord2(1.0, 1.0);
            GL.Vertex3(size, size, size);
            GL.TexCoord2(0.0, 1.0); 
            GL.Vertex3(-size, size, size);
            //Back Face
            GL.TexCoord2(1.0, 0.0); 
            GL.Vertex3(-size, -size, -size);
            GL.TexCoord2(1.0, 1.0); 
            GL.Vertex3(-size, size, -size);
            GL.TexCoord2(0.0, 1.0);
            GL.Vertex3(size, size, -size);
            GL.TexCoord2(0.0, 0.0);
            GL.Vertex3(size, -size, -size);
            //Top Face
            GL.TexCoord2(0.0, 1.0); 
            GL.Vertex3(-size, size, -size);
            GL.TexCoord2(0.0, 0.0);
            GL.Vertex3(-size, size, size);
            GL.TexCoord2(1.0, 0.0);
            GL.Vertex3(size, size, size);
            GL.TexCoord2(1.0, 1.0);
            GL.Vertex3(size, size, -size);
            //Bottom Face
            GL.TexCoord2(1.0, 1.0); 
            GL.Vertex3(-size, -size, -size);
            GL.TexCoord2(0.0, 1.0); 
            GL.Vertex3(size, -size, -size);
            GL.TexCoord2(0.0, 0.0); 
            GL.Vertex3(size, -size, size);
            GL.TexCoord2(1.0, 0.0); 
            GL.Vertex3(-size, -size, size);
            //Right Face
            GL.TexCoord2(1.0, 0.0);
            GL.Vertex3(size, -size, -size);
            GL.TexCoord2(1.0, 1.0);
            GL.Vertex3(size, size, -size);
            GL.TexCoord2(0.0, 1.0); 
            GL.Vertex3(size, size, size);
            GL.TexCoord2(0.0, 0.0);
            GL.Vertex3(size, -size, size);
            //Left Face
            GL.TexCoord2(0.0, 0.0);
            GL.Vertex3(-size, -size, -size);
            GL.TexCoord2(1.0, 0.0); 
            GL.Vertex3(-size, -size, size);
            GL.TexCoord2(1.0, 1.0); 
            GL.Vertex3(-size, size, size);
            GL.TexCoord2(0.0, 1.0); 
            GL.Vertex3(-size, size, -size);
            GL.End();
            GL.EndList();
            this.Loaded = true;
        }


        public void Render(IHasGeometry GameObject)
        {
            GL.Translate(GameObject.Position.ToVector3());
            AxisAngle aa = GameObject.Rotation.toAxisAngle();
            GL.Rotate(aa.Angle.Degrees, aa.Axis.ToVector3d());
            GL.CallList(List);
        }
        public void Unload()
        {
            GL.DeleteLists(List, 1);
            GL.DeleteTextures(1, ref texture);
        }
    }
}
