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
    public class DeBruijnSolverTests
    {

        /// <summary>
        /// Validate De Bruijn solver.
        /// </summary>
        [TestMethod]
        public void ValidateSolver()
        {
            // get
            GameManager gameManager = new GameManager(DefaultDices.GetAllDefaultDices());
            GameInstance gameInstance = gameManager.TryCreateGame();

            GameBoard gameBoard = gameInstance.Board;
            DeBruijnSolver dfsSolver = new DeBruijnSolver();

            // then
            int[,] solvedBoard = dfsSolver.Solve(gameBoard);

            Utilities.ValidateBlockSolution(gameBoard.Board, solvedBoard);
        }
    }
}