using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Tetris.GameManagement
{
    /// <summary>
    /// Enum describes the screen state.
    /// </summary>
    public enum ScreenState
    {
        Active,
        Hidden,
    }

    public abstract class GameScreen
    {
        #region Fields

        ScreenManager screenManager;
        ScreenState screenState = ScreenState.Active; //Default starting state.

        #endregion

        #region Properties

        /// <summary>
        /// Returns the state this GameScreen is in.
        /// </summary>
        public ScreenState ScreenState { 
            get
            { 
                return screenState; 
            } 
            internal set 
            { 
                screenState = value; 
            } 
        }

        /// <summary>
        /// Returns the ScreenManager this GameScreen belongs to.
        /// </summary>
        public ScreenManager ScreenManager
        {
            get { return screenManager; }
            internal set { screenManager = value; }
        }
        
        #endregion

        #region Initialization

        /// <summary>
        /// Initialize this GameScreen
        /// </summary>
        public virtual void Initialize() { }

        /// <summary>
        /// Load graphics content for this GameScreen
        /// 
        /// </summary>
        public virtual void LoadContent() { }

        /// <summary>
        /// Unloads graphics content for this GameScreen
        /// </summary>
        public virtual void UnloadContent() { }

        #endregion

        #region Update and Draw

        public virtual void Update(GameTime gameTime) { }

        public virtual void HandleInput(InputHandler input) { }

        public virtual void Draw(GameTime gameTime) { }

        #endregion

        #region Public Methods

        /// <summary>
        /// Tells the screen to exit.
        /// </summary>
        public void ExitScreen()
        {
            ScreenManager.RemoveScreen(this);
        }

        #endregion



    }
}
