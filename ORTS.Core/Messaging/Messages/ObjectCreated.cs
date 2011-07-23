using ORTS.Core.GameObject;
using ORTS.Core.Timing;

namespace ORTS.Core.Messaging.Messages
{
    public class ObjectCreated : BaseMessage
    {
        public IGameObject GameObject { get; private set; }

        public ObjectCreated(IGameTime timeSent, IGameObject gameObject)
            : base(timeSent)
        {
            GameObject = gameObject;
        }
    }
}
