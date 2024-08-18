using GameSolvers;
using GeniusSquareWeb.Models;
using GeniusSquareWeb.Server;

namespace DfsSolverTests
{
    /// <summary>
    /// RandomDice tests.
    /// </summary>
    [TestClass]
    public class DfsSolverTests
    {
        /// <summary>
        /// Should generate different random dice results.
        /// </summary>
        [TestMethod]
        public void ShouldGenerateDifferentDiceResults()
        {
            // get
            GameManager gameManager = new GameManager(DefaultDices.GetAllDefaultDices());
            GameInstance gameInstance = gameManager.TryCreateGame();

            GameBoard gameBoard = gameInstance.Board;
            DfsSolver dfsSolver = new DfsSolver();

            // then
            int[,] solvedBoard = dfsSolver.Solve(gameBoard);

            Print2DArray(solvedBoard);

        }

        private static void Print2DArray(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write($"{matrix[i,j],3}");
                }
                Console.WriteLine();
            }
        }
    }
}