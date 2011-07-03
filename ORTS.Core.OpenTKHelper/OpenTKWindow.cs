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
using ORTS.Core.Maths;

namespace ORTS.Core.OpenTKHelper
{
    public class OpenTKWindow : GameWindow
    {
        public GameEngine Engine { get; private set; }
        public ConcurrentDictionary<Type,IGameObjectView> Views { get; private set; }

        public Camera camera { get; private set; }

        public OpenTKWindow(GameEngine engine)
            : base(800, 600, new GraphicsMode(32, 24, 0, 2), "ORTS.Test")
        {
            VSync = VSyncMode.On;
            Views= new ConcurrentDictionary<Type,IGameObjectView>();
            this.Engine = engine;

            this.Engine.Bus.OfType<LoadObjectView>().Subscribe(m => Views.TryAdd(m.GameObjectType,m.View));

            VSync = VSyncMode.Off;
            KeyMap map = new KeyMap();

            Keyboard.KeyDown += (object sender, KeyboardKeyEventArgs e) => { 
                this.Engine.Bus.Add(new KeyDown(this.Engine.Timer.LastTickTime, map.Do(e.Key))); 
            };
            Keyboard.KeyUp += (object sender, KeyboardKeyEventArgs e) => {
                this.Engine.Bus.Add(new KeyUp(this.Engine.Timer.LastTickTime, map.Do(e.Key))); 
            };

            Mouse.WheelChanged += (object sender, MouseWheelEventArgs e) => {
                camera.Translate(new Vect3(0,0,-e.DeltaPrecise));
            };

            engine.Bus.Add(new GraphicsLoadedMessage(engine.Timer.LastTickTime));

            camera = new Camera();
            camera.Translate(new Vect3(0, 0, 20));

           // GL.Enable(EnableCap.Lighting);

        }
        public void LoadView(Type type, IGameObjectView View){
            Views.TryAdd(type, View);
        }
        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(new Color4(0.137f, 0.121f, 0.125f, 0f));
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.DepthTest | EnableCap.PolygonSmooth);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

            foreach (KeyValuePair<Type, IGameObjectView> pair in this.Views)
            {
                if (!pair.Value.Loaded)
                {
                    pair.Value.Load();
                    this.Engine.Bus.Add(new SystemMessage(this.Engine.Timer.LastTickTime, "Loaded: " + pair.Value.ToString()));
                }
            }

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();

            GL.MatrixMode(MatrixMode.Modelview);
            GL.Translate(camera.postion.ToVector3());
            AxisAngle aa = camera.rotation.toAxisAngle();
            GL.Rotate(aa.Angle.Degrees, aa.Axis.ToVector3d());

            /*
            GL.Begin(BeginMode.Lines);
            GL.Color4(Color4.Red);
            GL.Vertex3(0f, 0f, 0f);
            GL.Vertex3(1f, 0f, 0f);
            GL.Color4(Color4.Green);
            GL.Vertex3(0f, 0f, 0f);
            GL.Vertex3(0f, 1f, 0f);
            GL.Color4(Color4.Blue);
            GL.Vertex3(0f, 0f, 0f);
            GL.Vertex3(0f, 0f, 1f);
            GL.End();
            */
            
            lock (this.Engine.Factory.GameObjectsLock)
            {
                foreach (IGameObject go in this.Engine.Factory.GameObjects)
                {
                    
                    if (go is IHasGeometry && this.Views.ContainsKey(go.GetType()))
                    {
                        GL.PushMatrix();
                        this.Views[go.GetType()].Render(go as IHasGeometry);
                        GL.PopMatrix();
                    }
                    
                }
            }

            this.Title = "FPS: " + string.Format("{0:F}", 1.0 / e.Time) +" Views Loaded: "+Views.Count + " Game Objects: "+Engine.Factory.GameObjects.Count;
            this.SwapBuffers();
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            if (Keyboard[Key.W])
            {
                camera.Translate(new Vect3(0, 10f * e.Time, 0));
            }

            if (Keyboard[Key.S])
            {
                camera.Translate(new Vect3(0, -10f * e.Time, 0));
            }

            if (Keyboard[Key.A])
            {
                camera.Translate(new Vect3(-10f * e.Time,0, 0));
            }

            if (Keyboard[Key.D])
            {
                camera.Translate(new Vect3(10f * e.Time, 0, 0));
            }
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Width, Height);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)Width / (float)Height, 1, 128);
            GL.LoadMatrix(ref perspective);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
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
