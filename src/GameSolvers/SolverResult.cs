namespace GeniusSquareWeb.GameSolvers
{
    /// <summary>
    /// Struct representing the result from a Solver.
    /// </summary>
    public struct SolverResult
    {
        public int[,] SolvedBoard;
        public int IterationCount = 0;
        public int SolutionsFoundCount = 0;

        /// <summary>
        /// Ctor.
        /// </summary>
        public SolverResult() { }
    }
}
