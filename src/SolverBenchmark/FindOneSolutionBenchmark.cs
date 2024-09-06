using BenchmarkDotNet.Attributes;
using Perfolizer.Mathematics.OutlierDetection;
using GeniusSquareWeb.GameElements;
using GeniusSquareWeb.GameSolvers.Backtracking;
using GeniusSquareWeb.GameSolvers.DeBruijn;
using GeniusSquareWeb.GameSolvers.DancingLinks;
using GeniusSquareWeb.GameSolvers.Linear;
using GeniusSquareWeb.GameSolvers.Linear.IlpSolvers;
using GeniusSquareWeb.GameElements.Dices;

namespace GeniusSquareWeb.SolverBenchmark
{
    /// <summary>
    /// Find one solution benchmark.
    /// </summary>
    [MemoryDiagnoser]
    [MinColumn, MaxColumn]
    [Outliers(OutlierMode.DontRemove)]
    public class FindOneSolutionBenchmark
    {
        private GameManager gameManager;

        /// <summary>
        /// Ctor
        /// </summary>
        public FindOneSolutionBenchmark()
        {
            this.gameManager = new GameManager(DefaultDices.GetAllDefaultDices());
            RandomDice.SetRandomSeed(12345); // setting seed so execution use same pseudo-number sequences
        }

        [Benchmark]
        public int[,] DefaultBacktracking()
        {
            BacktrackingSolver backtrackingSolver = new BacktrackingSolver();

            GameInstance gameInstance = gameManager.TryCreateGame();
            return backtrackingSolver.FindOneSolution(gameInstance.Board.Board).SolvedBoard;
        }

        [Benchmark]
        public int[,] DeBruijn()
        {
            DeBruijnSolver deBruijnSolver = new DeBruijnSolver();

            GameInstance gameInstance = gameManager.TryCreateGame();
            return deBruijnSolver.FindOneSolution(gameInstance.Board.Board).SolvedBoard;
        }

        [Benchmark]
        public int[,] DancingLinksAlgorithmX()
        {
            DlxSolver dlxSolver = new DlxSolver(DancingLinksHelper.GenerateDancingLinksRoot());


            GameInstance gameInstance = gameManager.TryCreateGame();
            return dlxSolver.FindOneSolution(gameInstance.Board.Board).SolvedBoard;
        }

        [Benchmark]
        public int[,] ILP_SCIP()
        {
            LinearSolver scipLinearSolver = new LinearSolver(new ScipIlpSolver());

            GameInstance gameInstance = gameManager.TryCreateGame();
            return scipLinearSolver.FindOneSolution(gameInstance.Board.Board).SolvedBoard;
        }

        [Benchmark]
        public int[,] ILP_BOP()
        {
            LinearSolver bopLinearSolver = new LinearSolver(new BopIlpSolver());

            GameInstance gameInstance = gameManager.TryCreateGame();
            return bopLinearSolver.FindOneSolution(gameInstance.Board.Board).SolvedBoard;
        }

        [Benchmark]
        public int[,] ILP_CP_SAT()
        {
            LinearSolver satLinearSolver = new LinearSolver(new SatIlpSolver());

            GameInstance gameInstance = gameManager.TryCreateGame();
            return satLinearSolver.FindOneSolution(gameInstance.Board.Board).SolvedBoard;
        }
    }
}
