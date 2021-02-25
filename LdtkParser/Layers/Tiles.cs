using LdtkParser.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace LdtkParser.Layers
{
    public class Tiles : ILayer
    {
        public string Name { get; }
        public Tileset Tileset { get; }
        public List<(Vector2, Rectangle)> TileCoords { get; }

        public Vector2 Offset { get; }
        public Tiles(string identifier, Tileset tileset, Vector2 offset)
        {
            Name = identifier;
            Tileset = tileset;
            TileCoords = new List<(Vector2, Rectangle)>();
            Offset = offset;
        }

        public LayerType GetLayerType() => LayerType.Tiles;

        public void AddTile(Vector2 coord, Rectangle source) => TileCoords.Add((coord, source));
    }
}
