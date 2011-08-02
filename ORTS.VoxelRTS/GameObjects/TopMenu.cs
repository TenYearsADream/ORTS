using System;
using ORTS.Core.Messaging;
using ORTS.Core.GameObject;
using ORTS.Core.Timing;
using ORTS.Core.Maths;

namespace ORTS.VoxelRTS.GameObjects
{
    public class TopMenu : IGameObject
    {
        public MessageBus Bus { get; private set; }
        public int Id { get; private set; }
        public Vect3 Position { get; private set; }
        public Quat Rotation { get; private set; }
        public TopMenu(int id, MessageBus bus)
        {
            Id = id;
            Bus = bus;
            Position = Vect3.Zero;
            Rotation = Quat.Identity;
        }

        public void Update(TickTime tickTime)
        {

        }
    }
}
