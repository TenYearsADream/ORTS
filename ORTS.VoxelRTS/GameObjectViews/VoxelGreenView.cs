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

namespace ORTS.VoxelRTS.GameObjectViews
{

    class VoxelGreenView : IGameObjectView
    {
        public bool Loaded { get; private set; }
        private int List;

        public VoxelGreenView()
        {
            this.Loaded = false;
        }

        public void Load()
        {
            List = GL.GenLists(1);
            GL.NewList(List, ListMode.Compile);

            GL.Color3(Color.Green);

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
            GL.Translate(GameObject.Position.ToVector3());
            GL.CallList(List);
        }
        public void Unload()
        {
            GL.DeleteLists(List, 1);
        }
    }
}
