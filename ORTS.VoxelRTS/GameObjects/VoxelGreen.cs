using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using ORTS.Core.Messaging;
using ORTS.Core.GameObject;
using ORTS.Core.Timing;
using ORTS.Core.Maths;

namespace ORTS.VoxelRTS.GameObjects
{
    public class VoxelGreen : IGameObject, IHasVelocity, IHasAcceleration, IHasGeometry, IHasParent
    {
        public MessageBus Bus { get; private set; }
        public Vect3 Position { 
            get 
            {
                return Parent != null ? _position + Parent.Position : _position;
            }
            set
            {
                _position = value;
            }
        }

        private Vect3 _position = new Vect3(0, 0, 0);

        public Vect3 Velocity { get; private set; }
        public Vect3 Acceleration { get; private set; }

        public Quat RotationalAcceleration
        {
            get { throw new NotImplementedException(); }
        }

        public Quat Rotation
        {
            get { throw new NotImplementedException(); }
        }

        public Quat RotationalVelocity
        {
            get { throw new NotImplementedException(); }
        }


        public IParent Parent { get; private set; }

        public VoxelGreen(MessageBus bus)
        {
            this.Bus = bus;
            Random rnd = new Random();
            this.Velocity = new Vect3(rnd.Next(-5, 5), rnd.Next(-5, 5), 0);
            this.Acceleration = new Vect3(0, 0, 0);

        }

        public void Update(TickTime tickTime)
        {
    
            Velocity = Velocity + (Acceleration * tickTime.GameTimeDelta.TotalSeconds);
            Position = Position + (Velocity * tickTime.GameTimeDelta.TotalSeconds);
        }
    }
}
