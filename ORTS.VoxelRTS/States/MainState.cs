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

        public void KeyUp(KeyUp m)
        {
            if (m.Key == Keys.F)
            {
                _engine.Bus.Add(new PlanetCreationRequest(_engine.Timer.LastTickTime, PlanetType.Ice, 9));
            }

        }

        public void KeyDown(KeyDown m)
        {

        }
    }
}
