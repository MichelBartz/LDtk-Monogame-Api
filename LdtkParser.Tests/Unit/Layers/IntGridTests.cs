using Microsoft.Xna.Framework;
using LdtkParser.Layers;
using NUnit.Framework;

namespace LdtkParser.Tests.Unit.Layers
{
    [TestFixture]
    public class IntGridTests
    {
        private IntGrid intGridLayer;
        [SetUp]
        public void SetUp()
        {
            intGridLayer = new IntGrid("TestLayer");
        }
        [Test]
        public void AddValue_WithValidData_UpdatesList()
        {
            var coord = new Point(1, 1);
            int value = 1;

            intGridLayer.AddValue(coord, value);

            Assert.AreEqual(1, intGridLayer.Values.Count);
        }
        [Test]
        public void GetValue_WithNonExistantCoordinates_ReturnsMinusOne()
        {
            int value = intGridLayer.GetValue(new Point(2, 2));

            Assert.AreEqual(-1, value);
        }
        [Test]
        public void GetValue_AtExistingCoordinates_ReturnsCorrectValue()
        {
            intGridLayer.AddValue(new Point(2, 1), 2);

            int value = intGridLayer.GetValue(new Point(2, 1));

            Assert.AreEqual(2, value);
        }
    }
}
