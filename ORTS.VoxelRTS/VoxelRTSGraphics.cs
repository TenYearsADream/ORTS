using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORTS.Core.Messaging;
using ORTS.Core.OpenTKHelper;
using ORTS.Core.Messaging.Messages;
using ORTS.VoxelRTS.GameObjects;
using ORTS.VoxelRTS.GameObjectViews;
using ORTS.Core;

namespace ORTS.VoxelRTS
{
    public class VoxelRTSGraphics : OpenTKGraphics
    {
        public VoxelRTSGraphics(MessageBus bus)
            : base(bus)
        {

        }
        public override void Start(GameEngine engine)
        {
            Bus.Add(new SystemMessage(engine.Timer.LastTickTime, "VoxelRTS Graphics starting."));
            using (VoxelRTSWindow p = new VoxelRTSWindow(engine))
            {
                LoadViews(p);
                p.Run();
            }
        }
        public void LoadViews(VoxelRTSWindow p)
        {
            p.LoadView(typeof(VoxelGreen), new VoxelGreenView());
            p.LoadView(typeof(Planet), new PlanetView());
        }
    }
}
