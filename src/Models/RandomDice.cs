namespace GeniusSquareWeb.Models
{
    /// <summary>
    /// Class representing a dice for Genius Square.
    /// </summary>
    public class RandomDice : IDice
    {
        private static Random Random = new Random();

        private int diceSideCount;
        private DiceSide[] diceSides;

        /// <summary>
        /// Ctor.
        /// </summary>
        public RandomDice(DiceSide[] sides)
        {
            this.diceSides = sides;
            this.diceSideCount = sides.Count();
        }


        /// <inheritdoc/>
        public int GetDiceSideCount() => diceSides.Count();

        /// <inheritdoc/>
        public DiceSide[] GetAllDiceSides() =>
            (DiceSide[])diceSides.Clone();

        /// <inheritdoc/>
        public DiceSide GenerateDiceSide()
        {
           int randomSide = Random.Next(diceSideCount);

            return diceSides[randomSide];
        }
    }
}
