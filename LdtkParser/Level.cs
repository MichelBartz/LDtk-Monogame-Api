using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace LdtkParser
{
    public enum LayerType
    {
        Tiles,
        IntGrid,
        Entities
    };

    public class Level
    {
        public string LevelName { get; }
        public int WorldX { get; }
        public int WorldY { get; }
        public Texture2D BackgroundImage { get; }
        public Point BackgroundPosition { get; }

        private List<ILayer> layers;

        public Level(string name, int worldX, int worldY)
        {
            LevelName = name;
            WorldX = worldX;
            WorldY = worldY;

            layers = new List<ILayer>();
        }
        public Level(string name, int worldX, int worldY, Texture2D background, Point bgPosition)
        {
            LevelName = name;
            BackgroundImage = background;
            BackgroundPosition = bgPosition;
            WorldX = worldX;
            WorldY = worldY;

            layers = new List<ILayer>();
        }

        public void AddLayer(ILayer layer)
        {
            layers.Add(layer);
        }

        public IEnumerable<T> GetLayers<T>() => layers.OfType<T>();

        public T GetLayerByName<T>(string name) where T: ILayer => layers.OfType<T>().ToList<T>().Find(l => l.Name.Equals(name));
    }
}
