using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using ORTS.Core.GameObject;
using ORTS.Core.Graphics;
using ORTS.Core.Messaging;

namespace ORTS.Core.OpenTKHelper
{
    public class OpenTKWindow : GameWindow
    {
        public GameEngine Engine { get; private set; }
        public ConcurrentDictionary<Type,IGameObjectView> Views { get; private set; }

        public OpenTKWindow(GameEngine engine)
            : base(800, 600, new GraphicsMode(32, 24, 0, 2), "ORTS.Test")
        {
            VSync = VSyncMode.On;
            Views= new ConcurrentDictionary<Type,IGameObjectView>();
            this.Engine = engine;

            this.Engine.Bus.OfType<LoadObjectView>().Subscribe(m => Views.TryAdd(m.GameObjectType,m.View));

            VSync = VSyncMode.Off;
            KeyMap map = new KeyMap();
            Keyboard.KeyDown += (object sender, KeyboardKeyEventArgs e) => { this.Engine.Bus.Add(new KeyDown(this.Engine.Timer.LastTickTime, map.Do(e.Key))); };
            Keyboard.KeyUp += (object sender, KeyboardKeyEventArgs e) => { this.Engine.Bus.Add(new KeyUp(this.Engine.Timer.LastTickTime, map.Do(e.Key))); };
            engine.Bus.Add(new GraphicsLoadedMessage(engine.Timer.LastTickTime));
        }
        public void LoadView(Type type, IGameObjectView View){
            Views.TryAdd(type, View);
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.ClearColor(new Color4(0.137f, 0.121f, 0.125f, 0f));
            GL.Enable(EnableCap.DepthTest | EnableCap.PolygonSmooth);
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            foreach (KeyValuePair<Type, IGameObjectView> pair in this.Views)
            {
                if (!pair.Value.Loaded)
                {
                    pair.Value.Load();
                    this.Engine.Bus.Add(new SystemMessage(this.Engine.Timer.LastTickTime, "Loaded: "+pair.Value.ToString()));
                }
            }
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 lookat = Matrix4.LookAt(0, 5, 50, 0, 0, 0, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookat);

            lock (this.Engine.Factory.GameObjectsLock)
            {
                foreach (IGameObject go in this.Engine.Factory.GameObjects)
                {
                    GL.PushMatrix();
                    if (go is IHasGeometry && this.Views.ContainsKey(go.GetType()))
                    {
                        this.Views[go.GetType()].Render(go as IHasGeometry);
                    }
                    GL.PopMatrix();
                }
            }

            this.Title = "FPS: " + string.Format("{0:F}", 1.0 / e.Time) +" Views Loaded: "+Views.Count + "Game Objects: "+Engine.Factory.GameObjects.Count;
            this.SwapBuffers();
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Width, Height);
            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)Width / (float)Height, 1, 64);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspective);
        }
        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Engine.Stop();
            base.OnClosing(e);
        }
    }
}
