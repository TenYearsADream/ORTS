using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORTS.Core.Maths;

namespace ORTS.Core.Graphics.Primatives
{
    public class Triangle
    {
        public readonly Vect3 P1;
        public readonly Vect3 P2;
        public readonly Vect3 P3;
        public readonly Vect3 Normal;
        public Triangle(Vect3 p1, Vect3 p2, Vect3 p3)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
            //calc normal
        }
        public Triangle(Vect3 p1, Vect3 p2, Vect3 p3, Vect3 normal)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
            Normal = normal;
        }
    }
}