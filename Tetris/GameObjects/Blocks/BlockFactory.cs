using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tetris.GameObjects.Blocks
{
    class BlockFactory
    {

        private Random rand = new Random();
        private List<BlockTypes> bag = new List<BlockTypes>();
        
        /// <summary>
        /// Creates a new random block.
        /// </summary>
        /// <returns></returns>
        public Block GetNewBlock()
        {
            Block newBlock;

            if (bag.Count == 0)
            {
                FillBag();
            }

            //Retrieve random block from bag
            int randomValue = rand.Next(0, bag.Count);
            BlockTypes shapeType = bag[randomValue];
            bag.RemoveAt(randomValue);
            
            //Retrieve the block as integer matrix
            switch (shapeType)
            {
                case BlockTypes.I:
                    newBlock = new Shapes.I();
                    break;
                case BlockTypes.O:
                    newBlock = new Shapes.O();
                    break;
                case BlockTypes.J:
                    newBlock = new Shapes.J();
                    break;
                case BlockTypes.L:
                    newBlock = new Shapes.L();
                    break;
                case BlockTypes.S:
                    newBlock = new Shapes.S();
                    break;
                case BlockTypes.T:
                    newBlock = new Shapes.T();
                    break;
                case BlockTypes.Z:
                    newBlock = new Shapes.Z();
                    break;
                default:
                    throw new NotImplementedException();
            }
            
            return newBlock;
        }

        /// <summary>
        /// Generate a full bag. A full bag contains all different blocks.
        /// </summary>
        private void FillBag()
        {
            bag = Enum.GetValues(typeof(BlockTypes))
                .Cast<BlockTypes>()
                .ToList<BlockTypes>();
        }

    }
}
