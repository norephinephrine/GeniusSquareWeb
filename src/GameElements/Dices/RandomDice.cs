﻿namespace GeniusSquareWeb.GameElements.Dices
{
    /// <summary>
    /// Class representing a dice for Genius Square.
    /// </summary>
    public class RandomDice : IDice
    {
        private static Random Random = new Random();

        private int diceSideCount;
        private GameBoardField[] diceSides;

        /// <summary>
        /// Set the seed of the Random generator to control
        /// pseudo-random generation.
        /// </summary>
        /// <param name="seed"></param>
        public static void SetRandomSeed(int seed)
        {
            Random = new Random(seed);
        }

        /// <summary>
        /// Ctor.
        /// </summary>
        public RandomDice(GameBoardField[] sides)
        {
            diceSides = sides ?? throw new ArgumentNullException("Dice side array should not be null");
            diceSideCount = sides.Count();
        }


        /// <inheritdoc/>
        public int GetDiceSideCount() => diceSides.Count();

        /// <inheritdoc/>
        public GameBoardField[] GetAllDiceSides() =>
            (GameBoardField[])diceSides.Clone();

        /// <inheritdoc/>
        public GameBoardField GenerateDiceResult()
        {
            int randomSide = Random.Next(diceSideCount);

            return diceSides[randomSide];
        }
    }
}
