using LdtkParser.Graphics;
using LdtkParser.Json;
using LdtkParser.Layers;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using LdtkLevel = LdtkParser.Json.Level;
using LdtkIntGrid = LdtkParser.Json.IntGrid;
using IntGrid = LdtkParser.Layers.IntGrid;
using Tileset = LdtkParser.Graphics.Tileset;
using Entities = LdtkParser.Layers.Entities;
using LdtkParser.Exceptions;

namespace LdtkParser
{
    /// <summary>
    /// Wraps the LDtk imported file and gives access to it's contents (via levels)
    /// </summary>
    public class World : IWorld
    {
        private const string EnumPrefix = "LocalEnum.";

        private readonly string filename;
        private readonly Dictionary<string, List<string>> enums;
        private readonly List<Level> levels;
        private readonly GraphicsDevice GraphicsDevice;
        private Project ldtkProject;
        private string worldDirectory; // For loading tilesets

        private readonly static List<SpriteEnum> spriteEnums = new List<SpriteEnum>();
        private readonly static Dictionary<int, Tileset> tilesets = new Dictionary<int, Tileset>();

        public World(string worldFile, GraphicsDevice graphicsDevice)
        {
            filename = worldFile;
            GraphicsDevice = graphicsDevice;
            enums = new Dictionary<string, List<string>>();
            levels = new List<Level>();
            Load();
        }

        private void Load()
        {
            var fileContents = File.ReadAllText(filename);
            worldDirectory = Path.GetDirectoryName(filename);

            ldtkProject = Project.FromJson(fileContents);

            LoadTilesets();
            LoadEnums();
            LoadLevels(ldtkProject.ExternalLevels);
        }
        private void LoadTilesets()
        {
            ldtkProject.Defs.Tilesets.ForEach(delegate (Json.Tileset t)
            {
                var texture = Texture2D.FromFile(GraphicsDevice, Path.Combine(worldDirectory, t.RelPath));
                var tileset = new Tileset(texture, (int)t.PxWid, (int)t.PxHei, (int)t.TileGridSize);
                tilesets.Add((int)t.Uid, tileset);
            });
        }

        private void LoadEnums()
        {
            ldtkProject.Defs.Enums.FindAll(e => e.IconTilesetUid.Equals(null))
                .ForEach(LoadStringEnums);

            ldtkProject.Defs.Enums.FindAll(e => e.IconTilesetUid != null)
                .ForEach(LoadSpriteEnums);
        }

        private void LoadSpriteEnums(Json.Enum e)
        {
            var spriteEnum = new SpriteEnum(EnumPrefix + e.Identifier, GetTileset((int)e.IconTilesetUid));
            e.Values.ForEach(delegate (Value v)
            {
                spriteEnum.AddSprite(v.Id, new Rectangle(
                    new Point((int)v.TileSrcRect[0], (int)v.TileSrcRect[1]),
                    new Point((int)v.TileSrcRect[2], (int)v.TileSrcRect[3])
                ));
            });

            spriteEnums.Add(spriteEnum);
        }

        private void LoadStringEnums(Json.Enum e)
        {
            var enumValues = new List<string>();
            e.Values.ForEach(v => enumValues.Add(v.Id));
            enums.Add(EnumPrefix + e.Identifier, enumValues);
        }

        private void LoadLevels(bool externalLevels)
        {
            if (externalLevels)
            {
                ldtkProject.Levels.ForEach(l => LoadLevelFromExternal(l.ExternalRelPath));
            } else
            {
                ldtkProject.Levels.ForEach(LoadLevel);
            }
            //Link neighbours
        }

        private void LoadLevelFromExternal(string path)
        {
            var fullPath = Path.Combine(worldDirectory, path);
            var levelContents = File.ReadAllText(fullPath);

            var ldtkLevel = Newtonsoft.Json.JsonConvert.DeserializeObject<LdtkLevel>(levelContents);
            LoadLevel(ldtkLevel);
        }

