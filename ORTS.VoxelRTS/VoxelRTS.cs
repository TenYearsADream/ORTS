using System;
using System.Linq;
using ORTS.Core;
using ORTS.Core.Messaging;
using ORTS.Core.Messaging.Messages;
using Ninject;
using Ninject.Modules;
using ORTS.Core.Graphics;
using System.Windows.Forms;
using ORTS.Core.GameObject;
using ORTS.VoxelRTS.GameObjects;
using ORTS.VoxelRTS.Messaging;

namespace ORTS.VoxelRTS
{
    class VoxelRTS
    {
        public VoxelRTS()
        {
            IKernel kernal = new StandardKernel(new GameModule());
            using (var engine = kernal.Get<GameEngine>())
            {
                engine.Bus.OfType<SystemMessage>().Subscribe(m => Console.WriteLine("{0} SYSTEM - {1}", m.TimeSent.ToString(), m.Message));
                engine.Bus.OfType<KeyDown>().Subscribe(m => Console.WriteLine("{0} KeyDown - {1}", m.TimeSent.ToString(), m.Key.ToString()));
                engine.Bus.OfType<KeyUp>().Subscribe(m => KeyUp(m, engine));
                //engine.Bus.OfType<GraphicsLoadedMessage>().Subscribe(m => engine.Bus.Add(new LoadObjectView(engine.Timer.LastTickTime, new TestObjectView(), typeof(TestObject))));
                //engine.Timer.Subscribe(t => engine.Bus.Add(new ObjectCreationRequest(engine.Timer.LastTickTime, typeof(TestObject))));
                
                engine.Start();
                while (engine.IsRunning)
                {

                }
                engine.Stop();
            }
        }
        private void KeyUp(KeyUp m, GameEngine engine)
        {
            Console.WriteLine("{0} KeyUp - {1}", m.TimeSent, m.Key);
            if (m.Key == Keys.Space)
            {
                engine.Bus.Add(new ObjectCreationRequest(engine.Timer.LastTickTime, typeof(VoxelGreen)));
            }

            if (m.Key == Keys.F)
            {
                engine.Bus.Add(new PlanetCreationRequest(engine.Timer.LastTickTime,PlanetType.Ice,9));
            }


            if (m.Key == Keys.Escape)
            {
                engine.Stop();
            }
        }
    }

    public class GameModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<GameEngine>().ToSelf();
            Kernel.Bind<MessageBus>().ToSelf().InSingletonScope();
            Kernel.Bind<GameObjectFactory>().To<VoxelRTSGameObjectFactory>().InSingletonScope();
            Kernel.Bind<IGraphics>().To<VoxelRTSGraphics>();
        }
    }
}
