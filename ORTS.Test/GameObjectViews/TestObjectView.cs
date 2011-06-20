using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORTS.Core.Graphics;
using ORTS.Test.GameObjects;
using ORTS.Core.GameObject;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using ORTS.Core.OpenTKHelper;
using System.Drawing;
using AwesomiumSharp;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ORTS.Test.GameObjectViews
{
    class TestObjectView : IGameObjectView
    {
        public bool Loaded { get; private set; }

        WebView webView;

        int texture;
        
        public TestObjectView()
        {
            this.Loaded = false;

        }

        public void Load()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.GenTextures(1, out texture);
            GL.BindTexture(TextureTarget.Texture2D, texture);


            
            
            WebCore.Initialize(new WebCore.Config());
            webView = WebCore.CreateWebview(1000, 1000);
            webView.LoadURL("http://www.google.com");

            /*
            List = GL.GenLists(1);
            GL.NewList(List, ListMode.Compile);

            GL.Begin(BeginMode.Quads);
            GL.Vertex3(-0.5f, -0.5f, -0.5f);
            GL.Vertex3(-0.5f, 0.5f, -0.5f);
            GL.Vertex3(0.5f, 0.5f, -0.5f);
            GL.Vertex3(0.5f, -0.5f, -0.5f);

            GL.Vertex3(-0.5f, -0.5f, -0.5f);
            GL.Vertex3(0.5f, -0.5f, -0.5f);
            GL.Vertex3(0.5f, -0.5f, 0.5f);
            GL.Vertex3(-0.5f, -0.5f, 0.5f);

            GL.Vertex3(-0.5f, -0.5f, -0.5f);
            GL.Vertex3(-0.5f, -0.5f, 0.5f);
            GL.Vertex3(-0.5f, 0.5f, 0.5f);
            GL.Vertex3(-0.5f, 0.5f, -0.5f);

            GL.Vertex3(-0.5f, -0.5f, 0.5f);
            GL.Vertex3(0.5f, -0.5f, 0.5f);
            GL.Vertex3(0.5f, 0.5f, 0.5f);
            GL.Vertex3(-0.5f, 0.5f, 0.5f);

            GL.Vertex3(-0.5f, 0.5f, -0.5f);
            GL.Vertex3(-0.5f, 0.5f, 0.5f);
            GL.Vertex3(0.5f, 0.5f, 0.5f);
            GL.Vertex3(0.5f, 0.5f, -0.5f);

            GL.Vertex3(0.5f, -0.5f, -0.5f);
            GL.Vertex3(0.5f, 0.5f, -0.5f);
            GL.Vertex3(0.5f, 0.5f, 0.5f);
            GL.Vertex3(0.5f, -0.5f, 0.5f);

            GL.End();
            GL.EndList();
            */
            this.Loaded = true;
        }

        public void Render(IHasGeometry GameObject)
        {
            WebCore.Update();


            RenderBuffer rBuffer = webView.Render();

            int[] data = new int[100 * 100];
            Marshal.Copy(rBuffer.GetBuffer(), data, 0, 100 * 100);
            Bitmap bmp = new Bitmap(100, 100, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            BitmapData bits = bmp.LockBits(new Rectangle(0, 0, 100, 100),
                              ImageLockMode.ReadWrite, bmp.PixelFormat);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bits.Width, bits.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bits.Scan0);

            bmp.UnlockBits(bits);


            GL.Translate(GameObject.Position.ToVector3());
            GL.BindTexture(TextureTarget.Texture2D, texture);

            GL.Begin(BeginMode.Quads);

            GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(0,0);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(100,0);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(100,100);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(0,100);

            GL.End();




           // if (!webView.IsLoadingPage())
          //  {
            //GL.Translate(GameObject.Position.ToVector3());
           // GL.CallList(List);
               // GL.Translate(new Vector3(10, 10, 100));
             //   GL.PixelZoom(1.0f, -1.0f); 
               // GL.DrawPixels(100, 100, PixelFormat.Bgra, PixelType.UnsignedByte, webView.Render().GetBuffer());
           // }
                

            /*
            TestObject ob = GameObject as TestObject;
            GL.Color3(ob.Color);
            
            ;*/
        }
        public void Unload()
        {
            //GL.DeleteLists(List, 1);
        }
    }
}
