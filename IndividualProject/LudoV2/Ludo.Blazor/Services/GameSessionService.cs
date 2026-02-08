
using Ludo.Game.Controller;
using Ludo.Game.Interfaces;

public sealed class GameSessionService
{
    public GameController? Game { get; private set; }
    public IPlayer? Winner { get; private set; } = null;
    public void CreateGame(List<IPlayer> players, IBoard board, IDice dice)
    {
        Game = new GameController(board, dice, players);
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
