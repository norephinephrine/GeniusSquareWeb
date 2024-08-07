namespace GeniusSquareWeb.Models
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
        public DiceSide[] GetAllDiceSides();

        /// <summary>
        /// Generate dice throw result.
        /// </summary>
        /// <returns>DiceSide.</returns>
        public DiceSide GenerateDiceSide();
    }
}
