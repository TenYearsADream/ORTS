using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORTS.Core.Timing;

namespace ORTS.Core.Messaging.Messages
{
    public class ObjectCreationRequest : BaseMessage
    {
        public Type ObjectType { get; private set; }
        public ObjectCreationRequest(IGameTime timeSent, Type objectType)
            : base(timeSent)
        {
            ObjectType = objectType;
        }
    }
}
