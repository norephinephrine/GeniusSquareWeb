using GameSolvers;
using GameSolversTests;
using GeniusSquareWeb.Models;
using GeniusSquareWeb.Server;

namespace DfsSolverTests
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
            int[,] solvedBoard = solver.Solve(gameBoard);

            // then
            Utilities.ValidateBlockSolution(gameBoard.Board, solvedBoard);
        }
    }
}