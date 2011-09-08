using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORTS.Core.Timing;

namespace ORTS.Core.Messaging.Messages
{
    public class WidgetCreationRequest : BaseMessage
    {
        public Type WidgetType { get; private set; }
        public WidgetCreationRequest(IGameTime timeSent, Type type)
            : base(timeSent)
        {
            WidgetType = type;
        }
    }
}
