using LdtkParser.Exceptions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LdtkParser.Graphics
{
    public class Tileset
    {
        public Texture2D Texture;
        public int Width { get; }
        public int Height { get; }
        public int TileGridSize { get; }

        public int NumTiles { get => ((Width / TileGridSize) * (Height / TileGridSize)); }

        public Tileset(Texture2D txt, int width, int height, int gridSize)
        {
            Texture = txt;
            Width = width;
            Height = height;
            TileGridSize = gridSize;
        }

        public Rectangle GetTileById(int tileId)
        {
            if (tileId > NumTiles-1)
            {
                throw new InvalidTileException(tileId + " is not a valid tile id for this tileset");
            }

            int y = (int)(tileId / Width) * TileGridSize;
            int x = (int)(tileId - (y * Width)) * TileGridSize;

            return new Rectangle(x, y, TileGridSize, TileGridSize);
        }
    }
}
