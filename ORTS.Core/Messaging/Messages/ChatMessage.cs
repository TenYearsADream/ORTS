using ORTS.Core.Timing;

namespace ORTS.Core.Messaging.Messages
{
    public class ChatMessage : BaseMessage
    {
        public string Message { get; private set; }

        public ChatMessage(IGameTime timeSent, string message)
            : base(timeSent)
        {
            Message = message;
        }
        public override string ToString()
        {
            return "{0} Chat - {1}".fmt(TimeSent, Message);
        }
    }
}
