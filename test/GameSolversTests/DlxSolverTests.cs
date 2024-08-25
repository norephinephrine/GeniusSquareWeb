using GeniusSquareWeb.GameElements;
using Node = GeniusSquareWeb.GameSolvers.DancingLinks.DlxSolver.Node;
using GeniusSquareWeb.GameSolvers.DancingLinks;
using GeniusSquareWeb.GameSolvers;

namespace GameSolversTests
{
    /// <summary>
    /// Dancing links with Algorithm X solver tests.
    /// </summary>
    [TestClass]
    public class DlxSolverTests
    {
        // We add 1 more column that represents the root node.
        private const int NumberOfColumns = GameConstants.NodeColumnCountDancingLinks +1;

        /// <summary>
        /// Validate Board generation.
        /// </summary>
        [TestMethod]
        public void ValidateBoardGeneration()
        {
            // given
            Node root = GeniusSquareDancingLinks.GenerateBoard();
            int count = 0;

            // when & then
            Node current = root;
            do
            {
                count++;
                current = current.Right;
            }while(current != root);

            Assert.AreEqual(NumberOfColumns, count);
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
            DlxSolver solver = new DlxSolver(root);

            // when
            SolverResult solverResult = solver.Solve(gameBoard.Board);

            // then
            Utilities.ValidateBlockSolution(gameBoard.Board, solverResult.SolvedBoard);
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

            DlxSolver solver = new DlxSolver(root);

            // when
            SolverResult solverResult1 = solver.Solve(gameInstance1.Board.Board);
            SolverResult solverResult2 = solver.Solve(gameInstance2.Board.Board);

            // then
            Utilities.ValidateBlockSolution(gameInstance1.Board.Board, solverResult1.SolvedBoard);
            Utilities.ValidateBlockSolution(gameInstance2.Board.Board, solverResult2.SolvedBoard);
        }
    }
}