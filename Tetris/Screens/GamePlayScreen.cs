using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Tetris.GameManagement;
using Tetris.GameObjects;
using Tetris.GameObjects.Blocks;

namespace Tetris.Screens
{
    class GamePlayScreen : Tetris.GameManagement.GameScreen
    {

        #region Fields

        private ContentManager content;
        private Texture2D blockTexture;
        private Texture2D gameFieldBorderTexture;
        private Texture2D previewFieldBorderTexture;
        private Song backgroundMusic;
        private float fadeVolume;

        private RenderTarget2D gameField;
        private RenderTarget2D previewField;
        private RenderTarget2D backgroundTarget;


        //Rendering related settings
        private Vector2 gameFieldLocation;
        private Vector2 previewFieldLocation;
        private Vector2 textLocation;

        private Rectangle gameFieldBorder;
        private Rectangle previewFieldBorder;

        private int blockSize;

        //Game objects
        private ScoreManager scoreManager;
        private GamePlayManager currentLevel;
        private int levelWidth; //Expressed in number of blocks
        private int levelHeight; //Expressed in number of blocks

        private int numFreeFallSteps;

        //Timing
        private double nextTick;

        //Status
        private bool isResting;

        private GameState currentState = GameState.Playing;
        private enum GameState
        {
            Playing,
            GameOver,
        }

        #endregion

        #region Properties

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public GamePlayScreen()
        {
            //Set default values
            levelWidth = Settings.LevelWidth; //Expressed in number of blocks
            levelHeight = Settings.LevelHeight; //Expressed in number of blocks
        }


        /// <summary>
        /// Sets or resets the game to its starting position.
        /// </summary>
        public override void Initialize()
        {
            //Set size of blocks, accounting for scaling.
            blockSize = (int)Math.Round(Settings.BlockSize * Settings.ScalingFactor, 0);

            //Game controls
            scoreManager = new ScoreManager();
            scoreManager.Level = 1;
            nextTick = (0.05 * (11 - scoreManager.Level)) * 1000;
            numFreeFallSteps = 0;

            //Initialization of game variables
            isResting = true;
            currentState = GameState.Playing;

            currentLevel = new GamePlayManager(levelWidth, levelHeight);

        }

