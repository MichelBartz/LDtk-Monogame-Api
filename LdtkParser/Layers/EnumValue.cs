namespace LdtkParser.Layers
{
    public class EnumValue
    {
        public string Value { get; }
        public string EnumType { get; }

        public EnumValue(string value, string enumType)
        {
            Value = value;
            EnumType = enumType;
        }
    }
}
