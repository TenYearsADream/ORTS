using ORTS.Core.Timing;

namespace ORTS.Core.Messaging
{
    public interface IMessage
    {
        IGameTime TimeSent { get; }
    }
}
