using LicznikObiektow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var p1 = new Point(0, 0);
            var p2 = new Point(0, 0);
            Assert.IsTrue(Imaging.PointIsNextToOtherPoint(p1, p2));
        }

        [TestMethod]
        public void TestMethod2()
        {
            var p1 = new Point(0, 0);
            var p2 = new Point(1, 0);
            Assert.IsTrue(Imaging.PointIsNextToOtherPoint(p1, p2));
        }

        [TestMethod]
        public void TestMethod3()
        {
            var p1 = new Point(1, 1);
            var p2 = new Point(1, 1);
            Assert.IsTrue(Imaging.PointIsNextToOtherPoint(p1, p2));
        }
    }
}
