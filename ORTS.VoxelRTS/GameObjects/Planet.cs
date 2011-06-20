using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORTS.Core.GameObject;
using ORTS.Core.Timing;
using ORTS.Core.Messaging;

namespace ORTS.VoxelRTS.GameObjects
{
    class Planet : IGameObject, IHasVelocity, IHasAcceleration, IHasGeometry, IParent
    {
        public void Update(TickTime tickTime)
        {
            throw new NotImplementedException();
        }

        public MessageBus Bus
        {
            get { throw new NotImplementedException(); }
        }

        public Core.Maths.Vect3 Velocity
        {
            get { throw new NotImplementedException(); }
        }

        public Core.Maths.Quat RotationalVelocity
        {
            get { throw new NotImplementedException(); }
        }

        public Core.Maths.Vect3 Position
        {
            get { throw new NotImplementedException(); }
        }

        public Core.Maths.Quat Rotation
        {
            get { throw new NotImplementedException(); }
        }

        public Core.Maths.Vect3 Acceleration
        {
            get { throw new NotImplementedException(); }
        }

        public Core.Maths.Quat RotationalAcceleration
        {
            get { throw new NotImplementedException(); }
        }

        public List<IGameObject> Children
        {
            get { throw new NotImplementedException(); }
        }

        public void AddChild(IGameObject Child)
        {
            throw new NotImplementedException();
        }
    }
}
