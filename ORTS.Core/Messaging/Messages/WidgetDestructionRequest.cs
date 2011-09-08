using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORTS.Core.GameObject;
using ORTS.Core.Graphics;
using ORTS.Core.Timing;

namespace ORTS.Core.Messaging.Messages
{
    public class WidgetDestructionRequest : BaseMessage
    {
        public IWidget Widget { get; private set; }

        public WidgetDestructionRequest(IGameTime timeSent, IWidget widget)
            : base(timeSent)
        {
            Widget = widget;
        }
    }
}
