using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tetris.GameManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris.Screens
{
    class LoadingScreen : GameScreen
    {

        #region Fields

        private GameScreen screenToLoad;

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor is private, the Loading Screen should be called via the static Load() method.
        /// </summary>
        /// <param name="screenManager">The Screen Manager that the GameScreen should be loaded to.</param>
        /// <param name="screenToLoad">The Screen that needs to be loaded.</param>
        private LoadingScreen(ScreenManager screenManager,
            GameScreen screenToLoad)
        {
            this.screenToLoad = screenToLoad;
        }

        /// <summary>
        /// Activates the loading screen.
        /// </summary>
        public static void Load(ScreenManager screenManager,
                                 GameScreen screenToLoad)
        {
            // Tell all the current screens to transition off.
            foreach (GameScreen screen in screenManager.GetScreens())
                screen.ExitScreen();

            // Create and activate the loading screen.
            LoadingScreen loadingScreen = new LoadingScreen(screenManager,
                                                            screenToLoad);

            screenManager.AddScreen(loadingScreen);
        }

        #endregion

        #region Update and Draw

        public override void Update(GameTime gameTime)
        {
            //First update the base class, in case it is still performing operations on the screens for removal
            base.Update(gameTime);

            //Load the screen, if the screen object has been created yet.
            if (screenToLoad != null)
            {
                ScreenManager.RemoveScreen(this);
                ScreenManager.AddScreen(screenToLoad);
            }

            // Once the load has finished, we use ResetElapsedTime to tell
            // the  game timing mechanism that we have just finished a very
            // long frame, and that it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont spriteFont = ScreenManager.Font;
            string loadingText = "Loading...";

            //Calculate position for the drawing the text
            Vector2 stringSize = ScreenManager.Font.MeasureString(loadingText);
            Vector2 screenSize = new Vector2(
                ScreenManager.GraphicsDevice.Viewport.Width,
                ScreenManager.GraphicsDevice.Viewport.Height);

            Vector2 position = (screenSize - stringSize) / 2;

            spriteBatch.Begin();

            //Draw the loading text onto the screen.
            spriteBatch.DrawString(spriteFont,
                loadingText,
                position,
                Color.LimeGreen);

            spriteBatch.End();

            //base.Draw(gameTime);
        }

        #endregion
    }
}
