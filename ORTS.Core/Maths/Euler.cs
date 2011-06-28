using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORTS.Core.Maths
{
    public class Euler
    {
        public Angle Roll { get; private set; }
        public Angle Pitch { get; private set; }
        public Angle Yaw { get; private set; }

        public Euler(Angle roll, Angle pitch, Angle yaw)
        {
            this.Roll = roll;
            this.Pitch = pitch;
            this.Yaw = yaw;
        }

        public Quat toQuat()
        {
            double sinp = Math.Sin(this.Pitch.Radians / 2.0);
            double siny = Math.Sin(this.Yaw.Radians / 2.0);
            double sinr = Math.Sin(this.Roll.Radians / 2.0);
            double cosp = Math.Cos(this.Pitch.Radians / 2.0);
            double cosy = Math.Cos(this.Yaw.Radians / 2.0);
            double cosr = Math.Cos(this.Roll.Radians / 2.0);
            return new Quat(sinr * cosp * cosy - cosr * sinp * siny, cosr * sinp * cosy + sinr * cosp * siny, cosr * cosp * siny - sinr * sinp * cosy, cosr * cosp * cosy + sinr * sinp * siny);
        }
    }
}