        /// <summary>
        /// Load graphics content for this GameScreen
        /// </summary>
        public override void LoadContent()
        {
            //Simulate slow loading to show loading screen
            System.Threading.Thread.Sleep(1000);

            //Only create a new ContentManager if one isn't already in existence.
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            //Load textures specific to this screen.
            blockTexture = content.Load<Texture2D>("SimpleSquare100x100");
            gameFieldBorderTexture = content.Load<Texture2D>("GreenBorder340x660");
            previewFieldBorderTexture = content.Load<Texture2D>("GreenBorder340x330");

            //Load the background music track
            //"Tetris" by Ind.FX is licensedunder a Creative Commons Attribution-NonCommercial-ShareAlike license: http://creativecommons.org/licenses/by-nc-sa/3.0/
            backgroundMusic = content.Load<Song>("Ind.FX_-_Tetris");
            fadeVolume = 0f;
            MediaPlayer.Volume = fadeVolume;

            //Set up our rendertargets
            gameField = new RenderTarget2D(ScreenManager.GraphicsDevice,
                Settings.LevelWidth * blockSize,
                Settings.LevelHeight * blockSize,
                false,
                SurfaceFormat.Color,
                DepthFormat.None);
            previewField = new RenderTarget2D(ScreenManager.GraphicsDevice,
                4 * blockSize,
                4 * blockSize,
                false,
                SurfaceFormat.Color,
                DepthFormat.None);

            //Set up a rectangle the size of the entire window, to draw the background onto.
            backgroundTarget = new RenderTarget2D(ScreenManager.GraphicsDevice,
                ScreenManager.GraphicsDevice.PresentationParameters.BackBufferWidth,
                ScreenManager.GraphicsDevice.PresentationParameters.BackBufferHeight,
                false,
                SurfaceFormat.Color,
                DepthFormat.None);

            //Dividers are 10% of screen dimensions.
            int widthDivider = backgroundTarget.Width / 10;
            int heightDivider = backgroundTarget.Height / 10;

            //Determine location of elements on the screen. Game field is the root node where all 
            //locations are based from, so its position is critical to the alignment of all other
            //UI elements. We want it to always be centered in the middle of the screen.
            //From left to right, the 2d elements that need to be drawn are the game field, a
            //divider, and the preview field. We will need to calculate their total width to be
            //able to center it.
            gameFieldLocation = new Vector2(
                            (ScreenManager.GraphicsDevice.PresentationParameters.BackBufferWidth / 2) -
                             ((gameField.Width + widthDivider + previewField.Width) / 2),
                            backgroundTarget.Height / 10
                            );

            //Preview field is located directly to the right of the game field, with a divider 
            //of 10% of window width. Its top is leveled with the game field.
            previewFieldLocation = new Vector2(
                gameFieldLocation.X + gameField.Width + widthDivider,
                gameFieldLocation.Y);

            //Text is located directly below the preview field, with another 10% of window 
            //height divider. Its left is aligned with the preview field.
            textLocation = new Vector2(
                previewFieldLocation.X,
                previewFieldLocation.Y + previewField.Height + heightDivider);

            //Set up rectangles for displaying borders on. 
            int borderWidth = 10;
            gameFieldBorder = new Rectangle(
                (int)gameFieldLocation.X - borderWidth,
                (int)gameFieldLocation.Y - borderWidth,
                gameField.Width + (borderWidth * 2),
                gameField.Height + (borderWidth * 2));

            previewFieldBorder = new Rectangle(
                (int)previewFieldLocation.X - borderWidth,
                (int)previewFieldLocation.Y - borderWidth,
                previewField.Width + (borderWidth * 2),
                previewField.Height + (borderWidth * 2));
        }

        /// <summary>
        /// Unloads graphics content for this GameScreen
        /// </summary>
        public override void UnloadContent()
        {
            if (gameField != null)
            {
                try
                {
                    gameField.Dispose();
                    gameField = null;
                    previewField.Dispose();
                    previewField = null;
                }
                catch (Exception e)
                {
                    throw new ContentLoadException("Couldn't unload content of GamePlayScreen.", e);
                }
            }

            content.Unload();
        }

        #endregion

        #region Update

        public override void Update(GameTime gameTime)
        {
            //Fade in music volume
            if (MediaPlayer.Volume < 1)
            {
                fadeVolume += 0.001f;
                MediaPlayer.Volume = fadeVolume;
            }


            //Start the audio, if audio is enabled and it is not already playing.
            if (Settings.AudioEnabled && MediaPlayer.State != MediaState.Playing)
            {
                MediaPlayer.Play(backgroundMusic);
            }



            if (currentState == GameState.Playing)
            {
                nextTick -= gameTime.ElapsedGameTime.Milliseconds;
                HandleAutoBlockMovement();
            }

        }

        #endregion

        #region Draw

        public override void Draw(GameTime gameTime)
        {
            //Only draw the level and previewfield if the game is running.
            //The gameField and previewField rendertargets are not cleared 
            //when the status is GameOver, because we want to preserve the
            //image they are holding.
            if (currentState == GameState.Playing)
            {
                DrawLevel();
                DrawPreviewField();
            }

            //Always draw the background.
            DrawUIElements();

            //Reset the graphics device back to its default setting
            ScreenManager.GraphicsDevice.SetRenderTarget(null);
            ScreenManager.GraphicsDevice.Clear(Color.Black);

            //Draw rendertarget into the backbuffer
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();
            spriteBatch.Draw(backgroundTarget, Vector2.Zero, Color.White);
            spriteBatch.Draw(gameField, gameFieldLocation, Color.White);
            spriteBatch.Draw(previewField, previewFieldLocation, Color.White);
            spriteBatch.End();
        }

