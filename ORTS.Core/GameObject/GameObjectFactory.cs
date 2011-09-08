using System;
using System.Collections.Generic;
using System.Linq;
using ORTS.Core.Messaging;
using ORTS.Core.Messaging.Messages;

namespace ORTS.Core.GameObject
{
    public class GameObjectFactory:IGameObjectFactory
    {
        public List<IGameObject> GameObjects { get; private set; }
        public readonly object GameObjectsLock = new object();
        public MessageBus Bus { get; private set; }
        protected Int32 NameCounter;
        public GameObjectFactory(MessageBus bus)
        {
            GameObjects = new List<IGameObject>();
            Bus = bus;
            Bus.OfType<ObjectCreationRequest>().Subscribe(CreateGameObject);
            Bus.OfType<ObjectDestructionRequest>().Subscribe(DestroyGameObject);
            Bus.OfType<ObjectsDestroyAll>().Subscribe(m => GameObjects.Clear());
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
