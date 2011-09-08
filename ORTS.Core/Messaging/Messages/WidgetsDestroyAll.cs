using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORTS.Core.GameObject;
using ORTS.Core.Timing;

namespace ORTS.Core.Messaging.Messages
{
    public class WidgetsDestroyAll : BaseMessage
    {
        public WidgetsDestroyAll(IGameTime timeSent)
            : base(timeSent)
        {

        }
    }
}
