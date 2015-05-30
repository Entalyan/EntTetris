using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;

        //Game objects
        private GameManagement.ScreenManager screenManager;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //Apply settings
            graphics.IsFullScreen = Settings.IsFullScreen;
            if (graphics.IsFullScreen)
            {
                graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            }
            else
            {
                //graphics.PreferredBackBufferWidth = Settings.DefaultWindowWidth;
                //graphics.PreferredBackBufferHeight = Settings.DefaultWindowHeight;
                graphics.PreferredBackBufferWidth = 1024;
                graphics.PreferredBackBufferHeight = 768;

            }

            graphics.ApplyChanges();

            //Determine scaling factor. Calculate based on height only (game should fit on 4:3 screens, so 
            //height is the main concern when scaling to 16:10 or 16:9. The scaling factor should take into
            //account the 10% border we will assume around the bottom of the screen.
            Settings.ScalingFactor = (float)(graphics.PreferredBackBufferHeight - graphics.PreferredBackBufferHeight * 0.1) / (float)Settings.DefaultWindowHeight;

            //Determine offset so the game will be drawn in the center of the screen.

            Window.Title = Settings.GameTitle;

            //Set up the game objects.
            Components.Add(new GameManagement.InputHandler(this));
            screenManager = new GameManagement.ScreenManager(this);
            Components.Add(screenManager);


            //Add game screens
            screenManager.AddScreen(new Screens.MainMenuScreen());
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

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            //spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if ( Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
