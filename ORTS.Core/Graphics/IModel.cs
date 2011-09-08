using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORTS.Core.Graphics.Primatives;

namespace ORTS.Core.Graphics
{
    public interface IModel<T>
    {
        List<T> Elements { get; }
    }
}
