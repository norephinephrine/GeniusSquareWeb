using GeniusSquareWeb.GameElements;
using GeniusSquareWeb.GameSolvers.Backtracking;

namespace GameSolversTests
{
    /// <summary>
    /// Backtracking algorithm tests.
    /// </summary>
    [TestClass]
    public class BacktrackingSolverTests
    {

        /// <summary>
        /// Validate backtracking solver.
        /// </summary>
        [TestMethod]
        public void ValidateSolver()
        {
            // given
            GameManager gameManager = new GameManager(DefaultDices.GetAllDefaultDices());
            GameInstance gameInstance = gameManager.TryCreateGame();

            GameBoard gameBoard = gameInstance.Board;
            BacktrackingSolver solver = new BacktrackingSolver();

            // when
            int[,] solvedBoard = solver.Solve(gameBoard.Board);

            // then
            Utilities.ValidateBlockSolution(gameBoard.Board, solvedBoard);
        }

        /// <summary>
        /// Solver should be able to be used to solve 2 different problems.
        /// </summary>
        [TestMethod]
        public void ShouldRunSolverTwiceSuccessfully()
        {
            // given
            GameManager gameManager = new GameManager(DefaultDices.GetAllDefaultDices());
            GameInstance gameInstance1 = gameManager.TryCreateGame();
            GameInstance gameInstance2 = gameManager.TryCreateGame();

            BacktrackingSolver solver = new BacktrackingSolver();

            // when
            int[,] solvedBoard1 = solver.Solve(gameInstance1.Board.Board);
            int[,] solvedBoard2 = solver.Solve(gameInstance2.Board.Board);

            // then
            Utilities.ValidateBlockSolution(gameInstance1.Board.Board, solvedBoard1);
            Utilities.ValidateBlockSolution(gameInstance2.Board.Board, solvedBoard2);
        }
    }
}