        private void LoadLevel(LdtkLevel l)
        {
            Level level;

            if (l.BgRelPath != null)
            {
                var texture = Texture2D.FromFile(GraphicsDevice, Path.Combine(worldDirectory, l.BgRelPath));
                var bgPosition = new Point((int)l.BgPos.TopLeftPx[0], (int)l.BgPos.TopLeftPx[1]);

                level = new Level((int)l.Uid, l.Identifier, (int)l.WorldX, (int)l.WorldY, (int)l.PxWid, (int)l.PxHei, texture, bgPosition);
            }
            else
            {
                level = new Level((int)l.Uid, l.Identifier, (int)l.WorldX, (int)l.WorldY, (int)l.PxWid, (int)l.PxHei);
            }

            l.LayerInstances.ForEach(l => LoadLayer(level, l));
            levels.Add(level);
        }

        private void LoadLayer(Level level, LayerInstance layerInstance)
        {
            switch (layerInstance.Type)
            {
                case nameof(LayerType.Tiles):
                    level.AddLayer(ToTiles(layerInstance));
                    break;
                case nameof(LayerType.IntGrid):
                    level.AddLayer(ToIntGrid(layerInstance));
                    break;
                case nameof(LayerType.Entities):
                    level.AddLayer(ToEntities(layerInstance));
                    break;
            }
        }

        private Tiles ToTiles(LayerInstance layer)
        {
            var tiles = new Tiles(layer.Identifier, GetTileset((int)layer.TilesetDefUid), new Vector2((float)layer.PxOffsetX, (float)layer.PxOffsetY));
            layer.GridTiles.ForEach(delegate (GridTile gridTile)
            {
                var coords = new Vector2((float)gridTile.Px[0], (float)gridTile.Px[1]);
                var sourceRectangle = new Rectangle((int)gridTile.Src[0], (int)gridTile.Src[1], (int)layer.GridSize, (int)layer.GridSize);

                tiles.AddTile(coords, sourceRectangle);
            });
            return tiles;
        }

        private Entities ToEntities(LayerInstance layer)
        {
            return new Entities(layer.Identifier, layer.EntityInstances);
        }

        private IntGrid ToIntGrid(LayerInstance layer)
        {
            var intGrid = new IntGrid(layer.Identifier);
            layer.IntGrid.ForEach(delegate (LdtkIntGrid iGrid)
            {
                int y = (int)(iGrid.CoordId / layer.CWid);
                int x = (int)(iGrid.CoordId - (y * layer.CWid));

                intGrid.AddValue(new Point(x, y), (int)iGrid.V);
            });

            return intGrid;
        }

        public List<Level> GetLevels() => levels;

        public Level GetLevel(string identifier) => levels.Find(l => l.LevelName.Equals(identifier));

        public Level GetLevelByUid(int uid) => levels.Find(l => l.Uid.Equals(uid));

        /// <summary>
        /// Returns a given tileset
        /// </summary>
        /// <param name="uid">The LDtk UID for the tileset</param>
        /// <returns>The found tileset</returns>
        /// <exception cref="LdtkParser.Exceptions.TilesetNotFoundException">Thrown if the tileset for the given UID does not exists</exception>
        public static Tileset GetTileset(int uid)
        {
            if (tilesets.TryGetValue(uid, out Tileset t))
            {
                return t;
            }

            throw new TilesetNotFoundException("Tileset with id " + uid + " was not found");
        }

        /// <summary>
        /// Gets a SpriteEnum
        /// A SpriteEnum is an abstraction level on LDtk Enums that have icons/tileset attached
        /// </summary>
        /// <param name="identifier">The name of the Enum as defined in LDtk</param>
        /// <returns>The SpriteEnum</returns>
        public static SpriteEnum GetSpriteEnum(string identifier)
        {
            return spriteEnums.Find(sm => sm.Identifier.Equals(identifier));
        }

    }
}
