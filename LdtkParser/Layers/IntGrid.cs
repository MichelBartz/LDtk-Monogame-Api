using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace LdtkParser.Layers
{
    public class IntGrid : ILayer
    {
        public string Name { get; }
        public List<(Point Coord, int Value)> Values;
        public int GridSize { get; set; }

        public IntGrid(string identifier)
        {
            Name = identifier;
            Values = new List<(Point Coord, int Value)>();
        }

        public LayerType GetLayerType() => LayerType.IntGrid;

        public void AddValue(Point p, int value)
        {
            Values.Add((p, value));
        }

        /// <summary>
        /// Returns the given integer value for a gird coordinate
        /// </summary>
        /// <param name="gridCoord">The grid coordinate (in the current level the layer belongs to)</param>
        /// <returns>The value or -1</returns>
        public int GetValue(Point gridCoord)
        {
            int index = Values.FindIndex(((Point p, int v) intGrid) => intGrid.p.Equals(gridCoord));
            return index == -1 ? index : Values[index].Value;
        }
    }
}
