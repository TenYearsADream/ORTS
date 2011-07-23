using ORTS.Core.Messaging;
using ORTS.Core.Messaging.Messages;
using System.Collections.Generic;

namespace ORTS.Core.GameObject
{
    public interface IGameObjectFactory : IHasMessageBus
    {
        List<IGameObject> GameObjects { get; }
    }
}
