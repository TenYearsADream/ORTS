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

    class PlanetView : IGameObjectView
    {
        public bool Loaded { get; private set; }

        public PlanetView()
        {
            this.Loaded = false;
        }

        public void Load()
        {
           
            this.Loaded = true;
        }

        public void Render(IHasGeometry GameObject)
        {
            GL.Translate(GameObject.Position.ToVector3());
            
        }
        public void Unload()
        {
            
        }
    }
}
