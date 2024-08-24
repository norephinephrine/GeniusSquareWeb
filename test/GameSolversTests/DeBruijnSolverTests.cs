using GameSolvers;
using GameSolversTests;
using GeniusSquareWeb.Models;
using GeniusSquareWeb.Server;

namespace GameSolversTests
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
            DeBruijnSolver deBruijnSolver = new DeBruijnSolver();

            int[,] board = gameBoard.Board;
            // when
            int[,] solvedBoard = deBruijnSolver.Solve(board);

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

            DeBruijnSolver solver = new DeBruijnSolver();

            // when & then
            _ = solver.Solve(gameInstance1.Board.Board);
            _ = solver.Solve(gameInstance2.Board.Board);
        }
    }
}