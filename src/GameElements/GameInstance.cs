﻿namespace GeniusSquareWeb.GameElements
{
    /// <summary>
    /// Game instance
    /// </summary>
    public class GameInstance : IGameInstance
    {
        private readonly object mutex = new();

        private const int MaxPlayerCount = GameConstants.MaxPlayerCount;
        private GameBoard board;
        private List<string> players = new List<string>();

        public GameInstance(Guid gameId, GameBoard board)
        {
            this.board = board;
            this.GameId = gameId;
            this.IsGameActive = true;
        }

        /// <summary>
        /// Gets game board.
        /// </summary>
        public GameBoard Board => this.board;

        /// <summary>
        /// Get game id.
        /// </summary>
        public Guid GameId { get; private set; }

        /// <summary>
        /// Get game id.
        /// </summary>
        public bool IsGameActive { get; private set; }

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
                if (!this.IsGameActive)
                {
                    return false;
                }

                this.IsGameActive = false;
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
