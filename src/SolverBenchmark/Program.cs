using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using GeniusSquareWeb.GameElements;
using GeniusSquareWeb.GameSolvers.Backtracking;
using GeniusSquareWeb.GameSolvers.DancingLinks;
using GeniusSquareWeb.GameSolvers.DeBruijn;
using GeniusSquareWeb.GameSolvers;
using GeniusSquareWeb.SolverBenchmark;
using static GeniusSquareWeb.GameSolvers.DancingLinks.DlxSolver;

//object mutex = new object();
//GameManager gameManager = new GameManager(DefaultDices.GetAllDefaultDices());

//int n = 10000;

//int[] deBruijnSolutions = new int[n];
//int[] backtrackingSolutions = new int[n];
//int[] dlxSolutions = new int[n];

//DeBruijnSolver deBruijnSolver = new();
//BacktrackingSolver backtrackingSolver = new();

//Node root = GeniusSquareDancingLinks.GenerateBoard();
//DlxSolver dlxSolver = new DlxSolver(root);

//Parallel.For(0, n, (int i) =>
//{
//    GameInstance gameInstance = gameManager.TryCreateGame();
//    GameBoard gameBoard = gameInstance.Board;

//    deBruijnSolutions[i] = deBruijnSolver.Solve(gameBoard.Board).NumberOfIterations;
//    backtrackingSolutions[i] = backtrackingSolver.Solve(gameBoard.Board).NumberOfIterations;

//    // DLX solver is not thread safe
//    lock (mutex)
//    {
//        dlxSolutions[i] = dlxSolver.Solve(gameBoard.Board).NumberOfIterations;
//    }
//});

//// write Backtracking solver results
//int min = backtrackingSolutions.Min(result => result);
//int max = backtrackingSolutions.Max(result => result);
//double average = backtrackingSolutions.Average(result => result);

//Console.WriteLine($"N iteration{n}");
//Console.WriteLine();
//Console.WriteLine();
//Console.WriteLine("BackTracking:");
//Console.WriteLine($"\tMinimum: {min}");
//Console.WriteLine($"\tMaximum: {max}");
//Console.WriteLine($"\tAverage: {average}");

//// write De Bruijn solver results
//min = deBruijnSolutions.Min(result => result);
//max = deBruijnSolutions.Max(result => result);
//average = deBruijnSolutions.Average(result => result);

//Console.WriteLine("De Bruijn:");
//Console.WriteLine($"\tMinimum: {min}");
//Console.WriteLine($"\tMaximum: {max}");
//Console.WriteLine($"\tAverage: {average}");

//// write DLX solver results
//min = dlxSolutions.Min(result => result);
//max = dlxSolutions.Max(result => result);
//average = dlxSolutions.Average(result => result);

//Console.WriteLine();
//Console.WriteLine("DLX:");
//Console.WriteLine($"\tMinimum: {min}");
//Console.WriteLine($"\tMaximum: {max}");
//Console.WriteLine($"\tAverage: {average}");

var config =
    DefaultConfig.Instance.AddColumn(
        StatisticColumn.P50,
        StatisticColumn.P95);
var summary = BenchmarkRunner.Run<FindOneSolutionBenchmark>(config);
