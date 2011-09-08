using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ORTS.Core.Messaging;

namespace ORTS.Core.Graphics
{
    public interface IWidget : IHasMessageBus
    {
        bool Active { get; }
        void Load(Rectangle screen);
        bool Loaded { get; }
        void Unload();
        void Update(double delta);
        void Render();
    }
}
