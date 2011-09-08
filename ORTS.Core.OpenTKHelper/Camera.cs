using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORTS.Core.Graphics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using ORTS.Core.Maths;

namespace ORTS.Core.OpenTKHelper
{
    public class Camera: ICamera
    {
        public Quat Rotation { get; private set; }
        public Vect3 Postion { get; private set; }
        public Camera()
        {
            Rotation = Quat.Identity;
            Postion = Vect3.Zero;
        }
        public void Rotate(Quat q)
        {
            Rotation *= q;
        }
        public void Translate(Vect3 v)
        {
            Postion += -v;
        }
        public Matrix4 toMatrix4()
        {
            return new Matrix4();
        }
    }
}
