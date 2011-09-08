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
        public override string ToString()
        {
            return "{0} ObjectCreated - {1}:{2}".fmt(TimeSent, GameObject.Id, GameObject.GetType().Name);
        }
    }
}
