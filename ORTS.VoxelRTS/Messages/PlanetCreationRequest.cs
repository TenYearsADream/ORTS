using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORTS.Core.Timing;
using ORTS.VoxelRTS.GameObjects;
using ORTS.Core.Messaging;

namespace ORTS.VoxelRTS.Messaging
{
    public class PlanetCreationRequest : BaseMessage
    {
        public PlanetType PlanetType { get; private set; }
        public int PlanetSize { get; private set; }
        public PlanetCreationRequest(IGameTime timeSent, PlanetType planetType, int size)
            : base(timeSent)
        {
            this.PlanetType = planetType;
            this.PlanetSize = size;
        }
    }
}
