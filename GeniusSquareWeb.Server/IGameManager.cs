namespace GeniusSquareWeb.Models
{
    /// <summary>
    /// Interface representing game manager for Genius square.
    /// </summary>
    public interface IGameManager
    {
        /// <summary>
        /// Try create a game.
        /// </summary>
        /// <returns></returns>
        public GameInstance? TryCreateGame();

        /// <summary>
        /// Try get an existing game.
        /// </summary>
        /// <returns></returns>
        public GameInstance? TryGetExistingGame(Guid game);

        /// <summary>
        /// Try delete a game.
        /// </summary>
        /// <returns></returns>
        public bool TryDeleteGame(Guid game);

        /// <summary>
        /// Get all game instances.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IGameInstance> GetAllGameInstances();
    }
}
