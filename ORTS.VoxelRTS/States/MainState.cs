using System.Threading.Tasks;
using System.Windows.Forms;
using ORTS.Core;
using ORTS.Core.Messaging.Messages;
using ORTS.Core.States;
using ORTS.Core.Timing;
using ORTS.VoxelRTS.GameObjects;
using ORTS.VoxelRTS.Messaging;

namespace ORTS.VoxelRTS.States
{
    public class MainState : IState
    {
        private readonly GameEngine _engine;
        public MainState(GameEngine engine)
        {
            _engine = engine;
        }

        public void Load()
        {

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

        public void KeyUp(KeyUpMessage m)
        {
            if (m.Key == Keys.F)
            {
                _engine.Bus.Add(new PlanetCreationRequest(_engine.Timer.LastTickTime, PlanetType.Ice, 9));
            }

        }

        public void KeyPress(KeyPressMessage m)
        {
            throw new System.NotImplementedException();
        }

        public void MouseMove(MouseMoveMessage m)
        {
            throw new System.NotImplementedException();
        }

        public void MouseButtonDown(MouseButtonDownMessage m)
        {
            throw new System.NotImplementedException();
        }

        public void MouseButtonUp(MouseButtonUpMessage m)
        {
            throw new System.NotImplementedException();
        }

        public void MouseWheelChanged(MouseWheelChangedMessage m)
        {
            throw new System.NotImplementedException();
        }

        public void KeyDown(KeyDownMessage m)
        {

        }
    }
}
