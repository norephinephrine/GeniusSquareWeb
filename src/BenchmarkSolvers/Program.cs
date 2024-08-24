// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using GameSolvers;
using GeniusSquareWeb.Models;
using GeniusSquareWeb.Server;

var config = DefaultConfig.Instance.AddColumn(
    StatisticColumn.P50,
    StatisticColumn.P95);
var summary = BenchmarkRunner.Run<FindOneSolutionBenchmark>(config);

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

    [Benchmark]
    public int[,] DefaultBacktracking()
    {
        GameInstance gameInstance = gameManager.TryCreateGame();
        return BacktrackingSolver.Solve(gameInstance.Board.Board);
    }

    [Benchmark]
    public int[,] DeBuijn()
    {
        GameInstance gameInstance = gameManager.TryCreateGame();
        return DeBruijnSolver.Solve(gameInstance.Board.Board);
    }

    [Benchmark]
    public int[,] DancingLinksAlgorithmX()
    {
        GameInstance gameInstance = gameManager.TryCreateGame();
        return DlxSolver.Solve(gameInstance.Board.Board);
    }
}