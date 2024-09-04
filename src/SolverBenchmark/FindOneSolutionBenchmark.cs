﻿using BenchmarkDotNet.Attributes;
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

        private BacktrackingSolver BacktrackingSolver;
        private DeBruijnSolver DeBruijnSolver;
        private DlxSolver DlxSolver;

        private LinearSolver BopLinearSolver;
        private LinearSolver ScipLinearSolver;
        private LinearSolver SatLinearSolver;

        /// <summary>
        /// Ctor
        /// </summary>
        public FindOneSolutionBenchmark()
        {
            this.gameManager = new GameManager(DefaultDices.GetAllDefaultDices());

            BacktrackingSolver = new BacktrackingSolver();
            DeBruijnSolver = new DeBruijnSolver();
            DlxSolver = new DlxSolver(DancingLinksHelper.GenerateDancingLinksRoot());

            BopLinearSolver = new LinearSolver(new BopIlpSolver());
            ScipLinearSolver = new LinearSolver(new ScipIlpSolver());
            SatLinearSolver = new LinearSolver(new SatIlpSolver());

            RandomDice.SetRandomSeed(12345); // setting seed so execution use same pseudo-number sequences
        }

        [Benchmark]
        public int[,] DefaultBacktracking()
        {
            GameInstance gameInstance = gameManager.TryCreateGame();
            return BacktrackingSolver.FindOneSolution(gameInstance.Board.Board).SolvedBoard;
        }

        [Benchmark]
        public int[,] DeBruijn()
        {
            GameInstance gameInstance = gameManager.TryCreateGame();
            return DeBruijnSolver.FindOneSolution(gameInstance.Board.Board).SolvedBoard;
        }

        [Benchmark]
        public int[,] DancingLinksAlgorithmX()
        {
            GameInstance gameInstance = gameManager.TryCreateGame();
            return DlxSolver.FindOneSolution(gameInstance.Board.Board).SolvedBoard;
        }

        [Benchmark]
        public int[,] ILP_SCIP()
        {
            GameInstance gameInstance = gameManager.TryCreateGame();
            return ScipLinearSolver.FindOneSolution(gameInstance.Board.Board).SolvedBoard;
        }

        [Benchmark]
        public int[,] ILP_BOP()
        {
            GameInstance gameInstance = gameManager.TryCreateGame();
            return BopLinearSolver.FindOneSolution(gameInstance.Board.Board).SolvedBoard;
        }

        [Benchmark]
        public int[,] ILP_CP_SAT()
        {
            GameInstance gameInstance = gameManager.TryCreateGame();
            return SatLinearSolver.FindOneSolution(gameInstance.Board.Board).SolvedBoard;
        }
    }
}
