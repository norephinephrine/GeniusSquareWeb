using GeniusSquareWeb.GameElements;
using GeniusSquareWeb.GameSolvers.Backtracking;
using GeniusSquareWeb.GameSolvers.DeBruijn;
using GeniusSquareWeb.Server.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace GeniusSquareWeb.Server;

/// <summary>
/// Game hub.
/// </summary>
public class GameHub : Hub
{
    private IHubContext<GameHub> hubContext;
    private IGameManager gameManager;

    private readonly BacktrackingSolver BacktrackingSolver = new();
    private readonly DeBruijnSolver DeBruijnSolver =  new();

    private readonly TimeSpan MinimumSolverTime = TimeSpan.FromSeconds(30);
    private readonly TimeSpan DelayDeBruijnDelayBetweenIterations = TimeSpan.FromMilliseconds(50);
    private readonly TimeSpan DelayBacktrackingBetweenIterations = TimeSpan.FromMilliseconds(1);

    public GameHub(IHubContext<GameHub> hubContext)
    {
        this.hubContext = hubContext;
    }

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

    public async Task<GameRecord> CreateGameBotAsync(int type, IGameManager gameManager)
    {
        string player = this.Context.ConnectionId;
        GameInstance? gameInstance = gameManager.TryCreateGame();

        if (gameInstance == null || !gameInstance.TryAddPlayer(player))
        {
            return null;
        }

        // adding player representing bot so nobody else can see this game.
        string botPlayer = Guid.NewGuid().ToString();
        gameInstance.TryAddPlayer(botPlayer);

        await Groups.AddToGroupAsync(
            Context.ConnectionId,
            gameInstance.GameId.ToString());

        await Clients.All.SendAsync("ReloadGames");

        GameRecord record = new(
            gameGuid: gameInstance.GameId,
            board:
                this.ConvertMultiDimensionalToJaggedArray(
                    gameInstance.GetInitialBoardState()));

        _ = Task.Run(async () =>
        {
            if (type == 0)
            {
                await this.SolvesUsingBacktrackingAsync(
                    gameManager,
                    gameInstance,
                    botPlayer);
            }
            else if (type == 1)
            {
                await this.SolveUsingDeBruijnAsync(
                    gameManager,
                    gameInstance,
                    botPlayer);
            }
            else
            {
                throw new Exception("Type of game is non-existant");
            }
        });

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

    /// <summary>
    /// Solve game using De Bruijn solver.
    /// 
    /// The game will last 30 seconds minimum.
    /// </summary>
    /// <returns></returns>
    private async Task SolveUsingDeBruijnAsync(
        IGameManager gameManager,
        GameInstance gameInstance,
        string botPLayer)
    {
        DateTime currentTime = DateTime.Now;
        TimeSpan diff = DateTime.Now - currentTime;

        await DeBruijnSolver.SolveWithDelayAsync(
            gameInstance.Board.Board,
            DelayDeBruijnDelayBetweenIterations);

        if (diff < MinimumSolverTime)
        {
            await Task.Delay(MinimumSolverTime - diff);
        }

        await WinGameBotAsync(
            gameManager,
            gameInstance,
            botPLayer);
    }

    /// <summary>
    /// Solve game using De Backtracking solver.
    /// 
    /// The game will last 30 seconds minimum.
    private async Task SolvesUsingBacktrackingAsync(
        IGameManager gameManager,
        GameInstance gameInstance,
        string botPLayer)
    {
        DateTime currentTime = DateTime.Now;
        TimeSpan diff = DateTime.Now - currentTime;

        await BacktrackingSolver.SolveWithDelayAsync(
            gameInstance.Board.Board,
            DelayBacktrackingBetweenIterations);

        if (diff < MinimumSolverTime)
        {
            await Task.Delay(MinimumSolverTime - diff);
        }

        await WinGameBotAsync(
            gameManager,
            gameInstance,
            botPLayer);
    }

    private async Task WinGameBotAsync(
        IGameManager gameManager,
        GameInstance gameInstance,
        string botPlayer)
    {
        if (!gameInstance.TryCompleteGame(botPlayer))
        {
            return;
        }

        Guid gameId = gameInstance.GameId;

        await this.hubContext
            .Clients
            .GroupExcept(gameId.ToString(), new[]{ botPlayer})
            .SendAsync("LoseGame");

        gameManager.TryDeleteGame(gameId);

        await this.hubContext
            .Clients
            .GroupExcept(gameId.ToString(), new[] { botPlayer })
            .SendAsync("ReloadGames");
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