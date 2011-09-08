using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ORTS.Core.Graphics;
using ORTS.Core.Maths;
using ORTS.Core.Messaging;
using ORTS.Core;
using ORTS.Core.Attributes;
using ORTS.Core.Messaging.Messages;
using ORTS.Core.OpenTKHelper;
using ORTS.Core.States;
using ORTS.Space.GameObjects;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace ORTS.Space
{
    public class SpaceGraphics : IGraphics
    {
        public MessageBus Bus { get; private set; }

        public SpaceGraphics(MessageBus bus)
        {
            Bus = bus;
        }
        public void Start(GameEngine engine)
        {
            Bus.Add(new SystemMessage(engine.Timer.LastTickTime, "Graphics Starting."));
            var types = AppDomain.CurrentDomain.GetAssemblies().ToList()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(IGameObjectView).IsAssignableFrom(p) && p.IsClass);

            Bus.Add(new SystemMessage(engine.Timer.LastTickTime, "Found " + types.Count() + " Views"));

            using (var s = new SpaceWindow(engine))
            {
                foreach (var type1 in types.Where(type1 => type1.IsDefined(typeof (BindViewAttribute), false)))
                {
                    s.LoadView(type1.GetCustomAttributes(false).OfType<BindViewAttribute>().First().GameObjectType, (IGameObjectView) Activator.CreateInstance(type1,s.Camera));
                }
                s.Run();
            }
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }


    public class SpaceWindow : GameWindow
    {
        public GameEngine Engine { get; private set; }
        public ConcurrentDictionary<Type, IGameObjectView> Views { get; private set; }
        
        public Camera Camera { get; private set; }
        //private bool _graphicsDirty = true;
        private Matrix4 _perspective;

        private Rectangle _screen;


        public SpaceWindow(GameEngine engine)
            : base(1280, 720, new GraphicsMode(32, 0, 0, 4), "ORTS.Space")
        {
            _screen = new Rectangle(0, 0, 1280, 720);

            VSync = VSyncMode.On;
            Views = new ConcurrentDictionary<Type, IGameObjectView>();
            Engine = engine;
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
            engine.Bus.Add(new GraphicsLoadedMessage(engine.Timer.LastTickTime));
            engine.Bus.Add(new ObjectCreationRequest(engine.Timer.LastTickTime, typeof(SkyBox)));
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
                case MouseButton.Button1:
                case MouseButton.Button2:
                case MouseButton.Button3:
                case MouseButton.Button4:
                case MouseButton.Button5:
                case MouseButton.Button6:
                case MouseButton.Button7:
                case MouseButton.Button8:
                case MouseButton.Button9:
                case MouseButton.LastButton:
                    return Button.Left;
                default:
                    throw new ArgumentOutOfRangeException("button");
            }
        }
        void Mouse_ButtonDown(object sender, MouseButtonEventArgs e)
        {
            
        }


        public void LoadView(Type type, IGameObjectView view)
        {
            Views.TryAdd(type, view);
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
                Engine.Bus.Add(new SystemMessage(Engine.Timer.LastTickTime, "Loaded View: " + pair.Value.GetType().Name));
            }
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
            Title = "State: " + Engine.CurrentState + " FPS: " + string.Format("{0:F}", 1.0 / e.Time) + " Views Loaded: " + Views.Count + " Game Objects: " + Engine.Factory.GameObjects.Count;
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
