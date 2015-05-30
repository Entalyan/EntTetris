using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris.Screens
{
    class MenuEntry
    {

        #region Fields

        private Vector2 position = Vector2.Zero;
        private string text;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the position of the menu item.
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// Gets or sets the text of the menu item.
        /// </summary>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        #endregion

        #region Events

        /// <summary>
        /// Event raised when the menu entry is selected.
        /// </summary>
        public event EventHandler<EventArgs> Selected;

        /// <summary>
        /// Method for raising the Selected event.
        /// </summary>
        protected internal virtual void OnSelectEntry()
        {
            if (Selected != null)
                Selected(this, new EventArgs());
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Creates a new MenuEntry with the specified text.
        /// </summary>
        /// <param name="text"></param>
        public MenuEntry(string text)
        {
            this.text = text;
        }

        #endregion

        #region Update and Draw

        /// <summary>
        /// Updates the Menu Entry.
        /// </summary>
        /// <param name="gameTime">Current GameTime.</param>
        /// <param name="isSelected">Specifies if the item is currently selected.</param>
        /// <param name="screen">The screen this entry belongs to.</param>
        public virtual void Update(GameTime gameTime, bool isSelected, MenuScreen screen)
        {

        }

        /// <summary>
        /// Draws the Menu Entry onto the screen.
        /// </summary>
        /// <param name="gameTime">Current GameTime.</param>
        /// <param name="isSelected">Specifies if the item is currently selected.</param>
        /// <param name="screen">The screen this entry belongs to.</param>
        public virtual void Draw(GameTime gameTime, bool isSelected, MenuScreen screen)
        {
            //A selected entry is brighter than a non-selected entry.
            Color entryColor = isSelected ? Color.LimeGreen: Color.Green;
            
            //Set up rendering objects
            SpriteBatch spriteBatch = screen.ScreenManager.SpriteBatch;
            SpriteFont spriteFont = screen.ScreenManager.Font;

            //Draw the menu item to the screen.
            spriteBatch.DrawString(spriteFont,
                this.text,
                this.position,
                entryColor);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Calulates the width of the entry.
        /// </summary>
        /// <param name="screen">The screen this entry belongs to.</param>
        /// <returns></returns>
        public int GetWidth(MenuScreen screen)
        {
            return (int)screen.ScreenManager.Font.MeasureString(this.text).X;
        }

        /// <summary>
        /// Calulates the height of the entry.
        /// </summary>
        /// <param name="screen">The screen this entry belongs to.</param>
        /// <returns></returns>
        public int GetHeight(MenuScreen screen)
        {
            return (int)screen.ScreenManager.Font.MeasureString(this.text).Y;
        }

        #endregion

    }
}
