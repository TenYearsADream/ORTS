using System;
using ORTS.Core.GameObject;
using ORTS.Core.Messaging;
using ORTS.Core.Sound;
using ORTS.Core.Timing;
using ORTS.Core.Graphics;
using System.Threading.Tasks;
using System.Threading;
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

            this.Timer = new AsyncObservableTimer();
            this.Bus = bus;
            this.Factory = factory;
            this.Graphics = graphics;
             IsRunning = false;
            this.Timer.Subscribe(t => this.Update(t));
            this.Timer.SubSample(5).Subscribe(t => Bus.SendAll());
        }

        public void Update(TickTime tickTime)
        {
            lock (Factory.GameObjectsLock)
            {
                foreach (IGameObject gameobject in Factory.GameObjects)
                {
                    gameobject.Update(tickTime);
                }
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


                GraphicsTask = new Task(() => this.Graphics.Start(this));
                
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
