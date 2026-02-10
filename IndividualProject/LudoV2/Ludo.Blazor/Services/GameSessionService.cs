
using Ludo.Game.Controller;
using Ludo.Game.Interfaces;

public sealed class GameSessionService
{
    private readonly GameControllerFactory _factory;
    public GameController? Game { get; private set; }
    public IPlayer? Winner { get; private set; } = null;
    public GameSessionService(GameControllerFactory factory)
    {
        _factory = factory;
    }
    public void CreateGame(List<IPlayer> players, IBoard board, IDice dice)
    {
        Game = _factory.Create(board, dice, players);
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
