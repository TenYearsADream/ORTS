using ORTS.Core.Maths;

namespace ORTS.Core.GameObject
{
    public interface IHasPosition
    {
        Vect3 Position { get; }
        Quat Rotation { get; }
    }
}
