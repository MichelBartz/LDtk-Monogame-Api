using Game1.Entity;
using Game1.Entity.Factory;
using Game1.Graphics;
using LdtkParser;
using LdtkParser.Layers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Game1
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteRenderer spriteRenderer;
        private Factory entityFactory;
        private World world;
        private LevelTiles currentLevelTiles;
        private List<BaseEntity> entities;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            var filePath = AppContext.BaseDirectory + Content.RootDirectory + "\\world.ldtk";

            world = new World(filePath, GraphicsDevice);
            spriteRenderer = new SpriteRenderer(_spriteBatch);
            spriteRenderer.SetScale(2.0f);
            
            entityFactory = new Factory(world, spriteRenderer);

            var ldtkLevel = world.GetLevel("Level_1");
            currentLevelTiles = entityFactory.FromLdtkLevel(ldtkLevel);

            var entities = ldtkLevel.GetLayerByName<Entities>("Entities");
            var player = entities.GetEntity<Player>();

            // Load player
            // Do i need an entity factory of some sort?
            // var playerInstance = world.GetEntityInstance(currentLevel, Player.Identifier);
            // player = entityFactory.FromPlayerInstance(playerInstance);

            // Load Collision layer

            // Read entities

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteRenderer.Begin();

            currentLevelTiles.Draw();
            //player.Draw();

            spriteRenderer.Commit();

            base.Draw(gameTime);
        }
    }
}
