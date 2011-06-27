using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace ORTS.Core.Maths
{
    public class Quat : IEquatable<Quat>
    {
        public double W { get; private set; }
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }

        public static Quat Identity = new Quat(0, 0, 0, 1);

        public Quat()
        {
            this.X = 0.0;
            this.Y = 0.0;
            this.Z = 0.0;
            this.W = 1.0;
            this.Normalize();
        }

        public Quat(double X, double Y, double Z, double W)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.W = W;
            this.Normalize();
        }

        private void Normalize()
        {
            double mag = LengthSquared;
            if (mag != 0.0 && (Math.Round(mag * Math.Pow(10, 6)) / Math.Pow(10, 6) != 1.0))
            {
                mag = 1.0 / Math.Sqrt(mag);
                this.X = X * mag;
                this.Y = Y * mag;
                this.Z = Z * mag;
                this.W = W * mag;
            }
        }

        public double Length
        {
            get
            {
                return Math.Sqrt(this.LengthSquared);
            }
        }

        public double LengthSquared
        {
            get
            {
                return Math.Pow(W, 2) + Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2);
            }
        }

        #region Arithmetic Operations

        public static Quat operator +(Quat left, Quat right)
        {
            return new Quat(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
        }

        public static Quat operator -(Quat left, Quat right)
        {
            return new Quat(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);
        }

        public static Quat operator *(Quat q1, Quat q2)
        {
            return new Quat(
                q1.W * q2.X + q1.X * q2.W + q1.Y * q2.Z - q1.Z * q2.Y,
                q1.W * q2.Y + q1.Y * q2.W + q1.Z * q2.X - q1.X * q2.Z,
                q1.W * q2.Z + q1.Z * q2.W + q1.X * q2.Y - q1.Y * q2.X,
                q1.W * q2.W - q1.X * q2.X - q1.Y * q2.Y - q1.Z * q2.Z
                );
        }

        public static Quat operator *(Quat quat, double scalar)
        {
            return new Quat(quat.X * scalar, quat.Y * scalar, quat.Z * scalar, quat.W * scalar);
        }
        public static Quat operator *(double scalar, Quat quat)
        {
            return quat * scalar;
        }
        public static Quat operator /(Quat quat, double scalar)
        {
            return quat * (1/scalar);
        }

        public static bool operator ==(Quat q2, Quat q1)
        {
            return q2.Equals(q1);
        }
        public static bool operator !=(Quat q2, Quat q1)
        {
             return !q2.Equals(q1);
        }

        #endregion

        public Quat Inverse()
        {
            return new Quat(-X, -Y, -Z, W);
        }
        public Quat Conjugate()
        {
            return Inverse();   
        }
        

        public AxisAngle toAxisAngle()
        {
            double scale = this.Length;
            return new AxisAngle(new Vect3(this.X / scale, this.Y / scale, this.Z / scale), Math.Acos(this.W) * 2.0f);
        }

        public Euler toEuler()
        {
            throw new NotImplementedException();
        }

        public Mat4 toMat4()
        {
            throw new NotImplementedException();
        }

        public bool Equals(Quat other)
        {
           return this.X == other.X && this.Y == other.Y && this.Z == other.Z && this.W == other.W;
        }
        public override bool Equals(object other)
        {

            if (other is Quat)
            {
                Quat otherQuat = (Quat)other;
                return otherQuat == this;
            }
            else
            {
                return false;
            }
        }
        public override string ToString()
        {
            return "Quat({0},{1},{2},{3})".fmt(X, Y, Z, W);
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.Z.GetHashCode() ^ this.W.GetHashCode();
        }
    }
}
