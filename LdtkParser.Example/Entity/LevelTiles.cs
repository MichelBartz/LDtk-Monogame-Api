using LdtkParser.Layers;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Game1.Entity
{
    class LevelTiles : RenderableEntity
    {
        public List<Tiles> Tiles;

        public override void Draw() => Tiles.ForEach(t => RenderLayer(t));

        private void RenderLayer(Tiles tiles)
        {
            tiles.TileCoords.ForEach(((Vector2 coord, Rectangle source) tile) => spriteRenderer.DrawAt(tiles.Tileset.Texture, tile.source, tile.coord));
        }
    }
}
