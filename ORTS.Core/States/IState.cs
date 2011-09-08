using System;
using System.Windows.Forms;
using ORTS.Core.Messaging.Messages;
using ORTS.Core.Timing;

namespace ORTS.Core.States
{
    public interface IState
    {
        
        void Update(TickTime tickTime);
        void Show();
        void Hide();
        void Destroy();

        void KeyDown(KeyDownMessage m);
        void KeyUp(KeyUpMessage m);
        void KeyPress(KeyPressMessage m);

        void MouseMove(MouseMoveMessage m);
        void MouseButtonDown(MouseButtonDownMessage m);
        void MouseButtonUp(MouseButtonUpMessage m);
        void MouseWheelChanged(MouseWheelChangedMessage m);

    }
}
