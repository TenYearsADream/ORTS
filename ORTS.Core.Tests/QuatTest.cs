using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ORTS.Core.Maths;

namespace ORTS.Core.Tests
{
    [TestClass]
    public class QuatTest
    {
        [TestMethod]
        public void QuatMultiplyTest1()
        {
            Quat q1 = new Quat(0, 0, 0, 1);
            Quat q2 = new Quat(1, 0, 0, 0);
            Quat result = q1 * q2;
            Assert.AreEqual<double>(1.0, result.X);
            Assert.AreEqual<double>(0.0, result.Y);
            Assert.AreEqual<double>(0.0, result.Z);
            Assert.AreEqual<double>(0.0, result.W);
        }
        [TestMethod]
        public void QuatMultiplyTest2()
        {
            Quat q1 = new Quat(0, 1, 0, 0);
            Quat q2 = new Quat(1, 0, 0, 0);
            Quat result = q1 * q2;
            Assert.AreEqual<double>(0.0, result.X);
            Assert.AreEqual<double>(0.0, result.Y);
            Assert.AreEqual<double>(-1.0, result.Z);
            Assert.AreEqual<double>(0.0, result.W);
        }
    }
}
