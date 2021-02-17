using LdtkParser.Graphics;
using LdtkParser.Json;
using LdtkParser.Layers;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using System;
using LdtkLevel = LdtkParser.Json.Level;
using LdtkIntGrid = LdtkParser.Json.IntGrid;
using IntGrid = LdtkParser.Layers.IntGrid;
using Tileset = LdtkParser.Graphics.Tileset;
using Entities = LdtkParser.Layers.Entities;
using LdtkParser.Exceptions;

namespace LdtkParser
{
    // For now this is a bit of a entry point to the Ldtk file data
    // Might want to rethink it -> As an ouput I want something as Ldtk neutral as possible
    // We take the upfront approach: transform all of Ldtk at startup time rather than on demand
    public class World
    {
        private const string EnumPrefix = "LocalEnum.";

        private string filename;
        private List<SpriteEnum> spriteEnums;
        private Dictionary<string, List<string>> enums;
        private List<Level> levels;
        private Project ldtkProject;
        private string worldDirectory; // For loading tilesets
        private GraphicsDevice GraphicsDevice;

        private static Dictionary<int, Tileset> tilesets = new Dictionary<int, Tileset>();

        public World(string worldFile, GraphicsDevice graphicsDevice)
        {
            filename = worldFile;
            GraphicsDevice = graphicsDevice;
            spriteEnums = new List<SpriteEnum>();
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
            LoadLevels();
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

        private void LoadLevels()
        {
            ldtkProject.Levels.ForEach(delegate (LdtkLevel l)
            {
                Level level;
                
                if (l.BgRelPath != null)
                {
                    var texture = Texture2D.FromFile(GraphicsDevice, Path.Combine(worldDirectory, l.BgRelPath));
                    var bgPosition = new Point((int)l.BgPos.TopLeftPx[0], (int)l.BgPos.TopLeftPx[1]);

                    level = new Level(l.Identifier, (int)l.WorldX, (int)l.WorldY, texture, bgPosition);
                } 
                else
                {
                    level = new Level(l.Identifier, (int)l.WorldX, (int)l.WorldY);
                }

                l.LayerInstances.ForEach(l => LoadLayer(level, l));
                levels.Add(level);
            });
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
            var tiles = new Tiles(layer.Identifier, GetTileset((int)layer.TilesetDefUid));
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

        public Level GetLevel(string identifier) => levels.Find(l => l.LevelName.Equals(identifier));

        public static Tileset GetTileset(int uid)
        {
            Tileset t;
            if (tilesets.TryGetValue(uid, out t))
            {
                return t;
            }

            throw new TilesetNotFoundException("Tileset with id " + uid + " was not found");
        }

        public SpriteEnum GetSpriteEnum(string identifier)
        {
            return spriteEnums.Find(sm => sm.Identifier.Equals(identifier));
        }

    }
}
