using System;
using ORTS.Core.GameObject;
using ORTS.Core.Graphics;
using ORTS.Core.Messaging;
using ORTS.Core.Messaging.Messages;
using ORTS.Space.GameObjects;
using ORTS.Space.Widgets;

namespace ORTS.Space
{
    public class SpaceWidgetFactory: WidgetFactory
    {
        public SpaceWidgetFactory(MessageBus bus)
            : base(bus)
        {

        }

        public override void CreateWidget(WidgetCreationRequest m)
        {
            if (m.WidgetType == typeof(ChatWidget))
            {
                lock (WidgetsLock)
                {
                    Widgets.Add(new ChatWidget(Bus));
                }
            }
            if (m.WidgetType == typeof(MainMenuWidget))
            {
                lock (WidgetsLock)
                {
                    Widgets.Add(new MainMenuWidget(Bus));
                }
            }
        }
    }
}
