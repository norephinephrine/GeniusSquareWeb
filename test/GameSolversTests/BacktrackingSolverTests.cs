using GeniusSquareWeb.GameElements;
using GeniusSquareWeb.GameElements.Dices;
using GeniusSquareWeb.GameSolvers;
using GeniusSquareWeb.GameSolvers.Backtracking;
using GeniusSquareWeb.GameSolvers.DeBruijn;

namespace GameSolversTests
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
            SolverResult solverResult = solver.FindOneSolution(gameBoard.Board);

            // then
            Utilities.ValidateGameSolution(gameBoard.Board, solverResult.SolvedBoard);
        }

        /// <summary>
        /// Validate all solutions for backtracking solver.
        /// </summary>
        [TestMethod]
        [Ignore("Test runs too long. ~3hours.")]
        public void ValidateAllSolution()
        {
            // given
            BacktrackingSolver solver = new BacktrackingSolver();

            // when and then
            Utilities.SolveAndValidateAllGameBoards(solver, true);
        }

        /// <summary>
        /// Validate finding all solutions to one example board for Backtracking solver.
        /// </summary>
        [TestMethod]
        [Ignore("Test runs too long.")]
        public void ValidateAllSolutionForOneBoard()
        {
            // given
            GameManager gameManager = new GameManager(DefaultDices.GetAllDefaultDices());
            GameInstance gameInstance1 = gameManager.TryCreateGame();

            BacktrackingSolver solver = new BacktrackingSolver();

            // when and then
            SolverResult solverResult = solver.FindAllSolutions(gameInstance1.Board.Board);

            Console.WriteLine($"Number of iterations to find all solutions:{solverResult}");
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

            BacktrackingSolver solver = new BacktrackingSolver();

            // when
            SolverResult solverResult1 = solver.FindOneSolution(gameInstance1.Board.Board);
            SolverResult solverResult2 = solver.FindOneSolution(gameInstance2.Board.Board);

            // then
            Utilities.ValidateGameSolution(gameInstance1.Board.Board, solverResult1.SolvedBoard);
            Utilities.ValidateGameSolution(gameInstance2.Board.Board, solverResult2.SolvedBoard);
        }
    }
}