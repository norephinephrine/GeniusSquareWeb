using GeniusSquareWeb.Models;

namespace GeniusSquareWeb.Server
{
    /// <summary>
    /// Class that manages game instances of Genius Square.
    /// </summary>
    public class GameManager
    {

        private IEnumerable<IDice> dices;

        /// <summary>
        /// Ctor.
        /// </summary>
        public GameManager(IEnumerable<IDice> dices)
        {
            this.dices = dices;
        }

        public GameBoard CreateInitialBoard()
        {
            GameBoard board = new GameBoard();

            foreach (IDice dice in dices)
            {
                GameBoardField field = dice.GenerateDiceResult();
                board.SetGameBoardField(field, -1);
            }

            return board;
        }
    }
}
