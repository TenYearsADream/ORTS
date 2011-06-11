using ORTS.Core.Maths;

namespace ORTS.Core.GameObject
{
    public interface IHasPosition : IGameObject
    {
        Vect3 Position { get; }
        Quat Rotation { get; }
    }
}
