using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORTS.Core.Maths
{
    public class Vect2
    {

        public double X { get; private set; }
        public double Y { get; private set; }

        public Vect2(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static Vect2 Zero
        {
            get { return new Vect2(0, 0); }
        }
    }
}
