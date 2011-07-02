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
        //public Vect3 Position { get; private set; }
        public Vect3 Velocity { get; private set; }
        public Vect3 Acceleration { get; private set; }
        public Quat RotationalVelocity { get; private set; }

        public Quat Rotation
        {
            get
            {
                if (Parent != null)
                {
                    return _rotation * Parent.Rotation;
                }
                else
                {
                    return _rotation;
                }
            }
            set
            {
                _rotation = value;
            }
        }
        
        
        public Vect3 Position { 
            get 
            {
                if(Parent != null){
                    return Parent.Position + _position.Rotate(Parent.Rotation);
                }else{
                    return _position;
                }
            }
            set
            {
                _position = value;
            }
        }



        private Vect3 _position;
        private Quat _rotation;

        public Quat RotationalAcceleration
        {
            get { throw new NotImplementedException(); }
        }

        public IParent Parent { get; set; }

        public VoxelGreen(MessageBus bus)
        {
            this.Bus = bus;
            Random rnd = new Random();
            this.Position = Vect3.Zero;
            this.Velocity = new Vect3(0, 0, 0);//
            this.Acceleration = new Vect3(0, 0, 0);

            this.Rotation = new Euler(Angle.FromDegrees(rnd.Next(0, 180)), Angle.FromDegrees(rnd.Next(0, 180)), Angle.FromDegrees(rnd.Next(0, 180))).toQuat();//new Quat(0, 0, Math.Sqrt(0.5), Math.Sqrt(0.5));
            this.RotationalVelocity = new Euler(Angle.FromDegrees(rnd.Next(-5, 5)), Angle.FromDegrees(rnd.Next(-5, 5)), Angle.FromDegrees(rnd.Next(-5, 5))).toQuat();
            
        }


        public void Update(TickTime tickTime)
        {
            this.Rotation = this._rotation * (this.RotationalVelocity * tickTime.GameTimeDelta.TotalSeconds);
            if (Parent == null)
            {
                //Velocity = Velocity + (Acceleration * tickTime.GameTimeDelta.TotalSeconds);
                this._position = this._position + (this.Velocity * tickTime.GameTimeDelta.TotalSeconds);
            }
        }
    }
}
