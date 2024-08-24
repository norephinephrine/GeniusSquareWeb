using GeniusSquareWeb.Models;

namespace GameSolvers
{
    public interface ISolver
    {
        /// <summary>
        /// Solve game.
        /// </summary>
        public int[,] Solve(int[,] board);
    }
}
