﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORTS.Core.Timing;

namespace ORTS.Core.Messaging.Messages
{
    public class SystemMessage : BaseMessage
    {
        public string Message { get; private set; }

        public SystemMessage(IGameTime timeSent, string message)
            : base(timeSent)
        {
            Message = message;
        }
    }
}
