using Game1.Graphics;
using LdtkParser;
using LdtkParser.Layers;
using Microsoft.Xna.Framework;
using System.Linq;
using LdtkLevel = LdtkParser.Level;

namespace Game1.Entity.Factory
{
    class Factory
    {
        private World world;
        private SpriteRenderer spriteRenderer;
        
        public Factory(World w, SpriteRenderer sr)
        {
            world = w;
            spriteRenderer = sr;
        }

        public LevelTiles FromLdtkLevel(LdtkLevel ldtkLevel)
        {
            var level = new LevelTiles();
            level.SetSpriteRenderer(spriteRenderer); // <-- Not a massive fan of that tbh
            level.Tiles = ldtkLevel.GetLayers<Tiles>().ToList();
            // Fill with actual data from LDtk
            level.Location = new Point();
            level.Pos = new Vector2();

            return level;
        }
    }
}
