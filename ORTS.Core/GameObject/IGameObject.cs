using ORTS.Core.Messaging;
using ORTS.Core.Timing;

namespace ORTS.Core.GameObject
{
    public interface IGameObject : IHasMessageBus, IHasPosition
    {
        int ID { get; }
        void Update(TickTime tickTime);
    }
}
