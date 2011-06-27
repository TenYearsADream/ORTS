using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORTS.Core.Maths
{
    public class Vect3 : IFormattable, IComparable, IComparable<Vect3>, IEquatable<Vect3>
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
        public static Vect3 UnitX
        {
            get { return new Vect3(1.0, 0, 0); }
        }
        public static Vect3 UnitY
        {
            get { return new Vect3(0, 1.0, 0); }
        }
        public static Vect3 UnitZ
        {
            get { return new Vect3(0, 0, 1.0); }
        }

        public double Length
        {
            get { return Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2)); }
        }

        public double LengthSquared
        {
            get { return Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2); }
        }

        public Vect3 Normalize()
        {
            if (this.Length == 0)
            {
                return Vect3.Zero;
            }
            else
            {
                return new Vect3(this.X / this.Length, this.Y / this.Length, this.Z / this.Length);
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

        public Vect3 CrossProduct(Vect3 v)
        {
            return new Vect3(this.Y * v.Z - this.Z * v.Y, this.Z * v.X - this.X * v.Z, this.X * v.Y - this.Y * v.X);
        }

        public double DotProduct(Vect3 v)
        {
            return this.X * v.X + this.Y * v.Y + this.Z * v.Z;
        }

        public double Distance(Vect3 v)
        {
            return Math.Sqrt((this.X - v.X) * (this.X - v.X) + (this.Y - v.Y) * (this.Y - v.Y) + (this.Z - v.Z) * (this.Z - v.Z));
        }


        public double Angle(Vect3 v)
        {
            return Math.Acos((this.Normalize()).DotProduct(v.Normalize()));
        }

        public Vect3 Max(Vect3 v)
        {
            return Max(this, v);
        }

        public Vect3 Min(Vect3 v)
        {
            return Min(this, v);
        }

        public Vect3 Interpolate(Vect3 other, double control)
        {
            return Interpolate(this, other, control);
        }

        public static Vect3 operator +(Vect3 v1, Vect3 v2)
        {
            return new Vect3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vect3 operator -(Vect3 v1, Vect3 v2)
        {
            return new Vect3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        public static Vect3 operator *(Vect3 v1, double v2)
        {
            return v1.Multiply(v2);
        }

        public static Vect3 operator /(Vect3 v1, double v2)
        {
            return v1.Divide(v2);
        }

        public static Vect3 operator +(Vect3 v1)
        {
            return new Vect3(+v1.X, +v1.Y, +v1.Z);
        }

        public static Vect3 operator -(Vect3 v1)
        {
            return new Vect3(-v1.X, -v1.Y, -v1.Z);
        }

        public static bool operator <(Vect3 v1, Vect3 v2)
        {
            return v1.Length < v2.Length;
        }

        public static bool operator <=(Vect3 v1, Vect3 v2)
        {
            return v1.Length <= v2.Length;
        }

        public static bool operator >(Vect3 v1, Vect3 v2)
        {
            return v1.Length > v2.Length;
        }

        public static bool operator >=(Vect3 v1, Vect3 v2)
        {
            return v1.Length >= v2.Length;
        }

        public static bool operator ==(Vect3 v1, Vect3 v2)
        {
            return ((v1.X == v2.X) && (v1.Y == v2.Y) && (v1.Z == v2.Z));
        }

        public static bool operator !=(Vect3 v1, Vect3 v2)
        {
            return !(v1 == v2);
        }

        public static double Angle(Vect3 v1, Vect3 v2)
        {
            return Math.Acos((v1.Normalize()).DotProduct(v2.Normalize()));
        }

        public static Vect3 Max(Vect3 v1, Vect3 v2)
        {
            if (v1 >= v2) { return v1; }
            return v2;
        }

        public static Vect3 Min(Vect3 v1, Vect3 v2)
        {
            if (v1 <= v2) { return v1; }
            return v2;
        }

        public static Vect3 Interpolate(Vect3 v1, Vect3 v2, double control)
        {
            if (control > 1 || control < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            else
            {
                return new Vect3(v1.X * (1 - control) + v2.X * control, v1.Y * (1 - control) + v2.Y * control, v1.Z * (1 - control) + v2.Z * control);
            }
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.Z.GetHashCode();
        }

        public override string ToString()
        {
            return (System.String.Format("Vector3({0},{1},{2})", X, Y, Z));
        }
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return (System.String.Format("Vector3({0},{1},{2})", X, Y, Z));
        }

        public int CompareTo(object obj)
        {
            if (obj is Vect3)
            {
                Vect3 otherVector = (Vect3)obj;
                if (this < otherVector) { return -1; }
                else if (this > otherVector) { return 1; }
                return 0;
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public int CompareTo(Vect3 other)
        {
            if (this < other)
            {
                return -1;
            }
            else if (this > other)
            {
                return 1;
            }

            return 0;
        }
        public override bool Equals(object other)
        {

            if (other is Vect3)
            {
                Vect3 otherVector = (Vect3)other;
                return otherVector == this;
            }
            else
            {
                return false;
            }
        }
        public bool Equals(Vect3 other)
        {
            return this == other;
        }
    }
}
