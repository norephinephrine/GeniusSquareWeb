using GameSolvers;
using GameSolversTests;
using GeniusSquareWeb.Models;
using GeniusSquareWeb.Server;

namespace DfsSolverTests
{
    /// <summary>
    /// RandomDice tests.
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
            // get
            GameManager gameManager = new GameManager(DefaultDices.GetAllDefaultDices());
            GameInstance gameInstance = gameManager.TryCreateGame();

            GameBoard gameBoard = gameInstance.Board;
            BacktrackingSolver dfsSolver = new BacktrackingSolver();

            // then
            int[,] solvedBoard = dfsSolver.Solve(gameBoard);

            Utilities.ValidateBlockSolution(gameBoard.Board, solvedBoard);
        }
    }
}