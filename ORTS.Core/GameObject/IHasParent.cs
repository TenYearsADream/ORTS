using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORTS.Core.GameObject
{
    public interface IHasParent : IGameObject
    {
        IParent Parent { get; set; }
    }
}
