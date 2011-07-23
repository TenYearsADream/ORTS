using ORTS.Core.Maths;

namespace ORTS.Core.GameObject
{
    public interface IHasVelocity : IHasPosition
    {
        Vect3 Velocity { get; }
        Quat RotationalVelocity { get; }
    }
}
