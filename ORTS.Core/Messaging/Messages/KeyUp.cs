using ORTS.Core.Timing;
using System.Windows.Forms;
namespace ORTS.Core.Messaging
{
    public class KeyUp : BaseMessage
    {
        public Keys Key { get; private set; }
        public KeyUp(IGameTime timeSent, Keys key)
            :base(timeSent)
        {
            Key = key;
        }
    }
}
