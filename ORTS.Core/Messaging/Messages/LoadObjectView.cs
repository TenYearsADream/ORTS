using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORTS.Core.Timing;
using ORTS.Core.Graphics;

namespace ORTS.Core.Messaging.Messages
{
    public class LoadObjectView : BaseMessage
    {
        public Type GameObjectType { get; private set; }
        public IGameObjectView View { get; private set; }
        public LoadObjectView(IGameTime timeSent, IGameObjectView view, Type gameobjecttype)
            : base(timeSent)
        {
            this.GameObjectType = gameobjecttype;
            this.View = view;
        }
    }
}
