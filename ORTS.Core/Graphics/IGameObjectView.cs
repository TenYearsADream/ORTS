using ORTS.Core.GameObject;

namespace ORTS.Core.Graphics
{
    public interface IGameObjectView
    {
        void Load();
        bool Loaded { get; }
        void Unload();
        void Add(IGameObject gameObject);
        void Update();
        void Render();
    }
}
