
using Ludo.Game.Controller;
using Ludo.Game.Interfaces;
using Ludo.Game.Logging;

public sealed class GameSessionService
{
    private readonly IGameLogger _logger;
    public GameController? Game { get; private set; }
    public IPlayer? Winner { get; private set; } = null;
    public GameSessionService(IGameLogger logger)
    {
        _logger = logger;
    }
    public void CreateGame(List<IPlayer> players, IBoard board, IDice dice)
    {
        Game = new GameController(_logger, board, dice, players);
        Game.StartGame();
    }
    public void GetWinner()
    {
        if (Game != null)
        {
            Winner = Game.GetWinner();

        }
    }
    public void Reset()
    {
        if (Game != null)
        {
            Game.ResetGame();
        }
    }
    public void GameOver()
    {
        Game = null;
    }
}
