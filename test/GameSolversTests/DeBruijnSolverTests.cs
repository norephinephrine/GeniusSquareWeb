using GeniusSquareWeb.GameElements;
using GeniusSquareWeb.GameSolvers;
using GeniusSquareWeb.GameSolvers.DeBruijn;

namespace GameSolversTests
{
    /// <summary>
    /// De Brujin algorithm tests.
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
            DeBruijnSolver solver = new DeBruijnSolver();

            int[,] board = gameBoard.Board;

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

            DeBruijnSolver solver = new DeBruijnSolver();

            // when
            SolverResult solverResult1 = solver.Solve(gameInstance1.Board.Board);
            SolverResult solverResult2 = solver.Solve(gameInstance2.Board.Board);

            // then
            Utilities.ValidateBlockSolution(gameInstance1.Board.Board, solverResult1.SolvedBoard);
            Utilities.ValidateBlockSolution(gameInstance2.Board.Board, solverResult2.SolvedBoard);
        }
    }
}