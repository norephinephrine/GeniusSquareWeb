using BenchmarkDotNet.Attributes;
using Perfolizer.Mathematics.OutlierDetection;
using GeniusSquareWeb.GameElements;
using GeniusSquareWeb.GameSolvers.Backtracking;
using GeniusSquareWeb.GameSolvers.DeBruijn;
using GeniusSquareWeb.GameSolvers.DancingLinks;

namespace GeniusSquareWeb.SolverBenchmark
{
    [MinColumn, MaxColumn]
    public class FindOneSolutionBenchmark
    {
        private GameManager gameManager;

        private BacktrackingSolver BacktrackingSolver;
        private DeBruijnSolver DeBruijnSolver;
        private DlxSolver DlxSolver;

        public FindOneSolutionBenchmark()
        {
            this.gameManager = new GameManager(DefaultDices.GetAllDefaultDices());

            BacktrackingSolver = new BacktrackingSolver();
            DeBruijnSolver = new DeBruijnSolver();
            DlxSolver = new DlxSolver(GeniusSquareDancingLinks.GenerateBoard());
        }

        [Outliers(OutlierMode.DontRemove)]
        [Benchmark]
        public int[,] DefaultBacktracking()
        {
            GameInstance gameInstance = gameManager.TryCreateGame();
            return BacktrackingSolver.Solve(gameInstance.Board.Board);
        }

        [Outliers(OutlierMode.DontRemove)]
        [Benchmark]
        public int[,] DeBruijn()
        {
            GameInstance gameInstance = gameManager.TryCreateGame();
            return DeBruijnSolver.Solve(gameInstance.Board.Board);
        }

        [Outliers(OutlierMode.DontRemove)]
        [Benchmark]
        public int[,] DancingLinksAlgorithmX()
        {
            GameInstance gameInstance = gameManager.TryCreateGame();
            return DlxSolver.Solve(gameInstance.Board.Board);
        }
    }
}
