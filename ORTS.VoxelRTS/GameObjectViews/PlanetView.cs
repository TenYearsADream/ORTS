using System.Collections.Generic;
using ORTS.Core.Graphics;
using ORTS.Core.GameObject;

namespace ORTS.VoxelRTS.GameObjectViews
{

    class PlanetView : IGameObjectView
    {
        public bool Loaded { get; private set; }

        public PlanetView()
        {
            Loaded = false;
        }

        public void Load()
        {
            Loaded = true;
        }
        /*
        public void Render(IHasGeometry GameObject)
        {
            GL.Translate(GameObject.Position.ToVector3());
            GL.Begin(BeginMode.Lines);
            GL.Color4(Color4.Red);
            GL.Vertex3(0f, 0f, 0f);
            GL.Vertex3(1f, 0f, 0f);
            GL.Color4(Color4.Green);
            GL.Vertex3(0f, 0f, 0f);
            GL.Vertex3(0f, 1f, 0f);
            GL.Color4(Color4.Blue);
            GL.Vertex3(0f, 0f, 0f);
            GL.Vertex3(0f, 0f, 1f);
            GL.End();
        }
         */ 
        public void Unload()
        {
            
        }

        public void Update(IEnumerable<IGameObject> gameObjects, double delta)
        {
            throw new System.NotImplementedException();
        }


        public void Add(IGameObject gameObject)
        {
           // throw new NotImplementedException();
        }

        public void Update()
        {
           // throw new NotImplementedException();
        }

        public void Render()
        {
            //throw new NotImplementedException();
        }
    }
}
