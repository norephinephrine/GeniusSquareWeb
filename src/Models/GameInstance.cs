namespace GeniusSquareWeb.Models
{
    /// <summary>
    /// Game instance
    /// </summary>
    public class GameInstance : IGameInstance
    {
        public const int MaxPlayerCount = 2;
        private readonly object mutex = new();

        private GameBoard board;
        private bool isGameActive = true;

        List<string> players = new List<string>();     

        public GameInstance(Guid gameId, GameBoard board)
        {
            this.board = board;
            this.GameId = gameId;
        }

        /// <summary>
        /// Get game id.
        /// </summary>
        public Guid GameId { get; private set; }

        /// <inheritdoc/>
        public int[,] GetInitialBoardState()
        {
            return this.board.Board;
        }

        /// <inheritdoc/>
        public bool IsAvailableToJoin()
        {
            lock (mutex)
            {
                return this.players.Count() < MaxPlayerCount; 
            }
        }

        /// <inheritdoc/>
        public bool TryCompleteGame(string player)
        {
            lock (mutex)
            {
                if (!this.isGameActive)
                {
                    return false;
                }

                this.isGameActive = false;
            }

            return true;
        }

        /// <inheritdoc/>
        public bool TryAddPlayer(string player)
        {
            lock (mutex)
            {
                if (this.DoesContainPlayer(player))
                {
                    return true;
                }

                if (!this.IsAvailableToJoin())
                {
                    return false;
                }

                players.Add(player);
                return true;
            }
        }

        /// <inheritdoc/>
        public bool DoesContainPlayer(string player)
        {
            lock (mutex)
            {
                return players.Contains(player);
            }
        }
    }
}
