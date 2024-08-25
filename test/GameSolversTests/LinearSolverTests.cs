using GeniusSquareWeb.GameElements;
using GeniusSquareWeb.GameSolvers.Backtracking;
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
                    new object[] { new ScipIlpSolver() },
                    new object[] { new BopIlpSolver() },
                    new object[] { new SatIlpSolver() },
                };
            }
        }

        /// <summary>
        /// Validate linear solver.
        /// </summary>
        [TestMethod]
        public void ValidateGetAllPossibleColumns()
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
            int[,] solvedBoard = solver.Solve(gameBoard.Board);

            // then
            Utilities.ValidateBlockSolution(gameBoard.Board, solvedBoard);
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
            int[,] solvedBoard1 = solver.Solve(gameInstance1.Board.Board);
            int[,] solvedBoard2 = solver.Solve(gameInstance2.Board.Board);

            // then
            Utilities.ValidateBlockSolution(gameInstance1.Board.Board, solvedBoard1);
            Utilities.ValidateBlockSolution(gameInstance2.Board.Board, solvedBoard2);
        }   
    }
}