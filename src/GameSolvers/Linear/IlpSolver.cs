namespace GeniusSquareWeb.GameSolvers.Linear
{
    /// <summary>
    /// Interface for integer linear programming solvers
    /// </summary>
    public interface IlpSolver
    {
        /// <summary>
        /// Solved the 0-1 integer linear problem for A * x = b.
        /// 
        /// A is NxM matrix
        /// x is M array of variables
        /// b is N  result array
        /// </summary>
        /// <param name="AColumns">List of all columns from A.</param>
        /// <param name="b">Result array.</param>
        /// <param name="N">Row count.</param>
        /// <param name="M">Column count.</param>
        /// <returns>All selected columns from A for the solution.</returns>
        /// <exception cref="Exception"></exception>
        public IEnumerable<LinearColumn> Solve(
            List<LinearColumn> AColumns,
            int[] b,
            int N,
            int M);
    }
}
