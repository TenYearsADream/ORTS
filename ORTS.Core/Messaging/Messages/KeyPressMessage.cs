using ORTS.Core.Timing;
using System.Windows.Forms;
namespace ORTS.Core.Messaging.Messages
{
    public class KeyPressMessage : BaseMessage
    {
        public char Key { get; private set; }
        public KeyPressMessage(IGameTime timeSent, char key)
            :base(timeSent)
        {
            Key = key;
        }
        public override string ToString()
        {
            return "{0} KeyPress - {1}".fmt(TimeSent, Key);
        }
    }
}
