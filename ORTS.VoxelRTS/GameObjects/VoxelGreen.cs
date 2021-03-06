﻿using System;
using ORTS.Core.Messaging;
using ORTS.Core.Messaging.Messages;
using ORTS.Core.GameObject;
using ORTS.Core.Timing;
using ORTS.Core.Maths;

namespace ORTS.VoxelRTS.GameObjects
{
    public class VoxelGreen : IGameObject, IBody, IHasParent
    {
        public MessageBus Bus { get; private set; }
        public int Id { get; private set; }
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

        public VoxelGreen(int ID, MessageBus bus)
        {
            this.Id = ID;
            this.Bus = bus;
            Random rnd = new Random();
            this.Position = new Vect3(rnd.Next(0, 5),rnd.Next(0, 5),rnd.Next(0, 5));
            this.Velocity = new Vect3(0, 0, 0);//
            this.Acceleration = new Vect3(0, 0, 0);

            this.Rotation = Quat.Identity;//new Euler(Angle.FromDegrees(rnd.Next(0, 180)), Angle.FromDegrees(rnd.Next(0, 180)), Angle.FromDegrees(rnd.Next(0, 180))).toQuat();//new Quat(0, 0, Math.Sqrt(0.5), Math.Sqrt(0.5));
            this.RotationalVelocity = Quat.Identity;//new Euler(Angle.FromDegrees(rnd.Next(-5, 5)), Angle.FromDegrees(rnd.Next(-5, 5)), Angle.FromDegrees(rnd.Next(-5, 5))).toQuat();
            
        }

        public void Update(TickTime tickTime)
        {
            if (Parent != null)
            {
                //Velocity = Velocity + (Acceleration * tickTime.GameTimeDelta.TotalSeconds);
                this.Rotation = this._rotation * (this.RotationalVelocity * tickTime.GameTimeDelta.TotalSeconds);
                this._position = this._position + (this.Velocity * tickTime.GameTimeDelta.TotalSeconds);
            }
        }
    }
}
