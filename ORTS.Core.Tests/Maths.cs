using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ORTS.Core.Maths;

namespace ORTS.Core.Tests
{
    [TestClass]
    public class Maths
    {
        [TestMethod]
        public void Vect3InitBlankTest()
        {
            Vect3 vec = new Vect3();
            Assert.AreEqual<double>(0.0, vec.X);
            Assert.AreEqual<double>(0.0, vec.Y);
            Assert.AreEqual<double>(0.0, vec.Z);
        }
        [TestMethod]
        public void Vect3InitValuesTest()
        {

            Vect3 vec = new Vect3(1.0, 2.0, 3.0);
            Assert.AreEqual<double>(1.0, vec.X);
            Assert.AreEqual<double>(2.0, vec.Y);
            Assert.AreEqual<double>(3.0, vec.Z);
        }
        [TestMethod]
        public void Vect3LengthTest()
        {
            Assert.AreEqual<double>(new Vect3(5.0, 0.0, 0.0).LengthSquared, 25.0);
            Assert.AreEqual<double>(new Vect3(5.0, 0.0, 0.0).Length, 5.0);
        }
        [TestMethod]
        public void Vect3NormalTest()
        {
            Vect3 vec = new Vect3(5.0, 0.0, 0.0);
            Assert.AreEqual<double>(1.0, vec.Normalize().X);
            Assert.AreEqual<double>(0.0, vec.Normalize().Y);
            Assert.AreEqual<double>(0.0, vec.Normalize().Z);
        }
        [TestMethod]
        public void Vect3AddTest()
        {
            Vect3 vec = new Vect3(5.0, 8.0, 9.0) + new Vect3(5.0, 2.0,2.0);
            Assert.AreEqual<double>(10.0, vec.X);
            Assert.AreEqual<double>(10.0, vec.Y);
            Assert.AreEqual<double>(11.0, vec.Z);
            vec = new Vect3(5.0, 8.0, 9.0).Add(new Vect3(5.0, 2.0, 2.0));
            Assert.AreEqual<double>(10.0, vec.X);
            Assert.AreEqual<double>(10.0, vec.Y);
            Assert.AreEqual<double>(11.0, vec.Z);
        }
        [TestMethod]
        public void Vect3SubtractTest()
        {
            Vect3 vec = new Vect3(5.0, 8.0, 9.0) - new Vect3(5.0, 2.0, 2.0);
            Assert.AreEqual<double>(0.0, vec.X);
            Assert.AreEqual<double>(6.0, vec.Y);
            Assert.AreEqual<double>(7.0, vec.Z);
            vec = new Vect3(5.0, 8.0, 9.0).Subtract(new Vect3(5.0, 2.0, 2.0));
            Assert.AreEqual<double>(0.0, vec.X);
            Assert.AreEqual<double>(6.0, vec.Y);
            Assert.AreEqual<double>(7.0, vec.Z);
        }
        [TestMethod]
        public void Vect3MultiplyTest()
        {
            Vect3 vec = new Vect3(5.0, 8.0, 9.0) * 4.0;
            Assert.AreEqual<double>(20.0, vec.X);
            Assert.AreEqual<double>(32.0, vec.Y);
            Assert.AreEqual<double>(36.0, vec.Z);
        }
        [TestMethod]
        public void Vect3DivideTest()
        {
            Vect3 vec = new Vect3(5.0, 8.0, 9.0) / 4.0;
            Assert.AreEqual<double>(1.25, vec.X);
            Assert.AreEqual<double>(2.0, vec.Y);
            Assert.AreEqual<double>(2.25, vec.Z);
        }
        [TestMethod]
        public void Vect3CrossProductTest()
        {
            Vect3 vec = new Vect3(3.0, -3.0, 1.0).CrossProduct(new Vect3(4.0, 9.0, 2.0));
            Assert.AreEqual<double>(-15.0, vec.X);
            Assert.AreEqual<double>(-2.0, vec.Y);
            Assert.AreEqual<double>(39.0, vec.Z);
            vec = new Vect3(4.0, 9.0, 2.0).CrossProduct(new Vect3(3.0, -3.0, 1.0));
            Assert.AreEqual<double>(15.0, vec.X);
            Assert.AreEqual<double>(2.0, vec.Y);
            Assert.AreEqual<double>(-39.0, vec.Z);
        }
        [TestMethod]
        public void Vect3DotProductTest()
        {
            Assert.AreEqual<double>(new Vect3(1.0, 3.0, -5.0).DotProduct(new Vect3(4.0, -2.0, -1.0)), 3);
            Assert.AreEqual<double>(new Vect3(4.0, -2.0, -1.0).DotProduct(new Vect3(1.0, 3.0, -5.0)), 3);
        }
        [TestMethod]
        public void Vect3DistanceTest()
        {
            Assert.AreEqual<double>(0.0, new Vect3(5.0, 8.0, 9.0).Distance(new Vect3(5.0, 8.0, 9.0)));
            Assert.AreEqual<double>(5.0, new Vect3(0.0, 0.0, 0.0).Distance(new Vect3(5.0, 0.0, 0.0)));
            Assert.AreEqual<double>(0.0, new Vect3(5.0, 0.0, 0.0).Distance(new Vect3(5.0, 0.0, 0.0)));
        }

    }
}
