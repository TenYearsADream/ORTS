using System;
using System.Windows.Forms;
using ORTS.Core;
using ORTS.Core.Messaging.Messages;
using ORTS.Core.States;
using ORTS.Core.Timing;
using ORTS.Space.Widgets;
namespace ORTS.Space.States
{
    public class MainMenuState : IState
    {
        private GameEngine _engine;

        public MainMenuState(GameEngine engine)
        {
            _engine = engine;
        }

        public void Update(TickTime tickTime)
        {

        }

        public void Show()
        {
           // _engine.Bus.Add(new WidgetCreationRequest(_engine.Timer.LastTickTime, typeof(MainMenuWidget)));
            _engine.Bus.Add(new WidgetCreationRequest(_engine.Timer.LastTickTime, typeof(FPSWidget)));
        }

        public void Hide()
        {

        }

        public void Destroy()
        {

        }

        public void KeyDown(KeyDownMessage m)
        {
           if(m.Key == Keys.Enter)
           {
               _engine.Bus.Add(new WidgetsDestroyAll(_engine.Timer.LastTickTime));
               _engine.Bus.Add(new ObjectsDestroyAll(_engine.Timer.LastTickTime));
               _engine.CurrentState = new TestGameState(_engine);
           }
        }

        public void KeyUp(KeyUpMessage m)
        {
           
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
