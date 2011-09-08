using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORTS.Core.GameObject;
using ORTS.Core.Timing;

namespace ORTS.Core.Messaging.Messages
{
    public class ObjectDestructionRequest : BaseMessage
    {
        public IGameObject GameObject { get; private set; }

        public ObjectDestructionRequest(IGameTime timeSent, IGameObject gameObject)
            : base(timeSent)
        {
            GameObject = gameObject;
        }
    }
}
