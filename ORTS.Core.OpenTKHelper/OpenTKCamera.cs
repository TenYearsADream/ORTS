using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORTS.Core.Graphics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace ORTS.Core.OpenTKHelper
{
    public class OpenTKCamera: Camera
    {
        public Matrix4 toMatrix4()
        {
            return new Matrix4();
        }
    }
}
