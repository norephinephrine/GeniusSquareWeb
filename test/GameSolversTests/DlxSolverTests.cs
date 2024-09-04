using GeniusSquareWeb.GameElements;
using Node = GeniusSquareWeb.GameSolvers.DancingLinks.DlxSolver.Node;
using GeniusSquareWeb.GameSolvers.DancingLinks;
using GeniusSquareWeb.GameSolvers;
using GeniusSquareWeb.GameElements.Dices;
using GeniusSquareWeb.GameSolvers.DeBruijn;

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
            Node root = DancingLinksHelper.GenerateDancingLinksRoot();
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
            Node root = DancingLinksHelper.GenerateDancingLinksRoot();
            DlxSolver solver = new DlxSolver(root);

            // when
            SolverResult solverResult = solver.FindOneSolution(gameBoard.Board);

            // then
            Utilities.ValidateGameSolution(gameBoard.Board, solverResult.SolvedBoard);
        }

        /// <summary>
        /// Validate all solutions for Dlx solver.
        /// </summary>
        [TestMethod]
        [Ignore("Test runs too long. ~17 mins.")]
        public void ValidateAllSolution()
        {
            // given
            Node root = DancingLinksHelper.GenerateDancingLinksRoot();
            DlxSolver solver = new DlxSolver(root);

            // when and then
            Utilities.SolveAndValidateAllGameBoards(solver, false);
        }

        /// <summary>
        /// Validate finding all solutions to one example board for Dlx solver.
        /// </summary>
        [TestMethod]
        [Ignore("Test runs too long.")]
        public void ValidateAllSolutionForOneBoard()
        {
            // given
            GameManager gameManager = new GameManager(DefaultDices.GetAllDefaultDices());
            GameInstance gameInstance1 = gameManager.TryCreateGame();

            Node root = DancingLinksHelper.GenerateDancingLinksRoot();
            DlxSolver solver = new DlxSolver(root);

            // when and then
            SolverResult solverResult = solver.FindAllSolutions(gameInstance1.Board.Board);

            Console.WriteLine($"Number of iterations to find all solutions:{solverResult.IterationCount}");
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

            Node root = DancingLinksHelper.GenerateDancingLinksRoot();
            DlxSolver solver = new DlxSolver(root);

            // when
            SolverResult solverResult1 = solver.FindOneSolution(gameInstance1.Board.Board);
            SolverResult solverResult2 = solver.FindOneSolution(gameInstance2.Board.Board);

            // then
            Utilities.ValidateGameSolution(gameInstance1.Board.Board, solverResult1.SolvedBoard);
            Utilities.ValidateGameSolution(gameInstance2.Board.Board, solverResult2.SolvedBoard);
        }
    }
}