using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoGame.Framework;

namespace Tetris
{
    static class Settings
    {
        #region Constants

        //Content related
        private static string gameFontName = "gameFont";
        private static string blankTextureName = "blank";

        //Display settings
        private static int defaultWindowWidth = 598;
        private static int defaultWindowHeight = 740; 
        private static bool isFullScreen = false;
        private static float scalingFactor = 1.0f;

        //Audio settings
        private static bool audioEnabled = true;

        //Game specific settings
        private static int blockSize = 32; //Expressed in pixels
        private static int levelWidth = 10; //Expressed in number of blocks
        private static int levelHeight = 20; //Expressed in number of blocks

        //Texts
        private static string gameTitle = "Tetris v0.31";
        private static string welcomeText = "Welcome to Tetris \nPress Enter to start!";
        private static string gameOverText = "GAME OVER!";
        private static string levelText = "Score: ";
        private static string scoreText = "Level: ";

        //Timing
        private static TimeSpan repeatRate = TimeSpan.FromMilliseconds(100);

        #endregion

        #region Properties

        //Content related
        public static string FontName { get { return gameFontName; } }
        public static string BlankTextureName { get { return blankTextureName; } }

        //Display settings
        public static int DefaultWindowWidth { get { return defaultWindowWidth; } }
        public static int DefaultWindowHeight { get { return defaultWindowHeight; } }
        public static bool IsFullScreen { get { return isFullScreen; } }

        //Audio settings
        public static bool AudioEnabled { get { return audioEnabled; } }
        
        /// <summary>
        /// Factor with which all drawable object must be scaled.
        /// </summary>
        public static float ScalingFactor { get { return scalingFactor; } set { scalingFactor = value; } }

        //Game specific settings
        public static int BlockSize { get { return blockSize; } }
        public static int LevelWidth { get { return levelWidth; } }
        public static int LevelHeight { get { return levelHeight; } }

        //Texts
        public static string GameTitle { get { return gameTitle; } }
        public static string WelcomeText { get { return welcomeText; } }
        public static string GameOverText { get { return gameOverText; } }
        public static string LevelText { get { return levelText; } }
        public static string ScoreText { get { return scoreText; } }

        //Timing
        public static TimeSpan RepeatRate { get { return repeatRate; } }
        #endregion

    }
}
