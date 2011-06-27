using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORTS.Core.Maths
{
    public class AxisAngle
    {
        public Vect3 Axis { get;  set; }
        public double Angle { get;  set; }
        public AxisAngle(Vect3 axis, double angle)
        {
            this.Axis = axis;
            this.Angle = angle;
        }
        public Quat toQuat(){
            Vect3 nvect = this.Axis.Normalize();
            double sinAngle = Math.Sin(this.Angle * 0.5);
            return new Quat(nvect.X * sinAngle, nvect.Y * sinAngle, nvect.Z * sinAngle, Math.Cos(this.Angle * 0.5));
        }
    }
}
