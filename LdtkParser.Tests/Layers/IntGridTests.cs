using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using LdtkParser.Layers;

namespace LdtkParser.Tests
{
    [TestClass]
    public class IntGridTests
    {
        [TestMethod]
        public void AddValue_WithValidData_UpdatesList()
        {
            var coord = new Point(1, 1);
            int value = 1;

            var intGridLayer = GetIntGridLayer();

            intGridLayer.AddValue(coord, value);

            Assert.AreEqual<int>(1, intGridLayer.Values.Count);
        }

        [TestMethod]
        public void GetValue_WithNonExistantCoordinates_ReturnsMinusOne()
        {
            var intGridLayer = GetIntGridLayer();

            int value = intGridLayer.GetValue(new Point(2, 2));

            Assert.AreEqual(-1, value);
        }

        [TestMethod]
        public void GetValue_AtExistingCoordinates_ReturnsCorrectValue()
        {
            var intGridLayer = GetIntGridLayer();
            intGridLayer.AddValue(new Point(2, 1), 2);

            int value = intGridLayer.GetValue(new Point(2, 1));

            Assert.AreEqual(2, value);
        }

        private IntGrid GetIntGridLayer() => new IntGrid("TestLayer");
    }
}
