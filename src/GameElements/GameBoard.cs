namespace GeniusSquareWeb.GameElements
{ 
    /// <summary>
    /// Class representing a game board for Genius Square.
    /// </summary>
    public record class GameBoard
    {
        private const int RowCount = GameConstants.BoardColumnCount;
        private const int ColumnCount = GameConstants.BoardColumnCount;

        /// <summary>
        /// Actual board.
        /// </summary>
        private int[,] board = new int[RowCount, ColumnCount];

        // Get copy of the current game board.
        public int[,] Board => (int[,])board.Clone();

        /// <summary>
        /// Ctor.
        /// </summary>
        public GameBoard()
        {
            foreach (int row in board)
            {
                foreach(int column in board)
                {
                    board[row, column] = 0;
                }
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
            int row = (int)field / RowCount;
            int column = (int)field % RowCount;

            board[row, column] = newValue;
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

            board[x , y] = newValue;
        }
    }
}
