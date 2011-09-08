using ORTS.Core.Timing;
using System.Windows.Forms;
namespace ORTS.Core.Messaging.Messages
{
    public class KeyUpMessage : BaseMessage
    {
        public Keys Key { get; private set; }
        public KeyUpMessage(IGameTime timeSent, Keys key)
            :base(timeSent)
        {
            Key = key;
        }
        public override string ToString()
        {
            return "{0} KeyUp - {1}".fmt(TimeSent, Key);
        }
    }
}
