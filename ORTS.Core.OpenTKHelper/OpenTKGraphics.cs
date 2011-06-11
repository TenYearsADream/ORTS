using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORTS.Core.Graphics;
using ORTS.Core.Messaging;

namespace ORTS.Core.OpenTKHelper
{
    public class OpenTKGraphics : IGraphics,IHasMessageBus
    {
        public MessageBus Bus { get; private set; }

        public OpenTKGraphics(MessageBus bus)
        {
            this.Bus = bus;
            
        }
        public void Start(GameEngine engine)
        {
            Bus.Add(new SystemMessage(engine.Timer.LastTickTime, "Graphics starting."));
            using (OpenTKWindow p = new OpenTKWindow(engine))
            {
                p.Run();
            }
        }
        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
