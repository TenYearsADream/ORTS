using System;
using System.Windows.Forms;
using ORTS.Core;
using ORTS.Core.Messaging.Messages;
using ORTS.Core.States;
using ORTS.Core.Timing;
using ORTS.Space.GameObjects;
using ORTS.Space.Widgets;
namespace ORTS.Space.States
{
    public class TestGameState : IState
    {
        private GameEngine _engine;

        public TestGameState(GameEngine engine)
        {
            _engine = engine;
        }

        public void Update(TickTime tickTime)
        {

        }

        public void Show()
        {
            _engine.Bus.Add(new WidgetCreationRequest(_engine.Timer.LastTickTime, typeof(ChatWidget)));
        }

        public void Hide()
        {

        }

        public void Destroy()
        {

        }

        public void KeyDown(KeyDownMessage m)
        {
           
        }

        public void KeyUp(KeyUpMessage m)
        {
            if (m.Key == Keys.C)
            {
                _engine.Bus.Add(new ObjectCreationRequest(_engine.Timer.LastTickTime, typeof(Planet)));
            }
            if (m.Key == Keys.Escape)
            {
                _engine.Stop();
            }
        }

        public void MouseMove(MouseMoveMessage m)
        {
           
        }

        public void MouseButtonDown(MouseButtonDownMessage m)
        {
 
        }

        public void MouseButtonUp(MouseButtonUpMessage m)
        {

        }

        public void MouseWheelChanged(MouseWheelChangedMessage m)
        {
   
        }

        public void KeyPress(KeyPressMessage m)
        {

        }
    }
}
