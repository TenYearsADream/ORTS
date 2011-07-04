using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORTS.Core.GameObject;
using ORTS.Core.Timing;
using ORTS.Core.Messaging;
using ORTS.Core.Maths;

namespace ORTS.VoxelRTS.GameObjects
{
    public enum PlanetType { Ice };

    class Planet : IGameObject, IHasVelocity, IHasAcceleration, IHasGeometry, IParent
    {

        public MessageBus Bus { get; private set; }

        public Vect3 Position { get; private set; }
        public Vect3 Velocity { get; private set; }
        public Vect3 Acceleration { get; private set; }

        public Quat Rotation { get; private set; }
        public Quat RotationalVelocity { get; private set; }
        public Quat RotationalAcceleration { get; private set; }

        public List<IGameObject> Children { get; private set; }
        public Planet(MessageBus bus)
        {
            this.Bus = bus;
            this.Children = new List<IGameObject>();

            this.Position = new Vect3(0, 0, 0);
            this.Velocity = new Vect3(0, 0, 0);
            this.Acceleration = new Vect3(0, 0, 0);

            this.Rotation = new Quat(Math.Sqrt(0.5), 0, 0, Math.Sqrt(0.5));
            this.RotationalVelocity = new Euler(Angle.FromDegrees(1), Angle.FromDegrees(1), Angle.FromDegrees(1)).toQuat();
        }

        public void Update(TickTime tickTime)
        {
            this.Rotation = this.Rotation * (this.RotationalVelocity * tickTime.GameTimeDelta.TotalSeconds);
            this.Position = this.Position + (this.Velocity * tickTime.GameTimeDelta.TotalSeconds);
        }

        public void AddChild(IHasParent Child)
        {
            Children.Add(Child);
            Child.Parent = this;
            
        }
    }
}
