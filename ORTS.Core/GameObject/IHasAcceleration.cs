using ORTS.Core.Maths;

namespace ORTS.Core.GameObject
{
    public interface IHasAcceleration : IHasVelocity
    {
        Vect3 Acceleration { get; }
        Quat RotationalAcceleration { get; }
    }
}
