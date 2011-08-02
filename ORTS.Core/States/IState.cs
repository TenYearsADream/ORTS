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
        void KeyUp(KeyUp m);
        void KeyDown(KeyDown m);
    }
}
