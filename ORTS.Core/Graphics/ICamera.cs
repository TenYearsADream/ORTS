using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORTS.Core.Maths;

namespace ORTS.Core.Graphics
{
    public interface ICamera
    {
        Quat rotation { get; }
        Vect3 postion { get; }
        void Rotate(Quat q);
        void Translate(Vect3 v);
    }
}
