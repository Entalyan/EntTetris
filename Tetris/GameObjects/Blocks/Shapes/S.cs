﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tetris;
using Microsoft.Xna.Framework;

namespace Tetris.GameObjects.Blocks.Shapes
{
    class S : Block
    {

        protected override void InitializeBlock()
        {
            //List of shapes
            shapes = new List<int[,]>
            {
                new int[,] 
                {
                    { 0, 1 },
                    { 1, 1 },
                    { 1, 0 }
                },
                new int[,] 
                {
                    { 1, 1, 0 },
                    { 0, 1, 1 }
                },
                new int[,] 
                {
                    { 0, 1 },
                    { 1, 1 },
                    { 1, 0 }
                },
                new int[,] 
                {
                    { 1, 1, 0 },
                    { 0, 1, 1 }
                }
            };

            //List of offsets
            offSet = new List<Point>
            {
                new Point(0,0),
                new Point(1,0),
                new Point(0,1),
                new Point(0,0)
            };

            SetShapeColor(Color.Lime);
        }

    }
}
