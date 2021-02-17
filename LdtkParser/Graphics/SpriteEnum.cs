using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace LdtkParser.Graphics
{
    /// <summary>
    /// A SpriteEnum is simply an Enum that has elements mapping to tiles.
    /// This allows us to access arbitrary elements and retrieve the corresponding tile.
    /// 
    /// It is not strictly necesarry but I figured it was a nice way to define sprite animation/sequences in LDtk 
    /// and access these easily from code side.
    /// </summary>
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

        /// <summary>
        /// Returns the sprite identified by key if found
        /// </summary>
        /// <param name="key">A key name.</param>
        /// <returns>A sprite if found</returns>
        public Sprite GetSpriteByKey(string key)
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
