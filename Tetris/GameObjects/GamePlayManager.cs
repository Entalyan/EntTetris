using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Tetris.GameObjects;
using Tetris.GameObjects.Blocks;

namespace Tetris.GameObjects
{
    public enum MovementDirection
    {
        Left = 0,
        Right,
        Down,
        Up,
        None,
    }

    public enum RotationDirection
    {
        ClockWise = 0,
        CounterClockWise,
    }

    class GamePlayManager
    {
        #region Fields

        private int[,] levelGrid;
        private BlockFactory blockFactory;
        private WallKickData wallKickData;

        private Block activeBlock;
        private Point activeBlockPosition;
        private Point spawnPoint = new Point(3, 0);
        private Queue<Block> nextBlocks;

        #endregion

        #region Properties

        /// <summary>
        /// Returns the next block to be added to the grid.
        /// </summary>
        public Block NextBlock { get { return nextBlocks.Peek(); } }

        #endregion

        #region Initialization

        /// <summary>
        /// Create a new level.
        /// </summary>
        /// <param name="width">Width of the level in number of blocks.</param>
        /// <param name="height">Height of the level in number of blocks.</param>
        public GamePlayManager(int width, int height)
        {
            //Set up the starting matrix and set it to all zeroes.
            levelGrid = new int[width, height];
            Array.Clear(levelGrid, 0, levelGrid.Length);

            //Generate the first block, and set up rotation subsystem.
            blockFactory = new BlockFactory();
            wallKickData = new WallKickData();

            activeBlock = blockFactory.GetNewBlock();
            activeBlockPosition = spawnPoint;

            //Generate the next 3 blocks.
            nextBlocks = new Queue<Block>();
            for (int i = 0; i < 3; i++)
            {
                nextBlocks.Enqueue(blockFactory.GetNewBlock());
            }
        }
        
        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the current state of the level grid. Combines the currently active block with the level grid.
        /// </summary>
        public int[,] LevelGrid()
        {

            int[,] outputGrid = (int[,])levelGrid.Clone();

            for (int x = 0; x < activeBlock.Width; x++)
            {
                for (int y = 0; y < activeBlock.Height; y++)
                {
                    if (activeBlock.Shape[x, y] > 0)
                        outputGrid[x + activeBlockPosition.X + activeBlock.Offset.X, 
                            y + activeBlockPosition.Y + activeBlock.Offset.Y] 
                            = activeBlock.Shape[x, y];
                }
            }

            return outputGrid;
        }

        /// <summary>
        /// Clears full numLines out of the level. 
        /// </summary>
        /// <returns>The number of numLines that have been cleared.</returns>
        public List<int> DetermineFullLines()
        {
            List<int> linesToClear = new List<int>();
            int height = levelGrid.GetUpperBound(1);
            int width = levelGrid.GetUpperBound(0);

            //Loop through all numLines, check if they are full.
            for (int y = height; y >= 0; y--)
            {
                int blocksOnLine = 0;

                for (int x = 0; x <= width; x++)
                {
                    if (levelGrid[x, y] > 0)
                        blocksOnLine++;
                }

                //If all the blocks are filled, add the line to the clearing list.
                if (blocksOnLine == width + 1)
                {
                    linesToClear.Add(y);
                }
            }

            return linesToClear;
        }

        /// <summary>
        /// Clears all the lines specified in the input list.
        /// </summary>
        /// <param name="linesToClear">List of line numbers to clear.</param>
        public void ClearLines(List<int> linesToClear)
        {
            int height = levelGrid.GetUpperBound(1);
            int width = levelGrid.GetUpperBound(0);

            //Move all blocks above the numLines to be cleared 1 position down.
            linesToClear.Sort();
            foreach (int line in linesToClear)
            {
                for (int y = line; y > 0; y--)
                {
                    for (int x = 0; x <= width; x++)
                    {
                        levelGrid[x, y] = levelGrid[x, y - 1];
                    }
                }
            }
        }
        
        /// <summary>
        /// Puts a new block in the level.
        /// </summary>
        /// <returns>Returns a value indicating if switching to the new block was succesful.</returns>
        /// <remarks>TODO: Separate the check for the possibility of adding a block from the actual adding.</remarks>
        public void ActivateNextBlock()
        {
            activeBlockPosition = spawnPoint;

            //Get next block from the queue.
            activeBlock = nextBlocks.Dequeue();
            nextBlocks.Enqueue(blockFactory.GetNewBlock());
        }

        /// <summary>
        /// Fixes the active block to the level grid.
        /// </summary>
        public void FixActiveBlockPosition()
        {
            //Get the current state of the level grid combined with the active 
            //block. Save it to the permanent level grid.
            levelGrid = LevelGrid();
        }

