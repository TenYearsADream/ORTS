using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ORTS.Core.Messaging;
using ORTS.Core.Messaging.Messages;

namespace ORTS.Core.Graphics
{
    public class WidgetFactory: IWidgetFactory
    {
        public List<IWidget> Widgets { get; private set; }
        public readonly object WidgetsLock = new object();
        public MessageBus Bus { get; private set; }
        public WidgetFactory(MessageBus bus)
        {
            Widgets = new List<IWidget>();
            Bus = bus;
            Bus.OfType<WidgetCreationRequest>().Subscribe(CreateWidget);
            Bus.OfType<WidgetDestructionRequest>().Subscribe(DestroyWidget);
            Bus.OfType<WidgetsDestroyAll>().Subscribe(m =>
                                                          {
                                                              lock (WidgetsLock)
                                                              {
                                                                  Widgets.Clear();
                                                              }
                                                          });
        }
        public virtual void CreateWidget(WidgetCreationRequest m)
        {

        }

        public void DestroyWidget(WidgetDestructionRequest m)
        {
            lock (WidgetsLock)
            {
                Widgets.Remove(m.Widget);
            }
        }
    }
}
