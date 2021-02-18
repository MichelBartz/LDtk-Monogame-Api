using Microsoft.Xna.Framework;

namespace LdtkParser.Graphics
{
    public class ColorConverter
    {
        public static Color FromHTMLHex(string htmlValue)
        {
            string hexValue = htmlValue.Replace("#", "") + "FF";
            uint colorValue = uint.Parse(hexValue, System.Globalization.NumberStyles.AllowHexSpecifier);
            return new Color(colorValue);
        }
    }
}
