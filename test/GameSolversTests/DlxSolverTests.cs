using GameSolvers;
using GameSolversTests;
using GeniusSquareWeb.Models;
using GeniusSquareWeb.Server;
using Node = GameSolvers.DlxSolver.Node;

namespace DfsSolverTests
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
            DlxSolver dlxSolver = new DlxSolver(root, gameBoard);

            // when
            int[,] solvedBoard = dlxSolver.Solve();

            // then
            Utilities.ValidateBlockSolution(gameBoard.Board, solvedBoard);
        }
    }
}