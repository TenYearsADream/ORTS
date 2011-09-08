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
        public override string ToString()
        {
            return "{0} SYSTEM - {1}".fmt(TimeSent, Message);
        }
    }
}
