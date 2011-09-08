using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using Ninject;
using Ninject.Modules;
using ORTS.Core;
using ORTS.Core.Algorithms;
using ORTS.Core.Extensions;
using ORTS.Core.GameObject;
using ORTS.Core.Graphics;
using ORTS.Core.Graphics.ModelLoaders;
using ORTS.Core.Messaging;
using ORTS.Core.Messaging.Messages;
using ORTS.Core.Sound;
using ORTS.Space.GameObjects;
using ORTS.Space.States;
using ORTS.Space.Widgets;

namespace ORTS.Space
{
    public class SpaceModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<GameEngine>().ToSelf();
            Kernel.Bind<MessageBus>().ToSelf().InSingletonScope();
            Kernel.Bind<GameObjectFactory>().To<SpaceGameObjectFactory>().InSingletonScope();
            Kernel.Bind<WidgetFactory>().To<SpaceWidgetFactory>().InSingletonScope();
            Kernel.Bind<IGraphics>().To<SpaceGraphics>();
            Kernel.Bind<ISound>().To<SpaceSound>();
        }
    }
    public class Space
    {
        public Space()
        {
            IKernel kernal = new StandardKernel(new SpaceModule());
            using (var engine = kernal.Get<GameEngine>())
            {
                engine.Bus.Subscribe(m => Console.WriteLine(m.ToString()));
                engine.Bus.OfType<GraphicsLoadedMessage>().Subscribe(m => engine.CurrentState = new MainMenuState(engine));
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

            if(m.Key == Keys.P)
            {
                PerlinNoise perlinNoise = new PerlinNoise(99);
                Bitmap bitmap = new Bitmap(200, 200);
                double widthDivisor = 1 / (double)200;
                double heightDivisor = 1 / (double)200;
                bitmap.SetEachPixelColour(
                    (point, color) =>
                    {
                        double v =
                            // First octave
                            (perlinNoise.Noise(2 * point.X * widthDivisor, 2 * point.Y * heightDivisor, -0.5) + 1) / 2 * 0.7 +
                            // Second octave
                            (perlinNoise.Noise(4 * point.X * widthDivisor, 4 * point.Y * heightDivisor, 0) + 1) / 2 * 0.2 +
                            // Third octave
                            (perlinNoise.Noise(8 * point.X * widthDivisor, 8 * point.Y * heightDivisor, +0.5) + 1) / 2 * 0.1;

                        v = Math.Min(1, Math.Max(0, v));
                        byte b = (byte)(v * 255);
                        return Color.FromArgb(b, b, b);
                    });
                bitmap.Save("test.png", ImageFormat.Png);
            }
            if(m.Key == Keys.G)
            {
                
            }

        }
    }
}
