using GeniusSquareWeb.GameElements;
using GeniusSquareWeb.GameElements.Dices;
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
        /// Validate finding allsolutions for all boards for De Bruijn solver.
        /// </summary>
        [TestMethod]
        public void ValidateFindingOneSolutionsForAllBoards()
        {
            // given
            DeBruijnSolver solver = new DeBruijnSolver();

            // when and then
            Utilities.SolveAndValidateAllGameBoards(solver, true);
        }

        /// <summary>
        /// Validate finding all solutions to one example board for De Bruijn solver.
        /// </summary>
        [TestMethod]
        public void ValidateAllSolutionForOneBoard()
        {
            // given
            GameManager gameManager = new GameManager(DefaultDices.GetAllDefaultDices());
            GameInstance gameInstance1 = gameManager.TryCreateGame();

            DeBruijnSolver solver = new DeBruijnSolver();

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

            DeBruijnSolver solver = new DeBruijnSolver();

            // when
            SolverResult solverResult1 = solver.FindOneSolution(gameInstance1.Board.Board);
            SolverResult solverResult2 = solver.FindOneSolution(gameInstance2.Board.Board);

            // then
            Utilities.ValidateGameSolution(gameInstance1.Board.Board, solverResult1.SolvedBoard);
            Utilities.ValidateGameSolution(gameInstance2.Board.Board, solverResult2.SolvedBoard);
        }
    }
}