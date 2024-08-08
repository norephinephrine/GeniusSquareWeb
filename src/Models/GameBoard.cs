namespace GeniusSquareWeb.Models
{ 
    /// <summary>
    /// Class representing a game board for Genius Square.
    /// </summary>
    public record class GameBoard
    {
        private const int RowCount = 6;
        private const int ColumnCount = 6;

        /// <summary>
        /// Actual board.
        /// </summary>
        private int[] board = new int[RowCount * ColumnCount];

        // Get copy of the current game board.
        public int[] Board => (int[])board.Clone();

        /// <summary>
        /// Ctor.
        /// </summary>
        public GameBoard()
        {
            foreach (int i in board)
            {
                board[i] = 0;
            }
        }

        /// <summary>
        /// Set board element value.
        /// </summary>
        /// <param name="GameBoardField">Enum value.</param>
        /// <param name="newValue">New element value.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Invalid index value.</exception>
        public void SetGameBoardField(GameBoardField field, int newValue)
        {
            board[(int)field] = newValue;
        }

        /// <summary>
        /// Set board element value.
        /// </summary>
        /// <param name="x">Row index.</param>
        /// <param name="y">Column index.</param>
        /// <param name="newValue">New element value.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Invalid index value.</exception>
        public void SetGameBoardField(int x, int y, int newValue)
        {
            if (x < 0 || x >= RowCount)
            {
                throw new ArgumentException($"Invalid value for row index {x}");
            }

            if (y < 0 || y >= ColumnCount)
            {
                throw new ArgumentException($"Invalid value for row index {y}");
            }

            board[x * RowCount + y] = newValue;
        }
    }
}
