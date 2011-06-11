using ORTS.Core.GameObject;
using ORTS.Core.Timing;

namespace ORTS.Core.Messaging
{
    public class ObjectDestroyed : BaseMessage
    {
        public IGameObject GameObject { get; private set; }

        public ObjectDestroyed(IGameTime timeSent, IGameObject gameObject)
            : base(timeSent)
        {
            GameObject = gameObject;
        }
    }
}
