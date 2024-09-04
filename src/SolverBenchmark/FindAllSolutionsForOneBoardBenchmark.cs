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

        private BacktrackingSolver BacktrackingSolver;
        private DeBruijnSolver DeBruijnSolver;
        private DlxSolver DlxSolver;

        /// <summary>
        /// Ctor.
        /// </summary>
        public FindAllSolutionsForOneBoardBenchmark()
        {
            this.gameManager = new GameManager(DefaultDices.GetAllDefaultDices());

            BacktrackingSolver = new BacktrackingSolver();
            DeBruijnSolver = new DeBruijnSolver();
            DlxSolver = new DlxSolver(DancingLinksHelper.GenerateDancingLinksRoot());

            RandomDice.SetRandomSeed(12345); // setting seed so execution use same pseudo-number sequences
        }

        [Benchmark]
        public int[,] DefaultBacktracking()
        {
            GameInstance gameInstance = gameManager.TryCreateGame();
            return BacktrackingSolver.FindAllSolutions(gameInstance.Board.Board).SolvedBoard;
        }

        [Benchmark]
        public int[,] DeBruijn()
        {
            GameInstance gameInstance = gameManager.TryCreateGame();
            return DeBruijnSolver.FindAllSolutions(gameInstance.Board.Board).SolvedBoard;
        }

        [Benchmark]
        public int[,] DancingLinksAlgorithmX()
        {
            GameInstance gameInstance = gameManager.TryCreateGame();
            return DlxSolver.FindAllSolutions(gameInstance.Board.Board).SolvedBoard;
        }
    }
}
