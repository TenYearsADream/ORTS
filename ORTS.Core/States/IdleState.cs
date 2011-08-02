using System.Windows.Forms;
using ORTS.Core;
using ORTS.Core.Messaging.Messages;
using ORTS.Core.Timing;

namespace ORTS.Core.States
{
    public class IdleState : IState
    {
        private GameEngine _engine;
        public IdleState(GameEngine engine)
        {
            _engine = engine;
        }

        public void Update(TickTime tickTime)
        {

        }

        public void Show()
        {

        }

        public void Hide()
        {

        }

        public void Destroy()
        {

        }

        public void KeyUp(KeyUp m)
        {

        }

        public void KeyDown(KeyDown m)
        {

        }
    }
}
