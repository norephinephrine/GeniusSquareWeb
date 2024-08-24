using GameSolvers;
using GameSolversTests;
using GeniusSquareWeb.Models;
using GeniusSquareWeb.Server;

namespace DfsSolverTests
{
    /// <summary>
    /// DeBrujin algorithm tests.
    /// </summary>
    [TestClass]
    public class DeBruijnSolverTests
    {

        /// <summary>
        /// Validate De Bruijn solver.
        /// </summary>
        [TestMethod]
        public void ValidateSolver()
        {
            // given
            GameManager gameManager = new GameManager(DefaultDices.GetAllDefaultDices());
            GameInstance gameInstance = gameManager.TryCreateGame();

            GameBoard gameBoard = gameInstance.Board;
            DeBruijnSolver dfsSolver = new DeBruijnSolver();

            // when
            int[,] solvedBoard = dfsSolver.Solve(gameBoard);

            // then
            Utilities.ValidateBlockSolution(gameBoard.Board, solvedBoard);
        }
    }
}