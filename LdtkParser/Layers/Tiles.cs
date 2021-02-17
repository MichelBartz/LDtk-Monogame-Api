using LdtkParser.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace LdtkParser.Layers
{
    public class Tiles : ILayer
    {
        public string Name { get; }
        public Tileset Tileset { get; }
        public List<(Vector2, Rectangle)> TileCoords;
        public Tiles(string identifier, Tileset tileset)
        {
            Name = identifier;
            Tileset = tileset;
            TileCoords = new List<(Vector2, Rectangle)>();
        }

        public LayerType GetLayerType() => LayerType.Tiles;

        public void AddTile(Vector2 coord, Rectangle source) => TileCoords.Add((coord, source));
    }
}