        /// <summary>
        /// Draws the gameplay area to the RenderTarget.
        /// </summary>
        protected void DrawLevel()
        {
            //Set up drawing pass to the RenderTarget
            ScreenManager.GraphicsDevice.SetRenderTarget(gameField);
            ScreenManager.GraphicsDevice.Clear(Color.Black);

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            int[,] levelMatrix = currentLevel.LevelGrid();

            //Loop through all registered blocks in the LevelGrid
            for (int x = 0; x < levelWidth; x++)
            {
                for (int y = 0; y < levelHeight; y++)
                {
                    if (levelMatrix[x, y] > 0)
                    {
                        //Draw the block
                        spriteBatch.Draw(blockTexture,
                            new Rectangle((x * blockSize),
                                (y * blockSize),
                                blockSize,
                                blockSize),
                            BlockColors.GetColor(levelMatrix[x, y]));
                    }
                }
            }

            //End the drawing pass
            spriteBatch.End();

        }

        protected void DrawPreviewField()
        {
            //Get the block that should be put in the previewfield.
            Block nextBlock = currentLevel.NextBlock;

            //Because we want this preview block to be centered, we will not observe the matrix for drawing it.
            //Instead we will calculate an offset for it to be centered.
            int offSetX = (4 - nextBlock.Width) * (blockSize / 2);
            int offSetY = (4 - nextBlock.Height) * (blockSize / 2);

            //Set up drawing pass to the RenderTarget
            ScreenManager.GraphicsDevice.SetRenderTarget(previewField);
            ScreenManager.GraphicsDevice.Clear(Color.Black);

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            //Loop through all the individual blocks within the next shape.
            for (int x = 0; x < nextBlock.Width; x++)
            {
                for (int y = 0; y < nextBlock.Height; y++)
                {
                    if (nextBlock.Shape[x, y] > 0)
                    {
                        //Draw the square block
                        spriteBatch.Draw(blockTexture,
                            new Rectangle(offSetX + (x * blockSize),
                                offSetY + (y * blockSize),
                                blockSize,
                                blockSize),
                            BlockColors.GetColor(nextBlock.Shape[x, y]));
                    }

                }
            }

            //End the drawing pass.
            spriteBatch.End();

        }

        protected void DrawUIElements()
        {
            //Set up drawing pass to the RenderTarget
            ScreenManager.GraphicsDevice.SetRenderTarget(backgroundTarget);
            ScreenManager.GraphicsDevice.Clear(Color.Black);

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            spriteBatch.Draw(gameFieldBorderTexture,
                gameFieldBorder,
                Color.White);
            spriteBatch.Draw(previewFieldBorderTexture,
                previewFieldBorder,
                Color.White);

            //Draw the appropriate text for the game state. All text is aligned below the previewfield.
            switch (currentState)
            {
                case GameState.Playing:
                    spriteBatch.DrawString(ScreenManager.Font,
                        "Score: " + scoreManager.Score.ToString(),
                        new Vector2(previewFieldLocation.X, previewFieldLocation.Y + previewField.Height + 50),
                        Color.White);
                    spriteBatch.DrawString(ScreenManager.Font,
                       "Level: " + scoreManager.Level.ToString(),
                       new Vector2(previewFieldLocation.X, previewFieldLocation.Y + previewField.Height + 50 + ScreenManager.Font.LineSpacing),
                       Color.White);
                    break;
                case GameState.GameOver:
                    //Game over text
                    spriteBatch.DrawString(ScreenManager.Font,
                        "GAME OVER!",
                        new Vector2(previewFieldLocation.X, previewFieldLocation.Y + previewField.Height + (50 - ScreenManager.Font.LineSpacing)),
                        Color.White);
                    spriteBatch.DrawString(ScreenManager.Font,
                        "Score: " + scoreManager.Score.ToString(),
                        new Vector2(previewFieldLocation.X, previewFieldLocation.Y + previewField.Height + 50),
                        Color.White);
                    spriteBatch.DrawString(ScreenManager.Font,
                       "Level: " + scoreManager.Level.ToString(),
                       new Vector2(previewFieldLocation.X, previewFieldLocation.Y + previewField.Height + 50 + ScreenManager.Font.LineSpacing),
                       Color.White);
                    spriteBatch.DrawString(ScreenManager.Font,
                       "Press Enter \nto retry.",
                       new Vector2(previewFieldLocation.X, previewFieldLocation.Y + previewField.Height + 50 + (ScreenManager.Font.LineSpacing * 2)),
                       Color.White);
                    break;
            }

            spriteBatch.End();

        }

