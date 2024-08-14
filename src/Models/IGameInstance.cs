namespace GeniusSquareWeb.Models
{
    /// <summary>
    /// Interface for game instance.
    /// </summary>
    public interface IGameInstance
    {
        /// <summary>
        /// Get game id.
        /// </summary>
        public Guid GameId { get; }

        /// <summary>
        /// Gets value indicating whether current game is available to join.;
        /// </summary>
        /// <returns></returns>
        bool IsAvailableToJoin();

        /// <summary>
        /// A player tries to completes a game;
        /// </summary>
        /// <returns></returns>
        bool TryCompleteGame(string player);

        /// <summary>
        /// Try to add a player to the current game;
        /// </summary>
        /// <returns></returns>
        bool TryAddPlayer(string player);

        /// <summary>
        /// Check if game contains player.
        /// </summary>
        /// <returns></returns>
        bool DoesContainPlayer(string player);


        /// <summary>
        /// Get initial board state.
        /// </summary>
        /// <returns></returns>
        int[,] GetInitialBoardState();
    }
}
