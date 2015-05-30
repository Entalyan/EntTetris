using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tetris.GameObjects
{
    class ScoreManager
    {
        public int totalLinesRemoved;

        public long Score { get; set; }
        public int Level { get; set; }

        public ScoreManager()
        {
            this.Score = 0;
            this.Level = 0;
            this.totalLinesRemoved = 0;
        }

        /// <summary>
        /// Calculates the current level based on the number of numLines completed.
        /// </summary>
        /// <param name="numLines">The amount of numLines completed.</param>
        /// <remarks>Source: Tetris Specification @ http://www.colinfahey.com/tetris/tetris_en.html </remarks>
        private void CalculateLevel()
        {
            if (totalLinesRemoved <= 0)
            {
                this.Level = 1;
            }
            else if ((totalLinesRemoved >= 1) && (totalLinesRemoved <= 90))
            {
                this.Level = 1 + ((totalLinesRemoved - 1) / 10);
            }
            else if (totalLinesRemoved >= 91)
            {
                this.Level = 10;
            }
            else
            {
                //level calculation has failed, throw exception.
                throw new ArgumentOutOfRangeException();
            }

        }

        /// <summary>
        /// Calculates the score the player will receive. The player receives points for both
        /// landing blocks, and clearing numLines.
        /// </summary>
        /// <param name="numLines">Number of numLines cleared.</param>
        /// <param name="freeFallIterations">Number of steps the block has fallen before either player
        /// interaction or landing.</param>
        /// <remarks>
        /// Calculation for landing block score source: Tetris Specification @ http://www.colinfahey.com/tetris/tetris_en.html
        /// Points for clearing numLines are not in the specification, this implementation deviates.
        /// </remarks>
        public void UpdateScores(int numLines, int freeFallIterations)
        {
            int addedScore = 0;

            //Calculate score for landing blocks
            addedScore += ((21 + (3 * this.Level)) - freeFallIterations);

            /* Calculating score for clearing numLines.
             * The maximum number of numLines that can be cleared by a single block is 4. This
             * is valued the most. Scoring is as follows:
             * 1 | 100
             * 2 | 200
             * 3 | 400
             * 4 | 800
             */
            if (numLines > 0)
            {
                //Apparently C# does not use the ^ operator for PowerOf calculations - fix v0.3
                //addedScore += (2 ^ (numLines - 1)) * 100;
                addedScore += (int)Math.Pow(2, numLines - 1) * 100;
            }

            this.totalLinesRemoved += numLines;
            this.Score += addedScore;

            CalculateLevel();
        }
    }

}