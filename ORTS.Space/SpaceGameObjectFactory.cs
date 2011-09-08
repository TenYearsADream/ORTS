using System;
using ORTS.Core.GameObject;
using ORTS.Core.Messaging;
using ORTS.Core.Messaging.Messages;
using ORTS.Space.GameObjects;

namespace ORTS.Space
{
    public class SpaceGameObjectFactory: GameObjectFactory
    {
        private int _idTally = 1;
        public SpaceGameObjectFactory(MessageBus bus)
            : base(bus)
        {

        }

        public override void CreateGameObject(ObjectCreationRequest request)
        {

            if (request.ObjectType == typeof(Planet))
            {
                var item = new Planet(_idTally++, Bus);
                lock (GameObjectsLock)
                {
                    GameObjects.Add(item);
                }
                Bus.Add(new ObjectCreated(request.TimeSent, item));
            }
            else if (request.ObjectType == typeof(SkyBox))
            {
                var item = new SkyBox(_idTally++, Bus);
                lock (GameObjectsLock)
                {
                    GameObjects.Add(item);
                }
                Bus.Add(new ObjectCreated(request.TimeSent, item));

            }
            base.CreateGameObject(request);
        }
    }
}
