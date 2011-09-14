using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORTS.Core.Graphics;
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
        public IGraphics Graphics { get; private set; }
        public GraphicsLoadedMessage(IGameTime timeSent, IGraphics graphics)
            : base(timeSent)
        {
            Graphics = graphics;
        }
        public override string ToString()
        {
            return "{0} GRAPHICS LOADED".fmt(TimeSent);
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
