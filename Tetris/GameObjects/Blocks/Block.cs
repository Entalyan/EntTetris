using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Tetris.GameObjects.Blocks.Shapes;

namespace Tetris.GameObjects.Blocks
{
    public enum Orientation
    {
        Up = 0,
        Right,
        Down,
        Left,
    }

    /// <summary>
    /// Enumerate the different block shapes
    /// </summary>
    public enum BlockTypes
    {
        O = 1,
        I,
        L,
        J,
        S,
        T,
        Z
    }

    /// <summary>
    /// Represents a game block (tetromino).
    /// </summary>
    abstract class Block : ICloneable
    {
        protected List<int[,]> shapes;
        protected Orientation orientation = 0;
        protected List<Point> offSet;

        /// <summary>
        /// X and Y offsets for the Block. These offsets make sure the block rotates
        /// around a defined point.
        /// </summary>
        public Point Offset
        {
            get
            {
                return offSet[(int)orientation];
            }
        }

        /// <summary>
        /// Gets or sets the color of the block.
        /// </summary>
        public Color BlockColor { get; internal set; }

        /// <summary>
        /// Gets or sets the orientation of the block.
        /// </summary>
        public Orientation Orientation
        {
            get
            {
                return orientation;
            }

            set
            {
                orientation = value;
            }
        }

        /// <summary>
        /// Gets the width of the block in its current orientation.
        /// </summary>
        public int Width
        {
            get
            {
                //int width = 0;

                //for (int x = 0; x < shapes[(int)orientation].GetUpperBound(0) + 1; x++)
                //{
                //    for (int y = 0; y < shapes[(int)orientation].GetUpperBound(1) + 1; y++)
                //    {
                //        if (shapes[(int)orientation][x, y] > 0 && x + 1 > width)
                //        {
                //            width = x + 1;
                //        }
                //    }
                //}

                //return width;
                return shapes[(int)orientation].GetUpperBound(0) + 1;
            }
        }

        /// <summary>
        /// Gets the height of the block in its current orientation.
        /// </summary>
        public int Height
        {
            get
            {
                //int height = 0;

                //for (int x = 0; x < shapes[(int)orientation].GetUpperBound(0) + 1; x++)
                //{
                //    for (int y = 0; y < shapes[(int)orientation].GetUpperBound(1) + 1; y++)
                //    {
                //        if (shapes[(int)orientation][x, y] > 0 && y + 1 > height)
                //        {
                //            height = y + 1;
                //        }
                //    }
                //}

                //return height;
                return shapes[(int)orientation].GetUpperBound(1) + 1;
            }
        }

        /// <summary>
        /// Gets the shape of the block, with consideration for its current orientation.
        /// </summary>
        public int[,] Shape
        {
            get
            {
                return shapes[(int)orientation];
            }
        }

        /// <summary>
        /// Gets or sets the type of block.
        /// </summary>
        public BlockTypes BlockType
        {
            get;
            internal set;
        }

        public Block()
        {
            //Determine the shape of the block, based on the type of the class.
            System.Type blockType = this.GetType();

            if (blockType == typeof(I))
            {
                this.BlockType = BlockTypes.I;
            }
            else if (blockType == typeof(J))
            {
                this.BlockType = BlockTypes.J;
            }
            else if (blockType == typeof(L))
            {
                this.BlockType = BlockTypes.L;
            }
            else if (blockType == typeof(O))
            {
                this.BlockType = BlockTypes.O;
            }
            else if (blockType == typeof(S))
            {
                this.BlockType = BlockTypes.S;
            }
            else if (blockType == typeof(T))
            {
                this.BlockType = BlockTypes.T;
            }
            else if (blockType == typeof(Z))
            {
                this.BlockType = BlockTypes.Z;
            }
            else throw new Exception("Could not determinte type of block. The type of block that was loaded does not appear to be implemented correctly.");

            //Run non-generic initialization code.
            InitializeBlock();
        }

        protected abstract void InitializeBlock();

        /// <summary>
        /// Gets the orientation of a block after a rotation.
        /// </summary>
        /// <param name="rotDirection">Direction the block should be rotated in.</param>
        public Orientation GetNewOrientation(RotationDirection rotDirection)
        {
            Orientation newOrientation = new Orientation();

            switch (rotDirection)
            {
                case RotationDirection.ClockWise:
                    if ((int)orientation == shapes.Count - 1)
                    {
                        newOrientation = 0;
                    }
                    else
                    {
                        newOrientation = orientation + 1;
                    }
                    break;
                case RotationDirection.CounterClockWise:
                    if (orientation == 0)
                    {
                        newOrientation = (Orientation)shapes.Count - 1;
                    }
                    else
                    {
                        newOrientation = orientation - 1;
                    }
                    break;
            }

            return newOrientation;
        }

        public Object Clone()
        {
            return this.MemberwiseClone();
        }

        protected void SetShapeColor(Color blockColor)
        {
            BlockColor = blockColor;

            for (int currentShape = 0; currentShape < shapes.Count; currentShape++)
            {
                int height = shapes[currentShape].GetUpperBound(1);
                int width = shapes[currentShape].GetUpperBound(0);

                for (int y = 0; y < shapes[currentShape].GetLength(1); y++)
                {
                    for (int x = 0; x < shapes[currentShape].GetLength(0); x++)
                    {
                        if (shapes[currentShape][x, y] > 0)
                        {
                            shapes[currentShape][x, y] = BlockColors.GetColorCode(BlockColor);
                        }
                    }
                }
            }
        }
    }
}
