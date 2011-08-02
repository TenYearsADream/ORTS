using System;
using System.Linq;
using ORTS.Core.Extensions;
using ORTS.Core.GameObject;
using ORTS.Core.Messaging;
using ORTS.Core.Messaging.Messages;
using ORTS.Core.Sound;
using ORTS.Core.States;
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

        private IState _currentState;
        public IState CurrentState
        {
            get { return _currentState; }
            set
            {
                _currentState.Hide();
                _currentState = value;
                _currentState.Show();
            }
        }

        public GameEngine(MessageBus bus, GameObjectFactory factory, IGraphics graphics,ISound sound)
        {
            _currentState = new IdleState(this);
            Timer = new AsyncObservableTimer();
            Bus = bus;
            Factory = factory;
            Graphics = graphics;
            Sound = sound;
            IsRunning = false;
            Timer.Subscribe(Update);
            Timer.SubSample(5).Subscribe(t => Bus.SendAll());
            Timer.Subscribe(t => Bus.Add(new GraphicsDirtyMessage(t)));
            Bus.OfType<KeyUp>().Subscribe(m => _currentState.KeyUp(m));
            Bus.OfType<KeyDown>().Subscribe(m => _currentState.KeyDown(m));
        }

        public void Update(TickTime tickTime)
        {
            lock (Factory.GameObjectsLock)
            {
                Parallel.ForEach(Factory.GameObjects, gameobject => gameobject.Update(tickTime));
            }
            CurrentState.Update(tickTime);
        }

        public void Start()
        {
            if (IsRunning) return;
            Bus.Add(new SystemMessage(Timer.LastTickTime, "Engine starting."));
            Timer.Start();
            IsRunning = true;
            Bus.Add(new SystemMessage(Timer.LastTickTime, "Engine started."));
            GraphicsTask = new Task(() => Graphics.Start(this));
            GraphicsTask.Start();
        }

        public void Stop()
        {
            if (!IsRunning) return;
            Bus.Add(new SystemMessage(Timer.LastTickTime, "Engine stopping."));
            IsRunning = false;
            Timer.Stop();
            Bus.Add(new SystemMessage(Timer.LastTickTime, "Engine stopped."));
        }
        public void Dispose()
        {
            Stop();
        }

    }
}
