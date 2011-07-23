using ORTS.Core.Reactive;

namespace ORTS.Core.Messaging
{
    public class MessageBus : Observable<IMessage>
    {

        public IBuffer<IMessage> PendingMessages { get; private set; }

        public MessageBus()
        {
            PendingMessages = new Buffer<IMessage>();

        }

        public void Add(IMessage message)
        {
            PendingMessages.Add(message);
        }

        public void SendAll()
        {
            foreach (var message in PendingMessages)
            {
                OnNext(message);
            }
        }

        public override void Dispose()
        {
            PendingMessages = null;
            base.Dispose();
        }
    }
}
