using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Tetris.GameManagement
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class ScreenManager : DrawableGameComponent
    {

        #region Fields

        private InputHandler inputHandler;

        private List<GameScreen> screens = new List<GameScreen>();
        private List<GameScreen> screensToUpdate = new List<GameScreen>();

        private SpriteBatch spriteBatch;
        private SpriteFont font;
        private Texture2D blankTexture;

        private bool isInitialized = false;

        #endregion

        #region Properties

        /// <summary>
        /// Default SpriteBatch.
        /// </summary>
        public SpriteBatch SpriteBatch { get { return spriteBatch; } }

        /// <summary>
        /// A default font.
        /// </summary>
        public SpriteFont Font { get { return font; } }

        /// <summary>
        /// A blank texture.
        /// </summary>
        public Texture2D BlankTexture { get { return blankTexture; } }

        /// <summary>
        /// The input handler for this screen manager.
        /// </summary>
        public InputHandler InputHandler { get { return inputHandler; } }

        #endregion

        #region Initialization

        public ScreenManager(Game game)
            : base(game)
        {
            try
            {
                inputHandler = (InputHandler)game.Services.GetService(typeof(IInputHandler));
            }
            catch (Exception e)
            {
                throw new NullReferenceException("Could not locate InputHandler service.", e);
            }
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            Game.IsFixedTimeStep = false;

            //Initialize all objects for the screens belonging to this manager.
            foreach (GameScreen screen in screens)
                screen.Initialize();

            base.Initialize();

            //Initialization of this class is called after the constructor of the calling class
            //is done executing. This variable allows methods to check if it has happened yet.
            isInitialized = true;
        }

        protected override void LoadContent()
        {

            // Load content belonging to the screen manager.
            ContentManager content = Game.Content;

            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = content.Load<SpriteFont>(Settings.FontName);
            blankTexture = content.Load<Texture2D>(Settings.BlankTextureName);

            //Load all content for the screens belonging to this manager.
            foreach (GameScreen screen in screens)
                screen.LoadContent();

            base.LoadContent();
        }

        protected override void UnloadContent()
        {

            foreach (GameScreen screen in screens)
                screen.UnloadContent();

            base.UnloadContent();
        }

        #endregion

        #region Update and Draw

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //First update the input handler
            //inputHandler.Update(gameTime);

            // Work with a copy of the screens list, so the main list doesn't have
            // to be altered.
            screensToUpdate.Clear();
            foreach (GameScreen screen in screens)
                screensToUpdate.Add(screen);

            while (screensToUpdate.Count > 0)
            {
                //Remove screen from the update list, then call its Update method.
                GameScreen screen = screensToUpdate[screensToUpdate.Count - 1];
                screensToUpdate.RemoveAt(screensToUpdate.Count - 1);

                screen.Update(gameTime);
                screen.HandleInput(inputHandler);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (GameScreen screen in screens)
            {
                if (screen.ScreenState == ScreenState.Hidden)
                    continue;

                screen.Draw(gameTime);
            }

            base.Draw(gameTime);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add a screen to the screenmanager.
        /// </summary>
        /// <param name="screen"></param>
        public void AddScreen(GameScreen screen)
        {
            screen.ScreenManager = this;
            screens.Add(screen);

            if (isInitialized)
            {
                screen.Initialize();
                screen.LoadContent();
            }

        }

        /// <summary>
        /// Remove a screen from the screenmanager.
        /// </summary>
        /// <param name="screen"></param>
        public void RemoveScreen(GameScreen screen)
        {
            screen.UnloadContent();
            screens.Remove(screen);
            screensToUpdate.Remove(screen);
        }

        /// <summary>
        /// Expose an array holding all the screens. We return a copy rather
        /// than the real master list, because screens should only ever be added
        /// or removed using the AddScreen and RemoveScreen methods.
        /// </summary>
        public GameScreen[] GetScreens()
        {
            return screens.ToArray();
        }

        #endregion
    }
}
