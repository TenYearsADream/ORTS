using ORTS.Core.Messaging;
using System.Collections.Generic;

namespace ORTS.Core.GameObject
{
    public interface IGameObjectFactory : IHasMessageBus
    {
        List<IGameObject> GameObjects { get; }
    }
}