        #endregion

        #region State Information Functions

        /// <summary>
        /// Checks if a block can be placed at the specified position.
        /// </summary>
        /// <param name="block">The block that needs to be checked.</param>
        /// <param name="position">Relative position on the LevelGrid.</param>
        /// <returns></returns>
        private bool BlockPositionIsValid(Block block, Point position)
        {
            //Check if block would be out of bounds of the level.
            if (position.X + block.Offset.X < 0 ||
                position.X + block.Offset.X + block.Width > levelGrid.GetUpperBound(0) + 1 ||
                position.Y + block.Offset.Y < 0 ||
                position.Y + block.Offset.Y + block.Height > levelGrid.GetUpperBound(1) + 1
                )
                return false;

            //Check if block would overlap any existing blocks in the level.
            for (int x = 0; x < block.Width; x++)
            {
                for (int y = 0; y < block.Height; y++)
                {
                    if (levelGrid[x + position.X + block.Offset.X, y + position.Y + block.Offset.Y] > 0)
                    {
                        if (block.Shape[x, y] > 0)
                            return false;
                    }
                }
            }

            //The block is valid if it has passed all previous validations.
            return true;
        }

        /// <summary>
        /// Checks if the block can move 1 position down.
        /// </summary>
        public bool BlockCanMove()
        {
            return BlockPositionIsValid(activeBlock, new Point(activeBlockPosition.X, activeBlockPosition.Y + 1));
        }

        /// <summary>
        /// Determines if a new block can be added to the grid.
        /// </summary>
        public bool CanAddNewBlock()
        {
            return BlockPositionIsValid(nextBlocks.Peek(), spawnPoint);
        }

        #endregion

        #region Block Movement Methods

        /// <summary>
        /// Moves the block in the desired direction.
        /// </summary>
        /// <param name="direction">Direction the block should be moved in.</param>
        /// <returns>Returns true if block movement was succesful, false if not.</returns>
        public void MoveBlock(MovementDirection direction)
        {
            Point newPosition;

            switch (direction)
            {
                case MovementDirection.Left:
                    newPosition = new Point(activeBlockPosition.X - 1, activeBlockPosition.Y);
                    break;
                case MovementDirection.Right:
                    newPosition = new Point(activeBlockPosition.X + 1, activeBlockPosition.Y);
                    break;
                case MovementDirection.Down:
                    newPosition = new Point(activeBlockPosition.X, activeBlockPosition.Y + 1);
                    break;
                case MovementDirection.Up:
                    newPosition = new Point(activeBlockPosition.X, activeBlockPosition.Y - 1);
                    break;
                case MovementDirection.None:
                    newPosition = activeBlockPosition;
                    break;
                default:
                    throw new NotImplementedException();
            }

            if (BlockPositionIsValid(activeBlock, newPosition))
                activeBlockPosition = newPosition;
        }

        /// <summary>
        /// Rotates the active block.
        /// </summary>
        /// <param name="rotDirection">Direction the block should be rotated in.</param>
        public void RotateActiveBlock(RotationDirection rotDirection)
        {
            Orientation targetOrientation = activeBlock.GetNewOrientation(rotDirection);
            Point kickValue = new Point(0,0);
            Boolean isValid = false;


            //Retrieve the kickdata for the desired rotation.
            Point[] kickData = wallKickData.GetKickData(
                activeBlock.BlockType,
                activeBlock.Orientation,
                targetOrientation
                );

            foreach (Point p in kickData)
            {
                //The position of the rotated block, considering wall kick data.
                Point newPosition = new Point(
                    activeBlockPosition.X + p.X,
                    activeBlockPosition.Y + p.Y
                    );

                //A copy of the block so we can apply modifications, without changing the original object.
                Block rotatedBlock = (Block)activeBlock.Clone();
                rotatedBlock.Orientation = targetOrientation;

                //Check if the rotated block is valid. If it is, store offset values and exit loop.
                if (BlockPositionIsValid(
                    rotatedBlock,
                    newPosition
                    ))
                {
                    //If a valid position is found, use this kickdata value for offsetting the block.
                    isValid = true;
                    kickValue = p;
                    break;
                }

            }

            //Check if rotation is valid before applying.
            if (isValid)
            {
                //Apply the orientation changes.
                activeBlock.Orientation = targetOrientation;

                //Apply the offset to the block position.
                activeBlockPosition.X += kickValue.X;
                activeBlockPosition.Y += kickValue.Y;
            }

        }

        #endregion

    }
}
