using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORTS.Core.Messaging;
using ORTS.Core.Collections;

namespace ORTS.Core.GameObject
{
    public class GameObjectFactory:IGameObjectFactory
    {
        public List<IGameObject> GameObjects { get; private set; }
        public readonly object GameObjectsLock = new object();
        public MessageBus Bus { get; private set; }
        protected Int32 NameCounter = 0;
        public GameObjectFactory(MessageBus bus)
        {
            GameObjects = new List<IGameObject>();
            Bus = bus;
            Bus.OfType<ObjectCreationRequest>().Subscribe(m => CreateGameObject(m));
            Bus.OfType<ObjectDestructionRequest>().Subscribe(m => DestroyGameObject(m));
        }

        public virtual void CreateGameObject(ObjectCreationRequest m)
        {

        }

        public void DestroyGameObject(ObjectDestructionRequest m)
        {
            lock (GameObjectsLock)
            {
                GameObjects.Remove(m.GameObject);
            }
            Bus.Add(new ObjectDestroyed(m.TimeSent, m.GameObject));
        }
    }
}
