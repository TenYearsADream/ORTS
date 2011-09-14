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
        public WidgetFactory WidgetFactory { get; private set; }

        public IGraphics Graphics { get; private set; }
        private IGraphicsLoader _graphicsLoader { get; set; }

        public ISound Sound { get; private set; }

        
        public Task SoundTask { get; private set; }

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

        public GameEngine(MessageBus bus, GameObjectFactory factory, WidgetFactory widgetFactory, IGraphicsLoader graphicsLoader, ISound sound)
        {
            _currentState = new IdleState(this);
            Timer = new AsyncObservableTimer();
            Bus = bus;
            Factory = factory;
            _graphicsLoader = graphicsLoader;
            Sound = sound;

            WidgetFactory = widgetFactory;
            
            Bus.OfType<GraphicsLoadedMessage>().Subscribe(m => { 
                Graphics = m.Graphics;
                WidgetFactory.Initialise(Graphics);
                Bus.Add(new GameStartMessage(Timer.LastTickTime));
            });


            IsRunning = false;
            Timer.Subscribe(Update);
            Timer.SubSample(5).Subscribe(t => Bus.SendAll());

            //Timer.Subscribe(t => Bus.Add(new GraphicsDirtyMessage(t)));

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

            Bus.Add(new SystemMessage(Timer.LastTickTime, "Graphics Starting."));
            _graphicsLoader.Start(this);

            SoundTask = new Task(() => Sound.Start(this));
            SoundTask.Start();
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
