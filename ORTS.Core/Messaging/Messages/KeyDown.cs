using ORTS.Core.Timing;
using System.Windows.Forms;
namespace ORTS.Core.Messaging
{
    public class KeyDown : BaseMessage
    {
        public Keys Key { get; private set; }
        public KeyDown(IGameTime timeSent, Keys key)
            :base(timeSent)
        {
            Key = key;
        }
    }
}
