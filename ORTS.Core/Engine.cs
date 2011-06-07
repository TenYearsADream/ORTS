using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORTS.Core.Messaging;
using ORTS.Core.Timing;
using ORTS.Core.Messages;
using ORTS.Core.Video;
using ORTS.Core.Sound;

namespace ORTS.Core
{
    public class Engine : IHasMessageBus, IDisposable
    {
        public MessageBus Bus { get; private set; }
        public ObservableTimer Timer { get; private set; }
        public bool IsRunning { get; private set; }

        public IVideo Video { get; private set; }
        public ISound Sound { get; private set; }

        public Engine()
        {

            this.Timer = new ObservableTimer();
            this.Bus = new MessageBus();

            //start video, sound

            this.Timer.Subscribe(t => this.Update(t));
            this.Timer.SubSample(5).Subscribe(t => Bus.SendAll());
        }

        public void Update(TickTime tickTime)
        {
            Bus.Add(new SystemMessage(Timer.LastTickTime, "tick"));
        }

        public void Start()
        {
            if (!IsRunning)
            {
                Bus.Add(new SystemMessage(Timer.LastTickTime, "Engine starting."));
                Timer.Start();
                IsRunning = true;
                Bus.Add(new SystemMessage(Timer.LastTickTime, "Engine started."));
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
