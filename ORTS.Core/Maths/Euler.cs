using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORTS.Core.Maths
{
    public class Euler
    {
        public double Roll { get; private set; }
        public double Pitch { get; private set; }
        public double Yaw { get; private set; }

        public Quat toQuat()
        {
            double r = this.Roll * (Math.PI / 180) / 2.0;
            double p = this.Pitch * (Math.PI / 180) / 2.0;
            double y = this.Yaw * (Math.PI / 180) / 2.0;

            double sinp = Math.Sin(p);
            double siny = Math.Sin(y);
            double sinr = Math.Sin(r);
            double cosp = Math.Cos(p);
            double cosy = Math.Cos(y);
            double cosr = Math.Cos(r);

            return new Quat(sinr * cosp * cosy - cosr * sinp * siny, cosr * sinp * cosy + sinr * cosp * siny, cosr * cosp * siny - sinr * sinp * cosy, cosr * cosp * cosy + sinr * sinp * siny);
        }
    }
}
