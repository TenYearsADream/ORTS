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
        public static Quaternion ToQuaternion(this Quat q)
        {
            return new Quaternion((float)q.X, (float)q.Y, (float)q.Z, (float)q.W);
        }
        public static Quaterniond ToQuaterniond(this Quat q)
        {
            return new Quaterniond(q.X, q.Y, q.Z, q.W);
        }
    }
}
