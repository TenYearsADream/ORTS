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
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct VertexPosition
    {
        public Vector3 Position;
        public VertexPosition(float x, float y, float z)
        {
            Position = new Vector3(x, y, z);
        }

    }

    class VoxelGreenView : IGameObjectView
    {
        public bool Loaded { get; private set; }
        private int List;
        private float halfsize = 0.1f;
        private int texture;
        /*
         * VBO test
        struct Vbo { public int VboID, EboID, NumElements; }
        Vbo vbo = new Vbo();
        VertexPosition[] CubeVertices = new VertexPosition[]
        {
                new VertexPosition(-1.0f, -1.0f,  1.0f),
                new VertexPosition( 1.0f, -1.0f,  1.0f),
                new VertexPosition( 1.0f,  1.0f,  1.0f),
                new VertexPosition(-1.0f,  1.0f,  1.0f),
                new VertexPosition(-1.0f, -1.0f, -1.0f),
                new VertexPosition( 1.0f, -1.0f, -1.0f), 
                new VertexPosition( 1.0f,  1.0f, -1.0f),
                new VertexPosition(-1.0f,  1.0f, -1.0f) 
        };

        readonly short[] CubeElements = new short[]
        {
            0, 1, 2, 2, 3, 0, // front face
            3, 2, 6, 6, 7, 3, // top face
            7, 6, 5, 5, 4, 7, // back face
            4, 0, 3, 3, 7, 4, // left face
            0, 1, 5, 5, 4, 0, // bottom face
            1, 5, 6, 6, 2, 1, // right face
        };
        */
        public VoxelGreenView()
        {
            this.Loaded = false;

        }

        public void Load()
        {

            Bitmap bitmap = new Bitmap("GameObjectViews/test.jpg");
            GL.GenTextures(1, out texture);
            GL.BindTexture(TextureTarget.Texture2D, texture);
            BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bitmap.UnlockBits(data);
            bitmap.Dispose();
 

            /*
             * VBO TEST
            vbo = new Vbo();
            int size;
            GL.GenBuffers(1, out vbo.VboID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo.VboID);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(CubeVertices.Length * BlittableValueType.StrideOf(CubeVertices)), CubeVertices,
                          BufferUsageHint.StaticDraw);
            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);
            if (CubeVertices.Length * BlittableValueType.StrideOf(CubeVertices) != size)
                throw new ApplicationException("Vertex data not uploaded correctly");


            GL.GenBuffers(1, out vbo.EboID);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, vbo.EboID);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(CubeElements.Length * sizeof(short)), CubeElements,
                          BufferUsageHint.StaticDraw);
            GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out size);
            if (CubeElements.Length * sizeof(short) != size)
                throw new ApplicationException("Element data not uploaded correctly");
            vbo.NumElements = CubeElements.Length;
            */


            //    v6----- v5
            //   /|      /|
            //  v1------v0|
            //  | |     | |
            //  | |v7---|-|v4
            //  |/      |/
            //  v2------v3

            List = GL.GenLists(1);
            GL.NewList(List, ListMode.Compile);

            GL.Color3(Color.Green);

            GL.Begin(BeginMode.Quads);

            // face v0-v1-v2-v3
            
            GL.Normal3(0, 0, 1);
            GL.Vertex3(1, 1, 1);
            GL.Vertex3(-1, 1, 1);
            GL.Vertex3(-1, -1, 1);
            GL.Vertex3(1, -1, 1);

            // face v0-v3-v4-v6
            GL.Normal3(1, 0, 0);
            GL.Vertex3(1, 1, 1);
            GL.Vertex3(1, -1, 1);
            GL.Vertex3(1, -1, -1);
            GL.Vertex3(1, 1, -1);

            // face v0-v5-v6-v1
            GL.Normal3(0, 1, 0);
            GL.Vertex3(1, 1, 1);
            GL.Vertex3(1, 1, -1);
            GL.Vertex3(-1, 1, -1);
            GL.Vertex3(-1, 1, 1);

            // face  v1-v6-v7-v2
            GL.Normal3(-1, 0, 0);
            GL.Vertex3(-1, 1, 1);
            GL.Vertex3(-1, 1, -1);
            GL.Vertex3(-1, -1, -1);
            GL.Vertex3(-1, -1, 1);

            // face v7-v4-v3-v2
            GL.Normal3(0, -1, 0);
            GL.Vertex3(-1, -1, -1);
            GL.Vertex3(1, -1, -1);
            GL.Vertex3(1, -1, 1);
            GL.Vertex3(-1, -1, 1);

            // face v4-v7-v6-v5
            GL.Normal3(0, 0, -1);
            GL.Vertex3(1, -1, -1);
            GL.Vertex3(-1, -1, -1);
            GL.Vertex3(-1, 1, -1);
            GL.Vertex3(1, 1, -1);


            /*
            GL.Vertex3(-halfsize, -halfsize, -halfsize);
            GL.Vertex3(-halfsize, halfsize, -halfsize);
            GL.Vertex3(halfsize, halfsize, -halfsize);
            GL.Vertex3(halfsize, -halfsize, -halfsize);

            GL.Vertex3(-halfsize, -halfsize, -halfsize);
            GL.Vertex3(halfsize, -halfsize, -halfsize);
            GL.Vertex3(halfsize, -halfsize, halfsize);
            GL.Vertex3(-halfsize, -halfsize, halfsize);

            GL.Vertex3(-halfsize, -halfsize, -halfsize);
            GL.Vertex3(-halfsize, -halfsize, halfsize);
            GL.Vertex3(-halfsize, halfsize, halfsize);
            GL.Vertex3(-halfsize, halfsize, -halfsize);

            GL.Vertex3(-halfsize, -halfsize, halfsize);
            GL.Vertex3(halfsize, -halfsize, halfsize);
            GL.Vertex3(halfsize, halfsize, halfsize);
            GL.Vertex3(-halfsize, halfsize, halfsize);

            GL.Vertex3(-halfsize, halfsize, -halfsize);
            GL.Vertex3(-halfsize, halfsize, halfsize);
            GL.Vertex3(halfsize, halfsize, halfsize);
            GL.Vertex3(halfsize, halfsize, -halfsize);

            GL.Vertex3(halfsize, -halfsize, -halfsize);
            GL.Vertex3(halfsize, halfsize, -halfsize);
            GL.Vertex3(halfsize, halfsize, halfsize);
            GL.Vertex3(halfsize, -halfsize, halfsize);
            */
            GL.End();
            
            GL.EndList();
            
            this.Loaded = true;
        }


        public void Render(IHasGeometry GameObject)
        {
            GL.Translate(GameObject.Position.ToVector3());
            AxisAngle aa = GameObject.Rotation.toAxisAngle();
            GL.Rotate(aa.Angle.Degrees, aa.Axis.ToVector3d());
            //GL.CallList(List);


            
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.Begin(BeginMode.Quads);

            GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(-0.6f, -0.4f);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(0.6f, -0.4f);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(0.6f, 0.4f);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(-0.6f, 0.4f);

            GL.End();

            /*
             * VBO TEST
            //GL.EnableClientState(ArrayCap.ColorArray);
            GL.EnableClientState(ArrayCap.VertexArray);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo.VboID);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, vbo.EboID);

            GL.VertexPointer(3, VertexPointerType.Float, BlittableValueType.StrideOf(CubeVertices), new IntPtr(0));
            //GL.ColorPointer(4, ColorPointerType.UnsignedByte, BlittableValueType.StrideOf(CubeVertices), new IntPtr(12));
            GL.Color3(Color.Green);
            GL.DrawElements(BeginMode.Triangles, vbo.NumElements, DrawElementsType.UnsignedShort, IntPtr.Zero);
            */
        }
        public void Unload()
        {
            GL.DeleteLists(List, 1);
        }
    }
}
