using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORTS.Core.Timing;
using ORTS.Core.Messaging.Messages;

namespace ORTS.Core.Messaging.Messages
{
    public abstract class BaseMessage : IMessage
    {
        public IGameTime TimeSent { get; private set; }

        public BaseMessage(IGameTime timeSent)
        {
            TimeSent = timeSent;
        }
    }
}
