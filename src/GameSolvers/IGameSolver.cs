namespace GeniusSquareWeb.GameSolvers
{
    /// <summary>
    /// Interface for reppresenting various solvers for the Genius square game.
    /// </summary>
    public interface IGameSolver
    {
        /// <summary>
        /// Find one solution.
        /// </summary>
        /// <param name="board">Board to solve.</param>
        /// <returns>Solutions.</returns>
        public SolverResult FindOneSolution(int[,] board);

        /// <summary>
        /// Find all solutions.
        /// </summary>
        /// <param name="board">Board to solve.</param>
        /// <returns>Solutions.</returns>
        public SolverResult FindAllSolutions(int[,] board);
    }
}
