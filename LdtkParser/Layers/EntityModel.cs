using LdtkParser.Json;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Dynamic;

namespace LdtkParser.Layers
{
    public class EntityModel
    {
        public string Identifier { get; }
        public Point GridPos { get; }
        public Point PxPos { get; }
        public double[] Pivot { get; }
        public Rectangle TileSrc { get; }
        public Graphics.Tileset Tileset { get; }
        public ExpandoObject Fields { get; }

        public EntityModel(EntityInstance entityInstance)
        {
            Identifier = entityInstance.Identifier;
            GridPos = new Point((int)entityInstance.Grid[1], (int)entityInstance.Grid[1]);
            PxPos = new Point((int)entityInstance.Px[0], (int)entityInstance.Px[1]);
            Pivot = new double[2] { entityInstance.Pivot[0], entityInstance.Pivot[1] };
            if (entityInstance.Tile != null)
            {
                Tileset = World.GetTileset((int)entityInstance.Tile.TilesetUid);
                TileSrc = new Rectangle((int)entityInstance.Tile.SrcRect[0], (int)entityInstance.Tile.SrcRect[1], (int)entityInstance.Tile.SrcRect[2], (int)entityInstance.Tile.SrcRect[3]);
            }
            Fields = new ExpandoObject();
            ProcessFieldInstances(entityInstance);
        }

        private void ProcessFieldInstances(EntityInstance entityInstance)
        {
            // We have N types, if Enum we want to store the enum type so that we can easily do a lookup in the EntityFactory if we wanted to
            entityInstance.FieldInstances.ForEach(delegate (FieldInstance f)
            {
                string[] typeSplit = f.Type.Split('.');

                switch (typeSplit[0])
                {
                    case "Int":
                        Fields.TryAdd(f.Identifier, (int)f.Value.Double);
                        break;
                    case "Float":
                        Fields.TryAdd(f.Identifier, (float)f.Value.Double);
                        break;
                    case "Bool":
                        Fields.TryAdd(f.Identifier, f.Value.Bool);
                        break;
                    case "String":
                        Fields.TryAdd(f.Identifier, f.Value.String);
                        break;
                    case "Point":
                        Fields.TryAdd(f.Identifier, new Point((int)f.Value.ValueClass.Cx, (int)f.Value.ValueClass.Cy));
                        break;
                    case "Color":
                        string hexValue = f.Value.String.Remove('#') + "FF";
                        uint colorValue = uint.Parse(hexValue, System.Globalization.NumberStyles.AllowHexSpecifier);
                        Color color = new Color(colorValue);
                        Fields.TryAdd(f.Identifier, color);
                        break;
                    case "LocalEnum":
                        Fields.TryAdd(f.Identifier, new { Value = f.Value.String, EnumType = f.Type });
                        break;
                }
            });
        }
    }
}
