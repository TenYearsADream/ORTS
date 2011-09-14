using ORTS.Core.Graphics;
using ORTS.Core.Messaging;
using ORTS.Core.Messaging.Messages;
using ORTS.VoxelRTS.GameObjects;
using ORTS.VoxelRTS.GameObjectViews;
using ORTS.Core;

namespace ORTS.VoxelRTS
{
    public class VoxelRTSGraphics : IGraphics
    {
        public MessageBus Bus { get; private set; }
        public VoxelRTSGraphics(MessageBus bus)
        {
            Bus = bus;
        }
        public void Start(GameEngine engine)
        {
            Bus.Add(new SystemMessage(engine.Timer.LastTickTime, "VoxelRTS Graphics starting."));
            using (var p = new VoxelRTSWindow(engine))
            {
                p.LoadView(typeof(VoxelGreen), new VoxelGreenView());
                p.LoadView(typeof(Planet), new PlanetView());
                p.LoadView(typeof(TopMenu), new TopMenuView(p));
                p.Run();
            }
        }

        public void Initialise(GameEngine engine)
        {
            throw new System.NotImplementedException();
        }

        public void Run()
        {
            throw new System.NotImplementedException();
        }

        public void Start()
        {
            throw new System.NotImplementedException();
        }

        public void Stop()
        {

        }

        public float FramesPerSecond
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}
