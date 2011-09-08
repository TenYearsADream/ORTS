using System.Collections.Generic;
using ORTS.Core.GameObject;

namespace ORTS.Core.Graphics
{
    public interface IGameObjectView
    {
        void Load();
        bool Loaded { get; }
        void Unload();
        void Update(IEnumerable<IGameObject> gameObjects, double delta);
        void Render();
    }
}

