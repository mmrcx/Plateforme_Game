#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Plateforme_Game.Core;
#endregion

namespace Plateforme_Game
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        public const int WINDOW_WIDTH = 1024;
        public const int WINDOW_HEIGHT = 310;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        World world;
        Player player;

        public static bool IsPaused = false;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            graphics.ApplyChanges();

            world = new World(this);
            player = new Player();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            world.Initialize("level1");

            player.Texture = Content.Load<Texture2D>("MarioSpriteSheet");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            CheckKeyboardAndUpdateMovement();

            world.Update(gameTime);

            if(world.Door.Bounds.Intersects(new Rectangle((int)player.Position.X, (int)player.Position.Y, (int)player.Size.X, (int)player.Size.Y))
                || player.Position.Y > 32*10)
            {
                player.Position = new Vector2(64, 64);
            }
            
            Vector2 oldPosition = player.Position;
            player.Update(gameTime);

            Vector2 newPosition = world.CheckAndFixPlayerPosition(oldPosition, player.Position, player.Size);//Board.CurrentBoard.WhereCanIGetTo(oldPosition, Position, Bounds);

            player.Position = newPosition;


            base.Update(gameTime);
        }

        private void CheckKeyboardAndUpdateMovement()
        {
            var keyboardState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            if (keyboardState.IsKeyDown(Keys.Right))
            {
                player.MoveRight();
            }

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                player.MoveLeft();
            }

            if (keyboardState.IsKeyDown(Keys.Up) && world.IsOnGround(player))
            {
                player.Jump();
            }

            if (keyboardState.IsKeyDown(Keys.Enter))
            {
                IsPaused = !IsPaused;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            {
                world.Draw(spriteBatch);
                player.Draw(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
