namespace GeniusSquareWeb.GameElements
{
    public static class GameConstants
    {
        /// <summary>
        /// Genius square board row count.
        /// </summary>
        public const int BoardRowCount = 6;

        /// <summary>
        /// Genius square board column count.
        /// </summary>
        public const int BoardColumnCount = 6;

        /// <summary>
        /// Genius square figure count.
        /// </summary>
        public const int FigureCount = 9;

        /// <summary>
        /// Max player count.
        /// </summary>
        public const int MaxPlayerCount = 2;

        /// <summary>
        /// An offset used to differentiate Nodes representing figure columns
        /// in the Dancing Links implementation.
        /// </summary>
        public const int DancingLinkFigureOffset = 100;
    }
}
