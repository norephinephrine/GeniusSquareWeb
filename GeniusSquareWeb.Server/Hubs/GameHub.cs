using GeniusSquareWeb.GameElements;
using GeniusSquareWeb.Server.Hubs;
using GeniusSquareWeb.Server.SolversWithDelay;
using Microsoft.AspNetCore.SignalR;

namespace GeniusSquareWeb.Server;

/// <summary>
/// Game hub.
/// </summary>
public class GameHub : Hub
{
    private IHubContext<GameHub> hubContext;
    private IGameManager gameManager;

    private static readonly TimeSpan MinimumSolverTime = TimeSpan.FromSeconds(30);
    private static readonly TimeSpan DelayBetweenIterations = TimeSpan.FromMilliseconds(50);

    public GameHub(IHubContext<GameHub> hubContext)
    {
        this.hubContext = hubContext;
    }

    /// <summary>
    /// Get all active games.
    /// </summary>
    /// <param name="gameManager">Game manager.</param>
    /// <returns></returns>
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

    /// <summary>
    /// Create new game.
    /// </summary>
    /// <param name="gameManager">Game manager.</param>
    /// <returns>Returns a GameRecord containing gameId and initial board</returns>
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
                ConvertMultiDimensionalToJaggedArray(
                    gameInstance.GetInitialBoardState()));

        return record;
    }

    /// <summary>
    /// Create game versus a bot.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="gameManager"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
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
                ConvertMultiDimensionalToJaggedArray(
                    gameInstance.GetInitialBoardState()));

        _ = Task.Run(async () =>
        {
            if (type == 0)
            {
                await SolvesUsingBacktrackingAsync(
                    this.hubContext,
                    gameManager,
                    gameInstance,
                    botPlayer);
            }
            else if (type == 1)
            {
                await SolvesUsingDlxAsync(
                    this.hubContext,
                    gameManager,
                    gameInstance,
                    botPlayer);
            }
            else if (type == 2)
            {
                await SolveUsingDeBruijnAsync(
                    this.hubContext,
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

    /// <summary>
    /// Update player board.
    /// </summary>
    /// <param name="gameId">Game id</param>
    /// <param name="board"></param>
    /// <param name="gameManager"></param>
    /// <returns></returns>
    public async Task UpdateBoardAsync(Guid gameId, int[][] board, IGameManager gameManager)
    {
        string playerId = this.Context.ConnectionId;
        GameInstance? gameInstance = gameManager.TryGetExistingGame(gameId);

        if (gameInstance == null || !gameInstance.IsGameActive)
        {
            return;
        }

        await Clients.OthersInGroup(gameId.ToString())
            .SendAsync("EnemyUpdate", gameId.ToString(), board);
    }

    /// <summary>
    /// Join an existing game.
    /// </summary>
    /// <param name="gameId">Game id.</param>
    /// <param name="gameManager">Game manager.</param>
    /// <returns></returns>
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
                ConvertMultiDimensionalToJaggedArray(
                    gameInstance.GetInitialBoardState()));

        return record;
    }


    /// <summary>
    /// Try to win a game.
    /// </summary>
    /// <param name="gameId">Game Id.</param>
    /// <param name="gameManager">Game manager.</param>
    /// <returns>True if game is won, false if not.</returns>
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

    // START PRIVATE METHODS

    /// <summary>
    /// Solve game using De Bruijn solver with delay.
    /// 
    /// The game will last 30 seconds minimum.
    /// </summary>
    /// <returns></returns>
    /// <param name="hubContext">Hub context.</param>
    /// <param name="gameManager">Game manager</param>
    /// <param name="gameInstance">Game instance</param>
    /// <param name="botPLayer"></param>
    /// <returns></returns>
    private static async Task SolveUsingDeBruijnAsync(
        IHubContext<GameHub> hubContext,
        IGameManager gameManager,
        GameInstance gameInstance,
        string botPLayer)
    {
        DateTime currentTime = DateTime.Now;

        Func<int[,], Task<bool>> callback = (int[,] board) =>
        {
            return UpdateClientsAboutBotBoardChangesAsync(hubContext, gameInstance, board, botPLayer);
        };

        DeBruijnSolverWithDelay deBruijnSolverWithDelay = new(callback);

        if(!await deBruijnSolverWithDelay.Solve(
            gameInstance.Board.Board,
            DelayBetweenIterations))
        {
            Console.WriteLine("Solver could not find solution or game is over");
            return;
        }

        TimeSpan diff = DateTime.Now - currentTime;
        if (diff < MinimumSolverTime)
        {
            Console.WriteLine($"Solver finished early. Sleeping for {MinimumSolverTime - diff}");
            await Task.Delay(MinimumSolverTime - diff);
        }

        await TryWinGameBotAsync(
            hubContext,
            gameManager,
            gameInstance,
            botPLayer);
    }

    /// <summary>
    /// Solve game using Backtracking solver with delay.
    /// 
    /// The game will last 30 seconds minimum.
    /// </summary>
    /// <returns></returns>
    /// <param name="hubContext">Hub context.</param>
    /// <param name="gameManager">Game manager</param>
    /// <param name="gameInstance">Game instance</param>
    /// <param name="botPLayer"></param>
    /// <returns></returns>
    private static async Task SolvesUsingBacktrackingAsync(
        IHubContext<GameHub> hubContext,
        IGameManager gameManager,
        GameInstance gameInstance,
        string botPLayer)
    {
        DateTime currentTime = DateTime.Now;

        Func<int[,], Task<bool>> callback = (int[,] board) =>
        {
            return UpdateClientsAboutBotBoardChangesAsync(hubContext, gameInstance, board, botPLayer);
        };

        BacktrackingSolverWithDelay backtrackingSolver = new(callback);

        if (!await backtrackingSolver.Solve(
            gameInstance.Board.Board,
            DelayBetweenIterations))
        {
            Console.WriteLine("Solver could not find solution or game is over");
            return;
        }

        TimeSpan diff = DateTime.Now - currentTime;
        if (diff < MinimumSolverTime)
        {
            Console.WriteLine($"Solver finished early. Sleeping for {MinimumSolverTime - diff}");
            await Task.Delay(MinimumSolverTime - diff);
        }

        await TryWinGameBotAsync(
            hubContext,
            gameManager,
            gameInstance,
            botPLayer);
    }

    /// <summary>
    /// Solve game using Dlx solver with delay.
    /// 
    /// The game will last 30 seconds minimum.
    /// </summary>
    /// <returns></returns>
    /// <param name="hubContext">Hub context.</param>
    /// <param name="gameManager">Game manager</param>
    /// <param name="gameInstance">Game instance</param>
    /// <param name="botPLayer"></param>
    /// <returns></returns>
    private static async Task SolvesUsingDlxAsync(
        IHubContext<GameHub> hubContext,
        IGameManager gameManager,
        GameInstance gameInstance,
        string botPLayer)
    {
        DateTime currentTime = DateTime.Now;

        Func<int[,], Task<bool>> callback = (int[,] board) =>
        {
            return UpdateClientsAboutBotBoardChangesAsync(hubContext, gameInstance, board, botPLayer);
        };

        DlxSolverWithDelay dlxSolver = new(callback);

        if (!await dlxSolver.Solve(
            gameInstance.Board.Board,
            DelayBetweenIterations))
        {
            Console.WriteLine("Solver could not find solution or game is over");
            return;
        }

        TimeSpan diff = DateTime.Now - currentTime;
        if (diff < MinimumSolverTime)
        {
            Console.WriteLine($"Solver finished early. Sleeping for {MinimumSolverTime - diff}");
            await Task.Delay(MinimumSolverTime - diff);
        }

        await TryWinGameBotAsync(
            hubContext,
            gameManager,
            gameInstance,
            botPLayer);
    }

    /// <summary>
    /// Try to win game as a bot.
    /// </summary>
    /// <param name="hubContext">Hub context.</param>
    /// <param name="gameManager">Game manager.</param>
    /// <param name="gameInstance">Game instance.</param>
    /// <param name="botPlayer">Bot player.</param>
    /// <returns></returns>
    private static async Task TryWinGameBotAsync(
        IHubContext<GameHub> hubContext,
        IGameManager gameManager,
        GameInstance gameInstance,
        string botPlayer)
    {
        if (!gameInstance.TryCompleteGame(botPlayer))
        {
            return;
        }

        Guid gameId = gameInstance.GameId;

        await hubContext
            .Clients
            .GroupExcept(gameId.ToString(), new[]{ botPlayer})
            .SendAsync("LoseGame");

        gameManager.TryDeleteGame(gameId);

        await hubContext
            .Clients
            .GroupExcept(gameId.ToString(), new[] { botPlayer })
            .SendAsync("ReloadGames");
    }

    /// <summary>
    /// Tries to update clients about enemy state.
    /// </summary>
    /// <param name="hubContext">Hub context.</param>
    /// <param name="gameInstance">Game instance.</param>
    /// <param name="board">Board.</param>
    /// <param name="botId">Bot id.</param>
    /// <returns>True if game is still ongoing, false otherwise.</returns>
    private static async Task<bool> UpdateClientsAboutBotBoardChangesAsync(
        IHubContext<GameHub> hubContext,
        IGameInstance gameInstance,
        int[,] board,
        string botId)
    {
        if (!gameInstance.IsGameActive)
        {
            return false;
        }

        Guid gameId = gameInstance.GameId;

        await hubContext
            .Clients
            .GroupExcept(gameId.ToString(), botId)
            .SendAsync("EnemyUpdate", gameId.ToString(), ConvertMultiDimensionalToJaggedArray(board));

        return true;
    }

    private static int[][] ConvertMultiDimensionalToJaggedArray(int[,] multiDimensionalArray)
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