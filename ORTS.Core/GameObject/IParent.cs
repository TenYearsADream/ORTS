using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORTS.Core.GameObject
{
    public interface IParent : IGameObject, IHasPosition
    {
        List<IGameObject> Children { get; }
        void AddChild(IHasParent Child);
    }
}
