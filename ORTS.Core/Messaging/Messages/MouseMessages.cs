using System.Drawing;
using ORTS.Core.Timing;
using System.Windows.Forms;
namespace ORTS.Core.Messaging.Messages
{
    public enum Button
    {
        Left,
        Middle,
        Right,
    }

    public class MouseMoveMessage : BaseMessage
    {
        public Point Position { get; private set; }
        public MouseMoveMessage(IGameTime timeSent, Point position)
            :base(timeSent)
        {
            Position = position;
        }
        public override string ToString()
        {
            return "{0} MouseMove - {1}".fmt(TimeSent,Position);
        }
    }

    public class MouseButtonDownMessage : BaseMessage
    {
        public Point Position { get; private set; }
        public Button Button { get; private set; }
        public MouseButtonDownMessage(IGameTime timeSent, Point position, Button button)
            : base(timeSent)
        {
            Position = position;
            Button = button;
        }
        public override string ToString()
        {
            return "{0} MouseButtonDown - {1}".fmt(TimeSent, Position);
        }
    }

    public class MouseButtonUpMessage : BaseMessage
    {
        public Point Position { get; private set; }
        public Button Button { get; private set; }
        public MouseButtonUpMessage(IGameTime timeSent, Point position, Button button)
            : base(timeSent)
        {
            Position = position;
            Button = button;
        }
        public override string ToString()
        {
            return "{0} MouseButtonUp - {1}".fmt(TimeSent, Position);
        }
    }

    public class MouseWheelChangedMessage : BaseMessage
    {
        public Point Position { get; private set; }
        public float Delta { get; private set; }
        public MouseWheelChangedMessage(IGameTime timeSent, Point position, float delta)
            : base(timeSent)
        {
            Position = position;
            Delta = delta;
        }
        public override string ToString()
        {
            return "{0} MouseWheelChanged - {1}".fmt(TimeSent, Position);
        }
    }
}
