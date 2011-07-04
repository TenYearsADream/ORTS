using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORTS.Core.GameObject;
using ORTS.Core.Messaging;
using ORTS.VoxelRTS.GameObjects;
using ORTS.Core.Maths;
using ORTS.VoxelRTS.Messaging;

namespace ORTS.VoxelRTS
{
    public class VoxelRTSGameObjectFactory: GameObjectFactory
    {
        public VoxelRTSGameObjectFactory(MessageBus bus)
            : base(bus)
        {
            Bus.OfType<PlanetCreationRequest>().Subscribe(m => CreatePlanet(m));
        }

        private double Fvalue(double x, double y, double z, double R)
        {
            return x*x + y*y + z*z - R*R;
        }

        private void CreatePlanet(PlanetCreationRequest request)
        {
            lock (this.GameObjectsLock)
            {
                Planet item = new Planet(this.Bus);
                this.GameObjects.Add(item);
                for (int x = 0; x < request.PlanetSize; ++x)
                {
                    for (int y = 0; y < request.PlanetSize; ++y)
                    {
                        for (int z = 0; z < request.PlanetSize; ++z)
                        {
                            if ((Math.Pow(x - (request.PlanetSize - 1) / 2.0, 2) + Math.Pow(y - (request.PlanetSize - 1) / 2.0, 2) + Math.Pow(z - (request.PlanetSize - 1) / 2.0, 2) - Math.Pow(request.PlanetSize / 2.0, 2)) <= 0)
                            {
                                VoxelGreen child = new VoxelGreen(this.Bus)
                                {
                                    Position = new Vect3(x,y,z)
                                };
                                this.GameObjects.Add(child);
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
                        };                        this.GameObjects.Add(child);
                        item.AddChild(child);
                    }
                
                }*/
                //Bus.Add(new ObjectCreated(request.TimeSent, item));
            }
            base.CreateGameObject(request);
        }

    }
}
