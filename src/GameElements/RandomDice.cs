namespace GeniusSquareWeb.GameElements
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
        /// Ctor.
        /// </summary>
        public RandomDice(GameBoardField[] sides)
        {
            this.diceSides = sides ?? throw new ArgumentNullException("Dice side array should not be null");
            this.diceSideCount = sides.Count();
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
