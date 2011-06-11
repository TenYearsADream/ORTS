using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORTS.Core.Graphics
{
    public interface IGraphics
    {
        void Start(GameEngine engine);
        void Stop();
    }
}
