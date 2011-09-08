using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using ORTS.Core.Maths;

namespace ORTS.Core.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex
    {
        public float TexCoordX;
        public float TexCoordY;

        public float NormalX;
        public float NormalY;
        public float NormalZ;

        public float PositionX;
        public float PositionY;
        public float PositionZ;

        public Vertex(Vect2 texCoord, Vect3 normal, Vect3 position)
        {
            TexCoordX = (float)texCoord.X;
            TexCoordY = (float)texCoord.Y;
            NormalX = (float)normal.X;
            NormalY = (float)normal.Y;
            NormalZ = (float)normal.Z;
            PositionX = (float)position.X;
            PositionY = (float)position.Y;
            PositionZ = (float)position.Z;
        }
    }
}
