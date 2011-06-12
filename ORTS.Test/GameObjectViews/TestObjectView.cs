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
        
        public TestObjectView()
        {
            this.Loaded = false;
        }

        public void Load()
        {
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
            GL.CallList(List);
        }
        public void Unload()
        {
            GL.DeleteLists(List, 1);
        }
    }
}
