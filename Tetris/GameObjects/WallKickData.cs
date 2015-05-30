using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Tetris.GameObjects.Blocks;

namespace Tetris.GameObjects
{

    public class WallKickData
    {
        #region Fields

        private Point[,][] kickDataJLSTZ;
        private Point[,][] kickDataI;
        private Point[] kickDataO;

        #endregion

        #region Initialization

        public WallKickData()
        {
            //2 Arrays will be created, for the 2 different SRS Wall Kick Data tables. The
            //arrays will hold all kickdata values. Because the O block cannot rotate, no
            //values for different rotation situations need to be added.
            kickDataO = new Point[1];
            kickDataJLSTZ = new Point[4, 4][];
            kickDataI = new Point[4, 4][];

            //Add values to the O array
            kickDataO[0] = new Point(0, 0);
            
            //Add values to the I array
            kickDataI[(int)Orientation.Up, (int)Orientation.Right] = new Point[] {
                new Point(0, 0),
                new Point(-2, 0),
                new Point(1, 0),
                new Point(-2, -1),
                new Point(1, 2)
                };
            kickDataI[(int)Orientation.Right, (int)Orientation.Up] = new Point[] {
                new Point(0, 0),
                new Point(2, 0),
                new Point(-1, 0),
                new Point(2, 1),
                new Point(-1, -2)
                };
            kickDataI[(int)Orientation.Right, (int)Orientation.Down] = new Point[] {
                new Point(0, 0),
                new Point(-1, 0),
                new Point(2, 0),
                new Point(-1, 2),
                new Point(2, -1)
                };
            kickDataI[(int)Orientation.Down, (int)Orientation.Right] = new Point[] {
                new Point(0, 0),
                new Point(1, 0),
                new Point(-2, 0),
                new Point(1, -2),
                new Point(-2, 1)
                };
            kickDataI[(int)Orientation.Down, (int)Orientation.Left] = new Point[] {
                new Point(0, 0),
                new Point(2, 0),
                new Point(-1, 01),
                new Point(2, 1),
                new Point(-1, -2)
                };
            kickDataI[(int)Orientation.Left, (int)Orientation.Down] = new Point[] {
                new Point(0, 0),
                new Point(-2, 0),
                new Point(1, 0),
                new Point(-2, -1),
                new Point(1, 2)
                };
            kickDataI[(int)Orientation.Left, (int)Orientation.Up] = new Point[] {
                new Point(0, 0),
                new Point(1, 0),
                new Point(-2, 0),
                new Point(1, -2),
                new Point(-2, 1)
                };
            kickDataI[(int)Orientation.Up, (int)Orientation.Left] = new Point[] {
                new Point(0, 0),
                new Point(-1, 0),
                new Point(2, 0),
                new Point(-1, 2),
                new Point(2, -1)
                };

            //Add values to the JLSTZ array
            kickDataJLSTZ[(int)Orientation.Up, (int)Orientation.Right] = new Point[] {
                new Point(0, 0),
                new Point(-1, 0),
                new Point(-1, 1),
                new Point(0, -2),
                new Point(-1, -2)
                };
            kickDataJLSTZ[(int)Orientation.Right, (int)Orientation.Up] = new Point[] {
                new Point(0, 0),
                new Point(1, 0),
                new Point(1, -1),
                new Point(0, 2),
                new Point(1, 2)
                };
            kickDataJLSTZ[(int)Orientation.Right, (int)Orientation.Down] = new Point[] {
                new Point(0, 0),
                new Point(1, 0),
                new Point(1, -1),
                new Point(0, 2),
                new Point(1, 2)
                };
            kickDataJLSTZ[(int)Orientation.Down, (int)Orientation.Right] = new Point[] {
                new Point(0, 0),
                new Point(-1, 0),
                new Point(-1, 1),
                new Point(0, -2),
                new Point(-1, -2)
                };
            kickDataJLSTZ[(int)Orientation.Down, (int)Orientation.Left] = new Point[] {
                new Point(0, 0),
                new Point(1, 0),
                new Point(1, 1),
                new Point(0, -2),
                new Point(1, -2)
                };
            kickDataJLSTZ[(int)Orientation.Left, (int)Orientation.Down] = new Point[] {
                new Point(0, 0),
                new Point(-1, 0),
                new Point(-1, -1),
                new Point(0, 2),
                new Point(-1, 2)
                };
            kickDataJLSTZ[(int)Orientation.Left, (int)Orientation.Up] = new Point[] {
                new Point(0, 0),
                new Point(-1, 0),
                new Point(-1, -1),
                new Point(0, 2),
                new Point(-1, 2)
                };
            kickDataJLSTZ[(int)Orientation.Up, (int)Orientation.Left] = new Point[] {
                new Point(0, 0),
                new Point(1, 0),
                new Point(1, 1),
                new Point(0, -2),
                new Point(1, -2)
                };
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Retrieves the wallkick offsets for the given rotation. Uses SRS kick data.
        /// </summary>
        /// <param name="blockType">The type of block that is rotated.</param>
        /// <param name="oldOrientation">The starting orientation of the block.</param>
        /// <param name="newOrientation">The desired rotation of the block.</param>
        /// <remarks>
        /// Uses SRS (Super Rotation System) kick data. Specification was taken from:
        /// http://tetrisconcept.net/wiki/SRS at 05/26/2012.
        /// </remarks>
        public Point[] GetKickData(BlockTypes blockType, Orientation oldOrientation, Orientation newOrientation)
        {
            //Determine what the type of the block is, so we can read from the correct array.
            switch (blockType)
            {
                case BlockTypes.O:
                    return kickDataO;
                case BlockTypes.I:
                    return kickDataI[(int)oldOrientation, (int)newOrientation];
                case BlockTypes.J:
                case BlockTypes.L:
                case BlockTypes.S:
                case BlockTypes.T:
                case BlockTypes.Z:
                    return kickDataJLSTZ[(int)oldOrientation, (int)newOrientation];
                default:
                    return null;
            }

            //Type = J,L,S,T or Z. Old orient = spawn(up). New orient = Right
            //Point[] kickData = new Point[]
            //{
            //    new Point(0,0),
            //    new Point(-1, 0),
            //    new Point(-1, 1),
            //    new Point(0, -2),
            //    new Point(-1, -2)
            //};

        }
        #endregion







    }

}
