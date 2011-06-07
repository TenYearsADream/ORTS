using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORTS.Core.Maths
{
    class Vect3
    {
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }

        public Vect3()
        {

        }

        public Vect3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vect3 Zero
        {
            get { return new Vect3(); }
        }

        public double Length
        {
            get { return Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2)); }
        }

        public double LengthSquared
        {
            get { return Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2); }
        }

        public Vect3 Normal
        {
            get
            {
                if (this.Length == 0)
                    return Vect3.Zero;

                return this.Divide(this.Length);
            }
        }

        public Vect3 Add(Vect3 v)
        {
            return new Vect3(X + v.X, Y + v.Y, Z + v.Z);
        }

        public Vect3 Subtract(Vect3 v)
        {
            return new Vect3(X - v.X, Y - v.Y, Z - v.Z);
        }

        public Vect3 Multiply(double v)
        {
            return new Vect3(X * v, Y * v, Z * v);
        }

        public Vect3 Divide(double v)
        {
            return new Vect3(X / v, Y / v, Z / v);
        }
    }
}
