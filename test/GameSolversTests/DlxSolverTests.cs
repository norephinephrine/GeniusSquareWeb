using GameSolvers;
using GameSolversTests;
using GeniusSquareWeb.Models;
using GeniusSquareWeb.Server;
using Node = GameSolvers.DlxSolver.Node;

namespace GameSolversTests
{
    /// <summary>
    /// Dancing links with Algorithm X solver tests.
    /// </summary>
    [TestClass]
    public class DlxSolverTests
    {

        /// <summary>
        /// Validate Board generation.
        /// </summary>
        [TestMethod]
        public void ValidateBoardGeneration()
        {
            // when & then
            Node root = GeniusSquareDancingLinks.GenerateBoard();
        }

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
            Node root = GeniusSquareDancingLinks.GenerateBoard();
            DlxSolver dlxSolver = new DlxSolver(root);

            // when
            int[,] solvedBoard = dlxSolver.Solve(gameBoard.Board);

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

            Node root = GeniusSquareDancingLinks.GenerateBoard();

            DlxSolver dlxSolver = new DlxSolver(root);

            // when & then
            _ = dlxSolver.Solve(gameInstance1.Board.Board);
            _ = dlxSolver.Solve(gameInstance2.Board.Board);
        }
    }
}