using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Tetris.GameManagement;

namespace Tetris.Screens
{
    /// <summary>
    /// Class to hold menu entries.
    /// </summary>
    abstract class MenuScreen : GameManagement.GameScreen
    {
        #region Fields

        private ContentManager content;
        private List<MenuEntry> menuEntries = new List<MenuEntry>();
        private int selectedEntry = 0;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the list of menu entries, so derived classes can add
        /// or change the menu contents.
        /// </summary>
        protected IList<MenuEntry> MenuEntries
        {
            get { return menuEntries; }
        }

        #endregion

        #region Initialization

        public MenuScreen()
        {
        }

        public override void LoadContent()
        {
            //Only retrieve a ContentManager if one isn't already in existence.
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            base.LoadContent();
        }

        public override void UnloadContent()
        {
            content.Unload();

            base.UnloadContent();
        }

        #endregion

        #region Update and Draw

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < menuEntries.Count; i++)
            {
                MenuEntry entry = menuEntries[i];

                entry.Update(gameTime,
                    (i == selectedEntry),
                    this
                    );
            }
            
            base.Update(gameTime);
        }
                
        public override void Draw(GameTime gameTime)
        {
            //First update the menu item positions
            UpdateMenuEntryPositions();

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();

            for (int i = 0; i < menuEntries.Count; i++)
            {
                //Determine if current entry is selected.
                bool isSelected = (i == selectedEntry);
                menuEntries[i].Draw(gameTime, isSelected, this);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Updates the positions of the Menu Entries.
        /// </summary>
        protected virtual void UpdateMenuEntryPositions()
        {
            //Calculate total height of all menu entries
            int menuHeight = 0;
            foreach (MenuEntry entry in menuEntries)
            {
                menuHeight += entry.GetHeight(this);
            }

            //Starting position. The X coordinate is the exact center of the screen, so we can easily calculate
            //the offset to center the entry.
            Vector2 startPosition = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2,
                (ScreenManager.GraphicsDevice.Viewport.Height - menuHeight) / 2
                );

            //Set menu entry positions
            for (int i = 0; i < menuEntries.Count; i++)
            {
                MenuEntry entry = menuEntries[i];
                Vector2 entryPosition = startPosition;

                //Center the entry
                entryPosition.X -= entry.GetWidth(this) / 2;
                entry.Position = entryPosition;

                //Move down 1 line for the next entry.
                startPosition.Y += entry.GetHeight(this);
            }
        }

        /// <summary>
        /// Handle the input for the menu screen.
        /// </summary>
        /// <param name="input"></param>
        public override void HandleInput(InputHandler input)
        {
            //Move to previous entry
            if (input.IsNewKeyPress(Keys.Up, false))
            {
                selectedEntry--;

                if (selectedEntry < 0)
                    selectedEntry = menuEntries.Count - 1;
            }

            //Move to next entry
            if (input.IsNewKeyPress(Keys.Down, false))
            {
                selectedEntry++;

                if (selectedEntry > menuEntries.Count - 1)
                    selectedEntry = 0;
            }
            
            //Select an entry
            if (input.IsNewKeyPress(Keys.Enter, false))
            {
                menuEntries[selectedEntry].OnSelectEntry();
            }

        }

        #endregion
    }
}
