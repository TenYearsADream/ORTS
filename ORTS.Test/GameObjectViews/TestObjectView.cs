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

namespace ORTS.Test.GameObjectViews
{
    class TestObjectView : IGameObjectView
    {
        public bool Loaded { get; private set; }
        private int List;

        #region vbo test
        /*
        struct Vbo { public int VboID, EboID, NumElements; }
        Vbo handle;


        readonly short[] CubeElements = new short[]
        {
            0, 1, 2, 2, 3, 0, // front face
            3, 2, 6, 6, 7, 3, // top face
            7, 6, 5, 5, 4, 7, // back face
            4, 0, 3, 3, 7, 4, // left face
            0, 1, 5, 5, 4, 0, // bottom face
            1, 5, 6, 6, 2, 1, // right face
        };

        VertexPosition[] CubeVertices = new VertexPosition[]{
                new VertexPosition(-1.0f, -1.0f,  1.0f),
                new VertexPosition( 1.0f, -1.0f,  1.0f),
                new VertexPosition( 1.0f,  1.0f,  1.0f),
                new VertexPosition(-1.0f,  1.0f,  1.0f),
                new VertexPosition(-1.0f, -1.0f, -1.0f),
                new VertexPosition( 1.0f, -1.0f, -1.0f), 
                new VertexPosition( 1.0f,  1.0f, -1.0f),
                new VertexPosition(-1.0f,  1.0f, -1.0f) 
            };




        struct VertexPosition
        {
            public Vector3 Position;
            public VertexPosition(float x, float y, float z)
            {
                Position = new Vector3(x, y, z);

            }
        }
        */
        #endregion

        public TestObjectView()
        {
            this.Loaded = false;
        }

        public void Load()
        {
            #region vbo test
            /*
            GL.GenBuffers(1, out handle.VboID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, handle.VboID);

            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(CubeVertices.Length * BlittableValueType.StrideOf(CubeVertices)), CubeVertices, BufferUsageHint.StaticDraw);
            int size;
            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);
            if (CubeVertices.Length * BlittableValueType.StrideOf(CubeVertices) != size)
                throw new ApplicationException("Vertex data not uploaded correctly");

            GL.GenBuffers(1, out handle.EboID);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, handle.EboID);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(CubeElements.Length * sizeof(short)), CubeElements,
                          BufferUsageHint.StaticDraw);
            GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out size);
            if (CubeElements.Length * sizeof(short) != size)
                throw new ApplicationException("Element data not uploaded correctly");

            handle.NumElements = CubeElements.Length;
            */
            #endregion
            
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

            this.Loaded = true;
        }

        public void Render(IHasGeometry GameObject)
        {
            TestObject ob = GameObject as TestObject;

            GL.Color3(ob.Color);
            GL.Translate(GameObject.Position.ToVector3());
            GL.CallLists(1, ListNameType.Int, ref List);

            #region vbo test
            /*
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, handle.VboID);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, handle.EboID);
            GL.VertexPointer(3, VertexPointerType.Float, BlittableValueType.StrideOf(CubeVertices), new IntPtr(0));
            GL.DrawElements(BeginMode.Triangles, handle.NumElements, DrawElementsType.UnsignedShort, IntPtr.Zero);
            */
            //
            #endregion
            /*
             * intermediate mode
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
            */
        }


        public void Unload()
        {
            GL.DeleteLists(List, 1);
        }
    }
}
