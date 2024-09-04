using GeniusSquareWeb.GameElements;
using GeniusSquareWeb.GameElements.Dices;
using GeniusSquareWeb.GameSolvers;
using GeniusSquareWeb.GameSolvers.Backtracking;
using GeniusSquareWeb.GameSolvers.DeBruijn;
using GeniusSquareWeb.GameSolvers.Linear;
using GeniusSquareWeb.GameSolvers.Linear.IlpSolvers;

namespace GameSolversTests
{
    /// <summary>
    /// Linear solver tests.
    /// </summary>
    [TestClass]
    public class LinearSolverTests
    {
        private const int RowCount = GameConstants.LinearRowCount;
        private const int ColumnCount = GameConstants.MaxLinearColumCount;

        public static IEnumerable<object[]> IlpSolvers
        {
            get
            {
                return new[]
                {
                    new object[] { new BopIlpSolver() },
                    new object[] { new ScipIlpSolver() },
                    new object[] { new SatIlpSolver() },
                };
            }
        }

        /// <summary>
        /// Validate linear solver.
        /// </summary>
        [TestMethod]
        public void ValidateGetAllPossibleColumnsMethod()
        {
            // given
            LinearColumn[] columns = LinearGeniusSquare.GetAllPossibleColumns();

            // when & then
            Assert.AreEqual(columns.Count(), ColumnCount);

            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    Console.Write($"{columns[j].column[i],3}");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Validate linear solver.
        /// </summary>
        [TestMethod]
        [DynamicData(nameof(IlpSolvers))]
        public void ValidateSolver(IlpSolver ilpSolver)
        {
            // given
            GameManager gameManager = new GameManager(DefaultDices.GetAllDefaultDices());
            GameInstance gameInstance = gameManager.TryCreateGame();

            GameBoard gameBoard = gameInstance.Board;
            LinearSolver solver = new LinearSolver(ilpSolver);

            // when
            SolverResult solvedReulst = solver.FindOneSolution(gameBoard.Board);

            // then
            Utilities.ValidateGameSolution(gameBoard.Board, solvedReulst.SolvedBoard);
        }

        /// <summary>
        /// Validate all game boards using linear solver.
        /// </summary>
        [TestMethod]
        [DynamicData(nameof(IlpSolvers))]
        [Ignore("Test runs too long. Bop ~ 40s.Sat and SIP ~ 13.5min.")]
        public void ValidateAllSolutions(IlpSolver ilpSolver)
        {
            // given
            LinearSolver solver = new LinearSolver(ilpSolver);

            // when and
            Utilities.SolveAndValidateAllGameBoards(solver, true);
        }

        /// <summary>
        /// Solver should be able to be used to solve 2 different problems.
        /// </summary>
        [TestMethod]
        [DynamicData(nameof(IlpSolvers))]
        public void ShouldRunSolverTwiceSuccessfully(IlpSolver ilpSolver)
        {
            // given
            GameManager gameManager = new GameManager(DefaultDices.GetAllDefaultDices());
            GameInstance gameInstance1 = gameManager.TryCreateGame();
            GameInstance gameInstance2 = gameManager.TryCreateGame();

            LinearSolver solver = new LinearSolver(ilpSolver);

            // when
            SolverResult solverResult1 = solver.FindOneSolution(gameInstance1.Board.Board);
            SolverResult solverResult2 = solver.FindOneSolution(gameInstance2.Board.Board);

            // then
            Utilities.ValidateGameSolution(gameInstance1.Board.Board, solverResult1.SolvedBoard);
            Utilities.ValidateGameSolution(gameInstance2.Board.Board, solverResult2.SolvedBoard);
        }
    }
}