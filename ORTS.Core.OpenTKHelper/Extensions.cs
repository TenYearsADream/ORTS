using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using ORTS.Core.Maths;

namespace ORTS.Core.OpenTKHelper
{
    public static class Extensions
    {
        public static Vector3 ToVector3(this Vect3 v)
        {
            return new Vector3((float)v.X, (float)v.Y, (float)v.Z);
        }
    }
}
