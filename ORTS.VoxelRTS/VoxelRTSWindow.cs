using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using ORTS.Core;
using ORTS.Core.OpenTKHelper;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using ORTS.Core.Graphics;
using ORTS.Core.Messaging.Messages;
using ORTS.Core.Maths;

namespace ORTS.VoxelRTS
{
    public class VoxelRTSWindow : GameWindow
    {
        public GameEngine Engine { get; private set; }
        public ConcurrentDictionary<Type,IGameObjectView> Views { get; private set; }

        public Camera Camera { get; private set; }
        private bool _graphicsDirty = true;
        public VoxelRTSWindow(GameEngine engine)
            : base(800, 600, new GraphicsMode(32, 24, 0, 2), "ORTS.Test")
        {
            VSync = VSyncMode.Off;
            Views= new ConcurrentDictionary<Type,IGameObjectView>();
            Engine = engine;

            Engine.Bus.OfType<LoadObjectView>().Subscribe(m => Views.TryAdd(m.GameObjectType,m.View));
            Engine.Bus.OfType<GraphicsDirtyMessage>().Subscribe(m => _graphicsDirty = true);


            var map = new KeyMap();
            Keyboard.KeyDown += (sender, e) => Engine.Bus.Add(new KeyDown(Engine.Timer.LastTickTime, map.Do(e.Key)));
            Keyboard.KeyUp += (sender, e) => Engine.Bus.Add(new KeyUp(Engine.Timer.LastTickTime, map.Do(e.Key)));
            Mouse.WheelChanged += (sender, e) => Camera.Translate(new Vect3(0,0,-e.DeltaPrecise));

            engine.Bus.Add(new GraphicsLoadedMessage(engine.Timer.LastTickTime));

            Camera = new Camera();
            Camera.Translate(new Vect3(0, 0, 30));
        }
        public void LoadView(Type type, IGameObjectView view){
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

            foreach (KeyValuePair<Type, IGameObjectView> pair in Views)
            {
                if (!pair.Value.Loaded)
                {
                    pair.Value.Load();
                    Engine.Bus.Add(new SystemMessage(Engine.Timer.LastTickTime, "Loaded: " + pair.Value));
                }
            }
        }

        
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            if (_graphicsDirty)
            {
                lock (Engine.Factory.GameObjectsLock)
                {
                    foreach (var go in Engine.Factory.GameObjects.Where(go => Views.ContainsKey(go.GetType())))
                    {
                        Views[go.GetType()].Add(go);
                    }
                }
                foreach (var pair in Views)
                {
                    pair.Value.Update();
                }
                _graphicsDirty = false;
            }

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();

            GL.MatrixMode(MatrixMode.Modelview);
            GL.Translate(Camera.postion.ToVector3());
            AxisAngle aa = Camera.rotation.toAxisAngle();
            GL.Rotate(aa.Angle.Degrees, aa.Axis.ToVector3d());


            foreach (KeyValuePair<Type, IGameObjectView> pair in Views)
            {
                pair.Value.Render();
            }

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
            GL.Color4(Color4.White);
            */
            Title = "FPS: " + string.Format("{0:F}", 1.0 / e.Time) +" Views Loaded: "+Views.Count + " Game Objects: "+Engine.Factory.GameObjects.Count;
            SwapBuffers();
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);


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
                Camera.Translate(new Vect3(-20f * e.Time,0, 0));
            }

            if (Keyboard[Key.D])
            {
                Camera.Translate(new Vect3(20f * e.Time, 0, 0));
            }


            if (Keyboard[Key.H])
            {

            }


        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Width, Height);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            var perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Width / (float)Height, 1, 512);
            GL.LoadMatrix(ref perspective);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }
        protected override void OnUnload(EventArgs e)
        {
            foreach (KeyValuePair<Type, IGameObjectView> pair in Views)
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
