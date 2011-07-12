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
        public virtual void Start(GameEngine engine)
        {
            Bus.Add(new SystemMessage(engine.Timer.LastTickTime, "Graphics starting."));
            using (OpenTKWindow p = new OpenTKWindow(engine))
            {
                LoadViews(p);
                p.Run();
            }
        }
        public virtual void LoadViews(OpenTKWindow p)
        {

        }
        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
