using System.Drawing;
using ORTS.Core.Timing;
using System.Windows.Forms;
namespace ORTS.Core.Messaging.Messages
{
    public class ScreenResizeMessage : BaseMessage
    {
        public Rectangle Screen { get; private set; }
        public ScreenResizeMessage(IGameTime timeSent, Rectangle screen)
            :base(timeSent)
        {
            Screen = screen;
        }
        public override string ToString()
        {
            return "{0} Resize - {1}".fmt(TimeSent,Screen);
        }
    }
}
