using ORTS.Core.Messaging.Messages;

namespace ORTS.Core.Messaging
{
    public interface IHasMessageBus
    {
        MessageBus Bus { get; }
    }
}
