using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using ORTS.Core.Messaging;
using ORTS.Core.GameObject;
using ORTS.Core.Timing;
using ORTS.Core.Maths;

namespace ORTS.Test.GameObjects
{
    public class TestObject : IGameObject, IHasVelocity, IHasAcceleration, IHasGeometry
    {
        public MessageBus Bus { get; private set; }
        public Vect3 Position { get; private set; }
        public Vect3 Velocity { get; private set; }
        public Vect3 Acceleration { get; private set; }

        public Color Color { get; private set; }


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

        public TestObject(MessageBus bus)
        {
            this.Bus = bus;
            Random rnd = new Random();
            this.Position = new Vect3(rnd.Next(0, 10), rnd.Next(0, 10), 0);
            this.Velocity = new Vect3(rnd.Next(-5, 5), rnd.Next(-5, 5), 0);
            this.Acceleration = new Vect3(0, 0, 0);
            this.Color = Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256));
        }

        public void Update(TickTime tickTime)
        {
            Velocity = Velocity + (Acceleration * tickTime.GameTimeDelta.TotalSeconds); 
            Position = Position + (Velocity * tickTime.GameTimeDelta.TotalSeconds);
        }
    }
}
