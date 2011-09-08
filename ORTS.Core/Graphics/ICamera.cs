using ORTS.Core.Maths;

namespace ORTS.Core.Graphics
{
    public interface ICamera
    {
        Quat Rotation { get; }
        Vect3 Postion { get; }
        void Rotate(Quat q);
        void Translate(Vect3 v);
    }
}
