using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace LdtkParser.Graphics
{
    public class SpriteEnum
    {
        public string Identifier { get; }
        public Tileset Tileset { get; }

        private readonly List<Sprite> sources;

        public SpriteEnum(string id, Tileset tileset)
        {
            Identifier = id;
            Tileset = tileset;
            sources = new List<Sprite>();
        }

        public void AddSprite(string name, Rectangle r)
        {
            sources.Add(new Sprite(name, r));
        }
        
        public Sprite GetSprite(int index)
        {
            return sources[index];
        }

        public Sprite GetFrameByKey(string key)
        {
            return sources.Find(f => f.Key.Equals(key));
        }
    }

    public class Sprite
    {
        public string Key;
        public Rectangle Source;

        public Sprite(string key, Rectangle src)
        {
            Key = key;
            Source = src;
        }
    }
}
