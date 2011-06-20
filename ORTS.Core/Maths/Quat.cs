using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORTS.Core.Maths
{
    public class Quat
    {
        public double W { get; private set; }
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }

        public Quat()
        {

        }

        public Quat(double X, double Y, double Z, double W)
        {
            
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.W = W;
        }

        public double Length
        {
            get
            {
                return System.Math.Sqrt(W * W + X*X + Y*Y + Z*Z);
            }
        }
        public double LengthSquared
        {
            get
            {
                return W * W + X * X + Y * Y + Z * Z;
            }
        }
        public Quat Normalise()
        {
            double mag = W * W + X * X + Y * Y + Z * Z;
            if (mag == 0)
            {
                return new Quat();
            }
            mag = 1.0 / Math.Sqrt(mag);
            return new Quat( X * mag, Y * mag, Z * mag, W * mag);
        }

        public Quat Conjugate()
        {
            return new Quat(-X, -Y, -Z, W);
        }


        public static Quat operator *(Quat left, Quat right)
        {
            return new Quat(
                left.W * right.X + left.X * right.W + left.Y * right.Z - left.Z * right.Y,
                left.W * right.Y + left.Y * right.W + left.Z * right.X - left.X * right.Z,
                left.W * right.Z + left.Z * right.W + left.X * right.Y - left.Y * right.X,
                left.W * right.W - left.X * right.X - left.Y * right.Y - left.Z * right.Z
                );
        }

    }
}
