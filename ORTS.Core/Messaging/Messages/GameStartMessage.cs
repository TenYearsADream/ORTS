using ORTS.Core.Timing;

namespace ORTS.Core.Messaging.Messages
{
    public class GameStartMessage: BaseMessage
    {
        public GameStartMessage(IGameTime timeSent)
            : base(timeSent)
        {

        }
        public override string ToString()
        {
            return "{0} GAME STARTING".fmt(TimeSent);
        }
    }
}
