using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Tetris.GameObjects.Blocks
{
    static class BlockColors
    {
        private static Dictionary<int, Color> blockColors
            = new Dictionary<int, Color>
            {
                {1, Color.Cyan},
                {2, Color.Blue},
                {3, Color.Orange},
                {4, Color.Yellow},
                {5, Color.Lime},
                {6, Color.Fuchsia},
                {7, Color.Red}
            };
        
        public static Color GetColor(int colorCode)
        {
            return blockColors[colorCode];
        }

        public static int GetColorCode(Color color)
        {
            return blockColors.FirstOrDefault(x => x.Value == color).Key;
        }


    }
}
