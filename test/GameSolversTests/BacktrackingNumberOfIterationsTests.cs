using GeniusSquareWeb.GameElements;
using GeniusSquareWeb.GameElements.Dices;
using GeniusSquareWeb.GameSolvers;
using GeniusSquareWeb.GameSolvers.Backtracking;
using GeniusSquareWeb.GameSolvers.DancingLinks;
using GeniusSquareWeb.GameSolvers.DeBruijn;
using System;
using System.Reflection;
using Node = GeniusSquareWeb.GameSolvers.DancingLinks.DlxSolver.Node;


namespace GameSolversTests
{
    /// <summary>
    /// Backtracking number of iteration tests.
    /// </summary>
    [TestClass]
    [Ignore("Only useful to look at backtracking solver iteration count. Doesn't validate anything.")]
    public class BacktrackingNumberOfIterationsTests
    {
        /// <summary>
        /// Count iterations.
        /// </summary>
        [TestMethod]
        public void CountBacktrackingSolverIterations()
        {
            // given
            object mutex = new object();
            GameManager gameManager = new GameManager(DefaultDices.GetAllDefaultDices());

            int n = 100;

            SolverResult[] deBruijnSolutions = new SolverResult[n];
            SolverResult[] backtrackingSolutions = new SolverResult[n];
            SolverResult[] dlxSolutions = new SolverResult[n];

            DeBruijnSolver deBruijnSolver = new();
            BacktrackingSolver backtrackingSolver = new();

            Node root = GeniusSquareDancingLinks.GenerateBoard();
            DlxSolver dlxSolver = new DlxSolver(root);

            Parallel.For(0, n, (int i) =>
            {
                GameInstance gameInstance = gameManager.TryCreateGame();
                GameBoard gameBoard = gameInstance.Board;

                deBruijnSolutions[i] = deBruijnSolver.Solve(gameBoard.Board);
                backtrackingSolutions[i] = backtrackingSolver.Solve(gameBoard.Board);

                // DLX solver is not thread safe
                lock(mutex)
                {
                    dlxSolutions[i] = dlxSolver.Solve(gameBoard.Board);
                }
            });

            // write Backtracking solver results
            int min = backtrackingSolutions.Min(result => result.NumberOfIterations);
            int max = backtrackingSolutions.Max(result => result.NumberOfIterations);
            double average = backtrackingSolutions.Average(result => result.NumberOfIterations);

            Console.WriteLine();
            Console.WriteLine("BackTracking:");
            Console.WriteLine($"\tMinimum: {min}");
            Console.WriteLine($"\tMaximum: {max}");
            Console.WriteLine($"\tAverage: {average}");

            // write De Bruijn solver results
            min = deBruijnSolutions.Min(result => result.NumberOfIterations);
            max = deBruijnSolutions.Max(result => result.NumberOfIterations);
            average = deBruijnSolutions.Average(result => result.NumberOfIterations);

            Console.WriteLine("De Bruijn:");
            Console.WriteLine($"\tMinimum: {min}");
            Console.WriteLine($"\tMaximum: {max}");
            Console.WriteLine($"\tAverage: {average}");

            // write DLX solver results
            min = dlxSolutions.Min(result => result.NumberOfIterations);
            max = dlxSolutions.Max(result => result.NumberOfIterations);
            average = dlxSolutions.Average(result => result.NumberOfIterations);

            Console.WriteLine();
            Console.WriteLine("DLX:");
            Console.WriteLine($"\tMinimum: {min}");
            Console.WriteLine($"\tMaximum: {max}");
            Console.WriteLine($"\tAverage: {average}");
        }
    }
}
