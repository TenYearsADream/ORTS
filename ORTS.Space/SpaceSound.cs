using ORTS.Core;
using ORTS.Core.Messaging;
using ORTS.Core.Messaging.Messages;
using ORTS.Core.Sound;
using OpenTK;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;

namespace ORTS.Space
{
    class SpaceSound: ISound
    {
        public MessageBus Bus { get; private set; }
        public SpaceSound(MessageBus bus)
        {
            Bus = bus;
        }
        public void Start(GameEngine engine)
        {
            Bus.Add(new SystemMessage(engine.Timer.LastTickTime,"Sound Starting."));
        }

        public void Stop()
        {
            
        }
    }
}
