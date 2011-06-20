using ORTS.Core.Messaging;
using ORTS.Core.Timing;

namespace ORTS.Core.GameObject
{
    public interface IGameObject : IHasMessageBus
    {

        void Update(TickTime tickTime);
    }
}
