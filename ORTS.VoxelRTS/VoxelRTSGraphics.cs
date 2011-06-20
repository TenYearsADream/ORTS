using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORTS.Core.OpenTKHelper;
using ORTS.Core.Messaging;
using ORTS.VoxelRTS.GameObjects;
using ORTS.VoxelRTS.GameObjectViews;

namespace ORTS.VoxelRTS
{
    public class VoxelRTSGraphics : OpenTKGraphics
    {
        public VoxelRTSGraphics(MessageBus bus)
            : base(bus)
        {

        }
        public override void LoadViews(OpenTKWindow p)
        {
            p.LoadView(typeof(VoxelGreen), new VoxelGreenView());
            p.LoadView(typeof(Planet), new PlanetView());
        }
    }
}
