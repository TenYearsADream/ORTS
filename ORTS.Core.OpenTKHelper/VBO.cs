using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ORTS.Core.OpenTKHelper
{
    public class VBO<T>
    {
        private int _handle;
        public int Handle
        {
            get
            {
                if(_handle == 0)
                {
                    GL.GenBuffers(1, out _handle);
                }
                return _handle;
            }
        }

        private int _length;

        public void Load(T[] vertices)
        {
            _length = vertices.Length;
            GL.BindBuffer(BufferTarget.ArrayBuffer, Handle);
            //GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vertices.Length * Marshal.SizeOf(default(T))), vertices, BufferUsageHint.StaticDraw);
            int size;
            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);
            if (vertices.Length * BlittableValueType.StrideOf(vertices) != size)
                throw new ApplicationException("Vertex data not uploaded correctly");
        }

        public void Render(BeginMode mode)
        {
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, Handle);
            GL.VertexPointer(3, VertexPointerType.Float, Marshal.SizeOf(default(T)), new IntPtr(0));
            GL.DrawArrays(BeginMode.Quads, 0, _length);
        }
    }
}
