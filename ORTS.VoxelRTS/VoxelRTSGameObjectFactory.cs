using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORTS.Core.GameObject;
using ORTS.Core.Messaging;
using ORTS.VoxelRTS.GameObjects;

namespace ORTS.VoxelRTS
{
    public class VoxelRTSGameObjectFactory: GameObjectFactory
    {
        public VoxelRTSGameObjectFactory(MessageBus bus)
            : base(bus)
        {
        }

        public override void CreateGameObject(ObjectCreationRequest request)
        {
            if (request.ObjectType == typeof(VoxelGreen))
            {
                var item = new VoxelGreen(this.Bus);
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
