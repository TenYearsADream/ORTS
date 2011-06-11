using ORTS.Core.GameObject;

namespace ORTS.Core.Graphics
{
    public interface IGameObjectView
    {
        void Load();
        bool Loaded { get; }
        void Render(IHasGeometry GameObject);
    }
}
