namespace GeniusSquareWeb.GameSolvers
{
    public interface ISolver
    {
        /// <summary>
        /// Solve game.
        /// </summary>
        public int[,] Solve(int[,] board);
    }
}
