using BenchmarkDotNet.Attributes;
using Perfolizer.Mathematics.OutlierDetection;
using GeniusSquareWeb.GameElements;
using GeniusSquareWeb.GameSolvers.Backtracking;
using GeniusSquareWeb.GameSolvers.DeBruijn;
using GeniusSquareWeb.GameSolvers.DancingLinks;
using GeniusSquareWeb.GameElements.Dices;

namespace GeniusSquareWeb.SolverBenchmark
{
    /// <summary>
    /// Find all solution  for one board benchmark.
    /// </summary>
    [MemoryDiagnoser]
    [MinColumn, MaxColumn]
    [Outliers(OutlierMode.DontRemove)]
    public class FindAllSolutionsForOneBoardBenchmark
    {
        private GameManager gameManager;

        /// <summary>
        /// Ctor.
        /// </summary>
        public FindAllSolutionsForOneBoardBenchmark()
        {
            this.gameManager = new GameManager(DefaultDices.GetAllDefaultDices());
            RandomDice.SetRandomSeed(12345); // setting seed so execution use same pseudo-number sequences
        }

        [Benchmark]
        public int[,] DefaultBacktracking()
        {
            BacktrackingSolver backtrackingSolver = new BacktrackingSolver();

            GameInstance gameInstance = gameManager.TryCreateGame();
            return backtrackingSolver.FindAllSolutions(gameInstance.Board.Board).SolvedBoard;
        }

        [Benchmark]
        public int[,] DeBruijn()
        {
            DeBruijnSolver deBruijnSolver = new DeBruijnSolver();

            GameInstance gameInstance = gameManager.TryCreateGame();
            return deBruijnSolver.FindAllSolutions(gameInstance.Board.Board).SolvedBoard;
        }

        [Benchmark]
        public int[,] DancingLinksAlgorithmX()
        {
            DlxSolver dlxSolver = new DlxSolver(DancingLinksHelper.GenerateDancingLinksRoot());


            GameInstance gameInstance = gameManager.TryCreateGame();
            return dlxSolver.FindAllSolutions(gameInstance.Board.Board).SolvedBoard;
        }
    }
}
