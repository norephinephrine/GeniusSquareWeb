using GeniusSquareWeb.Models;
using GeniusSquareWeb.Server.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace GeniusSquareWeb.Server;

/// <summary>
/// Game hub.
/// </summary>
public class GameHub : Hub
{
    public Task<IEnumerable<Guid>> GetAllGames(IGameManager gameManager)
    {
        string player = this.Context.ConnectionId;

        IEnumerable<Guid> gameIds =
            gameManager.GetAllGameInstances()
            .Where(instance =>
            {
                if (instance.DoesContainPlayer(player))
                    return true;

                return instance.IsAvailableToJoin();
            })
            .Select(instance => instance.GameId)
            .ToList();

        return Task.FromResult(gameIds);
    }

    public async Task<GameRecord> CreateGameAsync(IGameManager gameManager)
    {
        string player = this.Context.ConnectionId;
        GameInstance? gameInstance = gameManager.TryCreateGame();

        if (gameInstance == null || !gameInstance.TryAddPlayer(player))
        {
            return null;
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, gameInstance.GameId.ToString());
        await Clients.All.SendAsync("ReloadGames");

        GameRecord record = new(
            gameGuid: gameInstance.GameId,
            board:
                this.ConvertMultiDimensionalToJaggedArray(
                    gameInstance.GetInitialBoardState()));

        return record;
    }

    public async Task<GameRecord> JoinGameAsync(Guid gameId, IGameManager gameManager)
    {
        string player = this.Context.ConnectionId;

        IGameInstance? gameInstance = gameManager.TryGetExistingGame(gameId);

        if (gameInstance == null || !gameInstance.TryAddPlayer(player))
        {
            return null;
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, gameId.ToString());
        await Clients.OthersInGroup(gameId.ToString()).SendAsync("PlayerJoined");
        await Clients.All.SendAsync("ReloadGames");

        // transform 2d array into a jagged array to allow serialization to JSON.
        int[,] multiDimensionalArray = gameInstance.GetInitialBoardState();

        GameRecord record = new(
            gameGuid: gameInstance.GameId,
            board:
                this.ConvertMultiDimensionalToJaggedArray(
                    gameInstance.GetInitialBoardState()));

        return record;
    }

    public async Task<bool> WinGameAsync(Guid gameId, IGameManager gameManager)
    {
        string player = this.Context.ConnectionId;
        IGameInstance? gameInstance = gameManager.TryGetExistingGame(gameId);

        if (gameInstance == null || !gameInstance.TryCompleteGame(player))
        {
            return false;
        }

        await Clients
            .OthersInGroup(gameInstance.GameId.ToString())
            .SendAsync("LoseGame");

        gameManager.TryDeleteGame(gameId);

        await Clients
            .Group(gameInstance.GameId.ToString())
            .SendAsync("ReloadGames");

        return true;
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        //await Clients.All.SendAsync("TerminateGame");
        await base.OnDisconnectedAsync(exception);
    }

    private int[][] ConvertMultiDimensionalToJaggedArray(int[,] multiDimensionalArray)
    {
        int rowCount = multiDimensionalArray.GetLength(0);
        int columnCount = multiDimensionalArray.GetLength(1);

        int[][] jaggedArray = new int[rowCount][];
        for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
        {
            jaggedArray[rowIndex] = new int[columnCount];
            for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
            {
                jaggedArray[rowIndex][columnIndex] = multiDimensionalArray[rowIndex, columnIndex];
            }
        }

        return jaggedArray;
    }
}