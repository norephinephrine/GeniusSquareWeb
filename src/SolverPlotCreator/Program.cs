using GeniusSquareWeb.GameSolvers.Backtracking;
using GeniusSquareWeb.GameSolvers.DancingLinks;
using GeniusSquareWeb.GameSolvers.DeBruijn;
using GeniusSquareWeb.GameSolvers;
using static GeniusSquareWeb.GameSolvers.DancingLinks.DlxSolver;
using SolverPlotter;

object mutex = new object();

DeBruijnSolver deBruijnSolver = new();
BacktrackingSolver backtrackingSolver = new();

Node root = DancingLinksHelper.GenerateDancingLinksRoot();
DlxSolver dlxSolver = new DlxSolver(root);

List<Tuple<Func<int[,], SolverResult>, string>> solverToPlotList = new()
{
    // find one solution to board
    new( 
        board => deBruijnSolver.FindOneSolution(board),
        "DeBruijn solver find ONE solution to board"),
    new(
        board  =>
        {
            // DLX is not thread safe.
            lock (mutex)
            {
                return dlxSolver.FindOneSolution(board);
            }
        },
       "DLX find ONE solution to board"),
    new(
        (int[,] board ) => backtrackingSolver.FindOneSolution(board),
        "Backtracking solver find ONE solution to board"),

     // find all solutions to board
    new(
        board => deBruijnSolver.FindAllSolutions(board),
        "DeBruijn solver find ALL solutions to board"),

    // DLX and Backtracking need a long time to stop...
    new(
        board  =>
        {
            // DLX is not thread safe.
            lock (mutex)
            {
                return dlxSolver.FindAllSolutions(board);
            }
        },
       "DLX solver find ALL solutions to board"),

    new(
        (int[,] board ) => backtrackingSolver.FindAllSolutions(board),
        "Backtracking solver find ALL solutions to board")
};

SolverPlotterHelper plotter = new SolverPlotterHelper();

foreach ((Func<int[,], SolverResult> solverCallback, string solverName) in solverToPlotList)
{
    Console.WriteLine($"Started solving: {solverName}");
    plotter.PlotResultsFromSolvingBoard(solverCallback, solverName);
    Console.WriteLine($"Solved: {solverName}\n");
}