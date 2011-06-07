using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORTS.Core.Reactive;
namespace ORTS.Core.Messaging
{
    public class MessageBus : Observable<IMessage>
    {
        public override void Dispose()
        {
            //PendingMessages = null;
            base.Dispose();
        }
    }
}