        #endregion

        #region Handlers

        /// <summary>
        /// Handles all the input controls.
        /// </summary>
        public override void HandleInput(InputHandler input)
        {
            //Control logic
            if (input.IsNewKeyPress(Keys.LeftAlt, false))
            {
                //Rotate clockwise
                currentLevel.RotateActiveBlock(RotationDirection.ClockWise);
            }
            if (input.IsNewKeyPress(Keys.LeftControl, false))
            {
                //Rotate counter clockwise
                currentLevel.RotateActiveBlock(RotationDirection.CounterClockWise);
            }
            if (input.IsNewKeyPress(Keys.Left, true))
            {
                //Move left
                currentLevel.MoveBlock(MovementDirection.Left);
            }
            if (input.IsNewKeyPress(Keys.Right, true))
            {
                //Move right
                currentLevel.MoveBlock(MovementDirection.Right);
            }
            if (input.IsNewKeyPress(Keys.Down, true))
            {
                //Move down
                currentLevel.MoveBlock(MovementDirection.Down);
            }
            if (input.IsNewKeyPress(Keys.Space, false))
            {
                //Drop the block
                while (currentLevel.BlockCanMove())
                {
                    currentLevel.MoveBlock(MovementDirection.Down);
                }
                //If the block is at the bottommost position, add the next block to the field.
                SetNextBlock();
            }

            //If Enter is pressed in any state of the screen that isn't Playing, restart the game.
            //This will be sufficient for now, because the only other States are Opening and GameOver.
            if (input.KeyIsPressed(Keys.Enter) && currentState != GameState.Playing)
            {
                this.Initialize();
                currentState = GameState.Playing;
            }
        }

        /// <summary>
        /// Handles the falling of the blocks, at the appropriate speed for the level.
        /// </summary>
        protected void HandleAutoBlockMovement()
        {
            if (nextTick < 0)
            {
                //Reset back to starting value
                //Source: Tetris Specification @ http://www.colinfahey.com/tetris/tetris_en.html
                nextTick = (0.05 * (11 - scoreManager.Level)) * 1000;

                //Every tick the block needs to fall 1 position
                currentLevel.MoveBlock(MovementDirection.Down);
                numFreeFallSteps++;

                //Determine if this is the final move for the current block.
                if (!currentLevel.BlockCanMove())
                {
                    //The block is allowed to move 
                    if (!isResting)
                        SetNextBlock();

                    isResting = !isResting;
                }

            }
        }

        /// <summary>
        /// Adds a new block to the game, if possible. Otherwise will declare player Game Over.
        /// </summary>
        protected void SetNextBlock()
        {
            //A new block can only be added if the previous block is fixed. We do not want
            //multiple active blocks.
            currentLevel.FixActiveBlockPosition();


            //Get a list of all the full lines.
            List<int> fullLines = currentLevel.DetermineFullLines();

            //If there are full lines, clear them.
            if (fullLines.Count > 0)
                currentLevel.ClearLines(fullLines);

            //Update score, and reset scorekeeping variables.
            scoreManager.UpdateScores(fullLines.Count, numFreeFallSteps);
            numFreeFallSteps = 0;

            //Put a new block on the grid. If this is not possible, declare the player Game Over.
            if (!currentLevel.CanAddNewBlock())
            {
                currentState = GameState.GameOver;
            }
            else
            {
                currentLevel.ActivateNextBlock();
            }
        }

        #endregion

    }
}
