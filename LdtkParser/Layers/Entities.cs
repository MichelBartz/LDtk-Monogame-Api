using LdtkParser.Json;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace LdtkParser.Layers
{
    public class Entities: ILayer
    {
        public string Name { get; }
        private readonly List<EntityInstance> entityInstances;

        public Entities(string identifier, List<EntityInstance> ei)
        {
            Name = identifier;
            entityInstances = ei;
        }
        public LayerType GetLayerType() => LayerType.Entities;


        //Will return firts one found, naive but useful for entities that have a max=1
        public T GetEntity<T>() where T: ILdtkEntity, new()
        {
            return Convert<T>(
                entityInstances.Where(ei => ei.Identifier.Equals(typeof(T).Name))
                    .First()
            );
        }
        // Do I worry about the performance of this?
        // Would I want to do a preloading indexing of EntityInstance by Identifier for easy yes/no lookup. (Seems like an obvious yes but is performance that bad as is?)
        public List<T> GetEntities<T>() where T: ILdtkEntity, new()
        {
           return entityInstances.Where(ei => ei.Identifier.Equals(typeof(T).Name))
                .Select(ei => Convert<T>(ei))
                .ToList();
        }

        private T Convert<T>(EntityInstance ei) where T: ILdtkEntity, new()
        {
            var entity = new T();
            entity.FromLdtk(new EntityModel(ei));
            return entity;
        }
    }

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

                switch(typeSplit[0])
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
