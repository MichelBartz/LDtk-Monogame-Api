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

    public enum Direction
    {
        North,
        East,
        South,
        West,
        Invalid
    }

    public class Level
    {
        public int Uid { get; }
        public string LevelName { get; }
        public int WorldX { get; }
        public int WorldY { get; }
        public Texture2D BackgroundImage { get; }
        public Point BackgroundPosition { get; }

        private readonly List<Neighbour> neighbours;

        private readonly List<ILayer> layers;

        public Level(int uid, string name, int worldX, int worldY)
        {
            Uid = uid;
            LevelName = name;
            WorldX = worldX;
            WorldY = worldY;

            layers = new List<ILayer>();
            neighbours = new List<Neighbour>();
        }
        public Level(int uid, string name, int worldX, int worldY, Texture2D background, Point bgPosition)
        {
            Uid = uid;
            LevelName = name;
            BackgroundImage = background;
            BackgroundPosition = bgPosition;
            WorldX = worldX;
            WorldY = worldY;

            layers = new List<ILayer>();
            neighbours = new List<Neighbour>();
        }

        public void AddNeighbour(int uid, string direction)
        {
            var n = new Neighbour(uid, direction);
            // We ignore invalid neighbours
            if (n.Dir != Direction.Invalid) {
                neighbours.Add(n);
            }
        }

        public int GetNeighbourUid(Direction dir)
        {
            Neighbour n = neighbours.Find(n => n.Dir.Equals(dir));
            if (n != null)
            {
                return n.Uid;
            }
            return -1;
        }
        public void AddLayer(ILayer layer)
        {
            layers.Add(layer);
        }

        public IEnumerable<T> GetLayers<T>() => layers.OfType<T>();

        public T GetLayerByName<T>(string name) where T: ILayer => layers.OfType<T>().ToList<T>().Find(l => l.Name.Equals(name));
    }
}
