using System;
using System.Linq;
using ORTS.Core.GameObject;
using ORTS.Core.Messaging;
using ORTS.Core.Messaging.Messages;
using ORTS.VoxelRTS.GameObjects;
using ORTS.Core.Maths;
using ORTS.VoxelRTS.Messaging;

namespace ORTS.VoxelRTS
{
    public class VoxelRTSGameObjectFactory: GameObjectFactory
    {
        private int _idTally = 1;
        public VoxelRTSGameObjectFactory(MessageBus bus)
            : base(bus)
        {
            Bus.OfType<PlanetCreationRequest>().Subscribe(CreatePlanet);
        }

        private void CreatePlanet(PlanetCreationRequest request)
        {
            lock (GameObjectsLock)
            {
                var item = new Planet(Bus);
                GameObjects.Add(item);
                for (int x = 0; x < request.PlanetSize; ++x)
                {
                    for (int y = 0; y < request.PlanetSize; ++y)
                    {
                        for (int z = 0; z < request.PlanetSize; ++z)
                        {
                            if ((Math.Pow(x - (request.PlanetSize - 1) / 2.0, 2) + Math.Pow(y - (request.PlanetSize - 1) / 2.0, 2) + Math.Pow(z - (request.PlanetSize - 1) / 2.0, 2) - Math.Pow(request.PlanetSize / 2.0, 2)) <= 0)
                            {
                                var child = new VoxelGreen(_idTally++, Bus)
                                {
                                    Position = new Vect3(x,y,z)
                                };
                                GameObjects.Add(child);
                                item.AddChild(child);
                            }
                        }
                    }
                }
            }

        }

        public override void CreateGameObject(ObjectCreationRequest request)
        {
            
            if (request.ObjectType == typeof(VoxelGreen))
            {
                var item = new VoxelGreen(_idTally++, Bus);
                lock (GameObjectsLock)
                {
                    GameObjects.Add(item);
                }
                Bus.Add(new ObjectCreated(request.TimeSent, item));
            }
            Console.WriteLine(request.ObjectType);
            if (request.ObjectType == typeof(TopMenu))
            {

                var item = new TopMenu(_idTally++, Bus);
                lock (GameObjectsLock)
                {
                    GameObjects.Add(item);
                    Bus.Add(new ObjectCreated(request.TimeSent, item));
                }
            }

            base.CreateGameObject(request);
        }

    }
}
