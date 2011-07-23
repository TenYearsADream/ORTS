using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORTS.Core.Timing;

namespace ORTS.Core.Messaging.Messages
{
    public class GraphicsDirtyMessage : BaseMessage
    {
        public GraphicsDirtyMessage(IGameTime timeSent)
            : base(timeSent)
        {

        }
    }
    public class GraphicsLoadedMessage : BaseMessage
    {
        public GraphicsLoadedMessage(IGameTime timeSent)
            : base(timeSent)
        {

        }
    }
    public class GraphicsReady : BaseMessage
    {
        public GraphicsReady(IGameTime timeSent)
            : base(timeSent)
        {

        }
    }
}
