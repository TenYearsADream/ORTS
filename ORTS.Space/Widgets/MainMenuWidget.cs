using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ORTS.Core.Graphics;
using ORTS.Core.Messaging;
using ORTS.Core.Messaging.Messages;
using OpenTK.Graphics.OpenGL;

namespace ORTS.Space.Widgets
{
    class MainMenuWidget : IWidget
    {
        public MessageBus Bus { get; private set; }
        public bool Active { get; private set; }
        public bool Loaded { get; private set; }
        private Rectangle _screen;

        private Rectangle _background = new Rectangle();
        public MainMenuWidget(MessageBus bus)
        {
            Bus = bus;
            Active = false;
            Loaded = false;
        }

        public void KeyUp(KeyUpMessage m)
        {

        }

        public void Load(Rectangle screen)
        {
            _screen = screen;
            Loaded = true;
        }

        private bool _hover = false;
        public void MouseMove(MouseMoveMessage m)
        {
            _hover = _background.Contains(m.Position);
        }

        public void Unload()
        {

        }

        public void Update(double delta)
        {
            _background = new Rectangle(100, 100, 400, 20);
        }

        public void Render()
        {
            GL.Disable(EnableCap.Texture2D);
    

            
            GL.Begin(BeginMode.Quads);

            GL.Color4(1, 0, 0, 0.5);
            GL.Vertex3(_background.Left, _background.Bottom, -0.1);
            GL.Vertex3(_background.Right, _background.Bottom, -0.1);
            GL.Vertex3(_background.Right, _background.Top, -0.1);
            GL.Vertex3(_background.Left, _background.Top, -0.1);

            GL.End();
            GL.Enable(EnableCap.Texture2D);
        }
    }
}
