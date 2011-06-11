using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORTS.Core.GameObject;
using ORTS.Core.Messaging;
using ORTS.Test.GameObjects;

namespace ORTS.Test
{
    public class TestGameFactory : GameObjectFactory
    {
        public TestGameFactory(MessageBus bus)
            : base(bus)
        {
        }

        public override void CreateGameObject(ObjectCreationRequest request)
        {
            if (request.ObjectType == typeof(TestObject))
            {
                var item = new TestObject(this.Bus);
                lock (this.GameObjectsLock)
                {
                    this.GameObjects.Add(item);
                }
                Bus.Add(new ObjectCreated(request.TimeSent, item));
            }
            base.CreateGameObject(request);
        }

    }
}
