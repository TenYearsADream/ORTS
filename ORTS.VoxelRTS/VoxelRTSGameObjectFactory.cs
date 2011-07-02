using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORTS.Core.GameObject;
using ORTS.Core.Messaging;
using ORTS.VoxelRTS.GameObjects;
using ORTS.Core.Maths;

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
            if (request.ObjectType == typeof(Planet))
            {
                /*
                var item = new Planet(this.Bus);
                lock (this.GameObjectsLock)
                {
                    this.GameObjects.Add(item);
                    VoxelGreen child;
                     Random rnd = new Random();
                    for (int i = 1; i <= 20; i++)
                    {

                        child = new VoxelGreen(this.Bus)
                        {
                            Position = new Vect3(rnd.Next(-10, 10), rnd.Next(-10, 10), rnd.Next(-10, 10))
                        };
                        this.GameObjects.Add(child);
                        item.AddChild(child);
                    }
                
                }*/
                //Bus.Add(new ObjectCreated(request.TimeSent, item));
            }
            base.CreateGameObject(request);
        }

    }
}
