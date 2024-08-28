using System.Collections.Concurrent;
using GeniusSquareWeb.GameElements.Dices;

namespace GeniusSquareWeb.GameElements
{
    /// <summary>
    /// Class that manages game instances of Genius Square.
    /// </summary>
    public class GameManager: IGameManager
    {
        private IEnumerable<IDice> dices;
        private static readonly ConcurrentDictionary<Guid, GameInstance> gameInstances = new();

        /// <summary>
        /// Ctor.
        /// </summary>
        public GameManager(IEnumerable<IDice> dices)
        {
            this.dices = dices;
        }

        /// <inheritdoc/>
        public GameInstance? TryCreateGame()
        {
            Guid gameId = Guid.NewGuid();
            GameBoard board = new GameBoard();

            foreach (IDice dice in dices)
            {
                GameBoardField field = dice.GenerateDiceResult();
                board.SetGameBoardField(field, -1);
            }

            GameInstance gameInstance = new(gameId, board);

            if (!gameInstances.TryAdd(gameId, gameInstance))
            {
                return null;
            }

            return gameInstance;
        }

        /// <inheritdoc/>
        public GameInstance? TryGetExistingGame(Guid gameId)
        {
            GameInstance gameInstance;
            if (!gameInstances.TryGetValue(gameId, out gameInstance))
            {
                return null;
            }

            return gameInstance;
        }

        /// <inheritdoc/>
        public bool TryDeleteGame(Guid gameId)
        {
            return gameInstances.TryRemove(gameId, out GameInstance gameBoard);
        }

        /// <inheritdoc/>
        public IEnumerable<IGameInstance> GetAllGameInstances()
        {
            return gameInstances.Values;
        }
    }
}
