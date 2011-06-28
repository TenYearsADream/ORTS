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
    public class VoxelGreen : IGameObject, IBody, IHasGeometry, IHasParent
    {
        public MessageBus Bus { get; private set; }
        public Vect3 Position { get; private set; }
        public Vect3 Velocity { get; private set; }
        public Vect3 Acceleration { get; private set; }

        public Quat Rotation { get; private set; }
        public Quat RotationalVelocity{ get; private set; }
        /*
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
        */


        public Quat RotationalAcceleration
        {
            get { throw new NotImplementedException(); }
        }

        public IParent Parent { get; private set; }

        public VoxelGreen(MessageBus bus)
        {
            this.Bus = bus;
            Random rnd = new Random();
            this.Position = new Vect3(0, 0, 0);
            this.Velocity = new Vect3(rnd.Next(-5, 5), rnd.Next(-5, 5), 0);
            this.Acceleration = new Vect3(0, 0, 0);
            this.Rotation = new Quat(Math.Sqrt(0.5), 0, 0, Math.Sqrt(0.5));
            this.RotationalVelocity = new Euler(Angle.FromDegrees(rnd.Next(-5, 5)), Angle.FromDegrees(rnd.Next(-5, 5)), Angle.FromDegrees(rnd.Next(-5, 5))).toQuat();
        }


        public void Update(TickTime tickTime)
        {

            this.Rotation = this.Rotation * (this.RotationalVelocity * tickTime.GameTimeDelta.TotalSeconds);
            //Velocity = Velocity + (Acceleration * tickTime.GameTimeDelta.TotalSeconds);
            this.Position = this.Position + (this.Velocity * tickTime.GameTimeDelta.TotalSeconds);

        }
    }
}
