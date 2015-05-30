using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Tetris.GameManagement
{
    public interface IInputHandler
    {        
        bool KeyIsPressed(Keys key);
        bool IsNewKeyPress(Keys key, bool isRepeatAble);
    }

    /// <summary>
    /// This is a game component that implements IUpdateable and IInputHandler.
    /// </summary>
    public class InputHandler : Microsoft.Xna.Framework.GameComponent, IInputHandler
    {

        #region Fields

        List<KeyTimer> pressedKeys = new List<KeyTimer>();

        #endregion

        #region Properties

        #endregion

        #region Initialization

        public InputHandler(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(IInputHandler), this);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        #endregion

        #region Update

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            List<Keys> keyboardState = Keyboard.GetState().GetPressedKeys().ToList<Keys>();

            foreach (Keys key in keyboardState)
            {
                //We can assume the key is pressed while looping through this list.

                KeyTimer keyTimer = pressedKeys.Find(x => x.Key == key);
                if (keyTimer != null)
                {
                    //Key is still pressed, update its timer.
                    keyTimer.Update(gameTime.ElapsedGameTime);
                }
                else
                {
                    //Key is newly pressed, so add it to the list.
                    pressedKeys.Add(new KeyTimer(key));
                }
            }

            //Remove all keys that are no longer pressed.
            pressedKeys.RemoveAll(a => !keyboardState.Exists(x => x == a.Key));

            base.Update(gameTime);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Checks if a certain key is currently pressed.
        /// </summary>
        /// <param name="key">The key that needs to be checked.</param>
        /// <returns></returns>
        public bool KeyIsPressed(Keys key)
        {
            return pressedKeys.Exists(x => x.Key == key);
        }

        /// <summary>
        /// Checks if the key was just pressed, or has been pressed in a previous cycle.
        /// </summary>
        /// <param name="key">The key that needs to be checked.</param>
        /// <returns></returns>
        public bool IsNewKeyPress(Keys key, bool isRepeatAble)
        {
            KeyTimer pressedKey = pressedKeys.Find(x => x.Key == key);

            if (pressedKey == null)
            {
                //Key is not pressed at all
                return false;
            }
            else
            {
                //Key is pressed, check if it should register as a new press.
                if ((!isRepeatAble && pressedKey.TimePressed == TimeSpan.Zero) 
                    ||
                    (isRepeatAble && pressedKey.TimePressed >= Settings.RepeatRate || pressedKey.TimePressed == TimeSpan.Zero))
                {
                    //A new press has been detected, reset the timer for the key.
                    pressedKey.ResetTimer();
                    return true;
                }
                else
                {
                    //The key was pressed, but it does not register as a new press.
                    return false;
                }
            }
        }

        #endregion
    }
}
