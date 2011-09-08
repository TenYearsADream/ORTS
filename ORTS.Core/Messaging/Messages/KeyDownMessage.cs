using ORTS.Core.Timing;
using System.Windows.Forms;
namespace ORTS.Core.Messaging.Messages
{
    public class KeyDownMessage : BaseMessage
    {
        public Keys Key { get; private set; }
        public bool Capital { get; private set; }
        public KeyDownMessage(IGameTime timeSent, Keys key)
            :base(timeSent)
        {
            Key = key;
        }
        public override string ToString()
        {
            return "{0} KeyDown - {1}".fmt(TimeSent,Key);
        }
    }
}
