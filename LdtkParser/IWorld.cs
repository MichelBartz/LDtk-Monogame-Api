using LdtkParser.Graphics;

namespace LdtkParser
{
    public interface IWorld
    {
        Level GetLevel(string identifier);
    }
}