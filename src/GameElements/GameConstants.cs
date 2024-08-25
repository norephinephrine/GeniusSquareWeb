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

        /// <summary>
        /// The sum of all possible placements for our 9 figures
        /// when using Dancing links can be is 625.
        /// 
        /// Additionally, we have 1 more row for list headers.
        /// </summary>
        public const int NodeRowCountDancingLinks = 626;

        /// <summary>
        /// 9 figures + 36 cell from board
        /// </summary>
        public const int NodeColumnCountDancingLinks = 45;

        /// <summary>
        /// The sum of all possible placements for our 9 figures
        /// in Genius square is 625 + 1 for root node.
        /// </summary>
        public const int MaxLinearColumCount = 625;

        /// <summary>
        /// 9 figures + 36 cell from board
        /// </summary>
        public const int LinearRowCount = 45;
    }
}
