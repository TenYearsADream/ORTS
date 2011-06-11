using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORTS.Core.Timing;
using ORTS.Core.Messaging;

namespace ORTS.Core.Messaging
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
