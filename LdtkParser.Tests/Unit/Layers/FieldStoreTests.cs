using NUnit.Framework;
using LdtkParser.Layers;
using LdtkParser.Exceptions;
using Microsoft.Xna.Framework;
using LdtkParser.Graphics;

namespace LdtkParser.Tests.Unit.Layers
{
    /// <summary>
    /// I won't test exception throwing on all Get methods, pointless.
    /// </summary>
    class FieldStoreTests
    {
        private FieldStore fieldStore;
        [SetUp]
        public void SetUp()
        {
            fieldStore = new FieldStore();
        }

        [TestCase("key", 12)]
        [TestCase("key", -1)]
        [TestCase("key", 8794)]
        public void GetInt_Valid_ReturnsValue(string key, int value)
        {
            fieldStore.Add(key, value);

            var intVal = fieldStore.GetInt(key);

            Assert.AreEqual(value, intVal);
        }

        [TestCase("key")]
        [TestCase("")]
        [TestCase("_ad;qwek")]
        public void GetInt_NonExistantKey_ThrowsException(string inexistantKey)
        {
            Assert.Throws<FieldStoreException>(() => fieldStore.GetInt(inexistantKey));
        }

        [TestCase("key", 12.0f)]
        [TestCase("key", 0f)]
        [TestCase("key", 8794f)]
        public void GetFloat_Valid_ReturnsValue(string key, float value)
        {
            fieldStore.Add(key, value);

            var floatVal = fieldStore.GetFloat(key);

            Assert.AreEqual(value, floatVal);
        }

        [TestCase("key", true)]
        [TestCase("key", false)]
        public void GetBool_Valid_ReturnsValue(string key, bool value)
        {
            fieldStore.Add(key, value);

            var boolVal = fieldStore.GetBool(key);

            Assert.AreEqual(value, boolVal);
        }

        [TestCase("key", "value")]
        [TestCase("key", "")]
        public void GetString_Valid_ReturnsValue(string key, string value)
        {
            fieldStore.Add(key, value);

            var stringVal = fieldStore.GetString(key);

            Assert.AreEqual(value, stringVal);
        }

        [TestCase("key", 0, 0)]
        [TestCase("key", 1, 1)]
        public void GetPoint_Valid_ReturnsValue(string key, int x, int y)
        {
            fieldStore.Add(key, new Point(x,y));

            var pointValue = fieldStore.GetPoint(key);

            Assert.AreEqual(pointValue.X, x);
            Assert.AreEqual(pointValue.Y, y);
        }

        [TestCase("key", "#FFFFFF")]
        [TestCase("key", "#000000")]
        public void GetColor_Valid_ReturnsValue(string key, string htmlHexColor)
        {
            Color c = ColorConverter.FromHTMLHex(htmlHexColor);
            fieldStore.Add(key, c);

            var colorValue = fieldStore.GetColor(key);
            Assert.AreEqual(c, colorValue);
        }

        [TestCase("key", "value", "enumType")]
        public void GetEnumValue_Valid_ReturnsValue(string key, string value, string enumType)
        {
            var v = new EnumValue(value, enumType);
            fieldStore.Add(key, v);

            var enumValue = fieldStore.GetEnumValue(key);
            Assert.AreEqual(v.Value, enumValue.Value);
            Assert.AreEqual(v.EnumType, enumValue.EnumType);
        }
    }
}
