namespace GeniusSquareWeb.GameSolvers
{
    /// <summary>
    /// Interface for reppresenting various solvers for the Genius square game.
    /// </summary>
    public interface IGameSolver
    {
        /// <summary>
        /// Solve game.
        /// </summary>
        public SolverResult Solve(int[,] board);
    }
}
