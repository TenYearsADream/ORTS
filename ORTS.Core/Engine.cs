using System;
using ORTS.Core.Extensions;
using ORTS.Core.GameObject;
using ORTS.Core.Messaging;
using ORTS.Core.Messaging.Messages;
using ORTS.Core.Sound;
using ORTS.Core.Timing;
using ORTS.Core.Graphics;
using System.Threading.Tasks;

namespace ORTS.Core
{
    public class GameEngine : IHasMessageBus, IDisposable
    {
        public MessageBus Bus { get; set; }
        public AsyncObservableTimer Timer { get; private set; }
        public bool IsRunning { get; private set; }

        public GameObjectFactory Factory { get; private set; }
        public IGraphics Graphics { get; private set; }
        public ISound Sound { get; private set; }

        public Task GraphicsTask { get; private set; }

        public GameEngine(MessageBus bus, GameObjectFactory factory, IGraphics graphics)
        {

            Timer = new AsyncObservableTimer();
            Bus = bus;
            Factory = factory;
            Graphics = graphics;
            IsRunning = false;
            Timer.Subscribe(Update);
            Timer.SubSample(5).Subscribe(t => Bus.SendAll());
            Timer.SubSample(5).Subscribe(t => Bus.Add(new GraphicsDirtyMessage(t)));
        }

        public void Update(TickTime tickTime)
        {
            lock (Factory.GameObjectsLock)
            {
                Parallel.ForEach(Factory.GameObjects, gameobject => gameobject.Update(tickTime));
                
                /*
                foreach (IGameObject gameobject in Factory.GameObjects)
                {
                    gameobject.Update(tickTime);
                }*/
            }
        }

        public void Start()
        {
            if (!IsRunning)
            {
                Bus.Add(new SystemMessage(Timer.LastTickTime, "Engine starting."));
                Timer.Start();
                IsRunning = true;
                Bus.Add(new SystemMessage(Timer.LastTickTime, "Engine started."));
                GraphicsTask = new Task(() => Graphics.Start(this));
                GraphicsTask.Start();

            }
        }

        public void Stop()
        {
            if (IsRunning)
            {
                Bus.Add(new SystemMessage(Timer.LastTickTime, "Engine stopping."));
                IsRunning = false;
                Timer.Stop();
                Bus.Add(new SystemMessage(Timer.LastTickTime, "Engine stopped."));

            }
        }
        public void Dispose()
        {
            Stop();
        }
    }
}
