using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORTS.Core.Maths
{
    public class AxisAngle
    {
        public Vect3 Axis { get;  set; }
        public Angle Angle { get;  set; }
        public AxisAngle(Vect3 axis, Angle angle)
        {
            this.Axis = axis;
            this.Angle = angle;
        }
        public Quat toQuat(){
            Vect3 nvect = this.Axis.Normalise();
            double sinAngle = Math.Sin(this.Angle.Radians * 0.5);
            return new Quat(nvect.X * sinAngle, nvect.Y * sinAngle, nvect.Z * sinAngle, Math.Cos(this.Angle.Radians * 0.5));
        }
        public override string ToString()
        {
            return "AxisAngle(Angle:{0},{1})".fmt(this.Angle, this.Axis);
        }
    }
}
