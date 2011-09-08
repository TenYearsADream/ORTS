using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using ORTS.Core.Attributes;
using ORTS.Core.GameObject;
using ORTS.Core.Graphics;
using ORTS.Core.Graphics.ModelLoaders;
using ORTS.Space.GameObjects;
using ORTS.Core.OpenTKHelper;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ORTS.Space.Views
{
    [BindView(typeof(Planet))]
    public class PlanetView : IGameObjectView
    {
        private readonly ICamera _camera;
        public bool Loaded { get; private set; }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]

        struct VertexP3N3T2
        {
            public Vector3 Position, Normal;
            public Vector2 TexCoord;
        }

        public PlanetView(ICamera camera)
        {
            _camera = camera;
            Loaded = false;
        }

        private int list = -1;
        public void Load()
        {
            //GL.PolygonMode(MaterialFace.FrontAndBack,PolygonMode.Line);
            var model = new STL("Models/test-bin.stl", STLFormat.Binary);
            GL.NewList(list, ListMode.Compile);
            GL.Begin(BeginMode.Triangles);

            foreach (var element in model.Elements)
            {
                GL.Normal3(element.Normal.ToVector3());
                GL.Vertex3(element.P1.ToVector3());
                GL.Vertex3(element.P2.ToVector3());
                GL.Vertex3(element.P3.ToVector3());
            }

            GL.End();
            GL.EndList();
           Loaded = true;
        }

        public void Unload()
        {

        }

        public void Update(IEnumerable<IGameObject> gameObjects, double delta)
        {

        }

        VertexP3N3T2[] CalculateVertices(float radius, float height, byte segments, byte rings)
        {
            var data = new VertexP3N3T2[segments * rings];

            int i = 0;

            for (double y = 0; y < rings; y++)
            {
                double phi = (y / (rings - 1)) * Math.PI;
                for (double x = 0; x < segments; x++)
                {
                    double theta = (x / (segments - 1)) * 2 * Math.PI;

                    Vector3 v = new Vector3()
                    {
                        X = (float)(radius * Math.Sin(phi) * Math.Cos(theta)),
                        Y = (float)(height * Math.Cos(phi)),
                        Z = (float)(radius * Math.Sin(phi) * Math.Sin(theta)),
                    };
                    Vector3 n = Vector3.Normalize(v);
                    Vector2 uv = new Vector2()
                    {
                        X = (float)(x / (segments - 1)),
                        Y = (float)(y / (rings - 1))
                    };
                    // Using data[i++] causes i to be incremented multiple times in Mono 2.2 (bug #479506).
                    data[i] = new VertexP3N3T2() { Position = v, Normal = n, TexCoord = uv };
                    i++;

                    // Top - down sphere projection.
                    //Vector2 uv = new Vector2()
                    //{
                    //    X = (float)(Math.Atan2(n.X, n.Z) / Math.PI / 2 + 0.5),
                    //    Y = (float)(Math.Asin(n.Y) / Math.PI / 2 + 0.5)
                    //};
                }

            }

            return data;
        }

        ushort[] CalculateElements(float radius, float height, byte segments, byte rings)
        {
            var num_vertices = segments * rings;
            var data = new ushort[num_vertices * 6];

            ushort i = 0;

            for (byte y = 0; y < rings - 1; y++)
            {
                for (byte x = 0; x < segments - 1; x++)
                {
                    data[i++] = (ushort)((y + 0) * segments + x);
                    data[i++] = (ushort)((y + 1) * segments + x);
                    data[i++] = (ushort)((y + 1) * segments + x + 1);

                    data[i++] = (ushort)((y + 1) * segments + x + 1);
                    data[i++] = (ushort)((y + 0) * segments + x + 1);
                    data[i++] = (ushort)((y + 0) * segments + x);
                }
            }

            // Verify that we don't access any vertices out of bounds:
            foreach (int index in data)
                if (index >= segments * rings)
                    throw new IndexOutOfRangeException();
            
            return data;
        }
        public void Render()
        {
            GL.Color4(Color.FromArgb(94, 169, 198));
            GL.Disable(EnableCap.Texture2D);
            GL.CallList(list);
            GL.Enable(EnableCap.Texture2D);
            /*
            
            float radius = 5;
            float height = 5;
            byte segments = 20;
            byte rings = 20;
            var vertices = CalculateVertices(radius, height, segments, rings);
            var elements = CalculateElements(radius, height, segments, rings);

            GL.Begin(BeginMode.Triangles);
            foreach (var element in elements)
            {
                var vertex = vertices[element];
                GL.TexCoord2(vertex.TexCoord);
                GL.Normal3(vertex.Normal);
                GL.Vertex3(vertex.Position);
            }
            GL.End();

            GL.End();
             */ 
        }
    }
}
