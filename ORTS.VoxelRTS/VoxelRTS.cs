using System;
using System.Linq;
using System.Windows.Forms;
using Ninject;
using Ninject.Modules;
using ORTS.Core;
using ORTS.Core.GameObject;
using ORTS.Core.Graphics;
using ORTS.Core.Messaging;
using ORTS.Core.Messaging.Messages;
using ORTS.Core.Sound;
using ORTS.VoxelRTS.States;

namespace ORTS.VoxelRTS
{
    internal class VoxelRTS
    {
        public VoxelRTS()
        {
            IKernel kernal = new StandardKernel(new GameModule());
            using (var engine = kernal.Get<GameEngine>())
            {
                
                engine.Bus.OfType<SystemMessage>().Subscribe(
                    m => Console.WriteLine("{0} SYSTEM - {1}", m.TimeSent.ToString(), m.Message));
                engine.Bus.OfType<KeyDownMessage>().Subscribe(
                    m => Console.WriteLine("{0} KeyDown - {1}", m.TimeSent.ToString(), m.Key.ToString()));
                engine.Bus.OfType<GraphicsLoadedMessage>().Subscribe(m => engine.CurrentState = new TopMenuState(engine));
                engine.Bus.OfType<KeyUpMessage>().Subscribe(m => KeyUp(m, engine));
                engine.Start();
                while (engine.IsRunning)
                {
                }
                engine.Stop();
            }
        }

        private void KeyUp(KeyUpMessage m, GameEngine engine)
        {
            engine.Bus.Add(new SystemMessage(engine.Timer.LastTickTime, "{0} KeyUp - {1}".fmt(m.TimeSent, m.Key)));
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
            Kernel.Bind<ISound>().To<VoxelRTSSound>();
        }
    }
}