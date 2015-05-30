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
    class MainMenuScreen : MenuScreen
    {

        public MainMenuScreen()
            : base()
        {
            //Initialize menu items
            MenuEntry startGameEntry = new MenuEntry("Start Game");
            MenuEntry optionsEntry = new MenuEntry("Options");
            MenuEntry quitGameEntry = new MenuEntry("Exit");

            //Link event handlers to the events of the entries.
            startGameEntry.Selected += StartGameEntrySelected;
            quitGameEntry.Selected += QuitGameEntrySelected;

            //Add menu items to list.
            MenuEntries.Add(startGameEntry);
            MenuEntries.Add(optionsEntry);
            MenuEntries.Add(quitGameEntry);
        }

        #region Event Handlers

        /// <summary>
        /// Event handler for when the Start Game entry is selected.
        /// </summary>
        private void StartGameEntrySelected(object sender, EventArgs e)
        {
            //If start game is selected, load the GamePlayScreen using the LoadingScreen.
            LoadingScreen.Load(ScreenManager, new GamePlayScreen());
        }

        /// <summary>
        /// Event handler for when the Quit Game entry is selected.
        /// </summary>
        private void QuitGameEntrySelected(object sender, EventArgs e)
        {
            //Quit the game
            this.ScreenManager.Game.Exit();
            
        }

        #endregion

    }
}
