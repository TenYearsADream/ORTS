using System;
using ORTS.Core.GameObject;
using ORTS.Core.Maths;
using ORTS.Core.Messaging;
using ORTS.Core.Timing;

namespace ORTS.Space.GameObjects
{
    internal class Planet : IGameObject, IHasVelocity
    {
        public Planet(int id, MessageBus bus)
        {
            Id = id;
            Bus = bus;
            Position = new Vect3(0, 0, 0);
            Rotation = new Quat(Math.Sqrt(0.5), 0, 0, Math.Sqrt(0.5));

            Velocity = new Vect3(0, 0, 0);
            RotationalVelocity = new Euler(Angle.FromDegrees(1), Angle.FromDegrees(1), Angle.FromDegrees(1)).toQuat();
        }

        #region IGameObject Members

        public MessageBus Bus { get; private set; }
        public int Id { get; private set; }

        public Vect3 Position { get; private set; }
        public Quat Rotation { get; private set; }

        public void Update(TickTime tickTime)
        {
            Rotation = Rotation*(RotationalVelocity*tickTime.GameTimeDelta.TotalSeconds);
        }

        #endregion

        #region IHasVelocity Members

        public Vect3 Velocity { get; private set; }
        public Quat RotationalVelocity { get; private set; }

        #endregion
    }
}