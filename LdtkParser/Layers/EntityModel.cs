using LdtkParser.Graphics;
using LdtkParser.Json;
using Microsoft.Xna.Framework;

namespace LdtkParser.Layers
{
    public class EntityModel
    {
        public string Identifier { get; }
        public Point GridPos { get; }
        public Vector2 PxPos { get; }
        public double[] Pivot { get; }
        public Rectangle TileSrc { get; }
        public Graphics.Tileset Tileset { get; }
        public FieldStore Fields { get; }

        public EntityModel(EntityInstance entityInstance)
        {
            Identifier = entityInstance.Identifier;
            GridPos = new Point((int)entityInstance.Grid[1], (int)entityInstance.Grid[1]);
            PxPos = new Vector2((int)entityInstance.Px[0], (int)entityInstance.Px[1]);
            Pivot = new double[2] { entityInstance.Pivot[0], entityInstance.Pivot[1] };
            if (entityInstance.Tile != null)
            {
                Tileset = World.GetTileset((int)entityInstance.Tile.TilesetUid);
                TileSrc = new Rectangle((int)entityInstance.Tile.SrcRect[0], (int)entityInstance.Tile.SrcRect[1], (int)entityInstance.Tile.SrcRect[2], (int)entityInstance.Tile.SrcRect[3]);
            }
            Fields = new FieldStore();
            ProcessFieldInstances(entityInstance);
        }

        private void ProcessFieldInstances(EntityInstance entityInstance)
        {
            entityInstance.FieldInstances.ForEach(delegate (FieldInstance f)
            {
                string[] typeSplit = f.Type.Split('.');

                switch (typeSplit[0])
                {
                    case "Int":
                        Fields.Add(f.Identifier, (int)f.Value.Double);
                        break;
                    case "Float":
                        Fields.Add(f.Identifier, (float)f.Value.Double);
                        break;
                    case "Bool":
                        Fields.Add(f.Identifier, f.Value.Bool);
                        break;
                    case "String":
                        Fields.Add(f.Identifier, f.Value.String);
                        break;
                    case "Point":
                        Fields.Add(f.Identifier, new Point((int)f.Value.ValueClass.Cx, (int)f.Value.ValueClass.Cy));
                        break;
                    case "Color":
                        Fields.Add(f.Identifier, ColorConverter.FromHTMLHex(f.Value.String));
                        break;
                    case "LocalEnum":
                        Fields.Add(f.Identifier, new EnumValue(f.Value.String, f.Type));
                        break;
                }
            });
        }
    }
}
