namespace GeniusSquareWeb.GameElements.Dices
{
    /// <summary>
    /// Interface representing dice.
    /// </summary>
    public interface IDice
    {
        /// <summary>
        /// Get dice side count.
        /// </summary>
        /// <returns></returns>
        public int GetDiceSideCount();

        /// <summary>
        /// Get all dice sides contained in this object.
        /// </summary>
        /// <returns></returns>
        public GameBoardField[] GetAllDiceSides();

        /// <summary>
        /// Generate a result from a dice throw.
        /// </summary>
        /// <returns>GameBoardField that the dice roll points to.</returns>
        public GameBoardField GenerateDiceResult();
    }
}
