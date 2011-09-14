using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using ORTS.Core.Graphics;
using ORTS.Core.Maths;
using ORTS.Core.Messaging;
using ORTS.Core;
using ORTS.Core.Attributes;
using ORTS.Core.Messaging.Messages;
using ORTS.Core.OpenTKHelper;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace ORTS.Space
{
    public class SpaceGraphicsLoader : IGraphicsLoader
    {
        public MessageBus Bus { get; private set; }
        public Task GraphicsTask { get; private set; }
        public SpaceGraphicsLoader(MessageBus bus)
        {
            Bus = bus;
        }

        public void Start(GameEngine engine)
        {
            
            GraphicsTask = new Task(() =>
                                        {
                                            using (var gfx = new SpaceWindow(engine))
                                            {
                                                gfx.Run();
                                            }
                                        });
            GraphicsTask.Start();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }


    public class SpaceWindow : GameWindow, IGraphics
    {
        public GameEngine Engine { get; private set; }
        public MessageBus Bus { get; private set; }
        public ConcurrentDictionary<Type, IGameObjectView> Views { get; private set; }
        public double FramesPerSecond { get; private set; }
        public Camera Camera { get; private set; }
        //private bool _graphicsDirty = true;
        private Matrix4 _perspective;

        private Rectangle _screen;

        public SpaceWindow(GameEngine engine)
            : base(1280, 720, new GraphicsMode(32, 0, 0, 4), "ORTS.Space")
        {
            _screen = new Rectangle(0, 0, 1280, 720);

            VSync = VSyncMode.Off;
            Views = new ConcurrentDictionary<Type, IGameObjectView>();
            Engine = engine;
            Bus = Engine.Bus;
            var map = new KeyMap();

            KeyPress += (sender, e) => Engine.CurrentState.KeyPress(new KeyPressMessage(Engine.Timer.LastTickTime, e.KeyChar));
            Keyboard.KeyDown += (sender, e) => Engine.CurrentState.KeyDown(new KeyDownMessage(Engine.Timer.LastTickTime, map.Match(e.Key)));
            Keyboard.KeyUp += (sender, e) => Engine.CurrentState.KeyUp(new KeyUpMessage(Engine.Timer.LastTickTime,map.Match(e.Key)));
            Mouse.Move += (sender, e) => Engine.CurrentState.MouseMove(new MouseMoveMessage(Engine.Timer.LastTickTime, e.Position));
            Mouse.ButtonDown += (sender, e) => Engine.CurrentState.MouseButtonDown(new MouseButtonDownMessage(Engine.Timer.LastTickTime, e.Position, MapMouseButton(e.Button)));
            Mouse.ButtonUp += (sender, e) => Engine.CurrentState.MouseButtonUp(new MouseButtonUpMessage(Engine.Timer.LastTickTime, e.Position, MapMouseButton(e.Button)));
            Mouse.WheelChanged += (sender, e) => Camera.Translate(new Vect3(0, 0, -e.DeltaPrecise));

            Camera = new Camera();
            Camera.Translate(new Vect3(0, 0, 30));
            LoadViews();

        }

        private Button MapMouseButton(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return Button.Left;
                case MouseButton.Middle:
                    return Button.Middle;
                case MouseButton.Right:
                    return Button.Right;
                default:
                    return Button.Left;
            }
        }

        private void LoadViews()
        {
            var count = 0;
            var types = AppDomain.CurrentDomain.GetAssemblies().ToList().SelectMany(s => s.GetTypes()).Where(p => typeof(IGameObjectView).IsAssignableFrom(p) && p.IsClass);
            foreach (var type1 in types.Where(type1 => type1.IsDefined(typeof(BindViewAttribute), false)))
            {
                Views.TryAdd(type1.GetCustomAttributes(false).OfType<BindViewAttribute>().First().GameObjectType, (IGameObjectView)Activator.CreateInstance(type1, Camera));
                count++;
            }
            Bus.Add(new SystemMessage(Engine.Timer.LastTickTime, "Found " + count + " Views"));
        }


        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(new Color4(0.137f, 0.121f, 0.125f, 0f));
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.ColorArray);
            GL.Enable(EnableCap.VertexArray);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
            foreach (var pair in Views.Where(pair => !pair.Value.Loaded))
            {
                pair.Value.Load();
                Bus.Add(new SystemMessage(Engine.Timer.LastTickTime, "Loaded View: " + pair.Value.GetType().Name));
            }
            Bus.Add(new GraphicsLoadedMessage(Engine.Timer.LastTickTime,this));
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Setup3D();
            lock (Engine.Factory.GameObjectsLock)
            {
                foreach (var pair in Views.Where(pair => Engine.Factory.GameObjects.Where(o => o.GetType() == pair.Key).Count() > 0))
                {
                    pair.Value.Render();
                }
            }
            Setup2D();
            lock (Engine.WidgetFactory.WidgetsLock)
            {
                foreach (var widget in Engine.WidgetFactory.Widgets.Where(widget => widget.Loaded))
                {
                    widget.Render();
                }
            }
            SwapBuffers();
            FramesPerSecond = 1/e.Time;
            Title = "State: " + Engine.CurrentState + " FPS: " + string.Format("{0:F}", FramesPerSecond) + " Views Loaded: " + Views.Count + " Game Objects: " + Engine.Factory.GameObjects.Count;
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            lock (Engine.Factory.GameObjectsLock)
            {
                foreach (var pair in Views)
                {
                    var pair1 = pair;
                    pair.Value.Update(Engine.Factory.GameObjects.Where(go => go.GetType() == pair1.Key),e.Time);
                }

            }

            lock (Engine.WidgetFactory.WidgetsLock)
            {
                foreach (var widget in Engine.WidgetFactory.Widgets)
                {
                    if(!widget.Loaded)
                        widget.Load(_screen);
                    widget.Update(e.Time);
                }
            }

            if (Keyboard[Key.W])
            {
                Camera.Translate(new Vect3(0, 20f * e.Time, 0));
            }

            if (Keyboard[Key.S])
            {
                Camera.Translate(new Vect3(0, -20f * e.Time, 0));
            }

            if (Keyboard[Key.A])
            {
                Camera.Translate(new Vect3(-20f * e.Time, 0, 0));
            }

            if (Keyboard[Key.D])
            {
                Camera.Translate(new Vect3(20f * e.Time, 0, 0));
            }

        }

        private void Setup3D()
        {
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref _perspective);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Translate(Camera.Postion.ToVector3());
            var aa = Camera.Rotation.toAxisAngle();
            GL.Rotate(aa.Angle.Degrees, aa.Axis.ToVector3d());
            GL.Enable(EnableCap.DepthTest);
        }

        private void Setup2D()
        {
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Scale(2.0 / Width, -2.0 / Height, -1);
            GL.Translate(-Width/2.0, -Height/2.0, 0);
            GL.Disable(EnableCap.DepthTest);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            _screen = new Rectangle(0, 0, Width, Height);
            Engine.Bus.Add(new ScreenResizeMessage(Engine.Timer.LastTickTime, _screen));
            GL.Viewport(0, 0, Width, Height);
            _perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Width / (float)Height, 1, 512);
        }

        protected override void OnUnload(EventArgs e)
        {
            foreach (var pair in Views)
            {
                pair.Value.Unload();
            }
            base.OnUnload(e);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Engine.Stop();
            base.OnClosing(e);
        }


    }
}
