using LdtkParser.Graphics;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace LdtkParser.Tests.Unit.Graphics
{
    class ColorConverterTests
    {
       
        [TestCase("#FFFFFF", 255, 255, 255)]
        public void FromHTMLHex_WithValidColors_ReturnsXnaColorObject(string hexColor, byte r, byte g, byte b)
        {
            Color color = ColorConverter.FromHTMLHex(hexColor);

            Assert.AreEqual(color.R, r);
            Assert.AreEqual(color.G, g);
            Assert.AreEqual(color.B, b);
            Assert.AreEqual(color.A, 255);
        }
    }
}
