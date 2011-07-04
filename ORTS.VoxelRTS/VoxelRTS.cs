using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORTS.Core;
using ORTS.Core.Messaging;
using ORTS.Core.Timing;
using Ninject;
using Ninject.Modules;
using ORTS.Core.Graphics;
using ORTS.Core.OpenTKHelper;
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
            using (GameEngine engine = kernal.Get<GameEngine>())
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
            Console.WriteLine("{0} KeyUp - {1}", m.TimeSent.ToString(), m.Key.ToString());
            if (m.Key == Keys.Space)
            {
                engine.Bus.Add(new ObjectCreationRequest(engine.Timer.LastTickTime, typeof(VoxelGreen)));
            }

            if (m.Key == Keys.F)
            {
                engine.Bus.Add(new PlanetCreationRequest(engine.Timer.LastTickTime,PlanetType.Ice,11));
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
