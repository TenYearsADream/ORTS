using ORTS.Core.Messaging;
using ORTS.Core.Messaging.Messages;
using ORTS.Core.Timing;

namespace ORTS.Core.GameObject
{
    public interface IGameObject : IHasMessageBus, IHasPosition
    {
        int Id { get; }
        void Update(TickTime tickTime);
    }
}
